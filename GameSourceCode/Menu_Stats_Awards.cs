using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_Awards : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private publisherScript pS_;

	private float updateTimer;

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

	private void Update()
	{
		MultiplayerUpdate();
	}

	private void MultiplayerUpdate()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 5f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void Init(publisherScript script_)
	{
		pS_ = script_;
		FindScripts();
		SetData();
	}

	private void SetData()
	{
		if ((bool)pS_)
		{
			uiObjects[0].GetComponent<Text>().text = pS_.awards[4] + "x";
			uiObjects[1].GetComponent<Text>().text = pS_.awards[2] + "x";
			uiObjects[2].GetComponent<Text>().text = pS_.awards[3] + "x";
			uiObjects[3].GetComponent<Text>().text = pS_.awards[0] + "x";
			uiObjects[4].GetComponent<Text>().text = pS_.awards[1] + "x";
			uiObjects[5].GetComponent<Text>().text = pS_.awards[8] + "x";
			uiObjects[6].GetComponent<Text>().text = pS_.awards[7] + "x";
			uiObjects[7].GetComponent<Text>().text = pS_.awards[6] + "x";
			uiObjects[8].GetComponent<Text>().text = pS_.awards[5] + "x";
			uiObjects[9].GetComponent<Text>().text = pS_.awards[9] + "x";
			uiObjects[10].GetComponent<Text>().text = pS_.awards[10] + "x";
			uiObjects[11].GetComponent<Text>().text = pS_.awards[11] + "x";
		}
	}
}
