using UnityEngine;

namespace Suimono.Core;

[ExecuteInEditMode]
public class SuimonoCamera_depth : MonoBehaviour
{
	[HideInInspector]
	public float _sceneDepth = 20f;

	[HideInInspector]
	public float _shoreDepth = 45f;

	private Material useMat;

	private void Start()
	{
		useMat = new Material(Shader.Find("Suimono2/SuimonoDepth"));
	}

	private void LateUpdate()
	{
		_sceneDepth = Mathf.Clamp(_sceneDepth, 0f, 100f);
		_shoreDepth = Mathf.Clamp(_shoreDepth, 0f, 100f);
		useMat.SetFloat("_sceneDepth", _sceneDepth);
		useMat.SetFloat("_shoreDepth", _shoreDepth);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination, useMat);
	}
}
