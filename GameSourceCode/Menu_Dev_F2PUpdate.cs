using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_F2PUpdate : MonoBehaviour
{
	public GameObject[] uiObjects;

	public float[] devCostsPercent;

	private bool[] buttonAdds = new bool[12];

	public float[] interestBoost;

	public GameObject[] goButtons;

	private int amount;

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

	private gameScript gS_;

	private roomScript rS_;

	private bool allAdds;

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
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(roomScript roomScript_, gameScript gameScript_)
	{
		FindScripts();
		rS_ = roomScript_;
		gS_ = gameScript_;
		allAdds = false;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			buttonAdds[i] = false;
		}
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[1].GetComponent<Image>().fillAmount = gS_.f2pInteresse * 0.01f;
		uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(gS_.f2pInteresse) + "%";
		for (int j = 0; j < goButtons.Length; j++)
		{
			if ((bool)goButtons[j])
			{
				long num = gS_.GetGesamteEntwicklungskosten();
				if (num > 100000000)
				{
					num = 100000000L;
				}
				goButtons[j].transform.GetChild(3).GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[j]), showDollar: true);
			}
		}
		UpdateGUI();
	}

	private void UpdateGUI()
	{
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				goButtons[i].GetComponent<Image>().color = guiMain_.colors[7];
			}
			else
			{
				goButtons[i].GetComponent<Image>().color = Color.white;
			}
		}
		uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true);
		if (amount <= 0)
		{
			string text = tS_.GetText(2065);
			text = text.Replace("<size=20>", "<size=30>");
			uiObjects[5].GetComponent<Text>().text = text;
		}
		else
		{
			uiObjects[5].GetComponent<Text>().text = "x" + amount;
		}
	}

	private long GetDevCosts()
	{
		long num = 0L;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				long num2 = gS_.GetGesamteEntwicklungskosten();
				if (num2 > 100000000)
				{
					num2 = 100000000L;
				}
				num += Mathf.RoundToInt((float)num2 * devCostsPercent[i]);
			}
		}
		return num;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[299]);
		guiMain_.uiObjects[299].GetComponent<Menu_Dev_F2PUpdateSelectGame>().Init(rS_);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Adds(int i)
	{
		sfx_.PlaySound(3, force: true);
		buttonAdds[i] = !buttonAdds[i];
		UpdateGUI();
	}

	public void BUTTON_AlleAdds()
	{
		sfx_.PlaySound(3, force: true);
		allAdds = !allAdds;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			buttonAdds[i] = allAdds;
		}
		UpdateGUI();
	}

	public void BUTTON_Start()
	{
		int num = Mathf.RoundToInt(GetDevCosts());
		if (!gS_ || !rS_)
		{
			return;
		}
		if (num <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(662), closeMenu: false);
			return;
		}
		if (uiObjects[4].GetComponent<Toggle>().isOn)
		{
			bool flag = false;
			for (int i = 0; i < buttonAdds.Length; i++)
			{
				if (buttonAdds[i])
				{
					flag = true;
				}
			}
			if (!flag)
			{
				guiMain_.MessageBox(tS_.GetText(727), closeMenu: false);
				return;
			}
		}
		sfx_.PlaySound(3, force: true);
		mS_.Pay(num, 15);
		taskF2PUpdate taskF2PUpdate2 = guiMain_.AddTask_F2PUpdate();
		taskF2PUpdate2.Init(fromSavegame: false);
		taskF2PUpdate2.targetID = gS_.myID;
		taskF2PUpdate2.devCosts = Mathf.RoundToInt(GetDevCosts());
		taskF2PUpdate2.automatic = uiObjects[4].GetComponent<Toggle>().isOn;
		if (amount > 0)
		{
			taskF2PUpdate2.autoAmount = amount + 1;
		}
		float num2 = gS_.GetGesamtDevPoints();
		num2 *= 0.1f;
		taskF2PUpdate2.points = Mathf.RoundToInt(num2);
		for (int j = 0; j < buttonAdds.Length; j++)
		{
			if (buttonAdds[j])
			{
				taskF2PUpdate2.quality += interestBoost[j];
				taskF2PUpdate2.points += Mathf.RoundToInt((float)gS_.GetGesamtDevPoints() * interestBoost[j] * 0.02f);
			}
		}
		taskF2PUpdate2.pointsLeft = taskF2PUpdate2.points;
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskF2PUpdate2.myID;
		}
		guiMain_.uiObjects[299].SetActive(value: false);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	private IEnumerator iMinus(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_Minus(i);
		}
	}

	public void BUTTON_Minus(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			amount -= 10;
		}
		else
		{
			amount--;
		}
		if (amount < 0)
		{
			amount = 0;
		}
		StartCoroutine(iMinus(i));
		UpdateGUI();
	}

	private IEnumerator iPlus(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_Plus(i);
		}
	}

	public void BUTTON_Plus(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			amount += 10;
		}
		else
		{
			amount++;
		}
		if (amount > 99)
		{
			amount = 99;
		}
		StartCoroutine(iPlus(i));
		UpdateGUI();
	}
}
