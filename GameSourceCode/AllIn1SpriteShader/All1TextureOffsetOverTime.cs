using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader;

public class All1TextureOffsetOverTime : MonoBehaviour
{
	[SerializeField]
	private string texturePropertyName = "_MainTex";

	[SerializeField]
	private Vector2 offsetSpeed;

	[SerializeField]
	[Header("If missing will search object Sprite Renderer or UI Image")]
	private Material mat;

	private int textureShaderId;

	private Vector2 currOffset = Vector2.zero;

	private void Start()
	{
		if (mat == null)
		{
			SpriteRenderer component = GetComponent<SpriteRenderer>();
			if (component != null)
			{
				mat = component.material;
			}
			else
			{
				Image component2 = GetComponent<Image>();
				if (component2 != null)
				{
					mat = component2.material;
				}
			}
		}
		if (mat == null)
		{
			DestroyComponentAndLogError(base.gameObject.name + " has no valid Material, deleting All1TextureOffsetOverTIme component");
		}
		else if (mat.HasProperty(texturePropertyName))
		{
			textureShaderId = Shader.PropertyToID(texturePropertyName);
		}
		else
		{
			DestroyComponentAndLogError(base.gameObject.name + "'s Material doesn't have a " + texturePropertyName + " property");
		}
	}

	private void Update()
	{
		currOffset.x += offsetSpeed.x * Time.deltaTime;
		currOffset.y += offsetSpeed.y * Time.deltaTime;
		mat.SetTextureOffset(textureShaderId, currOffset);
	}

	private void DestroyComponentAndLogError(string logError)
	{
		Debug.LogError(logError);
		Object.Destroy(this);
	}
}
