using UnityEngine;

public class cameraMovementScript : MonoBehaviour
{
	private GameObject cameraLimitA;

	private GameObject cameraLimitB;

	public bool disableMovement;

	public float movementSpeed = 1f;

	public float movementSpeedWithMouse = 0.01f;

	public float rotationSpeed = 1f;

	public float rotationSpeedWithMouse = 0.3f;

	private Vector3 lastMousePosition;

	public float rotation90Grad;

	public GUI_Main guiMain_;

	public settingsScript settings_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!settings_)
		{
			settings_ = GameObject.FindGameObjectWithTag("Main").GetComponent<settingsScript>();
		}
	}

	public void FindCameraLimits()
	{
		cameraLimitA = GameObject.Find("CameraLimitA");
		if ((bool)cameraLimitA)
		{
			cameraLimitB = GameObject.Find("CameraLimitB");
			_ = (bool)cameraLimitB;
		}
	}

	private void Update()
	{
		CameraInput();
	}

	private void InputVideo()
	{
		if (Input.GetKey(KeyCode.O))
		{
			base.transform.Rotate(0f, -10f * Time.smoothDeltaTime, 0f, Space.Self);
		}
		if (Input.GetKey(KeyCode.P))
		{
			base.transform.Rotate(0f, 10f * Time.smoothDeltaTime, 0f, Space.Self);
		}
		if (Input.GetKey(KeyCode.H))
		{
			base.transform.Translate(Vector3.left * Time.smoothDeltaTime * 2f);
		}
		if (Input.GetKey(KeyCode.J))
		{
			base.transform.Translate(-Vector3.left * Time.smoothDeltaTime * 2f);
		}
		if (Input.GetKey(KeyCode.U))
		{
			base.transform.Translate(Vector3.forward * Time.smoothDeltaTime * 2f);
		}
		if (Input.GetKey(KeyCode.N))
		{
			base.transform.Translate(-Vector3.forward * Time.smoothDeltaTime * 2f);
		}
	}

	private void CameraInput()
	{
		if (!settings_ || disableMovement || guiMain_.selectInputField)
		{
			return;
		}
		float num = movementSpeed * base.gameObject.transform.GetChild(0).transform.position.y / 9f;
		if (settings_.keyboard == 0 || settings_.keyboard == 1)
		{
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || (Input.mousePosition.x <= 1f && settings_.scrollScreen))
			{
				base.transform.Translate(Vector3.left * num * Time.smoothDeltaTime);
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (Input.mousePosition.x >= (float)(Screen.width - 1) && settings_.scrollScreen))
			{
				base.transform.Translate(-Vector3.left * num * Time.smoothDeltaTime);
			}
			if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || (Input.mousePosition.y >= (float)(Screen.height - 1) && settings_.scrollScreen))
			{
				base.transform.Translate(Vector3.forward * num * Time.smoothDeltaTime);
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || (Input.mousePosition.y <= 1f && settings_.scrollScreen))
			{
				base.transform.Translate(-Vector3.forward * num * Time.smoothDeltaTime);
			}
		}
		if (settings_.keyboard == 2)
		{
			if (Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.LeftArrow) || (Input.mousePosition.x <= 1f && settings_.scrollScreen))
			{
				base.transform.Translate(Vector3.left * num * Time.smoothDeltaTime);
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (Input.mousePosition.x >= (float)(Screen.width - 1) && settings_.scrollScreen))
			{
				base.transform.Translate(-Vector3.left * num * Time.smoothDeltaTime);
			}
			if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow) || (Input.mousePosition.y >= (float)(Screen.height - 1) && settings_.scrollScreen))
			{
				base.transform.Translate(Vector3.forward * num * Time.smoothDeltaTime);
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || (Input.mousePosition.y <= 1f && settings_.scrollScreen))
			{
				base.transform.Translate(-Vector3.forward * num * Time.smoothDeltaTime);
			}
		}
		if (settings_.keyboard == 3)
		{
			if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || (Input.mousePosition.x <= 1f && settings_.scrollScreen))
			{
				base.transform.Translate(Vector3.left * num * Time.smoothDeltaTime);
			}
			if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || (Input.mousePosition.x >= (float)(Screen.width - 1) && settings_.scrollScreen))
			{
				base.transform.Translate(-Vector3.left * num * Time.smoothDeltaTime);
			}
			if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.UpArrow) || (Input.mousePosition.y >= (float)(Screen.height - 1) && settings_.scrollScreen))
			{
				base.transform.Translate(Vector3.forward * num * Time.smoothDeltaTime);
			}
			if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || (Input.mousePosition.y <= 1f && settings_.scrollScreen))
			{
				base.transform.Translate(-Vector3.forward * num * Time.smoothDeltaTime);
			}
		}
		if (Input.GetMouseButton(1))
		{
			base.transform.Translate(-Vector3.forward * (Input.mousePosition.y - lastMousePosition.y) * movementSpeedWithMouse);
			base.transform.Translate(Vector3.left * (Input.mousePosition.x - lastMousePosition.x) * movementSpeedWithMouse);
		}
		if ((bool)cameraLimitA && (bool)cameraLimitB)
		{
			if (base.gameObject.transform.position.x < cameraLimitA.transform.position.x)
			{
				base.gameObject.transform.position = new Vector3(cameraLimitA.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z);
			}
			if (base.gameObject.transform.position.z < cameraLimitA.transform.position.z)
			{
				base.gameObject.transform.position = new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, cameraLimitA.transform.position.z);
			}
			if (base.gameObject.transform.position.x > cameraLimitB.transform.position.x)
			{
				base.gameObject.transform.position = new Vector3(cameraLimitB.transform.position.x, base.gameObject.transform.position.y, base.gameObject.transform.position.z);
			}
			if (base.gameObject.transform.position.z > cameraLimitB.transform.position.z)
			{
				base.gameObject.transform.position = new Vector3(base.gameObject.transform.position.x, base.gameObject.transform.position.y, cameraLimitB.transform.position.z);
			}
		}
		if (!settings_.camera90GradRotation)
		{
			if (settings_.keyboard == 0)
			{
				if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.E))
				{
					base.transform.Rotate(0f, (0f - rotationSpeed) * Time.smoothDeltaTime, 0f, Space.Self);
				}
				if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q))
				{
					base.transform.Rotate(0f, rotationSpeed * Time.smoothDeltaTime, 0f, Space.Self);
				}
			}
			if (settings_.keyboard == 1)
			{
				if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.E))
				{
					base.transform.Rotate(0f, (0f - rotationSpeed) * Time.smoothDeltaTime, 0f, Space.Self);
				}
				if (Input.GetKey(KeyCode.Y) || Input.GetKey(KeyCode.Q))
				{
					base.transform.Rotate(0f, rotationSpeed * Time.smoothDeltaTime, 0f, Space.Self);
				}
			}
			if (settings_.keyboard == 2)
			{
				if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.E))
				{
					base.transform.Rotate(0f, (0f - rotationSpeed) * Time.smoothDeltaTime, 0f, Space.Self);
				}
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A))
				{
					base.transform.Rotate(0f, rotationSpeed * Time.smoothDeltaTime, 0f, Space.Self);
				}
			}
			if (settings_.keyboard == 3)
			{
				if (Input.GetKey(KeyCode.X) || Input.GetKey(KeyCode.E))
				{
					base.transform.Rotate(0f, (0f - rotationSpeed) * Time.smoothDeltaTime, 0f, Space.Self);
				}
				if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.Q))
				{
					base.transform.Rotate(0f, rotationSpeed * Time.smoothDeltaTime, 0f, Space.Self);
				}
			}
		}
		else
		{
			if (settings_.keyboard == 0)
			{
				if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.E))
				{
					float num2 = base.transform.eulerAngles.y - 90f;
					num2 /= 90f;
					num2 = Mathf.RoundToInt(num2) * 90;
					rotation90Grad = num2;
				}
				if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.Q))
				{
					float num3 = base.transform.eulerAngles.y + 90f;
					num3 /= 90f;
					num3 = Mathf.RoundToInt(num3) * 90;
					rotation90Grad = num3;
				}
			}
			if (settings_.keyboard == 1)
			{
				if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.E))
				{
					float num4 = base.transform.eulerAngles.y - 90f;
					num4 /= 90f;
					num4 = Mathf.RoundToInt(num4) * 90;
					rotation90Grad = num4;
				}
				if (Input.GetKeyDown(KeyCode.Y) || Input.GetKeyDown(KeyCode.Q))
				{
					float num5 = base.transform.eulerAngles.y + 90f;
					num5 /= 90f;
					num5 = Mathf.RoundToInt(num5) * 90;
					rotation90Grad = num5;
				}
			}
			if (settings_.keyboard == 2)
			{
				if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.E))
				{
					float num6 = base.transform.eulerAngles.y - 90f;
					num6 /= 90f;
					num6 = Mathf.RoundToInt(num6) * 90;
					rotation90Grad = num6;
				}
				if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.A))
				{
					float num7 = base.transform.eulerAngles.y + 90f;
					num7 /= 90f;
					num7 = Mathf.RoundToInt(num7) * 90;
					rotation90Grad = num7;
				}
			}
			if (settings_.keyboard == 3)
			{
				if (Input.GetKeyDown(KeyCode.X) || Input.GetKeyDown(KeyCode.E))
				{
					float num8 = base.transform.eulerAngles.y - 90f;
					num8 /= 90f;
					num8 = Mathf.RoundToInt(num8) * 90;
					rotation90Grad = num8;
				}
				if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Q))
				{
					float num9 = base.transform.eulerAngles.y + 90f;
					num9 /= 90f;
					num9 = Mathf.RoundToInt(num9) * 90;
					rotation90Grad = num9;
				}
			}
			base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, Mathf.LerpAngle(base.transform.eulerAngles.y, rotation90Grad, 0.2f), base.transform.eulerAngles.z);
		}
		if (Input.GetMouseButton(2))
		{
			base.transform.Rotate(0f, (Input.mousePosition.x - lastMousePosition.x) * rotationSpeedWithMouse, 0f, Space.Self);
			rotation90Grad = base.transform.eulerAngles.y;
		}
		lastMousePosition = Input.mousePosition;
	}
}
