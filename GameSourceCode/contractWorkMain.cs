using UnityEngine;
using UnityEngine.UI;

public class contractWorkMain : MonoBehaviour
{
	public float pointsRegulator = 1f;

	public int anzContractGames;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private roomDataScript rdS_;

	private forschungSonstiges fS_;

	private genres genres_;

	private unlockScript unlock_;

	private forschungSonstiges forschungSonstiges_;

	private platforms platforms_;

	public int anzDEV;

	public int anzQA;

	public int anzGFX;

	public int anzSFX;

	public int anzMotion;

	public int anzProduction;

	public int anzWerkstatt;

	public int anzHardware;

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
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!rdS_)
		{
			rdS_ = main_.GetComponent<roomDataScript>();
		}
		if (!fS_)
		{
			fS_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
	}

	public contractAuftragsspiel CreateContractGame()
	{
		contractAuftragsspiel component = Object.Instantiate(uiPrefabs[1]).GetComponent<contractAuftragsspiel>();
		component.main_ = main_;
		component.mS_ = mS_;
		component.tS_ = tS_;
		return component;
	}

	public contractWork CreateContractWork()
	{
		contractWork component = Object.Instantiate(uiPrefabs[0]).GetComponent<contractWork>();
		component.main_ = main_;
		component.mS_ = mS_;
		component.tS_ = tS_;
		return component;
	}

	public void UpdateContractWork(bool forceNewContract)
	{
		FindScripts();
		GameObject[] array = GameObject.FindGameObjectsWithTag("ContractWork");
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					continue;
				}
				contractWork component = array[i].GetComponent<contractWork>();
				if (!component)
				{
					continue;
				}
				if (component.IsAngenommen())
				{
					component.zeitInWochen--;
					if (component.zeitInWochen < 0)
					{
						guiMain_.UpdateAuftragsansehen(0f - component.GetAuftragsansehen());
						mS_.Pay(Mathf.RoundToInt(component.GetStrafe()), 14);
						string text = tS_.GetText(610);
						text = text.Replace("<NAME>", "<b><color=blue>" + component.GetName() + "</color></b>");
						text = text.Replace("<NUM>", "<b><color=red>" + mS_.GetMoney(component.GetStrafe(), showDollar: true) + "</color></b>");
						switch (component.art)
						{
						case 0:
							guiMain_.CreateLeftNews(-1, guiMain_.uiSprites[12], text, rdS_.roomData_SPRITE[1]);
							break;
						case 1:
							guiMain_.CreateLeftNews(-1, guiMain_.uiSprites[12], text, rdS_.roomData_SPRITE[3]);
							break;
						case 2:
							guiMain_.CreateLeftNews(-1, guiMain_.uiSprites[12], text, rdS_.roomData_SPRITE[4]);
							break;
						case 3:
							guiMain_.CreateLeftNews(-1, guiMain_.uiSprites[12], text, rdS_.roomData_SPRITE[5]);
							break;
						case 4:
							guiMain_.CreateLeftNews(-1, guiMain_.uiSprites[12], text, rdS_.roomData_SPRITE[10]);
							break;
						case 5:
							guiMain_.CreateLeftNews(-1, guiMain_.uiSprites[12], text, rdS_.roomData_SPRITE[14]);
							break;
						case 6:
							guiMain_.CreateLeftNews(-1, guiMain_.uiSprites[12], text, rdS_.roomData_SPRITE[17]);
							break;
						case 7:
							guiMain_.CreateLeftNews(-1, guiMain_.uiSprites[12], text, rdS_.roomData_SPRITE[8]);
							break;
						}
						Object.Destroy(array[i]);
					}
				}
				else
				{
					component.wochenAlsAngebot++;
					if (component.wochenAlsAngebot > 16 && Random.Range(0, 100) > 90)
					{
						Object.Destroy(array[i]);
					}
				}
			}
		}
		if (mS_.globalEvent != 2 && ((array.Length < 20 && (float)Random.Range(0, 100) > 80f - mS_.auftragsAnsehen * 0.5f) || forceNewContract))
		{
			contractWork contractWork2 = CreateContractWork();
			contractWork2.myID = Random.Range(1, 999999999);
			switch (Random.Range(0, 8))
			{
			case 0:
				contractWork2.art = 0;
				break;
			case 1:
				if (forschungSonstiges_.IsErforscht(28))
				{
					contractWork2.art = 1;
				}
				break;
			case 2:
				if (forschungSonstiges_.IsErforscht(31))
				{
					contractWork2.art = 2;
				}
				break;
			case 3:
				if (forschungSonstiges_.IsErforscht(32))
				{
					contractWork2.art = 3;
				}
				break;
			case 4:
				if (unlock_.Get(8))
				{
					contractWork2.art = 4;
				}
				break;
			case 5:
				if (forschungSonstiges_.IsErforscht(33))
				{
					contractWork2.art = 5;
				}
				break;
			case 6:
				if (forschungSonstiges_.IsErforscht(38))
				{
					contractWork2.art = 6;
				}
				break;
			case 7:
				if (forschungSonstiges_.IsErforscht(39))
				{
					contractWork2.art = 7;
				}
				break;
			}
			contractWork2.angenommen = false;
			contractWork2.wochenAlsAngebot = 0;
			if (contractWork2.art != 5 && contractWork2.art != 6)
			{
				contractWork2.typ = tS_.GetRandomContractNumber(contractWork2.art);
			}
			contractWork2.points = 20 * Random.Range(10, 30 + Mathf.RoundToInt(mS_.auftragsAnsehen * 5f));
			contractWork2.gehalt = Mathf.RoundToInt(contractWork2.points * (6f - (float)mS_.difficulty)) * 13;
			contractWork2.strafe = Mathf.RoundToInt(Random.Range((float)contractWork2.gehalt * 0.1f, (float)contractWork2.gehalt * 0.3f));
			contractWork2.auftraggeberID = GetRandomPublisherID();
			if (contractWork2.typ == 25 && !unlock_.Get(31))
			{
				contractWork2.typ = 6;
			}
			switch (contractWork2.art)
			{
			case 1:
				contractWork2.points *= 0.8f;
				break;
			case 2:
				contractWork2.points *= 0.6f;
				break;
			case 3:
				contractWork2.points *= 0.4f;
				break;
			case 4:
				contractWork2.points *= 0.3f;
				break;
			case 5:
			{
				contractWork2.points *= 1000f;
				int num = Mathf.RoundToInt(contractWork2.points) / 100 * 100;
				contractWork2.points = num;
				break;
			}
			case 6:
				contractWork2.points *= 0.8f;
				break;
			case 7:
				contractWork2.points *= 0.8f;
				break;
			}
			contractWork2.points *= pointsRegulator;
			if (contractWork2.art != 5)
			{
				contractWork2.zeitInWochen = Mathf.RoundToInt(contractWork2.points / 50f + (float)Random.Range(5, 10));
			}
			else
			{
				contractWork2.zeitInWochen = Mathf.RoundToInt(contractWork2.points / 20000f + (float)Random.Range(5, 10));
			}
			if (contractWork2.auftraggeberID != -1)
			{
				contractWork2.Init();
			}
			else
			{
				contractWork2.gameObject.SetActive(value: false);
				Object.Destroy(contractWork2.gameObject);
			}
		}
		UpdateGUI();
	}

	private int GetRandomPublisherID()
	{
		int num = 0;
		if ((bool)mS_ && mS_.arrayPublisherScripts.Length != 0)
		{
			bool flag = false;
			while (!flag)
			{
				int num2 = Random.Range(0, mS_.arrayPublisherScripts.Length);
				if ((bool)mS_.arrayPublisherScripts[num2])
				{
					if (mS_.arrayPublisherScripts[num2].isUnlocked && !mS_.arrayPublisherScripts[num2].IsTochterfirma() && !mS_.arrayPublisherScripts[num2].isPlayer && !mS_.arrayPublisherScripts[num2].Geschlossen())
					{
						flag = true;
						return mS_.arrayPublisherScripts[num2].myID;
					}
					num++;
					if (num > 1000)
					{
						return -1;
					}
				}
			}
		}
		return -1;
	}

	public void UpdateGUI()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("ContractWork");
		if (array.Length != 0)
		{
			int num = 0;
			anzDEV = 0;
			anzQA = 0;
			anzGFX = 0;
			anzSFX = 0;
			anzMotion = 0;
			anzProduction = 0;
			anzWerkstatt = 0;
			anzHardware = 0;
			for (int i = 0; i < array.Length; i++)
			{
				contractWork component = array[i].GetComponent<contractWork>();
				if ((bool)component && !component.IsAngenommen() && component.gameObject.activeSelf)
				{
					num++;
					switch (component.art)
					{
					case 0:
						anzDEV++;
						break;
					case 1:
						anzQA++;
						break;
					case 2:
						anzGFX++;
						break;
					case 3:
						anzSFX++;
						break;
					case 4:
						anzMotion++;
						break;
					case 5:
						anzProduction++;
						break;
					case 6:
						anzWerkstatt++;
						break;
					case 7:
						anzHardware++;
						break;
					}
				}
			}
			if (num > 0)
			{
				if (!uiObjects[0].activeSelf)
				{
					uiObjects[0].SetActive(value: true);
				}
				uiObjects[0].transform.GetChild(0).GetComponent<Text>().text = num.ToString();
				string text = tS_.GetText(639);
				text = text + tS_.GetText(19) + ": <color=blue><b>" + anzDEV + "</b></color>\n";
				if (forschungSonstiges_.IsErforscht(28))
				{
					text = text + tS_.GetText(21) + ": <color=blue><b>" + anzQA + "</b></color>\n";
				}
				if (forschungSonstiges_.IsErforscht(31))
				{
					text = text + tS_.GetText(22) + ": <color=blue><b>" + anzGFX + "</b></color>\n";
				}
				if (forschungSonstiges_.IsErforscht(32))
				{
					text = text + tS_.GetText(23) + ": <color=blue><b>" + anzSFX + "</b></color>\n";
				}
				if (unlock_.Get(8))
				{
					text = text + tS_.GetText(28) + ": <color=blue><b>" + anzMotion + "</b></color>\n";
				}
				if (forschungSonstiges_.IsErforscht(33))
				{
					text = text + tS_.GetText(32) + ": <color=blue><b>" + anzProduction + "</b></color>\n";
				}
				if (forschungSonstiges_.IsErforscht(38))
				{
					text = text + tS_.GetText(1508) + ": <color=blue><b>" + anzWerkstatt + "</b></color>\n";
				}
				if (forschungSonstiges_.IsErforscht(39))
				{
					text = text + tS_.GetText(26) + ": <color=blue><b>" + anzHardware + "</b></color>\n";
				}
				uiObjects[0].GetComponent<tooltip>().c = text;
			}
			else if (uiObjects[0].activeSelf)
			{
				uiObjects[0].SetActive(value: false);
			}
		}
		else if (uiObjects[0].activeSelf)
		{
			uiObjects[0].SetActive(value: false);
		}
		anzContractGames = 0;
		if ((bool)mS_.games_)
		{
			if (mS_.games_.arrayGamesScripts.Length != 0)
			{
				string text2 = "<b>" + tS_.GetText(640) + "</b>\n\n";
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				int num5 = 0;
				int num6 = 0;
				int num7 = 0;
				int num8 = 0;
				for (int j = 0; j < mS_.games_.arrayGamesScripts.Length; j++)
				{
					if (!mS_.games_.arrayGamesScripts[j])
					{
						continue;
					}
					if (!mS_.games_.arrayGamesScripts[j].pS_)
					{
						mS_.games_.arrayGamesScripts[j].FindMyPublisher();
					}
					if ((bool)mS_.games_.arrayGamesScripts[j].pS_ && !mS_.games_.arrayGamesScripts[j].pS_.IsTochterfirma() && mS_.games_.arrayGamesScripts[j].auftragsspiel && !mS_.games_.arrayGamesScripts[j].auftragsspiel_Inivs)
					{
						num2++;
						anzContractGames++;
						switch (mS_.games_.arrayGamesScripts[j].gameSize)
						{
						case 0:
							num3++;
							break;
						case 1:
							num4++;
							break;
						case 2:
							num5++;
							break;
						case 3:
							num6++;
							break;
						case 4:
							num7++;
							break;
						case 5:
							num8++;
							break;
						}
					}
				}
				if (num2 > 0)
				{
					if (!uiObjects[1].activeSelf)
					{
						uiObjects[1].SetActive(value: true);
					}
					uiObjects[1].transform.GetChild(0).GetComponent<Text>().text = num2.ToString();
					if (num3 > 0)
					{
						text2 = (text2 = text2 + tS_.GetText(329) + ": <color=blue><b>" + num3 + "</b></color>\n");
					}
					if (num4 > 0)
					{
						text2 = (text2 = text2 + tS_.GetText(330) + ": <color=blue><b>" + num4 + "</b></color>\n");
					}
					if (num5 > 0)
					{
						text2 = (text2 = text2 + tS_.GetText(331) + ": <color=blue><b>" + num5 + "</b></color>\n");
					}
					if (num6 > 0)
					{
						text2 = (text2 = text2 + tS_.GetText(332) + ": <color=blue><b>" + num6 + "</b></color>\n");
					}
					if (num7 > 0)
					{
						text2 = (text2 = text2 + tS_.GetText(333) + ": <color=blue><b>" + num7 + "</b></color>\n");
					}
					if (num8 > 0)
					{
						text2 = (text2 = text2 + tS_.GetText(2193) + ": <color=blue><b>" + num8 + "</b></color>\n");
					}
					uiObjects[1].GetComponent<tooltip>().c = text2;
				}
				else if (uiObjects[1].activeSelf)
				{
					uiObjects[1].SetActive(value: false);
				}
			}
			else if (uiObjects[1].activeSelf)
			{
				uiObjects[1].SetActive(value: false);
			}
		}
		else if (uiObjects[1].activeSelf)
		{
			uiObjects[1].SetActive(value: false);
		}
	}
}
