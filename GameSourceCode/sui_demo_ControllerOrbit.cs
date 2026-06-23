using Suimono.Core;
using UnityEngine;

public class sui_demo_ControllerOrbit : MonoBehaviour
{
	public bool isActive;

	public bool isControllable = true;

	public bool isExtraZoom;

	public Transform cameraTarget;

	public bool reverseYAxis = true;

	public bool reverseXAxis;

	public Vector2 mouseSensitivity = new Vector2(4f, 4f);

	public float cameraFOV = 35f;

	public Vector3 cameraOffset = new Vector3(0f, 0f, 0f);

	public float cameraLean;

	public float rotationSensitivity = 6f;

	public Vector3 rotationLimits = new Vector3(0f, 0f, 0f);

	public float minZoomAmount = 1.25f;

	public float maxZoomAmount = 8f;

	public bool showDebug;

	public sui_demo_animCharacter targetAnimator;

	private Transform cameraObject;

	private Vector2 axisSensitivity = new Vector2(4f, 4f);

	private float followDistance = 10f;

	private float followHeight = 1f;

	private float followLat;

	private float camFOV = 35f;

	private float camRotation;

	private Vector3 camRot;

	private float camHeight = 4f;

	private bool isInWater;

	private bool isInWaterDeep;

	private bool isUnderWater;

	private Vector3 targetPosition;

	private float MouseRotationDistance;

	private float MouseVerticalDistance;

	private GameObject suimonoGameObject;

	private SuimonoModule suimonoModuleObject;

	private float followTgtDistance;

	private bool orbitView;

	private Quaternion targetRotation;

	private float MouseScrollDistance;

	private float setFOV = 1f;

	private float moveForward;

	private float moveSideways;

	private bool isWalking;

	private bool isRunning;

	private bool isSprinting;

	private Vector3 savePos;

	private float oldMouseRotation;

	private sui_demo_ControllerMaster MC;

	private sui_demo_InputController IC;

	private float runButtonTime;

	private bool toggleRun;

	private float gSlope;

	private float useSlope;

	private void Awake()
	{
		suimonoGameObject = GameObject.Find("SUIMONO_Module");
		if (suimonoGameObject != null)
		{
			suimonoModuleObject = suimonoGameObject.GetComponent<SuimonoModule>();
		}
		targetPosition = cameraTarget.position;
		targetRotation = cameraTarget.rotation;
		MC = base.gameObject.GetComponent<sui_demo_ControllerMaster>();
		IC = base.gameObject.GetComponent<sui_demo_InputController>();
	}

	private void FixedUpdate()
	{
		if (isActive)
		{
			cameraObject = MC.cameraObject;
			if (isControllable)
			{
				moveForward = 0f;
				moveSideways = 0f;
				if (IC.inputKeyW)
				{
					moveForward = 1f;
				}
				if (IC.inputKeyS)
				{
					moveForward = -1f;
				}
				if (IC.inputKeyA)
				{
					moveSideways = -1f;
				}
				if (IC.inputKeyD)
				{
					moveSideways = 1f;
				}
				isExtraZoom = IC.inputMouseKey1;
				if (isExtraZoom)
				{
					setFOV = 0.5f;
				}
				else
				{
					setFOV = 1f;
				}
				isWalking = false;
				if (moveForward != 0f || moveSideways != 0f)
				{
					isWalking = true;
				}
				if (IC.inputKeySHIFTL)
				{
					runButtonTime += Time.deltaTime;
					if (runButtonTime > 0.2f)
					{
						isSprinting = true;
					}
				}
				else
				{
					if (runButtonTime > 0f)
					{
						if (runButtonTime < 0.2f)
						{
							isRunning = !isRunning;
							if (isRunning)
							{
								toggleRun = true;
							}
						}
						if (runButtonTime > 0.2f)
						{
							isRunning = false;
						}
					}
					if (isSprinting && toggleRun)
					{
						isRunning = true;
					}
					isSprinting = false;
					runButtonTime = 0f;
				}
				if (Input.mousePosition.x > 325f || Input.mousePosition.y < 265f)
				{
					orbitView = IC.inputMouseKey0 || IC.inputMouseKey1;
				}
				else
				{
					orbitView = false;
					IC.inputMouseKey0 = false;
					IC.inputMouseKey1 = false;
				}
			}
			targetPosition = cameraTarget.position;
			oldMouseRotation = MouseRotationDistance;
			MouseRotationDistance = IC.inputMouseX;
			MouseVerticalDistance = IC.inputMouseY;
			MouseScrollDistance = IC.inputMouseWheel;
			if (reverseXAxis)
			{
				MouseRotationDistance = 0f - IC.inputMouseX;
			}
			if (reverseYAxis)
			{
				MouseVerticalDistance = 0f - IC.inputMouseY;
			}
			if (!isControllable)
			{
				camFOV = 63.2f;
				followLat = Mathf.Lerp(followLat, -0.85f, Time.deltaTime * 4f);
				followHeight = Mathf.Lerp(followHeight, 1.8f, Time.deltaTime * 4f);
				followDistance = Mathf.Lerp(followDistance, 5f, Time.deltaTime * 4f);
				axisSensitivity = new Vector2(Mathf.Lerp(axisSensitivity.x, mouseSensitivity.x, Time.deltaTime * 4f), Mathf.Lerp(axisSensitivity.y, mouseSensitivity.y, Time.deltaTime * 4f));
				cameraObject.GetComponent<Camera>().fieldOfView = camFOV;
			}
			camFOV = Mathf.Lerp(camFOV, cameraFOV * setFOV, Time.deltaTime * 4f);
			followLat = Mathf.Lerp(followLat, -0.4f, Time.deltaTime * 4f);
			followHeight = Mathf.Lerp(followHeight, 1.4f, Time.deltaTime * 2f);
			axisSensitivity = new Vector2(Mathf.Lerp(axisSensitivity.x, mouseSensitivity.x, Time.deltaTime * 4f), Mathf.Lerp(axisSensitivity.y, mouseSensitivity.y, Time.deltaTime * 4f));
			Cursor.lockState = CursorLockMode.None;
			if (suimonoModuleObject != null)
			{
				float num = suimonoModuleObject.SuimonoGetHeight(cameraTarget.position, "object depth");
				isInWater = false;
				if (num < 0f)
				{
					num = 0f;
				}
				if (num > 0f)
				{
					isInWater = true;
					isInWaterDeep = false;
					isUnderWater = false;
					if (num >= 0.9f && num < 1.8f)
					{
						isInWaterDeep = true;
					}
					if (num >= 1.8f)
					{
						isUnderWater = true;
					}
				}
			}
			if (isControllable)
			{
				float num2 = 2f;
				followDistance -= MouseScrollDistance * 20f;
				followDistance = Mathf.Clamp(followDistance, minZoomAmount, maxZoomAmount);
				followTgtDistance = Mathf.Lerp(followTgtDistance, followDistance, Time.deltaTime * num2);
				if (orbitView)
				{
					camRotation = Mathf.Lerp(oldMouseRotation, MouseRotationDistance * axisSensitivity.x, Time.deltaTime);
				}
				targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y + camRotation, targetRotation.eulerAngles.z);
				cameraObject.transform.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, cameraObject.transform.eulerAngles.z);
				if (orbitView)
				{
					camHeight = Mathf.Lerp(camHeight, camHeight + MouseVerticalDistance * axisSensitivity.y, Time.deltaTime);
				}
				camHeight = Mathf.Clamp(camHeight, -45f, 45f);
				cameraObject.transform.position = new Vector3(cameraTarget.transform.position.x + cameraOffset.x + (0f - cameraObject.transform.up.x) * followTgtDistance, Mathf.Lerp(camHeight, cameraTarget.transform.position.y + cameraOffset.y + (0f - cameraObject.transform.up.y) * followTgtDistance, Time.deltaTime * 0.5f), cameraTarget.transform.position.z + cameraOffset.z + (0f - cameraObject.transform.up.z) * followTgtDistance);
				cameraObject.transform.LookAt(new Vector3(targetPosition.x, targetPosition.y + followHeight, targetPosition.z));
			}
			if (isControllable)
			{
				cameraObject.GetComponent<Camera>().fieldOfView = camFOV;
			}
		}
		if (targetAnimator != null)
		{
			targetAnimator.isWalking = isWalking;
			targetAnimator.isRunning = isRunning;
			targetAnimator.isSprinting = isSprinting;
			targetAnimator.moveForward = moveForward;
			targetAnimator.moveSideways = moveSideways;
			targetAnimator.gSlope = gSlope;
			targetAnimator.useSlope = useSlope;
			targetAnimator.isInWater = isInWater;
			targetAnimator.isInWaterDeep = isInWaterDeep;
			targetAnimator.isUnderWater = isUnderWater;
		}
	}
}
