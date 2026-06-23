using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_FinanzJahr : MonoBehaviour
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

	private void Update()
	{
		if (mS_.multiplayer)
		{
			SetData();
		}
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
		text = text + mS_.GetMoney(mS_.finanzenJahr[0], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[1], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[2], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[3], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[4], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[5], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[12], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[17], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[13], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[14], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[18], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[6], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[7], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[15], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[8], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[10], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[11], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[16], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[9], showDollar: true) + "\n";
		text += "</color>\n";
		text += "<color=green>\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[50], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[57], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[58], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[62], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[51], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[52], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[53], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[59], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[54], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[55], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[60], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahr[61], showDollar: true) + "\n";
		text += mS_.GetMoney(mS_.finanzenJahr[56], showDollar: true);
		text += "</color>";
		uiObjects[1].GetComponent<Text>().text = text;
		text = "<color=red>\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[0], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[1], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[2], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[3], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[4], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[5], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[12], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[17], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[13], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[14], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[18], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[6], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[7], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[15], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[8], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[10], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[11], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[16], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[9], showDollar: true) + "\n";
		text += "</color>\n";
		text += "<color=green>\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[50], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[57], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[58], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[62], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[51], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[52], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[53], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[59], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[54], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[55], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[60], showDollar: true) + "\n";
		text = text + mS_.GetMoney(mS_.finanzenJahrLast[61], showDollar: true) + "\n";
		text += mS_.GetMoney(mS_.finanzenJahrLast[56], showDollar: true);
		text += "</color>";
		uiObjects[3].GetComponent<Text>().text = text;
		if (mS_.finanzenJahr_GetGewinn() < 0)
		{
			uiObjects[2].GetComponent<Text>().text = "<color=#A40000>" + mS_.GetMoney(mS_.finanzenJahr_GetGewinn(), showDollar: true) + "</color>";
		}
		else
		{
			uiObjects[2].GetComponent<Text>().text = "<color=green>" + mS_.GetMoney(mS_.finanzenJahr_GetGewinn(), showDollar: true) + "</color>";
		}
		if (mS_.finanzenJahrLast_GetGewinn() < 0)
		{
			uiObjects[4].GetComponent<Text>().text = "<color=#A40000>" + mS_.GetMoney(mS_.finanzenJahrLast_GetGewinn(), showDollar: true) + "</color>";
		}
		else
		{
			uiObjects[4].GetComponent<Text>().text = "<color=green>" + mS_.GetMoney(mS_.finanzenJahrLast_GetGewinn(), showDollar: true) + "</color>";
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
