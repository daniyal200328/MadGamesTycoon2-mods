using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader;

public class AllIn1ScrollProperty : MonoBehaviour
{
	[SerializeField]
	private string numericPropertyName = "_RotateUvAmount";

	[SerializeField]
	private float scrollSpeed;

	[Space]
	[SerializeField]
	private bool applyModulo;

	[SerializeField]
	private float modulo = 1f;

	[Space]
	[SerializeField]
	[Header("If missing will search object Sprite Renderer or UI Image")]
	private Material mat;

	private int propertyShaderID;

	private float currValue;

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
			return;
		}
		if (mat.HasProperty(numericPropertyName))
		{
			propertyShaderID = Shader.PropertyToID(numericPropertyName);
		}
		else
		{
			DestroyComponentAndLogError(base.gameObject.name + "'s Material doesn't have a " + numericPropertyName + " property");
		}
		currValue = mat.GetFloat(propertyShaderID);
	}

	private void Update()
	{
		currValue += scrollSpeed * Time.deltaTime;
		if (applyModulo)
		{
			currValue %= modulo;
		}
		mat.SetFloat(propertyShaderID, currValue);
	}

	private void DestroyComponentAndLogError(string logError)
	{
		Debug.LogError(logError);
		Object.Destroy(this);
	}
}
