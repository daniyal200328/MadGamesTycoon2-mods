using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace AllIn1SpriteShader;

[ExecuteInEditMode]
public class All1CreateUnifiedOutline : MonoBehaviour
{
	[SerializeField]
	private Material outlineMaterial;

	[SerializeField]
	private Transform outlineParentTransform;

	[Space]
	[Header("Only needed if Sprite (ignored if UI)")]
	[SerializeField]
	private int duplicateOrderInLayer = -100;

	[SerializeField]
	private string duplicateSortingLayer = "Default";

	[Space]
	[Header("This operation will delete the component")]
	[SerializeField]
	private bool createUnifiedOutline;

	private void Update()
	{
		if (!createUnifiedOutline)
		{
			return;
		}
		if (outlineMaterial == null)
		{
			createUnifiedOutline = false;
			MissingMaterial();
			return;
		}
		List<Transform> transforms = new List<Transform>();
		GetAllChildren(base.transform, ref transforms);
		foreach (Transform item in transforms)
		{
			CreateOutlineSpriteDuplicate(item.gameObject);
		}
		CreateOutlineSpriteDuplicate(base.gameObject);
		Object.DestroyImmediate(this);
	}

	private void CreateOutlineSpriteDuplicate(GameObject target)
	{
		bool flag = false;
		SpriteRenderer component = target.GetComponent<SpriteRenderer>();
		Image component2 = target.GetComponent<Image>();
		if (component != null)
		{
			flag = false;
		}
		else if (component2 != null)
		{
			flag = true;
		}
		else if (component == null && component2 == null && !base.transform.Equals(outlineParentTransform))
		{
			return;
		}
		GameObject gameObject = new GameObject();
		gameObject.name = target.name + "Outline";
		gameObject.transform.position = target.transform.position;
		gameObject.transform.rotation = target.transform.rotation;
		gameObject.transform.localScale = target.transform.lossyScale;
		if (outlineParentTransform == null)
		{
			gameObject.transform.parent = target.transform;
		}
		else
		{
			gameObject.transform.parent = outlineParentTransform;
		}
		if (!flag)
		{
			SpriteRenderer spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
			spriteRenderer.sprite = component.sprite;
			spriteRenderer.sortingOrder = duplicateOrderInLayer;
			spriteRenderer.sortingLayerName = duplicateSortingLayer;
			spriteRenderer.material = outlineMaterial;
			spriteRenderer.flipX = component.flipX;
			spriteRenderer.flipY = component.flipY;
		}
		else
		{
			Image image = gameObject.AddComponent<Image>();
			image.sprite = component2.sprite;
			image.material = outlineMaterial;
		}
	}

	private void MissingMaterial()
	{
	}

	private void GetAllChildren(Transform parent, ref List<Transform> transforms)
	{
		foreach (Transform item in parent)
		{
			transforms.Add(item);
			GetAllChildren(item, ref transforms);
		}
	}
}
