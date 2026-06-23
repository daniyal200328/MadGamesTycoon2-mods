using UnityEngine;
using UnityEngine.UI;

public class Menu_W_Aufgabe_Abbrechen : MonoBehaviour
{
	public GameObject[] uiObjects;

	private platformScript pS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private games games_;

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
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
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

	public void Init(roomScript script_)
	{
		FindScripts();
		if (!script_)
		{
			return;
		}
		rS_ = script_;
		int num = 0;
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			if ((bool)gameObject.GetComponent<taskEngine>())
			{
				num = gameObject.GetComponent<taskEngine>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskMarketing>())
			{
				num = gameObject.GetComponent<taskMarketing>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskMarketingSpezial>())
			{
				num = gameObject.GetComponent<taskMarketingSpezial>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskForschung>())
			{
				num = gameObject.GetComponent<taskForschung>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskForschungWait>())
			{
				num = 0;
			}
			if ((bool)gameObject.GetComponent<taskAutoForschung>())
			{
				num = 0;
			}
			if ((bool)gameObject.GetComponent<taskGame>())
			{
				taskGame component = gameObject.GetComponent<taskGame>();
				num = gameObject.GetComponent<taskGame>().GetRueckgeld();
				if ((bool)component.gS_ && component.gS_.portID == -1)
				{
					for (int i = 0; i < component.gS_.portExist.Length; i++)
					{
						if (!component.gS_.portExist[i])
						{
							continue;
						}
						for (int j = 0; j < games_.arrayGamesScripts.Length; j++)
						{
							if (games_.arrayGamesScripts[j].inDevelopment && games_.arrayGamesScripts[j].portID == component.gS_.myID)
							{
								BUTTON_Abbrechen();
								guiMain_.MessageBox(tS_.GetText(2076), closeMenu: true);
								return;
							}
						}
					}
				}
			}
			if ((bool)gameObject.GetComponent<taskTraining>())
			{
				num = gameObject.GetComponent<taskTraining>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskContractWork>())
			{
				num = -gameObject.GetComponent<taskContractWork>().GetStrafe();
			}
			if ((bool)gameObject.GetComponent<taskContractWait>())
			{
				num = 0;
			}
			if ((bool)gameObject.GetComponent<taskUpdate>())
			{
				num = gameObject.GetComponent<taskUpdate>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskF2PUpdate>())
			{
				num = gameObject.GetComponent<taskF2PUpdate>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskFankampagne>())
			{
				num = gameObject.GetComponent<taskFankampagne>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskMitarbeitersuche>())
			{
				num = gameObject.GetComponent<taskMitarbeitersuche>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskBugfixing>())
			{
				num = 0;
			}
			if ((bool)gameObject.GetComponent<taskSupport>())
			{
				num = 0;
			}
			if ((bool)gameObject.GetComponent<taskFanshop>())
			{
				num = 0;
			}
			if ((bool)gameObject.GetComponent<taskGameplayVerbessern>())
			{
				num = gameObject.GetComponent<taskGameplayVerbessern>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskGrafikVerbessern>())
			{
				num = gameObject.GetComponent<taskGrafikVerbessern>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskSoundVerbessern>())
			{
				num = gameObject.GetComponent<taskSoundVerbessern>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskAnimationVerbessern>())
			{
				num = gameObject.GetComponent<taskAnimationVerbessern>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskMarktforschung>())
			{
				num = 0;
			}
			if ((bool)gameObject.GetComponent<taskPolishing>())
			{
				num = 0;
			}
			if ((bool)gameObject.GetComponent<taskKonsole>())
			{
				num = gameObject.GetComponent<taskKonsole>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskKonsoleReduceCosts>())
			{
				num = gameObject.GetComponent<taskKonsoleReduceCosts>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskKonsoleHaltbarkeit>())
			{
				num = gameObject.GetComponent<taskKonsoleHaltbarkeit>().GetRueckgeld();
			}
			if ((bool)gameObject.GetComponent<taskArcadeProduction>())
			{
				num = 0;
			}
			if ((bool)gameObject.GetComponent<taskProduction>())
			{
				num = 0;
			}
			if ((bool)gameObject.GetComponent<taskSpielbericht>())
			{
				num = 0;
			}
		}
		uiObjects[0].GetComponent<Text>().text = "";
		if (num > 0)
		{
			string text = tS_.GetText(569);
			text = text.Replace("<NUM>", mS_.GetMoney(num, showDollar: true));
			uiObjects[0].GetComponent<Text>().text = text;
		}
		if (num < 0)
		{
			string text2 = tS_.GetText(608);
			text2 = text2.Replace("<NUM>", mS_.GetMoney(Mathf.Abs(num), showDollar: true));
			uiObjects[0].GetComponent<Text>().text = text2;
		}
		if (mS_.settings_.dontAsk_TaskAbbrechen)
		{
			BUTTON_Yes();
		}
	}

	public void BUTTON_Abbrechen()
	{
		if (uiObjects[1].GetComponent<Toggle>().isOn)
		{
			mS_.settings_.dontAsk_TaskAbbrechen = true;
			mS_.settings_.SaveSettings();
		}
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if (!rS_)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			if ((bool)gameObject.GetComponent<taskEngine>())
			{
				gameObject.GetComponent<taskEngine>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskMarketing>())
			{
				gameObject.GetComponent<taskMarketing>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskMarketingSpezial>())
			{
				gameObject.GetComponent<taskMarketingSpezial>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskForschung>())
			{
				gameObject.GetComponent<taskForschung>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskForschungWait>())
			{
				gameObject.GetComponent<taskForschungWait>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskAutoForschung>())
			{
				gameObject.GetComponent<taskAutoForschung>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskGame>())
			{
				gameObject.GetComponent<taskGame>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskTraining>())
			{
				gameObject.GetComponent<taskTraining>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskContractWork>())
			{
				gameObject.GetComponent<taskContractWork>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskContractWait>())
			{
				gameObject.GetComponent<taskContractWait>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskUpdate>())
			{
				gameObject.GetComponent<taskUpdate>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskF2PUpdate>())
			{
				gameObject.GetComponent<taskF2PUpdate>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskFankampagne>())
			{
				gameObject.GetComponent<taskFankampagne>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskMitarbeitersuche>())
			{
				gameObject.GetComponent<taskMitarbeitersuche>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskBugfixing>())
			{
				gameObject.GetComponent<taskBugfixing>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskSupport>())
			{
				gameObject.GetComponent<taskSupport>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskFanshop>())
			{
				gameObject.GetComponent<taskFanshop>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskGameplayVerbessern>())
			{
				gameObject.GetComponent<taskGameplayVerbessern>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskGrafikVerbessern>())
			{
				gameObject.GetComponent<taskGrafikVerbessern>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskSoundVerbessern>())
			{
				gameObject.GetComponent<taskSoundVerbessern>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskAnimationVerbessern>())
			{
				gameObject.GetComponent<taskAnimationVerbessern>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskMarktforschung>())
			{
				gameObject.GetComponent<taskMarktforschung>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskPolishing>())
			{
				gameObject.GetComponent<taskPolishing>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskKonsole>())
			{
				gameObject.GetComponent<taskKonsole>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskKonsoleReduceCosts>())
			{
				gameObject.GetComponent<taskKonsoleReduceCosts>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskKonsoleHaltbarkeit>())
			{
				gameObject.GetComponent<taskKonsoleHaltbarkeit>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskArcadeProduction>())
			{
				gameObject.GetComponent<taskArcadeProduction>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskProduction>())
			{
				gameObject.GetComponent<taskProduction>().Abbrechen();
			}
			if ((bool)gameObject.GetComponent<taskSpielbericht>())
			{
				gameObject.GetComponent<taskSpielbericht>().Abbrechen();
			}
		}
		rS_.taskID = -1;
		rS_.taskGameObject = null;
		BUTTON_Abbrechen();
	}
}
