using UnityEngine;
using UnityEngine.UI;

public class Item_Stats_GenreBeliebtheit : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GUI_Main guiMain_;

	public textScript tS_;

	public void Init(string text_, float prozent, Sprite pic)
	{
		uiObjects[0].GetComponent<Text>().text = text_;
		uiObjects[1].GetComponent<Text>().text = Mathf.RoundToInt(prozent) + "%";
		uiObjects[2].GetComponent<Image>().sprite = pic;
		uiObjects[3].GetComponent<Image>().fillAmount = prozent * 0.01f;
		if (prozent >= 100f)
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1381);
		}
		if (prozent <= 20f)
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1382);
		}
		if (prozent <= 50f)
		{
			uiObjects[3].GetComponent<Image>().color = guiMain_.colorsBalken[0];
		}
		else if (prozent < 70f)
		{
			uiObjects[3].GetComponent<Image>().color = guiMain_.colorsBalken[1];
		}
		else
		{
			uiObjects[3].GetComponent<Image>().color = guiMain_.colorsBalken[2];
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
