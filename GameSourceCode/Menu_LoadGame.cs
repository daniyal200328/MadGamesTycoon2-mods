using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_LoadGame : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private savegameScript save_;

	private mpCalls mpCalls_;

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
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
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
		string filePath = mS_.GetSavegameTitle() + i + ".sav";
		Transform child = uiObjects[0].transform.GetChild(i);
		Transform child2 = child.transform.GetChild(0);
		Transform child3 = child.transform.GetChild(1);
		Transform child4 = child.transform.GetChild(2);
		Transform child5 = child2.transform.GetChild(0);
		if (ES3.FileExists(filePath))
		{
			string text = ES3.LoadRawString(filePath);
			if (text.Length > 0 && text[0] == '{')
			{
				reader = ES3Reader.Create(filePath);
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
					return;
				}
			}
		}
		if ((bool)child2)
		{
			child2.GetComponent<Button>().interactable = false;
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

	private IEnumerator LoadSaveGameAfterOneFrame(int i)
	{
		guiMain_.uiObjects[152].SetActive(value: true);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		save_.Load(i);
		guiMain_.uiObjects[151].SetActive(value: false);
		guiMain_.uiObjects[152].SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_LoadGame(int i)
	{
		FindScripts();
		if (!mS_.multiplayer)
		{
			PlayerPrefs.SetInt("LoadSavegame", i);
			PlayerPrefs.SetInt("LoadMPSavegame", -1);
		}
		else
		{
			if (mpCalls_.isServer)
			{
				mS_.Multiplayer_LockLobby();
				save_.loadingSavegame = true;
				mS_.mpCalls_.SERVER_Send_Command(5);
			}
			PlayerPrefs.SetInt("LoadSavegame", -1);
			PlayerPrefs.SetInt("LoadMPSavegame", i);
		}
		guiMain_.RemoveVectrocity();
		guiMain_.uiObjects[152].SetActive(value: true);
		if (!mS_.multiplayer)
		{
			SceneManager.LoadScene("scene01");
		}
		else
		{
			StartCoroutine(iEnumLoadScene());
		}
	}

	private IEnumerator iEnumLoadScene()
	{
		yield return new WaitForSeconds(5f);
		yield return new WaitForEndOfFrame();
		SceneManager.LoadScene("scene01");
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
