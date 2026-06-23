using UnityEngine;
using UnityEngine.UI;

public class Menu_MitarbeitersucheResult : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private pickCharacterScript pcS_;

	private charArbeitsmarkt cA_;

	private cameraMovementScript cmS_;

	public GameObject[] uiObjects;

	private void Start()
	{
		FindScripts();
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
		if (!cA_)
		{
			BUTTON_Close();
		}
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
		if (!pcS_)
		{
			pcS_ = main_.GetComponent<pickCharacterScript>();
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
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
	}

	public void Init(charArbeitsmarkt charArbeitsmarkt_)
	{
		FindScripts();
		sfx_.PlaySound(31);
		mS_.CreateFoto(null, charArbeitsmarkt_);
		cA_ = charArbeitsmarkt_;
		string text = tS_.GetText(1717);
		text = text.Replace("<NAME>", cA_.myName);
		uiObjects[12].GetComponent<Text>().text = text;
		uiObjects[0].GetComponent<Text>().text = cA_.myName;
		uiObjects[11].GetComponent<Text>().text = tS_.GetText(137 + cA_.beruf);
		guiMain_.SetBalkenArbeitsmarkt(uiObjects[1], cA_.s_gamedesign, 0, cA_);
		guiMain_.SetBalkenArbeitsmarkt(uiObjects[2], cA_.s_programmieren, 1, cA_);
		guiMain_.SetBalkenArbeitsmarkt(uiObjects[3], cA_.s_grafik, 2, cA_);
		guiMain_.SetBalkenArbeitsmarkt(uiObjects[4], cA_.s_sound, 3, cA_);
		guiMain_.SetBalkenArbeitsmarkt(uiObjects[5], cA_.s_pr, 4, cA_);
		guiMain_.SetBalkenArbeitsmarkt(uiObjects[6], cA_.s_gametests, 5, cA_);
		guiMain_.SetBalkenArbeitsmarkt(uiObjects[7], cA_.s_technik, 6, cA_);
		guiMain_.SetBalkenArbeitsmarkt(uiObjects[8], cA_.s_forschen, 7, cA_);
		guiMain_.CreatePerkIconsArbeitsmarkt(cA_, uiObjects[9].transform);
		uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(cA_.GetGehalt(), showDollar: true);
	}

	private Color GetValColor(float val)
	{
		if (val < 30f)
		{
			return guiMain_.colorsBalken[0];
		}
		if (val >= 30f && val < 70f)
		{
			return guiMain_.colorsBalken[1];
		}
		if (val >= 70f)
		{
			return guiMain_.colorsBalken[2];
		}
		return guiMain_.colorsBalken[0];
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: false);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Einstellen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		if (guiMain_.uiObjects[30].activeSelf)
		{
			guiMain_.uiObjects[30].SetActive(value: false);
		}
		characterScript characterScript2 = main_.GetComponent<createCharScript>().CreateCharacter(cA_.myID, cA_.male, cA_.model_body);
		characterScript2.model_body = cA_.model_body;
		characterScript2.model_eyes = cA_.model_eyes;
		characterScript2.model_hair = cA_.model_hair;
		characterScript2.model_beard = cA_.model_beard;
		characterScript2.model_skinColor = cA_.model_skinColor;
		characterScript2.model_hairColor = cA_.model_hairColor;
		characterScript2.model_beardColor = cA_.model_beardColor;
		characterScript2.model_HoseColor = cA_.model_HoseColor;
		characterScript2.model_ShirtColor = cA_.model_ShirtColor;
		characterScript2.model_Add1Color = cA_.model_Add1Color;
		characterScript2.gameObject.transform.GetChild(0).GetComponent<characterGFXScript>().Init(forcedClothes: true);
		mS_.CopyArbeitsmarktCharacterData(cA_, characterScript2);
		pcS_.PickFromExternObject(characterScript2.gameObject);
		cA_.RemoveFromArbeitsmarkt(eingestellt: true);
		base.gameObject.SetActive(value: false);
	}
}
