using UnityEngine;

public class roomOptionsPosition : MonoBehaviour
{
	public settingsScript settings_;

	private RectTransform rect;

	private RectTransform globalRect;

	private void Start()
	{
	}

	private void FindScripts()
	{
		if (!settings_)
		{
			settings_ = GameObject.Find("Main").GetComponent<settingsScript>();
		}
		if (!rect)
		{
			rect = GetComponent<RectTransform>();
		}
		if (!globalRect)
		{
			globalRect = base.transform.parent.GetComponent<RectTransform>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		rect.anchoredPosition = new Vector2(90f, -40f);
	}

	private void Update()
	{
		float x = 187.3f;
		float y = -81f;
		if (globalRect.anchoredPosition.x + globalRect.sizeDelta.x + rect.sizeDelta.x > (float)Screen.width / settings_.uiScale)
		{
			x = -65f;
		}
		if (Mathf.Abs(globalRect.anchoredPosition.y) + globalRect.sizeDelta.y > (float)Screen.height / settings_.uiScale)
		{
			y = 81f;
		}
		rect.anchoredPosition = Vector2.Lerp(rect.anchoredPosition, new Vector2(x, y), 8f * Time.deltaTime);
	}
}
