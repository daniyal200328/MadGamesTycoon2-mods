using UnityEngine;

public class MakeSpheres : MonoBehaviour
{
	public GameObject spherePrefab;

	public int numberOfSpheres = 12;

	public float area = 4.5f;

	private void Start()
	{
		for (int i = 0; i < numberOfSpheres; i++)
		{
			Object.Instantiate(spherePrefab, new Vector3(Random.Range(0f - area, area), Random.Range(0f - area, area), Random.Range(0f - area, area)), Random.rotation);
		}
	}
}
