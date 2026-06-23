using UnityEngine;
using UnityEngine.UI;

public class Menu_RandomEventGlobal : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(int forceEvent)
	{
		Debug.Log("RandomEventGlobal");
		FindScripts();
		int num = 0;
		num = ((forceEvent != -1) ? forceEvent : Random.Range(0, 14));
		switch (num)
		{
		case 0:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1080);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(55, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 1:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1081);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(56, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 2:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1082);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(55, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 3:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1083);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(55, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 4:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1084);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(55, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 5:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1085);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(56, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 6:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1086);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(55, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 7:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1087);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(55, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 8:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1316);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(55, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 9:
			if (unlock_.unlock[21])
			{
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1088);
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
				sfx_.PlaySound(55, force: true);
				if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
				{
					mS_.SetGlobalEvent(num);
				}
			}
			else
			{
				Init(4);
			}
			break;
		case 10:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1377);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(55, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 11:
			if (unlock_.unlock[21])
			{
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1384);
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
				sfx_.PlaySound(55, force: true);
				if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
				{
					mS_.SetGlobalEvent(num);
				}
			}
			else
			{
				Init(3);
			}
			break;
		case 12:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1089);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(55, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		case 13:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1889);
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.iconGlobalEvents[num];
			sfx_.PlaySound(55, force: true);
			if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
			{
				mS_.SetGlobalEvent(num);
			}
			break;
		}
		if ((bool)mS_.settings_ && mS_.settings_.hideEvents)
		{
			BUTTON_Abbrechen();
		}
	}

	public void History()
	{
		FindScripts();
		if (mS_.year == 1984 && mS_.month == 2)
		{
			Init(0);
			return;
		}
		if (mS_.year == 1990 && mS_.month == 3)
		{
			Init(1);
			return;
		}
		if (mS_.year == 1995 && mS_.month == 4)
		{
			Init(10);
			return;
		}
		if (mS_.year == 2000 && mS_.month == 5)
		{
			Init(5);
			return;
		}
		if (mS_.year == 2004 && mS_.month == 2)
		{
			Init(11);
			return;
		}
		if (mS_.year == 2005 && mS_.month == 9)
		{
			Init(9);
			return;
		}
		if (mS_.year == 2007 && mS_.month == 4)
		{
			Init(4);
			return;
		}
		if (mS_.year == 2010 && mS_.month == 7)
		{
			Init(8);
			return;
		}
		if (mS_.year == 2013 && mS_.month == 8)
		{
			Init(2);
			return;
		}
		if (mS_.year == 2017 && mS_.month == 2)
		{
			Init(12);
			return;
		}
		if (mS_.year == 2019 && mS_.month == 4)
		{
			Init(7);
			return;
		}
		if (mS_.year == 2021 && mS_.month == 5)
		{
			Init(13);
			return;
		}
		if (mS_.year == 2023 && mS_.month == 3)
		{
			Init(6);
			return;
		}
		if (mS_.year == 2024 && mS_.month == 9)
		{
			Init(3);
			return;
		}
		if (mS_.year > 2026)
		{
			mS_.settings_randomEvents = 0;
			return;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
