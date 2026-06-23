using UnityEngine;
using UnityEngine.UI;

public class Item_PersonalGroup : MonoBehaviour
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

	private RectTransform myRect_;

	private int frames;

	private bool hasEnabled;

	private void Start()
	{
	}

	private void Update()
	{
		if (!cS_)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		frames++;
		if (frames >= 3)
		{
			if (!myRect_)
			{
				myRect_ = GetComponent<RectTransform>();
			}
			if (myRect_.position.y >= 0f && myRect_.position.y <= (float)Screen.height)
			{
				EnableObjects();
			}
		}
	}

	public void EnableObjects()
	{
		if (hasEnabled)
		{
			return;
		}
		hasEnabled = true;
		for (int i = 0; i < uiObjects.Length; i++)
		{
			if ((bool)uiObjects[i] && !uiObjects[i].activeSelf)
			{
				uiObjects[i].SetActive(value: true);
			}
		}
	}

	public void SetData(string s, float val)
	{
		if ((bool)cS_)
		{
			uiObjects[0].GetComponent<Text>().text = cS_.myName;
			uiObjects[10].GetComponent<Text>().text = tS_.GetText(137 + cS_.beruf);
			uiObjects[1].GetComponent<Text>().text = s;
			uiObjects[2].GetComponent<Text>().text = mS_.Round(val, 1).ToString();
			uiObjects[3].GetComponent<Image>().fillAmount = val * 0.01f;
			uiObjects[3].GetComponent<Image>().color = GetValColor(val);
			uiObjects[4].GetComponent<Image>().fillAmount = cS_.s_motivation * 0.01f;
			uiObjects[4].GetComponent<Image>().color = GetValColor(cS_.s_motivation);
			uiObjects[5].GetComponent<Text>().text = Mathf.RoundToInt(cS_.s_motivation).ToString();
			uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(cS_.GetGehalt(), showDollar: true);
			guiMain_.CreatePerkIcons(cS_, uiObjects[7].transform);
			if ((bool)cS_.roomS_)
			{
				uiObjects[9].GetComponent<Image>().sprite = rdS_.roomData_SPRITE[cS_.roomS_.typ];
			}
			if (cS_.krank > 0)
			{
				uiObjects[8].SetActive(value: true);
			}
			else
			{
				uiObjects[8].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
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

	public void BUTTON_Click(bool all)
	{
		sfx_.PlaySound(3, force: true);
		Menu_PersonalGroups component = guiMain_.uiObjects[32].GetComponent<Menu_PersonalGroups>();
		if (cS_.group == -1)
		{
			cS_.group = component.uiObjects[6].GetComponent<Dropdown>().value + 1;
			base.gameObject.transform.parent = component.uiObjects[4].transform;
		}
		else
		{
			cS_.group = -1;
			base.gameObject.transform.parent = component.uiObjects[0].transform;
		}
		if (!all)
		{
			component.DROPDOWN_Sort();
		}
	}
}
