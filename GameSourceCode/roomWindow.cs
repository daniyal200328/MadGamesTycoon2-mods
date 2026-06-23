using UnityEngine;
using UnityEngine.UI;

public class roomWindow : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject main_;

	public textScript tS_;

	public engineFeatures eF_;

	public gameplayFeatures gF_;

	public games games_;

	public GUI_Main guiMain_;

	private Menu_Marketing_GameKampagne scriptMarketing_;

	private contractWorkMain contractWorkMain_;

	private float window_ForschungTimer = 2f;

	private void Start()
	{
		FindScripts();
	}

	private void OnEnable()
	{
		if ((bool)uiObjects[1])
		{
			Image component = uiObjects[1].GetComponent<Image>();
			if ((bool)component)
			{
				component.fillAmount = 0.1f;
			}
		}
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!contractWorkMain_)
		{
			contractWorkMain_ = main_.GetComponent<contractWorkMain>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!scriptMarketing_)
		{
			scriptMarketing_ = guiMain_.uiObjects[89].GetComponent<Menu_Marketing_GameKampagne>();
		}
	}

	private void LerpBalken(GameObject go, float prozent)
	{
		if (!go)
		{
			return;
		}
		Image component = go.GetComponent<Image>();
		if ((bool)component)
		{
			if (prozent < component.fillAmount)
			{
				component.fillAmount = 0f;
			}
			else
			{
				component.fillAmount = Mathf.Lerp(component.fillAmount, prozent, 0.2f);
			}
		}
	}

	public void Window_Konsole(taskKonsole task_)
	{
		if (!task_ || !task_.mS_ || !task_.pS_ || !tS_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = task_.pS_.GetName();
		LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = task_.pS_.GetTypSprite();
		uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(task_.pS_.GetHype()).ToString();
		task_.pS_.SetPic(uiObjects[5]);
		uiObjects[6].GetComponent<Text>().text = task_.pS_.tech.ToString();
		uiObjects[8].GetComponent<Text>().text = task_.mS_.RoundString(task_.pS_.GetHaltbarkeit(), 1);
		if (task_.leitenderTechnikerID == -1)
		{
			if (!uiObjects[7].activeSelf)
			{
				uiObjects[7].SetActive(value: true);
			}
			else
			{
				uiObjects[7].transform.GetChild(0).GetComponent<Text>().text = tS_.GetText(1777);
			}
		}
		else if (uiObjects[7].activeSelf)
		{
			uiObjects[7].SetActive(value: false);
		}
	}

	public void Window_ArcadeProduction(taskArcadeProduction task_)
	{
		if (!task_ || !task_.mS_ || !tS_)
		{
			return;
		}
		if (!task_.produceAutomatikAllGames)
		{
			if (uiObjects[5].activeSelf)
			{
				uiObjects[5].SetActive(value: false);
			}
		}
		else if (!uiObjects[5].activeSelf)
		{
			uiObjects[5].SetActive(value: true);
		}
		if ((bool)task_.gS_)
		{
			if (!task_.produceAutomatikAllGames)
			{
				uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
			}
			else if (task_.gS_.vorbestellungen > 0)
			{
				uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
			}
			else
			{
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(476);
			}
			LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
			uiObjects[2].GetComponent<Text>().text = "(" + task_.mS_.GetMoney(task_.gS_.vorbestellungen, showDollar: false) + ") " + Round(task_.GetProzent(), 1) + "%";
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(2191);
			LerpBalken(uiObjects[1], 0f);
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(476);
		}
	}

	public void Window_Update(taskUpdate task_)
	{
		uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
		LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		if (uiObjects[4].activeSelf != task_.automatic)
		{
			uiObjects[4].SetActive(task_.automatic);
		}
		if (task_.automatic)
		{
			if (task_.autoAmount == 0)
			{
				uiObjects[5].GetComponent<Text>().text = "<size=30>∞</size>";
			}
			else
			{
				uiObjects[5].GetComponent<Text>().text = (task_.autoAmount - 2).ToString();
			}
		}
	}

	public void Window_F2PUpdate(taskF2PUpdate task_)
	{
		if (!task_ || !task_.gS_ || !tS_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
		LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		if (uiObjects[4].activeSelf != task_.automatic)
		{
			uiObjects[4].SetActive(task_.automatic);
		}
		if (task_.automatic)
		{
			if (task_.autoAmount == 0)
			{
				uiObjects[5].GetComponent<Text>().text = "<size=30>∞</size>";
			}
			else
			{
				uiObjects[5].GetComponent<Text>().text = (task_.autoAmount - 2).ToString();
			}
		}
	}

	public void Window_Wait(taskWait task_)
	{
		if ((bool)task_ && (bool)tS_ && task_.art == 0)
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1699);
			uiObjects[1].GetComponent<Image>().sprite = task_.GetPic();
		}
	}

	public void Window_ContractWorkWait(taskContractWait task_)
	{
		_ = (bool)task_;
	}

	public void Window_ContractWork(taskContractWork task_, roomScript rS_)
	{
		if (!task_ || !task_.mS_ || !contractWorkMain_ || !rS_)
		{
			return;
		}
		if (rS_.typ != 14)
		{
			uiObjects[0].GetComponent<Text>().text = task_.GetName();
			float prozent = task_.GetProzent();
			LerpBalken(uiObjects[1], prozent * 0.01f);
			uiObjects[2].GetComponent<Text>().text = Round(prozent, 1) + "%";
		}
		if (rS_.typ == 14)
		{
			uiObjects[0].GetComponent<Text>().text = task_.GetName();
			float prozent2 = task_.GetProzent();
			LerpBalken(uiObjects[1], prozent2 * 0.01f);
			uiObjects[2].GetComponent<Text>().text = task_.mS_.GetMoney(Mathf.RoundToInt(task_.pointsLeft), showDollar: false);
		}
		uiObjects[5].GetComponent<Text>().text = task_.contract_.GetWochen().ToString();
		if (uiObjects[6].activeSelf != task_.automatic)
		{
			uiObjects[6].SetActive(task_.automatic);
		}
		if (task_.automatic)
		{
			switch (rS_.typ)
			{
			case 1:
				uiObjects[7].GetComponent<Text>().text = contractWorkMain_.anzDEV.ToString();
				break;
			case 3:
				uiObjects[7].GetComponent<Text>().text = contractWorkMain_.anzQA.ToString();
				break;
			case 4:
				uiObjects[7].GetComponent<Text>().text = contractWorkMain_.anzGFX.ToString();
				break;
			case 5:
				uiObjects[7].GetComponent<Text>().text = contractWorkMain_.anzSFX.ToString();
				break;
			case 10:
				uiObjects[7].GetComponent<Text>().text = contractWorkMain_.anzMotion.ToString();
				break;
			case 14:
				uiObjects[7].GetComponent<Text>().text = contractWorkMain_.anzProduction.ToString();
				break;
			case 17:
				uiObjects[7].GetComponent<Text>().text = contractWorkMain_.anzWerkstatt.ToString();
				break;
			case 8:
				uiObjects[7].GetComponent<Text>().text = contractWorkMain_.anzHardware.ToString();
				break;
			}
		}
	}

	public void Window_Training(string name_, float prozent, Sprite icon, bool automatic)
	{
		uiObjects[0].GetComponent<Text>().text = name_;
		LerpBalken(uiObjects[1], prozent * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(prozent, 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = icon;
		if (uiObjects[4].activeSelf != automatic)
		{
			uiObjects[4].SetActive(automatic);
		}
	}

	public void Window_Anrufe(taskSupport task_)
	{
		if ((bool)task_ && (bool)task_.mS_ && (bool)tS_)
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(746);
			LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
			uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		}
	}

	public void Window_Fanshop(taskFanshop task_)
	{
		if (!task_ || !task_.mS_ || !tS_)
		{
			return;
		}
		string text = tS_.GetText(200);
		text = text.Replace("<NUM>", task_.mS_.GetMoney(task_.verdienst, showDollar: true));
		uiObjects[2].GetComponent<Text>().text = text;
		float num = task_.mS_.week - 1;
		uiObjects[1].GetComponent<Image>().fillAmount = num * 0.25f + task_.mS_.dayTimer * 0.25f;
		for (int i = 0; i < task_.bestellungen.Length; i++)
		{
			if (task_.bestellungen[i] <= 0)
			{
				if (uiObjects[4 + i].activeSelf)
				{
					uiObjects[4 + i].SetActive(value: false);
				}
				continue;
			}
			if (!uiObjects[4 + i].activeSelf)
			{
				uiObjects[4 + i].SetActive(value: true);
			}
			uiObjects[12 + i].GetComponent<Text>().text = task_.mS_.GetMoney(task_.bestellungen[i], showDollar: false);
		}
	}

	public void Window_Bugfixing(taskBugfixing task_)
	{
		if ((bool)task_ && (bool)task_.mS_ && (bool)task_.gS_ && (bool)tS_)
		{
			uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
			LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
			uiObjects[2].GetComponent<Text>().text = task_.mS_.GetMoney(Mathf.RoundToInt(task_.gS_.points_bugs), showDollar: false) + " " + tS_.GetText(424);
		}
	}

	public void Window_Polishing(taskPolishing task_)
	{
		if ((bool)task_ && (bool)task_.mS_ && (bool)task_.gS_ && (bool)tS_)
		{
			uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
			LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
			uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		}
	}

	public void Window_Lagerhaus(roomScript rS_)
	{
		if ((bool)rS_ && (bool)rS_.mS_ && (bool)guiMain_)
		{
			long lagerplatz = rS_.GetLagerplatz();
			uiObjects[0].GetComponent<Text>().text = rS_.mS_.GetMoney(rS_.lagerplatzUsed, showDollar: false) + " / " + rS_.mS_.GetMoney(lagerplatz, showDollar: false);
			if (rS_.lagerplatzUsed >= lagerplatz)
			{
				uiObjects[0].GetComponent<Text>().color = guiMain_.colors[8];
			}
			else
			{
				uiObjects[0].GetComponent<Text>().color = Color.white;
			}
			float num = lagerplatz;
			if (lagerplatz > 0)
			{
				num *= 0.01f;
				num = (float)rS_.lagerplatzUsed / num;
			}
			else
			{
				num = 0f;
			}
			LerpBalken(uiObjects[1], num * 0.01f);
			uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(num) + "%";
		}
	}

	public void Window_Serverraum(roomScript rS_)
	{
		if (!rS_ || !rS_.mS_ || !guiMain_)
		{
			return;
		}
		long serverplatz = rS_.GetServerplatz();
		uiObjects[0].GetComponent<Text>().text = rS_.mS_.GetMoney(rS_.serverplatzUsed, showDollar: false) + " / " + rS_.mS_.GetMoney(serverplatz, showDollar: false);
		if (rS_.serverplatzUsed >= serverplatz || rS_.serverDown)
		{
			uiObjects[0].GetComponent<Text>().color = guiMain_.colors[8];
		}
		else
		{
			uiObjects[0].GetComponent<Text>().color = Color.white;
		}
		switch (rS_.serverReservieren)
		{
		case 0:
			if (uiObjects[4].activeSelf)
			{
				uiObjects[4].SetActive(value: false);
			}
			break;
		case 1:
			if (!uiObjects[4].activeSelf)
			{
				uiObjects[4].SetActive(value: true);
			}
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[61];
			break;
		case 2:
			if (!uiObjects[4].activeSelf)
			{
				uiObjects[4].SetActive(value: true);
			}
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[62];
			break;
		case 3:
			if (!uiObjects[4].activeSelf)
			{
				uiObjects[4].SetActive(value: true);
			}
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[63];
			break;
		}
		if (rS_.serverDown)
		{
			if ((bool)tS_)
			{
				uiObjects[1].GetComponent<Image>().fillAmount = 0f;
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(1242);
				if (!uiObjects[3].activeSelf)
				{
					uiObjects[3].SetActive(value: true);
				}
				if (uiObjects[4].activeSelf)
				{
					uiObjects[4].SetActive(value: false);
				}
				if (uiObjects[5].activeSelf)
				{
					uiObjects[5].SetActive(value: false);
				}
			}
			return;
		}
		if (uiObjects[3].activeSelf)
		{
			uiObjects[3].SetActive(value: false);
		}
		float num = serverplatz;
		if (serverplatz > 0)
		{
			num *= 0.01f;
			num = (float)rS_.serverplatzUsed / num;
		}
		else
		{
			num = 0f;
		}
		LerpBalken(uiObjects[1], num * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(num) + "%";
		bool serverOverheat = rS_.serverOverheat;
		if (uiObjects[5].activeSelf != serverOverheat)
		{
			uiObjects[5].SetActive(serverOverheat);
			if (uiObjects[5].activeSelf)
			{
				uiObjects[5].transform.GetChild(0).GetComponent<Text>().text = tS_.GetText(1260);
			}
		}
	}

	public void Window_Production(taskProduction task_)
	{
		if (!task_ || !task_.mS_ || !tS_)
		{
			return;
		}
		if (!task_.produceAutomatikAllGames)
		{
			if ((bool)task_.gS_)
			{
				uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
				LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
				if (task_.WaitForAutomatic())
				{
					uiObjects[2].GetComponent<Text>().text = tS_.GetText(476);
				}
				else
				{
					uiObjects[2].GetComponent<Text>().text = task_.mS_.GetMoney(task_.GetAmount(), showDollar: false);
				}
				if (uiObjects[4].activeSelf != task_.automatic)
				{
					uiObjects[4].SetActive(task_.automatic);
				}
				if (uiObjects[6].activeSelf)
				{
					uiObjects[6].SetActive(value: false);
				}
			}
		}
		else
		{
			if (uiObjects[4].activeSelf)
			{
				uiObjects[4].SetActive(value: false);
			}
			if (!uiObjects[6].activeSelf)
			{
				uiObjects[6].SetActive(value: true);
			}
			if ((bool)task_.gS_)
			{
				uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
				uiObjects[2].GetComponent<Text>().text = task_.mS_.GetMoney(task_.GetAmount(), showDollar: false);
				LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
			}
			else
			{
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(2191);
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(476);
				LerpBalken(uiObjects[1], 0f);
			}
		}
		bool flag = true;
		if (games_.GetFreeLagerplatz() > 0)
		{
			flag = false;
		}
		if (uiObjects[5].activeSelf != flag)
		{
			uiObjects[5].SetActive(flag);
			if (uiObjects[5].activeSelf)
			{
				uiObjects[5].transform.GetChild(0).GetComponent<Text>().text = tS_.GetText(1147);
			}
		}
	}

	public void Window_Spielbericht(taskSpielbericht task_)
	{
		if ((bool)task_ && (bool)task_.mS_ && (bool)task_.gS_ && (bool)guiMain_)
		{
			uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
			LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
			uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
			if (uiObjects[6].activeSelf != task_.automatic)
			{
				uiObjects[6].SetActive(task_.automatic);
			}
			if (task_.automatic)
			{
				uiObjects[7].GetComponent<Text>().text = guiMain_.uiObjects[181].GetComponent<Menu_QA_NewSpielberichtSelectGame>().GetNumSpielberichteCanCreate().ToString();
			}
		}
	}

	public void Window_GameplayVerbessern(taskGameplayVerbessern task_)
	{
		if (!task_ || !task_.mS_ || !task_.gS_ || !games_ || !guiMain_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
		LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = task_.GetPic();
		int num = 0;
		for (int i = 0; i < task_.adds.Length; i++)
		{
			uiObjects[4 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			if (task_.adds[i] && task_.aktuellerAdd != i)
			{
				uiObjects[4 + num].GetComponent<Image>().sprite = games_.gameAdds[i];
				num++;
			}
		}
	}

	public void Window_GrafikVerbessern(taskGrafikVerbessern task_)
	{
		if (!task_ || !task_.mS_ || !task_.gS_ || !games_ || !guiMain_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
		LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = task_.GetPic();
		int num = 0;
		for (int i = 0; i < task_.adds.Length; i++)
		{
			uiObjects[4 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			if (task_.adds[i] && task_.aktuellerAdd != i)
			{
				uiObjects[4 + num].GetComponent<Image>().sprite = games_.gameAdds[i + 6];
				num++;
			}
		}
	}

	public void Window_SoundVerbessern(taskSoundVerbessern task_)
	{
		if (!task_ || !task_.mS_ || !task_.gS_ || !games_ || !guiMain_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
		LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = task_.GetPic();
		int num = 0;
		for (int i = 0; i < task_.adds.Length; i++)
		{
			uiObjects[4 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			if (task_.adds[i] && task_.aktuellerAdd != i)
			{
				uiObjects[4 + num].GetComponent<Image>().sprite = games_.gameAdds[i + 12];
				num++;
			}
		}
	}

	public void Window_AnimationVerbessern(taskAnimationVerbessern task_)
	{
		if (!task_ || !task_.mS_ || !task_.gS_ || !games_ || !guiMain_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
		LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = task_.GetPic();
		int num = 0;
		for (int i = 0; i < task_.adds.Length; i++)
		{
			uiObjects[4 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			if (task_.adds[i] && task_.aktuellerAdd != i)
			{
				uiObjects[4 + num].GetComponent<Image>().sprite = games_.gameAdds[i + 18];
				num++;
			}
		}
	}

	public void Window_KonsoleReduceCosts(taskKonsoleReduceCosts task_)
	{
		if (!task_ || !task_.mS_ || !task_.pS_ || !task_.menu_ || !games_ || !guiMain_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = task_.pS_.GetName();
		LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = task_.GetPic();
		task_.pS_.SetPic(uiObjects[13]);
		int num = 0;
		for (int i = 0; i < task_.adds.Length; i++)
		{
			uiObjects[4 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			if (task_.adds[i] && task_.aktuellerAdd != i)
			{
				uiObjects[4 + num].GetComponent<Image>().sprite = task_.menu_.sprites[i];
				num++;
			}
		}
	}

	public void Window_KonsoleHaltbarkeit(taskKonsoleHaltbarkeit task_)
	{
		if (!task_ || !task_.mS_ || !task_.pS_ || !task_.menu_ || !games_ || !guiMain_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = task_.pS_.GetName();
		LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = task_.GetPic();
		task_.pS_.SetPic(uiObjects[13]);
		int num = 0;
		for (int i = 0; i < task_.adds.Length; i++)
		{
			uiObjects[4 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			if (task_.adds[i] && task_.aktuellerAdd != i)
			{
				uiObjects[4 + num].GetComponent<Image>().sprite = task_.menu_.sprites[i];
				num++;
			}
		}
	}

	public void Window_Fankampagne(taskFankampagne task_)
	{
		if ((bool)task_ && (bool)task_.mS_ && (bool)tS_)
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(task_.kampagne + 740);
			LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
			uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
			uiObjects[3].GetComponent<Image>().sprite = task_.GetPic();
			if (uiObjects[4].activeSelf != task_.automatic)
			{
				uiObjects[4].SetActive(task_.automatic);
			}
		}
	}

	public void Window_Mitarbeitersuche(taskMitarbeitersuche task_)
	{
		if ((bool)task_ && (bool)task_.mS_ && (bool)tS_)
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(137 + task_.beruf);
			LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
			uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
			uiObjects[5].GetComponent<Image>().sprite = guiMain_.Get3Stars(task_.berufserfahrung);
			if (uiObjects[4].activeSelf != task_.automatic)
			{
				uiObjects[4].SetActive(task_.automatic);
			}
		}
	}

	public void Window_Marktforschung(taskMarktforschung task_)
	{
		if ((bool)task_ && (bool)task_.mS_)
		{
			LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
			uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
		}
	}

	public void Window_MarketingSpezial(taskMarketingSpezial task_)
	{
		if ((bool)task_ && (bool)task_.mS_ && (bool)task_.gS_ && (bool)tS_)
		{
			uiObjects[0].GetComponent<Text>().text = task_.gS_.GetNameWithTag();
			LerpBalken(uiObjects[1], task_.GetProzent() * 0.01f);
			uiObjects[2].GetComponent<Text>().text = Round(task_.GetProzent(), 1) + "%";
			uiObjects[3].GetComponent<Image>().sprite = task_.GetPic();
		}
	}

	public void Window_Marketing(string name_, float prozent, Sprite icon, taskMarketing taskMarketing_)
	{
		if (!tS_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = name_;
		LerpBalken(uiObjects[1], prozent * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(prozent, 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = icon;
		if (taskMarketing_.typ == 0)
		{
			if ((bool)taskMarketing_.gS_)
			{
				uiObjects[6].GetComponent<Image>().sprite = taskMarketing_.gS_.GetTypSprite();
			}
		}
		else if ((bool)taskMarketing_.pS_)
		{
			uiObjects[6].GetComponent<Image>().sprite = taskMarketing_.pS_.GetTypSprite();
		}
		if (uiObjects[4].activeSelf != taskMarketing_.automatic)
		{
			uiObjects[4].SetActive(taskMarketing_.automatic);
		}
		bool flag = taskMarketing_.WaitForMinimumHype();
		if (uiObjects[5].activeSelf != flag)
		{
			uiObjects[5].SetActive(flag);
		}
		if (uiObjects[5].activeSelf)
		{
			string text = tS_.GetText(726);
			text = text.Replace("<NUM>", "<color=blue>" + (scriptMarketing_.maxHype[taskMarketing_.kampagne] - scriptMarketing_.hypeProKampagne[taskMarketing_.kampagne]) + "</color>");
			uiObjects[5].transform.GetChild(0).GetComponent<Text>().text = text;
		}
	}

	public void Window_AutoForschung(taskAutoForschung task_)
	{
		if (!task_ || !tS_ || !guiMain_)
		{
			return;
		}
		for (int i = 0; i < task_.kategorie.Length; i++)
		{
			if (!task_.kategorie[i])
			{
				if (uiObjects[i + 6].activeSelf)
				{
					uiObjects[i + 6].SetActive(value: false);
				}
			}
			else if (!uiObjects[i + 6].activeSelf)
			{
				uiObjects[i + 6].SetActive(value: true);
			}
		}
	}

	public void Window_ForschungWait(taskForschungWait task_)
	{
		if ((bool)task_ && (bool)tS_ && (bool)guiMain_)
		{
			string text = tS_.GetText(2271);
			switch (task_.typ)
			{
			case 0:
				text = text.Replace("<NAME>", tS_.GetText(158));
				uiObjects[1].GetComponent<Text>().text = text;
				uiObjects[0].GetComponent<Image>().sprite = guiMain_.uiSprites[67];
				break;
			case 1:
				text = text.Replace("<NAME>", tS_.GetText(159));
				uiObjects[1].GetComponent<Text>().text = text;
				uiObjects[0].GetComponent<Image>().sprite = guiMain_.uiSprites[6];
				break;
			case 2:
				text = text.Replace("<NAME>", tS_.GetText(160));
				uiObjects[1].GetComponent<Text>().text = text;
				uiObjects[0].GetComponent<Image>().sprite = guiMain_.uiSprites[4];
				break;
			case 3:
				text = text.Replace("<NAME>", tS_.GetText(161));
				uiObjects[1].GetComponent<Text>().text = text;
				uiObjects[0].GetComponent<Image>().sprite = guiMain_.uiSprites[68];
				break;
			case 4:
				text = text.Replace("<NAME>", tS_.GetText(162));
				uiObjects[1].GetComponent<Text>().text = text;
				uiObjects[0].GetComponent<Image>().sprite = guiMain_.uiSprites[69];
				break;
			case 5:
				text = text.Replace("<NAME>", tS_.GetText(163));
				uiObjects[1].GetComponent<Text>().text = text;
				uiObjects[0].GetComponent<Image>().sprite = guiMain_.uiSprites[71];
				break;
			case 6:
				text = text.Replace("<NAME>", tS_.GetText(1617));
				uiObjects[1].GetComponent<Text>().text = text;
				uiObjects[0].GetComponent<Image>().sprite = guiMain_.uiSprites[70];
				break;
			}
		}
	}

	public void Window_Forschung(string name_, float prozent, Sprite icon, taskForschung task_)
	{
		if (!guiMain_ || !task_ || !tS_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = name_;
		LerpBalken(uiObjects[1], prozent * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(prozent, 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = icon;
		if (uiObjects[6].activeSelf != task_.automatic)
		{
			uiObjects[6].SetActive(task_.automatic);
		}
		if (task_.automatic)
		{
			window_ForschungTimer += Time.deltaTime;
			if (window_ForschungTimer < 1f)
			{
				return;
			}
			window_ForschungTimer = 0f;
			uiObjects[7].GetComponent<Text>().text = guiMain_.uiObjects[21].GetComponent<Menu_Forschung>().GetAmountForschung(task_.typ, getUnerforschtesObjekt: false).ToString();
		}
		if (uiObjects[8].activeSelf != task_.autoForschung)
		{
			uiObjects[8].SetActive(task_.autoForschung);
		}
		if (task_.autoForschung)
		{
			window_ForschungTimer += Time.deltaTime;
			if (window_ForschungTimer < 1f)
			{
				return;
			}
			window_ForschungTimer = 0f;
			int num = 0;
			Menu_Forschung component = guiMain_.uiObjects[21].GetComponent<Menu_Forschung>();
			if ((bool)component)
			{
				for (int i = 0; i < task_.kategorie.Length; i++)
				{
					if (task_.kategorie[i])
					{
						num += component.GetAmountForschung(i, getUnerforschtesObjekt: false);
					}
				}
			}
			uiObjects[9].GetComponent<Text>().text = num.ToString();
			if (!uiObjects[17].activeSelf)
			{
				uiObjects[17].SetActive(value: true);
			}
			for (int j = 0; j < task_.kategorie.Length; j++)
			{
				if (!task_.kategorie[j])
				{
					if (uiObjects[j + 10].activeSelf)
					{
						uiObjects[j + 10].SetActive(value: false);
					}
				}
				else if (!uiObjects[j + 10].activeSelf)
				{
					uiObjects[j + 10].SetActive(value: true);
				}
			}
		}
		else if (uiObjects[17].activeSelf)
		{
			uiObjects[17].SetActive(value: false);
		}
	}

	public void Window_DevEngine(string name_, float prozent, Sprite icon)
	{
		uiObjects[0].GetComponent<Text>().text = name_;
		LerpBalken(uiObjects[1], prozent * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(prozent, 1) + "%";
		uiObjects[3].GetComponent<Image>().sprite = icon;
	}

	public void Window_DevGame(gameScript gS_, taskGame task_)
	{
		if (!main_ || !tS_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[11].GetComponent<Image>().sprite = gS_.GetTypSprite();
		uiObjects[14].GetComponent<Image>().sprite = gS_.GetPlatformTypSprite();
		uiObjects[10].GetComponent<Text>().text = Mathf.RoundToInt(gS_.GetHype()).ToString();
		LerpBalken(uiObjects[1], gS_.GetProzentGesamt() * 0.01f);
		uiObjects[2].GetComponent<Text>().text = Round(gS_.GetProzentGesamt(), 1) + "%";
		LerpBalken(uiObjects[9], gS_.GetProzentFeature() * 0.01f);
		uiObjects[3].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_gameplay).ToString();
		uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_grafik).ToString();
		uiObjects[5].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_sound).ToString();
		uiObjects[6].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_technik).ToString();
		uiObjects[7].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_bugs).ToString();
		if (gS_.devPoints_Gesamt > 0f)
		{
			if (gS_.devAktFeature != -5)
			{
				if (gS_.devAktFeature < 0)
				{
					uiObjects[8].GetComponent<Text>().text = eF_.GetName(gS_.gameEngineFeature[gS_.devAktFeature + 4]);
					if (!uiObjects[16].activeSelf)
					{
						uiObjects[16].SetActive(value: true);
					}
					guiMain_.DrawStarsColor(uiObjects[16], eF_.engineFeatures_LEVEL[gS_.gameEngineFeature[gS_.devAktFeature + 4]], Color.white);
				}
				else
				{
					uiObjects[8].GetComponent<Text>().text = gF_.GetName(gS_.devAktFeature);
					if (!uiObjects[16].activeSelf)
					{
						uiObjects[16].SetActive(value: true);
					}
					guiMain_.DrawStarsColor(uiObjects[16], gF_.gameplayFeatures_LEVEL[gS_.devAktFeature], Color.white);
				}
			}
			if (gS_.devAktFeature == -5)
			{
				uiObjects[8].GetComponent<Text>().text = tS_.GetText(976);
				if (uiObjects[16].activeSelf)
				{
					uiObjects[16].SetActive(value: false);
				}
			}
			if (uiObjects[17].activeSelf)
			{
				uiObjects[17].SetActive(value: false);
			}
		}
		else
		{
			uiObjects[8].GetComponent<Text>().text = tS_.GetText(1046);
			uiObjects[9].GetComponent<Image>().fillAmount = 1f;
			if (uiObjects[16].activeSelf)
			{
				uiObjects[16].SetActive(value: false);
			}
			if (!uiObjects[17].activeSelf)
			{
				uiObjects[17].SetActive(value: true);
			}
			if (gS_.devFortsetzen > 1)
			{
				uiObjects[18].GetComponent<Text>().text = (gS_.devFortsetzen - 1).ToString();
			}
			else
			{
				uiObjects[18].GetComponent<Text>().text = "<size=25>∞</size>";
			}
		}
		if (gS_.typ_contractGame)
		{
			if (!uiObjects[12].activeSelf)
			{
				uiObjects[12].SetActive(value: true);
			}
			uiObjects[13].GetComponent<Text>().text = gS_.GetContractGameAbgabetermin().ToString();
		}
		else if (uiObjects[12].activeSelf)
		{
			uiObjects[12].SetActive(value: false);
		}
		if (task_.leitenderDesignerID == -1)
		{
			if (!uiObjects[15].activeSelf)
			{
				uiObjects[15].SetActive(value: true);
			}
			else
			{
				uiObjects[15].transform.GetChild(0).GetComponent<Text>().text = tS_.GetText(1000);
			}
		}
		else if (uiObjects[15].activeSelf)
		{
			uiObjects[15].SetActive(value: false);
		}
	}

	public void Window_Unterstuetzen(string roomName, float prozent)
	{
		if ((bool)tS_)
		{
			if (prozent >= 0f)
			{
				LerpBalken(uiObjects[1], prozent * 0.01f);
				uiObjects[2].GetComponent<Text>().text = Round(prozent, 1) + "%";
			}
			else
			{
				uiObjects[1].GetComponent<Image>().fillAmount = 0f;
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(476);
			}
		}
	}

	private float Round(float value, int digits)
	{
		float num = Mathf.Pow(10f, digits);
		return Mathf.Round(value * num) / num;
	}
}
