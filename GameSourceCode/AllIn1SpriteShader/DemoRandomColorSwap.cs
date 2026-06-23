using UnityEngine;

namespace AllIn1SpriteShader;

public class DemoRandomColorSwap : MonoBehaviour
{
	private Material mat;

	private void Start()
	{
		if (GetComponent<SpriteRenderer>() != null)
		{
			mat = GetComponent<Renderer>().material;
			if (mat != null)
			{
				InvokeRepeating("NewColor", 0f, 0.6f);
				return;
			}
			Debug.LogError("No material found");
			Object.Destroy(this);
		}
	}

	private void NewColor()
	{
		mat.SetColor("_ColorSwapRed", GenerateColor());
		mat.SetColor("_ColorSwapGreen", GenerateColor());
		mat.SetColor("_ColorSwapBlue", GenerateColor());
	}

	private Color GenerateColor()
	{
		return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), 1f);
	}
}
