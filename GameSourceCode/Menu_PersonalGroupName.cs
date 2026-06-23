using UnityEngine;
using UnityEngine.UI;

public class Menu_PersonalGroupName : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private int group = -1;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
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
			if (!cmS_)
			{
				cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
			}
		}
	}

	private void OnEnable()
	{
		FindScripts();
		cmS_.disableMovement = true;
	}

	private void OnDisable()
	{
		FindScripts();
		if ((bool)cmS_)
		{
			cmS_.disableMovement = false;
		}
	}

	public void Init(int group_)
	{
		FindScripts();
		group = group_;
		if (mS_.personal_group_names[group].Length > 0)
		{
			uiObjects[0].GetComponent<InputField>().text = mS_.personal_group_names[group];
		}
		else
		{
			uiObjects[0].GetComponent<InputField>().text = "";
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		mS_.personal_group_names[group] = uiObjects[0].GetComponent<InputField>().text;
		guiMain_.uiObjects[32].GetComponent<Menu_PersonalGroups>().InitDropdowns();
		guiMain_.uiObjects[32].GetComponent<Menu_PersonalGroups>().DROPDOWN_Group();
		BUTTON_Abbrechen();
	}
}
