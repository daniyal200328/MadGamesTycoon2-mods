using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_Publisher_Main : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private publisherScript pS_;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
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
		if (pS_.IsTochterfirma() || pS_.notForSale)
		{
			uiObjects[8].GetComponent<Button>().interactable = false;
		}
	}

	public void Init(publisherScript script_)
	{
		FindScripts();
		pS_ = script_;
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
		uiObjects[10].GetComponent<Image>().sprite = guiMain_.flagSprites[pS_.country];
		guiMain_.DrawStarsColor(uiObjects[2], Mathf.RoundToInt(pS_.stars / 20f), Color.white);
		guiMain_.DrawStarsColor(uiObjects[3], Mathf.RoundToInt(pS_.GetRelation() / 20f), Color.white);
		if (!pS_.isPlayer)
		{
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(436) + ": <b>$" + mS_.Round(pS_.share, 1) + "</b>";
		}
		else
		{
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(436) + ": <b>---</b>";
		}
		uiObjects[5].GetComponent<Text>().text = pS_.GetDateString();
		if (pS_.IsTochterfirma())
		{
			if (pS_.TochterfirmaGeschlossen())
			{
				Text component = uiObjects[5].GetComponent<Text>();
				component.text = component.text + "\n<color=red><b>" + tS_.GetText(1969) + "</b></color>";
			}
		}
		else if (pS_.Geschlossen())
		{
			Text component2 = uiObjects[5].GetComponent<Text>();
			component2.text = component2.text + "\n<color=red><b>" + tS_.GetText(1969) + "</b></color>";
		}
		if (pS_.IsTochterfirma())
		{
			if (!uiObjects[11].activeSelf)
			{
				uiObjects[11].SetActive(value: true);
			}
		}
		else if (uiObjects[11].activeSelf)
		{
			uiObjects[11].SetActive(value: false);
		}
		uiObjects[6].GetComponent<Image>().sprite = genres_.GetPic(pS_.fanGenre);
		uiObjects[6].GetComponent<tooltip>().c = tS_.GetText(437) + ": <b>" + genres_.GetName(pS_.fanGenre) + "</b>";
		uiObjects[7].GetComponent<Text>().text = tS_.GetText(685) + ": <b>" + pS_.GetFirmenwertString() + "</b>";
		if (pS_.Geschlossen())
		{
			if (!uiObjects[9].activeSelf)
			{
				uiObjects[9].SetActive(value: true);
			}
		}
		else if (uiObjects[9].activeSelf)
		{
			uiObjects[9].SetActive(value: false);
		}
		if (pS_.IsTochterfirma() || pS_.notForSale)
		{
			uiObjects[8].GetComponent<Button>().interactable = false;
			uiObjects[12].SetActive(value: false);
		}
		else if (!pS_.KaufangebotFree())
		{
			uiObjects[8].GetComponent<Button>().interactable = false;
			uiObjects[12].SetActive(value: true);
		}
		else
		{
			uiObjects[8].GetComponent<Button>().interactable = true;
			uiObjects[12].SetActive(value: false);
		}
		if (mS_.multiplayer)
		{
			if (mS_.settings_tochterfirmaOff)
			{
				uiObjects[8].GetComponent<Button>().interactable = false;
			}
			if (pS_.myID == 4)
			{
				uiObjects[5].GetComponent<Button>().interactable = false;
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Awards()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[144]);
		guiMain_.uiObjects[144].GetComponent<Menu_Stats_Awards>().Init(pS_);
	}

	public void BUTTON_Games()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[360]);
		guiMain_.uiObjects[360].GetComponent<Menu_Stats_Developer_Games>().Init(pS_);
	}

	public void BUTTON_IPs()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[361]);
		guiMain_.uiObjects[361].GetComponent<Menu_Stats_Developer_IPs>().Init(pS_);
	}

	public void BUTTON_Vertrieben()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[374]);
		guiMain_.uiObjects[374].GetComponent<Menu_Stats_Publisher_Vertrieben>().Init(pS_);
	}

	public void BUTTON_FirmaKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[386]);
		guiMain_.uiObjects[386].GetComponent<Menu_W_FirmaKaufen>().Init(pS_);
	}
}
