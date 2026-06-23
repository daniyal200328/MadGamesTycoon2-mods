using UnityEngine;

public class golfScript : MonoBehaviour
{
	private GameObject main_;

	private mainScript mS_;

	private sfxScript sfx_;

	public GameObject prefabFlyingBall;

	private GameObject myBall;

	public float timer;

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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
	}

	private void OnEnable()
	{
		timer = 0.72700006f;
	}

	private void Update()
	{
		timer += mS_.GetDeltaTime();
		if ((double)timer >= 1.417)
		{
			timer = 0f;
			if ((bool)myBall)
			{
				Object.Destroy(myBall);
			}
			myBall = Object.Instantiate(prefabFlyingBall);
			myBall.transform.position = base.gameObject.transform.position;
			myBall.transform.rotation = base.gameObject.transform.root.transform.rotation;
			myBall.transform.eulerAngles = new Vector3(myBall.transform.eulerAngles.x + (float)Random.Range(-15, 15), myBall.transform.eulerAngles.y + (float)Random.Range(-30, -15), myBall.transform.eulerAngles.z);
		}
	}
}
