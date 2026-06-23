using UnityEngine;

public class eyeScript : MonoBehaviour
{
	public float timer;

	public GameObject myCamera;

	public Animation myAnimation;

	private void Start()
	{
		myCamera = GameObject.Find("Camera");
		myAnimation = GetComponent<Animation>();
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (!(timer < 5f))
		{
			timer = 0f;
			if (myCamera.transform.localPosition.z < -9.5f)
			{
				myAnimation.Stop();
				Debug.Log("STOP");
			}
			else
			{
				myAnimation.Play();
				Debug.Log("PLAY");
			}
		}
	}
}
