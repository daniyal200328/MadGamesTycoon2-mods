using UnityEngine;

public class urinScript : MonoBehaviour
{
	public Material[] myMaterial;

	public GameObject myGFX;

	private void Start()
	{
		myGFX.GetComponent<MeshRenderer>().material = myMaterial[Random.Range(0, myMaterial.Length)];
		base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Random.Range(0f, 360f), base.transform.eulerAngles.z);
	}
}
