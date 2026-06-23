using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class U10PS_DissolveOverTime : MonoBehaviour
{
	private MeshRenderer meshRenderer;

	public float speed = 0.5f;

	private float t;

	private void Start()
	{
		meshRenderer = GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		Material[] materials = meshRenderer.materials;
		materials[0].SetFloat("_Cutoff", Mathf.Sin(t * speed));
		t += Time.deltaTime;
		meshRenderer.materials = materials;
	}
}
