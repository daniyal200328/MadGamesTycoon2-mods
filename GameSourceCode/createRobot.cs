using UnityEngine;

public class createRobot : MonoBehaviour
{
	public GameObject prefabRobot;

	public GameObject destroyThis;

	public GameObject myRobot;

	private void Start()
	{
	}

	public void Init(int id_)
	{
		if ((bool)myRobot)
		{
			Object.Destroy(myRobot);
		}
		myRobot = Object.Instantiate(prefabRobot, base.transform.position, Quaternion.identity);
		myRobot.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, base.transform.eulerAngles.y, base.transform.eulerAngles.z + 180f);
		myRobot.GetComponent<Animation>().Play();
		myRobot.GetComponent<robotScript>().stationID = id_;
		Object.Destroy(destroyThis);
	}

	private void OnDestroy()
	{
		if ((bool)myRobot)
		{
			GameObject myTarget = myRobot.GetComponent<robotScript>().myTarget;
			if ((bool)myTarget && myTarget.tag == "Muell_InUse")
			{
				myTarget.tag = "Muell";
			}
			Object.Destroy(myRobot);
		}
	}
}
