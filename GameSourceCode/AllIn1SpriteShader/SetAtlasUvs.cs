using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader;

[ExecuteInEditMode]
public class SetAtlasUvs : MonoBehaviour
{
	[SerializeField]
	private bool updateEveryFrame;

	private Renderer render;

	private SpriteRenderer spriteRender;

	private Image uiImage;

	private bool isUI;

	private void Start()
	{
		Setup();
	}

	private void Reset()
	{
		Setup();
	}

	private void Setup()
	{
		if (GetRendererReferencesIfNeeded())
		{
			GetAndSetUVs();
		}
		if (!updateEveryFrame && Application.isPlaying && this != null)
		{
			base.enabled = false;
		}
	}

	private void OnWillRenderObject()
	{
		if (updateEveryFrame)
		{
			GetAndSetUVs();
		}
	}

	public void GetAndSetUVs()
	{
		if (GetRendererReferencesIfNeeded())
		{
			if (!isUI)
			{
				Rect textureRect = spriteRender.sprite.textureRect;
				textureRect.x /= spriteRender.sprite.texture.width;
				textureRect.width /= spriteRender.sprite.texture.width;
				textureRect.y /= spriteRender.sprite.texture.height;
				textureRect.height /= spriteRender.sprite.texture.height;
				render.sharedMaterial.SetFloat("_MinXUV", textureRect.xMin);
				render.sharedMaterial.SetFloat("_MaxXUV", textureRect.xMax);
				render.sharedMaterial.SetFloat("_MinYUV", textureRect.yMin);
				render.sharedMaterial.SetFloat("_MaxYUV", textureRect.yMax);
			}
			else
			{
				Rect textureRect2 = uiImage.sprite.textureRect;
				textureRect2.x /= uiImage.sprite.texture.width;
				textureRect2.width /= uiImage.sprite.texture.width;
				textureRect2.y /= uiImage.sprite.texture.height;
				textureRect2.height /= uiImage.sprite.texture.height;
				uiImage.material.SetFloat("_MinXUV", textureRect2.xMin);
				uiImage.material.SetFloat("_MaxXUV", textureRect2.xMax);
				uiImage.material.SetFloat("_MinYUV", textureRect2.yMin);
				uiImage.material.SetFloat("_MaxYUV", textureRect2.yMax);
			}
		}
	}

	public void ResetAtlasUvs()
	{
		if (GetRendererReferencesIfNeeded())
		{
			if (!isUI)
			{
				render.sharedMaterial.SetFloat("_MinXUV", 0f);
				render.sharedMaterial.SetFloat("_MaxXUV", 1f);
				render.sharedMaterial.SetFloat("_MinYUV", 0f);
				render.sharedMaterial.SetFloat("_MaxYUV", 1f);
			}
			else
			{
				uiImage.material.SetFloat("_MinXUV", 0f);
				uiImage.material.SetFloat("_MaxXUV", 1f);
				uiImage.material.SetFloat("_MinYUV", 0f);
				uiImage.material.SetFloat("_MaxYUV", 1f);
			}
		}
	}

	public void UpdateEveryFrame(bool everyFrame)
	{
		updateEveryFrame = everyFrame;
	}

	private bool GetRendererReferencesIfNeeded()
	{
		if (spriteRender == null)
		{
			spriteRender = GetComponent<SpriteRenderer>();
		}
		if (spriteRender != null)
		{
			if (spriteRender.sprite == null)
			{
				Object.DestroyImmediate(this);
				return false;
			}
			if (render == null)
			{
				render = GetComponent<Renderer>();
			}
			isUI = false;
		}
		else
		{
			if (uiImage == null)
			{
				uiImage = GetComponent<Image>();
				if (!(uiImage != null))
				{
					Object.DestroyImmediate(this);
					return false;
				}
			}
			if (render == null)
			{
				render = GetComponent<Renderer>();
			}
			isUI = true;
		}
		if (spriteRender == null && uiImage == null)
		{
			Object.DestroyImmediate(this);
			return false;
		}
		return true;
	}
}
