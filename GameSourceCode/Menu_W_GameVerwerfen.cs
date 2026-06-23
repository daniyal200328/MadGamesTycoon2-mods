using UnityEngine;
using UnityEngine.UI;

public class Menu_W_GameVerwerfen : MonoBehaviour
{
	public GameObject[] uiObjects;

	private platformScript pS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private games games_;

	private gameScript gS_;

	private taskGame taskGame_;

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

	public void Init(gameScript script_, taskGame task_)
	{
		FindScripts();
		if (!script_)
		{
			return;
		}
		gS_ = script_;
		taskGame_ = task_;
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		if (!gS_ || gS_.portID != -1)
		{
			return;
		}
		for (int i = 0; i < gS_.portExist.Length; i++)
		{
			if (!gS_.portExist[i])
			{
				continue;
			}
			for (int j = 0; j < games_.arrayGamesScripts.Length; j++)
			{
				if (games_.arrayGamesScripts[j].inDevelopment && games_.arrayGamesScripts[j].portID == gS_.myID)
				{
					BUTTON_Abbrechen();
					guiMain_.MessageBox(tS_.GetText(2076), closeMenu: false);
					return;
				}
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)taskGame_)
		{
			taskGame_.Abbrechen();
		}
		else
		{
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_GameDestroy(gS_);
				}
				else
				{
					mS_.mpCalls_.CLIENT_Send_GameDestroy(gS_);
				}
			}
			gS_.gameObject.tag = "GameRemoved";
			Object.Destroy(gS_.gameObject);
			games_.FindGames();
		}
		base.gameObject.SetActive(value: false);
		if (guiMain_.uiObjects[69].activeSelf)
		{
			guiMain_.uiObjects[69].SetActive(value: false);
		}
		if (guiMain_.uiObjects[397].activeSelf)
		{
			guiMain_.uiObjects[397].SetActive(value: false);
		}
		if (guiMain_.uiObjects[387].activeSelf)
		{
			guiMain_.uiObjects[387].GetComponent<Menu_Stats_Tochterfirma_Main>().ResetGame();
		}
		else
		{
			guiMain_.CloseMenu();
		}
	}
}
