using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_PersonalEntlassen : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private List<characterScript> listPersonal = new List<characterScript>();

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
	}

	public void BUTTON_Abbrechen()
	{
		listPersonal.Clear();
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 0; i < listPersonal.Count; i++)
		{
			if ((bool)listPersonal[i])
			{
				listPersonal[i].Entlassen(eventMitarbeiterMotivation: true);
			}
		}
		BUTTON_Abbrechen();
	}

	public void AddCharacter(characterScript cS_)
	{
		FindScripts();
		listPersonal.Add(cS_);
		string text = "";
		for (int i = 0; i < listPersonal.Count; i++)
		{
			if ((bool)listPersonal[i])
			{
				text += listPersonal[i].myName;
				if (i + 1 < listPersonal.Count)
				{
					text += ", ";
				}
			}
		}
		string text2 = tS_.GetText(186);
		text2 = text2.Replace("<NAME>", "<color=blue>" + text + "</color>");
		uiObjects[0].GetComponent<Text>().text = text2;
	}
}
