using UnityEngine;
using UnityEngine.UI;

public class Menu_Marktforschung : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private unlockScript unlock_;

	private genres genres_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private roomScript rS_;

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
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
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
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
	}

	private void Update()
	{
	}

	public void Init(roomScript roomS_)
	{
		FindScripts();
		rS_ = roomS_;
		if (!rS_)
		{
			uiObjects[13].SetActive(value: false);
			uiObjects[14].SetActive(value: true);
		}
		else
		{
			uiObjects[13].SetActive(value: true);
			uiObjects[14].SetActive(value: false);
		}
		if (mS_.marktforschung_retail <= 0f)
		{
			uiObjects[0].SetActive(value: true);
			uiObjects[1].SetActive(value: false);
			return;
		}
		uiObjects[0].SetActive(value: false);
		uiObjects[1].SetActive(value: true);
		string text = tS_.GetText(1167);
		text = text.Replace("<NAME>", mS_.marktforschung_datum);
		uiObjects[6].GetComponent<Text>().text = text;
		SetPlatform(mS_.marktforschung_bestPlattform, 15);
		SetPlatform(mS_.marktforschung_bestPlattformKonsole, 16);
		SetPlatform(mS_.marktforschung_bestPlattformHandheld, 17);
		SetPlatform(mS_.marktforschung_bestPlattformHandy, 18);
		SetPlatform(-1, 19);
		SetPlatform(-1, 20);
		SetPlatform(-1, 21);
		SetPlatform(-1, 22);
		if (mS_.marktforschung_bestPlattform != mS_.marktforschung_badPlattform)
		{
			SetPlatform(mS_.marktforschung_badPlattform, 19);
		}
		if (mS_.marktforschung_bestPlattformKonsole != mS_.marktforschung_badPlattformKonsole)
		{
			SetPlatform(mS_.marktforschung_badPlattformKonsole, 20);
		}
		if (mS_.marktforschung_bestPlattformHandheld != mS_.marktforschung_badPlattformHandheld)
		{
			SetPlatform(mS_.marktforschung_badPlattformHandheld, 21);
		}
		if (mS_.marktforschung_bestPlattformHandy != mS_.marktforschung_badPlattformHandy)
		{
			SetPlatform(mS_.marktforschung_badPlattformHandy, 22);
		}
		text = tS_.GetText(1169);
		text = text.Replace("<NUM1>", mS_.Round(mS_.marktforschung_retail * 100f, 1) + "%");
		text = text.Replace("<NUM2>", mS_.Round(mS_.marktforschung_digtal * 100f, 1) + "%");
		uiObjects[7].GetComponent<Text>().text = text;
		text = tS_.GetText(1170);
		text = text.Replace("<NUM1>", mS_.Round(mS_.marktforschung_deluxe * 100f, 1) + "%");
		text = text.Replace("<NUM2>", mS_.Round(mS_.marktforschung_collectors * 100f, 1) + "%");
		uiObjects[8].GetComponent<Text>().text = text;
		text = tS_.GetText(1171);
		text = text.Replace("<NAME1>", genres_.GetName(mS_.marktforschung_nextGenre));
		text = text.Replace("<NAME2>", tS_.GetThemes(mS_.marktforschung_nextTopic));
		uiObjects[10].GetComponent<Text>().text = text;
		uiObjects[9].GetComponent<Image>().sprite = genres_.GetPic(mS_.marktforschung_nextGenre);
		text = tS_.GetText(1172);
		text = text.Replace("<NAME1>", genres_.GetName(mS_.marktforschung_nextBadGenre));
		text = text.Replace("<NAME2>", tS_.GetThemes(mS_.marktforschung_nextBadTopic));
		uiObjects[12].GetComponent<Text>().text = text;
		uiObjects[11].GetComponent<Image>().sprite = genres_.GetPic(mS_.marktforschung_nextBadGenre);
		text = tS_.GetText(1533);
		text = text.Replace("<NUM>", Mathf.RoundToInt(mS_.marktforschung_arcade * 100f) + "%");
		uiObjects[23].GetComponent<Text>().text = text;
		if (mS_.marktforschung_internet > 0f)
		{
			text = tS_.GetText(2024);
			text = text.Replace("<NUM>", Mathf.RoundToInt(mS_.marktforschung_internet * 100f) + "%");
			uiObjects[24].GetComponent<Text>().text = text;
		}
		else
		{
			uiObjects[24].GetComponent<Text>().text = tS_.GetText(2025);
		}
		if (mS_.marktforschung_gamePass > 0f)
		{
			text = tS_.GetText(2177);
			text = text.Replace("<NUM>", Mathf.RoundToInt(mS_.marktforschung_gamePass * 100f) + "%");
			uiObjects[25].GetComponent<Text>().text = text;
		}
		else
		{
			uiObjects[25].GetComponent<Text>().text = tS_.GetText(2025);
		}
	}

	private void SetPlatform(int id, int objectSlot)
	{
		GameObject gameObject = null;
		if (id != -1)
		{
			gameObject = GameObject.Find("PLATFORM_" + id);
		}
		if ((bool)gameObject)
		{
			uiObjects[objectSlot].SetActive(value: true);
			platformScript component = gameObject.GetComponent<platformScript>();
			component.SetPic(uiObjects[objectSlot]);
			uiObjects[objectSlot].GetComponent<tooltip>().c = component.GetTooltip();
		}
		else
		{
			uiObjects[objectSlot].SetActive(value: false);
			uiObjects[objectSlot].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			uiObjects[objectSlot].GetComponent<tooltip>().c = "";
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		if (!guiMain_.uiObjects[56].activeSelf)
		{
			guiMain_.CloseMenu();
		}
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_StarteMarktforschung()
	{
		if ((bool)rS_)
		{
			sfx_.PlaySound(3, force: true);
			taskMarktforschung taskMarktforschung2 = guiMain_.AddTask_Marktforschung();
			taskMarktforschung2.Init(fromSavegame: false);
			taskMarktforschung2.points = 100f;
			taskMarktforschung2.pointsLeft = 100f;
			GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
			if ((bool)gameObject)
			{
				gameObject.GetComponent<roomScript>().taskID = taskMarktforschung2.myID;
			}
			guiMain_.CloseMenu();
			base.gameObject.SetActive(value: false);
		}
	}
}
