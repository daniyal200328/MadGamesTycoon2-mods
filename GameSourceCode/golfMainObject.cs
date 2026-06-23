using UnityEngine;

public class golfMainObject : MonoBehaviour
{
	public GameObject mainObject;

	public AudioSource audio_;

	private void Start()
	{
	}

	public void RandomRotation()
	{
		if ((bool)mainObject)
		{
			mainObject.transform.localEulerAngles = new Vector3(0f, Random.Range(-13f, 6f), 0f);
			audio_.Play();
		}
	}
}
