using UnityEngine;
using UnityEngine.UI;

public class Menu_W_PublisherExklusivKuendigen : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void OnEnable()
	{
		Init();
	}

	public void Init()
	{
		FindScripts();
		pS_ = null;
		pS_ = mS_.GetExklusivPublisher();
		if (!pS_)
		{
			BUTTON_Abbrechen();
			return;
		}
		uiObjects[0].GetComponent<Image>().sprite = pS_.GetLogo();
		string text = tS_.GetText(1050);
		text = text.Replace("<NUM>", "<color=blue>" + mS_.exklusivVertrag_laufzeit + "</color>");
		text = text.Replace("<NAME>", "<color=blue>" + pS_.GetName() + "</color>");
		uiObjects[3].GetComponent<Text>().text = text;
		guiMain_.DrawStars(uiObjects[2], Mathf.RoundToInt(pS_.stars / 20f));
		long strafzahlung = GetStrafzahlung();
		text = tS_.GetText(1914);
		text = text.Replace("<NUM>", "<color=blue>" + mS_.GetMoney(strafzahlung, showDollar: true) + "</color>");
		uiObjects[1].GetComponent<Text>().text = text;
	}

	public long GetStrafzahlung()
	{
		if ((bool)pS_)
		{
			return Mathf.RoundToInt((float)((mS_.year - 1975) * 250000) * (pS_.stars / 100f) / 120f * (float)mS_.exklusivVertrag_laufzeit * 2.5f + (float)(mS_.exklusivVertrag_laufzeit * (30000 * (mS_.difficulty + 1))));
		}
		return 0L;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Kuendigen()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)pS_)
		{
			guiMain_.uiObjects[383].SetActive(value: true);
			guiMain_.uiObjects[383].GetComponent<Menu_W_PublisherKuendigen_MB>().Init(pS_);
		}
		else
		{
			BUTTON_Abbrechen();
		}
	}
}
