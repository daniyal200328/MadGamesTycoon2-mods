using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_ChangeCopyProtect : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private licences licences_;

	private engineFeatures eF_;

	private cameraMovementScript cmS_;

	private unlockScript unlock_;

	private gameplayFeatures gF_;

	private games games_;

	private platforms platforms_;

	public gameScript gS_;

	public int g_GameCopyProtect = -1;

	public copyProtectScript g_GameCopyProtectScript_;

	public int g_GameAntiCheat = -1;

	public antiCheatScript g_GameAntiCheatScript_;

	private float checkSoftwareTimer;

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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		SetCopyProtect(gS_.gameCopyProtect);
		SetAntiCheat(gS_.gameAntiCheat);
		Unlock(31, uiObjects[10], uiObjects[11]);
		Unlock(31, null, uiObjects[12]);
		Unlock(64, uiObjects[13], uiObjects[14]);
		Unlock(64, null, uiObjects[15]);
		if (game_.arcade)
		{
			uiObjects[11].GetComponent<Button>().interactable = false;
			uiObjects[12].GetComponent<Button>().interactable = false;
			uiObjects[14].GetComponent<Button>().interactable = false;
			uiObjects[15].GetComponent<Button>().interactable = false;
		}
		CalcDevCosts();
	}

	private void Update()
	{
		CheckNewSoftware();
	}

	private void Unlock(int id_, GameObject lock_, GameObject button_)
	{
		if (unlock_.unlock[id_])
		{
			button_.GetComponent<Button>().interactable = true;
			if ((bool)lock_)
			{
				lock_.SetActive(value: false);
			}
		}
		else
		{
			button_.GetComponent<Button>().interactable = false;
			if ((bool)lock_)
			{
				lock_.SetActive(value: true);
			}
		}
	}

	public void BUTTON_CopyProtect()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[68]);
	}

	public void BUTTON_CopyProtectKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[49]);
	}

	public void BUTTON_AntiCheat()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[236]);
	}

	public void BUTTON_AntiCheatKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[234]);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.CloseMenu();
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		mS_.Pay(CalcDevCosts(), 10);
		gS_.costs_entwicklung += CalcDevCosts();
		gS_.gameAntiCheat = g_GameAntiCheat;
		gS_.gameCopyProtect = g_GameCopyProtect;
		gS_.gameAntiCheatScript_ = g_GameAntiCheatScript_;
		gS_.gameCopyProtectScript_ = g_GameCopyProtectScript_;
		base.gameObject.SetActive(value: false);
		guiMain_.CloseMenu();
	}

	public void SetCopyProtect(int i)
	{
		g_GameCopyProtect = i;
		if (i >= 0)
		{
			GameObject gameObject = GameObject.Find("COPYPROTECT_" + i);
			if ((bool)gameObject)
			{
				copyProtectScript copyProtectScript2 = (g_GameCopyProtectScript_ = gameObject.GetComponent<copyProtectScript>());
				uiObjects[2].GetComponent<Text>().text = copyProtectScript2.GetName();
				uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(copyProtectScript2.GetDevCosts(), showDollar: true);
				uiObjects[4].GetComponent<Image>().fillAmount = copyProtectScript2.effekt * 0.01f;
				uiObjects[5].GetComponent<Text>().text = mS_.Round(copyProtectScript2.effekt, 2) + "%";
				uiObjects[4].GetComponent<Image>().color = GetValColor(copyProtectScript2.effekt);
			}
		}
		else
		{
			g_GameCopyProtectScript_ = null;
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(383);
			uiObjects[3].GetComponent<Text>().text = "";
			uiObjects[4].GetComponent<Image>().fillAmount = 0f;
			uiObjects[5].GetComponent<Text>().text = "0.0%";
			uiObjects[4].GetComponent<Image>().color = GetValColor(0f);
		}
		CalcDevCosts();
	}

	public void SetAntiCheat(int i)
	{
		g_GameAntiCheat = i;
		if (i >= 0)
		{
			GameObject gameObject = GameObject.Find("ANTICHEAT_" + i);
			if ((bool)gameObject)
			{
				antiCheatScript antiCheatScript2 = (g_GameAntiCheatScript_ = gameObject.GetComponent<antiCheatScript>());
				uiObjects[6].GetComponent<Text>().text = antiCheatScript2.GetName();
				uiObjects[7].GetComponent<Text>().text = mS_.GetMoney(antiCheatScript2.GetDevCosts(), showDollar: true);
				uiObjects[8].GetComponent<Image>().fillAmount = antiCheatScript2.effekt * 0.01f;
				uiObjects[9].GetComponent<Text>().text = mS_.Round(antiCheatScript2.effekt, 2) + "%";
				uiObjects[8].GetComponent<Image>().color = GetValColor(antiCheatScript2.effekt);
			}
		}
		else
		{
			g_GameAntiCheatScript_ = null;
			uiObjects[6].GetComponent<Text>().text = tS_.GetText(1213);
			uiObjects[7].GetComponent<Text>().text = "";
			uiObjects[8].GetComponent<Image>().fillAmount = 0f;
			uiObjects[9].GetComponent<Text>().text = "0.0%";
			uiObjects[8].GetComponent<Image>().color = GetValColor(0f);
		}
		CalcDevCosts();
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

	private int CalcDevCosts()
	{
		int num = 0;
		if ((bool)g_GameCopyProtectScript_ && g_GameCopyProtect != gS_.gameCopyProtect)
		{
			num += g_GameCopyProtectScript_.GetDevCosts();
		}
		if ((bool)g_GameAntiCheatScript_ && g_GameAntiCheat != gS_.gameAntiCheat)
		{
			num += g_GameAntiCheatScript_.GetDevCosts();
		}
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		return num;
	}

	private void CheckNewSoftware()
	{
		checkSoftwareTimer += Time.deltaTime;
		if ((double)checkSoftwareTimer < 1.0)
		{
			return;
		}
		checkSoftwareTimer = 0f;
		if (mS_.IsBetterCopyProtect() && uiObjects[11].GetComponent<Button>().interactable)
		{
			if (!uiObjects[16].activeSelf)
			{
				uiObjects[16].SetActive(value: true);
			}
		}
		else if (uiObjects[16].activeSelf)
		{
			uiObjects[16].SetActive(value: false);
		}
		if (mS_.IsBetterAntiCheat() && uiObjects[14].GetComponent<Button>().interactable)
		{
			if (!uiObjects[17].activeSelf)
			{
				uiObjects[17].SetActive(value: true);
			}
		}
		else if (uiObjects[17].activeSelf)
		{
			uiObjects[17].SetActive(value: false);
		}
	}
}
