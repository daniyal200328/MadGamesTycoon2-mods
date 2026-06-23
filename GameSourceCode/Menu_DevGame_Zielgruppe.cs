using UnityEngine;
using UnityEngine.UI;

public class Menu_DevGame_Zielgruppe : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private Menu_DevGame mDevGame_;

	private games games_;

	private genres genres_;

	private int zielgruppe;

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
		if (!mDevGame_)
		{
			mDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		zielgruppe = mDevGame_.g_GameZielgruppe;
		UpdateGUI();
	}

	private void UpdateGUI()
	{
		uiObjects[1].GetComponent<Image>().color = Color.white;
		uiObjects[2].GetComponent<Image>().color = Color.white;
		uiObjects[3].GetComponent<Image>().color = Color.white;
		uiObjects[4].GetComponent<Image>().color = Color.white;
		uiObjects[5].GetComponent<Image>().color = Color.white;
		if (FitTargetGroup(0) == 1)
		{
			uiObjects[1].GetComponent<Image>().color = guiMain_.colors[4];
		}
		if (FitTargetGroup(1) == 1)
		{
			uiObjects[2].GetComponent<Image>().color = guiMain_.colors[4];
		}
		if (FitTargetGroup(2) == 1)
		{
			uiObjects[3].GetComponent<Image>().color = guiMain_.colors[4];
		}
		if (FitTargetGroup(3) == 1)
		{
			uiObjects[4].GetComponent<Image>().color = guiMain_.colors[4];
		}
		if (FitTargetGroup(4) == 1)
		{
			uiObjects[5].GetComponent<Image>().color = guiMain_.colors[4];
		}
		if (FitTargetGroup(0) == -1)
		{
			uiObjects[1].GetComponent<Image>().color = guiMain_.colors[8];
		}
		if (FitTargetGroup(1) == -1)
		{
			uiObjects[2].GetComponent<Image>().color = guiMain_.colors[8];
		}
		if (FitTargetGroup(2) == -1)
		{
			uiObjects[3].GetComponent<Image>().color = guiMain_.colors[8];
		}
		if (FitTargetGroup(3) == -1)
		{
			uiObjects[4].GetComponent<Image>().color = guiMain_.colors[8];
		}
		if (FitTargetGroup(4) == -1)
		{
			uiObjects[5].GetComponent<Image>().color = guiMain_.colors[8];
		}
	}

	private int FitTargetGroup(int targetGroup_)
	{
		int g_GameMainGenre = mDevGame_.g_GameMainGenre;
		if (g_GameMainGenre != -1)
		{
			if (mS_.settings_sandbox && mS_.sandbox_fitTopicToGenre)
			{
				if (genres_.IsTargetGroup(g_GameMainGenre, targetGroup_))
				{
					return 1;
				}
				return -1;
			}
			for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
			{
				if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].spielbericht && games_.arrayGamesScripts[i].maingenre == g_GameMainGenre && (games_.arrayGamesScripts[i].ownerID == mS_.myID || games_.arrayGamesScripts[i].developerID == mS_.myID) && games_.arrayGamesScripts[i].gameZielgruppe == targetGroup_)
				{
					if (genres_.IsTargetGroup(g_GameMainGenre, targetGroup_))
					{
						return 1;
					}
					return -1;
				}
			}
		}
		return 0;
	}

	public void BUTTON_GameZielgruppe(int i)
	{
		sfx_.PlaySound(3, force: true);
		zielgruppe = i;
		UpdateGUI();
		BUTTON_OK();
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		mDevGame_.SetZielgruppe(zielgruppe);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
