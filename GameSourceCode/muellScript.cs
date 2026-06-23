using UnityEngine;

public class muellScript : MonoBehaviour
{
	public int myGFXSlot = -1;

	public mainScript mS_;

	public GameObject main_;

	private void Start()
	{
		FindScripts();
		if ((bool)mS_)
		{
			mS_.findMuell = true;
		}
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
	}

	private void OnDestroy()
	{
		if ((bool)mS_)
		{
			mS_.findMuell = true;
		}
	}
}
