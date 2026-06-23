using UnityEngine;
using UnityEngine.UI;

public class GUI_Tooltip : MonoBehaviour
{
	public settingsScript settings_;

	public float randInPixel = 8f;

	public float timeToShow = 1f;

	public GameObject tooltipPic;

	public GameObject tooltipText;

	private RectTransform rt_tooltipPic;

	private RectTransform rt_tooltipText;

	public Text myText;

	private float timer;

	public bool tooltipEnabled;

	private void Start()
	{
		settings_ = GameObject.Find("Main").GetComponent<settingsScript>();
		myText = tooltipText.GetComponent<Text>();
		rt_tooltipPic = tooltipPic.GetComponent<RectTransform>();
		rt_tooltipText = tooltipText.GetComponent<RectTransform>();
	}

	public void SetActive(string s)
	{
		if (s != null)
		{
			if (s.Length > 0)
			{
				s = s.Replace("<br>", "\n");
				s = s.Replace("<c>", "</color>");
				s = s.Replace("<red>", "<color=red>");
				s = s.Replace("<blue>", "<color=blue>");
				s = s.Replace("<green>", "<color=green>");
				myText.text = s;
				timer = 0f;
				tooltipEnabled = true;
			}
			else
			{
				SetInactive();
			}
		}
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
		}
		Vector2 sizeDelta = rt_tooltipText.sizeDelta;
		sizeDelta.x += randInPixel;
		sizeDelta.y += randInPixel;
		rt_tooltipPic.sizeDelta = sizeDelta;
		float x = Input.mousePosition.x;
		float y = Input.mousePosition.y;
		x /= settings_.uiScale;
		y /= settings_.uiScale;
		if (x < 0f)
		{
			x = 0f;
		}
		if (rt_tooltipPic.sizeDelta.x + x > (float)Screen.width / settings_.uiScale)
		{
			x = (float)Screen.width / settings_.uiScale - rt_tooltipPic.sizeDelta.x;
		}
		if (y < 0f)
		{
			y = 0f;
		}
		if (rt_tooltipPic.sizeDelta.y + y > (float)Screen.height / settings_.uiScale)
		{
			y = (float)Screen.height / settings_.uiScale - rt_tooltipPic.sizeDelta.y;
		}
		rt_tooltipPic.anchoredPosition = new Vector2(x, y);
	}
}
