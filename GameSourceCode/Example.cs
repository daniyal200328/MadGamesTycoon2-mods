using UnityEngine;

public class Example : MonoBehaviour
{
	public GameObject sphere;

	public void Update()
	{
		base.transform.parent.eulerAngles = new Vector3(base.transform.parent.eulerAngles.x, base.transform.parent.eulerAngles.y + Time.deltaTime * 10f, base.transform.parent.eulerAngles.z);
	}

	public void generateSphereOnLeft()
	{
		Object.Instantiate(sphere).transform.position = new Vector3(-11.09958f, 16f, 3.5f);
	}

	public void generateSphereInCentre()
	{
		Object.Instantiate(sphere).transform.position = new Vector3(Random.Range(-0.01f, 0.01f), 16f, Random.Range(-0.01f, 0.01f));
	}

	public void generateSphereOnRight()
	{
		Object.Instantiate(sphere).transform.position = new Vector3(11.09958f, 16f, -3.5f);
	}
}
