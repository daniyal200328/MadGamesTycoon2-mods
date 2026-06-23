using System.Collections;
using UnityEngine;

public class dartPfeil : MonoBehaviour
{
	private GameObject main_;

	private mainScript mS_;

	private sfxScript sfx_;

	public GameObject prefabFlyingDart;

	private GameObject myDart;

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
			if ((bool)myDart)
			{
				Object.Destroy(myDart);
			}
			myDart = Object.Instantiate(prefabFlyingDart);
			myDart.transform.position = base.gameObject.transform.position;
			myDart.transform.rotation = base.gameObject.transform.root.transform.rotation;
			myDart.transform.eulerAngles = new Vector3(myDart.transform.eulerAngles.x + (float)Random.Range(-15, 15), myDart.transform.eulerAngles.y + (float)Random.Range(-30, -15), myDart.transform.eulerAngles.z);
			StartCoroutine(Fly());
		}
	}

	private IEnumerator Fly()
	{
		base.gameObject.GetComponent<MeshRenderer>().enabled = false;
		Vector3 startPos = myDart.transform.position;
		while (Vector3.Distance(startPos, myDart.transform.position) < 0.65f)
		{
			myDart.transform.Translate(Vector3.forward * mS_.GetDeltaTime() * 5f);
			yield return new WaitForEndOfFrame();
		}
		Object.Destroy(myDart);
		if (myDart.GetComponent<MeshRenderer>().isVisible)
		{
			sfx_.Play3DSound(42, 0f, force: false, base.transform.position);
		}
		base.gameObject.GetComponent<MeshRenderer>().enabled = true;
	}
}
