using System.Collections;
using UnityEngine;

namespace ReachableGames.PostLinerPro;

public class PostLinerOutline : MonoBehaviour
{
	private Coroutine _inProcess;

	private bool _isQuitting;

	private void OnEnable()
	{
		if (_inProcess != null)
		{
			StopCoroutine(_inProcess);
		}
		if (PostLinerRenderer.Instance != null)
		{
			PostLinerRenderer.Instance.AddToOutlines(base.transform);
		}
		else
		{
			_inProcess = StartCoroutine(Add());
		}
	}

	private void OnDisable()
	{
		if (_inProcess != null)
		{
			StopCoroutine(_inProcess);
		}
		if (PostLinerRenderer.Instance != null)
		{
			PostLinerRenderer.Instance.RemoveFromOutlines(base.transform);
		}
		else if (!_isQuitting)
		{
			_inProcess = StartCoroutine(Remove());
		}
	}

	private void OnApplicationQuit()
	{
		_isQuitting = true;
	}

	private IEnumerator Add()
	{
		yield return new WaitUntil(() => PostLinerRenderer.Instance != null);
		PostLinerRenderer.Instance.AddToOutlines(base.transform);
	}

	private IEnumerator Remove()
	{
		yield return new WaitUntil(() => PostLinerRenderer.Instance != null);
		PostLinerRenderer.Instance.RemoveFromOutlines(base.transform);
	}
}
