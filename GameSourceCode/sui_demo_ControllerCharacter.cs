using Suimono.Core;
using UnityEngine;

public class sui_demo_ControllerCharacter : MonoBehaviour
{
	public bool isActive;

	public bool isControllable = true;

	public bool isExtraZoom;

	public bool keepAboveSurface;

	public bool handleObjectOcclusion = true;

	public Transform cameraTarget;

	public Transform buoyancyTarget;

	public bool reverseYAxis = true;

	public bool reverseXAxis;

	public Vector2 mouseSensitivity = new Vector2(4f, 4f);

	public float cameraFOV = 35f;

	public Vector2 cameraOffset = new Vector2(0f, 0f);

	public float cameraLean;

	public float walkSpeed = 0.02f;

	public float runSpeed = 0.4f;

	public float sprintSpeed = 0.4f;

	public float rotationSensitivity = 6f;

	public Vector3 rotationLimits = new Vector3(0f, 0f, 0f);

	public float minZoomAmount = 1.25f;

	public float maxZoomAmount = 8f;

	public bool showDebug;

	public sui_demo_animCharacter targetAnimator;

	public bool isInBoat;

	private Transform cameraObject;

	private fx_buoyancy buoyancyObject;

	private float rotSense;

	private Vector2 axisSensitivity = new Vector2(4f, 4f);

	private float followDistance = 5f;

	private float followHeight = 1f;

	private float followLat;

	private float camFOV = 35f;

	private float camRotation;

	private Vector3 camRot;

	private float camHeight = 2f;

	private bool isInWater;

	private bool isInWaterDeep;

	private bool isUnderWater;

	private bool isAtSurface;

	private bool isFloating;

	private bool isFalling;

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

	private float useSpeed;

	private float useSideSpeed;

	private float useVertSpeed;

	private float moveSpeed = 0.05f;

	private float moveForward;

	private float moveSideways;

	private float moveForwardTgt;

	private float moveSidewaysTgt;

	private float moveVert;

	private bool isWalking;

	private bool isRunning;

	private bool isSprinting;

	private Vector3 savePos;

	private float oldMouseRotation;

	private sui_demo_ControllerMaster MC;

	private sui_demo_InputController IC;

	private float xMove;

	private float runButtonTime;

	private bool toggleRun;

	private float gSlope;

	private float useSlope;

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
			targetAnimator = cameraTarget.gameObject.GetComponent<sui_demo_animCharacter>();
		}
		if (buoyancyTarget != null)
		{
			buoyancyObject = buoyancyTarget.GetComponent<fx_buoyancy>();
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
				moveVert = 0f;
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
				if (IC.inputKeyQ)
				{
					moveVert = -1f;
				}
				if (IC.inputKeyE)
				{
					moveVert = 1f;
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
				orbitView = IC.inputKeySPACE;
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
					isFloating = false;
					isAtSurface = false;
					if (waterLevel >= 0.9f && waterLevel < 1.8f)
					{
						isInWaterDeep = true;
					}
					if (waterLevel >= 1.8f)
					{
						isUnderWater = true;
					}
					if (waterLevel >= 1.2f && waterLevel < 1.8f)
					{
						isAtSurface = true;
					}
					if (isInWaterDeep && waterLevel > 2f)
					{
						isFloating = true;
					}
				}
				if (isUnderWater)
				{
					if (buoyancyObject != null)
					{
						buoyancyObject.engageBuoyancy = false;
					}
				}
				else if (buoyancyObject != null)
				{
					buoyancyObject.engageBuoyancy = true;
				}
			}
			if (isControllable)
			{
				if (!orbitView)
				{
					rotSense = rotationSensitivity;
					if (isSprinting)
					{
						rotSense *= 2f;
					}
					float num = 0f;
					float num2 = 0f;
					if (moveForward == 1f && moveSideways == 0f)
					{
						num2 = cameraObject.transform.eulerAngles.y;
						if (cameraTarget.eulerAngles.y - num2 > 180f)
						{
							num = -360f;
						}
						if (cameraTarget.eulerAngles.y - num2 < -180f)
						{
							num = 360f;
						}
						cameraTarget.eulerAngles = new Vector3(cameraTarget.eulerAngles.x, Mathf.Lerp(cameraTarget.eulerAngles.y + num, num2, Time.deltaTime * rotSense), cameraTarget.eulerAngles.z);
					}
					else if (moveForward == -1f && moveSideways == 0f)
					{
						rotSense *= 1f;
						num2 = cameraObject.transform.eulerAngles.y - 180f;
						if (cameraTarget.eulerAngles.y - num2 > 180f)
						{
							num = -360f;
						}
						if (cameraTarget.eulerAngles.y - num2 < -180f)
						{
							num = 360f;
						}
						cameraTarget.eulerAngles = new Vector3(cameraTarget.eulerAngles.x, Mathf.Lerp(cameraTarget.eulerAngles.y + num, num2, Time.deltaTime * rotSense), cameraTarget.eulerAngles.z);
					}
					else if (moveSideways != 0f && moveForward == 0f)
					{
						num2 = cameraObject.transform.eulerAngles.y + 90f * moveSideways;
						if (cameraTarget.eulerAngles.y - num2 > 180f)
						{
							num = -360f;
						}
						if (cameraTarget.eulerAngles.y - num2 < -180f)
						{
							num = 360f;
						}
						cameraTarget.eulerAngles = new Vector3(cameraTarget.eulerAngles.x, Mathf.Lerp(cameraTarget.eulerAngles.y + num, num2, Time.deltaTime * rotSense), cameraTarget.eulerAngles.z);
					}
					else if (moveSideways != 0f && moveForward == 1f)
					{
						rotSense *= 1.4f;
						num2 = cameraObject.transform.eulerAngles.y + 45f * moveSideways;
						if (cameraTarget.eulerAngles.y - num2 > 180f)
						{
							num = -360f;
						}
						if (cameraTarget.eulerAngles.y - num2 < -180f)
						{
							num = 360f;
						}
						cameraTarget.eulerAngles = new Vector3(cameraTarget.eulerAngles.x, Mathf.Lerp(cameraTarget.eulerAngles.y + num, num2, Time.deltaTime * rotSense), cameraTarget.eulerAngles.z);
					}
					else if (moveSideways != 0f && moveForward == -1f)
					{
						rotSense *= 1.4f;
						num2 = cameraObject.transform.eulerAngles.y - 180f - 45f * moveSideways;
						if (cameraTarget.eulerAngles.y - num2 > 180f)
						{
							num = 360f;
						}
						if (cameraTarget.eulerAngles.y - num2 < -180f)
						{
							num = -360f;
						}
						cameraTarget.eulerAngles = new Vector3(cameraTarget.eulerAngles.x, Mathf.Lerp(cameraTarget.eulerAngles.y - num, num2, Time.deltaTime * rotSense), cameraTarget.eulerAngles.z);
					}
					else
					{
						xMove = Mathf.Lerp(xMove, 0f, Time.deltaTime);
					}
					cameraTarget.eulerAngles = new Vector3(0f, cameraTarget.eulerAngles.y, 0f);
				}
				gSlope = 0f;
				useSlope = 0f;
				Vector3 vector = cameraTarget.position + new Vector3(0f, 1f, 0f);
				Vector3 vector2 = cameraTarget.position + new Vector3(0f, -0.25f, 0f);
				if (Physics.Linecast(vector, vector2, out var hitInfo) && hitInfo.transform != cameraTarget.transform)
				{
					if (showDebug)
					{
						Debug.DrawLine(vector, vector2, Color.red);
					}
					if (Physics.Linecast(vector + cameraTarget.forward * 0.25f, vector2 + cameraTarget.forward * 0.25f, out var hitInfo2))
					{
						if (showDebug)
						{
							Debug.DrawLine(vector + cameraTarget.forward * 0.25f, vector2 + cameraTarget.forward * 0.25f, Color.green);
						}
						if (showDebug)
						{
							Debug.DrawLine(hitInfo.point, hitInfo2.point, Color.black);
						}
						float y = hitInfo2.point.y - hitInfo.point.y;
						float x = hitInfo2.point.x - hitInfo.point.x;
						gSlope = Mathf.Atan2(y, x) * 57.29578f;
						useSlope = gSlope;
						if (gSlope < 0f)
						{
							useSlope = 90f + Mathf.Atan2(y, x) * 57.29578f % 90f;
						}
					}
				}
				if (isUnderWater)
				{
					moveSideways = 0f;
				}
				if (moveForward == 0f && moveSideways == 0f)
				{
					isWalking = false;
					isRunning = false;
					isSprinting = false;
				}
				float num3 = 1.7f;
				if (isRunning)
				{
					num3 = 2.5f;
				}
				if (isSprinting)
				{
					num3 = 3.5f;
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
				if (isSprinting)
				{
					moveSpeed = sprintSpeed;
				}
				if (moveForward != 0f && moveSideways != 0f)
				{
					moveSpeed *= 0.5f;
				}
				float num4 = 1f;
				if (isInWater)
				{
					num4 = 0.8f;
				}
				if (isInWaterDeep)
				{
					num4 = 0.6f;
				}
				if (isUnderWater)
				{
					num4 = 0.5f;
				}
				moveSpeed *= num4;
				if (gSlope > 0f && useSlope > 0.25f && useSlope < 90f)
				{
					moveSpeed *= 1.25f - useSlope / 90f;
				}
				useSpeed = Mathf.Lerp(useSpeed, moveSpeed * moveForward, Time.deltaTime * num3);
				useSideSpeed = Mathf.Lerp(useSideSpeed, moveSpeed * moveSideways, Time.deltaTime * num3);
				if (isUnderWater || isInWater)
				{
					useVertSpeed = Mathf.Lerp(useVertSpeed, moveSpeed * moveVert, Time.deltaTime * num3);
				}
				else
				{
					useVertSpeed = 0f;
				}
				if ((bool)cameraTarget.GetComponent<Rigidbody>())
				{
					Vector3 vector3 = cameraTarget.transform.forward * useSpeed * moveForwardTgt;
					if (moveForward != 0f && moveForwardTgt != moveForward)
					{
						moveForwardTgt = moveForward;
					}
					Vector3 vector4 = cameraTarget.transform.forward * useSideSpeed * moveSidewaysTgt;
					if (moveSideways != 0f && moveSidewaysTgt != moveSideways)
					{
						moveSidewaysTgt = moveSideways;
					}
					Vector3 vector5 = new Vector3(0f, useVertSpeed, 0f);
					cameraTarget.GetComponent<Rigidbody>().MovePosition(cameraTarget.GetComponent<Rigidbody>().position + (vector3 + vector4 + vector5));
				}
				float num5 = 2f;
				followDistance -= MouseScrollDistance * 8f;
				followDistance = Mathf.Clamp(followDistance, minZoomAmount, maxZoomAmount);
				followTgtDistance = Mathf.Lerp(followTgtDistance, followDistance, Time.deltaTime * num5);
				camRotation = Mathf.Lerp(oldMouseRotation, MouseRotationDistance * axisSensitivity.x, Time.deltaTime);
				targetRotation.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y + camRotation, targetRotation.eulerAngles.z);
				cameraObject.transform.eulerAngles = new Vector3(targetRotation.eulerAngles.x, targetRotation.eulerAngles.y, cameraObject.transform.eulerAngles.z);
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
					RaycastHit hitInfo3 = default(RaycastHit);
					if (Physics.Linecast(position, cameraObject.transform.position, out hitInfo3) && hitInfo3.transform.gameObject.layer != suimonoModuleObject.layerWaterNum && !(hitInfo3.transform == base.transform) && !(hitInfo3.transform == cameraTarget))
					{
						bool flag = false;
						if (hitInfo3.transform.GetComponent<Collider>() != null && hitInfo3.transform.GetComponent<Collider>().isTrigger)
						{
							flag = true;
						}
						if (!flag)
						{
							cameraObject.transform.position = hitInfo3.point;
						}
					}
				}
				cameraObject.transform.eulerAngles = new Vector3(cameraObject.transform.rotation.eulerAngles.x, cameraObject.transform.rotation.eulerAngles.y, cameraLean);
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
			targetAnimator.isFloating = isFloating;
			targetAnimator.isAtSurface = isAtSurface;
			targetAnimator.isFalling = isFalling;
			targetAnimator.isInBoat = isInBoat;
		}
	}
}
