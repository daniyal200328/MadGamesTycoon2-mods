using UnityEngine;
using UnityEngine.UI;

public class checkServerReserve : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	public GameObject[] uiObjects;

	private void Start()
	{
		FindScripts();
		Init();
	}

	private void OnEnable()
	{
		FindScripts();
		Init();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
	}

	private void Init()
	{
		uiObjects[1].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
		uiObjects[2].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
		uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
		uiObjects[5].GetComponent<Text>().color = Color.black;
		uiObjects[6].GetComponent<Text>().color = Color.black;
		uiObjects[7].GetComponent<Text>().color = Color.black;
		roomScript rS_ = uiObjects[0].GetComponent<roomButtonScript>().rS_;
		if ((bool)rS_)
		{
			switch (rS_.serverReservieren)
			{
			case 0:
				uiObjects[4].GetComponent<Text>().color = Color.grey;
				break;
			case 1:
				uiObjects[1].GetComponent<Image>().sprite = guiMain_.uiSprites[14];
				uiObjects[4].GetComponent<Text>().color = Color.black;
				uiObjects[5].GetComponent<Text>().color = Color.blue;
				break;
			case 2:
				uiObjects[2].GetComponent<Image>().sprite = guiMain_.uiSprites[14];
				uiObjects[4].GetComponent<Text>().color = Color.black;
				uiObjects[6].GetComponent<Text>().color = Color.blue;
				break;
			case 3:
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[14];
				uiObjects[4].GetComponent<Text>().color = Color.black;
				uiObjects[7].GetComponent<Text>().color = Color.blue;
				break;
			}
		}
	}
}
