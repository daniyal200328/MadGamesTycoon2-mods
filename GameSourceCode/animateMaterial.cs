using UnityEngine;

public class animateMaterial : MonoBehaviour
{
	private GameObject main_;

	private mainScript mS_;

	public Material[] frames;

	public float speed = 1f;

	private MeshRenderer myRenderer;

	private float timer;

	private int aktFrame;

	private void Start()
	{
		FindScripts();
		myRenderer = GetComponent<MeshRenderer>();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
	}

	private void Update()
	{
		timer += speed * mS_.GetDeltaTime();
		if ((double)timer > 1.0)
		{
			timer = 0f;
			aktFrame++;
			if (aktFrame >= frames.Length)
			{
				aktFrame = 0;
			}
			myRenderer.sharedMaterial = frames[aktFrame];
		}
	}
}
