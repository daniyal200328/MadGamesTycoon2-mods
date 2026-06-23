using UnityEngine;
using UnityEngine.UI;

public class Menu_Support_Anrufe : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

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
		uiObjects[4].GetComponent<Toggle>().isOn = mS_.support_kostenpflichtig;
		SetData();
	}

	private void SetData()
	{
		float anrufe100Prozent = mS_.GetAnrufe100Prozent();
		uiObjects[0].GetComponent<Text>().text = Mathf.RoundToInt(anrufe100Prozent) + "%";
		uiObjects[1].GetComponent<Image>().fillAmount = anrufe100Prozent * 0.01f;
		uiObjects[1].GetComponent<Image>().color = colors[mS_.GetAnrufeAmount()];
		uiObjects[2].GetComponent<Text>().text = tS_.GetText(754 + mS_.GetAnrufeAmount());
		string text = tS_.GetText(758);
		text = text.Replace("<NUM>", mS_.GetMoney(mS_.anrufe, showDollar: false));
		uiObjects[3].GetComponent<Text>().text = text;
		if ((bool)rS_)
		{
			uiObjects[5].SetActive(value: true);
			uiObjects[6].SetActive(value: false);
		}
		else
		{
			uiObjects[5].SetActive(value: false);
			uiObjects[6].SetActive(value: true);
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		mS_.support_kostenpflichtig = uiObjects[4].GetComponent<Toggle>().isOn;
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Start()
	{
		if ((bool)rS_)
		{
			sfx_.PlaySound(3, force: true);
			mS_.support_kostenpflichtig = uiObjects[4].GetComponent<Toggle>().isOn;
			taskSupport taskSupport2 = guiMain_.AddTask_Support();
			taskSupport2.Init(fromSavegame: false);
			GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
			if ((bool)gameObject)
			{
				gameObject.GetComponent<roomScript>().taskID = taskSupport2.myID;
			}
			guiMain_.CloseMenu();
			base.gameObject.SetActive(value: false);
		}
	}
}
