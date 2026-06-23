using UnityEngine;

public class floorScript : MonoBehaviour
{
	public GameObject myObject;

	public Material[] materials;

	public void SetFilterTexture()
	{
		myObject.GetComponent<Renderer>().material = materials[1];
	}

	public void SetStandardTexture()
	{
		myObject.GetComponent<Renderer>().material = materials[0];
	}
}
