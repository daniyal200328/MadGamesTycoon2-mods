using UnityEngine;
using UnityEngine.UI;

public class Item_Arbeitsmarkt : MonoBehaviour
{
	public int characterID = -1;

	public charArbeitsmarkt charAM_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	private void Start()
	{
	}

	private void Update()
	{
		if (!charAM_)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void SetData(string s, float val)
	{
		if ((bool)charAM_)
		{
			uiObjects[0].GetComponent<Text>().text = charAM_.myName;
			uiObjects[9].GetComponent<Text>().text = tS_.GetText(137 + charAM_.beruf);
			uiObjects[1].GetComponent<Text>().text = s;
			uiObjects[2].GetComponent<Text>().text = mS_.Round(val, 1).ToString();
			uiObjects[3].GetComponent<Image>().fillAmount = val * 0.01f;
			uiObjects[3].GetComponent<Image>().color = GetValColor(val);
			uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(charAM_.GetGehalt(), showDollar: true);
			guiMain_.CreatePerkIconsArbeitsmarkt(charAM_, uiObjects[7].transform);
			if (mS_.multiplayer && uiObjects[4].activeSelf)
			{
				uiObjects[4].SetActive(value: false);
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	private Color GetValColor(float val)
	{
		if (val < 30f)
		{
			return guiMain_.colorsBalken[0];
		}
		if (val >= 30f && val < 70f)
		{
			return guiMain_.colorsBalken[1];
		}
		if (val >= 70f)
		{
			return guiMain_.colorsBalken[2];
		}
		return guiMain_.colorsBalken[0];
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[31]);
		guiMain_.uiObjects[31].GetComponent<Menu_PersonalViewArbeitsmarkt>().Init(charAM_);
	}

	public void BUTTON_Remove()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)charAM_)
		{
			Object.Destroy(charAM_.gameObject);
			Object.Destroy(base.gameObject);
		}
	}
}
