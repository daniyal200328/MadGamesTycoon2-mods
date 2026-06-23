using UnityEngine;
using UnityEngine.UI;

public class Menu_Support_Fanshop : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

	private games games_;

	public Color[] colors;

	private float updateTimer;

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
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
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

	public void Init(roomScript script_)
	{
		FindScripts();
		rS_ = script_;
		SetData();
	}

	private void SetData()
	{
		if (!rS_)
		{
			return;
		}
		float num = 0f;
		for (int i = 0; i < games_.arrayMyIpScripts.Count; i++)
		{
			if ((bool)games_.arrayMyIpScripts[i])
			{
				for (int j = 0; j < games_.arrayMyIpScripts[i].merchBestellungen.Length; j++)
				{
					num += (float)games_.arrayMyIpScripts[i].merchBestellungen[j];
				}
			}
		}
		num /= 500f;
		guiMain_.DrawStars10_Color(uiObjects[0], Mathf.RoundToInt(num), Color.white);
		if (num <= 10f)
		{
			uiObjects[1].GetComponent<Text>().text = mS_.Round(num, 2).ToString();
		}
		else
		{
			uiObjects[1].GetComponent<Text>().text = ">10.0";
		}
	}

	public void BUTTON_Fanshop()
	{
		sfx_.PlaySound(3, force: false);
		guiMain_.uiObjects[366].SetActive(value: true);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Start()
	{
		sfx_.PlaySound(3, force: true);
		taskFanshop taskFanshop2 = guiMain_.AddTask_Fanshop();
		taskFanshop2.Init(fromSavegame: false);
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskFanshop2.myID;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
