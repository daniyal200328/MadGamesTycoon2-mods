using System.Collections;
using LylekGames.Tools;
using UnityEngine;

public class skinnedMeshCombineScript : MonoBehaviour
{
	public CombineSkinMeshesTextureAtlas myCombine;

	private void Awake()
	{
	}

	private void Start()
	{
		StartCoroutine(iCombineMesh());
	}

	private IEnumerator iCombineMesh()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		CombineMesh();
	}

	public void CombineMesh()
	{
		if ((bool)base.gameObject.transform.parent && base.gameObject.transform.parent.CompareTag("Character"))
		{
			base.transform.position = new Vector3(0f, 0f, 0f);
			base.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			base.transform.GetComponent<Animator>().speed = 0f;
			myCombine.BeginCombineMeshes();
		}
	}
}
