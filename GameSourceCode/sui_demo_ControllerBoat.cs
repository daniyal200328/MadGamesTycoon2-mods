using Suimono.Core;
using UnityEngine;

public class sui_demo_ControllerBoat : MonoBehaviour
{
	public bool isActive;

	public bool isControllable = true;

	public bool isExtraZoom;

	public bool keepAboveSurface = true;

	public bool handleObjectOcclusion = true;

	public Transform cameraTarget;

	public bool reverseYAxis = true;

	public bool reverseXAxis;

	public Vector2 mouseSensitivity = new Vector2(4f, 4f);

	public float cameraFOV = 35f;

	public Vector2 cameraOffset = new Vector2(0f, 0f);

	public float cameraLean;

	public float walkSpeed = 0.02f;

	public float runSpeed = 0.4f;

	public Vector3 rotationLimits = new Vector3(0f, 0f, -30f);

	public float minZoomAmount = 1.25f;

	public float maxZoomAmount = 8f;

	public sui_demo_animBoat targetAnimator;

	private Transform cameraObject;

	private Vector2 axisSensitivity = new Vector2(4f, 4f);

	private float followDistance = 5f;

	private float followHeight = 1f;

	private float followLat;

	private float camFOV = 35f;

	private float camRotation;

	private Vector3 camRot;

	private float camHeight;

	private bool isInWater;

	private bool isInWaterDeep;

	private bool isUnderWater;

	private Vector3 targetPosition;

	private float MouseRotationDistance;

	private float MouseVerticalDistance;

	private GameObject suimonoGameObject;

	private SuimonoModule suimonoModuleObject;

	private float followTgtDistance;

	private Quaternion targetRotation;

	private float MouseScrollDistance;

	private float setFOV = 1f;

	private float useSpeed;

	private float useSideSpeed;

	private float moveSpeed = 0.05f;

	private float moveForward;

	private float moveSideways;

	private bool isRunning;

	private Vector3 savePos;

	private float oldMouseRotation;

	private float xMove;

	private float zMove;

	private sui_demo_ControllerMaster MC;

	private sui_demo_InputController IC;

	private float waterLevel;

	private void Awake()
	{
		suimonoGameObject = GameObject.Find("SUIMONO_Module");
		if (suimonoGameObject != null)
		{
			suimonoModuleObject = suimonoGameObject.GetComponent<SuimonoModule>();
		}
		targetPosition = cameraTarget.position;
		targetRotation = cameraTarget.rotation;
		if (cameraTarget != null)
		{
			targetAnimator = cameraTarget.gameObject.GetComponent<sui_demo_animBoat>();
		}
		MC = base.gameObject.GetComponent<sui_demo_ControllerMaster>();
		IC = base.gameObject.GetComponent<sui_demo_InputController>();
	}

	private void LateUpdate()
	{
		if (rotationLimits.x != 0f)
		{
			if (cameraTarget.transform.eulerAngles.x < 360f - rotationLimits.x && cameraTarget.transform.eulerAngles.x > rotationLimits.x + 10f)
			{
				cameraTarget.transform.eulerAngles = new Vector3(360f - rotationLimits.x, cameraTarget.transform.eulerAngles.y, cameraTarget.transform.eulerAngles.z);
			}
			else if (cameraTarget.transform.eulerAngles.x > rotationLimits.x && cameraTarget.transform.eulerAngles.x < 360f - rotationLimits.x)
			{
				cameraTarget.transform.eulerAngles = new Vector3(rotationLimits.x, cameraTarget.transform.eulerAngles.y, cameraTarget.transform.eulerAngles.z);
			}
		}
		if (rotationLimits.y != 0f)
		{
			if (cameraTarget.transform.eulerAngles.y < 360f - rotationLimits.y && cameraTarget.transform.eulerAngles.y > rotationLimits.y + 10f)
			{
				cameraTarget.transform.eulerAngles = new Vector3(cameraTarget.transform.eulerAngles.x, 360f - rotationLimits.y, cameraTarget.transform.eulerAngles.z);
			}
			else if (cameraTarget.transform.eulerAngles.y > rotationLimits.y && cameraTarget.transform.eulerAngles.y < 360f - rotationLimits.y)
			{
				cameraTarget.transform.eulerAngles = new Vector3(cameraTarget.transform.eulerAngles.x, rotationLimits.y, cameraTarget.transform.eulerAngles.z);
			}
		}
		if (rotationLimits.z != 0f)
		{
			if (cameraTarget.transform.eulerAngles.z < 360f - rotationLimits.z && cameraTarget.transform.eulerAngles.z > rotationLimits.z + 10f)
			{
				cameraTarget.transform.eulerAngles = new Vector3(cameraTarget.transform.eulerAngles.x, cameraTarget.transform.eulerAngles.y, 360f - rotationLimits.z);
			}
			else if (cameraTarget.transform.eulerAngles.z > rotationLimits.z && cameraTarget.transform.eulerAngles.z < 360f - rotationLimits.z)
			{
				cameraTarget.transform.eulerAngles = new Vector3(cameraTarget.transform.eulerAngles.x, cameraTarget.transform.eulerAngles.y, rotationLimits.z);
			}
		}
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
				isRunning = IC.inputKeySHIFTL;
				if (moveForward == -1f)
				{
					isRunning = false;
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
			axisSensitivity = new Vector2(Mathf.Lerp(axisSensitivity.x, mouseSensitivity.x, Time.deltaTime * 4f), axisSensitivity.y);
			axisSensitivity = new Vector2(axisSensitivity.x, Mathf.Lerp(axisSensitivity.y, mouseSensitivity.y, Time.deltaTime * 4f));
			Cursor.lockState = CursorLockMode.None;
			if (suimonoModuleObject != null)
			{
				waterLevel = suimonoModuleObject.SuimonoGetHeight(cameraTarget.position, "object depth");
				isInWater = false;
				if (waterLevel < 0f)
				{
					waterLevel = 0f;
				}
				if (waterLevel > 0f)
				{
					isInWater = true;
					isInWaterDeep = false;
					isUnderWater = false;
					if (waterLevel >= 0.9f && waterLevel < 1.8f)
					{
						isInWaterDeep = true;
					}
					if (waterLevel >= 1.8f)
					{
						isUnderWater = true;
					}
				}
			}
			float num = 5f;
			if (isRunning && moveForward > 0f)
			{
				num = 2.5f;
			}
			moveSpeed = walkSpeed;
			if (isInWaterDeep || isUnderWater)
			{
				isRunning = false;
			}
			if (isRunning)
			{
				moveSpeed = runSpeed;
			}
			if (moveForward != 0f && moveSideways != 0f)
			{
				moveSpeed *= 0.75f;
			}
			if (!isInWater)
			{
				moveSpeed *= 0f;
			}
			useSpeed = Mathf.Lerp(useSpeed, moveSpeed * moveForward, Time.deltaTime * num);
			useSideSpeed = Mathf.Lerp(useSideSpeed, moveSpeed * moveSideways, Time.deltaTime * num);
			if (isControllable)
			{
				if (moveForward != 0f)
				{
					xMove = Mathf.Lerp(xMove, useSpeed, Time.deltaTime);
					zMove = Mathf.Lerp(zMove, useSpeed, Time.deltaTime);
					cameraTarget.eulerAngles = new Vector3(cameraTarget.eulerAngles.x, cameraTarget.eulerAngles.y + Mathf.Lerp(0f, 20f * moveSideways * moveForward, Time.deltaTime * Mathf.Abs(xMove * 10f)), cameraTarget.eulerAngles.z);
					if (isInWater)
					{
						cameraTarget.eulerAngles = new Vector3(cameraTarget.eulerAngles.x, cameraTarget.eulerAngles.y, cameraTarget.eulerAngles.z + Mathf.Lerp(0f, -130f * moveSideways * moveForward, Time.deltaTime * Mathf.Abs(zMove * 5f)));
					}
				}
				else
				{
					xMove = Mathf.Lerp(xMove, 0f, Time.deltaTime);
				}
				if ((bool)cameraTarget.GetComponent<Rigidbody>())
				{
					Vector3 vector = cameraTarget.transform.forward * xMove;
					Vector3 vector2 = new Vector3(0f, 0f, 0f);
					cameraTarget.GetComponent<Rigidbody>().MovePosition(cameraTarget.GetComponent<Rigidbody>().position + (vector + vector2));
				}
				float num2 = 2f;
				followDistance -= MouseScrollDistance * 8f;
				followDistance = Mathf.Clamp(followDistance, minZoomAmount, maxZoomAmount);
				followTgtDistance = Mathf.Lerp(followTgtDistance, followDistance, Time.deltaTime * num2);
				camRotation = Mathf.Lerp(oldMouseRotation, MouseRotationDistance * axisSensitivity.x, Time.deltaTime);
				targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y + camRotation, targetRotation.eulerAngles.z);
				cameraObject.transform.eulerAngles = new Vector3(targetRotation.eulerAngles.x, cameraObject.transform.eulerAngles.y, cameraObject.transform.eulerAngles.z);
				cameraObject.transform.eulerAngles = new Vector3(cameraObject.transform.eulerAngles.x, targetRotation.eulerAngles.y, cameraObject.transform.eulerAngles.z);
				camHeight = Mathf.Lerp(camHeight, camHeight + MouseVerticalDistance * axisSensitivity.y, Time.deltaTime);
				if (keepAboveSurface && suimonoModuleObject != null)
				{
					camHeight = Mathf.Clamp(camHeight, waterLevel + 0.25f, 12f);
				}
				else
				{
					camHeight = Mathf.Clamp(camHeight, -1f, 12f);
				}
				cameraObject.transform.position = cameraTarget.transform.position + -cameraObject.transform.forward * followTgtDistance;
				cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, cameraObject.transform.position.y + camHeight, cameraObject.transform.position.z);
				cameraObject.transform.LookAt(new Vector3(targetPosition.x, targetPosition.y + followHeight, targetPosition.z));
				if (handleObjectOcclusion)
				{
					Vector3 position = cameraTarget.transform.position;
					position = new Vector3(position.x, position.y + followHeight, position.z);
					RaycastHit hitInfo = default(RaycastHit);
					if (Physics.Linecast(position, cameraObject.transform.position, out hitInfo) && hitInfo.transform.gameObject.layer != 4 && !(hitInfo.transform == base.transform) && !(hitInfo.transform == cameraTarget))
					{
						bool flag = false;
						if (hitInfo.transform.GetComponent<Collider>() != null && hitInfo.transform.GetComponent<Collider>().isTrigger)
						{
							flag = true;
						}
						if (!flag)
						{
							cameraObject.transform.position = hitInfo.point;
						}
					}
				}
				cameraObject.transform.position = new Vector3(cameraObject.transform.position.x + cameraOffset.x, cameraObject.transform.position.y, cameraObject.transform.position.z);
				cameraObject.transform.position = new Vector3(cameraObject.transform.position.x, cameraObject.transform.position.y + cameraOffset.y, cameraObject.transform.position.z);
				cameraObject.transform.eulerAngles = new Vector3(cameraObject.transform.rotation.eulerAngles.x, cameraObject.transform.rotation.eulerAngles.y, cameraLean);
			}
			if (isControllable)
			{
				cameraObject.GetComponent<Camera>().fieldOfView = camFOV;
			}
			if (targetAnimator != null)
			{
				if (moveForward > 0f)
				{
					targetAnimator.behaviorIsRevving = true;
					targetAnimator.behaviorIsRevvingHigh = isRunning;
					targetAnimator.behaviorIsRevvingBack = false;
				}
				else if (moveForward < 0f)
				{
					targetAnimator.behaviorIsRevving = false;
					targetAnimator.behaviorIsRevvingHigh = false;
					targetAnimator.behaviorIsRevvingBack = true;
				}
				else if (moveForward == 0f)
				{
					targetAnimator.behaviorIsRevving = false;
					targetAnimator.behaviorIsRevvingHigh = false;
					targetAnimator.behaviorIsRevvingBack = false;
				}
				targetAnimator.engineRotation = moveSideways;
			}
		}
		if (targetAnimator != null)
		{
			targetAnimator.behaviorIsOn = isActive;
		}
	}
}
