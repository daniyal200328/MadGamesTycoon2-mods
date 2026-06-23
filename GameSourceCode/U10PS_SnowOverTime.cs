using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class U10PS_SnowOverTime : MonoBehaviour
{
	private MeshRenderer meshRenderer;

	public float speed = 0.6f;

	private float totalTime;

	private void Start()
	{
		meshRenderer = base.gameObject.GetComponent<MeshRenderer>();
		totalTime = 1f / speed * 4.71f;
	}

	private void Update()
	{
		Material[] materials = meshRenderer.materials;
		materials[0].SetFloat("_SnowAmount", (Mathf.Sin(totalTime * speed) + 1f) / 2f);
		totalTime += Time.deltaTime;
		meshRenderer.materials = materials;
	}
}
