using UnityEngine;
using UnityEngine.UI;

public class objectTooltip : MonoBehaviour
{
	public float timeToShow = 1f;

	public GameObject tooltipPic;

	public GameObject tooltipText;

	public GameObject tooltipFill;

	private RectTransform rt_tooltipPic;

	private RectTransform rt_tooltipText;

	public Text myText;

	public Image myFill;

	private float timer;

	public bool tooltipEnabled;

	private objectScript objectScript_;

	public settingsScript settings_;

	private void Start()
	{
		settings_ = GameObject.Find("Main").GetComponent<settingsScript>();
		myText = tooltipText.GetComponent<Text>();
		myFill = tooltipFill.GetComponent<Image>();
		rt_tooltipPic = tooltipPic.GetComponent<RectTransform>();
		rt_tooltipText = tooltipText.GetComponent<RectTransform>();
	}

	public void SetActive(objectScript script_)
	{
		if (!script_)
		{
			SetInactive();
			return;
		}
		objectScript_ = script_;
		timer = 0f;
		tooltipEnabled = true;
	}

	public void SetInactive()
	{
		timer = 0f;
		tooltipEnabled = false;
		myText.text = "";
	}

	private void Update()
	{
		if (!tooltipEnabled)
		{
			if (tooltipPic.activeSelf)
			{
				tooltipPic.SetActive(value: false);
			}
		}
		else
		{
			if (!objectScript_)
			{
				SetInactive();
				return;
			}
			timer += Time.deltaTime;
			if (timer < timeToShow)
			{
				if (tooltipPic.activeSelf)
				{
					tooltipPic.SetActive(value: false);
				}
				return;
			}
			if (!tooltipPic.activeSelf)
			{
				tooltipPic.SetActive(value: true);
			}
			float num = 100f / (float)objectScript_.aufladungenMax * (float)objectScript_.aufladungenAkt;
			myText.text = Mathf.RoundToInt(num) + "%";
			myFill.fillAmount = num * 0.01f;
			if (myFill.fillAmount < 0.05f)
			{
				myFill.fillAmount = 0.05f;
			}
		}
		if ((bool)settings_)
		{
			float num2 = Input.mousePosition.x + 15f;
			float num3 = Input.mousePosition.y - 10f;
			num2 /= settings_.uiScale;
			num3 /= settings_.uiScale;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
			if (rt_tooltipPic.sizeDelta.x + num2 > (float)Screen.width / settings_.uiScale)
			{
				num2 = (float)Screen.width / settings_.uiScale - rt_tooltipPic.sizeDelta.x;
			}
			if (num3 < 0f)
			{
				num3 = 0f;
			}
			if (rt_tooltipPic.sizeDelta.y + num3 > (float)Screen.height / settings_.uiScale)
			{
				num3 = (float)Screen.height / settings_.uiScale - rt_tooltipPic.sizeDelta.y;
			}
			rt_tooltipPic.anchoredPosition = new Vector2(num2, num3);
		}
	}
}
