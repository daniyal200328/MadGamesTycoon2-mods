using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vectrosity;

public class roomScript : MonoBehaviour
{
	private GameObject main_;

	public mainScript mS_;

	public Camera camera_;

	private settingsScript settings_;

	private mapScript mapS_;

	private GUI_Main guiMain_;

	private mainCameraScript mCamS_;

	private genres genres_;

	private themes themes_;

	private engineFeatures eF_;

	private gameplayFeatures gF_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	private textScript tS_;

	private buildRoomScript brS_;

	private roomDataScript rdS_;

	private forschungSonstiges fS_;

	public int myID;

	public int typ;

	public string myName;

	public int taskID = -1;

	public GameObject taskGameObject;

	public bool pause;

	public bool lockKI;

	private int arbeitsplaetze;

	private long lagerplatz;

	public long lagerplatzUsed;

	private long serverplatz;

	public long serverplatzUsed;

	public int mitarbeiterZugeteilt;

	public bool serverDown;

	public bool serverOverheat;

	public int leitenderGamedesigner = -1;

	public int leitenderTechniker = -1;

	public int serverReservieren;

	public Vector3 uiPos;

	public GameObject myDoor;

	public GameObject[] uiObjects;

	public GameObject myUI;

	private roomButtonScript rbS_;

	public GameObject myUI_Line;

	public GameObject myUI_UnterstuetzenLine;

	public Toggle toggleRoomUI;

	private bool outline;

	public List<GameObject> listGameObjects = new List<GameObject>();

	public List<objectScript> listInventar = new List<objectScript>();

	private Vector2 invisibleGUI = new Vector2(-300f, 0f);

	private RectTransform myGUIrectTransform;

	private Vector3 ROOMLINE_cameraPos;

	private Quaternion ROOMLINE_cameraRot;

	private VectorLine roomLine3D;

	private bool initRoomLine;

	private bool isCrunchTime;

	private float DrawLine_timer;

	private Vector3 cameraPos;

	private Quaternion cameraRot;

	private VectorLine drawLine3D;

	private bool initLine;

	private float lagerraumTimer;

	public long updateCosts;

	private taskFanshop myTaskFanshop;

	private taskWait myTaskWait;

	private taskUnterstuetzen myTaskUnterstuetzen;

	private taskPolishing myTaskPolishing;

	private taskMarktforschung myTaskMarktforschung;

	private taskAutoForschung myTaskAutoForschung;

	private taskForschungWait myTaskForschungWait;

	private taskContractWait myTaskContractWait;

	private taskContractWork myTaskContractWork;

	private taskSupport myTaskSupport;

	private taskFankampagne myTaskFankampagne;

	private taskKonsole myTaskKonsole;

	private taskKonsoleReduceCosts myTaskKonsoleReduceCosts;

	private taskKonsoleHaltbarkeit myTaskKonsoleHaltbarkeit;

	private taskArcadeProduction myTaskArcadeProduction;

	private taskProduction myTaskProduction;

	private taskAnimationVerbessern myTaskAnimationVerbessern;

	private taskSoundVerbessern myTaskSoundVerbessern;

	private taskGrafikVerbessern myTaskGrafikVerbessern;

	private taskBugfixing myTaskBugfixing;

	private taskGameplayVerbessern myTaskGameplayVerbessern;

	private taskSpielbericht myTaskSpielbericht;

	private taskTraining myTaskTraining;

	private taskMitarbeitersuche myTaskMitarbeitersuche;

	private taskMarketingSpezial myTaskMarketingSpezial;

	private taskMarketing myTaskMarketing;

	private taskF2PUpdate myTaskF2PUpdate;

	private taskGame myTaskGame;

	private taskForschung myTaskForschung;

	private taskEngine myTaskEngine;

	private taskUpdate myTaskUpdate;

	private void Start()
	{
		FindScripts();
		InitUI();
		mS_.findRooms = true;
	}

	private void OnDestroy()
	{
		if ((bool)mS_)
		{
			mS_.findRooms = true;
		}
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
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
		}
		if (!camera_)
		{
			camera_ = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
		}
		if (!mCamS_)
		{
			mCamS_ = camera_.GetComponent<mainCameraScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!brS_)
		{
			brS_ = main_.GetComponent<buildRoomScript>();
		}
		if (!hardware_)
		{
			hardware_ = main_.GetComponent<hardware>();
		}
		if (!hardwareFeatures_)
		{
			hardwareFeatures_ = main_.GetComponent<hardwareFeatures>();
		}
		if (!rdS_)
		{
			rdS_ = main_.GetComponent<roomDataScript>();
		}
		if (!fS_)
		{
			fS_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!toggleRoomUI)
		{
			toggleRoomUI = guiMain_.uiObjects[4].GetComponent<Toggle>();
		}
	}

	private void Update()
	{
		FindTasks();
		UpdateListInventar();
		UpdateLagerraumGFX();
		isCrunchTime = IsCrunchtime();
		UpdateUI();
	}

	private void InitUI()
	{
		if (!myUI)
		{
			myUI = Object.Instantiate(uiObjects[0], new Vector3(99999f, 99999f, 0f), Quaternion.identity);
			myUI.transform.SetParent(mS_.guiRooms.transform);
			rbS_ = myUI.GetComponent<roomButtonScript>();
			rbS_.rS_ = this;
			myGUIrectTransform = myUI.GetComponent<RectTransform>();
		}
	}

	private void FindTasks()
	{
		if (taskID == -1)
		{
			taskGameObject = null;
		}
		if (!taskGameObject && taskID != -1)
		{
			GameObject gameObject = GameObject.Find("Task_" + taskID);
			if ((bool)gameObject)
			{
				taskGameObject = gameObject;
				return;
			}
			taskID = -1;
			taskGameObject = null;
		}
	}

	private void UpdateUI()
	{
		if (!myUI)
		{
			return;
		}
		Vector3 position = uiPos;
		position.y += 2f;
		if (guiMain_.disableRoomGUI || toggleRoomUI.isOn || myID == mapScript.ID_FLOOROUTSIDE)
		{
			if (myUI.activeSelf)
			{
				myUI.SetActive(value: false);
			}
			if ((bool)myUI_UnterstuetzenLine && myUI_UnterstuetzenLine.activeSelf)
			{
				myUI_UnterstuetzenLine.SetActive(value: false);
			}
			if ((bool)myUI_Line && myUI_Line.activeSelf)
			{
				myUI_Line.SetActive(value: false);
			}
			return;
		}
		if (!myUI.activeSelf)
		{
			myUI.SetActive(value: true);
		}
		if ((bool)myUI_UnterstuetzenLine && !myUI_UnterstuetzenLine.activeSelf)
		{
			myUI_UnterstuetzenLine.SetActive(value: true);
		}
		if ((bool)myUI_Line && !myUI_Line.activeSelf)
		{
			myUI_Line.SetActive(value: true);
		}
		Vector2 vector = camera_.WorldToScreenPoint(position);
		Vector2 vector2 = vector;
		ShouldDrawLine();
		if (vector2.x < -200f || vector2.x >= (float)(Screen.width + 200) || vector2.y < -200f || vector2.y >= (float)(Screen.height + 200))
		{
			UpdateWindowForschung(show: false);
			UpdateWindowForschungWait(show: false);
			UpdateWindowAutoForschung(show: false);
			UpdateWindowEngine(show: false);
			UpdateWindowUpdate(show: false);
			UpdateWindowGame(show: false);
			UpdateWindowUnterstuetzen(show: false);
			UpdateWindowMarketing(show: false);
			UpdateWindowMarketingSpezial(show: false);
			UpdateWindowFankampagne(show: false);
			UpdateWindowTraining(show: false);
			UpdateWindowContractWork(show: false);
			UpdateWindowContractWorkWait(show: false);
			UpdateWindowAnrufe(show: false);
			UpdateWindowFanshop(show: false);
			UpdateWindowBugfixing(show: false);
			UpdateWindowGameplayVerbessern(show: false);
			UpdateWindowGrafikVerbessern(show: false);
			UpdateWindowSoundVerbessern(show: false);
			UpdateWindowAnimationVerbessern(show: false);
			UpdateWindowSpielbericht(show: false);
			UpdateWindowProduction(show: false);
			UpdateWindowMarktforschung(show: false);
			UpdateWindowPolishing(show: false);
			UpdateWindowF2PUpdate(show: false);
			UpdateWindowArcadeProduction(show: false);
			UpdateWindowLagerhaus(show: false);
			UpdateWindowServerraum(show: false);
			UpdateWindowKonsole(show: false);
			UpdateWindowWait(show: false);
			UpdateWindowMitarbeitersuche(show: false);
			UpdateWindowKonsoleReduceCosts(show: false);
			UpdateWindowKonsoleHaltbarkeit(show: false);
			myGUIrectTransform.anchoredPosition = guiMain_.GetAnchoredPosition(invisibleGUI);
			if (rbS_.CloseAllMenus())
			{
				mS_.PauseGame(p: false);
			}
			return;
		}
		vector = new Vector2(vector.x, vector.y - (float)Screen.height - 35f);
		myGUIrectTransform.anchoredPosition = guiMain_.GetAnchoredPosition(vector);
		position = uiPos;
		DrawRoomLine(position, new Vector3(position.x, position.y + 2f, position.z));
		if ((bool)taskGameObject)
		{
			switch (typ)
			{
			case 2:
				if ((bool)GetTaskForschung())
				{
					UpdateWindowForschung(show: true);
				}
				else
				{
					UpdateWindowForschung(show: false);
				}
				if ((bool)GetTaskForschungWait())
				{
					UpdateWindowForschungWait(show: true);
				}
				else
				{
					UpdateWindowForschungWait(show: false);
				}
				if ((bool)GetTaskAutoForschung())
				{
					UpdateWindowAutoForschung(show: true);
				}
				else
				{
					UpdateWindowAutoForschung(show: false);
				}
				break;
			case 1:
				if ((bool)GetTaskEngine())
				{
					UpdateWindowEngine(show: true);
				}
				else
				{
					UpdateWindowEngine(show: false);
				}
				if ((bool)GetTaskUpdate())
				{
					UpdateWindowUpdate(show: true);
				}
				else
				{
					UpdateWindowUpdate(show: false);
				}
				if ((bool)GetTaskGame())
				{
					UpdateWindowGame(show: true);
				}
				else
				{
					UpdateWindowGame(show: false);
				}
				if ((bool)GetTaskF2PUpdate())
				{
					UpdateWindowF2PUpdate(show: true);
				}
				else
				{
					UpdateWindowF2PUpdate(show: false);
				}
				break;
			case 6:
				if ((bool)GetTaskMarketing())
				{
					UpdateWindowMarketing(show: true);
				}
				else
				{
					UpdateWindowMarketing(show: false);
				}
				if ((bool)GetTaskMarketingSpezial())
				{
					UpdateWindowMarketingSpezial(show: true);
				}
				else
				{
					UpdateWindowMarketingSpezial(show: false);
				}
				if ((bool)GetTaskMitarbeitersuche())
				{
					UpdateWindowMitarbeitersuche(show: true);
				}
				else
				{
					UpdateWindowMitarbeitersuche(show: false);
				}
				if ((bool)GetTaskMarktforschung())
				{
					UpdateWindowMarktforschung(show: true);
				}
				else
				{
					UpdateWindowMarktforschung(show: false);
				}
				break;
			case 13:
				if ((bool)GetTaskTraining())
				{
					UpdateWindowTraining(show: true);
				}
				else
				{
					UpdateWindowTraining(show: false);
				}
				break;
			case 3:
				if ((bool)GetTaskSpielbericht())
				{
					UpdateWindowSpielbericht(show: true);
				}
				else
				{
					UpdateWindowSpielbericht(show: false);
				}
				if ((bool)GetTaskGameplayVerbessern())
				{
					UpdateWindowGameplayVerbessern(show: true);
				}
				else
				{
					UpdateWindowGameplayVerbessern(show: false);
				}
				if ((bool)GetTaskBugfixing())
				{
					UpdateWindowBugfixing(show: true);
				}
				else
				{
					UpdateWindowBugfixing(show: false);
				}
				break;
			case 4:
				if ((bool)GetTaskGrafikVerbessern())
				{
					UpdateWindowGrafikVerbessern(show: true);
				}
				else
				{
					UpdateWindowGrafikVerbessern(show: false);
				}
				break;
			case 5:
				if ((bool)GetTaskSoundVerbessern())
				{
					UpdateWindowSoundVerbessern(show: true);
				}
				else
				{
					UpdateWindowSoundVerbessern(show: false);
				}
				break;
			case 10:
				if ((bool)GetTaskAnimationVerbessern())
				{
					UpdateWindowAnimationVerbessern(show: true);
				}
				else
				{
					UpdateWindowAnimationVerbessern(show: false);
				}
				break;
			case 14:
				if ((bool)GetTaskProduction())
				{
					UpdateWindowProduction(show: true);
				}
				else
				{
					UpdateWindowProduction(show: false);
				}
				break;
			case 17:
				if ((bool)GetTaskArcadeProduction())
				{
					UpdateWindowArcadeProduction(show: true);
				}
				else
				{
					UpdateWindowArcadeProduction(show: false);
				}
				break;
			case 8:
				if ((bool)GetTaskKonsole())
				{
					UpdateWindowKonsole(show: true);
				}
				else
				{
					UpdateWindowKonsole(show: false);
				}
				if ((bool)GetTaskKonsoleReduceCosts())
				{
					UpdateWindowKonsoleReduceCosts(show: true);
				}
				else
				{
					UpdateWindowKonsoleReduceCosts(show: false);
				}
				if ((bool)GetTaskKonsoleHaltbarkeit())
				{
					UpdateWindowKonsoleHaltbarkeit(show: true);
				}
				else
				{
					UpdateWindowKonsoleHaltbarkeit(show: false);
				}
				break;
			case 7:
				if ((bool)GetTaskFankampagne())
				{
					UpdateWindowFankampagne(show: true);
				}
				else
				{
					UpdateWindowFankampagne(show: false);
				}
				if ((bool)GetTaskSupport())
				{
					UpdateWindowAnrufe(show: true);
				}
				else
				{
					UpdateWindowAnrufe(show: false);
				}
				if ((bool)GetTaskFanshop())
				{
					UpdateWindowFanshop(show: true);
				}
				else
				{
					UpdateWindowFanshop(show: false);
				}
				break;
			}
			if ((bool)GetTaskContractWork())
			{
				UpdateWindowContractWork(show: true);
			}
			else
			{
				UpdateWindowContractWork(show: false);
			}
			if ((bool)GetTaskContractWait())
			{
				UpdateWindowContractWorkWait(show: true);
			}
			else
			{
				UpdateWindowContractWorkWait(show: false);
			}
			if ((bool)GetTaskPolishing())
			{
				UpdateWindowPolishing(show: true);
			}
			else
			{
				UpdateWindowPolishing(show: false);
			}
			if ((bool)GetTaskUnterstuetzen())
			{
				UpdateWindowUnterstuetzen(show: true);
			}
			else
			{
				UpdateWindowUnterstuetzen(show: false);
			}
			if ((bool)GetTaskWait())
			{
				UpdateWindowWait(show: true);
			}
			else
			{
				UpdateWindowWait(show: false);
			}
		}
		else
		{
			UpdateWindowForschung(show: false);
			UpdateWindowForschungWait(show: false);
			UpdateWindowAutoForschung(show: false);
			UpdateWindowEngine(show: false);
			UpdateWindowUpdate(show: false);
			UpdateWindowGame(show: false);
			UpdateWindowUnterstuetzen(show: false);
			UpdateWindowMarketing(show: false);
			UpdateWindowMarketingSpezial(show: false);
			UpdateWindowFankampagne(show: false);
			UpdateWindowTraining(show: false);
			UpdateWindowContractWork(show: false);
			UpdateWindowContractWorkWait(show: false);
			UpdateWindowAnrufe(show: false);
			UpdateWindowFanshop(show: false);
			UpdateWindowBugfixing(show: false);
			UpdateWindowGameplayVerbessern(show: false);
			UpdateWindowGrafikVerbessern(show: false);
			UpdateWindowSoundVerbessern(show: false);
			UpdateWindowAnimationVerbessern(show: false);
			UpdateWindowSpielbericht(show: false);
			UpdateWindowProduction(show: false);
			UpdateWindowMarktforschung(show: false);
			UpdateWindowPolishing(show: false);
			UpdateWindowF2PUpdate(show: false);
			UpdateWindowArcadeProduction(show: false);
			UpdateWindowKonsole(show: false);
			UpdateWindowWait(show: false);
			UpdateWindowMitarbeitersuche(show: false);
			UpdateWindowKonsoleReduceCosts(show: false);
			UpdateWindowKonsoleHaltbarkeit(show: false);
			if (typ == 9)
			{
				UpdateWindowLagerhaus(show: true);
			}
			else
			{
				UpdateWindowLagerhaus(show: false);
			}
			if (typ == 15)
			{
				UpdateWindowServerraum(show: true);
			}
			else
			{
				UpdateWindowServerraum(show: false);
			}
		}
	}

	private void DrawRoomLine(Vector3 pStart, Vector3 pEnd)
	{
		if (guiMain_.initVectrocity)
		{
			if (!initRoomLine)
			{
				VectorManager.useDraw3D = true;
				initRoomLine = true;
				roomLine3D = new VectorLine("RoomLine3D_Room" + myID, new List<Vector3>(2), 12f, LineType.Continuous, Joins.Weld);
				roomLine3D.endCap = "RoomLine";
				GameObject gameObject = roomLine3D.rectTransform.gameObject;
				myUI_Line = gameObject;
				roomLine3D.color = Color.white;
				roomLine3D.points3[0] = pEnd;
				roomLine3D.points3[1] = pStart;
			}
			if (camera_.transform.position != ROOMLINE_cameraPos || camera_.transform.rotation != ROOMLINE_cameraRot || roomLine3D.points3[0] != pEnd || roomLine3D.points3[1] != pStart)
			{
				StartCoroutine(SetLineShaders());
				roomLine3D.points3[0] = pEnd;
				roomLine3D.points3[1] = pStart;
				ROOMLINE_cameraPos = camera_.transform.position;
				ROOMLINE_cameraRot = camera_.transform.rotation;
				roomLine3D.Draw3D();
			}
		}
	}

	public bool IsCrunchtimeRead()
	{
		return isCrunchTime;
	}

	private bool IsCrunchtime()
	{
		if (!taskGameObject)
		{
			return false;
		}
		if (typ == 11)
		{
			return false;
		}
		if (typ == 9)
		{
			return false;
		}
		if (typ == 0)
		{
			return false;
		}
		if (typ == 15)
		{
			return false;
		}
		if (typ == 12)
		{
			return false;
		}
		if (typ == 14)
		{
			return false;
		}
		float num = 0f;
		if (typ == 2 && num == 0f)
		{
			if ((bool)GetTaskForschung())
			{
				num = GetTaskForschung().GetProzent();
			}
			if ((bool)GetTaskForschungWait())
			{
				return false;
			}
			if ((bool)GetTaskAutoForschung())
			{
				return false;
			}
		}
		if (typ == 1)
		{
			if (num == 0f && (bool)GetTaskEngine())
			{
				num = GetTaskEngine().GetProzent();
			}
			if (num == 0f && (bool)GetTaskUpdate())
			{
				num = GetTaskUpdate().GetProzent();
			}
			if (num == 0f && (bool)GetTaskGame())
			{
				num = GetTaskGame().GetProzent();
			}
			if (num == 0f && (bool)GetTaskF2PUpdate())
			{
				num = GetTaskF2PUpdate().GetProzent();
			}
		}
		if (typ == 6)
		{
			if (num == 0f && (bool)GetTaskMarketing())
			{
				num = GetTaskMarketing().GetProzent();
			}
			if (num == 0f && (bool)GetTaskMarketingSpezial())
			{
				num = GetTaskMarketingSpezial().GetProzent();
			}
			if (num == 0f && (bool)GetTaskMitarbeitersuche())
			{
				num = GetTaskMitarbeitersuche().GetProzent();
			}
		}
		if (typ == 13 && num == 0f && (bool)GetTaskTraining())
		{
			num = GetTaskTraining().GetProzent();
		}
		if (num == 0f && (bool)GetTaskContractWork())
		{
			num = GetTaskContractWork().GetProzent();
		}
		if (num == 0f)
		{
			if ((bool)GetTaskContractWait())
			{
				return false;
			}
			if ((bool)GetTaskWait())
			{
				return false;
			}
		}
		if (typ == 7)
		{
			if (num == 0f && (bool)GetTaskFankampagne())
			{
				num = GetTaskFankampagne().GetProzent();
			}
			if (num == 0f && (bool)GetTaskSupport())
			{
				num = GetTaskSupport().GetProzent();
				if (num > 99.9f)
				{
					num = 99.9f;
				}
				return false;
			}
			if (num == 0f && (bool)GetTaskFanshop())
			{
				return false;
			}
		}
		if (typ == 3)
		{
			if (num == 0f && (bool)GetTaskBugfixing())
			{
				num = GetTaskBugfixing().GetProzent();
			}
			if (num == 0f && (bool)GetTaskGameplayVerbessern())
			{
				num = GetTaskGameplayVerbessern().GetProzent();
			}
			if (num == 0f && (bool)GetTaskSpielbericht())
			{
				num = GetTaskSpielbericht().GetProzent();
			}
		}
		if (typ == 4 && num == 0f && (bool)GetTaskGrafikVerbessern())
		{
			num = GetTaskGrafikVerbessern().GetProzent();
		}
		if (typ == 5 && num == 0f && (bool)GetTaskSoundVerbessern())
		{
			num = GetTaskSoundVerbessern().GetProzent();
		}
		if (typ == 10 && num == 0f && (bool)GetTaskAnimationVerbessern())
		{
			num = GetTaskAnimationVerbessern().GetProzent();
		}
		if (typ == 17 && num == 0f && (bool)GetTaskArcadeProduction())
		{
			num = GetTaskArcadeProduction().GetProzent();
		}
		if (typ == 8 && num == 0f)
		{
			if ((bool)GetTaskKonsole())
			{
				num = GetTaskKonsole().GetProzent();
			}
			if ((bool)GetTaskKonsoleReduceCosts())
			{
				num = GetTaskKonsoleReduceCosts().GetProzent();
			}
		}
		if (num <= 0f && (bool)GetTaskUnterstuetzen() && GetTaskUnterstuetzen().roomID != myID)
		{
			return GetTaskUnterstuetzen().IsCrunchtime();
		}
		if (num >= 100f)
		{
			return false;
		}
		if (num <= (float)mS_.personal_crunch)
		{
			return false;
		}
		return true;
	}

	private void ShouldDrawLine()
	{
		if (taskID != -1 && settings_.roomConnections)
		{
			if (!taskGameObject)
			{
				return;
			}
			taskPolishing taskPolishing2 = GetTaskPolishing();
			if ((bool)taskPolishing2)
			{
				if ((bool)taskPolishing2.rS_)
				{
					DrawLine(guiMain_.colors[22], taskPolishing2.rS_.uiPos);
				}
				return;
			}
			taskUnterstuetzen taskUnterstuetzen2 = GetTaskUnterstuetzen();
			if ((bool)taskUnterstuetzen2)
			{
				if ((bool)taskUnterstuetzen2.rS_)
				{
					DrawLine(new Color(1f, 1f, 1f, 0.5f), taskUnterstuetzen2.rS_.uiPos);
				}
				return;
			}
			if (typ == 3)
			{
				taskBugfixing taskBugfixing2 = GetTaskBugfixing();
				if ((bool)taskBugfixing2)
				{
					if ((bool)taskBugfixing2.rS_)
					{
						DrawLine(new Color(0f, 0f, 1f, 0.5f), taskBugfixing2.rS_.uiPos);
					}
					return;
				}
				taskGameplayVerbessern taskGameplayVerbessern2 = GetTaskGameplayVerbessern();
				if ((bool)taskGameplayVerbessern2)
				{
					if ((bool)taskGameplayVerbessern2.rS_)
					{
						DrawLine(new Color(0f, 0f, 1f, 0.5f), taskGameplayVerbessern2.rS_.uiPos);
					}
					return;
				}
			}
			if (typ == 4)
			{
				taskGrafikVerbessern taskGrafikVerbessern2 = GetTaskGrafikVerbessern();
				if ((bool)taskGrafikVerbessern2)
				{
					if ((bool)taskGrafikVerbessern2.rS_)
					{
						DrawLine(guiMain_.colors[9], taskGrafikVerbessern2.rS_.uiPos);
					}
					return;
				}
			}
			if (typ == 5)
			{
				taskSoundVerbessern taskSoundVerbessern2 = GetTaskSoundVerbessern();
				if ((bool)taskSoundVerbessern2)
				{
					if ((bool)taskSoundVerbessern2.rS_)
					{
						DrawLine(guiMain_.colors[10], taskSoundVerbessern2.rS_.uiPos);
					}
					return;
				}
			}
			if (typ == 10)
			{
				taskAnimationVerbessern taskAnimationVerbessern2 = GetTaskAnimationVerbessern();
				if ((bool)taskAnimationVerbessern2)
				{
					if ((bool)taskAnimationVerbessern2.rS_)
					{
						DrawLine(guiMain_.colors[11], taskAnimationVerbessern2.rS_.uiPos);
					}
					return;
				}
			}
			if ((bool)myUI_UnterstuetzenLine)
			{
				initLine = false;
				Object.Destroy(myUI_UnterstuetzenLine);
			}
		}
		else if ((bool)myUI_UnterstuetzenLine)
		{
			initLine = false;
			Object.Destroy(myUI_UnterstuetzenLine);
		}
	}

	private void DrawLine(Color color_, Vector3 uiPos_)
	{
		if (guiMain_.uiObjects[150].activeSelf || guiMain_.uiObjects[154].activeSelf)
		{
			return;
		}
		if (!initLine)
		{
			VectorManager.useDraw3D = true;
			initLine = true;
			drawLine3D = new VectorLine("Line3D_Room" + myID, new List<Vector3>(2), 20f, LineType.Continuous, Joins.Weld);
			drawLine3D.endCap = "Arrows";
			GameObject gameObject = drawLine3D.rectTransform.gameObject;
			myUI_UnterstuetzenLine = gameObject;
			mS_.gameObject.transform.position = uiPos;
			mS_.gameObject.transform.LookAt(uiPos_);
			mS_.gameObject.transform.Translate(Vector3.forward * 0.4f);
			Vector3 position = mS_.gameObject.transform.position;
			mS_.gameObject.transform.position = uiPos_;
			mS_.gameObject.transform.LookAt(uiPos);
			mS_.gameObject.transform.Translate(Vector3.forward * 0.4f);
			Vector3 position2 = mS_.gameObject.transform.position;
			mS_.gameObject.transform.position = new Vector3(0f, 0f, 0f);
			drawLine3D.color = color_;
			drawLine3D.points3[0] = position2;
			drawLine3D.points3[1] = position;
			cameraPos = new Vector3(-1f, -1f, -1f);
		}
		DrawLine_timer += Time.deltaTime;
		if (DrawLine_timer > 0.3f)
		{
			DrawLine_timer = 0f;
			mS_.gameObject.transform.position = uiPos;
			mS_.gameObject.transform.LookAt(uiPos_);
			mS_.gameObject.transform.Translate(Vector3.forward * 0.4f);
			Vector3 position3 = mS_.gameObject.transform.position;
			mS_.gameObject.transform.position = uiPos_;
			mS_.gameObject.transform.LookAt(uiPos);
			mS_.gameObject.transform.Translate(Vector3.forward * 0.4f);
			Vector3 position4 = mS_.gameObject.transform.position;
			mS_.gameObject.transform.position = new Vector3(0f, 0f, 0f);
			if (drawLine3D.points3[0] != position4 || drawLine3D.points3[1] != position3)
			{
				drawLine3D.points3[0] = position4;
				drawLine3D.points3[1] = position3;
				cameraPos = new Vector3(-1f, -1f, -1f);
			}
		}
		if (camera_.transform.position != cameraPos || camera_.transform.rotation != cameraRot)
		{
			StartCoroutine(SetLineShaders());
			cameraPos = camera_.transform.position;
			cameraRot = camera_.transform.rotation;
			drawLine3D.Draw3D();
		}
	}

	private IEnumerator SetLineShaders()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if ((bool)myUI_Line)
		{
			MeshRenderer component = myUI_Line.GetComponent<MeshRenderer>();
			if ((bool)component)
			{
				component.material.shader = mS_.shaders[0];
			}
		}
		if ((bool)myUI_UnterstuetzenLine)
		{
			MeshRenderer component2 = myUI_UnterstuetzenLine.GetComponent<MeshRenderer>();
			if ((bool)component2)
			{
				component2.material.shader = mS_.shaders[0];
			}
		}
	}

	public Vector2 Get2DPos()
	{
		Vector3 position = uiPos;
		return camera_.WorldToScreenPoint(position);
	}

	private void UpdateWindowForschung(bool show)
	{
		if (!rbS_.uiWindows[0])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[0].activeSelf)
			{
				rbS_.uiWindows[0].SetActive(value: false);
			}
			return;
		}
		taskForschung taskForschung2 = GetTaskForschung();
		if (taskForschung2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[0].activeSelf)
		{
			rbS_.uiWindows[0].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			switch (taskForschung2.typ)
			{
			case 0:
				rbS_.uiWindows[0].GetComponent<roomWindow>().Window_Forschung(genres_.GetName(taskForschung2.slot), genres_.GetProzent(taskForschung2.slot), genres_.GetPic(taskForschung2.slot), taskForschung2);
				break;
			case 1:
				rbS_.uiWindows[0].GetComponent<roomWindow>().Window_Forschung(tS_.GetThemes(taskForschung2.slot), themes_.GetProzent(taskForschung2.slot), themes_.icon, taskForschung2);
				break;
			case 2:
				rbS_.uiWindows[0].GetComponent<roomWindow>().Window_Forschung(eF_.GetName(taskForschung2.slot), eF_.GetProzent(taskForschung2.slot), eF_.GetTypPic(taskForschung2.slot), taskForschung2);
				break;
			case 3:
				rbS_.uiWindows[0].GetComponent<roomWindow>().Window_Forschung(gF_.GetName(taskForschung2.slot), gF_.GetProzent(taskForschung2.slot), gF_.GetTypSprite(taskForschung2.slot), taskForschung2);
				break;
			case 4:
				rbS_.uiWindows[0].GetComponent<roomWindow>().Window_Forschung(hardware_.GetName(taskForschung2.slot), hardware_.GetProzent(taskForschung2.slot), hardware_.GetTypPic(taskForschung2.slot), taskForschung2);
				break;
			case 5:
				rbS_.uiWindows[0].GetComponent<roomWindow>().Window_Forschung(fS_.GetName(taskForschung2.slot), fS_.GetProzent(taskForschung2.slot), fS_.RES_SPRITE[taskForschung2.slot], taskForschung2);
				break;
			case 6:
				rbS_.uiWindows[0].GetComponent<roomWindow>().Window_Forschung(hardwareFeatures_.GetName(taskForschung2.slot), hardwareFeatures_.GetProzent(taskForschung2.slot), hardwareFeatures_.GetSprite(taskForschung2.slot), taskForschung2);
				break;
			}
		}
	}

	private void UpdateWindowEngine(bool show)
	{
		if (!rbS_.uiWindows[1])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[1].activeSelf)
			{
				rbS_.uiWindows[1].SetActive(value: false);
			}
			return;
		}
		taskEngine taskEngine2 = GetTaskEngine();
		if (taskEngine2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[1].activeSelf)
		{
			rbS_.uiWindows[1].SetActive(value: true);
		}
		if ((bool)taskGameObject && (bool)taskEngine2.eS_)
		{
			if (!taskEngine2.eS_.updating)
			{
				rbS_.uiWindows[1].GetComponent<roomWindow>().Window_DevEngine(taskEngine2.eS_.GetName(), taskEngine2.eS_.GetProzent(), guiMain_.uiSprites[4]);
			}
			else
			{
				rbS_.uiWindows[1].GetComponent<roomWindow>().Window_DevEngine(taskEngine2.eS_.GetName(), taskEngine2.eS_.GetProzent(), guiMain_.uiSprites[5]);
			}
		}
	}

	private void UpdateWindowGame(bool show)
	{
		if (!rbS_.uiWindows[2])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[2].activeSelf)
			{
				rbS_.uiWindows[2].SetActive(value: false);
			}
			return;
		}
		taskGame taskGame2 = GetTaskGame();
		if (taskGame2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[2].activeSelf)
		{
			rbS_.uiWindows[2].SetActive(value: true);
		}
		if ((bool)taskGameObject && (bool)taskGame2.gS_)
		{
			rbS_.uiWindows[2].GetComponent<roomWindow>().Window_DevGame(taskGame2.gS_, taskGame2);
		}
	}

	private void UpdateWindowUnterstuetzen(bool show)
	{
		if (!rbS_.uiWindows[3])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[3].activeSelf)
			{
				rbS_.uiWindows[3].SetActive(value: false);
			}
			return;
		}
		taskUnterstuetzen taskUnterstuetzen2 = GetTaskUnterstuetzen();
		if (taskUnterstuetzen2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[3].activeSelf)
		{
			rbS_.uiWindows[3].SetActive(value: true);
		}
		if (!taskGameObject || !taskUnterstuetzen2.rS_)
		{
			return;
		}
		float prozent = -1f;
		if ((bool)taskUnterstuetzen2.rS_.taskGameObject)
		{
			bool flag = false;
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskGame())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskGame().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskEngine())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskEngine().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskForschung())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskForschung().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskForschungWait())
			{
				prozent = -1f;
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskAutoForschung())
			{
				prozent = -1f;
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskPolishing())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskPolishing().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskMarketing())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskMarketing().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskMarketingSpezial())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskMarketingSpezial().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskContractWork())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskContractWork().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskContractWait())
			{
				prozent = -1f;
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskWait())
			{
				prozent = -1f;
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskUpdate())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskUpdate().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskF2PUpdate())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskF2PUpdate().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskFankampagne())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskFankampagne().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskMitarbeitersuche())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskMitarbeitersuche().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskSupport())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskSupport().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskFanshop())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskFanshop().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskBugfixing())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskBugfixing().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskGameplayVerbessern())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskGameplayVerbessern().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskGrafikVerbessern())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskGrafikVerbessern().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskSoundVerbessern())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskSoundVerbessern().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskAnimationVerbessern())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskAnimationVerbessern().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskSpielbericht())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskSpielbericht().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskProduction())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskProduction().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskArcadeProduction())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskArcadeProduction().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskKonsole())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskKonsole().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskKonsoleReduceCosts())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskKonsoleReduceCosts().GetProzent();
				flag = true;
			}
			if (!flag && (bool)taskUnterstuetzen2.rS_.GetTaskKonsoleHaltbarkeit())
			{
				prozent = taskUnterstuetzen2.rS_.GetTaskKonsoleHaltbarkeit().GetProzent();
				flag = true;
			}
		}
		if (taskUnterstuetzen2.rS_.myName.Length > 0)
		{
			rbS_.uiWindows[3].GetComponent<roomWindow>().Window_Unterstuetzen(taskUnterstuetzen2.rS_.myName, prozent);
		}
		else
		{
			rbS_.uiWindows[3].GetComponent<roomWindow>().Window_Unterstuetzen(rdS_.GetName(taskUnterstuetzen2.rS_.typ), prozent);
		}
	}

	private void UpdateWindowContractWork(bool show)
	{
		if (!rbS_.uiWindows[6])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[6].activeSelf)
			{
				rbS_.uiWindows[6].SetActive(value: false);
			}
			return;
		}
		taskContractWork taskContractWork2 = GetTaskContractWork();
		if (taskContractWork2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[6].activeSelf)
		{
			rbS_.uiWindows[6].SetActive(value: true);
		}
		if ((bool)taskGameObject && (bool)taskContractWork2.contract_)
		{
			rbS_.uiWindows[6].GetComponent<roomWindow>().Window_ContractWork(taskContractWork2, this);
		}
	}

	private void UpdateWindowAutoForschung(bool show)
	{
		if (!rbS_.uiWindows[32])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[32].activeSelf)
			{
				rbS_.uiWindows[32].SetActive(value: false);
			}
			return;
		}
		taskAutoForschung taskAutoForschung2 = GetTaskAutoForschung();
		if (taskAutoForschung2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[32].activeSelf)
		{
			rbS_.uiWindows[32].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[32].GetComponent<roomWindow>().Window_AutoForschung(taskAutoForschung2);
		}
	}

	private void UpdateWindowForschungWait(bool show)
	{
		if (!rbS_.uiWindows[29])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[29].activeSelf)
			{
				rbS_.uiWindows[29].SetActive(value: false);
			}
			return;
		}
		taskForschungWait taskForschungWait2 = GetTaskForschungWait();
		if (taskForschungWait2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[29].activeSelf)
		{
			rbS_.uiWindows[29].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[29].GetComponent<roomWindow>().Window_ForschungWait(taskForschungWait2);
		}
	}

	private void UpdateWindowContractWorkWait(bool show)
	{
		if (!rbS_.uiWindows[25])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[25].activeSelf)
			{
				rbS_.uiWindows[25].SetActive(value: false);
			}
			return;
		}
		taskContractWait taskContractWait2 = GetTaskContractWait();
		if (taskContractWait2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[25].activeSelf)
		{
			rbS_.uiWindows[25].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[25].GetComponent<roomWindow>().Window_ContractWorkWait(taskContractWait2);
		}
	}

	private void UpdateWindowWait(bool show)
	{
		if (!rbS_.uiWindows[26])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[26].activeSelf)
			{
				rbS_.uiWindows[26].SetActive(value: false);
			}
			return;
		}
		taskWait taskWait2 = GetTaskWait();
		if (taskWait2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[26].activeSelf)
		{
			rbS_.uiWindows[26].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[26].GetComponent<roomWindow>().Window_Wait(taskWait2);
		}
	}

	private void UpdateWindowKonsole(bool show)
	{
		if (!rbS_.uiWindows[24])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[24].activeSelf)
			{
				rbS_.uiWindows[24].SetActive(value: false);
			}
			return;
		}
		taskKonsole taskKonsole2 = GetTaskKonsole();
		if (taskKonsole2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[24].activeSelf)
		{
			rbS_.uiWindows[24].SetActive(value: true);
		}
		if ((bool)taskGameObject && (bool)taskKonsole2.pS_)
		{
			rbS_.uiWindows[24].GetComponent<roomWindow>().Window_Konsole(taskKonsole2);
		}
	}

	private void UpdateWindowArcadeProduction(bool show)
	{
		if (!rbS_.uiWindows[23])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[23].activeSelf)
			{
				rbS_.uiWindows[23].SetActive(value: false);
			}
			return;
		}
		taskArcadeProduction taskArcadeProduction2 = GetTaskArcadeProduction();
		if (taskArcadeProduction2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[23].activeSelf)
		{
			rbS_.uiWindows[23].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[23].GetComponent<roomWindow>().Window_ArcadeProduction(taskArcadeProduction2);
		}
	}

	private void UpdateWindowF2PUpdate(bool show)
	{
		if (!rbS_.uiWindows[22])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[22].activeSelf)
			{
				rbS_.uiWindows[22].SetActive(value: false);
			}
			return;
		}
		taskF2PUpdate taskF2PUpdate2 = GetTaskF2PUpdate();
		if (taskF2PUpdate2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[22].activeSelf)
		{
			rbS_.uiWindows[22].SetActive(value: true);
		}
		if ((bool)taskGameObject && (bool)taskF2PUpdate2.gS_)
		{
			rbS_.uiWindows[22].GetComponent<roomWindow>().Window_F2PUpdate(taskF2PUpdate2);
		}
	}

	private void UpdateWindowUpdate(bool show)
	{
		if (!rbS_.uiWindows[7])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[7].activeSelf)
			{
				rbS_.uiWindows[7].SetActive(value: false);
			}
			return;
		}
		taskUpdate taskUpdate2 = GetTaskUpdate();
		if (taskUpdate2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[7].activeSelf)
		{
			rbS_.uiWindows[7].SetActive(value: true);
		}
		if ((bool)taskGameObject && (bool)taskUpdate2.gS_)
		{
			rbS_.uiWindows[7].GetComponent<roomWindow>().Window_Update(taskUpdate2);
		}
	}

	private void UpdateWindowTraining(bool show)
	{
		if (!rbS_.uiWindows[5])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[5].activeSelf)
			{
				rbS_.uiWindows[5].SetActive(value: false);
			}
			return;
		}
		taskTraining taskTraining2 = GetTaskTraining();
		if (taskTraining2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[5].activeSelf)
		{
			rbS_.uiWindows[5].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[5].GetComponent<roomWindow>().Window_Training(tS_.GetText(taskTraining2.slot + 538), taskTraining2.GetProzent(), taskTraining2.GetPic(), taskTraining2.automatic);
		}
	}

	private void UpdateWindowFankampagne(bool show)
	{
		if (!rbS_.uiWindows[8])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[8].activeSelf)
			{
				rbS_.uiWindows[8].SetActive(value: false);
			}
			return;
		}
		taskFankampagne taskFankampagne2 = GetTaskFankampagne();
		if (taskFankampagne2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[8].activeSelf)
		{
			rbS_.uiWindows[8].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[8].GetComponent<roomWindow>().Window_Fankampagne(taskFankampagne2);
		}
	}

	private void UpdateWindowMitarbeitersuche(bool show)
	{
		if (!rbS_.uiWindows[27])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[27].activeSelf)
			{
				rbS_.uiWindows[27].SetActive(value: false);
			}
			return;
		}
		taskMitarbeitersuche taskMitarbeitersuche2 = GetTaskMitarbeitersuche();
		if (taskMitarbeitersuche2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[27].activeSelf)
		{
			rbS_.uiWindows[27].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[27].GetComponent<roomWindow>().Window_Mitarbeitersuche(taskMitarbeitersuche2);
		}
	}

	private void UpdateWindowMarktforschung(bool show)
	{
		if (!rbS_.uiWindows[18])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[18].activeSelf)
			{
				rbS_.uiWindows[18].SetActive(value: false);
			}
			return;
		}
		taskMarktforschung taskMarktforschung2 = GetTaskMarktforschung();
		if (taskMarktforschung2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[18].activeSelf)
		{
			rbS_.uiWindows[18].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[18].GetComponent<roomWindow>().Window_Marktforschung(taskMarktforschung2);
		}
	}

	private void UpdateWindowAnrufe(bool show)
	{
		if (!rbS_.uiWindows[9])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[9].activeSelf)
			{
				rbS_.uiWindows[9].SetActive(value: false);
			}
			return;
		}
		taskSupport taskSupport2 = GetTaskSupport();
		if (taskSupport2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[9].activeSelf)
		{
			rbS_.uiWindows[9].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[9].GetComponent<roomWindow>().Window_Anrufe(taskSupport2);
		}
	}

	private void UpdateWindowFanshop(bool show)
	{
		if (!rbS_.uiWindows[28])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[28].activeSelf)
			{
				rbS_.uiWindows[28].SetActive(value: false);
			}
			return;
		}
		taskFanshop taskFanshop2 = GetTaskFanshop();
		if (taskFanshop2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[28].activeSelf)
		{
			rbS_.uiWindows[28].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[28].GetComponent<roomWindow>().Window_Fanshop(taskFanshop2);
		}
	}

	private void UpdateWindowBugfixing(bool show)
	{
		if (!rbS_.uiWindows[10])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[10].activeSelf)
			{
				rbS_.uiWindows[10].SetActive(value: false);
			}
			return;
		}
		taskBugfixing taskBugfixing2 = GetTaskBugfixing();
		if (taskBugfixing2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[10].activeSelf)
		{
			rbS_.uiWindows[10].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[10].GetComponent<roomWindow>().Window_Bugfixing(taskBugfixing2);
		}
	}

	private void UpdateWindowPolishing(bool show)
	{
		if (!rbS_.uiWindows[20])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[20].activeSelf)
			{
				rbS_.uiWindows[20].SetActive(value: false);
			}
			return;
		}
		taskPolishing taskPolishing2 = GetTaskPolishing();
		if (taskPolishing2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[20].activeSelf)
		{
			rbS_.uiWindows[20].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[20].GetComponent<roomWindow>().Window_Polishing(taskPolishing2);
		}
	}

	private void UpdateWindowSpielbericht(bool show)
	{
		if (!rbS_.uiWindows[15])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[15].activeSelf)
			{
				rbS_.uiWindows[15].SetActive(value: false);
			}
			return;
		}
		taskSpielbericht taskSpielbericht2 = GetTaskSpielbericht();
		if (taskSpielbericht2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[15].activeSelf)
		{
			rbS_.uiWindows[15].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[15].GetComponent<roomWindow>().Window_Spielbericht(taskSpielbericht2);
		}
	}

	private void UpdateWindowProduction(bool show)
	{
		if (!rbS_.uiWindows[16])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[16].activeSelf)
			{
				rbS_.uiWindows[16].SetActive(value: false);
			}
			return;
		}
		taskProduction taskProduction2 = GetTaskProduction();
		if (taskProduction2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[16].activeSelf)
		{
			rbS_.uiWindows[16].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[16].GetComponent<roomWindow>().Window_Production(taskProduction2);
		}
	}

	private void UpdateWindowLagerhaus(bool show)
	{
		if (!rbS_.uiWindows[17])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[17].activeSelf)
			{
				rbS_.uiWindows[17].SetActive(value: false);
			}
			return;
		}
		if (!rbS_.uiWindows[17].activeSelf)
		{
			rbS_.uiWindows[17].SetActive(value: true);
		}
		rbS_.uiWindows[17].GetComponent<roomWindow>().Window_Lagerhaus(this);
	}

	private void UpdateWindowServerraum(bool show)
	{
		if (!rbS_.uiWindows[19])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[19].activeSelf)
			{
				rbS_.uiWindows[19].SetActive(value: false);
			}
			return;
		}
		if (!rbS_.uiWindows[19].activeSelf)
		{
			rbS_.uiWindows[19].SetActive(value: true);
		}
		rbS_.uiWindows[19].GetComponent<roomWindow>().Window_Serverraum(this);
	}

	private void UpdateWindowGameplayVerbessern(bool show)
	{
		if (!rbS_.uiWindows[11])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[11].activeSelf)
			{
				rbS_.uiWindows[11].SetActive(value: false);
			}
			return;
		}
		taskGameplayVerbessern taskGameplayVerbessern2 = GetTaskGameplayVerbessern();
		if (taskGameplayVerbessern2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[11].activeSelf)
		{
			rbS_.uiWindows[11].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[11].GetComponent<roomWindow>().Window_GameplayVerbessern(taskGameplayVerbessern2);
		}
	}

	private void UpdateWindowGrafikVerbessern(bool show)
	{
		if (!rbS_.uiWindows[12])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[12].activeSelf)
			{
				rbS_.uiWindows[12].SetActive(value: false);
			}
			return;
		}
		taskGrafikVerbessern taskGrafikVerbessern2 = GetTaskGrafikVerbessern();
		if (taskGrafikVerbessern2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[12].activeSelf)
		{
			rbS_.uiWindows[12].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[12].GetComponent<roomWindow>().Window_GrafikVerbessern(taskGrafikVerbessern2);
		}
	}

	private void UpdateWindowKonsoleReduceCosts(bool show)
	{
		if (!rbS_.uiWindows[30])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[30].activeSelf)
			{
				rbS_.uiWindows[30].SetActive(value: false);
			}
			return;
		}
		taskKonsoleReduceCosts taskKonsoleReduceCosts2 = GetTaskKonsoleReduceCosts();
		if (taskKonsoleReduceCosts2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[30].activeSelf)
		{
			rbS_.uiWindows[30].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[30].GetComponent<roomWindow>().Window_KonsoleReduceCosts(taskKonsoleReduceCosts2);
		}
	}

	private void UpdateWindowKonsoleHaltbarkeit(bool show)
	{
		if (!rbS_.uiWindows[31])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[31].activeSelf)
			{
				rbS_.uiWindows[31].SetActive(value: false);
			}
			return;
		}
		taskKonsoleHaltbarkeit taskKonsoleHaltbarkeit2 = GetTaskKonsoleHaltbarkeit();
		if (taskKonsoleHaltbarkeit2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[31].activeSelf)
		{
			rbS_.uiWindows[31].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[31].GetComponent<roomWindow>().Window_KonsoleHaltbarkeit(taskKonsoleHaltbarkeit2);
		}
	}

	private void UpdateWindowSoundVerbessern(bool show)
	{
		if (!rbS_.uiWindows[13])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[13].activeSelf)
			{
				rbS_.uiWindows[13].SetActive(value: false);
			}
			return;
		}
		taskSoundVerbessern taskSoundVerbessern2 = GetTaskSoundVerbessern();
		if (taskSoundVerbessern2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[13].activeSelf)
		{
			rbS_.uiWindows[13].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[13].GetComponent<roomWindow>().Window_SoundVerbessern(taskSoundVerbessern2);
		}
	}

	private void UpdateWindowAnimationVerbessern(bool show)
	{
		if (!rbS_.uiWindows[14])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[14].activeSelf)
			{
				rbS_.uiWindows[14].SetActive(value: false);
			}
			return;
		}
		taskAnimationVerbessern taskAnimationVerbessern2 = GetTaskAnimationVerbessern();
		if (taskAnimationVerbessern2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[14].activeSelf)
		{
			rbS_.uiWindows[14].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[14].GetComponent<roomWindow>().Window_AnimationVerbessern(taskAnimationVerbessern2);
		}
	}

	private void UpdateWindowMarketingSpezial(bool show)
	{
		if (!rbS_.uiWindows[21])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[21].activeSelf)
			{
				rbS_.uiWindows[21].SetActive(value: false);
			}
			return;
		}
		taskMarketingSpezial taskMarketingSpezial2 = GetTaskMarketingSpezial();
		if (taskMarketingSpezial2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[21].activeSelf)
		{
			rbS_.uiWindows[21].SetActive(value: true);
		}
		if ((bool)taskGameObject)
		{
			rbS_.uiWindows[21].GetComponent<roomWindow>().Window_MarketingSpezial(taskMarketingSpezial2);
		}
	}

	private void UpdateWindowMarketing(bool show)
	{
		if (!rbS_.uiWindows[4])
		{
			return;
		}
		if (!show)
		{
			if (rbS_.uiWindows[4].activeSelf)
			{
				rbS_.uiWindows[4].SetActive(value: false);
			}
			return;
		}
		taskMarketing taskMarketing2 = GetTaskMarketing();
		if (taskMarketing2.myID != taskID)
		{
			taskGameObject = null;
			return;
		}
		if (!rbS_.uiWindows[4].activeSelf)
		{
			rbS_.uiWindows[4].SetActive(value: true);
		}
		if (!taskGameObject)
		{
			return;
		}
		switch (taskMarketing2.typ)
		{
		case 0:
			if ((bool)taskMarketing2.gS_)
			{
				rbS_.uiWindows[4].GetComponent<roomWindow>().Window_Marketing(taskMarketing2.gS_.GetNameWithTag(), taskMarketing2.GetProzent(), taskMarketing2.GetPic(), taskMarketing2);
			}
			break;
		case 1:
			if ((bool)taskMarketing2.pS_)
			{
				rbS_.uiWindows[4].GetComponent<roomWindow>().Window_Marketing(taskMarketing2.pS_.GetName(), taskMarketing2.GetProzent(), taskMarketing2.GetPic(), taskMarketing2);
			}
			break;
		}
	}

	public void SetOutlineLayer()
	{
		if (outline)
		{
			return;
		}
		outline = true;
		mCamS_.SetOutlineColor(2, 0.3f, 4);
		for (int i = 0; i < listGameObjects.Count; i++)
		{
			if ((bool)listGameObjects[i])
			{
				SetLayer(11, listGameObjects[i].transform.GetChild(0));
			}
		}
	}

	public void SetListGameObjectsLayer(int l)
	{
		for (int i = 0; i < listGameObjects.Count; i++)
		{
			if ((bool)listGameObjects[i])
			{
				SetLayer(l, listGameObjects[i].transform.GetChild(0));
			}
		}
	}

	public void DisableOutlineLayer()
	{
		if (!outline)
		{
			return;
		}
		outline = false;
		for (int i = 0; i < listGameObjects.Count; i++)
		{
			if ((bool)listGameObjects[i])
			{
				SetLayer(0, listGameObjects[i].transform.GetChild(0));
			}
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

	public void UpdateListInventar()
	{
		arbeitsplaetze = 0;
		lagerplatz = 0L;
		serverplatz = 0L;
		serverOverheat = false;
		if (!mS_)
		{
			FindScripts();
		}
		int count = listInventar.Count;
		for (int i = 0; i < count; i++)
		{
			if (!listInventar[i])
			{
				listInventar.RemoveAt(i);
				UpdateListInventar();
				break;
			}
			if (typ == 11 || typ == 12 || typ == 16 || typ == 14 || typ == 0)
			{
				continue;
			}
			if (listInventar[i].isArbeitsplatz)
			{
				arbeitsplaetze++;
			}
			else if (listInventar[i].isLager)
			{
				if (!mS_.settings_sandbox)
				{
					lagerplatz += listInventar[i].lagerplatz;
					continue;
				}
				if (mS_.sandbox_lager <= 0)
				{
					mS_.sandbox_lager = 1;
				}
				lagerplatz += listInventar[i].lagerplatz * mS_.sandbox_lager;
			}
			else
			{
				if (!listInventar[i].isServer)
				{
					continue;
				}
				if (!mS_.settings_sandbox)
				{
					serverplatz += listInventar[i].GetServerplatz();
					continue;
				}
				if (mS_.sandbox_server <= 0)
				{
					mS_.sandbox_server = 1;
				}
				serverplatz += listInventar[i].GetServerplatz() * mS_.sandbox_server;
			}
		}
	}

	public bool IsUberberfuell()
	{
		if (arbeitsplaetze <= 0)
		{
			return false;
		}
		if (GetMitarbeiter() > AnzahlArbeitsplaetzeBisUberfuellt())
		{
			return true;
		}
		return false;
	}

	public int AnzahlArbeitsplaetzeBisUberfuellt()
	{
		float num = 3.3f;
		switch (typ)
		{
		case 10:
			num = 10f;
			break;
		case 5:
			num = 5f;
			break;
		case 13:
			num = 2.7f;
			break;
		}
		int num2 = Mathf.FloorToInt((float)listGameObjects.Count / num);
		num2--;
		if (num2 < 0)
		{
			num2 = 0;
		}
		return num2;
	}

	public GameObject GetRandomFloor()
	{
		return listGameObjects[Random.Range(0, listGameObjects.Count)];
	}

	public int GetArbeitsplaetze()
	{
		return arbeitsplaetze;
	}

	public int GetMitarbeiter()
	{
		return mitarbeiterZugeteilt;
	}

	public void Demolish()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if ((bool)myUI)
		{
			Object.Destroy(myUI);
		}
		if ((bool)myUI_Line)
		{
			Object.Destroy(myUI_Line);
		}
		if ((bool)myUI_UnterstuetzenLine)
		{
			Object.Destroy(myUI_UnterstuetzenLine);
		}
		for (int i = 0; i < listInventar.Count; i++)
		{
			if ((bool)listInventar[i])
			{
				Object.Destroy(listInventar[i].gameObject);
			}
		}
		mS_.findRooms = true;
		mapS_.RemoveRoom(myID, particle: true);
		Object.Destroy(base.gameObject);
	}

	public bool KeineAnrufe()
	{
		if (mS_.anrufe > 0)
		{
			return false;
		}
		if (typ != 7)
		{
			return false;
		}
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject && (bool)GetTaskSupport() && mS_.anrufe <= 0)
		{
			return true;
		}
		return false;
	}

	public bool WERK_GameHasBestellungen()
	{
		if (typ != 17)
		{
			return false;
		}
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject)
		{
			taskArcadeProduction taskArcadeProduction2 = GetTaskArcadeProduction();
			if ((bool)taskArcadeProduction2 && (bool)taskArcadeProduction2.gS_ && taskArcadeProduction2.gS_.vorbestellungen > 0)
			{
				return true;
			}
		}
		return false;
	}

	public bool GameIsPort()
	{
		if (typ != 1 && typ != 3 && typ != 5 && typ != 4 && typ != 10)
		{
			return false;
		}
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject)
		{
			taskGame taskGame2 = GetTaskGame();
			if ((bool)taskGame2 && (bool)taskGame2.gS_ && taskGame2.gS_.portID != -1)
			{
				return true;
			}
			taskGameplayVerbessern taskGameplayVerbessern2 = GetTaskGameplayVerbessern();
			if ((bool)taskGameplayVerbessern2 && (bool)taskGameplayVerbessern2.gS_ && taskGameplayVerbessern2.gS_.portID != -1)
			{
				return true;
			}
			taskSoundVerbessern taskSoundVerbessern2 = GetTaskSoundVerbessern();
			if ((bool)taskSoundVerbessern2 && (bool)taskSoundVerbessern2.gS_ && taskSoundVerbessern2.gS_.portID != -1)
			{
				return true;
			}
			taskGameplayVerbessern taskGameplayVerbessern3 = GetTaskGameplayVerbessern();
			if ((bool)taskGameplayVerbessern3 && (bool)taskGameplayVerbessern3.gS_ && taskGameplayVerbessern3.gS_.portID != -1)
			{
				return true;
			}
			taskAnimationVerbessern taskAnimationVerbessern2 = GetTaskAnimationVerbessern();
			if ((bool)taskAnimationVerbessern2 && (bool)taskAnimationVerbessern2.gS_ && taskAnimationVerbessern2.gS_.portID != -1)
			{
				return true;
			}
			taskPolishing taskPolishing2 = GetTaskPolishing();
			if ((bool)taskPolishing2 && (bool)taskPolishing2.gS_ && taskPolishing2.gS_.portID != -1)
			{
				return true;
			}
			taskBugfixing taskBugfixing2 = GetTaskBugfixing();
			if ((bool)taskBugfixing2 && (bool)taskBugfixing2.gS_ && taskBugfixing2.gS_.portID != -1)
			{
				return true;
			}
			taskUnterstuetzen taskUnterstuetzen2 = GetTaskUnterstuetzen();
			if ((bool)taskUnterstuetzen2 && (bool)taskUnterstuetzen2.rS_)
			{
				return taskUnterstuetzen2.rS_.GameIsPort();
			}
		}
		return false;
	}

	public bool GameIsMMO()
	{
		if (typ != 1 && typ != 3 && typ != 5 && typ != 4 && typ != 10)
		{
			return false;
		}
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject)
		{
			taskGame taskGame2 = GetTaskGame();
			if ((bool)taskGame2 && (bool)taskGame2.gS_ && taskGame2.gS_.gameTyp == 1)
			{
				return true;
			}
			taskGameplayVerbessern taskGameplayVerbessern2 = GetTaskGameplayVerbessern();
			if ((bool)taskGameplayVerbessern2 && (bool)taskGameplayVerbessern2.gS_ && taskGameplayVerbessern2.gS_.gameTyp == 1)
			{
				return true;
			}
			taskSoundVerbessern taskSoundVerbessern2 = GetTaskSoundVerbessern();
			if ((bool)taskSoundVerbessern2 && (bool)taskSoundVerbessern2.gS_ && taskSoundVerbessern2.gS_.gameTyp == 1)
			{
				return true;
			}
			taskGameplayVerbessern taskGameplayVerbessern3 = GetTaskGameplayVerbessern();
			if ((bool)taskGameplayVerbessern3 && (bool)taskGameplayVerbessern3.gS_ && taskGameplayVerbessern3.gS_.gameTyp == 1)
			{
				return true;
			}
			taskAnimationVerbessern taskAnimationVerbessern2 = GetTaskAnimationVerbessern();
			if ((bool)taskAnimationVerbessern2 && (bool)taskAnimationVerbessern2.gS_ && taskAnimationVerbessern2.gS_.gameTyp == 1)
			{
				return true;
			}
			taskPolishing taskPolishing2 = GetTaskPolishing();
			if ((bool)taskPolishing2 && (bool)taskPolishing2.gS_ && taskPolishing2.gS_.gameTyp == 1)
			{
				return true;
			}
			taskBugfixing taskBugfixing2 = GetTaskBugfixing();
			if ((bool)taskBugfixing2 && (bool)taskBugfixing2.gS_ && taskBugfixing2.gS_.gameTyp == 1)
			{
				return true;
			}
			taskUnterstuetzen taskUnterstuetzen2 = GetTaskUnterstuetzen();
			if ((bool)taskUnterstuetzen2 && (bool)taskUnterstuetzen2.rS_)
			{
				return taskUnterstuetzen2.rS_.GameIsMMO();
			}
		}
		return false;
	}

	public gameScript DEV_GetGame()
	{
		if (typ != 1)
		{
			return null;
		}
		if (taskID == -1)
		{
			return null;
		}
		if ((bool)taskGameObject)
		{
			taskGame taskGame2 = GetTaskGame();
			if ((bool)taskGame2 && (bool)taskGame2.gS_)
			{
				return taskGame2.gS_;
			}
			taskUnterstuetzen taskUnterstuetzen2 = GetTaskUnterstuetzen();
			if ((bool)taskUnterstuetzen2 && (bool)taskUnterstuetzen2.rS_)
			{
				return taskUnterstuetzen2.rS_.DEV_GetGame();
			}
		}
		return null;
	}

	public int GetGameSize()
	{
		if (typ != 1 && typ != 3 && typ != 5 && typ != 4 && typ != 10)
		{
			return -1;
		}
		if (taskID == -1)
		{
			return -1;
		}
		if ((bool)taskGameObject)
		{
			taskGame taskGame2 = GetTaskGame();
			if ((bool)taskGame2 && (bool)taskGame2.gS_)
			{
				return taskGame2.gS_.gameSize;
			}
			taskGameplayVerbessern taskGameplayVerbessern2 = GetTaskGameplayVerbessern();
			if ((bool)taskGameplayVerbessern2 && (bool)taskGameplayVerbessern2.gS_)
			{
				return taskGameplayVerbessern2.gS_.gameSize;
			}
			taskSoundVerbessern taskSoundVerbessern2 = GetTaskSoundVerbessern();
			if ((bool)taskSoundVerbessern2 && (bool)taskSoundVerbessern2.gS_)
			{
				return taskSoundVerbessern2.gS_.gameSize;
			}
			taskGameplayVerbessern taskGameplayVerbessern3 = GetTaskGameplayVerbessern();
			if ((bool)taskGameplayVerbessern3 && (bool)taskGameplayVerbessern3.gS_)
			{
				return taskGameplayVerbessern3.gS_.gameSize;
			}
			taskAnimationVerbessern taskAnimationVerbessern2 = GetTaskAnimationVerbessern();
			if ((bool)taskAnimationVerbessern2 && (bool)taskAnimationVerbessern2.gS_)
			{
				return taskAnimationVerbessern2.gS_.gameSize;
			}
			taskPolishing taskPolishing2 = GetTaskPolishing();
			if ((bool)taskPolishing2 && (bool)taskPolishing2.gS_)
			{
				return taskPolishing2.gS_.gameSize;
			}
			taskBugfixing taskBugfixing2 = GetTaskBugfixing();
			if ((bool)taskBugfixing2 && (bool)taskBugfixing2.gS_)
			{
				return taskBugfixing2.gS_.gameSize;
			}
			taskUnterstuetzen taskUnterstuetzen2 = GetTaskUnterstuetzen();
			if ((bool)taskUnterstuetzen2 && (bool)taskUnterstuetzen2.rS_)
			{
				return taskUnterstuetzen2.rS_.GetGameSize();
			}
			taskGrafikVerbessern taskGrafikVerbessern2 = GetTaskGrafikVerbessern();
			if ((bool)taskGrafikVerbessern2 && (bool)taskGrafikVerbessern2.gS_)
			{
				return taskGrafikVerbessern2.gS_.gameSize;
			}
		}
		return -1;
	}

	public bool QA_GameHasNoBugs()
	{
		if (typ != 3)
		{
			return false;
		}
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject)
		{
			taskBugfixing taskBugfixing2 = GetTaskBugfixing();
			if ((bool)taskBugfixing2 && (bool)taskBugfixing2.gS_ && taskBugfixing2.gS_.points_bugs <= 0f)
			{
				if (taskBugfixing2.gS_.devPoints_Gesamt <= 0f)
				{
					guiMain_.uiObjects[279].GetComponent<Menu_ROOM_Polishing>().StartPolishingAutomatic(taskBugfixing2.gS_, taskBugfixing2.myID);
					Object.Destroy(taskBugfixing2.gameObject);
				}
				return true;
			}
		}
		return false;
	}

	public bool WaitForMinimumHype()
	{
		if (typ != 6)
		{
			return false;
		}
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject)
		{
			taskMarketing taskMarketing2 = GetTaskMarketing();
			if ((bool)taskMarketing2)
			{
				return taskMarketing2.WaitForMinimumHype();
			}
		}
		return false;
	}

	public bool IsDevAddon()
	{
		if (typ != 1)
		{
			return false;
		}
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject)
		{
			taskGame taskGame2 = GetTaskGame();
			if ((bool)taskGame2 && (bool)taskGame2.gS_ && (taskGame2.gS_.typ_addon || taskGame2.gS_.typ_addonStandalone || taskGame2.gS_.typ_contractGame || taskGame2.gS_.typ_mmoaddon))
			{
				return true;
			}
		}
		return false;
	}

	public bool KeineAutomatenBestellungen()
	{
		if (typ != 17)
		{
			return false;
		}
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject)
		{
			taskArcadeProduction taskArcadeProduction2 = GetTaskArcadeProduction();
			if ((bool)taskArcadeProduction2)
			{
				if (!taskArcadeProduction2.gS_)
				{
					return true;
				}
				if (taskArcadeProduction2.gS_.vorbestellungen <= 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsGameDevComplete()
	{
		if (typ != 1)
		{
			return false;
		}
		_ = taskID;
		_ = -1;
		return false;
	}

	public bool IsGameDevCompleteOrg()
	{
		if (typ != 1)
		{
			return false;
		}
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject)
		{
			taskGame taskGame2 = GetTaskGame();
			if ((bool)taskGame2 && (bool)taskGame2.gS_ && taskGame2.gS_.devPoints_Gesamt <= 0f)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsKonsoleDevCompleteOrg()
	{
		if (typ != 8)
		{
			return false;
		}
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject)
		{
			taskKonsole taskKonsole2 = GetTaskKonsole();
			if ((bool)taskKonsole2 && (bool)taskKonsole2.pS_ && taskKonsole2.pS_.devPoints <= 0f)
			{
				return true;
			}
		}
		return false;
	}

	public bool IstForschungWait()
	{
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject && (bool)GetTaskForschungWait())
		{
			return true;
		}
		return false;
	}

	public bool IstAutoForschung()
	{
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject && (bool)GetTaskAutoForschung())
		{
			return true;
		}
		return false;
	}

	public bool IstContractWorkWait()
	{
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject && (bool)GetTaskContractWait())
		{
			return true;
		}
		return false;
	}

	public bool IstTaskWait()
	{
		if (taskID == -1)
		{
			return false;
		}
		if ((bool)taskGameObject && (bool)GetTaskWait())
		{
			return true;
		}
		return false;
	}

	public long GetLagerplatz()
	{
		return lagerplatz;
	}

	public long GetFreeLagerplatz()
	{
		return lagerplatz - lagerplatzUsed;
	}

	public long GetServerplatz()
	{
		return serverplatz;
	}

	public long GetFreeServerplatz()
	{
		return serverplatz - serverplatzUsed;
	}

	public long SetAbos(long i)
	{
		if (serverDown)
		{
			return i;
		}
		long num = serverplatz - serverplatzUsed;
		if (num > 0)
		{
			if (num >= i)
			{
				serverplatzUsed += i;
				if (serverplatzUsed < 0)
				{
					serverplatzUsed = 0L;
				}
				return 0L;
			}
			serverplatzUsed = serverplatz;
			if (serverplatzUsed < 0)
			{
				serverplatzUsed = 0L;
			}
			return i - num;
		}
		return i;
	}

	private void UpdateLagerraumGFX()
	{
		if (typ != 9)
		{
			return;
		}
		lagerraumTimer += Time.deltaTime;
		if (lagerraumTimer < 1f)
		{
			return;
		}
		lagerraumTimer = 0f;
		List<GameObject> list = new List<GameObject>();
		for (int i = 0; i < listInventar.Count; i++)
		{
			if (!listInventar[i] || !listInventar[i].isLager)
			{
				continue;
			}
			if (!listInventar[i].lagerScript_)
			{
				listInventar[i].lagerScript_ = listInventar[i].gameObject.GetComponent<lagerScript>();
				continue;
			}
			for (int j = 0; j < listInventar[i].lagerScript_.goKartons.Length; j++)
			{
				list.Add(listInventar[i].lagerScript_.goKartons[j]);
			}
		}
		float num = GetLagerplatz();
		if (num > 0f)
		{
			num *= 0.01f;
			num = (float)lagerplatzUsed / num;
		}
		else
		{
			num = 0f;
		}
		float num2 = 100f / (float)list.Count;
		for (int k = 0; k < list.Count; k++)
		{
			if ((float)(k + 1) * num2 <= num)
			{
				if (!list[k].activeSelf)
				{
					list[k].SetActive(value: true);
				}
			}
			else if (list[k].activeSelf)
			{
				list[k].SetActive(value: false);
			}
		}
	}

	public void ServerAbschalten(bool shutdown)
	{
		serverDown = shutdown;
	}

	public bool UpdateInventar(bool buy)
	{
		bool result = false;
		updateCosts = 0L;
		if (typ != -1)
		{
			int oldTyp = 102;
			int newTyp = 107;
			if (mS_.year >= 2005 && RemoveOldInventar(oldTyp, newTyp, buy))
			{
				result = true;
			}
		}
		if (typ == 1)
		{
			int oldTyp2 = 1;
			int num = 50;
			int num2 = 51;
			int num3 = 52;
			int newTyp2 = 53;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp2, newTyp2, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num, newTyp2, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num2, newTyp2, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num3, newTyp2, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp2, num3, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num, num3, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num2, num3, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp2, num2, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num, num2, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp2, num, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 2)
		{
			int oldTyp3 = 6;
			int num4 = 56;
			int num5 = 66;
			int num6 = 67;
			int newTyp3 = 68;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp3, newTyp3, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num4, newTyp3, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num5, newTyp3, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num6, newTyp3, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp3, num6, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num4, num6, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num5, num6, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp3, num5, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num4, num5, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp3, num4, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 6)
		{
			int oldTyp4 = 48;
			int num7 = 57;
			int num8 = 58;
			int num9 = 59;
			int newTyp4 = 60;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp4, newTyp4, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num7, newTyp4, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num8, newTyp4, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num9, newTyp4, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp4, num9, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num7, num9, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num8, num9, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp4, num8, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num7, num8, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp4, num7, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 15)
		{
			int oldTyp5 = 45;
			int num10 = 125;
			int num11 = 126;
			int num12 = 127;
			int newTyp5 = 128;
			int oldTyp6 = 46;
			int newTyp6 = 154;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp5, newTyp5, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num10, newTyp5, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num11, newTyp5, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num12, newTyp5, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(oldTyp6, newTyp6, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp5, num12, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num10, num12, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num11, num12, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(oldTyp6, newTyp6, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp5, num11, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num10, num11, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp5, num10, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 9)
		{
			int oldTyp7 = 47;
			int num13 = 79;
			int newTyp7 = 80;
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp7, newTyp7, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num13, newTyp7, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp7, num13, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 13)
		{
			int oldTyp8 = 54;
			int num14 = 111;
			int num15 = 112;
			int num16 = 113;
			int newTyp8 = 114;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp8, newTyp8, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num14, newTyp8, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num15, newTyp8, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num16, newTyp8, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp8, num16, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num14, num16, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num15, num16, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp8, num15, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num14, num15, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp8, num14, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 7)
		{
			int oldTyp9 = 61;
			int num17 = 62;
			int num18 = 63;
			int num19 = 64;
			int newTyp9 = 65;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp9, newTyp9, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num17, newTyp9, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num18, newTyp9, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num19, newTyp9, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp9, num19, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num17, num19, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num18, num19, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp9, num18, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num17, num18, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp9, num17, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 3)
		{
			int oldTyp10 = 74;
			int num20 = 88;
			int num21 = 89;
			int num22 = 90;
			int newTyp10 = 91;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp10, newTyp10, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num20, newTyp10, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num21, newTyp10, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num22, newTyp10, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp10, num22, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num20, num22, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num21, num22, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp10, num21, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num20, num21, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp10, num20, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 4)
		{
			int oldTyp11 = 75;
			int num23 = 103;
			int num24 = 104;
			int num25 = 105;
			int newTyp11 = 106;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp11, newTyp11, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num23, newTyp11, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num24, newTyp11, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num25, newTyp11, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp11, num25, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num23, num25, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num24, num25, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp11, num24, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num23, num24, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp11, num23, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 5)
		{
			int oldTyp12 = 76;
			int num26 = 81;
			int num27 = 82;
			int num28 = 119;
			int newTyp12 = 120;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp12, newTyp12, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num26, newTyp12, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num27, newTyp12, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num28, newTyp12, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp12, num28, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num26, num28, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num27, num28, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp12, num27, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num26, num27, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp12, num26, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 10)
		{
			int oldTyp13 = 77;
			int num29 = 121;
			int newTyp13 = 122;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp13, newTyp13, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num29, newTyp13, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp13, num29, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 17)
		{
			int oldTyp14 = 144;
			int num30 = 145;
			int num31 = 146;
			int num32 = 147;
			int newTyp14 = 148;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp14, newTyp14, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num30, newTyp14, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num31, newTyp14, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num32, newTyp14, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp14, num32, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num30, num32, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num31, num32, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp14, num31, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num30, num31, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp14, num30, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 14)
		{
			int oldTyp15 = 36;
			int num33 = 115;
			int num34 = 116;
			int num35 = 117;
			int newTyp15 = 118;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp15, newTyp15, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num33, newTyp15, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num34, newTyp15, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num35, newTyp15, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp15, num35, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num33, num35, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num34, num35, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp15, num34, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num33, num34, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp15, num33, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 8)
		{
			int oldTyp16 = 149;
			int num36 = 150;
			int num37 = 151;
			int num38 = 152;
			int newTyp16 = 153;
			if (mS_.year >= 2015)
			{
				if (RemoveOldInventar(oldTyp16, newTyp16, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num36, newTyp16, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num37, newTyp16, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num38, newTyp16, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 2005)
			{
				if (RemoveOldInventar(oldTyp16, num38, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num36, num38, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num37, num38, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1995)
			{
				if (RemoveOldInventar(oldTyp16, num37, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(num36, num37, buy))
				{
					result = true;
				}
				return result;
			}
			if (mS_.year >= 1985)
			{
				if (RemoveOldInventar(oldTyp16, num36, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		if (typ == 11)
		{
			if (mS_.year >= 2010)
			{
				if (RemoveOldInventar(10, 186, buy))
				{
					result = true;
				}
				if (RemoveOldInventar(11, 187, buy))
				{
					result = true;
				}
				return result;
			}
			return result;
		}
		return result;
	}

	private bool RemoveOldInventar(int oldTyp, int newTyp, bool buy)
	{
		bool result = false;
		for (int i = 0; i < listInventar.Count; i++)
		{
			if (!listInventar[i])
			{
				continue;
			}
			objectScript objectScript2 = listInventar[i];
			if ((bool)objectScript2 && oldTyp == objectScript2.typ)
			{
				updateCosts += mapS_.prefabsInventar[newTyp].GetComponent<objectScript>().preis;
				updateCosts -= objectScript2.preis / 2;
				if (buy)
				{
					objectScript component = Object.Instantiate(mapS_.prefabsInventar[newTyp]).GetComponent<objectScript>();
					component.mS_ = mS_;
					component.sfx_ = mS_.sfx_;
					component.tS_ = tS_;
					component.mapS_ = mapS_;
					component.myID = Mathf.RoundToInt(Random.Range(1, 1999999999));
					component.typ = newTyp;
					component.InitObjectFromSavegame();
					mS_.objectRotation = objectScript2.transform.eulerAngles.y;
					component.PlatziereObject(objectScript2.transform.position, fromSavegame: true, updatePathfinding: false, autoInventar: false, partikel: false);
					mS_.Pay(component.preis, 1);
					guiMain_.MoneyPop(component.preis, component.transform.position, green: false);
					mS_.Multiplayer_SendObject(component.myID, newTyp, component.transform.position.x, component.transform.position.z, component.transform.eulerAngles.y);
					mS_.Earn(Mathf.RoundToInt(objectScript2.GetVerkaufspreis()), 0);
					mS_.Multiplayer_SendObjectDelete(objectScript2.myID);
					Object.Destroy(objectScript2.gameObject);
				}
				else
				{
					result = true;
				}
			}
		}
		return result;
	}

	private bool TaskCheckFailed(int taskID_)
	{
		if (taskID == -1)
		{
			return true;
		}
		if (!taskGameObject)
		{
			return true;
		}
		if (Mathf.RoundToInt(taskGameObject.transform.position.x) != taskID_)
		{
			return true;
		}
		return false;
	}

	public taskFanshop GetTaskFanshop()
	{
		if (TaskCheckFailed(270))
		{
			return null;
		}
		if ((bool)myTaskFanshop)
		{
			if (taskID == myTaskFanshop.myID)
			{
				return myTaskFanshop;
			}
			myTaskFanshop = taskGameObject.GetComponent<taskFanshop>();
			return myTaskFanshop;
		}
		myTaskFanshop = taskGameObject.GetComponent<taskFanshop>();
		return myTaskFanshop;
	}

	public taskWait GetTaskWait()
	{
		if (TaskCheckFailed(260))
		{
			return null;
		}
		if ((bool)myTaskWait)
		{
			if (taskID == myTaskWait.myID)
			{
				return myTaskWait;
			}
			myTaskWait = taskGameObject.GetComponent<taskWait>();
			return myTaskWait;
		}
		myTaskWait = taskGameObject.GetComponent<taskWait>();
		return myTaskWait;
	}

	public taskUnterstuetzen GetTaskUnterstuetzen()
	{
		if (TaskCheckFailed(250))
		{
			return null;
		}
		if ((bool)myTaskUnterstuetzen)
		{
			if (taskID == myTaskUnterstuetzen.myID)
			{
				return myTaskUnterstuetzen;
			}
			myTaskUnterstuetzen = taskGameObject.GetComponent<taskUnterstuetzen>();
			return myTaskUnterstuetzen;
		}
		myTaskUnterstuetzen = taskGameObject.GetComponent<taskUnterstuetzen>();
		return myTaskUnterstuetzen;
	}

	public taskPolishing GetTaskPolishing()
	{
		if (TaskCheckFailed(240))
		{
			return null;
		}
		if ((bool)myTaskPolishing)
		{
			if (taskID == myTaskPolishing.myID)
			{
				return myTaskPolishing;
			}
			myTaskPolishing = taskGameObject.GetComponent<taskPolishing>();
			return myTaskPolishing;
		}
		myTaskPolishing = taskGameObject.GetComponent<taskPolishing>();
		return myTaskPolishing;
	}

	public taskMarktforschung GetTaskMarktforschung()
	{
		if (TaskCheckFailed(230))
		{
			return null;
		}
		if ((bool)myTaskMarktforschung)
		{
			if (taskID == myTaskMarktforschung.myID)
			{
				return myTaskMarktforschung;
			}
			myTaskMarktforschung = taskGameObject.GetComponent<taskMarktforschung>();
			return myTaskMarktforschung;
		}
		myTaskMarktforschung = taskGameObject.GetComponent<taskMarktforschung>();
		return myTaskMarktforschung;
	}

	public taskAutoForschung GetTaskAutoForschung()
	{
		if (TaskCheckFailed(310))
		{
			return null;
		}
		if ((bool)myTaskAutoForschung)
		{
			if (taskID == myTaskAutoForschung.myID)
			{
				return myTaskAutoForschung;
			}
			myTaskAutoForschung = taskGameObject.GetComponent<taskAutoForschung>();
			return myTaskAutoForschung;
		}
		myTaskAutoForschung = taskGameObject.GetComponent<taskAutoForschung>();
		return myTaskAutoForschung;
	}

	public taskForschungWait GetTaskForschungWait()
	{
		if (TaskCheckFailed(280))
		{
			return null;
		}
		if ((bool)myTaskForschungWait)
		{
			if (taskID == myTaskForschungWait.myID)
			{
				return myTaskForschungWait;
			}
			myTaskForschungWait = taskGameObject.GetComponent<taskForschungWait>();
			return myTaskForschungWait;
		}
		myTaskForschungWait = taskGameObject.GetComponent<taskForschungWait>();
		return myTaskForschungWait;
	}

	public taskContractWait GetTaskContractWait()
	{
		if (TaskCheckFailed(220))
		{
			return null;
		}
		if ((bool)myTaskContractWait)
		{
			if (taskID == myTaskContractWait.myID)
			{
				return myTaskContractWait;
			}
			myTaskContractWait = taskGameObject.GetComponent<taskContractWait>();
			return myTaskContractWait;
		}
		myTaskContractWait = taskGameObject.GetComponent<taskContractWait>();
		return myTaskContractWait;
	}

	public taskContractWork GetTaskContractWork()
	{
		if (TaskCheckFailed(210))
		{
			return null;
		}
		if ((bool)myTaskContractWork)
		{
			if (taskID == myTaskContractWork.myID)
			{
				return myTaskContractWork;
			}
			myTaskContractWork = taskGameObject.GetComponent<taskContractWork>();
			return myTaskContractWork;
		}
		myTaskContractWork = taskGameObject.GetComponent<taskContractWork>();
		return myTaskContractWork;
	}

	public taskSupport GetTaskSupport()
	{
		if (TaskCheckFailed(200))
		{
			return null;
		}
		if ((bool)myTaskSupport)
		{
			if (taskID == myTaskSupport.myID)
			{
				return myTaskSupport;
			}
			myTaskSupport = taskGameObject.GetComponent<taskSupport>();
			return myTaskSupport;
		}
		myTaskSupport = taskGameObject.GetComponent<taskSupport>();
		return myTaskSupport;
	}

	public taskFankampagne GetTaskFankampagne()
	{
		if (TaskCheckFailed(190))
		{
			return null;
		}
		if ((bool)myTaskFankampagne)
		{
			if (taskID == myTaskFankampagne.myID)
			{
				return myTaskFankampagne;
			}
			myTaskFankampagne = taskGameObject.GetComponent<taskFankampagne>();
			return myTaskFankampagne;
		}
		myTaskFankampagne = taskGameObject.GetComponent<taskFankampagne>();
		return myTaskFankampagne;
	}

	public taskKonsole GetTaskKonsole()
	{
		if (TaskCheckFailed(180))
		{
			return null;
		}
		if ((bool)myTaskKonsole)
		{
			if (taskID == myTaskKonsole.myID)
			{
				return myTaskKonsole;
			}
			myTaskKonsole = taskGameObject.GetComponent<taskKonsole>();
			return myTaskKonsole;
		}
		myTaskKonsole = taskGameObject.GetComponent<taskKonsole>();
		return myTaskKonsole;
	}

	public taskKonsoleReduceCosts GetTaskKonsoleReduceCosts()
	{
		if (TaskCheckFailed(290))
		{
			return null;
		}
		if ((bool)myTaskKonsoleReduceCosts)
		{
			if (taskID == myTaskKonsoleReduceCosts.myID)
			{
				return myTaskKonsoleReduceCosts;
			}
			myTaskKonsoleReduceCosts = taskGameObject.GetComponent<taskKonsoleReduceCosts>();
			return myTaskKonsoleReduceCosts;
		}
		myTaskKonsoleReduceCosts = taskGameObject.GetComponent<taskKonsoleReduceCosts>();
		return myTaskKonsoleReduceCosts;
	}

	public taskKonsoleHaltbarkeit GetTaskKonsoleHaltbarkeit()
	{
		if (TaskCheckFailed(300))
		{
			return null;
		}
		if ((bool)myTaskKonsoleHaltbarkeit)
		{
			if (taskID == myTaskKonsoleHaltbarkeit.myID)
			{
				return myTaskKonsoleHaltbarkeit;
			}
			myTaskKonsoleHaltbarkeit = taskGameObject.GetComponent<taskKonsoleHaltbarkeit>();
			return myTaskKonsoleHaltbarkeit;
		}
		myTaskKonsoleHaltbarkeit = taskGameObject.GetComponent<taskKonsoleHaltbarkeit>();
		return myTaskKonsoleHaltbarkeit;
	}

	public taskArcadeProduction GetTaskArcadeProduction()
	{
		if (TaskCheckFailed(170))
		{
			return null;
		}
		if ((bool)myTaskArcadeProduction)
		{
			if (taskID == myTaskArcadeProduction.myID)
			{
				return myTaskArcadeProduction;
			}
			myTaskArcadeProduction = taskGameObject.GetComponent<taskArcadeProduction>();
			return myTaskArcadeProduction;
		}
		myTaskArcadeProduction = taskGameObject.GetComponent<taskArcadeProduction>();
		return myTaskArcadeProduction;
	}

	public taskProduction GetTaskProduction()
	{
		if (TaskCheckFailed(160))
		{
			return null;
		}
		if ((bool)myTaskProduction)
		{
			if (taskID == myTaskProduction.myID)
			{
				return myTaskProduction;
			}
			myTaskProduction = taskGameObject.GetComponent<taskProduction>();
			return myTaskProduction;
		}
		myTaskProduction = taskGameObject.GetComponent<taskProduction>();
		return myTaskProduction;
	}

	public taskAnimationVerbessern GetTaskAnimationVerbessern()
	{
		if (TaskCheckFailed(150))
		{
			return null;
		}
		if ((bool)myTaskAnimationVerbessern)
		{
			if (taskID == myTaskAnimationVerbessern.myID)
			{
				return myTaskAnimationVerbessern;
			}
			myTaskAnimationVerbessern = taskGameObject.GetComponent<taskAnimationVerbessern>();
			return myTaskAnimationVerbessern;
		}
		myTaskAnimationVerbessern = taskGameObject.GetComponent<taskAnimationVerbessern>();
		return myTaskAnimationVerbessern;
	}

	public taskSoundVerbessern GetTaskSoundVerbessern()
	{
		if (TaskCheckFailed(140))
		{
			return null;
		}
		if ((bool)myTaskSoundVerbessern)
		{
			if (taskID == myTaskSoundVerbessern.myID)
			{
				return myTaskSoundVerbessern;
			}
			myTaskSoundVerbessern = taskGameObject.GetComponent<taskSoundVerbessern>();
			return myTaskSoundVerbessern;
		}
		myTaskSoundVerbessern = taskGameObject.GetComponent<taskSoundVerbessern>();
		return myTaskSoundVerbessern;
	}

	public taskGrafikVerbessern GetTaskGrafikVerbessern()
	{
		if (TaskCheckFailed(130))
		{
			return null;
		}
		if ((bool)myTaskGrafikVerbessern)
		{
			if (taskID == myTaskGrafikVerbessern.myID)
			{
				return myTaskGrafikVerbessern;
			}
			myTaskGrafikVerbessern = taskGameObject.GetComponent<taskGrafikVerbessern>();
			return myTaskGrafikVerbessern;
		}
		myTaskGrafikVerbessern = taskGameObject.GetComponent<taskGrafikVerbessern>();
		return myTaskGrafikVerbessern;
	}

	public taskBugfixing GetTaskBugfixing()
	{
		if (TaskCheckFailed(120))
		{
			return null;
		}
		if ((bool)myTaskBugfixing)
		{
			if (taskID == myTaskBugfixing.myID)
			{
				return myTaskBugfixing;
			}
			myTaskBugfixing = taskGameObject.GetComponent<taskBugfixing>();
			return myTaskBugfixing;
		}
		myTaskBugfixing = taskGameObject.GetComponent<taskBugfixing>();
		return myTaskBugfixing;
	}

	public taskGameplayVerbessern GetTaskGameplayVerbessern()
	{
		if (TaskCheckFailed(110))
		{
			return null;
		}
		if ((bool)myTaskGameplayVerbessern)
		{
			if (taskID == myTaskGameplayVerbessern.myID)
			{
				return myTaskGameplayVerbessern;
			}
			myTaskGameplayVerbessern = taskGameObject.GetComponent<taskGameplayVerbessern>();
			return myTaskGameplayVerbessern;
		}
		myTaskGameplayVerbessern = taskGameObject.GetComponent<taskGameplayVerbessern>();
		return myTaskGameplayVerbessern;
	}

	public taskSpielbericht GetTaskSpielbericht()
	{
		if (TaskCheckFailed(100))
		{
			return null;
		}
		if ((bool)myTaskSpielbericht)
		{
			if (taskID == myTaskSpielbericht.myID)
			{
				return myTaskSpielbericht;
			}
			myTaskSpielbericht = taskGameObject.GetComponent<taskSpielbericht>();
			return myTaskSpielbericht;
		}
		myTaskSpielbericht = taskGameObject.GetComponent<taskSpielbericht>();
		return myTaskSpielbericht;
	}

	public taskTraining GetTaskTraining()
	{
		if (TaskCheckFailed(90))
		{
			return null;
		}
		if ((bool)myTaskTraining)
		{
			if (taskID == myTaskTraining.myID)
			{
				return myTaskTraining;
			}
			myTaskTraining = taskGameObject.GetComponent<taskTraining>();
			return myTaskTraining;
		}
		myTaskTraining = taskGameObject.GetComponent<taskTraining>();
		return myTaskTraining;
	}

	public taskMitarbeitersuche GetTaskMitarbeitersuche()
	{
		if (TaskCheckFailed(80))
		{
			return null;
		}
		if ((bool)myTaskMitarbeitersuche)
		{
			if (taskID == myTaskMitarbeitersuche.myID)
			{
				return myTaskMitarbeitersuche;
			}
			myTaskMitarbeitersuche = taskGameObject.GetComponent<taskMitarbeitersuche>();
			return myTaskMitarbeitersuche;
		}
		myTaskMitarbeitersuche = taskGameObject.GetComponent<taskMitarbeitersuche>();
		return myTaskMitarbeitersuche;
	}

	public taskMarketingSpezial GetTaskMarketingSpezial()
	{
		if (TaskCheckFailed(70))
		{
			return null;
		}
		if ((bool)myTaskMarketingSpezial)
		{
			if (taskID == myTaskMarketingSpezial.myID)
			{
				return myTaskMarketingSpezial;
			}
			myTaskMarketingSpezial = taskGameObject.GetComponent<taskMarketingSpezial>();
			return myTaskMarketingSpezial;
		}
		myTaskMarketingSpezial = taskGameObject.GetComponent<taskMarketingSpezial>();
		return myTaskMarketingSpezial;
	}

	public taskMarketing GetTaskMarketing()
	{
		if (TaskCheckFailed(60))
		{
			return null;
		}
		if ((bool)myTaskMarketing)
		{
			if (taskID == myTaskMarketing.myID)
			{
				return myTaskMarketing;
			}
			myTaskMarketing = taskGameObject.GetComponent<taskMarketing>();
			return myTaskMarketing;
		}
		myTaskMarketing = taskGameObject.GetComponent<taskMarketing>();
		return myTaskMarketing;
	}

	public taskF2PUpdate GetTaskF2PUpdate()
	{
		if (TaskCheckFailed(50))
		{
			return null;
		}
		if ((bool)myTaskF2PUpdate)
		{
			if (taskID == myTaskF2PUpdate.myID)
			{
				return myTaskF2PUpdate;
			}
			myTaskF2PUpdate = taskGameObject.GetComponent<taskF2PUpdate>();
			return myTaskF2PUpdate;
		}
		myTaskF2PUpdate = taskGameObject.GetComponent<taskF2PUpdate>();
		return myTaskF2PUpdate;
	}

	public taskGame GetTaskGame()
	{
		if (TaskCheckFailed(40))
		{
			return null;
		}
		if ((bool)myTaskGame)
		{
			if (taskID == myTaskGame.myID)
			{
				return myTaskGame;
			}
			myTaskGame = taskGameObject.GetComponent<taskGame>();
			return myTaskGame;
		}
		myTaskGame = taskGameObject.GetComponent<taskGame>();
		return myTaskGame;
	}

	public taskForschung GetTaskForschung()
	{
		if (TaskCheckFailed(10))
		{
			return null;
		}
		if ((bool)myTaskForschung)
		{
			if (taskID == myTaskForschung.myID)
			{
				return myTaskForschung;
			}
			myTaskForschung = taskGameObject.GetComponent<taskForschung>();
			return myTaskForschung;
		}
		myTaskForschung = taskGameObject.GetComponent<taskForschung>();
		return myTaskForschung;
	}

	public taskEngine GetTaskEngine()
	{
		if (TaskCheckFailed(20))
		{
			return null;
		}
		if ((bool)myTaskEngine)
		{
			if (taskID == myTaskEngine.myID)
			{
				return myTaskEngine;
			}
			myTaskEngine = taskGameObject.GetComponent<taskEngine>();
			return myTaskEngine;
		}
		myTaskEngine = taskGameObject.GetComponent<taskEngine>();
		return myTaskEngine;
	}

	public taskUpdate GetTaskUpdate()
	{
		if (TaskCheckFailed(30))
		{
			return null;
		}
		if ((bool)myTaskUpdate)
		{
			if (taskID == myTaskUpdate.myID)
			{
				return myTaskUpdate;
			}
			myTaskUpdate = taskGameObject.GetComponent<taskUpdate>();
			return myTaskUpdate;
		}
		myTaskUpdate = taskGameObject.GetComponent<taskUpdate>();
		return myTaskUpdate;
	}
}
