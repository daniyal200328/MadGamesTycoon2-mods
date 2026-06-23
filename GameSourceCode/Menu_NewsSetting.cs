using UnityEngine;
using UnityEngine.UI;

public class Menu_NewsSetting : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject allToggle;

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
		if ((bool)main_)
		{
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
	}

	private void OnEnable()
	{
		FindScripts();
		Init();
	}

	private void Init()
	{
		uiObjects[0].GetComponent<Toggle>().isOn = mS_.newsSetting[0];
		uiObjects[1].GetComponent<Toggle>().isOn = mS_.newsSetting[1];
		uiObjects[2].GetComponent<Toggle>().isOn = mS_.newsSetting[2];
		uiObjects[3].GetComponent<Toggle>().isOn = mS_.newsSetting[3];
		uiObjects[4].GetComponent<Toggle>().isOn = mS_.newsSetting[4];
		uiObjects[5].GetComponent<Toggle>().isOn = mS_.newsSetting[5];
		uiObjects[6].GetComponent<Toggle>().isOn = mS_.newsSetting[6];
		uiObjects[7].GetComponent<Toggle>().isOn = mS_.newsSetting[7];
		uiObjects[8].GetComponent<Toggle>().isOn = mS_.newsSetting[8];
		uiObjects[9].GetComponent<Toggle>().isOn = mS_.newsSetting[9];
		uiObjects[10].GetComponent<Toggle>().isOn = mS_.newsSetting[10];
		uiObjects[11].GetComponent<Toggle>().isOn = mS_.newsSetting[11];
		uiObjects[12].GetComponent<Toggle>().isOn = mS_.newsSetting[12];
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_OK()
	{
		mS_.newsSetting[0] = uiObjects[0].GetComponent<Toggle>().isOn;
		mS_.newsSetting[1] = uiObjects[1].GetComponent<Toggle>().isOn;
		mS_.newsSetting[2] = uiObjects[2].GetComponent<Toggle>().isOn;
		mS_.newsSetting[3] = uiObjects[3].GetComponent<Toggle>().isOn;
		mS_.newsSetting[4] = uiObjects[4].GetComponent<Toggle>().isOn;
		mS_.newsSetting[5] = uiObjects[5].GetComponent<Toggle>().isOn;
		mS_.newsSetting[6] = uiObjects[6].GetComponent<Toggle>().isOn;
		mS_.newsSetting[7] = uiObjects[7].GetComponent<Toggle>().isOn;
		mS_.newsSetting[8] = uiObjects[8].GetComponent<Toggle>().isOn;
		mS_.newsSetting[9] = uiObjects[9].GetComponent<Toggle>().isOn;
		mS_.newsSetting[10] = uiObjects[10].GetComponent<Toggle>().isOn;
		mS_.newsSetting[11] = uiObjects[11].GetComponent<Toggle>().isOn;
		mS_.newsSetting[12] = uiObjects[12].GetComponent<Toggle>().isOn;
		sfx_.PlaySound(3, force: true);
		BUTTON_Abbrechen();
	}

	public void TOGGLE_All()
	{
		for (int i = 0; i < uiObjects.Length; i++)
		{
			mS_.newsSetting[i] = allToggle.GetComponent<Toggle>().isOn;
			uiObjects[i].GetComponent<Toggle>().isOn = allToggle.GetComponent<Toggle>().isOn;
		}
	}
}
