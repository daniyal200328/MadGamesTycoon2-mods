using UnityEngine;

public class daylightScript : MonoBehaviour
{
	private GameObject main_;

	private mainScript mS_;

	private void Start()
	{
		FindScripts();
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

	private void Update()
	{
		if ((bool)mS_)
		{
			float y = mS_.dayTimer * 90f + (float)(mS_.week - 1) * 90f;
			base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, y, base.transform.eulerAngles.z);
		}
	}
}
