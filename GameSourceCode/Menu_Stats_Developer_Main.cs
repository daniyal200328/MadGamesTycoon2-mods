using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_Developer_Main : MonoBehaviour
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

	private gameScript nextGame_;

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
			uiObjects[5].GetComponent<Button>().interactable = false;
		}
	}

	public void Init(publisherScript script_)
	{
		FindScripts();
		pS_ = script_;
		nextGame_ = script_.FindAngekuendigtesGame();
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
		guiMain_.DrawStarsColor(uiObjects[2], Mathf.RoundToInt(pS_.stars / 20f), Color.white);
		uiObjects[9].GetComponent<Image>().sprite = guiMain_.flagSprites[pS_.country];
		uiObjects[4].GetComponent<Text>().text = tS_.GetText(685) + ": <b>" + pS_.GetFirmenwertString() + "</b>";
		uiObjects[7].GetComponent<Image>().sprite = genres_.GetPic(pS_.fanGenre);
		uiObjects[7].GetComponent<tooltip>().c = tS_.GetText(437) + ": <b>" + genres_.GetName(pS_.fanGenre) + "</b>";
		uiObjects[3].GetComponent<Text>().text = pS_.GetDateString();
		if ((bool)nextGame_ && !pS_.Geschlossen())
		{
			uiObjects[10].GetComponent<Text>().text = nextGame_.GetNameWithTag();
			uiObjects[11].GetComponent<Text>().text = nextGame_.GetGenreString();
			if (nextGame_.subgenre != -1)
			{
				Text component = uiObjects[11].GetComponent<Text>();
				component.text = component.text + " / " + nextGame_.GetSubGenreString();
			}
		}
		else
		{
			uiObjects[10].GetComponent<Text>().text = "<color=grey>" + tS_.GetText(2057) + "</color>";
			uiObjects[11].GetComponent<Text>().text = "";
		}
		if (pS_.IsTochterfirma())
		{
			if (pS_.TochterfirmaGeschlossen())
			{
				Text component2 = uiObjects[3].GetComponent<Text>();
				component2.text = component2.text + "\n<color=red><b>" + tS_.GetText(1969) + "</b></color>";
			}
		}
		else if (pS_.Geschlossen())
		{
			Text component3 = uiObjects[3].GetComponent<Text>();
			component3.text = component3.text + "\n<color=red><b>" + tS_.GetText(1969) + "</b></color>";
		}
		if (pS_.IsTochterfirma())
		{
			if (!uiObjects[8].activeSelf)
			{
				uiObjects[8].SetActive(value: true);
			}
		}
		else if (uiObjects[8].activeSelf)
		{
			uiObjects[8].SetActive(value: false);
		}
		if (pS_.Geschlossen())
		{
			if (!uiObjects[6].activeSelf)
			{
				uiObjects[6].SetActive(value: true);
			}
		}
		else if (uiObjects[6].activeSelf)
		{
			uiObjects[6].SetActive(value: false);
		}
		if (pS_.IsTochterfirma() || pS_.notForSale)
		{
			uiObjects[5].GetComponent<Button>().interactable = false;
			uiObjects[12].SetActive(value: false);
		}
		else if (!pS_.KaufangebotFree())
		{
			uiObjects[5].GetComponent<Button>().interactable = false;
			uiObjects[12].SetActive(value: true);
		}
		else
		{
			uiObjects[5].GetComponent<Button>().interactable = true;
			uiObjects[12].SetActive(value: false);
		}
		if (mS_.multiplayer)
		{
			if (mS_.settings_tochterfirmaOff)
			{
				uiObjects[5].GetComponent<Button>().interactable = false;
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

	public void BUTTON_FirmaKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[386]);
		guiMain_.uiObjects[386].GetComponent<Menu_W_FirmaKaufen>().Init(pS_);
	}
}
