using UnityEngine;
using UnityEngine.UI;

public class Menu_W_PublisherKuendigen_MB : MonoBehaviour
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

	public void Init(publisherScript script_)
	{
		pS_ = script_;
		if (!pS_)
		{
			BUTTON_Abbrechen();
			return;
		}
		FindScripts();
		string text = tS_.GetText(1915);
		text = text.Replace("<NAME>", "<color=blue>" + pS_.GetName() + "</color>");
		uiObjects[0].GetComponent<Text>().text = text;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		sfx_.PlaySound(3, force: true);
		Menu_W_PublisherExklusivKuendigen component = guiMain_.uiObjects[382].GetComponent<Menu_W_PublisherExklusivKuendigen>();
		guiMain_.uiObjects[382].SetActive(value: false);
		mS_.Pay(component.GetStrafzahlung(), 14);
		pS_.relation = 0f;
		mS_.RemovePublisherExklusivVertrag();
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
