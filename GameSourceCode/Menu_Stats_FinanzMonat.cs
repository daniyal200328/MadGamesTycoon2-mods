using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_FinanzMonat : MonoBehaviour
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

	private void Start()
	{
		FindScripts();
	}

	private void OnEnable()
	{
		Init();
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
	}

	public void Init()
	{
		FindScripts();
		SetData();
	}

	private void SetData()
	{
		string text = "";
		text = text + "<b>" + tS_.GetText(711) + "</b>\n";
		text = text + tS_.GetText(707) + "\n";
		text = text + tS_.GetText(708) + "\n";
		text = text + tS_.GetText(19) + "\n";
		text = text + tS_.GetText(20) + "\n";
		text = text + tS_.GetText(117) + "\n";
		text = text + tS_.GetText(747) + "\n";
		text = text + tS_.GetText(530) + "\n";
		text = text + tS_.GetText(531) + "\n";
		text = text + tS_.GetText(1656) + "\n";
		text = text + tS_.GetText(1655) + "\n";
		text = text + tS_.GetText(2382) + "\n";
		text = text + tS_.GetText(709) + "\n";
		text = text + tS_.GetText(570) + "\n";
		text = text + tS_.GetText(1842) + "\n";
		text = text + tS_.GetText(710) + "\n";
		text = text + tS_.GetText(211) + "\n";
		text = text + tS_.GetText(734) + "\n";
		text = text + tS_.GetText(1923) + "\n";
		text = text + tS_.GetText(163) + "\n";
		text += "\n";
		text = text + "<b>" + tS_.GetText(712) + "</b>\n";
		text = text + tS_.GetText(713) + "\n";
		text = text + tS_.GetText(1236) + "\n";
		text = text + tS_.GetText(1177) + "\n";
		text = text + tS_.GetText(1243) + "\n";
		text = text + tS_.GetText(714) + "\n";
		text = text + tS_.GetText(715) + "\n";
		text = text + tS_.GetText(716) + "\n";
		text = text + tS_.GetText(943) + "\n";
		text = text + tS_.GetText(708) + "\n";
		text = text + tS_.GetText(570) + "\n";
		text = text + tS_.GetText(1842) + "\n";
		text = text + tS_.GetText(1923) + "\n";
		text += tS_.GetText(163);
		uiObjects[0].GetComponent<Text>().text = text;
		text = "<color=red>\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[0], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[1], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[2], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[3], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[4], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[5], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[12], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[17], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[13], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[14], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[18], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[6], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[7], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[15], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[8], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[10], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[11], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[16], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[9], showDollar: true) + "\n";
		text += "</color>\n";
		text += "<color=green>\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[50], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[57], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[58], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[62], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[51], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[52], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[53], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[59], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[54], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[55], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[60], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonat[61], showDollar: true) + "\n";
		text += mS_.GetMoney(mS_.finanzenMonat[56], showDollar: true);
		text += "</color>";
		uiObjects[1].GetComponent<Text>().text = text;
		text = "<color=red>\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[0], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[1], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[2], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[3], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[4], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[5], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[12], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[17], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[13], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[14], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[18], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[6], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[7], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[15], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[8], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[10], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[11], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[16], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[9], showDollar: true) + "\n";
		text += "</color>\n";
		text += "<color=green>\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[50], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[57], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[58], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[62], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[51], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[52], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[53], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[59], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[54], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[55], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[60], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenMonatLast[61], showDollar: true) + "\n";
		text += mS_.GetMoney(mS_.finanzenMonatLast[56], showDollar: true);
		text += "</color>";
		uiObjects[3].GetComponent<Text>().text = text;
		if (mS_.finanzenMonat_GetGewinn() < 0)
		{
			uiObjects[2].GetComponent<Text>().text = "<color=#A40000>" + mS_.GetMoney(mS_.finanzenMonat_GetGewinn(), showDollar: true) + "</color>";
		}
		else
		{
			uiObjects[2].GetComponent<Text>().text = "<color=green>" + mS_.GetMoney(mS_.finanzenMonat_GetGewinn(), showDollar: true) + "</color>";
		}
		if (mS_.finanzenMonatLast_GetGewinn() < 0)
		{
			uiObjects[4].GetComponent<Text>().text = "<color=#A40000>" + mS_.GetMoney(mS_.finanzenMonatLast_GetGewinn(), showDollar: true) + "</color>";
		}
		else
		{
			uiObjects[4].GetComponent<Text>().text = "<color=green>" + mS_.GetMoney(mS_.finanzenMonatLast_GetGewinn(), showDollar: true) + "</color>";
		}
	}

	private void Update()
	{
		if (mS_.multiplayer)
		{
			SetData();
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		if (!guiMain_.uiObjects[131].activeSelf)
		{
			guiMain_.CloseMenu();
		}
		base.gameObject.SetActive(value: false);
	}
}
