using UnityEngine;
using UnityEngine.UI;

public class Menu_CloseGame : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private void Start()
	{
		FindScripts();
	}

	private void OnEnable()
	{
		FindScripts();
		if (mS_.multiplayer)
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(782) + "\n\n<color=red>" + tS_.GetText(2199) + "</color>";
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(782);
		}
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
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		Application.Quit();
	}
}
