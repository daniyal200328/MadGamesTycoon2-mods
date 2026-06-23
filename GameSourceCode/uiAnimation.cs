using UnityEngine;

public class uiAnimation : MonoBehaviour
{
	public string anim = "";

	private void Start()
	{
	}

	private void OnEnable()
	{
		Debug.Log("LKJK" + Random.Range(0, 100000) + " " + base.gameObject.name);
		GetComponent<Animator>().Play(anim);
		GetComponent<characterScript>().male = true;
	}
}
