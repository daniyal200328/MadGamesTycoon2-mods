using UnityEngine;
using UnityEngine.UI;

public class Menu_Immobilien : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private unlockScript unlock_;

	private roomScript rS_;

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
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
	}

	public void Init(roomScript script_)
	{
		FindScripts();
		rS_ = script_;
		if (!rS_)
		{
			BUTTON_Close();
			return;
		}
		int count = rS_.listGameObjects.Count;
		string text = tS_.GetText(1065);
		text = text.Replace("<NUM1>", count.ToString());
		text = text.Replace("<NUM2>", mS_.GetMoney(GetPreis(), showDollar: true));
		uiObjects[0].GetComponent<Text>().text = text;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.CloseMenu();
	}

	public void BUTTON_OK()
	{
		int preis = GetPreis();
		if (mS_.NotEnoughMoney(preis))
		{
			guiMain_.ShowNoMoney();
			return;
		}
		mS_.Pay(preis, 19);
		mS_.buildings[rS_.myID] = true;
		rS_.Demolish();
		mS_.SendSystemMessage("<IMMOBILIE>");
		BUTTON_Close();
	}

	private int GetPreis()
	{
		int count = rS_.listGameObjects.Count;
		int num = count * ((mS_.difficulty + 1) * 1600);
		if (count <= 100)
		{
			num = num;
		}
		if (count > 100 && count <= 200)
		{
			num *= 2;
		}
		if (count > 200 && count <= 300)
		{
			num *= 5;
		}
		if (count > 300 && count <= 400)
		{
			num *= 10;
		}
		if (count > 400 && count <= 500)
		{
			num *= 15;
		}
		if (count > 500 && count <= 600)
		{
			num *= 20;
		}
		if (count > 600)
		{
			num *= 30;
		}
		if (mS_.globalEvent == 6)
		{
			num *= 2;
		}
		if (mS_.globalEvent == 8)
		{
			num /= 2;
		}
		return num;
	}
}
