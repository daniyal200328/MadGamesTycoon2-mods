using UnityEngine;

namespace MeshDistortLite;

public class Math : ScriptableObject
{
	public static float Repeat(float num, float min, float max)
	{
		if (num < min)
		{
			return max - (min - num) % (max - min);
		}
		return min + (num - min) % (max - min);
	}

	public static float PingPong(float num, float min, float max)
	{
		min = Repeat(num, min, 2f * max);
		if (min < max)
		{
			return min;
		}
		return 2f * max - min;
	}
}
