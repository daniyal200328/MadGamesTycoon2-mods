using UnityEngine;

namespace ReachableGames.PostLinerPro;

public class IfMissingPackageShow : MonoBehaviour
{
	public Object _checkAsset;

	public GameObject _showIfMissing;

	private void OnEnable()
	{
		if (_checkAsset == null)
		{
			_showIfMissing.SetActive(value: true);
		}
	}
}
