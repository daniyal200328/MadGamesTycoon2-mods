using UnityEngine;

public class sui_demo_trigger : MonoBehaviour
{
	public enum Sui_Demo_TriggerType
	{
		switchtovehicle,
		watersurface
	}

	public bool requireLineOfSight = true;

	public Sui_Demo_TriggerType triggerType;

	public Texture2D showDot;

	public Texture2D showIcon;

	public Texture2D backgroundImage;

	public string label = "";

	public Color labelColor = new Color(0f, 0f, 0f, 1f);

	public Vector2 dotOffset = new Vector2(0.5f, 0.5f);

	public Vector2 labelOffset = new Vector2(0.5f, 0.5f);

	public string actionKey = "f";

	public bool requireReset = true;

	public Transform trackObject;

	public float fadeSpeed;

	public float checkDistance = 200f;

	private sui_demo_ControllerMaster CM;

	private bool isInRange;

	private bool onAction;

	private string useLabel = "";

	private GUISkin style;

	private float fadeTimer;

	private bool isInSight;

	private bool enableAction;

	private Vector3 savedPos = new Vector3(0f, 0f, 0f);

	private void Start()
	{
		CM = GameObject.Find("_CONTROLLER").GetComponent<sui_demo_ControllerMaster>();
	}

	private void FixedUpdate()
	{
		useLabel = label;
		if (Camera.main != null && savedPos != Camera.main.transform.position)
		{
			savedPos = Camera.main.transform.position;
			isInSight = CheckLineOfSight();
		}
		isInRange = false;
		if (Vector3.Distance(base.transform.position, trackObject.transform.position) <= checkDistance * 0.75f)
		{
			isInRange = true;
		}
		enableAction = false;
		if (isInRange && !requireLineOfSight)
		{
			enableAction = true;
		}
		else if (isInRange && requireLineOfSight && isInSight)
		{
			enableAction = true;
		}
		onAction = false;
		if (Input.GetKeyUp(actionKey) && enableAction)
		{
			onAction = true;
		}
		if (onAction)
		{
			useLabel = "";
			if (triggerType == Sui_Demo_TriggerType.switchtovehicle && CM != null)
			{
				if (CM.currentControllerType == sui_demo_ControllerMaster.Sui_Demo_ControllerType.character)
				{
					CM.currentControllerType = sui_demo_ControllerMaster.Sui_Demo_ControllerType.boat;
				}
				else if (CM.currentControllerType == sui_demo_ControllerMaster.Sui_Demo_ControllerType.boat)
				{
					CM.currentControllerType = sui_demo_ControllerMaster.Sui_Demo_ControllerType.character;
				}
			}
		}
		if (enableAction)
		{
			fadeTimer = Mathf.Lerp(fadeTimer, 0.8f, Time.deltaTime * fadeSpeed * 1f);
		}
		else
		{
			fadeTimer = Mathf.Lerp(fadeTimer, 0f, Time.deltaTime * fadeSpeed * 1f);
		}
		if (isInRange)
		{
			if ((bool)GetComponent<Renderer>())
			{
				GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0f, 1f, 0f, 0.1f));
			}
		}
		else if ((bool)GetComponent<Renderer>())
		{
			GetComponent<Renderer>().material.SetColor("_TintColor", new Color(0.5f, 0f, 0f, 0.1f));
		}
	}

	public bool CheckLineOfSight()
	{
		bool result = false;
		if (requireLineOfSight && Camera.main != null)
		{
			float num = 0f;
			Ray ray = new Ray
			{
				origin = Camera.main.transform.position,
				direction = Camera.main.transform.forward
			};
			RaycastHit[] array = Physics.RaycastAll(ray, 1000f);
			for (int i = 0; i < array.Length; i++)
			{
				RaycastHit raycastHit = array[i];
				Collider collider = raycastHit.collider;
				if ((bool)collider && collider == trackObject.GetComponent<Collider>())
				{
					num = raycastHit.distance;
				}
			}
			RaycastHit[] array2 = Physics.RaycastAll(ray, checkDistance + num);
			foreach (RaycastHit raycastHit2 in array2)
			{
				Collider collider2 = raycastHit2.collider;
				if ((bool)collider2 && collider2 == GetComponent<Collider>())
				{
					result = true;
				}
			}
		}
		return result;
	}

	private void OnGUI()
	{
		if (fadeTimer > 0f && useLabel != "")
		{
			int num = useLabel.Length * 6 + 5;
			GUI.color = new Color(0f, 0f, 0f, fadeTimer);
			GUI.Label(new Rect((float)Screen.width * labelOffset.x - (float)num * 0.5f + 8f, (float)Screen.height * labelOffset.y + 21f, num, 20f), useLabel);
			GUI.color = new Color(labelColor.r, labelColor.g, labelColor.b, fadeTimer);
			GUI.Label(new Rect((float)Screen.width * labelOffset.x - (float)num * 0.5f + 7f, (float)Screen.height * labelOffset.y + 20f, num, 20f), useLabel);
			if (showIcon != null)
			{
				GUI.color = new Color(labelColor.r, labelColor.g, labelColor.b, fadeTimer);
				GUI.Label(new Rect((float)Screen.width * labelOffset.x - (float)num * 0.8f + 7f, (float)Screen.height * labelOffset.y + 16f, showIcon.width, showIcon.height), showIcon);
				GUI.color = new Color(0f, 0f, 0f, fadeTimer);
				GUI.Label(new Rect((float)Screen.width * labelOffset.x - (float)num * 0.8f + 16f, (float)Screen.height * labelOffset.y + 20f, 20f, 30f), actionKey.ToUpper());
			}
		}
	}
}
