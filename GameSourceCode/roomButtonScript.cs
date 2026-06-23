using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class roomButtonScript : MonoBehaviour
{
	public roomScript rS_;

	public Sprite[] roomIcons;

	public GameObject[] uiObjects;

	public GameObject[] uiOptions;

	public GameObject[] uiWindows;

	public Sprite[] uiSprites;

	private GameObject main_;

	private mainScript mS_;

	private roomDataScript rdS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private buildRoomScript brS_;

	private unlockScript unlock_;

	private textScript tS_;

	private forschungSonstiges forschungSonstiges_;

	private RectTransform myRectTransform;

	private copyRoomScript cpS_;

	private pickCharacterScript pcS_;

	private Text roomNameText;

	private bool mouseOver;

	private bool invisible;

	private float invisibleTimer;

	private float updateDataTimer;

	private GameObject child1;

	private GameObject child2;

	private void Start()
	{
		FindScripts();
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
		if (!rdS_)
		{
			rdS_ = main_.GetComponent<roomDataScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!brS_)
		{
			brS_ = main_.GetComponent<buildRoomScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!cpS_)
		{
			cpS_ = main_.GetComponent<copyRoomScript>();
		}
		if (!pcS_)
		{
			pcS_ = main_.GetComponent<pickCharacterScript>();
		}
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!roomNameText)
		{
			roomNameText = uiObjects[1].GetComponent<Text>();
		}
	}

	private void Init()
	{
		myRectTransform = base.gameObject.GetComponent<RectTransform>();
		uiObjects[0].GetComponent<Image>().sprite = roomIcons[rS_.typ];
		RemoveMenus();
		if ((bool)uiObjects[7])
		{
			uiObjects[7].GetComponent<Text>().text = rdS_.GetName(rS_.typ);
		}
		if ((bool)uiObjects[11])
		{
			uiObjects[11].GetComponent<Text>().text = rdS_.GetName(rS_.typ);
		}
		if ((bool)uiObjects[14])
		{
			uiObjects[14].GetComponent<Text>().text = rdS_.GetName(rS_.typ);
		}
		if ((bool)uiObjects[15])
		{
			uiObjects[15].GetComponent<Text>().text = rdS_.GetName(rS_.typ);
		}
		if ((bool)uiObjects[16])
		{
			uiObjects[16].GetComponent<Text>().text = rdS_.GetName(rS_.typ);
		}
	}

	public void MouseEnter()
	{
		if (guiMain_.IsRoomMenuOpen())
		{
			mouseOver = false;
			return;
		}
		mouseOver = true;
		StartCoroutine(iSetAsLastSibling());
	}

	public void MouseLeave()
	{
		mouseOver = false;
	}

	private void SortGuiY()
	{
		if (uiObjects[2].activeInHierarchy || mouseOver)
		{
			StartCoroutine(iSetAsLastSibling());
			return;
		}
		int siblingIndex = base.gameObject.transform.GetSiblingIndex();
		if (siblingIndex > 0)
		{
			Transform child = base.gameObject.transform.parent.GetChild(siblingIndex - 1);
			if ((bool)child && child.GetComponent<RectTransform>().position.y < myRectTransform.position.y)
			{
				base.gameObject.transform.SetSiblingIndex(siblingIndex - 1);
			}
		}
	}

	private IEnumerator iSetAsLastSibling()
	{
		yield return new WaitForEndOfFrame();
		base.gameObject.transform.SetAsLastSibling();
	}

	private void Update()
	{
		SortGuiY();
		if (guiMain_.uiObjects[25].activeSelf || guiMain_.uiObjects[15].activeSelf || guiMain_.uiObjects[74].activeSelf)
		{
			if (uiObjects[6].GetComponent<Button>().interactable)
			{
				uiObjects[6].GetComponent<Button>().interactable = false;
				uiObjects[6].GetComponent<Image>().raycastTarget = false;
			}
		}
		else if (!uiObjects[6].GetComponent<Button>().interactable)
		{
			uiObjects[6].GetComponent<Button>().interactable = true;
			uiObjects[6].GetComponent<Image>().raycastTarget = true;
		}
		if (invisible)
		{
			invisibleTimer += Time.deltaTime;
			if (invisibleTimer < 0.1f)
			{
				return;
			}
			invisibleTimer = 0f;
		}
		if (!rS_)
		{
			return;
		}
		if ((bool)rS_.camera_)
		{
			Vector2 vector = rS_.camera_.WorldToScreenPoint(rS_.uiPos);
			if (vector.x < -200f || vector.x >= (float)(Screen.width + 200) || vector.y < -200f || vector.y >= (float)(Screen.height + 200))
			{
				if (!child1)
				{
					child1 = base.transform.GetChild(1).gameObject;
				}
				if (!child2)
				{
					child2 = base.transform.GetChild(2).gameObject;
				}
				invisible = true;
				if (child1.activeSelf)
				{
					child1.SetActive(value: false);
				}
				if (child2.activeSelf)
				{
					child2.SetActive(value: false);
				}
				return;
			}
			if (!child1)
			{
				child1 = base.transform.GetChild(1).gameObject;
			}
			if (!child2)
			{
				child2 = base.transform.GetChild(2).gameObject;
			}
			invisible = false;
			if (!child1.activeSelf)
			{
				child1.SetActive(value: true);
			}
			if (!child2.activeSelf)
			{
				child2.SetActive(value: true);
			}
		}
		updateDataTimer += Time.deltaTime;
		if (updateDataTimer < 0.1f)
		{
			return;
		}
		updateDataTimer = 0f;
		if (!rdS_.KeineMitarbeiter(rS_.typ))
		{
			if (rS_.myName.Length > 0)
			{
				roomNameText.text = rS_.myName + " [" + rS_.GetMitarbeiter() + "/" + rS_.GetArbeitsplaetze() + "]";
			}
			else
			{
				roomNameText.text = "[" + rS_.GetMitarbeiter() + "/" + rS_.GetArbeitsplaetze() + "]";
			}
			if (rS_.GetMitarbeiter() > rS_.GetArbeitsplaetze())
			{
				roomNameText.color = Color.red;
			}
			else
			{
				roomNameText.color = Color.white;
			}
			if (rS_.pause)
			{
				if (!uiObjects[4].activeSelf)
				{
					uiObjects[4].SetActive(value: true);
				}
			}
			else if (uiObjects[4].activeSelf)
			{
				uiObjects[4].SetActive(value: false);
			}
			if (uiObjects[3].activeSelf)
			{
				if (!rS_.pause)
				{
					uiObjects[3].GetComponent<Image>().sprite = uiSprites[0];
				}
				else
				{
					uiObjects[3].GetComponent<Image>().sprite = uiSprites[1];
				}
			}
			if (uiObjects[5].activeSelf)
			{
				if (!rS_.lockKI)
				{
					uiObjects[5].GetComponent<Image>().sprite = uiSprites[3];
					if (uiObjects[12].activeSelf)
					{
						uiObjects[12].SetActive(value: false);
					}
				}
				else
				{
					uiObjects[5].GetComponent<Image>().sprite = uiSprites[2];
					if (!uiObjects[12].activeSelf)
					{
						uiObjects[12].SetActive(value: true);
					}
				}
			}
			if (!rS_)
			{
				return;
			}
			if (rS_.IsCrunchtimeRead())
			{
				if (!uiObjects[10].activeSelf)
				{
					uiObjects[10].SetActive(value: true);
				}
			}
			else if (uiObjects[10].activeSelf)
			{
				uiObjects[10].SetActive(value: false);
			}
		}
		else if (rS_.myName.Length > 0)
		{
			roomNameText.text = rS_.myName;
		}
		else if (roomNameText.text.Length > 0)
		{
			roomNameText.text = "";
		}
	}

	private void OnDisable()
	{
		if ((bool)rS_ && (bool)rS_.myUI_Line)
		{
			rS_.myUI_Line.SetActive(value: false);
		}
	}

	private void OnEnable()
	{
		if ((bool)rS_ && (bool)rS_.myUI_Line)
		{
			rS_.myUI_Line.SetActive(value: true);
		}
	}

	public bool CloseAllMenus()
	{
		if (!uiObjects[2].activeSelf)
		{
			return false;
		}
		for (int i = 0; i < uiOptions.Length; i++)
		{
			if ((bool)uiOptions[i] && uiOptions[i].activeSelf)
			{
				uiOptions[i].SetActive(value: false);
				return true;
			}
		}
		return false;
	}

	public bool IsMenuOpen()
	{
		for (int i = 0; i < uiOptions.Length; i++)
		{
			if ((bool)uiOptions[i] && uiOptions[i].activeSelf)
			{
				return true;
			}
		}
		return false;
	}

	private void RemoveMenus()
	{
		switch (rS_.typ)
		{
		case 1:
		{
			for (int k = 0; k < uiOptions.Length; k++)
			{
				if (k != 1 && k != 3 && k != 7 && k != 8 && k != 15 && k != 16 && k != 33 && k != 38 && (bool)uiOptions[k])
				{
					Object.Destroy(uiOptions[k]);
					uiOptions[k] = null;
				}
			}
			for (int l = 0; l < uiWindows.Length; l++)
			{
				if (l != 1 && l != 2 && l != 3 && l != 6 && l != 7 && l != 22 && l != 25 && (bool)uiWindows[l])
				{
					Object.Destroy(uiWindows[l]);
					uiWindows[l] = null;
				}
			}
			break;
		}
		case 2:
		{
			for (int m = 0; m < uiOptions.Length; m++)
			{
				if (m != 0 && m != 2 && m != 8 && m != 42 && m != 45 && (bool)uiOptions[m])
				{
					Object.Destroy(uiOptions[m]);
					uiOptions[m] = null;
				}
			}
			for (int n = 0; n < uiWindows.Length; n++)
			{
				if (n != 0 && n != 3 && n != 29 && n != 32 && (bool)uiWindows[n])
				{
					Object.Destroy(uiWindows[n]);
					uiWindows[n] = null;
				}
			}
			break;
		}
		case 11:
		{
			for (int num17 = 0; num17 < uiOptions.Length; num17++)
			{
				if (num17 != 4 && (bool)uiOptions[num17])
				{
					Object.Destroy(uiOptions[num17]);
					uiOptions[num17] = null;
				}
			}
			for (int num18 = 0; num18 < uiWindows.Length; num18++)
			{
				if ((bool)uiWindows[num18])
				{
					Object.Destroy(uiWindows[num18]);
					uiWindows[num18] = null;
				}
			}
			break;
		}
		case 12:
		{
			for (int num9 = 0; num9 < uiOptions.Length; num9++)
			{
				if (num9 != 5 && (bool)uiOptions[num9])
				{
					Object.Destroy(uiOptions[num9]);
					uiOptions[num9] = null;
				}
			}
			for (int num10 = 0; num10 < uiWindows.Length; num10++)
			{
				if ((bool)uiWindows[num10])
				{
					Object.Destroy(uiWindows[num10]);
					uiWindows[num10] = null;
				}
			}
			break;
		}
		case 14:
		{
			for (int num13 = 0; num13 < uiOptions.Length; num13++)
			{
				if (num13 != 6 && num13 != 8 && num13 != 29 && num13 != 15 && num13 != 38 && (bool)uiOptions[num13])
				{
					Object.Destroy(uiOptions[num13]);
					uiOptions[num13] = null;
				}
			}
			for (int num14 = 0; num14 < uiWindows.Length; num14++)
			{
				if (num14 != 3 && num14 != 6 && num14 != 16 && num14 != 25 && (bool)uiWindows[num14])
				{
					Object.Destroy(uiWindows[num14]);
					uiWindows[num14] = null;
				}
			}
			break;
		}
		case 9:
		{
			for (int num23 = 0; num23 < uiOptions.Length; num23++)
			{
				if (num23 != 9 && (bool)uiOptions[num23])
				{
					Object.Destroy(uiOptions[num23]);
					uiOptions[num23] = null;
				}
			}
			for (int num24 = 0; num24 < uiWindows.Length; num24++)
			{
				if (num24 != 17 && (bool)uiWindows[num24])
				{
					Object.Destroy(uiWindows[num24]);
					uiWindows[num24] = null;
				}
			}
			break;
		}
		case 15:
		{
			for (int num = 0; num < uiOptions.Length; num++)
			{
				if (num != 10 && (bool)uiOptions[num])
				{
					Object.Destroy(uiOptions[num]);
					uiOptions[num] = null;
				}
			}
			for (int num2 = 0; num2 < uiWindows.Length; num2++)
			{
				if (num2 != 19 && (bool)uiWindows[num2])
				{
					Object.Destroy(uiWindows[num2]);
					uiWindows[num2] = null;
				}
			}
			break;
		}
		case 6:
		{
			for (int num21 = 0; num21 < uiOptions.Length; num21++)
			{
				if (num21 != 11 && num21 != 8 && num21 != 12 && num21 != 30 && num21 != 32 && num21 != 40 && (bool)uiOptions[num21])
				{
					Object.Destroy(uiOptions[num21]);
					uiOptions[num21] = null;
				}
			}
			for (int num22 = 0; num22 < uiWindows.Length; num22++)
			{
				if (num22 != 3 && num22 != 4 && num22 != 18 && num22 != 21 && num22 != 27 && (bool)uiWindows[num22])
				{
					Object.Destroy(uiWindows[num22]);
					uiWindows[num22] = null;
				}
			}
			break;
		}
		case 13:
		{
			for (int num7 = 0; num7 < uiOptions.Length; num7++)
			{
				if (num7 != 13 && num7 != 14 && (bool)uiOptions[num7])
				{
					Object.Destroy(uiOptions[num7]);
					uiOptions[num7] = null;
				}
			}
			for (int num8 = 0; num8 < uiWindows.Length; num8++)
			{
				if (num8 != 5 && (bool)uiWindows[num8])
				{
					Object.Destroy(uiWindows[num8]);
					uiWindows[num8] = null;
				}
			}
			break;
		}
		case 7:
		{
			for (int num25 = 0; num25 < uiOptions.Length; num25++)
			{
				if (num25 != 17 && num25 != 8 && num25 != 18 && num25 != 19 && num25 != 41 && (bool)uiOptions[num25])
				{
					Object.Destroy(uiOptions[num25]);
					uiOptions[num25] = null;
				}
			}
			for (int num26 = 0; num26 < uiWindows.Length; num26++)
			{
				if (num26 != 3 && num26 != 8 && num26 != 9 && num26 != 28 && (bool)uiWindows[num26])
				{
					Object.Destroy(uiWindows[num26]);
					uiWindows[num26] = null;
				}
			}
			break;
		}
		case 3:
		{
			for (int num15 = 0; num15 < uiOptions.Length; num15++)
			{
				if (num15 != 20 && num15 != 8 && num15 != 21 && num15 != 15 && num15 != 28 && num15 != 31 && num15 != 38 && num15 != 39 && (bool)uiOptions[num15])
				{
					Object.Destroy(uiOptions[num15]);
					uiOptions[num15] = null;
				}
			}
			for (int num16 = 0; num16 < uiWindows.Length; num16++)
			{
				if (num16 != 3 && num16 != 10 && num16 != 11 && num16 != 6 && num16 != 15 && num16 != 20 && num16 != 25 && num16 != 26 && (bool)uiWindows[num16])
				{
					Object.Destroy(uiWindows[num16]);
					uiWindows[num16] = null;
				}
			}
			break;
		}
		case 4:
		{
			for (int num5 = 0; num5 < uiOptions.Length; num5++)
			{
				if (num5 != 22 && num5 != 8 && num5 != 23 && num5 != 15 && num5 != 31 && num5 != 38 && (bool)uiOptions[num5])
				{
					Object.Destroy(uiOptions[num5]);
					uiOptions[num5] = null;
				}
			}
			for (int num6 = 0; num6 < uiWindows.Length; num6++)
			{
				if (num6 != 3 && num6 != 12 && num6 != 6 && num6 != 20 && num6 != 25 && (bool)uiWindows[num6])
				{
					Object.Destroy(uiWindows[num6]);
					uiWindows[num6] = null;
				}
			}
			break;
		}
		case 5:
		{
			for (int num27 = 0; num27 < uiOptions.Length; num27++)
			{
				if (num27 != 24 && num27 != 8 && num27 != 25 && num27 != 15 && num27 != 31 && num27 != 38 && (bool)uiOptions[num27])
				{
					Object.Destroy(uiOptions[num27]);
					uiOptions[num27] = null;
				}
			}
			for (int num28 = 0; num28 < uiWindows.Length; num28++)
			{
				if (num28 != 3 && num28 != 13 && num28 != 6 && num28 != 20 && num28 != 25 && (bool)uiWindows[num28])
				{
					Object.Destroy(uiWindows[num28]);
					uiWindows[num28] = null;
				}
			}
			break;
		}
		case 10:
		{
			for (int num19 = 0; num19 < uiOptions.Length; num19++)
			{
				if (num19 != 26 && num19 != 8 && num19 != 27 && num19 != 15 && num19 != 31 && num19 != 38 && (bool)uiOptions[num19])
				{
					Object.Destroy(uiOptions[num19]);
					uiOptions[num19] = null;
				}
			}
			for (int num20 = 0; num20 < uiWindows.Length; num20++)
			{
				if (num20 != 3 && num20 != 14 && num20 != 6 && num20 != 20 && num20 != 25 && (bool)uiWindows[num20])
				{
					Object.Destroy(uiWindows[num20]);
					uiWindows[num20] = null;
				}
			}
			break;
		}
		case 17:
		{
			for (int num11 = 0; num11 < uiOptions.Length; num11++)
			{
				if (num11 != 8 && num11 != 15 && num11 != 34 && num11 != 35 && num11 != 38 && (bool)uiOptions[num11])
				{
					Object.Destroy(uiOptions[num11]);
					uiOptions[num11] = null;
				}
			}
			for (int num12 = 0; num12 < uiWindows.Length; num12++)
			{
				if (num12 != 3 && num12 != 6 && num12 != 23 && num12 != 25 && (bool)uiWindows[num12])
				{
					Object.Destroy(uiWindows[num12]);
					uiWindows[num12] = null;
				}
			}
			break;
		}
		case 8:
		{
			for (int num3 = 0; num3 < uiOptions.Length; num3++)
			{
				if (num3 != 8 && num3 != 15 && num3 != 36 && num3 != 37 && num3 != 38 && num3 != 43 && num3 != 44 && (bool)uiOptions[num3])
				{
					Object.Destroy(uiOptions[num3]);
					uiOptions[num3] = null;
				}
			}
			for (int num4 = 0; num4 < uiWindows.Length; num4++)
			{
				if (num4 != 3 && num4 != 6 && num4 != 24 && num4 != 25 && num4 != 30 && num4 != 31 && (bool)uiWindows[num4])
				{
					Object.Destroy(uiWindows[num4]);
					uiWindows[num4] = null;
				}
			}
			break;
		}
		case 16:
		{
			for (int i = 0; i < uiOptions.Length; i++)
			{
				if ((bool)uiOptions[i])
				{
					Object.Destroy(uiOptions[i]);
					uiOptions[i] = null;
				}
			}
			for (int j = 0; j < uiWindows.Length; j++)
			{
				if ((bool)uiWindows[j])
				{
					Object.Destroy(uiWindows[j]);
					uiWindows[j] = null;
				}
			}
			break;
		}
		}
	}

	public void BUTTON_Main()
	{
		sfx_.PlaySound(3, force: true);
		int num = -1;
		guiMain_.guiMainButtons_.CloseAllDropdowns();
		switch (rS_.typ)
		{
		case 1:
		{
			if (rS_.taskID == -1)
			{
				num = 1;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject12 = GameObject.Find("Task_" + rS_.taskID);
			if (!gameObject12)
			{
				break;
			}
			if ((bool)gameObject12.GetComponent<taskGame>())
			{
				num = 7;
				if (rS_.IsGameDevCompleteOrg())
				{
					uiObjects[8].GetComponent<Button>().interactable = true;
					uiObjects[8].transform.GetChild(0).GetComponent<Text>().color = Color.black;
				}
				else
				{
					uiObjects[8].GetComponent<Button>().interactable = false;
					uiObjects[8].transform.GetChild(0).GetComponent<Text>().color = Color.grey;
				}
				if (rS_.IsDevAddon())
				{
					uiObjects[9].GetComponent<Button>().interactable = false;
					uiObjects[9].transform.GetChild(0).GetComponent<Text>().color = Color.grey;
				}
				else
				{
					uiObjects[9].GetComponent<Button>().interactable = true;
					uiObjects[9].transform.GetChild(0).GetComponent<Text>().color = Color.black;
				}
			}
			else if ((bool)gameObject12.GetComponent<taskEngine>())
			{
				num = 3;
			}
			else if ((bool)gameObject12.GetComponent<taskUnterstuetzen>())
			{
				num = 8;
			}
			else if ((bool)gameObject12.GetComponent<taskContractWork>())
			{
				num = 15;
			}
			else if ((bool)gameObject12.GetComponent<taskContractWait>())
			{
				num = 38;
			}
			else if ((bool)gameObject12.GetComponent<taskUpdate>())
			{
				num = 16;
			}
			else if ((bool)gameObject12.GetComponent<taskF2PUpdate>())
			{
				num = 33;
			}
			break;
		}
		case 2:
		{
			if (rS_.taskID == -1)
			{
				num = 0;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject11 = GameObject.Find("Task_" + rS_.taskID);
			if ((bool)gameObject11)
			{
				if ((bool)gameObject11.GetComponent<taskForschung>())
				{
					num = 2;
				}
				else if ((bool)gameObject11.GetComponent<taskUnterstuetzen>())
				{
					num = 8;
				}
				else if ((bool)gameObject11.GetComponent<taskForschungWait>())
				{
					num = 42;
				}
				else if ((bool)gameObject11.GetComponent<taskAutoForschung>())
				{
					num = 45;
				}
			}
			break;
		}
		case 11:
			num = 4;
			break;
		case 12:
			num = 5;
			break;
		case 14:
		{
			if (rS_.taskID == -1)
			{
				num = 6;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject9 = GameObject.Find("Task_" + rS_.taskID);
			if ((bool)gameObject9)
			{
				if ((bool)gameObject9.GetComponent<taskProduction>())
				{
					num = 29;
				}
				else if ((bool)gameObject9.GetComponent<taskContractWork>())
				{
					num = 15;
				}
				else if ((bool)gameObject9.GetComponent<taskContractWait>())
				{
					num = 38;
				}
				else if ((bool)gameObject9.GetComponent<taskUnterstuetzen>())
				{
					num = 8;
				}
			}
			break;
		}
		case 9:
			num = 9;
			break;
		case 15:
			num = 10;
			break;
		case 6:
		{
			if (rS_.taskID == -1)
			{
				num = 11;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject6 = GameObject.Find("Task_" + rS_.taskID);
			if ((bool)gameObject6)
			{
				if ((bool)gameObject6.GetComponent<taskMarketing>())
				{
					num = 12;
				}
				else if ((bool)gameObject6.GetComponent<taskMarktforschung>())
				{
					num = 30;
				}
				else if ((bool)gameObject6.GetComponent<taskMarketingSpezial>())
				{
					num = 32;
				}
				else if ((bool)gameObject6.GetComponent<taskMitarbeitersuche>())
				{
					num = 40;
				}
				else if ((bool)gameObject6.GetComponent<taskUnterstuetzen>())
				{
					num = 8;
				}
			}
			break;
		}
		case 13:
			if (rS_.taskID == -1)
			{
				num = 13;
			}
			if (rS_.taskID != -1)
			{
				GameObject gameObject10 = GameObject.Find("Task_" + rS_.taskID);
				if ((bool)gameObject10 && (bool)gameObject10.GetComponent<taskTraining>())
				{
					num = 14;
				}
			}
			break;
		case 7:
		{
			if (rS_.taskID == -1)
			{
				num = 17;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject7 = GameObject.Find("Task_" + rS_.taskID);
			if ((bool)gameObject7)
			{
				if ((bool)gameObject7.GetComponent<taskFankampagne>())
				{
					num = 18;
				}
				else if ((bool)gameObject7.GetComponent<taskSupport>())
				{
					num = 19;
				}
				else if ((bool)gameObject7.GetComponent<taskFanshop>())
				{
					num = 41;
				}
				else if ((bool)gameObject7.GetComponent<taskUnterstuetzen>())
				{
					num = 8;
				}
			}
			break;
		}
		case 3:
		{
			if (rS_.taskID == -1)
			{
				num = 20;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject8 = GameObject.Find("Task_" + rS_.taskID);
			if ((bool)gameObject8)
			{
				if ((bool)gameObject8.GetComponent<taskBugfixing>())
				{
					num = 21;
				}
				else if ((bool)gameObject8.GetComponent<taskGameplayVerbessern>())
				{
					num = 21;
				}
				else if ((bool)gameObject8.GetComponent<taskContractWork>())
				{
					num = 15;
				}
				else if ((bool)gameObject8.GetComponent<taskContractWait>())
				{
					num = 38;
				}
				else if ((bool)gameObject8.GetComponent<taskSpielbericht>())
				{
					num = 28;
				}
				else if ((bool)gameObject8.GetComponent<taskUnterstuetzen>())
				{
					num = 8;
				}
				else if ((bool)gameObject8.GetComponent<taskPolishing>())
				{
					num = 31;
				}
				else if ((bool)gameObject8.GetComponent<taskWait>())
				{
					num = 39;
				}
			}
			break;
		}
		case 4:
		{
			if (rS_.taskID == -1)
			{
				num = 22;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject2 = GameObject.Find("Task_" + rS_.taskID);
			if ((bool)gameObject2)
			{
				if ((bool)gameObject2.GetComponent<taskGrafikVerbessern>())
				{
					num = 23;
				}
				else if ((bool)gameObject2.GetComponent<taskContractWork>())
				{
					num = 15;
				}
				else if ((bool)gameObject2.GetComponent<taskContractWait>())
				{
					num = 38;
				}
				else if ((bool)gameObject2.GetComponent<taskUnterstuetzen>())
				{
					num = 8;
				}
				else if ((bool)gameObject2.GetComponent<taskPolishing>())
				{
					num = 31;
				}
			}
			break;
		}
		case 5:
		{
			if (rS_.taskID == -1)
			{
				num = 24;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject4 = GameObject.Find("Task_" + rS_.taskID);
			if ((bool)gameObject4)
			{
				if ((bool)gameObject4.GetComponent<taskSoundVerbessern>())
				{
					num = 25;
				}
				else if ((bool)gameObject4.GetComponent<taskContractWork>())
				{
					num = 15;
				}
				else if ((bool)gameObject4.GetComponent<taskContractWait>())
				{
					num = 38;
				}
				else if ((bool)gameObject4.GetComponent<taskUnterstuetzen>())
				{
					num = 8;
				}
				else if ((bool)gameObject4.GetComponent<taskPolishing>())
				{
					num = 31;
				}
			}
			break;
		}
		case 10:
		{
			if (rS_.taskID == -1)
			{
				num = 26;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject3 = GameObject.Find("Task_" + rS_.taskID);
			if ((bool)gameObject3)
			{
				if ((bool)gameObject3.GetComponent<taskAnimationVerbessern>())
				{
					num = 27;
				}
				else if ((bool)gameObject3.GetComponent<taskContractWork>())
				{
					num = 15;
				}
				else if ((bool)gameObject3.GetComponent<taskContractWait>())
				{
					num = 38;
				}
				else if ((bool)gameObject3.GetComponent<taskUnterstuetzen>())
				{
					num = 8;
				}
				else if ((bool)gameObject3.GetComponent<taskPolishing>())
				{
					num = 31;
				}
			}
			break;
		}
		case 17:
		{
			if (rS_.taskID == -1)
			{
				num = 34;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject5 = GameObject.Find("Task_" + rS_.taskID);
			if ((bool)gameObject5)
			{
				if ((bool)gameObject5.GetComponent<taskArcadeProduction>())
				{
					num = 35;
				}
				else if ((bool)gameObject5.GetComponent<taskContractWork>())
				{
					num = 15;
				}
				else if ((bool)gameObject5.GetComponent<taskContractWait>())
				{
					num = 38;
				}
				else if ((bool)gameObject5.GetComponent<taskUnterstuetzen>())
				{
					num = 8;
				}
			}
			break;
		}
		case 8:
		{
			if (rS_.taskID == -1)
			{
				num = 36;
			}
			if (rS_.taskID == -1)
			{
				break;
			}
			GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
			if ((bool)gameObject)
			{
				if (rS_.IsKonsoleDevCompleteOrg())
				{
					uiObjects[13].GetComponent<Button>().interactable = true;
					uiObjects[13].transform.GetChild(0).GetComponent<Text>().color = Color.black;
				}
				else
				{
					uiObjects[13].GetComponent<Button>().interactable = false;
					uiObjects[13].transform.GetChild(0).GetComponent<Text>().color = Color.grey;
				}
				if ((bool)gameObject.GetComponent<taskKonsole>())
				{
					num = 37;
				}
				else if ((bool)gameObject.GetComponent<taskKonsoleReduceCosts>())
				{
					num = 43;
				}
				else if ((bool)gameObject.GetComponent<taskKonsoleHaltbarkeit>())
				{
					num = 43;
				}
				else if ((bool)gameObject.GetComponent<taskContractWork>())
				{
					num = 15;
				}
				else if ((bool)gameObject.GetComponent<taskContractWait>())
				{
					num = 38;
				}
				else if ((bool)gameObject.GetComponent<taskUnterstuetzen>())
				{
					num = 8;
				}
			}
			break;
		}
		case 16:
			guiMain_.uiObjects[213].SetActive(value: true);
			guiMain_.uiObjects[213].GetComponent<Menu_Immobilien>().Init(rS_);
			guiMain_.OpenMenu(hideChars: true);
			break;
		}
		if (num != -1)
		{
			bool activeSelf = uiOptions[num].activeSelf;
			guiMain_.CloseAllRoomButtons();
			uiOptions[num].SetActive(!activeSelf);
			uiObjects[2].SetActive(!activeSelf);
			uiObjects[2].transform.parent = uiOptions[num].transform;
			if (rdS_.KeineMitarbeiter(rS_.typ) && uiObjects[2].transform.childCount >= 9)
			{
				Object.Destroy(uiObjects[2].transform.GetChild(0).gameObject);
				Object.Destroy(uiObjects[2].transform.GetChild(1).gameObject);
				Object.Destroy(uiObjects[2].transform.GetChild(2).gameObject);
			}
			base.gameObject.transform.SetAsLastSibling();
			mS_.PauseGame(!activeSelf);
		}
	}

	public void BUTTON_AutoUpdate()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		if (rS_.UpdateInventar(buy: false))
		{
			guiMain_.uiObjects[253].SetActive(value: true);
			guiMain_.uiObjects[253].GetComponent<Menu_W_UpdateObjects>().Init(rS_);
		}
		else
		{
			guiMain_.MessageBox(tS_.GetText(1286), closeMenu: true);
		}
	}

	public void BUTTON_RenameRoom()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.uiObjects[17].SetActive(value: true);
		guiMain_.uiObjects[17].GetComponent<Menu_RenameRoom>().Init(rS_);
	}

	public void BUTTON_DemolishRoom()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.uiObjects[18].SetActive(value: true);
		guiMain_.uiObjects[18].GetComponent<Menu_DemolishRoom>().Init(rS_);
	}

	public void BUTTON_RedesignRoom()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: true);
		guiMain_.uiObjects[19].SetActive(value: true);
		guiMain_.uiObjects[19].GetComponent<Menu_BuildRoom>().BUTTON_SelectRoom(rS_.typ);
		brS_.CreateOldRoomLayout(rS_);
	}

	public void BUTTON_CopyRoom()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: true);
		cpS_.InitCopyRoom(rS_);
	}

	public void BUTTON_MoveRoom()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: true);
		cpS_.InitMoveRoom(rS_);
	}

	public void BUTTON_PauseRoom()
	{
		sfx_.PlaySound(3, force: true);
		rS_.pause = !rS_.pause;
	}

	public void BUTTON_LockKIRoom()
	{
		sfx_.PlaySound(3, force: true);
		rS_.lockKI = !rS_.lockKI;
	}

	public void BUTTON_RoomPersonal()
	{
		sfx_.PlaySound(3, force: true);
		if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		{
			for (int i = 0; i < mS_.arrayCharactersScripts.Length; i++)
			{
				if ((bool)mS_.arrayCharactersScripts[i] && mS_.arrayCharactersScripts[i].roomID == rS_.myID)
				{
					pcS_.PickFromExternObject(mS_.arrayCharactersScripts[i].gameObject);
				}
			}
		}
		else
		{
			guiMain_.OpenMenu(hideChars: false);
			guiMain_.ActivateMenu(guiMain_.uiObjects[26]);
			guiMain_.uiObjects[26].GetComponent<Menu_Personal_InRoom>().Init(rS_.myID);
		}
	}

	public void BUTTON_Verschiebe_Task()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.disableRoomGUI = false;
		guiMain_.ActivateMenu(guiMain_.uiObjects[25]);
		guiMain_.uiObjects[25].GetComponent<Menu_Verschiebe_Task>().rS_ = rS_;
	}

	public void BUTTON_Unterstuetzen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.disableRoomGUI = false;
		guiMain_.ActivateMenu(guiMain_.uiObjects[74]);
		guiMain_.uiObjects[74].GetComponent<Menu_Unterstuetzen>().rS_ = rS_;
	}

	public void BUTTON_LagerRestbestand()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[225]);
		guiMain_.uiObjects[225].GetComponent<Menu_LagerSelect>().Init(rS_);
	}

	public void BUTTON_AutoForschung()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[459]);
		guiMain_.uiObjects[459].GetComponent<Menu_AutoForschung>().Init(rS_);
	}

	public void BUTTON_Forschung(int i)
	{
		if ((i != 4 && i != 6) || forschungSonstiges_.IsErforscht(39))
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.OpenMenu(hideChars: false);
			guiMain_.ActivateMenu(guiMain_.uiObjects[21]);
			guiMain_.uiObjects[21].GetComponent<Menu_Forschung>().Init(rS_.myID, i);
		}
	}

	public void BUTTON_Forschung_AutoEnd()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)rS_.taskGameObject)
		{
			if (rS_.taskGameObject.GetComponent<taskForschung>().autoForschung)
			{
				CloseAllMenus();
				mS_.PauseGame(p: false);
				BUTTON_AutoForschung();
				return;
			}
			bool automatic = rS_.taskGameObject.GetComponent<taskForschung>().automatic;
			rS_.taskGameObject.GetComponent<taskForschung>().automatic = !automatic;
		}
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_Spielbericht_AutoEnd()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)rS_.taskGameObject)
		{
			bool automatic = rS_.taskGameObject.GetComponent<taskSpielbericht>().automatic;
			rS_.taskGameObject.GetComponent<taskSpielbericht>().automatic = !automatic;
		}
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_GameUpdate_AutoEnd()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)rS_.taskGameObject)
		{
			bool automatic = rS_.taskGameObject.GetComponent<taskUpdate>().automatic;
			rS_.taskGameObject.GetComponent<taskUpdate>().automatic = !automatic;
		}
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_F2PUpdate_AutoEnd()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)rS_.taskGameObject)
		{
			bool automatic = rS_.taskGameObject.GetComponent<taskF2PUpdate>().automatic;
			rS_.taskGameObject.GetComponent<taskF2PUpdate>().automatic = !automatic;
		}
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_Dev_Game()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[57]);
		guiMain_.uiObjects[57].GetComponent<Menu_DevGameMain>().Init(rS_);
	}

	public void BUTTON_Dev_Game_GameplayFeatures()
	{
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskGame component = gameObject.GetComponent<taskGame>();
			if ((bool)component)
			{
				GameObject gameObject2 = GameObject.Find("GAME_" + component.gameID);
				if ((bool)gameObject2)
				{
					gameScript component2 = gameObject2.GetComponent<gameScript>();
					if ((bool)component2)
					{
						guiMain_.ActivateMenu(guiMain_.uiObjects[348]);
						guiMain_.uiObjects[348].GetComponent<Menu_Dev_ChangeGameplayFeatures>().Init(component2);
					}
				}
			}
		}
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
	}

	public void BUTTON_Dev_Game_Entwicklungsbericht()
	{
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskGame component = gameObject.GetComponent<taskGame>();
			if ((bool)component)
			{
				GameObject gameObject2 = GameObject.Find("GAME_" + component.gameID);
				if ((bool)gameObject2)
				{
					gameScript component2 = gameObject2.GetComponent<gameScript>();
					if ((bool)component2)
					{
						guiMain_.ActivateMenu(guiMain_.uiObjects[73]);
						guiMain_.uiObjects[73].GetComponent<Menu_Dev_GameEntwicklungsbericht>().Init(component2, rS_);
					}
				}
			}
		}
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
	}

	public void BUTTON_Dev_Game_Abschliessen()
	{
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<taskGame>().CompleteOpenMenue();
		}
	}

	public void BUTTON_Dev_Konsole_Abschliessen()
	{
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<taskKonsole>().CompleteOpenMenue();
		}
	}

	public void BUTTON_Dev_ChangeDesignprioritaet()
	{
		sfx_.PlaySound(3, force: true);
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskGame component = gameObject.GetComponent<taskGame>();
			if ((bool)component)
			{
				GameObject gameObject2 = GameObject.Find("GAME_" + component.gameID);
				if ((bool)gameObject2)
				{
					gameScript component2 = gameObject2.GetComponent<gameScript>();
					if ((bool)component2)
					{
						guiMain_.ActivateMenu(guiMain_.uiObjects[371]);
						guiMain_.uiObjects[371].GetComponent<Menu_Dev_ChangeDesignproritaet>().Init(component2);
					}
				}
			}
		}
		guiMain_.OpenMenu(hideChars: false);
	}

	public void BUTTON_Dev_ChangeCopyProtect()
	{
		sfx_.PlaySound(3, force: true);
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskGame component = gameObject.GetComponent<taskGame>();
			if ((bool)component)
			{
				GameObject gameObject2 = GameObject.Find("GAME_" + component.gameID);
				if ((bool)gameObject2)
				{
					gameScript component2 = gameObject2.GetComponent<gameScript>();
					if ((bool)component2)
					{
						guiMain_.ActivateMenu(guiMain_.uiObjects[365]);
						guiMain_.uiObjects[365].GetComponent<Menu_Dev_ChangeCopyProtect>().Init(component2);
					}
				}
			}
		}
		guiMain_.OpenMenu(hideChars: false);
	}

	public void BUTTON_Dev_ChangePlatform()
	{
		sfx_.PlaySound(3, force: true);
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskGame component = gameObject.GetComponent<taskGame>();
			if ((bool)component)
			{
				GameObject gameObject2 = GameObject.Find("GAME_" + component.gameID);
				if ((bool)gameObject2)
				{
					gameScript component2 = gameObject2.GetComponent<gameScript>();
					if ((bool)component2)
					{
						guiMain_.ActivateMenu(guiMain_.uiObjects[102]);
						guiMain_.uiObjects[102].GetComponent<Menu_Dev_ChangePlatform>().Init(component2);
					}
				}
			}
		}
		guiMain_.OpenMenu(hideChars: false);
	}

	public void BUTTON_Dev_Auftrag()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[95]);
		guiMain_.uiObjects[95].GetComponent<Menu_Dev_Auftrag>().Init(rS_);
	}

	public void BUTTON_GFX_Auftrag()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[96]);
		guiMain_.uiObjects[96].GetComponent<Menu_Dev_AuftragSelect>().Init(rS_);
	}

	public void BUTTON_WERK_Auftrag()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[96]);
		guiMain_.uiObjects[96].GetComponent<Menu_Dev_AuftragSelect>().Init(rS_);
	}

	public void BUTTON_PROD_Auftrag()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[96]);
		guiMain_.uiObjects[96].GetComponent<Menu_Dev_AuftragSelect>().Init(rS_);
	}

	public void BUTTON_HARD_Auftrag()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[96]);
		guiMain_.uiObjects[96].GetComponent<Menu_Dev_AuftragSelect>().Init(rS_);
	}

	public void BUTTON_Dev_Engines()
	{
		if (unlock_.Get(58))
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.OpenMenu(hideChars: false);
			guiMain_.ActivateMenu(guiMain_.uiObjects[36]);
			guiMain_.uiObjects[36].GetComponent<Menu_Dev_EngineMain>().Init(rS_);
		}
	}

	public void BUTTON_Dev_Addons()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[104]);
		guiMain_.uiObjects[104].GetComponent<Menu_Dev_Addon>().Init(rS_);
	}

	public void BUTTON_Unterstuetzung_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject && (bool)gameObject.GetComponent<taskUnterstuetzen>())
		{
			gameObject.GetComponent<taskUnterstuetzen>().Abbrechen();
		}
		rS_.taskID = -1;
		rS_.taskGameObject = null;
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_Marketing_Mitarbeitersuche()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[344]);
		guiMain_.uiObjects[344].GetComponent<Menu_Mitarbeitersuche>().Init(rS_);
	}

	public void BUTTON_Marketing_NeueKampagne()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[88]);
		guiMain_.uiObjects[88].GetComponent<Menu_Marketing_Main>().Init(rS_);
	}

	public void BUTTON_Marketing_SpezialKampagne()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[294]);
		guiMain_.uiObjects[294].GetComponent<Menu_MarketingSpezial>().Init(rS_);
	}

	public void BUTTON_Marketing_Marktforschung()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[230]);
		guiMain_.uiObjects[230].GetComponent<Menu_Marktforschung>().Init(rS_);
	}

	public void BUTTON_Support_Fankampagne()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[139]);
		guiMain_.uiObjects[139].GetComponent<Menu_Support_Fankampagne>().Init(rS_);
	}

	public void BUTTON_Support_Anrufe()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[141]);
		guiMain_.uiObjects[141].GetComponent<Menu_Support_Anrufe>().Init(rS_);
	}

	public void BUTTON_Support_Fanshop()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[368]);
		guiMain_.uiObjects[368].GetComponent<Menu_Support_Fanshop>().Init(rS_);
	}

	public void BUTTON_Support_AutoEnd()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)rS_.taskGameObject && (bool)rS_.taskGameObject.GetComponent<taskFankampagne>())
		{
			bool automatic = rS_.taskGameObject.GetComponent<taskFankampagne>().automatic;
			rS_.taskGameObject.GetComponent<taskFankampagne>().automatic = !automatic;
		}
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_Marketing_AutoEnd()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)rS_.taskGameObject)
		{
			bool automatic = rS_.taskGameObject.GetComponent<taskMarketing>().automatic;
			rS_.taskGameObject.GetComponent<taskMarketing>().automatic = !automatic;
		}
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_Mitarbeitersuche_AutoEnd()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)rS_.taskGameObject)
		{
			bool automatic = rS_.taskGameObject.GetComponent<taskMitarbeitersuche>().automatic;
			rS_.taskGameObject.GetComponent<taskMitarbeitersuche>().automatic = !automatic;
		}
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_Production_AutoEnd()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)rS_.taskGameObject)
		{
			bool automatic = rS_.taskGameObject.GetComponent<taskProduction>().automatic;
			rS_.taskGameObject.GetComponent<taskProduction>().automatic = !automatic;
			if (rS_.taskGameObject.GetComponent<taskProduction>().GetAmount() <= 0 && !rS_.taskGameObject.GetComponent<taskProduction>().automatic)
			{
				Object.Destroy(rS_.taskGameObject);
			}
		}
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_Training_NeuerKurs()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[92]);
		guiMain_.uiObjects[92].GetComponent<Menu_Training_Select>().Init(rS_);
	}

	public void BUTTON_Training_AutoEnd()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)rS_.taskGameObject)
		{
			bool automatic = rS_.taskGameObject.GetComponent<taskTraining>().automatic;
			rS_.taskGameObject.GetComponent<taskTraining>().automatic = !automatic;
		}
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_QA_Bugfixing()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[171]);
		guiMain_.uiObjects[171].GetComponent<Menu_QA_BugfixingSelectGame>().Init(rS_);
	}

	public void BUTTON_QA_GameplayVerbessern()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[172]);
		guiMain_.uiObjects[172].GetComponent<Menu_QA_GameplayVerbessern>().Init(rS_);
	}

	public void BUTTON_QA_Spielbericht()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[180]);
		guiMain_.uiObjects[180].GetComponent<Menu_QA_SpielberichtMain>().Init(rS_);
	}

	public void BUTTON_GFX_Polishing()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[279]);
		guiMain_.uiObjects[279].GetComponent<Menu_ROOM_Polishing>().Init(rS_);
	}

	public void BUTTON_GFX_GrafikVerbessern()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[174]);
		guiMain_.uiObjects[174].GetComponent<Menu_GFX_GrafikVerbessern>().Init(rS_);
	}

	public void BUTTON_SFX_SoundsVerbessern()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[176]);
		guiMain_.uiObjects[176].GetComponent<Menu_SFX_SoundVerbessern>().Init(rS_);
	}

	public void BUTTON_MOCAP_AnimationVerbessern()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[178]);
		guiMain_.uiObjects[178].GetComponent<Menu_MOCAP_AnimationVerbessern>().Init(rS_);
	}

	public void BUTTON_PROD_Packung()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[220]);
		guiMain_.uiObjects[220].GetComponent<Menu_PackungSelect>().Init(rS_);
	}

	public void BUTTON_PROD_Produce()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[221]);
		guiMain_.uiObjects[221].GetComponent<Menu_ProductionSelect>().Init(rS_);
	}

	public void BUTTON_PROD_Automatik()
	{
		sfx_.PlaySound(3, force: true);
		taskProduction taskProduction2 = guiMain_.AddTask_Production();
		taskProduction2.Init(fromSavegame: false);
		taskProduction2.targetID = -1;
		taskProduction2.automatic = true;
		taskProduction2.produceAutomatikAllGames = true;
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskProduction2.myID;
		}
		guiMain_.CloseAllRoomButtons();
		guiMain_.CloseMenu();
	}

	public void BUTTON_SERVER_AboPreis()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[245]);
	}

	public void BUTTON_SERVER_Shutdown()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[244]);
		guiMain_.uiObjects[244].GetComponent<Menu_W_ServerDown>().Init(rS_);
	}

	public void BUTTON_SERVER_Reservieren(int i)
	{
		if ((i != 2 || unlock_.Get(22)) && (i != 3 || unlock_.Get(68)))
		{
			sfx_.PlaySound(3, force: true);
			rS_.serverReservieren = i;
			CloseAllMenus();
			mS_.PauseGame(p: false);
		}
	}

	public void BUTTON_SERVER_F2P()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.MessageBox(tS_.GetText(408), closeMenu: true);
	}

	public void BUTTON_SERVER_MMOtoF2P()
	{
		if (unlock_.Get(22))
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.OpenMenu(hideChars: false);
			guiMain_.ActivateMenu(guiMain_.uiObjects[285]);
		}
	}

	public void BUTTON_SERVER_GamePass()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[415]);
	}

	public void BUTTON_WERK_Produce()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[304]);
		guiMain_.uiObjects[304].GetComponent<Menu_ProductionArcadeSelect>().Init(rS_);
	}

	public void BUTTON_WERK_Automatik()
	{
		sfx_.PlaySound(3, force: true);
		taskArcadeProduction taskArcadeProduction2 = guiMain_.AddTask_ArcadeProduction();
		taskArcadeProduction2.Init(fromSavegame: false);
		taskArcadeProduction2.targetID = -1;
		taskArcadeProduction2.points = 25f;
		taskArcadeProduction2.pointsLeft = 25f;
		taskArcadeProduction2.produceAutomatikAllGames = true;
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskArcadeProduction2.myID;
		}
		guiMain_.CloseAllRoomButtons();
		guiMain_.CloseMenu();
	}

	public void BUTTON_HARD_NeueKonsoleEntwickeln()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[317]);
		guiMain_.uiObjects[317].GetComponent<Menu_Dev_KonsoleMain>().Init(rS_);
	}

	public void BUTTON_HARD_Kostenoptimierung()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[450]);
		guiMain_.uiObjects[450].GetComponent<Menu_Dev_KonsoleKostenreduktion>().Init(rS_);
	}

	public void BUTTON_HARD_Haltbarkeit()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[452]);
		guiMain_.uiObjects[452].GetComponent<Menu_Dev_KonsoleHaltbarkeit>().Init(rS_);
	}

	public void BUTTON_HARD_Entwicklungsbericht()
	{
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskKonsole component = gameObject.GetComponent<taskKonsole>();
			if ((bool)component)
			{
				GameObject gameObject2 = GameObject.Find("PLATFORM_" + component.konsoleID);
				if ((bool)gameObject2)
				{
					platformScript component2 = gameObject2.GetComponent<platformScript>();
					if ((bool)component2)
					{
						guiMain_.ActivateMenu(guiMain_.uiObjects[325]);
						guiMain_.uiObjects[325].GetComponent<Menu_Dev_KonsoleEntwicklungsbericht>().Init(component2, rS_);
					}
				}
			}
		}
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
	}

	public void BUTTON_HARD_KonsolenFeatures()
	{
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskKonsole component = gameObject.GetComponent<taskKonsole>();
			if ((bool)component)
			{
				GameObject gameObject2 = GameObject.Find("PLATFORM_" + component.konsoleID);
				if ((bool)gameObject2)
				{
					platformScript component2 = gameObject2.GetComponent<platformScript>();
					if ((bool)component2)
					{
						guiMain_.ActivateMenu(guiMain_.uiObjects[405]);
						guiMain_.uiObjects[405].GetComponent<Menu_Dev_ChangeKonsolenFeature>().Init(component2);
					}
				}
			}
		}
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
	}

	public void BUTTON_ContractWork_AutoEnd()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)rS_.taskGameObject)
		{
			bool automatic = rS_.taskGameObject.GetComponent<taskContractWork>().automatic;
			rS_.taskGameObject.GetComponent<taskContractWork>().automatic = !automatic;
		}
		CloseAllMenus();
		mS_.PauseGame(p: false);
	}

	public void BUTTON_Aufgabe_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[94]);
		guiMain_.uiObjects[94].GetComponent<Menu_W_Aufgabe_Abbrechen>().Init(rS_);
	}
}
