using UnityEngine;
using UnityEngine.UI;

public class Menu_W_FirmaAufwerten : MonoBehaviour
{
	public GameObject[] uiObjects;

	private publisherScript pS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	public long[] costs;

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

	public void Init(publisherScript script_)
	{
		FindScripts();
		pS_ = script_;
		if ((bool)pS_)
		{
			int starsAmount = pS_.GetStarsAmount();
			string text = tS_.GetText(1938);
			text = text.Replace("<NAME>", "<color=blue>" + pS_.GetName() + "</color>");
			text = text.Replace("<NUM>", "<color=blue>" + mS_.GetMoney(costs[starsAmount], showDollar: true) + "</color>");
			uiObjects[0].GetComponent<Text>().text = text;
			uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
			guiMain_.DrawStarsColor(uiObjects[2], starsAmount, Color.white);
		}
		else
		{
			BUTTON_Abbrechen();
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if ((bool)pS_)
		{
			int starsAmount = pS_.GetStarsAmount();
			if (mS_.money < costs[starsAmount])
			{
				guiMain_.ShowNoMoney();
				return;
			}
			mS_.Pay(costs[starsAmount], 29);
			pS_.stars = starsAmount * 20 + 20;
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_Publisher(pS_);
				}
				else
				{
					mS_.mpCalls_.CLIENT_Send_Publisher(pS_);
				}
			}
			if (guiMain_.uiObjects[387].activeSelf)
			{
				guiMain_.uiObjects[387].GetComponent<Menu_Stats_Tochterfirma_Main>().UpdateData();
			}
		}
		BUTTON_Abbrechen();
	}
}
