using UnityEngine;

public class maschineScreen : MonoBehaviour
{
	public MeshRenderer renderer;

	public Material[] mat;

	private float timer;

	private float rnd;

	private roomScript roomS_;

	private mapScript mapS_;

	private mainScript mS_;

	private objectScript oS_;

	private void Start()
	{
		mS_ = GameObject.FindGameObjectWithTag("Main").GetComponent<mainScript>();
	}

	private void Update()
	{
		timer += mS_.GetDeltaTime();
		if (timer > rnd)
		{
			timer = 0f;
			rnd = Random.Range(0.1f, 1.5f);
			renderer.sharedMaterial = mat[Random.Range(1, mat.Length)];
		}
	}
}
