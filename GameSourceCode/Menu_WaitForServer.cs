using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Menu_WaitForServer : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	public NetworkManager manager;

	private void Awake()
	{
		FindScripts();
		uiObjects[2].GetComponent<Text>().text = tS_.GetText(1225);
		manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
	}

	private void Start()
	{
		FindScripts();
	}

	private void OnEnable()
	{
		FindScripts();
	}

	private void OnDisable()
	{
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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
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
		if (NetworkServer.active && manager.numPlayers > 1)
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
	}
}
