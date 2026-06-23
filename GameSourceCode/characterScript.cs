using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

public class characterScript : MonoBehaviour
{
	public mainScript mS_;

	public Camera camera_;

	public mainCameraScript mCamS_;

	public GameObject main_;

	public settingsScript settings_;

	public GUI_Tooltip guiTooltip;

	public GUI_Main guiMain_;

	public sfxScript sfx_;

	public textScript tS_;

	public clipScript clipS_;

	public roomDataScript rdS_;

	public movementScript moveS_;

	public mapScript mapS_;

	public SkinnedMeshRenderer myRenderer;

	public SkinnedMeshRenderer myLodRenderer;

	public GameObject[] uiPrefabs;

	public Color[] colors;

	public int myID;

	public bool male;

	public string myName;

	public int group = -1;

	public int roomID = -1;

	public roomScript roomS_;

	public int objectUsingID = -1;

	public objectScript objectUsingS_;

	public int objectBelegtID = -1;

	public objectScript objectBelegtS_;

	public objectScript mainArbeitsplatzS_;

	public int gehalt;

	public int beruf;

	public float s_motivation;

	public float s_gamedesign;

	public float s_programmieren;

	public float s_grafik;

	public float s_sound;

	public float s_pr;

	public float s_gametests;

	public float s_technik;

	public float s_forschen;

	public bool[] perks;

	public int legend = -1;

	public float workProgress;

	public float durst;

	public float hunger;

	public float klo;

	public float waschbecken;

	public float muell;

	public float giessen;

	public float pause;

	public float freezer;

	public int krank;

	private bool outline;

	private bool uiVisible;

	public bool picked;

	public bool hided;

	public int model_body = -1;

	public int model_eyes = -1;

	public int model_hair = -1;

	public int model_beard = -1;

	public int model_skinColor = -1;

	public int model_hairColor = -1;

	public int model_beardColor = -1;

	public int model_HoseColor = -1;

	public int model_ShirtColor = -1;

	public int model_Add1Color = -1;

	private GameObject myUI;

	private RectTransform myUIRect;

	private GameObject uiIconMain;

	private GameObject uiWorkProgress;

	private Image uiWorkProgress_Image;

	private GameObject uiIcon;

	private Image uiIcon_Image;

	private GameObject uiNoRoom;

	private GameObject uiSprechblase;

	private GameObject uiKrank;

	private GameObject uiLeitenderDesigner;

	private GameObject uiFrieren;

	private GameObject uiGarbage;

	private GameObject uiUeberfuellt;

	private GameObject uiLowQuality;

	private GameObject[] ePop_Object;

	private Animation[] ePopAnim;

	private Text[] ePopText;

	private int ePop_Gameplay;

	private int ePop_Grafik = 1;

	private int ePop_Sound = 2;

	private int ePop_Technik = 3;

	private int ePop_Bug = 4;

	private int ePop_BugRemove = 5;

	private int ePop_Forschung = 6;

	private int ePop_Marketing = 7;

	private int ePop_Support = 8;

	private int ePop_QA = 9;

	private int ePop_Training = 10;

	private int ePop_Hype = 11;

	private int ePop_BeduerfnisErfuellt = 12;

	private int ePop_BeduerfnisNichtErfuellt = 13;

	private int ePop_Arzt = 14;

	private int ePop_ProdArcade = 15;

	private int ePop_Hardware = 16;

	private int ePop_Misc = 17;

	public GameObject[] addObjects;

	public LayerMask layerMaskChar;

	public LayerMask layerMaskFloor;

	private float invisibleTimer;

	private float updateIconTimer;

	private float timerForMovementActions;

	private float timerUpdateBeduerfnisse;

	private float deltaTimeUpdateBeduerfnisse;

	private int aktuellesUiIcon = -1;

	private float timerUpdateWork;

	private float deltaTimeUpdateWork;

	public bool iDoWork;

	private Menu_Training_Select menuTrain_;

	private bool updateLeitenderEntwicklerIcon = true;

	private void Start()
	{
		FindScripts();
		InitUI();
		mS_.findCharacters = true;
		if ((bool)mS_.achScript_ && legend != -1)
		{
			mS_.achScript_.SetAchivement(68);
		}
	}

	private void OnDestroy()
	{
		if ((bool)mS_)
		{
			mS_.findCharacters = true;
		}
	}

	private void FindScripts()
	{
		if (!main_)
		{
			if (!main_)
			{
				main_ = GameObject.FindWithTag("Main");
			}
			if (!mS_)
			{
				mS_ = main_.GetComponent<mainScript>();
			}
			if (!rdS_)
			{
				rdS_ = main_.GetComponent<roomDataScript>();
			}
			if (!clipS_)
			{
				clipS_ = main_.GetComponent<clipScript>();
			}
			if (!settings_)
			{
				settings_ = main_.GetComponent<settingsScript>();
			}
			if (!camera_)
			{
				camera_ = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
			}
			if (!mCamS_)
			{
				mCamS_ = GameObject.FindWithTag("MainCamera").GetComponent<mainCameraScript>();
			}
			if (!guiTooltip)
			{
				guiTooltip = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Tooltip>();
			}
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			}
			if (!sfx_)
			{
				sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
			}
			if (!moveS_)
			{
				moveS_ = GetComponent<movementScript>();
			}
			if (!mapS_)
			{
				mapS_ = main_.GetComponent<mapScript>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
		}
	}

	public void Init()
	{
		base.name = "CHAR_" + myID;
		s_motivation = 100f;
		durst = Random.Range(15f, 100f);
		hunger = Random.Range(15f, 100f);
		klo = Random.Range(15f, 100f);
		waschbecken = Random.Range(15f, 100f);
		muell = Random.Range(15f, 100f);
		giessen = Random.Range(15f, 100f);
		pause = Random.Range(15f, 100f);
		freezer = Random.Range(15f, 100f);
	}

	public void UpdateMe()
	{
		UpdateMyRoom();
		UpdateUsingObject();
		UpdateBelegtObject();
		UpdateBeduerfnisse();
		UpdateWork();
		UpdateUI();
		if ((bool)moveS_)
		{
			moveS_.UpdateMe();
		}
	}

	private void InitUI()
	{
		myUI = Object.Instantiate(uiPrefabs[0], new Vector3(99999f, 99999f, 0f), Quaternion.identity);
		myUI.transform.SetParent(mS_.guiPops.transform);
		myUI.transform.SetSiblingIndex(0);
		myUIRect = myUI.GetComponent<RectTransform>();
		uiIconMain = myUI.transform.Find("IconMain").gameObject;
		uiWorkProgress = uiIconMain.transform.Find("WorkProgress").gameObject;
		if ((bool)uiWorkProgress)
		{
			uiWorkProgress_Image = uiWorkProgress.GetComponent<Image>();
		}
		uiIcon = uiIconMain.transform.Find("Icon").gameObject;
		if ((bool)uiIcon)
		{
			uiIcon_Image = uiIcon.GetComponent<Image>();
		}
		uiNoRoom = myUI.transform.Find("iNoRoom").gameObject;
		uiSprechblase = myUI.transform.Find("Sprechblase").gameObject;
		uiSprechblase.SetActive(value: false);
		uiKrank = uiIconMain.transform.Find("IconKrank").gameObject;
		uiKrank.SetActive(value: false);
		uiLeitenderDesigner = uiIconMain.transform.Find("IconLeitenderDesigner").gameObject;
		uiLeitenderDesigner.SetActive(value: false);
		uiFrieren = uiIconMain.transform.Find("IconFrieren").gameObject;
		uiFrieren.SetActive(value: false);
		uiGarbage = uiIconMain.transform.Find("IconGarbage").gameObject;
		uiGarbage.SetActive(value: false);
		uiUeberfuellt = uiIconMain.transform.Find("IconUeberfuellt").gameObject;
		uiUeberfuellt.SetActive(value: false);
		uiLowQuality = uiIconMain.transform.Find("IconLowQuality").gameObject;
		uiLowQuality.SetActive(value: false);
		ePop_Object = new GameObject[20];
		ePopAnim = new Animation[20];
		ePopText = new Text[20];
		ePop_Object[ePop_Gameplay] = myUI.transform.Find("eGameplay").gameObject;
		ePop_Object[ePop_Grafik] = myUI.transform.Find("eGraphic").gameObject;
		ePop_Object[ePop_Sound] = myUI.transform.Find("eSound").gameObject;
		ePop_Object[ePop_Technik] = myUI.transform.Find("eTechnik").gameObject;
		ePop_Object[ePop_Bug] = myUI.transform.Find("eBug").gameObject;
		ePop_Object[ePop_BugRemove] = myUI.transform.Find("eBugRemove").gameObject;
		ePop_Object[ePop_Forschung] = myUI.transform.Find("eForschung").gameObject;
		ePop_Object[ePop_Marketing] = myUI.transform.Find("eMarketing").gameObject;
		ePop_Object[ePop_Support] = myUI.transform.Find("eSupport").gameObject;
		ePop_Object[ePop_QA] = myUI.transform.Find("eQA").gameObject;
		ePop_Object[ePop_Training] = myUI.transform.Find("eTraining").gameObject;
		ePop_Object[ePop_Hype] = myUI.transform.Find("eHype").gameObject;
		ePop_Object[ePop_BeduerfnisErfuellt] = myUI.transform.Find("eBeduerfnisErfuellt").gameObject;
		ePop_Object[ePop_BeduerfnisNichtErfuellt] = myUI.transform.Find("eBeduerfnisNichtErfuellt").gameObject;
		ePop_Object[ePop_Arzt] = myUI.transform.Find("eArzt").gameObject;
		ePop_Object[ePop_ProdArcade] = myUI.transform.Find("eProdArcade").gameObject;
		ePop_Object[ePop_Hardware] = myUI.transform.Find("eHardware").gameObject;
		ePop_Object[ePop_Misc] = myUI.transform.Find("eMisc").gameObject;
		for (int i = 0; i < ePop_Object.Length; i++)
		{
			if ((bool)ePop_Object[i])
			{
				ePopAnim[i] = ePop_Object[i].GetComponent<Animation>();
				if (ePop_Object[i].transform.childCount > 0)
				{
					ePopText[i] = ePop_Object[i].transform.GetChild(0).GetComponent<Text>();
				}
			}
		}
		HidePops();
	}

	private void HidePops()
	{
		Vector3 localScale = new Vector3(0f, 0f, 0f);
		for (int i = 0; i < ePop_Object.Length; i++)
		{
			if ((bool)ePop_Object[i])
			{
				ePop_Object[i].transform.localScale = localScale;
			}
		}
	}

	public void StopPopAnimations()
	{
		for (int i = 0; i < ePop_Object.Length; i++)
		{
			if ((bool)ePop_Object[i] && ePop_Object[i].activeSelf && ePop_Object[i].transform.localScale.x <= 0f && ePopAnim[i].enabled)
			{
				ePopAnim[i].enabled = false;
			}
		}
	}

	public void HideChar()
	{
		if (!picked)
		{
			hided = true;
			base.transform.localScale = new Vector3(0f, 0f, 0f);
		}
	}

	public void UnhideChar()
	{
		hided = false;
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		HidePops();
	}

	public void UpdateKI(bool roomSpecific)
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (!mS_.personal_ki || picked || (roomID != -1 && (bool)roomS_ && roomS_.lockKI))
		{
			return;
		}
		bool flag = false;
		bool flag2 = false;
		if (roomID != -1 && (bool)roomS_)
		{
			if (roomS_.taskID == -1)
			{
				flag = true;
			}
			if (!mainArbeitsplatzS_ && objectBelegtID == -1)
			{
				flag2 = true;
			}
		}
		if (roomID != -1 && !flag && !flag2)
		{
			return;
		}
		List<roomScript> list = new List<roomScript>();
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if (!mS_.arrayRoomScripts[i])
			{
				continue;
			}
			roomScript roomScript2 = mS_.arrayRoomScripts[i];
			if ((bool)roomScript2 && !roomScript2.lockKI && roomScript2.GetArbeitsplaetze() > 0 && roomScript2.typ != 13 && roomScript2.GetArbeitsplaetze() > roomScript2.GetMitarbeiter() && roomScript2.taskID != -1)
			{
				int num = Mathf.RoundToInt(base.transform.position.x);
				int num2 = Mathf.RoundToInt(base.transform.position.z);
				if (mapS_.IsInMapLimit(num, num2) && (!mS_.personal_dontLeaveBuilding || mapS_.mapBuilding[num, num2] == mapS_.mapBuilding[Mathf.RoundToInt(roomScript2.uiPos.x), Mathf.RoundToInt(roomScript2.uiPos.z)]))
				{
					list.Add(roomScript2);
				}
			}
		}
		for (int j = 0; j < list.Count; j++)
		{
			if ((bool)list[j])
			{
				if (!roomSpecific)
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
				if (list[j].typ == 4 && beruf == 2)
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
				if (list[j].typ == 5 && beruf == 3)
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
				if (list[j].typ == 3 && beruf == 5)
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
				if (list[j].typ == 10 && beruf == 1)
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
				if (list[j].typ == 2 && beruf == 7)
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
				if (list[j].typ == 6 && beruf == 4)
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
				if (list[j].typ == 7 && beruf == 4)
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
				if (list[j].typ == 17 && beruf == 6)
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
				if (list[j].typ == 8 && beruf == 6)
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
				if (list[j].typ == 1 && (beruf == 2 || beruf == 3 || beruf == 1 || beruf == 0))
				{
					list[j].mitarbeiterZugeteilt++;
					roomS_ = list[j];
					roomID = list[j].myID;
					RemoveObjectUsing();
					mainArbeitsplatzS_ = null;
					break;
				}
			}
		}
	}

	private void UpdateMyRoom()
	{
		if (roomID == -1)
		{
			roomS_ = null;
		}
		else
		{
			if ((bool)roomS_)
			{
				return;
			}
			GameObject gameObject = GameObject.Find("Room_" + roomID);
			if ((bool)gameObject)
			{
				roomS_ = gameObject.GetComponent<roomScript>();
				if (!mS_.settings_TutorialOff && roomS_.typ == 1)
				{
					guiMain_.SetTutorialStep(9);
				}
			}
			else
			{
				roomID = -1;
				moveS_.waitForceAnimation = 0.01f;
			}
		}
	}

	public void UpdateUI()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (!myUI)
		{
			return;
		}
		if (!IsVisible() || settings_.disableMitarbeiterIcons)
		{
			uiVisible = false;
			if (myUI.activeSelf)
			{
				myUI.SetActive(value: false);
			}
			return;
		}
		bool flag = false;
		if (guiMain_.menuOpen && !guiMain_.uiObjects[15].activeSelf)
		{
			flag = true;
		}
		if (hided || picked || flag || !IsVisible())
		{
			uiVisible = false;
			if (myUI.activeSelf)
			{
				myUI.SetActive(value: false);
			}
			return;
		}
		if (!uiVisible)
		{
			invisibleTimer += Time.deltaTime;
			if (invisibleTimer < 0.1f)
			{
				return;
			}
			invisibleTimer = 0f;
		}
		Vector3 position = base.gameObject.transform.position;
		position.y += 1f;
		Vector2 vector = camera_.WorldToScreenPoint(position);
		if (vector.x >= 0f && vector.x <= (float)Screen.width && vector.y >= 0f && vector.y <= (float)Screen.height)
		{
			uiVisible = true;
			if (!myUI.activeSelf)
			{
				myUI.SetActive(value: true);
				myUI.GetComponent<Animation>().enabled = true;
			}
			vector = new Vector2(vector.x, vector.y - (float)Screen.height);
			myUIRect.anchoredPosition = guiMain_.GetAnchoredPosition(vector);
			updateIconTimer += Time.deltaTime;
			if (updateIconTimer >= 0.04f)
			{
				updateIconTimer = 0f;
				UpdateIcon();
			}
			UpdateSprechblase();
			DrawRoomLine();
		}
		else
		{
			uiVisible = false;
			if (myUI.activeSelf)
			{
				myUI.SetActive(value: false);
				HidePops();
			}
		}
	}

	private IEnumerator CreatePopInSeconds_SPRITE(Sprite sprite_, string text_, float waitTime, int sound)
	{
		if (!settings_.disableWorkIcons)
		{
			yield return new WaitForSeconds(waitTime);
			CreatePop_SPRITE(sprite_, text_, sound);
		}
	}

	private void CreatePop_SPRITE(Sprite sprite_, string text_, int sound)
	{
		if (uiVisible)
		{
			if (!ePop_Object[ePop_Misc].activeSelf)
			{
				ePop_Object[ePop_Misc].SetActive(value: true);
			}
			ePopAnim[ePop_Misc].enabled = true;
			ePopAnim[ePop_Misc].Stop();
			ePopAnim[ePop_Misc].Play();
			if (!settings_.disableWorkIconSound)
			{
				sfx_.PlaySound(sound, force: true);
			}
			ePop_Object[ePop_Misc].GetComponent<Image>().sprite = sprite_;
			ePopText[ePop_Misc].text = text_;
		}
	}

	private IEnumerator CreatePopInSeconds(int popID, float f, float waitTime, int sound)
	{
		if (!settings_.disableWorkIcons)
		{
			yield return new WaitForSeconds(waitTime);
			f = mS_.Round(f, 2);
			CreatePop(popID, f, sound);
		}
	}

	private void CreatePop(int popID, float f, int sound)
	{
		if (uiVisible)
		{
			if (!ePop_Object[popID].activeSelf)
			{
				ePop_Object[popID].SetActive(value: true);
			}
			ePopAnim[popID].enabled = true;
			ePopAnim[popID].Stop();
			ePopAnim[popID].Play();
			if (!settings_.disableWorkIconSound)
			{
				sfx_.PlaySound(sound, force: true);
			}
			if ((bool)ePopText[popID])
			{
				ePopText[popID].text = "+" + f;
			}
		}
	}

	public int GetBestSkill()
	{
		float num = 0f;
		num = s_gamedesign;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 0;
		}
		num = s_programmieren;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 1;
		}
		num = s_grafik;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 2;
		}
		num = s_sound;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 3;
		}
		num = s_pr;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 4;
		}
		num = s_gametests;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 5;
		}
		num = s_technik;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 6;
		}
		num = s_forschen;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return 7;
		}
		return 0;
	}

	public float GetBestSkillValue()
	{
		float num = 0f;
		num = s_gamedesign;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_programmieren;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_grafik;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_sound;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_pr;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_gametests;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_technik;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik && num >= s_forschen)
		{
			return num;
		}
		num = s_forschen;
		if (num >= s_gamedesign && num >= s_programmieren && num >= s_grafik && num >= s_sound && num >= s_pr && num >= s_gametests && num >= s_technik)
		{
			_ = s_forschen;
			return num;
		}
		return num;
	}

	public string GetTooltip()
	{
		if (!tS_)
		{
			return "";
		}
		string text = "";
		text = "<b>" + GetGroupString("magenta") + " " + myName + "</b>";
		if (group != -1 && mS_.personal_group_names[group - 1].Length > 0)
		{
			text = text + "\n" + GetGroupStringWithName("magenta");
		}
		if (perks[0])
		{
			text = text + "\n<b><color=green>" + tS_.GetText(506) + "</color></b>";
		}
		text = text + "\n<color=blue>" + tS_.GetText(137 + beruf) + "</color>";
		if (GetBestSkill() == 0)
		{
			text = text + "\n" + tS_.GetText(119) + "[" + mS_.Round(s_gamedesign, 1) + "]";
		}
		if (GetBestSkill() == 1)
		{
			text = text + "\n" + tS_.GetText(120) + " [" + mS_.Round(s_programmieren, 1) + "]";
		}
		if (GetBestSkill() == 2)
		{
			text = text + "\n" + tS_.GetText(121) + " [" + mS_.Round(s_grafik, 1) + "]";
		}
		if (GetBestSkill() == 3)
		{
			text = text + "\n" + tS_.GetText(122) + " [" + mS_.Round(s_sound, 1) + "]";
		}
		if (GetBestSkill() == 4)
		{
			text = text + "\n" + tS_.GetText(123) + " [" + mS_.Round(s_pr, 1) + "]";
		}
		if (GetBestSkill() == 5)
		{
			text = text + "\n" + tS_.GetText(124) + " [" + mS_.Round(s_gametests, 1) + "]";
		}
		if (GetBestSkill() == 6)
		{
			text = text + "\n" + tS_.GetText(125) + " [" + mS_.Round(s_technik, 1) + "]";
		}
		if (GetBestSkill() == 7)
		{
			text = text + "\n" + tS_.GetText(126) + " [" + mS_.Round(s_forschen, 1) + "]";
		}
		text = text + "\n" + tS_.GetText(109) + " [" + mS_.Round(s_motivation, 1) + "]";
		if ((bool)objectBelegtS_ && !objectUsingS_ && !objectBelegtS_.isGhost)
		{
			text = text + "\n\n<b><color=blue>" + tS_.GetText(314) + " " + tS_.GetObjects(objectBelegtS_.typ) + "</color></b>";
		}
		if ((bool)objectUsingS_ && !objectUsingS_.isGhost)
		{
			text = text + "\n\n<b><color=blue>" + tS_.GetText(315) + " " + tS_.GetObjects(objectUsingS_.typ) + "</color></b>";
		}
		if (roomID == -1)
		{
			text = text + "\n<b><color=red>" + tS_.GetText(507) + "</color></b>";
		}
		if (krank > 0)
		{
			text = text + "\n<b><color=red>" + tS_.GetText(773) + "</color></b>";
		}
		int num = Mathf.RoundToInt(base.transform.position.x);
		int num2 = Mathf.RoundToInt(base.transform.position.z);
		if (mapS_.IsInMapLimit(num, num2))
		{
			if (!perks[16] && mapS_.mapMuell[num, num2] > 0f)
			{
				text = text + "\n<b><color=red>" + tS_.GetText(1287) + "</color></b>";
			}
			if (mapS_.mapRoomID[num, num2] != 0)
			{
				if (!perks[11] && mapS_.mapWaerme[num, num2] <= 0.2f)
				{
					text = text + "\n<b><color=red>" + tS_.GetText(1285) + "</color></b>";
				}
				if (!perks[12] && mapS_.mapAusstattung[num, num2] <= 0.2f)
				{
					text = text + "\n<b><color=red>" + tS_.GetText(1288) + "</color></b>";
				}
				if (!perks[17] && IsUeberfuellt())
				{
					text = text + "\n<b><color=red>" + tS_.GetText(1303) + "</color></b>";
				}
			}
		}
		return text;
	}

	private void DrawRoomLine()
	{
		if (guiMain_.menuOpen || base.gameObject.transform.GetChild(0).gameObject.layer != 11 || !roomS_)
		{
			return;
		}
		if (!mS_.roomLine.active)
		{
			mS_.roomLine = new VectorLine("mainGUI_RoomLine", new List<Vector3>(2), 20f, LineType.Continuous, Joins.Weld);
			mS_.roomLine.endCap = "ArrowsCharRoom";
			return;
		}
		GameObject gameObject = mS_.roomLine.rectTransform.gameObject;
		if ((bool)gameObject.GetComponent<MeshRenderer>())
		{
			gameObject.GetComponent<MeshRenderer>().material.shader = mS_.shaders[0];
		}
		mS_.gameObject.transform.position = roomS_.uiPos;
		mS_.gameObject.transform.LookAt(base.transform.position);
		mS_.gameObject.transform.Translate(Vector3.forward * 0.7f);
		Vector3 position = mS_.gameObject.transform.position;
		mS_.gameObject.transform.position = base.transform.position;
		mS_.gameObject.transform.LookAt(roomS_.uiPos);
		mS_.gameObject.transform.Translate(Vector3.forward * 0.7f);
		Vector3 position2 = mS_.gameObject.transform.position;
		mS_.gameObject.transform.position = new Vector3(0f, 0f, 0f);
		mS_.roomLine.points3[0] = position;
		mS_.roomLine.points3[1] = position2;
		mS_.roomLine.color = guiMain_.colors[12];
		mS_.roomLine.Draw3D();
	}

	public void MouseOver()
	{
		SetOutlineLayer();
		guiTooltip.SetActive(GetTooltip());
		if ((bool)guiMain_)
		{
			guiMain_.uiObjects[214].SetActive(value: true);
			guiMain_.uiObjects[214].GetComponent<Menu_TooltipCharacter>().Init(this);
		}
	}

	public void MouseLeave()
	{
		if (mS_.roomLine.active)
		{
			mS_.roomLine.points3[0] = new Vector3(0f, 0f, 0f);
			mS_.roomLine.points3[1] = new Vector3(0f, 0f, 0f);
			mS_.roomLine.Draw3D();
		}
		DisableOutlineLayer();
		guiTooltip.SetInactive();
		if ((bool)guiMain_)
		{
			guiMain_.uiObjects[214].SetActive(value: false);
		}
	}

	public void SetOutlineLayer()
	{
		mCamS_.EnablePostLiner();
		if (!outline)
		{
			outline = true;
			if ((bool)mCamS_)
			{
				mCamS_.SetOutlineColor(2, 0.3f, 4);
			}
			SetLayer(11, base.gameObject.transform.GetChild(0));
		}
	}

	private void DisableOutlineLayer()
	{
		if (outline)
		{
			outline = false;
			SetLayer(0, base.gameObject.transform.GetChild(0));
			mCamS_.DisablePostLineRenderer();
		}
	}

	private void SetLayer(int newLayer, Transform trans)
	{
		trans.gameObject.layer = newLayer;
		foreach (Transform tran in trans)
		{
			tran.gameObject.layer = newLayer;
			if (tran.childCount > 0)
			{
				SetLayer(newLayer, tran.transform);
			}
		}
	}

	public void PickUp()
	{
		if ((bool)objectUsingS_)
		{
			if (moveS_.currentAnimation == clipS_.hash_wc && (bool)objectUsingS_ && objectUsingS_.isWC)
			{
				objectUsingS_.gfxAnimation.Play("wcKabine1Auf");
			}
			if ((bool)objectUsingS_.gfxShow)
			{
				objectUsingS_.gfxShow.SetActive(value: false);
			}
		}
		picked = true;
		base.gameObject.layer = 2;
		roomID = -1;
		roomS_ = null;
		mS_.AddPickedChar(base.gameObject);
		RemoveObjectUsing();
		guiMain_.uiObjects[15].GetComponent<Menu_PickCharacter>().AddCharToList(this);
	}

	public void DropChar(Vector3 v)
	{
		picked = false;
		base.gameObject.layer = 12;
		mS_.RemovePickedChar(base.gameObject);
		guiMain_.CloseMenu();
		base.transform.position = new Vector3(v.x, 0f, v.z);
		sfx_.PlaySound(10, force: true);
		moveS_.SetAnimationForce(clipS_.hash_drop, clipS_.clip_drop);
		moveS_.RecalculatePath();
		mainArbeitsplatzS_ = null;
		int num = Mathf.RoundToInt(v.x);
		int num2 = Mathf.RoundToInt(v.z);
		if (mapS_.mapRoomID[num, num2] > 1)
		{
			if ((bool)mapS_.mapRoomScript[num, num2])
			{
				if (!rdS_.KeineMitarbeiter(mapS_.mapRoomScript[num, num2].typ))
				{
					roomID = mapS_.mapRoomID[num, num2];
				}
			}
			else
			{
				roomID = -1;
				roomS_ = null;
			}
		}
		else
		{
			roomID = -1;
			roomS_ = null;
		}
		moveS_.DeleteTarget();
		guiMain_.DeactivateMenu(guiMain_.uiObjects[15]);
		guiMain_.CloseMenu();
	}

	public void RemoveObjectUsing()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (objectUsingID != -1 && (bool)objectUsingS_ && (objectUsingS_.isArbeitsplatz || objectUsingS_.isSeat || objectUsingS_.isSeatAufenthalt))
		{
			base.transform.position = new Vector3(objectUsingS_.vWaypoint.x, 0f, objectUsingS_.vWaypoint.z);
		}
		moveS_.waitForceAnimation = 0f;
		moveS_.DeleteTarget();
		if (objectUsingID != -1)
		{
			objectUsingID = -1;
			objectUsingS_ = null;
		}
		if (objectBelegtID == -1)
		{
			return;
		}
		if (!objectBelegtS_)
		{
			for (int i = 0; i < mS_.arrayObjectScripts.Length; i++)
			{
				if ((bool)mS_.arrayObjectScripts[i] && mS_.arrayObjectScripts[i].myID == objectBelegtID)
				{
					mS_.arrayObjectScripts[i].besetztCharID = -1;
				}
			}
		}
		else
		{
			objectBelegtS_.besetztCharID = -1;
		}
		objectBelegtID = -1;
		objectBelegtS_ = null;
	}

	private void UpdateUsingObject()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (objectUsingID == -1 || (bool)objectUsingS_ || !mS_)
		{
			return;
		}
		for (int i = 0; i < mS_.arrayObjectScripts.Length; i++)
		{
			if ((bool)mS_.arrayObjectScripts[i] && mS_.arrayObjectScripts[i].myID == objectUsingID)
			{
				objectUsingS_ = mS_.arrayObjectScripts[i];
				return;
			}
		}
		RemoveObjectUsing();
	}

	private void UpdateBelegtObject()
	{
		if (objectBelegtID == -1 || (bool)objectBelegtS_)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		if (!mS_)
		{
			return;
		}
		for (int i = 0; i < mS_.arrayObjectScripts.Length; i++)
		{
			if ((bool)mS_.arrayObjectScripts[i] && mS_.arrayObjectScripts[i].myID == objectBelegtID)
			{
				objectBelegtS_ = mS_.arrayObjectScripts[i];
				return;
			}
		}
		RemoveObjectUsing();
	}

	public void UpdateKuendigen(bool force)
	{
		if ((bool)mS_ && !perks[4] && !perks[0] && !(mS_.GetGameSpeed() <= 0f) && (force || (!(s_motivation > 0f) && objectUsingID == -1 && Random.Range(0, 500) != 1)))
		{
			RemoveObjectUsing();
			guiMain_.ActivateMenu(guiMain_.uiObjects[84]);
			guiMain_.uiObjects[84].GetComponent<Menu_MitarberKuendigt>().Init(this);
		}
	}

	public void UpdateKrank()
	{
		if (!mS_ || perks[10] || (mS_.settings_sandbox && mS_.sandbox_mitarbeiterKrank))
		{
			return;
		}
		if (krank <= 0)
		{
			int num = 0;
			if (perks[19])
			{
				num = 1;
			}
			if (Random.Range(0, 100) <= num || (mS_.globalEvent == 7 && Random.Range(0, 100) >= 98))
			{
				krank = Random.Range(2, 8);
				if (IsVisible())
				{
					sfx_.PlaySound(46);
				}
			}
		}
		else
		{
			krank--;
		}
	}

	private void UpdateBeduerfnisse()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (mS_.GetDeltaTime() <= 0f || !moveS_ || !mapS_ || picked || moveS_.waitForceAnimation > 0f)
		{
			return;
		}
		timerForMovementActions += Time.deltaTime;
		deltaTimeUpdateBeduerfnisse += mS_.GetDeltaTime();
		timerUpdateBeduerfnisse += Time.deltaTime;
		if (timerUpdateBeduerfnisse < 0.1f)
		{
			return;
		}
		timerUpdateBeduerfnisse = 0f;
		float num = deltaTimeUpdateBeduerfnisse;
		deltaTimeUpdateBeduerfnisse = 0f;
		float num2 = 0.1f;
		int num3 = Mathf.RoundToInt(base.transform.position.x);
		int num4 = Mathf.RoundToInt(base.transform.position.z);
		if (!mapS_.IsInMapLimit(num3, num4))
		{
			return;
		}
		float num5 = 0f;
		if (!perks[16])
		{
			num5 += mapS_.mapMuell[num3, num4] * 0.02f;
		}
		if (mapS_.mapRoomID[num3, num4] != 0)
		{
			if (!perks[12] && mapS_.mapAusstattung[num3, num4] <= 0.2f)
			{
				num5 += 0.02f;
				if (iDoWork)
				{
					guiMain_.ShowBeschwerde(7, myName);
				}
			}
			if (!perks[11] && mapS_.mapWaerme[num3, num4] <= 0.2f)
			{
				num5 += 0.02f;
				if (iDoWork)
				{
					guiMain_.ShowBeschwerde(8, myName);
				}
			}
			if (!perks[17] && IsUeberfuellt())
			{
				num5 += 0.02f;
				if (iDoWork)
				{
					guiMain_.ShowBeschwerde(9, myName);
				}
			}
		}
		if (!mS_.settings_sabotageOff && mS_.sabotage_motivation > 0)
		{
			num5 *= 2f;
		}
		AddMotivation((0f - num5) * num);
		if (roomID <= -1 || objectUsingID == -1 || !objectUsingS_ || !objectUsingS_.isArbeitsplatz)
		{
			return;
		}
		if (iDoWork)
		{
			AddMotivation(-0.02f * num);
			switch (mS_.personal_pausen)
			{
			case 1:
				AddMotivation(-0.01f * num);
				break;
			case 2:
				AddMotivation(-0.02f * num);
				break;
			}
			if (!perks[20] && (bool)roomS_ && roomS_.IsCrunchtimeRead())
			{
				return;
			}
		}
		durst -= num * num2;
		klo -= num * num2;
		waschbecken -= num * num2;
		giessen -= num * num2;
		pause -= num * num2;
		muell -= num * num2;
		if (durst < 0f)
		{
			durst = 0f;
		}
		if (klo < 0f)
		{
			klo = 0f;
		}
		if (waschbecken < 0f)
		{
			waschbecken = 0f;
		}
		if (giessen < 0f)
		{
			giessen = 0f;
		}
		if (pause < 0f)
		{
			pause = 0f;
		}
		if (muell < 0f)
		{
			muell = 0f;
		}
		if (mS_.settings_sandbox && mS_.sandbox_mitarbeiterPause)
		{
			durst = 100f;
			klo = 100f;
			waschbecken = 100f;
			giessen = 100f;
			pause = 100f;
			muell = 100f;
		}
		if (timerForMovementActions < 1f)
		{
			return;
		}
		timerForMovementActions = 0f;
		if (krank > 0)
		{
			if (moveS_.FindObjectInRoom(11, null, onlyInRoom: false))
			{
				return;
			}
			guiMain_.ShowBeschwerde(0, myName);
		}
		if (s_motivation <= (float)mS_.personal_motivation)
		{
			switch (Random.Range(0, 6))
			{
			case 0:
				if (moveS_.FindObjectInRoom(8, null, onlyInRoom: false))
				{
					return;
				}
				break;
			case 1:
				if (moveS_.FindObjectInRoom(9, null, onlyInRoom: false))
				{
					return;
				}
				break;
			case 2:
				if (moveS_.FindObjectInRoom(10, null, onlyInRoom: false))
				{
					return;
				}
				break;
			case 3:
				if (moveS_.FindObjectInRoom(12, null, onlyInRoom: false))
				{
					return;
				}
				break;
			case 4:
				if (moveS_.FindObjectInRoom(16, null, onlyInRoom: false))
				{
					return;
				}
				break;
			case 5:
				if (moveS_.FindObjectInRoom(18, null, onlyInRoom: false))
				{
					return;
				}
				break;
			}
			if (moveS_.FindObjectInRoom(17, null, onlyInRoom: false))
			{
				return;
			}
			if (s_motivation <= 0f)
			{
				guiMain_.ShowBeschwerde(11, myName);
			}
			else
			{
				guiMain_.ShowBeschwerde(1, myName);
			}
		}
		if (durst <= 0f)
		{
			durst = 0f;
			if (!moveS_.FindObjectInRoom(1, null, onlyInRoom: false))
			{
				GoToGhostObject(2, inRoom_: true);
				guiMain_.ShowBeschwerde(2, myName);
			}
		}
		else if (!perks[13] && klo <= 0f)
		{
			klo = 0f;
			if (!moveS_.FindObjectInRoom(4, null, onlyInRoom: false))
			{
				GoToGhostObject(8, inRoom_: false);
				guiMain_.ShowBeschwerde(3, myName);
			}
		}
		else if (waschbecken <= 0f)
		{
			waschbecken = 0f;
			if (!moveS_.FindObjectInRoom(5, null, onlyInRoom: false))
			{
				GoToGhostObject(10, inRoom_: false);
				guiMain_.ShowBeschwerde(4, myName);
			}
		}
		else if (!perks[8] && muell <= 0f)
		{
			muell = 0f;
			if (!moveS_.FindObjectInRoom(2, null, onlyInRoom: false))
			{
				GoToGhostObject(1, inRoom_: true);
				guiMain_.ShowBeschwerde(5, myName);
			}
		}
		else if (perks[9] && giessen <= 0f)
		{
			giessen = 0f;
			if (!moveS_.FindObjectInRoom(3, null, onlyInRoom: false))
			{
				GoToGhostObject(3, inRoom_: true);
				guiMain_.ShowBeschwerde(6, myName);
			}
		}
		else
		{
			if (perks[2] || !(pause <= 0f))
			{
				return;
			}
			pause = 0f;
			switch (Random.Range(0, 4))
			{
			case 0:
				moveS_.FindObjectInRoom(7, null, onlyInRoom: false);
				break;
			case 1:
				moveS_.FindObjectInRoom(13, null, onlyInRoom: false);
				break;
			case 2:
				moveS_.FindObjectInRoom(14, null, onlyInRoom: false);
				break;
			case 3:
				moveS_.FindObjectInRoom(15, null, onlyInRoom: false);
				break;
			}
			if (Random.Range(0, 100) > 50)
			{
				if (!moveS_.FindObjectInRoom(7, null, onlyInRoom: false) && !moveS_.FindObjectInRoom(13, null, onlyInRoom: false) && !moveS_.FindObjectInRoom(14, null, onlyInRoom: false))
				{
					moveS_.FindObjectInRoom(15, null, onlyInRoom: false);
				}
				return;
			}
			switch (Random.Range(0, 4))
			{
			case 0:
				GoToGhostObject(4, inRoom_: true);
				break;
			case 1:
				GoToGhostObject(5, inRoom_: true);
				break;
			case 2:
				GoToGhostObject(6, inRoom_: true);
				break;
			case 3:
				GoToGhostObject(7, inRoom_: true);
				break;
			}
		}
	}

	private void GoToGhostObject(int i, bool inRoom_)
	{
		GameObject gameObject = Object.Instantiate(mS_.miscGamePrefabs[i]);
		if (inRoom_)
		{
			gameObject.transform.position = roomS_.myDoor.transform.position;
			gameObject.GetComponent<objectScript>().InitGhostObject(i);
			gameObject.GetComponent<objectScript>().GetWaypoints();
			moveS_.FindObjectInRoom(-1, gameObject, onlyInRoom: false);
			return;
		}
		int num = Mathf.RoundToInt(base.transform.position.x);
		int num2 = Mathf.RoundToInt(base.transform.position.z);
		if (mapS_.IsInMapLimit(num, num2))
		{
			Vector2 vector = mapS_.FindRandomFloorInMyBuilding(mapS_.mapBuilding[num, num2]);
			gameObject.transform.position = new Vector3(vector.x, 0f, vector.y);
			gameObject.GetComponent<objectScript>().InitGhostObject(i);
			gameObject.GetComponent<objectScript>().GetWaypoints();
			moveS_.FindObjectInRoom(-1, gameObject, onlyInRoom: false);
		}
		else
		{
			Vector2 vector = mapS_.FindRandomFloor();
			gameObject.transform.position = new Vector3(vector.x, 0f, vector.y);
			gameObject.GetComponent<objectScript>().InitGhostObject(i);
			gameObject.GetComponent<objectScript>().GetWaypoints();
			moveS_.FindObjectInRoom(-1, gameObject, onlyInRoom: false);
		}
	}

	public void AddMotivation(float f)
	{
		if (perks[0])
		{
			s_motivation = 100f;
			return;
		}
		if (mS_.settings_sandbox && mS_.sandbox_mitarbeiterMotivation)
		{
			s_motivation = 100f;
			return;
		}
		s_motivation += f;
		if (s_motivation < 0f)
		{
			s_motivation = 0f;
		}
		if (s_motivation > 100f)
		{
			s_motivation = 100f;
		}
	}

	public void ResetKrank()
	{
		krank = 0;
		objectUsingS_.ConsumeAufladung(1);
		CreatePop(ePop_Arzt, 0f, 15);
	}

	public void ResetMotivation(bool erfuellt, float motivationRegen)
	{
		if (erfuellt)
		{
			if (IsVisible())
			{
				CreatePop(ePop_BeduerfnisErfuellt, mS_.Round(motivationRegen, 1), 15);
			}
			AddMotivation(motivationRegen);
		}
	}

	public void ResetGiessen(bool erfuellt)
	{
		giessen = 100f;
		if (erfuellt)
		{
			if (IsVisible())
			{
				CreatePop(ePop_BeduerfnisErfuellt, 25f, 15);
			}
			AddMotivation(25f);
			if (Random.Range(0, 100) < 10)
			{
				mS_.CreateMuell(9, 9, base.transform.position);
			}
		}
		else
		{
			if (IsVisible())
			{
				CreatePop(ePop_BeduerfnisNichtErfuellt, 5f, 16);
			}
			AddMotivation(-5f);
			Object.Destroy(objectUsingS_.gameObject);
		}
	}

	public void ResetDurst(bool erfuellt)
	{
		durst = 100f;
		if (erfuellt)
		{
			objectUsingS_.ConsumeAufladung(1);
			if (Random.Range(0, 100) < 10)
			{
				mS_.CreateMuell(9, 9, base.transform.position);
			}
			return;
		}
		if (IsVisible())
		{
			CreatePop(ePop_BeduerfnisNichtErfuellt, 5f, 16);
		}
		AddMotivation(-5f);
		Object.Destroy(objectUsingS_.gameObject);
	}

	public void ResetWC(bool erfuellt, float motivationRegen)
	{
		klo = 100f;
		if (erfuellt)
		{
			if (motivationRegen > 0f)
			{
				if (IsVisible())
				{
					CreatePop(ePop_BeduerfnisErfuellt, mS_.Round(motivationRegen, 1), 15);
				}
				AddMotivation(motivationRegen);
			}
		}
		else
		{
			if (IsVisible())
			{
				CreatePop(ePop_BeduerfnisNichtErfuellt, 40f, 16);
			}
			AddMotivation(-40f);
			mS_.CreateMuell(9, 9, base.transform.position);
			Object.Destroy(objectUsingS_.gameObject);
		}
	}

	public void ResetWaschbecken(bool erfuellt, float motivationRegen)
	{
		waschbecken = 100f;
		if (erfuellt)
		{
			if (motivationRegen > 0f)
			{
				if (IsVisible())
				{
					CreatePop(ePop_BeduerfnisErfuellt, mS_.Round(motivationRegen, 1), 15);
				}
				AddMotivation(motivationRegen);
			}
			if (Random.Range(0, 100) < 10)
			{
				mS_.CreateMuell(9, 9, base.transform.position);
			}
		}
		else
		{
			if (IsVisible())
			{
				CreatePop(ePop_BeduerfnisNichtErfuellt, 5f, 16);
			}
			AddMotivation(-5f);
			Object.Destroy(objectUsingS_.gameObject);
		}
	}

	public void ResetFreezer(bool erfuellt)
	{
		freezer = 100f;
		if (erfuellt)
		{
			if (Random.Range(0, 100) < 10)
			{
				mS_.CreateMuell(9, 9, base.transform.position);
			}
			return;
		}
		if (IsVisible())
		{
			CreatePop(ePop_BeduerfnisNichtErfuellt, 5f, 16);
		}
		AddMotivation(-5f);
		Object.Destroy(objectUsingS_.gameObject);
	}

	public void ResetPause(bool erfuellt)
	{
		pause = 100f - (float)(mS_.personal_pausen * 20);
		if (erfuellt && objectUsingS_.isGhost)
		{
			Object.Destroy(objectUsingS_.gameObject);
		}
	}

	public void ResetMuell(bool erfuellt)
	{
		muell = 100f;
		if (erfuellt)
		{
			if (objectUsingS_.aufladungenAkt > 0)
			{
				objectUsingS_.ConsumeAufladung(1);
			}
			else
			{
				CreateMuell();
			}
			return;
		}
		if (IsVisible())
		{
			CreatePop(ePop_BeduerfnisNichtErfuellt, 5f, 16);
		}
		CreateMuell();
		Object.Destroy(objectUsingS_.gameObject);
		AddMotivation(-5f);
	}

	private void CreateMuell()
	{
		switch (Random.Range(0, 5))
		{
		case 0:
			mS_.CreateMuell(0, 0, base.transform.position);
			break;
		case 1:
			mS_.CreateMuell(11, 11, base.transform.position);
			break;
		case 2:
			mS_.CreateMuell(12, 12, base.transform.position);
			break;
		case 3:
			mS_.CreateMuell(13, 13, base.transform.position);
			break;
		case 4:
			mS_.CreateMuell(14, 14, base.transform.position);
			break;
		}
	}

	public void ShowAddObject(int i)
	{
		if ((bool)addObjects[i] && !addObjects[i].activeSelf)
		{
			addObjects[i].SetActive(value: true);
		}
	}

	public void HideAddObjects()
	{
		for (int i = 0; i < addObjects.Length; i++)
		{
			if ((bool)addObjects[i] && addObjects[i].activeSelf)
			{
				addObjects[i].SetActive(value: false);
			}
		}
	}

	public bool IsVisible()
	{
		if (!myRenderer)
		{
			Transform child = base.transform.GetChild(0);
			if ((bool)child)
			{
				myRenderer = child.GetChild(child.childCount - 1).GetComponent<SkinnedMeshRenderer>();
				if ((bool)myRenderer)
				{
					if (myRenderer.gameObject.name.Contains("MyCharacterMesh"))
					{
						base.transform.GetChild(0).transform.localPosition = new Vector3(0f, 0.5f, 0f);
						base.transform.GetChild(0).transform.localEulerAngles = new Vector3(0f, 0f, -180f);
					}
					else
					{
						myRenderer = null;
					}
				}
			}
		}
		if ((bool)myRenderer && !myLodRenderer)
		{
			if (myRenderer.isVisible)
			{
				return true;
			}
			return false;
		}
		if ((bool)myRenderer && (bool)myLodRenderer)
		{
			if (myRenderer.isVisible || myLodRenderer.isVisible)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	private Color GetMotivationColor(float val)
	{
		if (val < 30f)
		{
			return colors[0];
		}
		if (val >= 30f && val < 70f)
		{
			return colors[1];
		}
		if (val >= 70f)
		{
			return colors[2];
		}
		return colors[0];
	}

	private void UpdateIcon()
	{
		if (roomID == -1)
		{
			if (uiIconMain.activeSelf)
			{
				uiIconMain.SetActive(value: false);
			}
			if (!uiNoRoom.activeSelf)
			{
				uiNoRoom.SetActive(value: true);
			}
			return;
		}
		if (!uiIconMain.activeSelf)
		{
			uiIconMain.SetActive(value: true);
		}
		if (uiNoRoom.activeSelf)
		{
			uiNoRoom.SetActive(value: false);
		}
		if (uiIconMain.activeSelf)
		{
			if (iDoWork)
			{
				uiWorkProgress_Image.fillAmount = workProgress;
			}
			uiWorkProgress_Image.color = GetMotivationColor(s_motivation);
		}
		bool flag = false;
		if (krank > 0)
		{
			flag = true;
			if (!uiKrank.activeSelf)
			{
				uiKrank.SetActive(value: true);
			}
		}
		else if (uiKrank.activeSelf)
		{
			uiKrank.SetActive(value: false);
		}
		if ((bool)mapS_)
		{
			if (!settings_.disableArbeiterBeschwerden)
			{
				int num = Mathf.RoundToInt(base.transform.position.x);
				int num2 = Mathf.RoundToInt(base.transform.position.z);
				if (mapS_.IsInMapLimit(num, num2))
				{
					if (!perks[11])
					{
						if (!flag && mapS_.mapWaerme[num, num2] <= 0.2f && mapS_.mapRoomID[num, num2] != 0)
						{
							flag = true;
							if (!uiFrieren.activeSelf)
							{
								uiFrieren.SetActive(value: true);
							}
						}
						else if (uiFrieren.activeSelf)
						{
							uiFrieren.SetActive(value: false);
						}
					}
					if (!perks[16])
					{
						if (!flag && mapS_.mapMuell[num, num2] > 0f)
						{
							flag = true;
							if (!uiGarbage.activeSelf)
							{
								uiGarbage.SetActive(value: true);
							}
						}
						else if (uiGarbage.activeSelf)
						{
							uiGarbage.SetActive(value: false);
						}
					}
					if (!perks[12])
					{
						if (!flag && mapS_.mapAusstattung[num, num2] <= 0.2f && mapS_.mapRoomID[num, num2] != 0)
						{
							flag = true;
							if (!uiLowQuality.activeSelf)
							{
								uiLowQuality.SetActive(value: true);
							}
						}
						else if (uiLowQuality.activeSelf)
						{
							uiLowQuality.SetActive(value: false);
						}
					}
					if (!perks[17])
					{
						if (!flag && IsUeberfuellt())
						{
							flag = true;
							if (!uiUeberfuellt.activeSelf)
							{
								uiUeberfuellt.SetActive(value: true);
							}
						}
						else if (uiUeberfuellt.activeSelf)
						{
							uiUeberfuellt.SetActive(value: false);
						}
					}
				}
			}
			else
			{
				if (uiFrieren.activeSelf)
				{
					uiFrieren.SetActive(value: false);
				}
				if (uiGarbage.activeSelf)
				{
					uiGarbage.SetActive(value: false);
				}
				if (uiLowQuality.activeSelf)
				{
					uiLowQuality.SetActive(value: false);
				}
				if (uiUeberfuellt.activeSelf)
				{
					uiUeberfuellt.SetActive(value: false);
				}
			}
		}
		UpdateLeitenderEntwicklerIcon();
		if ((bool)objectBelegtS_ && !objectBelegtS_.isArbeitsplatz)
		{
			if (objectBelegtS_.canDrink || objectBelegtS_.isGhostDrink)
			{
				SetUiIcon(2);
				return;
			}
			if (objectBelegtS_.isWC || objectBelegtS_.isGhostWC)
			{
				SetUiIcon(7);
				return;
			}
			if (objectBelegtS_.isSink || objectBelegtS_.isGhostSink || objectBelegtS_.isHandtrockner)
			{
				SetUiIcon(8);
				return;
			}
			if (objectBelegtS_.isGhostMuelleimer || objectBelegtS_.isMuelleimer)
			{
				SetUiIcon(3);
				return;
			}
			if (objectBelegtS_.isPlant || objectBelegtS_.isGhostPlant)
			{
				SetUiIcon(5);
				return;
			}
			if (objectBelegtS_.isGhostPause1 || objectBelegtS_.isGhostPause2 || objectBelegtS_.isGhostPause3 || objectBelegtS_.isGhostPause4 || objectBelegtS_.isSeat || objectBelegtS_.isFreezer || objectBelegtS_.isRadio || objectBelegtS_.isTV)
			{
				SetUiIcon(6);
				return;
			}
			if (objectBelegtS_.isArcade || objectBelegtS_.isDart || objectBelegtS_.isMinigolf || objectBelegtS_.isPiano || objectBelegtS_.isSeatAufenthalt || objectBelegtS_.isGhostPause4 || objectBelegtS_.isSeat || objectBelegtS_.isLaufband)
			{
				SetUiIcon(9);
				return;
			}
		}
		if (!mainArbeitsplatzS_ && objectBelegtID == -1)
		{
			SetUiIcon(1);
			return;
		}
		roomScript workRoomScript = GetWorkRoomScript();
		if (!workRoomScript)
		{
			return;
		}
		if (workRoomScript.taskID != -1)
		{
			if (TrainingState(roomS_) == 2)
			{
				SetUiIcon(10);
			}
			else
			{
				SetUiIcon(0);
			}
		}
		else
		{
			SetUiIcon(4);
		}
	}

	public void SetUiIcon(int i)
	{
		if (aktuellesUiIcon == i)
		{
			return;
		}
		aktuellesUiIcon = i;
		uiIcon_Image.sprite = guiMain_.charIcons[i];
		if (i == 0)
		{
			if (uiIcon.activeSelf)
			{
				uiIcon.SetActive(value: false);
			}
		}
		else if (!uiIcon.activeSelf)
		{
			uiIcon.SetActive(value: true);
		}
	}

	public bool IsUeberfuellt()
	{
		if (roomID != -1 && (bool)roomS_ && objectUsingID != -1 && (bool)objectUsingS_ && objectUsingS_.isArbeitsplatz && roomS_.IsUberberfuell())
		{
			return true;
		}
		return false;
	}

	public roomScript GetWorkRoomScript()
	{
		roomScript result = null;
		if ((bool)roomS_)
		{
			result = roomS_;
		}
		if ((bool)roomS_ && (bool)roomS_.taskGameObject)
		{
			taskUnterstuetzen taskUnterstuetzen2 = roomS_.GetTaskUnterstuetzen();
			if ((bool)taskUnterstuetzen2)
			{
				result = taskUnterstuetzen2.rS_;
			}
		}
		return result;
	}

	private void UpdateSprechblase()
	{
		if (!settings_.sprechblasen)
		{
			mS_.anzSprechblasen = 0;
			if (uiSprechblase.activeSelf)
			{
				uiSprechblase.SetActive(value: false);
			}
			return;
		}
		if (uiSprechblase.activeSelf && !uiSprechblase.GetComponent<Animation>().isPlaying)
		{
			uiSprechblase.SetActive(value: false);
			mS_.anzSprechblasen--;
		}
		if (mS_.GetGameSpeed() != 0f && mS_.anzSprechblasen < 5 && Random.Range(0, 2500) == 1 && !uiSprechblase.activeSelf)
		{
			uiSprechblase.SetActive(value: true);
			mS_.anzSprechblasen++;
			if ((bool)mS_.tS_)
			{
				uiSprechblase.transform.GetChild(0).GetComponent<Text>().text = mS_.tS_.GetQuotes();
			}
		}
	}

	public bool IsNoWork_Unterstuetzen()
	{
		if ((bool)roomS_.taskGameObject)
		{
			taskUnterstuetzen taskUnterstuetzen2 = roomS_.GetTaskUnterstuetzen();
			if ((bool)taskUnterstuetzen2)
			{
				if (!taskUnterstuetzen2.rS_)
				{
					return true;
				}
				if (!taskUnterstuetzen2.rS_.taskGameObject)
				{
					return true;
				}
				switch (roomS_.typ)
				{
				case 1:
					if (taskUnterstuetzen2.rS_.IsGameDevComplete())
					{
						return true;
					}
					break;
				case 6:
					if (taskUnterstuetzen2.rS_.WaitForMinimumHype())
					{
						return true;
					}
					break;
				case 7:
					if (taskUnterstuetzen2.rS_.KeineAnrufe())
					{
						return true;
					}
					break;
				case 3:
					if (taskUnterstuetzen2.rS_.QA_GameHasNoBugs())
					{
						return true;
					}
					break;
				case 17:
					if (taskUnterstuetzen2.rS_.KeineAutomatenBestellungen())
					{
						return true;
					}
					break;
				case 2:
					if (taskUnterstuetzen2.rS_.IstForschungWait())
					{
						return true;
					}
					if (taskUnterstuetzen2.rS_.IstAutoForschung())
					{
						return true;
					}
					break;
				}
			}
		}
		return false;
	}

	private void UpdateWork()
	{
		if (mS_.GetDeltaTime() <= 0f)
		{
			return;
		}
		deltaTimeUpdateWork += mS_.GetDeltaTime();
		if (!IsVisible() || !uiVisible)
		{
			timerUpdateWork += Time.deltaTime;
			if (timerUpdateWork < 0.1f)
			{
				return;
			}
			timerUpdateWork = 0f;
		}
		float num = deltaTimeUpdateWork;
		deltaTimeUpdateWork = 0f;
		iDoWork = false;
		if (!roomS_)
		{
			workProgress = 0f;
		}
		else if (roomS_.taskID == -1)
		{
			workProgress = 0f;
		}
		else if (!roomS_.taskGameObject)
		{
			workProgress = 0f;
		}
		else
		{
			if (objectUsingID == -1 || !objectUsingS_ || !objectUsingS_.isArbeitsplatz || moveS_.waitForceAnimation > 0f || roomS_.pause || IsNoWork_Unterstuetzen())
			{
				return;
			}
			int typ = roomS_.typ;
			switch (typ)
			{
			case 6:
				if (roomS_.WaitForMinimumHype())
				{
					return;
				}
				break;
			case 13:
				if (TrainingState(roomS_) == 2)
				{
					return;
				}
				break;
			case 7:
				if (roomS_.KeineAnrufe())
				{
					return;
				}
				break;
			case 3:
				if (roomS_.QA_GameHasNoBugs())
				{
					return;
				}
				break;
			case 17:
				if (roomS_.KeineAutomatenBestellungen())
				{
					return;
				}
				break;
			case 8:
				if (roomS_.IsKonsoleDevCompleteOrg())
				{
					return;
				}
				break;
			}
			if (roomS_.IstContractWorkWait() || roomS_.IstForschungWait() || roomS_.IstAutoForschung() || roomS_.IstTaskWait())
			{
				return;
			}
			iDoWork = true;
			int num2 = 0;
			if (typ == 1 || typ == 3 || typ == 5 || typ == 4 || typ == 10)
			{
				float num3 = 1f;
				if (roomS_.GameIsPort())
				{
					num3 = 2f;
				}
				if (roomS_.GameIsMMO())
				{
					num3 = 0.5f;
				}
				if (typ == 1)
				{
					gameScript gameScript2 = roomS_.DEV_GetGame();
					if ((bool)gameScript2 && (bool)gameScript2)
					{
						if (gameScript2.devAktFeature != -5 && gameScript2.devAktFeature < 0 && (bool)mS_.gF_)
						{
							num2 = mS_.eF_.engineFeatures_LEVEL[gameScript2.devAktFeature + 4];
							switch (num2)
							{
							case 0:
								num3 *= 0.5f;
								break;
							case 1:
								num3 *= 0.6f;
								break;
							case 2:
								num3 *= 0.7f;
								break;
							case 3:
								num3 *= 0.8f;
								break;
							case 4:
								num3 *= 0.9f;
								break;
							}
						}
						if (gameScript2.devAktFeature >= 0 && (bool)mS_.gF_)
						{
							num2 = mS_.gF_.gameplayFeatures_LEVEL[gameScript2.devAktFeature];
							switch (num2)
							{
							case 0:
								num3 *= 0.5f;
								break;
							case 1:
								num3 *= 0.6f;
								break;
							case 2:
								num3 *= 0.7f;
								break;
							case 3:
								num3 *= 0.8f;
								break;
							case 4:
								num3 *= 0.9f;
								break;
							}
						}
					}
				}
				switch (roomS_.GetGameSize())
				{
				case 1:
					num3 = ((mS_.devTimeSetting != 1) ? (num3 * 0.95f) : (num3 * 0.8f));
					break;
				case 2:
					num3 = ((mS_.devTimeSetting != 1) ? (num3 * 0.9f) : (num3 * 0.6f));
					break;
				case 3:
					num3 = ((mS_.devTimeSetting != 1) ? (num3 * 0.85f) : (num3 * 0.4f));
					break;
				case 4:
					num3 = ((mS_.devTimeSetting != 1) ? (num3 * 0.8f) : (num3 * 0.3f));
					break;
				case 5:
					num3 = ((mS_.devTimeSetting != 1) ? (num3 * 0.75f) : (num3 * 0.2f));
					break;
				}
				if (!mS_.settings_sabotageOff && mS_.sabotage_work > 0)
				{
					num3 *= 0.5f;
				}
				if (mS_.devTimeSetting == 2)
				{
					num3 *= 2f;
				}
				workProgress += num * GetWorkSpeed() * num3;
			}
			else
			{
				workProgress += num * GetWorkSpeed();
			}
			if (!(workProgress >= 1f))
			{
				return;
			}
			workProgress = 0f;
			if ((bool)roomS_ && mS_.personal_crunch < 85 && roomS_.IsCrunchtimeRead() && Random.Range(0, 500 + mS_.personal_crunch * 10) == 1 && s_motivation < 80f)
			{
				UpdateKuendigen(force: true);
				return;
			}
			switch (roomS_.typ)
			{
			case 1:
				if (WORK_Engine(roomS_) || WORK_Game(roomS_, num2) || WORK_Update(roomS_) || WORK_F2PUpdate(roomS_))
				{
					return;
				}
				break;
			case 6:
				if (WORK_Marketing(roomS_) || WORK_MarketingSpezial(roomS_) || WORK_Marktforschung(roomS_) || WORK_Mitarbeitersuche(roomS_))
				{
					return;
				}
				break;
			case 7:
				if (WORK_Fankampagne(roomS_) || WORK_Support(roomS_) || WORK_Fanshop(roomS_))
				{
					return;
				}
				break;
			case 3:
				if (WORK_Bugfixing(roomS_) || WORK_GameplayVerbessern(roomS_) || WORK_Spielbericht(roomS_))
				{
					return;
				}
				break;
			case 2:
				if (WORK_Forschung(roomS_))
				{
					return;
				}
				break;
			case 13:
				if (WORK_Training(roomS_))
				{
					return;
				}
				break;
			case 4:
				if (WORK_GrafikVerbessern(roomS_))
				{
					return;
				}
				break;
			case 5:
				if (WORK_SoundVerbessern(roomS_))
				{
					return;
				}
				break;
			case 10:
				if (WORK_AnimationVerbessern(roomS_))
				{
					return;
				}
				break;
			case 17:
				if (WORK_ArcadeProduction(roomS_))
				{
					return;
				}
				break;
			case 8:
				if (WORK_Hardware(roomS_) || WORK_KonsoleReduceCosts(roomS_) || WORK_KonsoleHaltbarkeit(roomS_))
				{
					return;
				}
				break;
			}
			if (!WORK_Polishing(roomS_) && !WORK_ContractWork(roomS_))
			{
				WORK_Untersteutzen(num2);
			}
		}
	}

	private float GetWorkSpeed()
	{
		float num = 0.01f * ((s_motivation + 10f) * 0.5f);
		switch (mS_.personal_druck)
		{
		case 1:
			num *= 1.25f;
			break;
		case 2:
			num *= 1.5f;
			break;
		}
		if (perks[29])
		{
			num *= 1.1f;
		}
		float num2 = mS_.GetAchivementBonus(9);
		num2 *= 0.01f;
		num += num * num2;
		if (krank > 0)
		{
			num *= 0.25f;
		}
		if (mS_.settings_sandbox && mS_.sandbox_mitarbeiterSpeed > 0f)
		{
			num *= mS_.sandbox_mitarbeiterSpeed;
		}
		if (mS_.settings_arbeitsgeschwindigkeitAnpassen)
		{
			return num * (mS_.speedSetting * 20f);
		}
		return num;
	}

	private float GetWorkResult(float f)
	{
		switch (mS_.personal_druck)
		{
		case 1:
			f *= 0.8f;
			break;
		case 2:
			f *= 0.6f;
			break;
		}
		return mS_.Round(f, 1);
	}

	private void WORK_Untersteutzen(int level_aktuellesFeature_)
	{
		taskUnterstuetzen taskUnterstuetzen2 = roomS_.GetTaskUnterstuetzen();
		if ((bool)taskUnterstuetzen2 && (bool)taskUnterstuetzen2.rS_ && (bool)taskUnterstuetzen2.rS_.taskGameObject && !WORK_Forschung(taskUnterstuetzen2.rS_) && !WORK_Engine(taskUnterstuetzen2.rS_) && !WORK_Game(taskUnterstuetzen2.rS_, level_aktuellesFeature_) && !WORK_Marketing(taskUnterstuetzen2.rS_) && !WORK_MarketingSpezial(taskUnterstuetzen2.rS_) && !WORK_Fankampagne(taskUnterstuetzen2.rS_) && !WORK_Mitarbeitersuche(taskUnterstuetzen2.rS_) && !WORK_Support(taskUnterstuetzen2.rS_) && !WORK_Fanshop(taskUnterstuetzen2.rS_) && !WORK_ContractWork(taskUnterstuetzen2.rS_) && !WORK_Update(taskUnterstuetzen2.rS_) && !WORK_F2PUpdate(taskUnterstuetzen2.rS_) && !WORK_Bugfixing(taskUnterstuetzen2.rS_) && !WORK_GameplayVerbessern(taskUnterstuetzen2.rS_) && !WORK_GrafikVerbessern(taskUnterstuetzen2.rS_) && !WORK_SoundVerbessern(taskUnterstuetzen2.rS_) && !WORK_AnimationVerbessern(taskUnterstuetzen2.rS_) && !WORK_Spielbericht(taskUnterstuetzen2.rS_) && !WORK_Marktforschung(taskUnterstuetzen2.rS_) && !WORK_Polishing(taskUnterstuetzen2.rS_) && !WORK_ArcadeProduction(taskUnterstuetzen2.rS_) && !WORK_Hardware(taskUnterstuetzen2.rS_) && !WORK_KonsoleReduceCosts(taskUnterstuetzen2.rS_))
		{
			WORK_KonsoleHaltbarkeit(taskUnterstuetzen2.rS_);
		}
	}

	public int TrainingState(roomScript rS_)
	{
		if (!rS_)
		{
			return 0;
		}
		if (rS_.typ != 13)
		{
			return 0;
		}
		if (!rS_.taskGameObject)
		{
			return 0;
		}
		if (!guiMain_)
		{
			return 0;
		}
		taskTraining taskTraining2 = rS_.GetTaskTraining();
		if (!taskTraining2)
		{
			return 0;
		}
		if (!menuTrain_)
		{
			menuTrain_ = guiMain_.uiObjects[92].GetComponent<Menu_Training_Select>();
		}
		int result = 2;
		switch (taskTraining2.slot)
		{
		case 0:
			if (s_gamedesign < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(0) > s_gamedesign)
			{
				result = 1;
			}
			break;
		case 1:
			if (s_gamedesign < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(0) > s_gamedesign)
			{
				result = 1;
			}
			break;
		case 2:
			if (s_gamedesign < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(0) > s_gamedesign)
			{
				result = 1;
			}
			break;
		case 3:
			if (s_programmieren < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(1) > s_programmieren)
			{
				result = 1;
			}
			break;
		case 4:
			if (s_programmieren < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(1) > s_programmieren)
			{
				result = 1;
			}
			break;
		case 5:
			if (s_programmieren < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(1) > s_programmieren)
			{
				result = 1;
			}
			break;
		case 6:
			if (s_grafik < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(2) > s_grafik)
			{
				result = 1;
			}
			break;
		case 7:
			if (s_grafik < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(2) > s_grafik)
			{
				result = 1;
			}
			break;
		case 8:
			if (s_grafik < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(2) > s_grafik)
			{
				result = 1;
			}
			break;
		case 9:
			if (s_sound < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(3) > s_sound)
			{
				result = 1;
			}
			break;
		case 10:
			if (s_sound < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(3) > s_sound)
			{
				result = 1;
			}
			break;
		case 11:
			if (s_sound < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(3) > s_sound)
			{
				result = 1;
			}
			break;
		case 12:
			if (s_pr < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(4) > s_pr)
			{
				result = 1;
			}
			break;
		case 13:
			if (s_pr < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(4) > s_pr)
			{
				result = 1;
			}
			break;
		case 14:
			if (s_pr < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(4) > s_pr)
			{
				result = 1;
			}
			break;
		case 15:
			if (s_gametests < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(5) > s_gametests)
			{
				result = 1;
			}
			break;
		case 16:
			if (s_gametests < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(5) > s_gametests)
			{
				result = 1;
			}
			break;
		case 17:
			if (s_gametests < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(5) > s_gametests)
			{
				result = 1;
			}
			break;
		case 18:
			if (s_technik < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(6) > s_technik)
			{
				result = 1;
			}
			break;
		case 19:
			if (s_technik < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(6) > s_technik)
			{
				result = 1;
			}
			break;
		case 20:
			if (s_technik < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(6) > s_technik)
			{
				result = 1;
			}
			break;
		case 21:
			if (s_forschen < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(7) > s_forschen)
			{
				result = 1;
			}
			break;
		case 22:
			if (s_forschen < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(7) > s_forschen)
			{
				result = 1;
			}
			break;
		case 23:
			if (s_forschen < menuTrain_.trainingMaxLearn[taskTraining2.slot] && GetSkillCap_Skill(7) > s_forschen)
			{
				result = 1;
			}
			break;
		}
		return result;
	}

	private bool WORK_Training(roomScript rS_)
	{
		if (!guiMain_)
		{
			return false;
		}
		if (!rS_)
		{
			return false;
		}
		taskTraining taskTraining2 = rS_.GetTaskTraining();
		if (!taskTraining2)
		{
			return false;
		}
		if (!menuTrain_)
		{
			menuTrain_ = guiMain_.uiObjects[92].GetComponent<Menu_Training_Select>();
		}
		int num = objectUsingS_.qualitaet - 1;
		bool flag = false;
		int num2 = 10 + num * 2 + menuTrain_.trainingEffekt[taskTraining2.slot] * 10;
		for (int i = 0; i < num2; i++)
		{
			switch (taskTraining2.slot)
			{
			case 0:
				if (s_gamedesign < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: true, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 1:
				if (s_gamedesign < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: true, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 2:
				if (s_gamedesign < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: true, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 3:
				if (s_programmieren < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: true, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 4:
				if (s_programmieren < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: true, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 5:
				if (s_programmieren < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: true, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 6:
				if (s_grafik < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: true, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 7:
				if (s_grafik < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: true, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 8:
				if (s_grafik < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: true, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 9:
				if (s_sound < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: true, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 10:
				if (s_sound < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: true, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 11:
				if (s_sound < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: true, pr_: false, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 12:
				if (s_pr < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 13:
				if (s_pr < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 14:
				if (s_pr < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 15:
				if (s_gametests < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: true, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 16:
				if (s_gametests < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: true, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 17:
				if (s_gametests < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: true, technik_: false, forschen_: false);
					flag = true;
				}
				break;
			case 18:
				if (s_technik < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: true, forschen_: false);
					flag = true;
				}
				break;
			case 19:
				if (s_technik < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: true, forschen_: false);
					flag = true;
				}
				break;
			case 20:
				if (s_technik < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: true, forschen_: false);
					flag = true;
				}
				break;
			case 21:
				if (s_forschen < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: true);
					flag = true;
				}
				break;
			case 22:
				if (s_forschen < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: true);
					flag = true;
				}
				break;
			case 23:
				if (s_forschen < menuTrain_.trainingMaxLearn[taskTraining2.slot])
				{
					Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: true);
					flag = true;
				}
				break;
			}
		}
		if (flag)
		{
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Training, 1f, 0f, 13));
			}
			taskTraining2.Work(1f);
		}
		return true;
	}

	private bool WORK_ContractWork(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskContractWork taskContractWork2 = rS_.GetTaskContractWork();
		if (!taskContractWork2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		if (rS_.typ == 1)
		{
			num2 = Random.Range(0.1f, s_programmieren * 0.1f) * num3;
			num2 = GetWorkResult(num2);
			if (perks[28])
			{
				num2 *= 2f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
			}
			num += 0.4f;
			taskContractWork2.Work(num2);
			Learn(gamedesign_: false, programmieren_: true, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
			if (critic)
			{
				num2 = Random.Range(0.1f, s_programmieren * 0.1f);
				num2 = GetWorkResult(num2);
				if (!settings_.disableWorkIcons && uiVisible)
				{
					StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
				}
				num += 0.4f;
				taskContractWork2.Work(num2);
			}
			return true;
		}
		if (rS_.typ == 3)
		{
			num2 = Random.Range(0.1f, s_gamedesign * 0.1f) * num3;
			num2 = GetWorkResult(num2);
			if (perks[28])
			{
				num2 *= 2f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Gameplay, num2, num, 13));
			}
			num += 0.4f;
			taskContractWork2.Work(num2);
			Learn(gamedesign_: true, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
			if (critic)
			{
				num2 = Random.Range(0.1f, s_gamedesign * 0.1f);
				num2 = GetWorkResult(num2);
				if (!settings_.disableWorkIcons && uiVisible)
				{
					StartCoroutine(CreatePopInSeconds(ePop_Gameplay, num2, num, 13));
				}
				num += 0.4f;
				taskContractWork2.Work(num2);
			}
			return true;
		}
		if (rS_.typ == 4)
		{
			num2 = Random.Range(0.1f, s_grafik * 0.1f) * num3;
			num2 = GetWorkResult(num2);
			if (perks[28])
			{
				num2 *= 2f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Grafik, num2, num, 13));
			}
			num += 0.4f;
			taskContractWork2.Work(num2);
			Learn(gamedesign_: false, programmieren_: false, grafik_: true, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
			if (critic)
			{
				num2 = Random.Range(0.1f, s_grafik * 0.1f);
				num2 = GetWorkResult(num2);
				if (!settings_.disableWorkIcons && uiVisible)
				{
					StartCoroutine(CreatePopInSeconds(ePop_Grafik, num2, num, 13));
				}
				num += 0.4f;
				taskContractWork2.Work(num2);
			}
			return true;
		}
		if (rS_.typ == 5)
		{
			num2 = Random.Range(0.1f, s_sound * 0.1f) * num3;
			num2 = GetWorkResult(num2);
			if (perks[28])
			{
				num2 *= 2f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Sound, num2, num, 13));
			}
			num += 0.4f;
			taskContractWork2.Work(num2);
			Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: true, pr_: false, gametests_: false, technik_: false, forschen_: false);
			if (critic)
			{
				num2 = Random.Range(0.1f, s_sound * 0.1f);
				num2 = GetWorkResult(num2);
				if (!settings_.disableWorkIcons && uiVisible)
				{
					StartCoroutine(CreatePopInSeconds(ePop_Sound, num2, num, 13));
				}
				num += 0.4f;
				taskContractWork2.Work(num2);
			}
			return true;
		}
		if (rS_.typ == 10)
		{
			num2 = Random.Range(0.1f, s_programmieren * 0.1f) * num3;
			num2 = GetWorkResult(num2);
			if (perks[28])
			{
				num2 *= 2f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
			}
			num += 0.4f;
			taskContractWork2.Work(num2);
			Learn(gamedesign_: false, programmieren_: true, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
			if (critic)
			{
				num2 = Random.Range(0.1f, s_programmieren * 0.1f);
				num2 = GetWorkResult(num2);
				if (!settings_.disableWorkIcons && uiVisible)
				{
					StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
				}
				num += 0.4f;
				taskContractWork2.Work(num2);
			}
			return true;
		}
		if (rS_.typ == 17)
		{
			num2 = Random.Range(0.1f, s_technik * 0.1f) * num3;
			num2 = GetWorkResult(num2);
			if (perks[28])
			{
				num2 *= 2f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_ProdArcade, num2, num, 13));
			}
			num += 0.4f;
			taskContractWork2.Work(num2);
			Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: true, forschen_: false);
			if (critic)
			{
				num2 = Random.Range(0.1f, s_technik * 0.1f);
				num2 = GetWorkResult(num2);
				if (!settings_.disableWorkIcons && uiVisible)
				{
					StartCoroutine(CreatePopInSeconds(ePop_ProdArcade, num2, num, 13));
				}
				num += 0.4f;
				taskContractWork2.Work(num2);
			}
			return true;
		}
		if (rS_.typ == 8)
		{
			num2 = Random.Range(0.1f, s_technik * 0.1f) * num3;
			num2 = GetWorkResult(num2);
			if (perks[28])
			{
				num2 *= 2f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Hardware, num2, num, 13));
			}
			num += 0.4f;
			taskContractWork2.Work(num2);
			Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: true, forschen_: false);
			if (critic)
			{
				num2 = Random.Range(0.1f, s_technik * 0.1f);
				num2 = GetWorkResult(num2);
				if (!settings_.disableWorkIcons && uiVisible)
				{
					StartCoroutine(CreatePopInSeconds(ePop_Hardware, num2, num, 13));
				}
				num += 0.4f;
				taskContractWork2.Work(num2);
			}
			return true;
		}
		return true;
	}

	private bool WORK_Fankampagne(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskFankampagne taskFankampagne2 = rS_.GetTaskFankampagne();
		if (!taskFankampagne2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_pr * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Support, num2, num, 13));
		}
		num += 0.4f;
		taskFankampagne2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_pr * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Support, num2, num, 13));
			}
			num += 0.4f;
			taskFankampagne2.Work(num2);
		}
		return true;
	}

	private bool WORK_Mitarbeitersuche(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskMitarbeitersuche taskMitarbeitersuche2 = rS_.GetTaskMitarbeitersuche();
		if (!taskMitarbeitersuche2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_pr * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Marketing, num2, num, 13));
		}
		num += 0.4f;
		taskMitarbeitersuche2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_pr * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Marketing, num2, num, 13));
			}
			num += 0.4f;
			taskMitarbeitersuche2.Work(num2);
		}
		return true;
	}

	private bool WORK_Marktforschung(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskMarktforschung taskMarktforschung2 = rS_.GetTaskMarktforschung();
		if (!taskMarktforschung2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_pr * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Support, num2, num, 13));
		}
		num += 0.4f;
		taskMarktforschung2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_pr * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Support, num2, num, 13));
			}
			num += 0.4f;
			taskMarktforschung2.Work(num2);
		}
		return true;
	}

	private bool WORK_Support(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskSupport taskSupport2 = rS_.GetTaskSupport();
		if (!taskSupport2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_pr * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Support, num2, num, 13));
		}
		num += 0.4f;
		taskSupport2.Work(num2, base.transform.position);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_pr * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Support, num2, num, 13));
			}
			num += 0.4f;
			taskSupport2.Work(num2, base.transform.position);
		}
		return true;
	}

	private bool WORK_Fanshop(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskFanshop taskFanshop2 = rS_.GetTaskFanshop();
		if (!taskFanshop2)
		{
			return false;
		}
		float num = 0f;
		float num2 = objectUsingS_.qualitaet - 1;
		num2 = num2 / 10f + 1f;
		num = Random.Range(0.1f, s_pr * 0.1f) * num2;
		num = GetWorkResult(num);
		bool flag = true;
		if ((bool)taskFanshop2.menuFanshop_)
		{
			for (int i = Random.Range(0, mS_.games_.arrayMyIpScripts.Count); i < mS_.games_.arrayMyIpScripts.Count; i++)
			{
				gameScript gameScript2 = mS_.games_.arrayMyIpScripts[i];
				if (!gameScript2 || gameScript2.merchKeinVerkauf)
				{
					continue;
				}
				for (int j = 0; j < gameScript2.merchBestellungen.Length; j++)
				{
					if (gameScript2.merchBestellungen[j] > 0)
					{
						flag = false;
						if (gameScript2.merchVerkaufspreis[j] <= 0f)
						{
							gameScript2.merchVerkaufspreis[j] = taskFanshop2.menuFanshop_.GetMindestVerkaufspreis(j);
						}
						int num3 = 1 + Mathf.RoundToInt(num) * 30;
						if (gameScript2.merchBestellungen[j] < num3)
						{
							num3 = gameScript2.merchBestellungen[j];
						}
						gameScript2.merchBestellungen[j] -= num3;
						gameScript2.merchDiesenMonat[j] += num3;
						gameScript2.merchGesamtSells[j] += num3;
						float f = (float)num3 * (gameScript2.merchVerkaufspreis[j] - taskFanshop2.menuFanshop_.einkaufspreis[j]);
						int num4 = Mathf.RoundToInt(f);
						gameScript2.merchGesamtGewinn += Mathf.RoundToInt(f);
						gameScript2.merchGewinnDiesenMonat += Mathf.RoundToInt(f);
						mS_.Earn(num4, 11);
						mS_.AddFanshopverlauf(num4);
						if (!settings_.disableWorkIcons && uiVisible)
						{
							StartCoroutine(CreatePopInSeconds_SPRITE(guiMain_.uiSprites[50 + j], mS_.GetMoney(num4, showDollar: true), 0f, 13));
						}
						taskFanshop2.Work(j, num3, num4);
						Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
						break;
					}
				}
			}
		}
		if (flag)
		{
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds_SPRITE(guiMain_.uiSprites[58], "", 0f, 13));
			}
			Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
		}
		return true;
	}

	private bool WORK_Marketing(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskMarketing taskMarketing2 = rS_.GetTaskMarketing();
		if (!taskMarketing2)
		{
			return false;
		}
		if (taskMarketing2.WaitForMinimumHype())
		{
			return true;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_pr * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Marketing, num2, num, 13));
		}
		num += 0.4f;
		taskMarketing2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_pr * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Marketing, num2, num, 13));
			}
			num += 0.4f;
			taskMarketing2.Work(num2);
		}
		return true;
	}

	private bool WORK_MarketingSpezial(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskMarketingSpezial taskMarketingSpezial2 = rS_.GetTaskMarketingSpezial();
		if (!taskMarketingSpezial2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_pr * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Marketing, num2, num, 13));
		}
		num += 0.4f;
		taskMarketingSpezial2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: true, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_pr * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Marketing, num2, num, 13));
			}
			num += 0.4f;
			taskMarketingSpezial2.Work(num2);
		}
		return true;
	}

	private bool WORK_Hardware(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskKonsole taskKonsole2 = rS_.GetTaskKonsole();
		if (!taskKonsole2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_technik * 0.1f) * num3;
		if ((bool)taskKonsole2.techniker_)
		{
			num2 = ((!taskKonsole2.techniker_.perks[14]) ? (num2 + taskKonsole2.techniker_.s_technik * 0.01f) : (num2 + taskKonsole2.techniker_.s_technik * 0.02f));
		}
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Hardware, num2, num, 13));
		}
		num += 0.4f;
		taskKonsole2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: true, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_technik * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Hardware, num2, num, 13));
			}
			num += 0.4f;
			taskKonsole2.Work(num2);
		}
		return true;
	}

	private bool WORK_KonsoleReduceCosts(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskKonsoleReduceCosts taskKonsoleReduceCosts2 = rS_.GetTaskKonsoleReduceCosts();
		if (!taskKonsoleReduceCosts2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_technik * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Hardware, num2, num, 13));
		}
		num += 0.4f;
		taskKonsoleReduceCosts2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: true, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_technik * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Hardware, num2, num, 13));
			}
			num += 0.4f;
			taskKonsoleReduceCosts2.Work(num2);
		}
		return true;
	}

	private bool WORK_KonsoleHaltbarkeit(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskKonsoleHaltbarkeit taskKonsoleHaltbarkeit2 = rS_.GetTaskKonsoleHaltbarkeit();
		if (!taskKonsoleHaltbarkeit2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_technik * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Hardware, num2, num, 13));
		}
		num += 0.4f;
		taskKonsoleHaltbarkeit2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: true, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_technik * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Hardware, num2, num, 13));
			}
			num += 0.4f;
			taskKonsoleHaltbarkeit2.Work(num2);
		}
		return true;
	}

	private bool WORK_ArcadeProduction(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskArcadeProduction taskArcadeProduction2 = rS_.GetTaskArcadeProduction();
		if (!taskArcadeProduction2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		if (uiVisible)
		{
			sfx_.PlaySound(59, force: false);
		}
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(s_technik * 0.05f, s_technik * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_ProdArcade, num2, num, 13));
		}
		num += 0.4f;
		taskArcadeProduction2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: true, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_technik * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_ProdArcade, num2, num, 13));
			}
			num += 0.4f;
			taskArcadeProduction2.Work(num2);
		}
		return true;
	}

	private bool WORK_Update(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskUpdate taskUpdate2 = rS_.GetTaskUpdate();
		if (!taskUpdate2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_programmieren * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
		}
		num += 0.4f;
		taskUpdate2.Work(num2);
		Learn(gamedesign_: true, programmieren_: true, grafik_: true, sound_: true, pr_: false, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_programmieren * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
			}
			num += 0.4f;
			taskUpdate2.Work(num2);
		}
		return true;
	}

	private bool WORK_F2PUpdate(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskF2PUpdate taskF2PUpdate2 = rS_.GetTaskF2PUpdate();
		if (!taskF2PUpdate2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_programmieren * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
		}
		num += 0.4f;
		taskF2PUpdate2.Work(num2);
		Learn(gamedesign_: true, programmieren_: true, grafik_: true, sound_: true, pr_: false, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_programmieren * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
			}
			num += 0.4f;
			taskF2PUpdate2.Work(num2);
		}
		return true;
	}

	private bool WORK_Bugfixing(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskBugfixing taskBugfixing2 = rS_.GetTaskBugfixing();
		if (!taskBugfixing2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_gametests * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_QA, num2, num, 13));
		}
		num += 0.4f;
		taskBugfixing2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: true, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_gametests * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_QA, num2, num, 13));
			}
			num += 0.4f;
			taskBugfixing2.Work(num2);
		}
		return true;
	}

	private bool WORK_Polishing(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskPolishing taskPolishing2 = rS_.GetTaskPolishing();
		if (!taskPolishing2)
		{
			return false;
		}
		float f = 0f;
		float num = objectUsingS_.qualitaet - 1;
		num = num / 10f + 1f;
		switch (rS_.typ)
		{
		case 4:
			f = Random.Range(0.1f, s_grafik * 0.1f) * num;
			f = GetWorkResult(f);
			f *= 0.1f;
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Grafik, f, 0f, 13));
			}
			Learn(gamedesign_: false, programmieren_: false, grafik_: true, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
			break;
		case 5:
			f = Random.Range(0.1f, s_sound * 0.1f) * num;
			f = GetWorkResult(f);
			f *= 0.1f;
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Sound, f, 0f, 13));
			}
			Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: true, pr_: false, gametests_: false, technik_: false, forschen_: false);
			break;
		case 10:
			f = Random.Range(0.1f, s_programmieren * 0.1f) * num;
			f = GetWorkResult(f);
			f *= 0.1f;
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Technik, f, 0f, 13));
			}
			Learn(gamedesign_: false, programmieren_: true, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
			break;
		case 3:
			f = Random.Range(0.1f, s_gametests * 0.1f) * num;
			f = GetWorkResult(f);
			f *= 0.1f;
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Gameplay, f, 0f, 13));
			}
			Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: true, technik_: false, forschen_: false);
			break;
		}
		taskPolishing2.Work(f, roomS_, hype: false);
		if (perks[1] && Random.Range(0, 30) == 1)
		{
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Hype, 1f, 0.4f, 13));
			}
			taskPolishing2.Work(1f, roomS_, hype: true);
		}
		return true;
	}

	private bool WORK_Spielbericht(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskSpielbericht taskSpielbericht2 = rS_.GetTaskSpielbericht();
		if (!taskSpielbericht2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_gametests * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_QA, num2, num, 13));
		}
		num += 0.4f;
		taskSpielbericht2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: true, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_gametests * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_QA, num2, num, 13));
			}
			num += 0.4f;
			taskSpielbericht2.Work(num2);
		}
		return true;
	}

	private bool WORK_GameplayVerbessern(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskGameplayVerbessern taskGameplayVerbessern2 = rS_.GetTaskGameplayVerbessern();
		if (!taskGameplayVerbessern2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_gametests * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (perks[25] && taskGameplayVerbessern2.gS_.typ_nachfolger)
		{
			num2 *= 2f;
		}
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Gameplay, num2, num, 13));
		}
		num += 0.4f;
		taskGameplayVerbessern2.Work(num2, 0);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: true, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_gametests * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Gameplay, num2, num, 13));
			}
			num += 0.4f;
			taskGameplayVerbessern2.Work(num2, 0);
		}
		if (perks[1] && Random.Range(0, 30) == 1)
		{
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Hype, 1f, num, 13));
			}
			taskGameplayVerbessern2.Work(1f, 5);
		}
		return true;
	}

	private bool WORK_GrafikVerbessern(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskGrafikVerbessern taskGrafikVerbessern2 = rS_.GetTaskGrafikVerbessern();
		if (!taskGrafikVerbessern2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_grafik * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (perks[23] && taskGrafikVerbessern2.gS_.retro)
		{
			num2 *= 2f;
		}
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Grafik, num2, num, 13));
		}
		num += 0.4f;
		taskGrafikVerbessern2.Work(num2, 1);
		Learn(gamedesign_: false, programmieren_: false, grafik_: true, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_grafik * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Grafik, num2, num, 13));
			}
			num += 0.4f;
			taskGrafikVerbessern2.Work(num2, 1);
		}
		if (perks[1] && Random.Range(0, 30) == 1)
		{
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Hype, 1f, num, 13));
			}
			taskGrafikVerbessern2.Work(1f, 5);
		}
		return true;
	}

	private bool WORK_SoundVerbessern(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskSoundVerbessern taskSoundVerbessern2 = rS_.GetTaskSoundVerbessern();
		if (!taskSoundVerbessern2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_sound * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Sound, num2, num, 13));
		}
		num += 0.4f;
		taskSoundVerbessern2.Work(num2, 2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: true, pr_: false, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_sound * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Sound, num2, num, 13));
			}
			num += 0.4f;
			taskSoundVerbessern2.Work(num2, 2);
		}
		if (perks[1] && Random.Range(0, 30) == 1)
		{
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Hype, 1f, num, 13));
			}
			taskSoundVerbessern2.Work(1f, 5);
		}
		return true;
	}

	private bool WORK_AnimationVerbessern(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskAnimationVerbessern taskAnimationVerbessern2 = rS_.GetTaskAnimationVerbessern();
		if (!taskAnimationVerbessern2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_programmieren * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
		}
		num += 0.4f;
		taskAnimationVerbessern2.Work(num2, 3);
		Learn(gamedesign_: false, programmieren_: true, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_programmieren * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
			}
			num += 0.4f;
			taskAnimationVerbessern2.Work(num2, 3);
		}
		if (perks[1] && Random.Range(0, 30) == 1)
		{
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Hype, 1f, num, 13));
			}
			taskAnimationVerbessern2.Work(1f, 5);
		}
		return true;
	}

	private bool WORK_Forschung(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskForschung taskForschung2 = rS_.GetTaskForschung();
		if (!taskForschung2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_forschen * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Forschung, num2, num, 13));
		}
		num += 0.4f;
		taskForschung2.Work(num2);
		Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: true);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_forschen * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Forschung, num2, num, 13));
			}
			num += 0.4f;
			taskForschung2.Work(num2);
		}
		return true;
	}

	private bool WORK_Engine(roomScript rS_)
	{
		if (!rS_)
		{
			return false;
		}
		taskEngine taskEngine2 = rS_.GetTaskEngine();
		if (!taskEngine2)
		{
			return false;
		}
		float num = 0f;
		float num2 = 0f;
		bool critic = GetCritic(0);
		float num3 = objectUsingS_.qualitaet - 1;
		num3 = num3 / 10f + 1f;
		num2 = Random.Range(0.1f, s_programmieren * 0.1f) * num3;
		num2 = GetWorkResult(num2);
		if (perks[26])
		{
			num2 *= 2f;
		}
		if (!settings_.disableWorkIcons && uiVisible)
		{
			StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
		}
		num += 0.4f;
		taskEngine2.Work(num2);
		Learn(gamedesign_: false, programmieren_: true, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
		if (critic)
		{
			num2 = Random.Range(0.1f, s_programmieren * 0.1f);
			num2 = GetWorkResult(num2);
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
			}
			num += 0.4f;
			taskEngine2.Work(num2);
		}
		return true;
	}

	private bool WORK_Game(roomScript rS_, int level_aktuellesFeature_)
	{
		if (!rS_)
		{
			return false;
		}
		taskGame taskGame2 = rS_.GetTaskGame();
		if (!taskGame2)
		{
			return false;
		}
		if (!taskGame2.gS_)
		{
			return true;
		}
		_ = taskGame2.gS_.devPoints_Gesamt;
		_ = 0f;
		float num = 0f;
		float num2 = 0f;
		int num3 = 0;
		engineScript engineScript2 = taskGame2.gS_.GetEngineScript();
		if ((bool)engineScript2)
		{
			if (taskGame2.gS_.maingenre == engineScript2.spezialgenre)
			{
				num3++;
			}
			if (taskGame2.gS_.gamePlatform[0] == engineScript2.spezialplatform)
			{
				num3++;
			}
			if (taskGame2.gS_.gamePlatform[1] == engineScript2.spezialplatform)
			{
				num3++;
			}
			if (taskGame2.gS_.gamePlatform[2] == engineScript2.spezialplatform)
			{
				num3++;
			}
			if (taskGame2.gS_.gamePlatform[3] == engineScript2.spezialplatform)
			{
				num3++;
			}
		}
		bool critic = GetCritic(num3);
		int num4 = Random.Range(0, 4);
		if (Random.Range(0, taskGame2.gS_.gameAP_Gameplay * 5) > Random.Range(0, 100))
		{
			num4 = 0;
		}
		if (Random.Range(0, taskGame2.gS_.gameAP_Grafik * 5) > Random.Range(0, 100))
		{
			num4 = 1;
		}
		if (Random.Range(0, taskGame2.gS_.gameAP_Sound * 5) > Random.Range(0, 100))
		{
			num4 = 2;
		}
		if (Random.Range(0, taskGame2.gS_.gameAP_Technik * 5) > Random.Range(0, 100))
		{
			num4 = 3;
		}
		int num5 = -1;
		if (critic)
		{
			num5 = Random.Range(0, 4);
			if (num4 == num5)
			{
				num5 = ((num5 <= 0) ? (num5 + 1) : (num5 - 1));
			}
		}
		float num6 = objectUsingS_.qualitaet - 1;
		num6 = num6 / 10f + 1f;
		if (num4 == 0 || num5 == 0)
		{
			num2 = Random.Range(0.1f, s_gamedesign * 0.1f) * num6;
			if ((bool)taskGame2.designer_)
			{
				num2 = ((!taskGame2.designer_.perks[14]) ? (num2 + taskGame2.designer_.s_gamedesign * 0.01f) : (num2 + taskGame2.designer_.s_gamedesign * 0.02f));
			}
			num2 = GetWorkResult(num2);
			if (perks[25] && taskGame2.gS_.typ_nachfolger)
			{
				num2 *= 2f;
			}
			if (taskGame2.gS_.devPoints <= 0f)
			{
				num2 *= 0.1f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Gameplay, num2, num, 13));
			}
			num += 0.4f;
			taskGame2.Work(num2, 0);
			Learn(gamedesign_: true, programmieren_: false, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
		}
		if (num4 == 1 || num5 == 1)
		{
			num2 = Random.Range(0.1f, s_grafik * 0.1f) * num6;
			num2 = GetWorkResult(num2);
			if (perks[23] && taskGame2.gS_.retro)
			{
				num2 *= 2f;
			}
			if (taskGame2.gS_.devPoints <= 0f)
			{
				num2 *= 0.1f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Grafik, num2, num, 13));
			}
			num += 0.4f;
			taskGame2.Work(num2, 1);
			Learn(gamedesign_: false, programmieren_: false, grafik_: true, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
		}
		if (num4 == 2 || num5 == 2)
		{
			num2 = Random.Range(0.1f, s_sound * 0.1f) * num6;
			num2 = GetWorkResult(num2);
			if (taskGame2.gS_.devPoints <= 0f)
			{
				num2 *= 0.1f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Sound, num2, num, 13));
			}
			num += 0.4f;
			taskGame2.Work(num2, 2);
			Learn(gamedesign_: false, programmieren_: false, grafik_: false, sound_: true, pr_: false, gametests_: false, technik_: false, forschen_: false);
		}
		if (num4 == 3 || num5 == 3)
		{
			num2 = Random.Range(0.1f, s_programmieren * 0.1f) * num6;
			num2 = GetWorkResult(num2);
			if (perks[24] && taskGame2.gS_.portID != -1)
			{
				num2 *= 2f;
			}
			if (taskGame2.gS_.devPoints <= 0f)
			{
				num2 *= 0.1f;
			}
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Technik, num2, num, 13));
			}
			num += 0.4f;
			taskGame2.Work(num2, 3);
			Learn(gamedesign_: false, programmieren_: true, grafik_: false, sound_: false, pr_: false, gametests_: false, technik_: false, forschen_: false);
		}
		if (perks[1] && Random.Range(0, 30) == 1)
		{
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_Hype, 1f, num, 13));
			}
			taskGame2.Work(1f, 5);
		}
		if (!perks[3] && taskGame2.gS_.devPoints_Gesamt > 0f)
		{
			bool flag = false;
			if (mS_.settings_sandbox && mS_.sandbox_bugs != 0)
			{
				if (mS_.sandbox_bugs == 10)
				{
					flag = true;
				}
				if (mS_.sandbox_bugs > 0 && Random.Range(0, 11) < mS_.sandbox_bugs)
				{
					flag = true;
				}
			}
			if (!flag)
			{
				if (Random.Range(0f, 10f + s_programmieren * 0.1f) < (float)(6 - level_aktuellesFeature_))
				{
					if (!settings_.disableWorkIcons && uiVisible)
					{
						StartCoroutine(CreatePopInSeconds(ePop_Bug, 1f, num, 24));
					}
					num += 0.4f;
					taskGame2.Work(1f, 4);
				}
				if (perks[21] && Random.Range(0f, 10f + s_programmieren * 0.1f) < (float)(6 - level_aktuellesFeature_))
				{
					if (!settings_.disableWorkIcons && uiVisible)
					{
						StartCoroutine(CreatePopInSeconds(ePop_Bug, 1f, num, 24));
					}
					num += 0.4f;
					taskGame2.Work(1f, 4);
				}
			}
		}
		if (taskGame2.gS_.devPoints_Gesamt <= 0f && taskGame2.gS_.points_bugs > 0f && Random.Range(0f, 100f) > 90f)
		{
			if (!settings_.disableWorkIcons && uiVisible)
			{
				StartCoroutine(CreatePopInSeconds(ePop_BugRemove, 1f, num, 13));
			}
			num += 0.4f;
			taskGame2.Work(1f, 6);
		}
		return true;
	}

	private float GetSkillCap()
	{
		if (mS_.settings_sandbox && mS_.sandbox_mitarbeiterSkill100)
		{
			return 100f;
		}
		if (!perks[15])
		{
			return 50f;
		}
		return 60f;
	}

	private float GetSkillCap_Skill(int i)
	{
		switch (i)
		{
		case 0:
			if (beruf != 0)
			{
				return GetSkillCap();
			}
			return 100f;
		case 1:
			if (beruf != 1)
			{
				return GetSkillCap();
			}
			return 100f;
		case 2:
			if (beruf != 2)
			{
				return GetSkillCap();
			}
			return 100f;
		case 3:
			if (beruf != 3)
			{
				return GetSkillCap();
			}
			return 100f;
		case 4:
			if (beruf != 4)
			{
				return GetSkillCap();
			}
			return 100f;
		case 5:
			if (beruf != 5)
			{
				return GetSkillCap();
			}
			return 100f;
		case 6:
			if (beruf != 6)
			{
				return GetSkillCap();
			}
			return 100f;
		case 7:
			if (beruf != 7)
			{
				return GetSkillCap();
			}
			return 100f;
		default:
			return GetSkillCap();
		}
	}

	private void Learn(bool gamedesign_, bool programmieren_, bool grafik_, bool sound_, bool pr_, bool gametests_, bool technik_, bool forschen_)
	{
		float num = Random.Range(0.001f, 0.002f);
		if (mS_.settings_sandbox && mS_.sandbox_trainingSpeed > 1f)
		{
			num *= mS_.sandbox_trainingSpeed;
		}
		if (perks[5])
		{
			num *= 2f;
		}
		if (perks[22])
		{
			num *= 0.5f;
		}
		float num2 = mS_.GetAchivementBonus(8);
		num2 *= 0.01f;
		num += num * num2;
		if (gamedesign_)
		{
			s_gamedesign += num;
			if (beruf != 0 && s_gamedesign > GetSkillCap())
			{
				s_gamedesign = GetSkillCap();
			}
			if (s_gamedesign > 100f)
			{
				s_gamedesign = 100f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(59);
				}
			}
		}
		else if (programmieren_)
		{
			s_programmieren += num;
			if (beruf != 1 && s_programmieren > GetSkillCap())
			{
				s_programmieren = GetSkillCap();
			}
			if (s_programmieren > 100f)
			{
				s_programmieren = 100f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(59);
				}
			}
		}
		else if (grafik_)
		{
			s_grafik += num;
			if (beruf != 2 && s_grafik > GetSkillCap())
			{
				s_grafik = GetSkillCap();
			}
			if (s_grafik > 100f)
			{
				s_grafik = 100f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(59);
				}
			}
		}
		else if (sound_)
		{
			s_sound += num;
			if (beruf != 3 && s_sound > GetSkillCap())
			{
				s_sound = GetSkillCap();
			}
			if (s_sound > 100f)
			{
				s_sound = 100f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(59);
				}
			}
		}
		else if (pr_)
		{
			s_pr += num;
			if (beruf != 4 && s_pr > GetSkillCap())
			{
				s_pr = GetSkillCap();
			}
			if (s_pr > 100f)
			{
				s_pr = 100f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(59);
				}
			}
		}
		else if (gametests_)
		{
			s_gametests += num;
			if (beruf != 5 && s_gametests > GetSkillCap())
			{
				s_gametests = GetSkillCap();
			}
			if (s_gametests > 100f)
			{
				s_gametests = 100f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(59);
				}
			}
		}
		else if (technik_)
		{
			s_technik += num;
			if (beruf != 6 && s_technik > GetSkillCap())
			{
				s_technik = GetSkillCap();
			}
			if (s_technik > 100f)
			{
				s_technik = 100f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(59);
				}
			}
		}
		else
		{
			if (!forschen_)
			{
				return;
			}
			s_forschen += num;
			if (beruf != 7 && s_forschen > GetSkillCap())
			{
				s_forschen = GetSkillCap();
			}
			if (s_forschen > 100f)
			{
				s_forschen = 100f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(59);
				}
			}
		}
	}

	private bool GetCritic(int criticBonus)
	{
		if (perks[27])
		{
			return false;
		}
		bool result = false;
		if (!perks[6])
		{
			if (Random.Range(0, 20 - criticBonus) == 1)
			{
				result = true;
			}
		}
		else if (Random.Range(0, 10 - criticBonus) == 1)
		{
			result = true;
		}
		return result;
	}

	public void Gehaltsverhandlung()
	{
		if ((bool)mS_ && (bool)guiMain_ && !perks[0])
		{
			if (mS_.personal_autoGehaltsverhandlung)
			{
				gehalt = CalcGehalt();
			}
			else if (!(mS_.GetGameSpeed() <= 0f) && gehalt > 0 && CalcGehalt() - gehalt > 500 && Random.Range(0, 100) > 90 && !guiMain_.uiObjects[409].activeSelf)
			{
				guiMain_.ActivateMenu(guiMain_.uiObjects[409]);
				guiMain_.uiObjects[409].GetComponent<Menu_PersonalLohnverhandlung>().Init(this);
			}
		}
	}

	public float RecalculateGehalt(float g)
	{
		if (mS_.settings_sandbox)
		{
			if (mS_.sandbox_mitarbeiterGehalt > 99f)
			{
				return 0f;
			}
			if (mS_.sandbox_mitarbeiterGehalt > 0f)
			{
				return Mathf.RoundToInt(g * mS_.sandbox_mitarbeiterGehalt);
			}
		}
		return g;
	}

	public int GetGehalt()
	{
		if (perks[0])
		{
			return 0;
		}
		if (gehalt <= 0)
		{
			gehalt = CalcGehalt();
		}
		if (mS_.settings_sandbox)
		{
			if (mS_.sandbox_mitarbeiterGehalt > 99f)
			{
				return 0;
			}
			if (mS_.sandbox_mitarbeiterGehalt > 0f)
			{
				return Mathf.RoundToInt((float)gehalt * mS_.sandbox_mitarbeiterGehalt);
			}
		}
		return gehalt;
	}

	public int CalcGehalt()
	{
		if (perks[0])
		{
			return 0;
		}
		int num = Mathf.RoundToInt(0f + s_gamedesign + s_programmieren + s_grafik + s_sound + s_pr + s_gametests + s_technik + s_forschen) * 10;
		for (int i = 0; i < perks.Length; i++)
		{
			if (perks[i])
			{
				num = i switch
				{
					0 => num, 
					1 => num + 10000, 
					14 => num + 1000, 
					15 => num + 2000, 
					18 => num - 500, 
					19 => num - 500, 
					20 => num - 500, 
					21 => num - 500, 
					22 => num - 500, 
					27 => num - 500, 
					_ => num + 500, 
				};
			}
		}
		if (num < 1000)
		{
			num = 1000;
		}
		if (perks[18])
		{
			num *= 2;
		}
		return num;
	}

	public void Entlassen(bool eventMitarbeiterMotivation)
	{
		if (perks[0])
		{
			return;
		}
		if (legend != -1 && mS_.devLegendsInUse.Length > legend)
		{
			mS_.devLegendsInUse[legend] = false;
		}
		if ((bool)myUI)
		{
			Object.Destroy(myUI);
		}
		if (objectUsingID != -1)
		{
			GameObject gameObject = GameObject.Find("O_" + objectUsingID);
			if ((bool)gameObject)
			{
				gameObject.GetComponent<objectScript>().besetztCharID = -1;
			}
		}
		if (objectBelegtID != -1)
		{
			GameObject gameObject2 = GameObject.Find("O_" + objectBelegtID);
			if ((bool)gameObject2)
			{
				gameObject2.GetComponent<objectScript>().besetztCharID = -1;
			}
		}
		if (eventMitarbeiterMotivation && Random.Range(0, 100) > 90)
		{
			guiMain_.EVENT_MitarbeiterMotivation();
		}
		mS_.findCharacters = true;
		Object.Destroy(base.gameObject);
	}

	public string GetGroupString(string farbe)
	{
		if (group != -1)
		{
			return "<color=" + farbe + ">[" + group + "]</color>";
		}
		return "";
	}

	public string GetGroupStringWithName(string farbe)
	{
		if (group != -1)
		{
			return "<color=" + farbe + ">" + mS_.personal_group_names[group - 1] + " (" + group + ")</color>";
		}
		return "";
	}

	public void Monatskosten()
	{
		int num = GetGehalt();
		if (num <= 0 || !mS_)
		{
			return;
		}
		mS_.Pay(num, 9);
		if (!settings_.moneyPop)
		{
			StartCoroutine(guiMain_.MoneyPopEnumerate(num, base.transform.position, green: false));
		}
		if (!roomS_)
		{
			return;
		}
		roomScript rS_ = roomS_;
		if ((bool)roomS_.taskGameObject && (bool)roomS_.GetTaskUnterstuetzen())
		{
			rS_ = roomS_.GetTaskUnterstuetzen().rS_;
		}
		if (!rS_ || !rS_.taskGameObject)
		{
			return;
		}
		taskGame taskGame2 = rS_.GetTaskGame();
		if ((bool)taskGame2)
		{
			if ((bool)taskGame2.gS_)
			{
				taskGame2.gS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskKonsole taskKonsole2 = rS_.GetTaskKonsole();
		if ((bool)taskKonsole2)
		{
			if ((bool)taskKonsole2.pS_)
			{
				taskKonsole2.pS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskPolishing taskPolishing2 = rS_.GetTaskPolishing();
		if ((bool)taskPolishing2)
		{
			if ((bool)taskPolishing2.gS_)
			{
				taskPolishing2.gS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskGrafikVerbessern taskGrafikVerbessern2 = rS_.GetTaskGrafikVerbessern();
		if ((bool)taskGrafikVerbessern2)
		{
			if ((bool)taskGrafikVerbessern2.gS_)
			{
				taskGrafikVerbessern2.gS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskSoundVerbessern taskSoundVerbessern2 = rS_.GetTaskSoundVerbessern();
		if ((bool)taskSoundVerbessern2)
		{
			if ((bool)taskSoundVerbessern2.gS_)
			{
				taskSoundVerbessern2.gS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskGameplayVerbessern taskGameplayVerbessern2 = rS_.GetTaskGameplayVerbessern();
		if ((bool)taskGameplayVerbessern2)
		{
			if ((bool)taskGameplayVerbessern2.gS_)
			{
				taskGameplayVerbessern2.gS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskBugfixing taskBugfixing2 = rS_.GetTaskBugfixing();
		if ((bool)taskBugfixing2)
		{
			if ((bool)taskBugfixing2.gS_)
			{
				taskBugfixing2.gS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskMarketing taskMarketing2 = rS_.GetTaskMarketing();
		if ((bool)taskMarketing2)
		{
			if ((bool)taskMarketing2.gS_)
			{
				taskMarketing2.gS_.costs_mitarbeiter += num;
			}
			if ((bool)taskMarketing2.pS_)
			{
				taskMarketing2.pS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskMarketingSpezial taskMarketingSpezial2 = rS_.GetTaskMarketingSpezial();
		if ((bool)taskMarketingSpezial2)
		{
			if ((bool)taskMarketingSpezial2.gS_)
			{
				taskMarketingSpezial2.gS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskKonsoleReduceCosts taskKonsoleReduceCosts2 = rS_.GetTaskKonsoleReduceCosts();
		if ((bool)taskKonsoleReduceCosts2)
		{
			if ((bool)taskKonsoleReduceCosts2.pS_)
			{
				taskKonsoleReduceCosts2.pS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskAnimationVerbessern taskAnimationVerbessern2 = rS_.GetTaskAnimationVerbessern();
		if ((bool)taskAnimationVerbessern2)
		{
			if ((bool)taskAnimationVerbessern2.gS_)
			{
				taskAnimationVerbessern2.gS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskF2PUpdate taskF2PUpdate2 = rS_.GetTaskF2PUpdate();
		if ((bool)taskF2PUpdate2)
		{
			if ((bool)taskF2PUpdate2.gS_)
			{
				taskF2PUpdate2.gS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskUpdate taskUpdate2 = rS_.GetTaskUpdate();
		if ((bool)taskUpdate2)
		{
			if ((bool)taskUpdate2.gS_)
			{
				taskUpdate2.gS_.costs_mitarbeiter += num;
			}
			return;
		}
		taskKonsoleHaltbarkeit taskKonsoleHaltbarkeit2 = rS_.GetTaskKonsoleHaltbarkeit();
		if ((bool)taskKonsoleHaltbarkeit2 && (bool)taskKonsoleHaltbarkeit2.pS_)
		{
			taskKonsoleHaltbarkeit2.pS_.costs_mitarbeiter += num;
		}
	}

	private void UpdateLeitenderEntwicklerIcon()
	{
		updateLeitenderEntwicklerIcon = !updateLeitenderEntwicklerIcon;
		if (!updateLeitenderEntwicklerIcon)
		{
			return;
		}
		if ((bool)roomS_)
		{
			if (roomS_.typ == 1 && (bool)roomS_.taskGameObject)
			{
				taskGame taskGame2 = roomS_.GetTaskGame();
				if ((bool)taskGame2 && taskGame2.leitenderDesignerID == myID)
				{
					if (!uiLeitenderDesigner.activeSelf)
					{
						uiLeitenderDesigner.SetActive(value: true);
					}
					return;
				}
			}
			if (roomS_.typ == 8 && (bool)roomS_.taskGameObject)
			{
				taskKonsole taskKonsole2 = roomS_.GetTaskKonsole();
				if ((bool)taskKonsole2 && taskKonsole2.leitenderTechnikerID == myID)
				{
					if (!uiLeitenderDesigner.activeSelf)
					{
						uiLeitenderDesigner.SetActive(value: true);
					}
					return;
				}
			}
		}
		if (uiLeitenderDesigner.activeSelf)
		{
			uiLeitenderDesigner.SetActive(value: false);
		}
	}
}
