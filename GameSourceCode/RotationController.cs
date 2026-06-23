using UnityEngine;

[ExecuteInEditMode]
public class RotationController : MonoBehaviour
{
	public bool rotateObjects;

	public bool X;

	public bool Y;

	public bool Z;

	public bool grad90;

	private void Update()
	{
		if (!rotateObjects)
		{
			return;
		}
		if (X)
		{
			for (int i = 0; i < base.transform.childCount; i++)
			{
				if (grad90)
				{
					base.transform.GetChild(i).transform.eulerAngles = new Vector3(Random.Range(0, 4) * 90, base.transform.GetChild(i).transform.eulerAngles.y, base.transform.GetChild(i).transform.eulerAngles.z);
				}
				else
				{
					base.transform.GetChild(i).transform.eulerAngles = new Vector3(Random.Range(0, 360), base.transform.GetChild(i).transform.eulerAngles.y, base.transform.GetChild(i).transform.eulerAngles.z);
				}
			}
		}
		if (Y)
		{
			for (int j = 0; j < base.transform.childCount; j++)
			{
				if (grad90)
				{
					base.transform.GetChild(j).transform.eulerAngles = new Vector3(base.transform.GetChild(j).transform.eulerAngles.x, Random.Range(0, 4) * 90, base.transform.GetChild(j).transform.eulerAngles.z);
				}
				else
				{
					base.transform.GetChild(j).transform.eulerAngles = new Vector3(base.transform.GetChild(j).transform.eulerAngles.x, Random.Range(0, 360), base.transform.GetChild(j).transform.eulerAngles.z);
				}
			}
		}
		if (Z)
		{
			for (int k = 0; k < base.transform.childCount; k++)
			{
				if (grad90)
				{
					base.transform.GetChild(k).transform.eulerAngles = new Vector3(base.transform.GetChild(k).transform.eulerAngles.x, base.transform.GetChild(k).transform.eulerAngles.y, Random.Range(0, 4) * 90);
				}
				else
				{
					base.transform.GetChild(k).transform.eulerAngles = new Vector3(base.transform.GetChild(k).transform.eulerAngles.x, base.transform.GetChild(k).transform.eulerAngles.y, Random.Range(0, 360));
				}
			}
		}
		rotateObjects = false;
	}
}
