using UnityEngine;
using UnityEngine.UI;

public class Menu_MultiplayerView : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private mpCalls mpCalls_;

	private GameObject cameraMovement;

	public int playerID = -1;

	private Vector3 cameraRotation;

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
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
		if (!cameraMovement)
		{
			cameraMovement = GameObject.Find("CamMovement");
		}
	}

	public void Init(int p)
	{
		FindScripts();
		cameraRotation = cameraMovement.transform.eulerAngles;
		cameraMovement.transform.eulerAngles = new Vector3(cameraMovement.transform.eulerAngles.x, Random.Range(0, 359), cameraMovement.transform.eulerAngles.z);
		playerID = p;
		SetMainGuiToggles(b: false);
		guiMain_.CameraBlend();
		sfx_.PlaySound(58);
		if (p != -1)
		{
			string text = tS_.GetText(1277);
			text = text.Replace("<NAME>", mpCalls_.GetPlayerName(playerID));
			uiObjects[1].GetComponent<Text>().text = text;
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: true);
		}
		for (int i = 0; i < mS_.arrayCharacters.Length; i++)
		{
			if ((bool)mS_.arrayCharacters[i])
			{
				mS_.arrayCharacters[i].transform.localScale = new Vector3(0f, 0f, 0f);
			}
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(58);
		guiMain_.CameraBlend();
		playerID = -1;
		SetMainGuiToggles(b: true);
		mS_.CloseMultiplayerView();
		guiMain_.CloseMenu();
		sfx_.PlaySound(3, force: true);
		cameraMovement.transform.eulerAngles = cameraRotation;
		base.gameObject.SetActive(value: false);
	}

	private void SetMainGuiToggles(bool b)
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			if ((bool)uiObjects[0].transform.GetChild(i) && (bool)uiObjects[0].transform.GetChild(i).GetComponent<Toggle>())
			{
				uiObjects[0].transform.GetChild(i).GetComponent<Toggle>().interactable = b;
			}
		}
	}
}
