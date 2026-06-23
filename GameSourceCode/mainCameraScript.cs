using ReachableGames.PostLinerPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.PostProcessing;

public class mainCameraScript : MonoBehaviour
{
	public bool startZoomOut = true;

	public float zoomSpeed = 2f;

	public float maxZoomIn = 3f;

	public float maxZoomOut = 20f;

	public PostProcessVolume postVolume;

	public ColorParameter[] colorParameter;

	public GameObject[] additionalCamera;

	private Vector3 cameraPosition;

	private cameraMovementScript cmS_;

	public PostLinerPro postLiner;

	public PostLinerRenderer postLineRenderer;

	private void Start()
	{
		cmS_ = base.transform.root.gameObject.GetComponent<cameraMovementScript>();
		cameraPosition = base.transform.localPosition;
		InitPostProcess();
	}

	private void InitPostProcess()
	{
		postVolume.profile.TryGetSettings<PostLinerPro>(out postLiner);
	}

	public void SetOutlineColor(int fillColor_, float fillBlend_, int outlineColor_)
	{
		if (postLiner == null)
		{
			InitPostProcess();
			return;
		}
		if (!postLiner.active)
		{
			postLiner.active = true;
		}
		if (!postLineRenderer.enabled)
		{
			postLineRenderer.enabled = true;
		}
		postLiner.fillColor.Override(colorParameter[fillColor_]);
		postLiner.fillBlend.Override(fillBlend_);
		postLiner.outlineColor.Override(colorParameter[outlineColor_]);
	}

	public void DisablePostLineRenderer()
	{
		if (postLineRenderer.enabled)
		{
			postLineRenderer.enabled = false;
		}
		if (postLiner.active)
		{
			postLiner.active = false;
		}
	}

	public void EnablePostLiner()
	{
		if (!postLiner.active)
		{
			postLiner.active = true;
		}
		if (!postLineRenderer.enabled)
		{
			postLineRenderer.enabled = true;
		}
	}

	private void Update()
	{
		CameraInput();
		LookAtCameraMovement();
	}

	private void LookAtCameraMovement()
	{
		base.transform.LookAt(base.transform.parent.transform);
	}

	private void CameraInput()
	{
		if (!cmS_.guiMain_ || cmS_.disableMovement)
		{
			return;
		}
		if (Input.mouseScrollDelta.y != 0f)
		{
			if (!Input.GetMouseButton(1))
			{
				Vector3 localPosition = base.transform.localPosition;
				base.transform.localPosition = cameraPosition;
				if (!EventSystem.current.IsPointerOverGameObject() || cmS_.guiMain_.uiObjects[252].activeSelf)
				{
					base.transform.Translate(Vector3.forward * Input.mouseScrollDelta.y * zoomSpeed);
				}
				cameraPosition = base.transform.localPosition;
				base.transform.localPosition = localPosition;
				if (cameraPosition.y < maxZoomIn)
				{
					cameraPosition = localPosition;
				}
				if (cameraPosition.y > maxZoomOut)
				{
					cameraPosition = localPosition;
				}
			}
		}
		else
		{
			float num = 0f;
			if (Input.GetKey(KeyCode.PageUp))
			{
				num = 0.2f;
			}
			if (Input.GetKey(KeyCode.PageDown))
			{
				num = -0.2f;
			}
			Vector3 localPosition2 = base.transform.localPosition;
			base.transform.localPosition = cameraPosition;
			if (!EventSystem.current.IsPointerOverGameObject())
			{
				base.transform.Translate(Vector3.forward * num * zoomSpeed);
			}
			cameraPosition = base.transform.localPosition;
			base.transform.localPosition = localPosition2;
			if (cameraPosition.y < maxZoomIn)
			{
				cameraPosition = localPosition2;
			}
			if (cameraPosition.y > maxZoomOut)
			{
				cameraPosition = localPosition2;
			}
		}
		if (!(Vector3.Distance(base.transform.localPosition, cameraPosition) < 0.01f))
		{
			base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, cameraPosition, 0.1f);
		}
	}
}
