using UnityEngine;
using UnityEngine.UI;

public class GUI_Tabs : MonoBehaviour
{
	public int activTab;

	public int tabHeight = 50;

	private float tabInactivHeight = 50f;

	public GameObject[] tabs;

	public GameObject[] menus;

	public Color colorOn;

	public Color colorOff;

	private void Start()
	{
		tabInactivHeight = tabs[0].GetComponent<RectTransform>().sizeDelta.y;
		for (int i = 0; i < tabs.Length; i++)
		{
			tabs[i].GetComponent<Image>().color = colorOff;
			RectTransform component = tabs[i].GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, tabInactivHeight);
		}
	}

	public void Click_Tab(int t)
	{
		activTab = t;
		for (int i = 0; i < tabs.Length; i++)
		{
			tabs[i].GetComponent<Image>().color = colorOff;
			RectTransform component = tabs[i].GetComponent<RectTransform>();
			component.sizeDelta = new Vector2(component.sizeDelta.x, tabInactivHeight);
		}
		tabs[t].GetComponent<Image>().color = colorOn;
		RectTransform component2 = tabs[activTab].GetComponent<RectTransform>();
		component2.sizeDelta = new Vector2(component2.sizeDelta.x, tabHeight);
		if (menus.Length != 0)
		{
			for (int j = 0; j < menus.Length; j++)
			{
				menus[j].SetActive(value: false);
			}
			menus[activTab].SetActive(value: true);
		}
	}
}
