using UnityEngine;
using UnityEngine.UI;

public class Menu_SaveGame : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private savegameScript save_;

	private ES3Writer writer;

	private ES3Reader reader;

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
		if (!save_)
		{
			save_ = main_.GetComponent<savegameScript>();
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
		FindScripts();
		Init();
	}

	public void Init()
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			LoadFile(i);
		}
	}

	private void LoadFile(int i)
	{
		string text = mS_.GetSavegameTitle() + i + ".sav";
		Debug.Log("Savegame: " + text);
		Transform child = uiObjects[0].transform.GetChild(i);
		Transform child2 = child.transform.GetChild(0);
		Transform child3 = child.transform.GetChild(1);
		Transform child4 = child.transform.GetChild(2);
		Transform child5 = child2.transform.GetChild(0);
		if (ES3.FileExists(text))
		{
			string text2 = ES3.LoadRawString(text);
			if (text2.Length > 0 && text2[0] == '{')
			{
				reader = ES3Reader.Create(text);
				if (reader != null)
				{
					reader.Dispose();
					if ((bool)child2)
					{
						child2.GetComponent<Button>().interactable = true;
					}
					if ((bool)child3)
					{
						child3.GetComponent<Button>().interactable = true;
					}
					if ((bool)child5)
					{
						child5.GetComponent<Text>().text = save_.LoadSaveGameName(i);
					}
					if (save_.IsSaveGameOutdatet(i))
					{
						if ((bool)child4)
						{
							child4.gameObject.SetActive(value: true);
						}
						if ((bool)child5)
						{
							child5.GetComponent<Text>().color = guiMain_.colors[5];
						}
					}
					else
					{
						if ((bool)child4)
						{
							child4.gameObject.SetActive(value: false);
						}
						if ((bool)child5)
						{
							child5.GetComponent<Text>().color = guiMain_.colors[6];
						}
					}
					if (i == 0)
					{
						child2.GetComponent<Button>().interactable = false;
					}
					return;
				}
			}
		}
		if ((bool)child3)
		{
			child3.GetComponent<Button>().interactable = false;
		}
		if ((bool)child5)
		{
			child5.GetComponent<Text>().text = tS_.GetText(783);
		}
		if ((bool)child5)
		{
			child5.GetComponent<Text>().color = guiMain_.colors[6];
		}
		if ((bool)child4)
		{
			child4.gameObject.SetActive(value: false);
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_SaveGame(int i)
	{
		sfx_.PlaySound(3, force: true);
		string filePath = mS_.GetSavegameTitle() + i + ".sav";
		reader = ES3Reader.Create(filePath);
		if (reader == null)
		{
			if (!mS_.multiplayer)
			{
				save_.Save(i);
			}
			else
			{
				save_.SaveMultiplayer(i);
			}
			guiMain_.uiObjects[155].SetActive(value: false);
			BUTTON_Abbrechen();
			guiMain_.ShowGameHasSaved();
		}
		else
		{
			reader.Dispose();
			guiMain_.uiObjects[157].SetActive(value: true);
			guiMain_.uiObjects[157].GetComponent<Menu_OverwriteSavegame>().Init(i, save_.LoadSaveGameName(i));
		}
	}

	public void BUTTON_DeleteSaveGame(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[153].SetActive(value: true);
		guiMain_.uiObjects[153].GetComponent<Menu_DeleteSaveGame>().Init(i, save_.LoadSaveGameName(i));
	}

	public void BUTTON_DeleteAllSaveGames()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[264].SetActive(value: true);
	}
}
