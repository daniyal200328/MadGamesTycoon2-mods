using UnityEngine;

public class randomRotation : MonoBehaviour
{
	public bool randX;

	public bool randY;

	public bool randZ;

	public bool only90Degrees;

	public bool destroyMe;

	private void Start()
	{
		if (!only90Degrees)
		{
			if (randX)
			{
				base.transform.eulerAngles = new Vector3(Random.Range(0f, 359f), base.transform.eulerAngles.y, base.transform.eulerAngles.z);
			}
			if (randY)
			{
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Random.Range(0f, 359f), base.transform.eulerAngles.z);
			}
			if (randZ)
			{
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, Random.Range(0f, 359f));
			}
		}
		else
		{
			if (randX)
			{
				base.transform.eulerAngles = new Vector3(Random.Range(0, 4) * 90, base.transform.eulerAngles.y, base.transform.eulerAngles.z);
			}
			if (randY)
			{
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Random.Range(0, 4) * 90, base.transform.eulerAngles.z);
			}
			if (randZ)
			{
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, Random.Range(0, 4) * 90);
			}
		}
		if (destroyMe)
		{
			Object.Destroy(this);
		}
	}
}
