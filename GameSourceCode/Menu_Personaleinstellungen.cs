using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Personaleinstellungen : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void OnEnable()
	{
		InitDropdowns();
		Init();
	}

	public void Init()
	{
		FindScripts();
		uiObjects[0].GetComponent<Dropdown>().value = mS_.personal_pausen;
		uiObjects[1].GetComponent<Dropdown>().value = mS_.personal_druck;
		uiObjects[4].GetComponent<Slider>().value = mS_.personal_motivation;
		uiObjects[5].GetComponent<Slider>().value = mS_.personal_crunch;
		uiObjects[6].GetComponent<Toggle>().isOn = mS_.personal_dontLeaveBuilding;
		uiObjects[7].GetComponent<Toggle>().isOn = mS_.personal_RobotDontLeaveBuilding;
		uiObjects[8].GetComponent<Toggle>().isOn = mS_.personal_ki;
		uiObjects[9].GetComponent<Toggle>().isOn = mS_.personal_autoGehaltsverhandlung;
		SLIDER_Motivation();
		SLIDER_Crunch();
	}

	public void InitDropdowns()
	{
		FindScripts();
		List<string> list = new List<string>();
		list.Add(tS_.GetText(1002));
		list.Add(tS_.GetText(1003));
		list.Add(tS_.GetText(1004));
		uiObjects[0].GetComponent<Dropdown>().ClearOptions();
		uiObjects[0].GetComponent<Dropdown>().AddOptions(list);
		List<string> list2 = new List<string>();
		list2.Add(tS_.GetText(1005));
		list2.Add(tS_.GetText(1006));
		list2.Add(tS_.GetText(1007));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list2);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
		mS_.personal_pausen = uiObjects[0].GetComponent<Dropdown>().value;
		mS_.personal_druck = uiObjects[1].GetComponent<Dropdown>().value;
		mS_.personal_motivation = Mathf.RoundToInt(uiObjects[4].GetComponent<Slider>().value);
		mS_.personal_crunch = Mathf.RoundToInt(uiObjects[5].GetComponent<Slider>().value);
		mS_.personal_dontLeaveBuilding = uiObjects[6].GetComponent<Toggle>().isOn;
		mS_.personal_RobotDontLeaveBuilding = uiObjects[7].GetComponent<Toggle>().isOn;
		mS_.personal_ki = uiObjects[8].GetComponent<Toggle>().isOn;
		mS_.personal_autoGehaltsverhandlung = uiObjects[9].GetComponent<Toggle>().isOn;
	}

	public void SLIDER_Motivation()
	{
		uiObjects[2].GetComponent<Text>().text = uiObjects[4].GetComponent<Slider>().value + "%";
	}

	public void SLIDER_Crunch()
	{
		uiObjects[3].GetComponent<Text>().text = uiObjects[5].GetComponent<Slider>().value + "%";
		if (Mathf.RoundToInt(uiObjects[5].GetComponent<Slider>().value) >= 100)
		{
			uiObjects[3].GetComponent<Text>().text = tS_.GetText(1626);
		}
	}
}
