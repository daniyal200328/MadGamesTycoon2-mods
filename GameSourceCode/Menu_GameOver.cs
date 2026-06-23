using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_GameOver : MonoBehaviour
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
		FindScripts();
		if (!mS_.multiplayer)
		{
			return;
		}
		mS_.bankWarning = 0;
		mS_.kredit = 0L;
		mS_.money = 0L;
		int num = mS_.studioPoints / 2;
		mS_.AddStudioPoints(-num);
		int num2 = mS_.genres_.GetAmountFans() / 2;
		mS_.AddFans(-num2, -1);
		mS_.auftragsAnsehen /= 2f;
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i])
			{
				mS_.arrayPublisherScripts[i].relation = mS_.arrayPublisherScripts[i].relation / 2f;
			}
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				gameScript component = array[j].GetComponent<gameScript>();
				if ((bool)component && component.ownerID == mS_.myID && component.mainIP == component.myID)
				{
					float num3 = component.ipPunkte / 2f;
					component.AddIpPoints(0f - num3);
				}
			}
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void BUTTON_Yes_Singleplayer()
	{
		if (mS_.multiplayer)
		{
			guiMain_.uiObjects[201].GetComponent<mpMain>().StopNetwork();
		}
		guiMain_.RemoveVectrocity();
		SceneManager.LoadScene("scene01");
	}

	public void BUTTON_Yes_Multiplayer()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
