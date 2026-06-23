using System.Collections.Generic;
using UnityEngine;

public class GrassController : MonoBehaviour
{
	public List<GameObject> grassPrefabs = new List<GameObject>();

	public int grassNumber = 64;

	public float grassAreaWidth = 5f;

	public float grassAreaHeight = 5f;

	public string interactionTag = "Player";

	private Vector4[] grassInteractionPositions = new Vector4[4];

	private Transform ground;

	private List<GameObject> grass = new List<GameObject>();

	private void Awake()
	{
		ground = base.transform;
		float num = grassAreaWidth / 2f;
		float num2 = grassAreaHeight / 2f;
		for (int i = 0; i < grassNumber; i++)
		{
			Vector3 position = base.transform.position + new Vector3(Random.Range(0f - num, num), 0f, Random.Range(0f - num2, num2));
			GameObject item = Object.Instantiate(grassPrefabs[Random.Range(0, grassPrefabs.Count)], position, Quaternion.Euler(0f, Random.Range(0, 360), 0f), ground.transform);
			grass.Add(item);
		}
	}

	private void Update()
	{
		int num = 0;
		GameObject[] array = GameObject.FindGameObjectsWithTag(interactionTag);
		foreach (GameObject gameObject in array)
		{
			grassInteractionPositions[num++] = gameObject.transform.position + new Vector3(0f, 0.5f, 0f);
		}
		Shader.SetGlobalFloat("_PositionArray", num);
		Shader.SetGlobalVectorArray("_Positions", grassInteractionPositions);
	}
}
