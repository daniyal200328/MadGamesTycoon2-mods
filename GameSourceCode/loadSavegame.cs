using System.Collections;
using UnityEngine;

public class loadSavegame : MonoBehaviour
{
	private GameObject main_;

	private mainScript mS_;

	private mpMain mpMain_;

	private savegameScript save_;

	private GUI_Main guiMain_;

	private mpCalls mpCalls_;

	private sfxScript sfX_;

	private ES3Writer writer;

	private ES3Reader reader;

	private void Start()
	{
		FindScripts();
		if (!ShouldSavegameLoad() && !ShouldMultiplayerSavegameLoad())
		{
			guiMain_.uiObjects[151].SetActive(value: true);
		}
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = base.gameObject;
		}
		if (!mS_)
		{
			mS_ = GetComponent<mainScript>();
		}
		if (!save_)
		{
			save_ = GetComponent<savegameScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!mpMain_)
		{
			mpMain_ = guiMain_.uiObjects[201].GetComponent<mpMain>();
		}
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
		if (!sfX_)
		{
			sfX_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
	}

	private bool ShouldSavegameLoad()
	{
		int num = PlayerPrefs.GetInt("LoadSavegame", -1);
		if (num >= 0)
		{
			StartCoroutine(LoadSaveGameAfterOneFrame(num));
			PlayerPrefs.SetInt("LoadSavegame", -1);
			return true;
		}
		return false;
	}

	private bool ShouldMultiplayerSavegameLoad()
	{
		int num = PlayerPrefs.GetInt("LoadMPSavegame", -1);
		if (num >= 0)
		{
			mS_.multiplayer = true;
			mS_.SetGameSpeed(0f);
			StartCoroutine(LoadSaveGameAfterOneFrame(num));
			PlayerPrefs.SetInt("LoadMPSavegame", -1);
			return true;
		}
		return false;
	}

	public void ForceClientLoadSaveGame(int i)
	{
		StartCoroutine(LoadSaveGameAfterOneFrame(i));
		PlayerPrefs.SetInt("LoadMPSavegame", -1);
	}

	private IEnumerator LoadSaveGameAfterOneFrame(int i)
	{
		if (mS_.multiplayer)
		{
			save_.loadingSavegame = true;
		}
		sfX_.SetRandomMusic();
		guiMain_.uiObjects[152].SetActive(value: true);
		guiMain_.uiObjects[151].SetActive(value: false);
		guiMain_.uiObjects[155].SetActive(value: false);
		guiMain_.ShowInGameUI(show: false);
		mS_.LoadOffice(save_.GetOfficeFromSavegame(i), fromSavegame: true);
		Debug.Log("LoadOffice: Wait()");
		yield return new WaitUntil(() => mS_.officeLoaded);
		Debug.Log("LoadOffice: Complete() -> Start");
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Debug.Log("LoadOffice: Complete() -> End");
		save_.Load(i);
		guiMain_.uiObjects[152].SetActive(value: false);
		guiMain_.ShowInGameUI(show: true);
		mS_.DestroyMainMenuObjects();
		if (!mS_.multiplayer)
		{
			yield break;
		}
		if (mpCalls_.isServer)
		{
			mpCalls_.SetPlayersUnready();
		}
		guiMain_.ActivateMenu(guiMain_.uiObjects[202]);
		if (mpCalls_.isServer)
		{
			if (mS_.multiplayer)
			{
				save_.loadingSavegame = true;
			}
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			mpCalls_.SERVER_Send_Load(mS_.multiplayerSaveID);
		}
		else
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
		}
	}
}
