using UnityEngine;
using UnityEngine.UI;

public class Menu_MultiplayerSave : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private savegameScript save_;

	private int saveID = -1;

	private float timer;

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
		if (!save_)
		{
			save_ = main_.GetComponent<savegameScript>();
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

	public void OnEnable()
	{
		FindScripts();
	}

	public void Init(int saveID_)
	{
		saveID = saveID_;
	}

	private void Update()
	{
		if (uiObjects[1].GetComponent<Button>().interactable)
		{
			timer += Time.deltaTime;
			if (timer > 1f)
			{
				BUTTON_Yes();
			}
		}
		else
		{
			timer = 0f;
		}
	}

	public void BUTTON_Yes()
	{
		save_.Save(saveID);
		base.gameObject.SetActive(value: false);
	}
}
