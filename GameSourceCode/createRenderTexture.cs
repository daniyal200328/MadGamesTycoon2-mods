using UnityEngine;
using UnityEngine.UI;

public class createRenderTexture : MonoBehaviour
{
	private RenderTexture rt;

	private Camera camera_;

	public GameObject cameraOutlineImage;

	private int screenW;

	private int screenH;

	private void Start()
	{
		CreateNewTexture();
		base.gameObject.SetActive(value: false);
	}

	private void Update()
	{
		if (screenW != Screen.width)
		{
			CreateNewTexture();
		}
		else if (screenH != Screen.height)
		{
			CreateNewTexture();
		}
	}

	private void OnEnable()
	{
		if ((bool)cameraOutlineImage)
		{
			cameraOutlineImage.SetActive(value: true);
		}
	}

	private void OnDisable()
	{
		if ((bool)cameraOutlineImage)
		{
			cameraOutlineImage.SetActive(value: false);
		}
	}

	private void CreateNewTexture()
	{
		rt = new RenderTexture(Screen.width, Screen.height, 16, RenderTextureFormat.ARGB32);
		rt.Create();
		camera_ = GetComponent<Camera>();
		camera_.targetTexture = rt;
		cameraOutlineImage.GetComponent<RawImage>().texture = rt;
		screenW = Screen.width;
		screenH = Screen.height;
	}
}
