using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu_WaitForPlayers : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private mpCalls mpCalls_;

	private float sendTimer;

	private void Awake()
	{
		FindScripts();
		uiObjects[2].GetComponent<Text>().text = tS_.GetText(1040);
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	private void Start()
	{
		FindScripts();
	}

	private void OnEnable()
	{
		FindScripts();
		if (mpCalls_.isClient)
		{
			mpCalls_.CLIENT_Send_Command(1);
		}
	}

	private void OnDisable()
	{
		FindScripts();
		if ((bool)guiMain_ && !guiMain_.uiObjects[167].activeSelf)
		{
			guiMain_.uiObjects[167].SetActive(value: true);
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
		if (!sfx_ && (bool)GameObject.Find("SFX"))
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_ && (bool)GameObject.Find("CanvasInGameMenu"))
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void Update()
	{
		if ((bool)guiMain_ && guiMain_.uiObjects[167].activeSelf)
		{
			guiMain_.uiObjects[167].SetActive(value: false);
		}
		if (!NetworkClient.isConnected)
		{
			if (!uiObjects[1].activeSelf)
			{
				uiObjects[1].SetActive(value: true);
			}
		}
		else if (uiObjects[1].activeSelf)
		{
			uiObjects[1].SetActive(value: false);
		}
		if (!mS_.multiplayer)
		{
			base.gameObject.SetActive(value: false);
		}
		else if (mpCalls_.isServer)
		{
			if (mpCalls_.GetAllPlayersReady())
			{
				mS_.save_.loadingSavegame = false;
				mpCalls_.SERVER_Send_Command(2);
				base.gameObject.SetActive(value: false);
				Debug.Log("WaitForPlayers() CLOSE");
			}
		}
		else
		{
			sendTimer += Time.deltaTime;
			if ((double)sendTimer >= 1.0)
			{
				sendTimer = 0f;
				mpCalls_.CLIENT_Send_Command(1);
			}
		}
	}

	public void SetText(string s)
	{
		uiObjects[2].GetComponent<Text>().text = s;
	}

	public void BUTTON_Close()
	{
		FindScripts();
		sfx_.PlaySound(3, force: true);
		if (mS_.multiplayer)
		{
			guiMain_.uiObjects[201].SetActive(value: true);
			guiMain_.uiObjects[201].GetComponent<mpMain>().StopNetwork();
			guiMain_.uiObjects[201].SetActive(value: false);
		}
		guiMain_.RemoveVectrocity();
		SceneManager.LoadScene("scene01");
	}
}
