using UnityEngine;
using UnityEngine.UI;

public class Menu_W_PublisherExklusiv : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private publisherScript publisherS_;

	public int laufzeit;

	public int sofortzahlung;

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

	public void Init(publisherScript pS_)
	{
		FindScripts();
		if (!pS_)
		{
			BUTTON_Abbrechen();
			return;
		}
		publisherS_ = pS_;
		laufzeit = pS_.exklusivLaufzeit;
		sofortzahlung = pS_.GetMoneyExklusiv();
		uiObjects[0].GetComponent<Image>().sprite = pS_.GetLogo();
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(sofortzahlung, showDollar: true);
		uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(pS_.GetShareExklusiv(), showDollar: true);
		guiMain_.DrawStars(uiObjects[5], Mathf.RoundToInt(pS_.stars / 20f));
		string text = tS_.GetText(1048);
		text = text.Replace("<NUM>", laufzeit.ToString());
		uiObjects[1].GetComponent<Text>().text = text;
		text = tS_.GetText(1049);
		text = text.Replace("<NAME>", "<color=blue>" + pS_.GetName() + "</color>");
		uiObjects[3].GetComponent<Text>().text = text;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if ((bool)publisherS_)
		{
			mS_.Earn(sofortzahlung, 1);
			mS_.exklusivVertrag_ID = publisherS_.myID;
			mS_.exklusivVertrag_laufzeit = laufzeit;
			if ((bool)mS_.achScript_)
			{
				mS_.achScript_.SetAchivement(42);
			}
		}
		guiMain_.uiObjects[200].GetComponent<Menu_PublisherExklusiv>().BUTTON_Close();
		BUTTON_Abbrechen();
	}
}
