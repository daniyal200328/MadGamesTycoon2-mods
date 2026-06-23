using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;

namespace BrainFailProductions.PolyFew;

[Serializable]
[DebuggerDisplay("Count = {Count}")]
public class SerializableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable
{
	private static class PrimeHelper
	{
		public static readonly int[] Primes = new int[72]
		{
			3, 7, 11, 17, 23, 29, 37, 47, 59, 71,
			89, 107, 131, 163, 197, 239, 293, 353, 431, 521,
			631, 761, 919, 1103, 1327, 1597, 1931, 2333, 2801, 3371,
			4049, 4861, 5839, 7013, 8419, 10103, 12143, 14591, 17519, 21023,
			25229, 30293, 36353, 43627, 52361, 62851, 75431, 90523, 108631, 130363,
			156437, 187751, 225307, 270371, 324449, 389357, 467237, 560689, 672827, 807403,
			968897, 1162687, 1395263, 1674319, 2009191, 2411033, 2893249, 3471899, 4166287, 4999559,
			5999471, 7199369
		};

		public static bool IsPrime(int candidate)
		{
			if ((candidate & 1) != 0)
			{
				int num = (int)Math.Sqrt(candidate);
				for (int i = 3; i <= num; i += 2)
				{
					if (candidate % i == 0)
					{
						return false;
					}
				}
				return true;
			}
			return candidate == 2;
		}

		public static int GetPrime(int min)
		{
			if (min < 0)
			{
				throw new ArgumentException("min < 0");
			}
			for (int i = 0; i < Primes.Length; i++)
			{
				int num = Primes[i];
				if (num >= min)
				{
					return num;
				}
			}
			for (int j = min | 1; j < int.MaxValue; j += 2)
			{
				if (IsPrime(j) && (j - 1) % 101 != 0)
				{
					return j;
				}
			}
			return min;
		}

		public static int ExpandPrime(int oldSize)
		{
			int num = 2 * oldSize;
			if (num > 2146435069 && 2146435069 > oldSize)
			{
				return 2146435069;
			}
			return GetPrime(num);
		}
	}

	public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
	{
		private readonly SerializableDictionary<TKey, TValue> _Dictionary;

		private int _Version;

		private int _Index;

		private KeyValuePair<TKey, TValue> _Current;

		public KeyValuePair<TKey, TValue> Current => _Current;

		object IEnumerator.Current => Current;

		internal Enumerator(SerializableDictionary<TKey, TValue> dictionary)
		{
			_Dictionary = dictionary;
			_Version = dictionary._Version;
			_Current = default(KeyValuePair<TKey, TValue>);
			_Index = 0;
		}

		public bool MoveNext()
		{
			if (_Version != _Dictionary._Version)
			{
				throw new InvalidOperationException($"Enumerator version {_Version} != Dictionary version {_Dictionary._Version}");
			}
			while (_Index < _Dictionary._Count)
			{
				if (_Dictionary._HashCodes[_Index] >= 0)
				{
					_Current = new KeyValuePair<TKey, TValue>(_Dictionary._Keys[_Index], _Dictionary._Values[_Index]);
					_Index++;
					return true;
				}
				_Index++;
			}
			_Index = _Dictionary._Count + 1;
			_Current = default(KeyValuePair<TKey, TValue>);
			return false;
		}

		void IEnumerator.Reset()
		{
			if (_Version != _Dictionary._Version)
			{
				throw new InvalidOperationException($"Enumerator version {_Version} != Dictionary version {_Dictionary._Version}");
			}
			_Index = 0;
			_Current = default(KeyValuePair<TKey, TValue>);
		}

		public void Dispose()
		{
		}
	}

	[SerializeField]
	[HideInInspector]
	private int[] _Buckets;

	[SerializeField]
	[HideInInspector]
	private int[] _HashCodes;

	[SerializeField]
	[HideInInspector]
	private int[] _Next;

	[SerializeField]
	[HideInInspector]
	private int _Count;

	[SerializeField]
	[HideInInspector]
	private int _Version;

	[SerializeField]
	[HideInInspector]
	private int _FreeList;

	[SerializeField]
	[HideInInspector]
	private int _FreeCount;

	[SerializeField]
	[HideInInspector]
	private TKey[] _Keys;

	[SerializeField]
	[HideInInspector]
	private TValue[] _Values;

	private readonly IEqualityComparer<TKey> _Comparer;

	public Dictionary<TKey, TValue> AsDictionary => new Dictionary<TKey, TValue>(this);

	public int Count => _Count - _FreeCount;

	public TValue this[TKey key, TValue defaultValue]
	{
		get
		{
			int num = FindIndex(key);
			if (num >= 0)
			{
				return _Values[num];
			}
			return defaultValue;
		}
	}

	public TValue this[TKey key]
	{
		get
		{
			int num = FindIndex(key);
			if (num >= 0)
			{
				return _Values[num];
			}
			throw new KeyNotFoundException(key.ToString());
		}
		set
		{
			Insert(key, value, add: false);
		}
	}

	public ICollection<TKey> Keys => _Keys.Take(Count).ToArray();

	public ICollection<TValue> Values => _Values.Take(Count).ToArray();

	public bool IsReadOnly => false;

	public SerializableDictionary()
		: this(0, (IEqualityComparer<TKey>)null)
	{
	}

	public SerializableDictionary(int capacity)
		: this(capacity, (IEqualityComparer<TKey>)null)
	{
	}

	public SerializableDictionary(IEqualityComparer<TKey> comparer)
		: this(0, comparer)
	{
	}

	public SerializableDictionary(int capacity, IEqualityComparer<TKey> comparer)
	{
		if (capacity < 0)
		{
			throw new ArgumentOutOfRangeException("capacity");
		}
		Initialize(capacity);
		_Comparer = comparer ?? EqualityComparer<TKey>.Default;
	}

	public SerializableDictionary(IDictionary<TKey, TValue> dictionary)
		: this(dictionary, (IEqualityComparer<TKey>)null)
	{
	}

	public SerializableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		: this(dictionary?.Count ?? 0, comparer)
	{
		if (dictionary == null)
		{
			throw new ArgumentNullException("dictionary");
		}
		foreach (KeyValuePair<TKey, TValue> item in dictionary)
		{
			Add(item.Key, item.Value);
		}
	}

	public bool ContainsValue(TValue value)
	{
		if (value == null)
		{
			for (int i = 0; i < _Count; i++)
			{
				if (_HashCodes[i] >= 0 && _Values[i] == null)
				{
					return true;
				}
			}
		}
		else
		{
			EqualityComparer<TValue> equalityComparer = EqualityComparer<TValue>.Default;
			for (int j = 0; j < _Count; j++)
			{
				if (_HashCodes[j] >= 0 && equalityComparer.Equals(_Values[j], value))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool ContainsKey(TKey key)
	{
		return FindIndex(key) >= 0;
	}

	public void Clear()
	{
		if (_Count > 0)
		{
			for (int i = 0; i < _Buckets.Length; i++)
			{
				_Buckets[i] = -1;
			}
			Array.Clear(_Keys, 0, _Count);
			Array.Clear(_Values, 0, _Count);
			Array.Clear(_HashCodes, 0, _Count);
			Array.Clear(_Next, 0, _Count);
			_FreeList = -1;
			_Count = 0;
			_FreeCount = 0;
			_Version++;
		}
	}

	public void Add(TKey key, TValue value)
	{
		Insert(key, value, add: true);
	}

	private void Resize(int newSize, bool forceNewHashCodes)
	{
		int[] array = new int[newSize];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = -1;
		}
		TKey[] array2 = new TKey[newSize];
		TValue[] array3 = new TValue[newSize];
		int[] array4 = new int[newSize];
		int[] array5 = new int[newSize];
		Array.Copy(_Values, 0, array3, 0, _Count);
		Array.Copy(_Keys, 0, array2, 0, _Count);
		Array.Copy(_HashCodes, 0, array4, 0, _Count);
		Array.Copy(_Next, 0, array5, 0, _Count);
		if (forceNewHashCodes)
		{
			for (int j = 0; j < _Count; j++)
			{
				if (array4[j] != -1)
				{
					array4[j] = _Comparer.GetHashCode(array2[j]) & 0x7FFFFFFF;
				}
			}
		}
		for (int k = 0; k < _Count; k++)
		{
			int num = array4[k] % newSize;
			array5[k] = array[num];
			array[num] = k;
		}
		_Buckets = array;
		_Keys = array2;
		_Values = array3;
		_HashCodes = array4;
		_Next = array5;
	}

	private void Resize()
	{
		Resize(PrimeHelper.ExpandPrime(_Count), forceNewHashCodes: false);
	}

	public bool Remove(TKey key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		int num = _Comparer.GetHashCode(key) & 0x7FFFFFFF;
		int num2 = num % _Buckets.Length;
		int num3 = -1;
		for (int num4 = _Buckets[num2]; num4 >= 0; num4 = _Next[num4])
		{
			if (_HashCodes[num4] == num && _Comparer.Equals(_Keys[num4], key))
			{
				if (num3 < 0)
				{
					_Buckets[num2] = _Next[num4];
				}
				else
				{
					_Next[num3] = _Next[num4];
				}
				_HashCodes[num4] = -1;
				_Next[num4] = _FreeList;
				_Keys[num4] = default(TKey);
				_Values[num4] = default(TValue);
				_FreeList = num4;
				_FreeCount++;
				_Version++;
				return true;
			}
			num3 = num4;
		}
		return false;
	}

	private void Insert(TKey key, TValue value, bool add)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (_Buckets == null)
		{
			Initialize(0);
		}
		int num = _Comparer.GetHashCode(key) & 0x7FFFFFFF;
		int num2 = num % _Buckets.Length;
		int num3 = 0;
		for (int num4 = _Buckets[num2]; num4 >= 0; num4 = _Next[num4])
		{
			if (_HashCodes[num4] == num && _Comparer.Equals(_Keys[num4], key))
			{
				if (add)
				{
					TKey val = key;
					throw new ArgumentException("Key already exists: " + val);
				}
				_Values[num4] = value;
				_Version++;
				return;
			}
			num3++;
		}
		int num5;
		if (_FreeCount > 0)
		{
			num5 = _FreeList;
			_FreeList = _Next[num5];
			_FreeCount--;
		}
		else
		{
			if (_Count == _Keys.Length)
			{
				Resize();
				num2 = num % _Buckets.Length;
			}
			num5 = _Count;
			_Count++;
		}
		_HashCodes[num5] = num;
		_Next[num5] = _Buckets[num2];
		_Keys[num5] = key;
		_Values[num5] = value;
		_Buckets[num2] = num5;
		_Version++;
	}

	private void Initialize(int capacity)
	{
		int prime = PrimeHelper.GetPrime(capacity);
		_Buckets = new int[prime];
		for (int i = 0; i < _Buckets.Length; i++)
		{
			_Buckets[i] = -1;
		}
		_Keys = new TKey[prime];
		_Values = new TValue[prime];
		_HashCodes = new int[prime];
		_Next = new int[prime];
		_FreeList = -1;
	}

	private int FindIndex(TKey key)
	{
		if (key == null)
		{
			throw new ArgumentNullException("key");
		}
		if (_Buckets != null)
		{
			int num = _Comparer.GetHashCode(key) & 0x7FFFFFFF;
			for (int num2 = _Buckets[num % _Buckets.Length]; num2 >= 0; num2 = _Next[num2])
			{
				if (_HashCodes[num2] == num && _Comparer.Equals(_Keys[num2], key))
				{
					return num2;
				}
			}
		}
		return -1;
	}

	public bool TryGetValue(TKey key, out TValue value)
	{
		int num = FindIndex(key);
		if (num >= 0)
		{
			value = _Values[num];
			return true;
		}
		value = default(TValue);
		return false;
	}

	public void Add(KeyValuePair<TKey, TValue> item)
	{
		Add(item.Key, item.Value);
	}

	public bool Contains(KeyValuePair<TKey, TValue> item)
	{
		int num = FindIndex(item.Key);
		if (num >= 0)
		{
			return EqualityComparer<TValue>.Default.Equals(_Values[num], item.Value);
		}
		return false;
	}

	public void CopyTo(KeyValuePair<TKey, TValue>[] array, int index)
	{
		if (array == null)
		{
			throw new ArgumentNullException("array");
		}
		if (index < 0 || index > array.Length)
		{
			throw new ArgumentOutOfRangeException($"index = {index} array.Length = {array.Length}");
		}
		if (array.Length - index < Count)
		{
			throw new ArgumentException($"The number of elements in the dictionary ({Count}) is greater than the available space from index to the end of the destination array {array.Length}.");
		}
		for (int i = 0; i < _Count; i++)
		{
			if (_HashCodes[i] >= 0)
			{
				array[index++] = new KeyValuePair<TKey, TValue>(_Keys[i], _Values[i]);
			}
		}
	}

	public bool Remove(KeyValuePair<TKey, TValue> item)
	{
		return Remove(item.Key);
	}

	public Enumerator GetEnumerator()
	{
		return new Enumerator(this);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
	{
		return GetEnumerator();
	}
}
