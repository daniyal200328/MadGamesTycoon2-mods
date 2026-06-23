using System.Collections.Generic;
using UnityEngine;

namespace ReachableGames.PostLinerPro;

[ExecuteInEditMode]
public class PostLinerRenderer : MonoBehaviour
{
	private delegate void DoAction(Transform g);

	[HideInInspector]
	public int _outlineLayer;

	private Camera _hiddenCamera;

	private RenderTexture _renderTexture;

	private static int _globalTextureId = Shader.PropertyToID("_OutlineDepth");

	private HashSet<Transform> _outlineObjects = new HashSet<Transform>();

	private List<int> _objectLayers = new List<int>();

	public static PostLinerRenderer Instance = null;

	private Queue<Transform> _recursiveList = new Queue<Transform>();

	private void Awake()
	{
		Instance = null;
	}

	private void Start()
	{
		Instance = this;
	}

	private void OnDestroy()
	{
		Instance = null;
	}

	public void ClearAllOutlines()
	{
		_recursiveList.Clear();
		_outlineObjects.Clear();
		_objectLayers.Clear();
	}

	public void AddToOutlines(Transform t)
	{
		DoRecursive(t, delegate(Transform o)
		{
			_outlineObjects.Add(o);
		});
	}

	public void RemoveFromOutlines(Transform t)
	{
		DoRecursive(t, delegate(Transform o)
		{
			_outlineObjects.Remove(o);
		});
	}

	private void DoRecursive(Transform root, DoAction action)
	{
		_recursiveList.Clear();
		_recursiveList.Enqueue(root);
		while (_recursiveList.Count > 0)
		{
			Transform transform = _recursiveList.Dequeue();
			foreach (Transform item in transform)
			{
				_recursiveList.Enqueue(item);
			}
			action(transform);
		}
	}

	public void OnPreRender()
	{
		UpdateRenderTexture(Camera.current);
	}

	private void UpdateRenderTexture(Camera c)
	{
		if (c.depthTextureMode == DepthTextureMode.None)
		{
			c.depthTextureMode = DepthTextureMode.Depth;
		}
		_recursiveList.Clear();
		_objectLayers.Clear();
		foreach (Transform outlineObject in _outlineObjects)
		{
			if (outlineObject == null)
			{
				_recursiveList.Enqueue(outlineObject);
				continue;
			}
			_objectLayers.Add(outlineObject.gameObject.layer);
			outlineObject.gameObject.layer = _outlineLayer;
		}
		while (_recursiveList.Count > 0)
		{
			_outlineObjects.Remove(_recursiveList.Dequeue());
		}
		if (_hiddenCamera == null)
		{
			GameObject gameObject = new GameObject("OutlineCamera");
			gameObject.hideFlags = HideFlags.HideAndDontSave;
			gameObject.transform.SetParent(base.transform);
			_hiddenCamera = gameObject.AddComponent<Camera>();
		}
		_hiddenCamera.CopyFrom(c);
		if (_renderTexture == null || _hiddenCamera.pixelWidth != _renderTexture.width || _hiddenCamera.pixelHeight != _renderTexture.height)
		{
			if (_renderTexture != null)
			{
				_renderTexture.Release();
			}
			_renderTexture = new RenderTexture(_hiddenCamera.pixelWidth, _hiddenCamera.pixelHeight, 16, RenderTextureFormat.Depth, RenderTextureReadWrite.Default);
			_renderTexture.hideFlags = HideFlags.HideAndDontSave;
		}
		_hiddenCamera.enabled = false;
		_hiddenCamera.depthTextureMode = DepthTextureMode.Depth;
		_hiddenCamera.clearFlags = CameraClearFlags.Depth;
		_hiddenCamera.targetTexture = _renderTexture;
		_hiddenCamera.forceIntoRenderTexture = true;
		_hiddenCamera.rect = new Rect(0f, 0f, 1f, 1f);
		_hiddenCamera.cullingMask = 1 << _outlineLayer;
		_hiddenCamera.Render();
		Shader.SetGlobalTexture(_globalTextureId, _renderTexture);
		int num = 0;
		foreach (Transform outlineObject2 in _outlineObjects)
		{
			outlineObject2.gameObject.layer = _objectLayers[num++];
		}
	}
}
