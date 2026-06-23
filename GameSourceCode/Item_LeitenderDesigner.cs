using UnityEngine;
using UnityEngine.UI;

public class Item_LeitenderDesigner : MonoBehaviour
{
	public int characterID = -1;

	public characterScript cS_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public roomDataScript rdS_;

	private void Start()
	{
	}

	private void Update()
	{
		if (!cS_)
		{
			Object.Destroy(base.gameObject);
		}
		if (cS_.group != -1)
		{
			uiObjects[0].GetComponent<Text>().text = cS_.GetGroupString("magenta") + " " + cS_.myName;
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = cS_.myName;
		}
		uiObjects[4].GetComponent<Image>().fillAmount = cS_.s_motivation * 0.01f;
		uiObjects[4].GetComponent<Image>().color = GetValColor(cS_.s_motivation);
		uiObjects[5].GetComponent<Text>().text = Mathf.RoundToInt(cS_.s_motivation).ToString();
	}

	public void SetData(string s, float val)
	{
		if ((bool)cS_)
		{
			uiObjects[1].GetComponent<Text>().text = s;
			uiObjects[2].GetComponent<Text>().text = mS_.Round(val, 1).ToString();
			uiObjects[3].GetComponent<Image>().fillAmount = val * 0.01f;
			uiObjects[3].GetComponent<Image>().color = GetValColor(val);
			uiObjects[8].GetComponent<Text>().text = tS_.GetText(137 + cS_.beruf);
			uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(cS_.GetGehalt(), showDollar: true);
			guiMain_.CreatePerkIcons(cS_, uiObjects[7].transform);
			if ((bool)cS_.roomS_)
			{
				uiObjects[9].GetComponent<Image>().sprite = rdS_.roomData_SPRITE[cS_.roomS_.typ];
			}
			if (cS_.krank > 0)
			{
				uiObjects[10].SetActive(value: true);
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
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[28]);
		guiMain_.uiObjects[28].GetComponent<Menu_PersonalView>().Init(cS_);
	}
}
