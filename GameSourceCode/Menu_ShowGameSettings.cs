using UnityEngine;
using UnityEngine.UI;

public class Menu_ShowGameSettings : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript camMove_;

	private mpCalls mpCalls_;

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
		if ((bool)main_)
		{
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
			if (!camMove_)
			{
				camMove_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
			}
			if (!mpCalls_)
			{
				mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
			}
		}
	}

	private void OnEnable()
	{
		FindScripts();
		Init();
	}

	private void Init()
	{
		if (mS_.settings_sandbox)
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(2064);
			uiObjects[14].SetActive(value: true);
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(2063);
			uiObjects[14].SetActive(value: false);
		}
		switch (mS_.difficulty)
		{
		case 0:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(802);
			break;
		case 1:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(803);
			break;
		case 2:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(804);
			break;
		case 3:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(805);
			break;
		case 4:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1685);
			break;
		case 5:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(806);
			break;
		}
		if (mS_.settings_startjahr > 0)
		{
			uiObjects[2].GetComponent<Text>().text = mS_.settings_startjahr.ToString();
		}
		else
		{
			uiObjects[2].GetComponent<Text>().text = "-";
		}
		if (mS_.anzKonkurrenten < 99999)
		{
			uiObjects[3].GetComponent<Text>().text = mS_.anzKonkurrenten.ToString();
		}
		else
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
			uiObjects[3].GetComponent<Text>().text = (array.Length - 1).ToString();
		}
		uiObjects[17].GetComponent<Toggle>().isOn = mS_.settings_closeNPCs;
		switch (mS_.settings_competition)
		{
		case 0:
			uiObjects[16].GetComponent<Text>().text = tS_.GetText(2398);
			break;
		case 1:
			uiObjects[16].GetComponent<Text>().text = tS_.GetText(2399);
			break;
		case 2:
			uiObjects[16].GetComponent<Text>().text = tS_.GetText(2400);
			break;
		}
		switch (mS_.devTimeSetting)
		{
		case 0:
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(2073);
			break;
		case 1:
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(2074);
			break;
		case 2:
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(2269);
			break;
		}
		for (int i = 0; i < mS_.gameSpeeds.Length; i++)
		{
			if (mS_.gameSpeeds[i] == mS_.speedSetting)
			{
				switch (i)
				{
				case 0:
					uiObjects[5].GetComponent<Text>().text = tS_.GetText(1335);
					break;
				case 1:
					uiObjects[5].GetComponent<Text>().text = tS_.GetText(807);
					break;
				case 2:
					uiObjects[5].GetComponent<Text>().text = tS_.GetText(808);
					break;
				case 3:
					uiObjects[5].GetComponent<Text>().text = tS_.GetText(809);
					break;
				case 4:
					uiObjects[5].GetComponent<Text>().text = tS_.GetText(810);
					break;
				}
			}
		}
		uiObjects[6].GetComponent<Toggle>().isOn = mS_.settings_arbeitsgeschwindigkeitAnpassen;
		switch (mS_.settings_randomEvents)
		{
		case 0:
			uiObjects[15].GetComponent<Text>().text = tS_.GetText(2073);
			break;
		case 1:
			uiObjects[15].GetComponent<Text>().text = tS_.GetText(1001);
			break;
		case 2:
			uiObjects[15].GetComponent<Text>().text = tS_.GetText(837);
			break;
		}
		uiObjects[7].GetComponent<Toggle>().isOn = mS_.settings_randomPlattformPop;
		uiObjects[8].GetComponent<Toggle>().isOn = mS_.settings_plattformEnd;
		uiObjects[9].GetComponent<Toggle>().isOn = mS_.settings_randomGameConcept;
		uiObjects[10].GetComponent<Toggle>().isOn = mS_.settings_randomGenreCombination;
		uiObjects[11].GetComponent<Toggle>().isOn = mS_.settings_RandomReviews;
		uiObjects[20].GetComponent<Toggle>().isOn = mS_.sandbox_tochterfirmaKonsole;
		if (mS_.settings_RandomReviews)
		{
			uiObjects[18].GetComponent<Text>().text = "+/- " + mS_.settings_RandomReviewsNum * 3 + "%";
		}
		else
		{
			uiObjects[18].GetComponent<Text>().text = "";
		}
		if (mS_.settings_randomPlattformPop)
		{
			switch (mS_.settings_randomPlattformNum)
			{
			case 1:
				uiObjects[19].GetComponent<Text>().text = tS_.GetText(1908);
				break;
			case 2:
				uiObjects[19].GetComponent<Text>().text = tS_.GetText(1909);
				break;
			case 3:
				uiObjects[19].GetComponent<Text>().text = tS_.GetText(1910);
				break;
			case 4:
				uiObjects[19].GetComponent<Text>().text = tS_.GetText(2436);
				break;
			}
		}
		else
		{
			uiObjects[19].GetComponent<Text>().text = "";
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_SandboxSettings()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[427].SetActive(value: true);
		guiMain_.uiObjects[427].GetComponent<Menu_ShowSandboxSettings>().Init(mS_.sandbox_string, close: false);
	}
}
