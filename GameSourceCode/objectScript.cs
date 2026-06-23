using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class objectScript : MonoBehaviour
{
	public mainScript mS_;

	public Camera myCamera;

	public GUI_Main guiMain;

	public mainCameraScript mCamS_;

	public sfxScript sfx_;

	public mapScript mapS_;

	public textScript tS_;

	public pickObjectScript pickObject_;

	public lagerScript lagerScript_;

	public GameObject[] GFX;

	public GameObject waypoint;

	public GameObject pointMale;

	public GameObject pointFemale;

	public Animation gfxAnimation;

	public GameObject gfxShow;

	public GameObject gfxHide;

	public GameObject[] removeObjects;

	public GameObject particle;

	public GameObject footprint;

	public GameObject gfxObjectEmpty;

	public Vector3 vWaypoint = new Vector3(0f, 0f, 0f);

	public Vector3 vPointMale = new Vector3(0f, 0f, 0f);

	public Vector3 vPointFemale = new Vector3(0f, 0f, 0f);

	public Vector3 vEulerMale = new Vector3(0f, 0f, 0f);

	public Vector3 vEulerFemale = new Vector3(0f, 0f, 0f);

	public bool existWaypoint;

	public bool exitPointMale;

	public bool exitPointFemale;

	public Rigidbody rigidbody;

	public int myID;

	public int typ;

	public int typGhost = -1;

	public int preis;

	public int monatsKosten;

	public float waerme;

	public float kaelte;

	public float ausstattung;

	public float motivationRegen;

	public bool wallObject;

	public bool dontBuildOnWindows;

	public int unlockYear = -1;

	public int aufladungenMax;

	public int aufladungenAkt;

	public float maschieneTimer;

	public bool wirdNichtbesetzt;

	public bool checkWaitDistance;

	public bool isArbeitsplatz;

	public bool isHeizung;

	public bool isServer;

	public bool isLager;

	public bool isMaschine;

	public bool isMuelleimer;

	public bool isPlant;

	public bool isWC;

	public bool isSink;

	public bool isHandtrockner;

	public bool isMedizinSchrank;

	public bool isFreezer;

	public bool isTV;

	public bool isRadio;

	public bool isArcade;

	public bool isDart;

	public bool isMinigolf;

	public bool isLaufband;

	public bool isPiano;

	public bool isSeat;

	public bool isSeatAufenthalt;

	public bool isRobotClean;

	public bool isGhost;

	public bool isGhostMuelleimer;

	public bool isGhostDrink;

	public bool isGhostWC;

	public bool isGhostSink;

	public bool isGhostPlant;

	public bool isGhostPause1;

	public bool isGhostPause2;

	public bool isGhostPause3;

	public bool isGhostPause4;

	public bool canDrink;

	public bool canEat;

	public int qualitaet;

	public int lagerplatz;

	public int serverplatz;

	public int myRoomID = 1;

	public int besetztCharID = -1;

	public bool inUse;

	public bool gekauft;

	public bool picked;

	public bool colided;

	private bool outline;

	public bool canSet_Floor;

	public bool canSet_Development;

	public bool canSet_Research;

	public bool canSet_QA;

	public bool canSet_Grafikstudio;

	public bool canSet_Soundstudio;

	public bool canSet_Marketing;

	public bool canSet_Support;

	public bool canSet_Hardware;

	public bool canSet_Lager;

	public bool canSet_Motion;

	public bool canSet_WC;

	public bool canSet_Aufenthalt;

	public bool canSet_Training;

	public bool canSet_Produktion;

	public bool canSet_Server;

	public bool canSet_Werkstatt;

	public LayerMask layerMask;

	public bool multiplayerObject;

	public int tilesX = 1;

	public int tilesY = 1;

	public bool ignoreDestroy;

	private int lastTileX;

	private int lastTileY;

	public bool autoInventarItem;

	public int collisionAmount;

	private bool checkCollideWithDelay;

	public void GetWaypoints()
	{
		if ((bool)waypoint)
		{
			existWaypoint = true;
			vWaypoint = waypoint.transform.position;
			Object.Destroy(waypoint);
			waypoint = null;
		}
		if ((bool)pointFemale)
		{
			exitPointFemale = true;
			vPointFemale = pointFemale.transform.position;
			vEulerFemale = pointFemale.transform.eulerAngles;
			Object.Destroy(pointFemale);
			pointFemale = null;
		}
		if ((bool)pointMale)
		{
			exitPointMale = true;
			vPointMale = pointMale.transform.position;
			vEulerMale = pointMale.transform.eulerAngles;
			Object.Destroy(pointMale);
			pointMale = null;
		}
	}

	private void Start()
	{
		FindScripts();
		RemoveObjects();
		if ((bool)mS_)
		{
			mS_.findObjects = true;
		}
	}

	private void OnDestroy()
	{
		if (base.gameObject.CompareTag("Untagged") || multiplayerObject)
		{
			return;
		}
		if (!mS_)
		{
			if ((bool)GameObject.FindWithTag("Main"))
			{
				mS_ = GameObject.FindWithTag("Main").GetComponent<mainScript>();
			}
			if (!mS_)
			{
				return;
			}
		}
		if (gekauft && !picked)
		{
			switch (typ)
			{
			case 17:
				mS_.goldeneSchallplatten++;
				break;
			case 129:
				mS_.platinSchallplatten++;
				break;
			case 130:
				mS_.diamantSchallplatten++;
				break;
			case 132:
				mS_.award_GOTY++;
				break;
			case 133:
				mS_.award_Studio++;
				break;
			case 134:
				mS_.award_Sound++;
				break;
			case 135:
				mS_.award_Grafik++;
				break;
			case 142:
				mS_.award_Trendsetter++;
				break;
			case 143:
				mS_.award_Publisher++;
				break;
			}
		}
		if (!picked)
		{
			mS_.Multiplayer_SendObjectDelete(myID);
		}
		if ((bool)mS_)
		{
			mS_.findObjects = true;
		}
	}

	public void InitNewObject(int typ_)
	{
		FindScripts();
		InitGFX();
		base.transform.eulerAngles = new Vector3(0f, mS_.objectRotation, 0f);
		myID = Mathf.RoundToInt(Random.Range(1, 1999999999));
		base.gameObject.name = "O_" + myID;
		typ = typ_;
		gekauft = false;
		PickUp();
	}

	public void InitObjectFromSavegame()
	{
		FindScripts();
		InitGFX();
		base.gameObject.name = "O_" + myID;
		gekauft = true;
	}

	public void InitGhostObject(int typ_)
	{
		FindScripts();
		myID = Mathf.RoundToInt(Random.Range(1, 1999999999));
		base.gameObject.name = "O_" + myID;
		typ = -1;
		typGhost = typ_;
		gekauft = true;
	}

	private void FindScripts()
	{
		if (!mS_)
		{
			mS_ = GameObject.FindWithTag("Main").GetComponent<mainScript>();
		}
		if (!mapS_)
		{
			mapS_ = mS_.mapScript_;
		}
		if (!guiMain)
		{
			guiMain = mS_.guiMain_;
		}
		if (!myCamera)
		{
			myCamera = mS_.myCamera;
		}
		if (!mCamS_)
		{
			mCamS_ = mS_.mcS_;
		}
		if (!sfx_)
		{
			sfx_ = mS_.sfx_;
		}
		if (!tS_)
		{
			tS_ = mS_.tS_;
		}
		if (!pickObject_)
		{
			pickObject_ = mS_.pickObject_;
		}
		if (typGhost == -1 && !rigidbody)
		{
			rigidbody = GetComponent<Rigidbody>();
		}
	}

	private void RemoveObjects()
	{
		for (int i = 0; i < removeObjects.Length; i++)
		{
			if ((bool)removeObjects[i])
			{
				Object.Destroy(removeObjects[i]);
				removeObjects[i] = null;
			}
		}
	}

	private void InitGFX()
	{
		if (GFX.Length == 0)
		{
			return;
		}
		int num = Random.Range(0, GFX.Length);
		for (int i = 0; i < GFX.Length; i++)
		{
			if (i != num)
			{
				Object.Destroy(GFX[i]);
			}
		}
		GFX[num].SetActive(value: true);
	}

	public void UpdateMe()
	{
		MouseMovement();
		UpdateUnkorrekterRoom();
	}

	public void WakeUpObject()
	{
		if ((bool)rigidbody && rigidbody.IsSleeping())
		{
			rigidbody.WakeUp();
		}
	}

	public void UpdateUnkorrekterRoom()
	{
		if (!gekauft)
		{
			return;
		}
		if (!guiMain)
		{
			FindScripts();
		}
		else
		{
			if (guiMain.menuOpen || isGhost)
			{
				return;
			}
			int num = Mathf.RoundToInt(base.transform.position.x);
			int num2 = Mathf.RoundToInt(base.transform.position.z);
			if (!mapS_.IsInMapLimit(num, num2))
			{
				return;
			}
			if (mapS_.mapRoomID[num, num2] != myRoomID)
			{
				Debug.Log("UpdateUnkorrekterRoom(): " + base.gameObject.name);
				mCamS_.SetOutlineColor(1, 0.3f, 1);
				colided = true;
				pickObject_.Click(base.gameObject);
			}
			else if (wallObject && mapS_.IsInMapLimit(num + 1, num2) && mapS_.IsInMapLimit(num - 1, num2) && mapS_.IsInMapLimit(num, num2 + 1) && mapS_.IsInMapLimit(num, num2 - 1))
			{
				int num3 = Mathf.RoundToInt(base.transform.eulerAngles.y);
				if ((num3 == 270 && mapS_.mapRoomID[num, num2] == mapS_.mapRoomID[num + 1, num2]) || (num3 == 90 && mapS_.mapRoomID[num, num2] == mapS_.mapRoomID[num - 1, num2]) || (num3 == 180 && mapS_.mapRoomID[num, num2] == mapS_.mapRoomID[num, num2 + 1]) || (num3 == 0 && mapS_.mapRoomID[num, num2] == mapS_.mapRoomID[num, num2 - 1]))
				{
					Debug.Log("UpdateUnkorrekterRoom() -> Wandobjekt -> Objekt steht in Luft:" + base.gameObject.name);
					mCamS_.SetOutlineColor(1, 0.3f, 1);
					colided = true;
					pickObject_.Click(base.gameObject);
				}
			}
		}
	}

	public void PickUp()
	{
		collisionAmount = 0;
		picked = true;
		mS_.pickedObject = base.gameObject;
		mCamS_.SetOutlineColor(2, 0.1f, 4);
		SetLayer(11, base.gameObject.transform.GetChild(0));
	}

	private void SetLayer(int newLayer, Transform trans)
	{
		trans.gameObject.layer = newLayer;
		foreach (Transform tran in trans)
		{
			if ((newLayer == 11 && tran.tag != "NoOutline") || newLayer != 11)
			{
				tran.gameObject.layer = newLayer;
				if (tran.childCount > 0)
				{
					SetLayer(newLayer, tran.transform);
				}
			}
		}
	}

	public void MouseMovement()
	{
		if (!picked || checkCollideWithDelay)
		{
			return;
		}
		float y = 0.05f;
		if ((waerme > 0f || kaelte > 0f || ausstattung > 0f) && (bool)guiMain && guiMain.IsFilterAktiv() && (lastTileX != Mathf.RoundToInt(base.transform.position.x) || (float)lastTileY != Mathf.Round(base.transform.position.z)))
		{
			lastTileX = Mathf.RoundToInt(base.transform.position.x);
			lastTileY = Mathf.RoundToInt(base.transform.position.z);
			mapS_.UpdateMapFilter(force: true);
			mS_.DrawFilter(guiMain.filterToggles, force: true);
		}
		bool mouseButtonUp = Input.GetMouseButtonUp(0);
		if (guiMain.IsMouseOverGUI())
		{
			base.transform.position = new Vector3(-50f, -50f, -50f);
			return;
		}
		if (Input.GetKeyUp(KeyCode.Escape) && pickObject_.oldPosition.x >= 0f)
		{
			Vector3 oldPosition = pickObject_.oldPosition;
			pickObject_.oldPosition = new Vector3(-1000f, 0f, 0f);
			if ((typ != 17 || mS_.goldeneSchallplatten > 0) && (typ != 129 || mS_.platinSchallplatten > 0) && (typ != 130 || mS_.diamantSchallplatten > 0) && (typ != 132 || mS_.award_GOTY > 0) && (typ != 133 || mS_.award_Studio > 0) && (typ != 134 || mS_.award_Sound > 0) && (typ != 135 || mS_.award_Grafik > 0) && (typ != 142 || mS_.award_Trendsetter > 0) && (typ != 143 || mS_.award_Publisher > 0))
			{
				mS_.objectRotation = pickObject_.oldRotation.y;
				StartCoroutine(CheckCollideWithDelay(oldPosition));
				mapS_.UpdateMapFilter(force: true);
			}
			return;
		}
		float num = 0f;
		num = Mathf.LerpAngle(base.transform.eulerAngles.y, mS_.objectRotation, 15f * Time.deltaTime);
		base.transform.eulerAngles = new Vector3(0f, num, 0f);
		if (Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out var hitInfo, 200f, layerMask))
		{
			float x;
			float z;
			if (mS_.snapObject)
			{
				x = SnapTo(hitInfo.point.x, 0.5f);
				z = SnapTo(hitInfo.point.z, 0.5f);
			}
			else
			{
				x = SnapTo(hitInfo.point.x, 0.01f);
				z = SnapTo(hitInfo.point.z, 0.01f);
			}
			Vector3 vector = new Vector3(x, y, z);
			base.transform.position = Vector3.Lerp(base.transform.position, vector, 15f * Time.deltaTime);
			if (base.transform.position.y > 100f)
			{
				base.transform.position = vector;
			}
			if (mS_.snapRotation)
			{
				mS_.objectRotation = Mathf.RoundToInt(mS_.objectRotation / 90f) * 90;
				if (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.F))
				{
					mS_.objectRotation += 90f;
					sfx_.PlaySound(6, force: true);
				}
				if (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.R))
				{
					mS_.objectRotation -= 90f;
					sfx_.PlaySound(6, force: true);
				}
				if (Input.GetMouseButton(1))
				{
					if (Input.mouseScrollDelta.y > 0f)
					{
						mS_.objectRotation -= 90f;
					}
					if (Input.mouseScrollDelta.y < 0f)
					{
						mS_.objectRotation += 90f;
					}
				}
			}
			else
			{
				if (Input.GetKey(KeyCode.Period) || Input.GetKey(KeyCode.F))
				{
					mS_.objectRotation += 130f * Time.deltaTime;
					mS_.objectRotation = mS_.Round(mS_.objectRotation, 2);
				}
				if (Input.GetKey(KeyCode.Comma) || Input.GetKey(KeyCode.R))
				{
					mS_.objectRotation -= 130f * Time.deltaTime;
					mS_.objectRotation = mS_.Round(mS_.objectRotation, 2);
				}
				if (Input.GetMouseButton(1) && Input.mouseScrollDelta.y != 0f)
				{
					mS_.objectRotation -= Input.mouseScrollDelta.y * 10f;
					mS_.objectRotation = mS_.Round(mS_.objectRotation, 2);
				}
			}
			int num2 = Mathf.RoundToInt(hitInfo.transform.position.x);
			int num3 = Mathf.RoundToInt(hitInfo.transform.position.z);
			if (wallObject)
			{
				mS_.objectRotation = Mathf.RoundToInt(hitInfo.transform.eulerAngles.y - 90f);
				if (mapS_.mapRoomID[num2, num3] != mapS_.mapRoomID[num2 + 1, num3])
				{
					base.transform.position = new Vector3(hitInfo.transform.position.x, y, base.transform.position.z);
				}
				if (mapS_.mapRoomID[num2, num3] != mapS_.mapRoomID[num2 - 1, num3])
				{
					base.transform.position = new Vector3(hitInfo.transform.position.x, y, base.transform.position.z);
				}
				if (mapS_.mapRoomID[num2, num3] != mapS_.mapRoomID[num2, num3 + 1])
				{
					base.transform.position = new Vector3(base.transform.position.x, y, hitInfo.transform.position.z);
				}
				if (mapS_.mapRoomID[num2, num3] != mapS_.mapRoomID[num2, num3 - 1])
				{
					base.transform.position = new Vector3(base.transform.position.x, y, hitInfo.transform.position.z);
				}
				vector = base.transform.position;
			}
			bool flag = false;
			if (!colided)
			{
				if (!mS_.IsMyBuilding(mapS_.mapBuilding[num2, num3]))
				{
					flag = true;
				}
				if (!IsCorrectRoomForThisObject(vector))
				{
					flag = true;
				}
				if (wallObject && mapS_.mapRoomID[num2, num3] == mapS_.mapRoomID[num2 + 1, num3] && mapS_.mapRoomID[num2, num3] == mapS_.mapRoomID[num2 - 1, num3] && mapS_.mapRoomID[num2, num3] == mapS_.mapRoomID[num2, num3 + 1] && mapS_.mapRoomID[num2, num3] == mapS_.mapRoomID[num2, num3 - 1])
				{
					flag = true;
				}
				if (typ == 17 && mS_.goldeneSchallplatten <= 0)
				{
					flag = true;
				}
				if (typ == 129 && mS_.platinSchallplatten <= 0)
				{
					flag = true;
				}
				if (typ == 130 && mS_.diamantSchallplatten <= 0)
				{
					flag = true;
				}
				if (typ == 132 && mS_.award_GOTY <= 0)
				{
					flag = true;
				}
				if (typ == 133 && mS_.award_Studio <= 0)
				{
					flag = true;
				}
				if (typ == 134 && mS_.award_Sound <= 0)
				{
					flag = true;
				}
				if (typ == 135 && mS_.award_Grafik <= 0)
				{
					flag = true;
				}
				if (typ == 142 && mS_.award_Trendsetter <= 0)
				{
					flag = true;
				}
				if (typ == 143 && mS_.award_Publisher <= 0)
				{
					flag = true;
				}
				if (flag)
				{
					mCamS_.SetOutlineColor(1, 0.3f, 1);
					if (mouseButtonUp)
					{
						sfx_.PlaySound(2, force: true);
					}
				}
				else
				{
					mCamS_.SetOutlineColor(2, 0.1f, 4);
				}
			}
			if (!flag && mouseButtonUp && !colided && (typ != 17 || mS_.goldeneSchallplatten > 0) && (typ != 129 || mS_.platinSchallplatten > 0) && (typ != 130 || mS_.diamantSchallplatten > 0) && (typ != 132 || mS_.award_GOTY > 0) && (typ != 133 || mS_.award_Studio > 0) && (typ != 134 || mS_.award_Sound > 0) && (typ != 135 || mS_.award_Grafik > 0) && (typ != 142 || mS_.award_Trendsetter > 0) && (typ != 143 || mS_.award_Publisher > 0))
			{
				StartCoroutine(CheckCollideWithDelay(vector));
			}
		}
		else
		{
			base.transform.position = new Vector3(0f, 9999f, 0f);
		}
	}

	public static float SnapTo(float a, float snap)
	{
		return Mathf.Round(a / snap) * snap;
	}

	private void OnCollisionEnter(Collision collision)
	{
		mCamS_.SetOutlineColor(1, 0.3f, 1);
		colided = true;
		if (picked)
		{
			collisionAmount++;
		}
		if (autoInventarItem && (bool)mS_.autoInventar_ && mS_.autoInventar_.autoBuildEnabled)
		{
			if (gekauft)
			{
				Debug.Log("Preis zurück: " + preis);
				gekauft = false;
				mS_.Earn(preis, 0);
			}
			Object.Destroy(base.gameObject);
		}
	}

	private void OnCollisionStay(Collision collision)
	{
		mCamS_.SetOutlineColor(1, 0.3f, 1);
		colided = true;
		if (picked && collision.transform != base.transform)
		{
			if (!mCamS_.additionalCamera[0].activeSelf)
			{
				mCamS_.additionalCamera[0].SetActive(value: true);
			}
			if (collision.gameObject.CompareTag("HideWall") || collision.gameObject.CompareTag("Object"))
			{
				if (collision.gameObject.transform.childCount > 0)
				{
					if (collision.gameObject.CompareTag("HideWall"))
					{
						if ((bool)mS_.guiMain_ && !mS_.guiMain_.uiObjects[241].GetComponent<Toggle>().isOn)
						{
							SetLayer(15, collision.gameObject.transform.GetChild(0));
							mS_.AddColliderLayer(collision.gameObject.transform.GetChild(0));
						}
					}
					else
					{
						SetLayer(15, collision.gameObject.transform.GetChild(0));
						mS_.AddColliderLayer(collision.gameObject.transform.GetChild(0));
					}
				}
				else
				{
					SetLayer(15, collision.gameObject.transform.parent.GetChild(0));
					mS_.AddColliderLayer(collision.gameObject.transform.parent.GetChild(0));
				}
			}
		}
		if (!guiMain.menuOpen && gekauft)
		{
			Debug.Log("COL-STAY: " + base.gameObject.name + " / " + Random.Range(0, 10000));
			StartCoroutine(iPickObjectAfterCollision());
		}
	}

	private IEnumerator iPickObjectAfterCollision()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (!guiMain.menuOpen && gekauft)
		{
			pickObject_.Click(base.gameObject);
			Debug.Log("PickObject -> Kollision");
		}
	}

	private void OnCollisionExit(Collision collision)
	{
		if (picked)
		{
			collisionAmount--;
		}
		if (collisionAmount <= 0)
		{
			colided = false;
		}
		if (!picked || (!collision.gameObject.CompareTag("HideWall") && !collision.gameObject.CompareTag("Object")))
		{
			return;
		}
		if (collisionAmount <= 0)
		{
			mCamS_.SetOutlineColor(2, 0.1f, 4);
		}
		if (collision.transform != base.transform)
		{
			if (mCamS_.additionalCamera[0].activeSelf)
			{
				mCamS_.additionalCamera[0].SetActive(value: false);
			}
			if (collision.gameObject.transform.childCount > 0)
			{
				SetLayer(0, collision.gameObject.transform.GetChild(0));
			}
			else
			{
				SetLayer(0, collision.gameObject.transform.parent.GetChild(0));
			}
		}
	}

	public IEnumerator CheckCollideWithDelay(Vector3 pos)
	{
		Debug.Log("CheckCollideWithDelay()");
		checkCollideWithDelay = true;
		base.transform.position = new Vector3(pos.x, pos.y, pos.z);
		base.transform.eulerAngles = new Vector3(0f, mS_.objectRotation, 0f);
		yield return new WaitForFixedUpdate();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (guiMain.uiObjects[20].activeSelf)
		{
			Debug.Log("PlatziereObject() Mit Item-Kaufen-Menü");
			PlatziereObject(pos, fromSavegame: false, updatePathfinding: false, autoInventar: false, partikel: true);
		}
		else
		{
			PlatziereObject(pos, fromSavegame: false, updatePathfinding: true, autoInventar: false, partikel: true);
		}
		checkCollideWithDelay = false;
	}

	private bool IsCorrectRoomForThisObject(Vector3 pos)
	{
		int num = Mathf.RoundToInt(pos.x);
		int num2 = Mathf.RoundToInt(pos.z);
		if (!mapS_.IsInMapLimit(num, num2))
		{
			return false;
		}
		int num3 = mapS_.mapRoomID[num, num2];
		if (num3 != 1 && num3 != mapScript.ID_FLOOROUTSIDE)
		{
			roomScript roomScript2 = mapS_.mapRoomScript[num, num2];
			if (!roomScript2)
			{
				return false;
			}
			if (roomScript2.typ == 1 && !canSet_Development)
			{
				return false;
			}
			if (roomScript2.typ == 2 && !canSet_Research)
			{
				return false;
			}
			if (roomScript2.typ == 3 && !canSet_QA)
			{
				return false;
			}
			if (roomScript2.typ == 4 && !canSet_Grafikstudio)
			{
				return false;
			}
			if (roomScript2.typ == 5 && !canSet_Soundstudio)
			{
				return false;
			}
			if (roomScript2.typ == 6 && !canSet_Marketing)
			{
				return false;
			}
			if (roomScript2.typ == 7 && !canSet_Support)
			{
				return false;
			}
			if (roomScript2.typ == 8 && !canSet_Hardware)
			{
				return false;
			}
			if (roomScript2.typ == 9 && !canSet_Lager)
			{
				return false;
			}
			if (roomScript2.typ == 10 && !canSet_Motion)
			{
				return false;
			}
			if (roomScript2.typ == 11 && !canSet_WC)
			{
				return false;
			}
			if (roomScript2.typ == 12 && !canSet_Aufenthalt)
			{
				return false;
			}
			if (roomScript2.typ == 13 && !canSet_Training)
			{
				return false;
			}
			if (roomScript2.typ == 14 && !canSet_Produktion)
			{
				return false;
			}
			if (roomScript2.typ == 15 && !canSet_Server)
			{
				return false;
			}
			if (roomScript2.typ == 17 && !canSet_Werkstatt)
			{
				return false;
			}
		}
		else if (!canSet_Floor)
		{
			return false;
		}
		return true;
	}

	public void PlatziereObject(Vector3 pos, bool fromSavegame, bool updatePathfinding, bool autoInventar, bool partikel)
	{
		if (!fromSavegame && !autoInventar)
		{
			if (!IsCorrectRoomForThisObject(pos))
			{
				sfx_.PlaySound(2, force: true);
				Debug.Log("IsCorrectRoomForThisObject()");
				return;
			}
			if (colided)
			{
				sfx_.PlaySound(2, force: true);
				Debug.Log("PlatziereObject(): Colided");
				return;
			}
			if (base.transform.position.y > 100f)
			{
				sfx_.PlaySound(2, force: true);
				Debug.Log("PlatziereObject(): transform.position.y > 100.0f");
				return;
			}
		}
		base.transform.position = new Vector3(pos.x, 0f, pos.z);
		base.transform.eulerAngles = new Vector3(0f, mS_.objectRotation, 0f);
		base.transform.Translate(Vector3.zero);
		GetWaypoints();
		if ((bool)footprint)
		{
			Object.Destroy(footprint);
			footprint = null;
		}
		mS_.pickedObject = null;
		picked = false;
		SetLayer(0, base.gameObject.transform.GetChild(0));
		mCamS_.DisablePostLineRenderer();
		if (!fromSavegame && !autoInventar)
		{
			Animation component = GetComponent<Animation>();
			if ((bool)component)
			{
				component.Play();
			}
		}
		int num = Mathf.RoundToInt(pos.x);
		int num2 = Mathf.RoundToInt(pos.z);
		if (!isGhost)
		{
			if (!mapS_.IsInMapLimit(num, num2))
			{
				num = 0;
				num2 = 0;
				base.transform.position = new Vector3(num, 0f, num2);
			}
			if ((bool)mapS_.mapRoomScript[num, num2])
			{
				mapS_.mapRoomScript[num, num2].listInventar.Add(base.gameObject.GetComponent<objectScript>());
				myRoomID = mapS_.mapRoomScript[num, num2].myID;
			}
		}
		if (!fromSavegame)
		{
			bool flag = false;
			switch (typ)
			{
			case 17:
				mS_.goldeneSchallplatten--;
				flag = true;
				if (mS_.goldeneSchallplatten < 0)
				{
					Object.Destroy(base.gameObject);
					mS_.goldeneSchallplatten = 0;
					return;
				}
				break;
			case 129:
				mS_.platinSchallplatten--;
				if (mS_.platinSchallplatten < 0)
				{
					Object.Destroy(base.gameObject);
					mS_.platinSchallplatten = 0;
					return;
				}
				flag = true;
				break;
			case 130:
				mS_.diamantSchallplatten--;
				if (mS_.diamantSchallplatten < 0)
				{
					Object.Destroy(base.gameObject);
					mS_.diamantSchallplatten = 0;
					return;
				}
				flag = true;
				break;
			case 132:
				mS_.award_GOTY--;
				if (mS_.award_GOTY < 0)
				{
					Object.Destroy(base.gameObject);
					mS_.award_GOTY = 0;
					return;
				}
				flag = true;
				break;
			case 133:
				mS_.award_Studio--;
				if (mS_.award_Studio < 0)
				{
					Object.Destroy(base.gameObject);
					mS_.award_Studio = 0;
					return;
				}
				flag = true;
				break;
			case 134:
				mS_.award_Sound--;
				if (mS_.award_Sound < 0)
				{
					Object.Destroy(base.gameObject);
					mS_.award_Sound = 0;
					return;
				}
				flag = true;
				break;
			case 135:
				mS_.award_Grafik--;
				if (mS_.award_Grafik < 0)
				{
					Object.Destroy(base.gameObject);
					mS_.award_Grafik = 0;
					return;
				}
				flag = true;
				break;
			case 142:
				mS_.award_Trendsetter--;
				if (mS_.award_Trendsetter < 0)
				{
					Object.Destroy(base.gameObject);
					mS_.award_Trendsetter = 0;
					return;
				}
				flag = true;
				break;
			case 143:
				mS_.award_Publisher--;
				if (mS_.award_Publisher < 0)
				{
					Object.Destroy(base.gameObject);
					mS_.award_Publisher = 0;
					return;
				}
				flag = true;
				break;
			}
			if (!isGhost && !fromSavegame)
			{
				mS_.Multiplayer_SendObject(myID, typ, base.transform.position.x, base.transform.position.z, base.transform.eulerAngles.y);
			}
			if (partikel)
			{
				Object.Instantiate(mS_.miscParticlePrefabs[0]).transform.position = base.transform.position;
			}
			if (!fromSavegame)
			{
				sfx_.PlaySound(4, force: false);
			}
			if (!gekauft)
			{
				gekauft = true;
				mS_.Pay(preis, 1);
				Debug.Log("PAY: " + preis);
				if (!autoInventar)
				{
					if (!flag)
					{
						guiMain.MoneyPop(preis, base.transform.position, green: false);
					}
					mapS_.CreateObject(typ, createdWithAutoInventar: false);
				}
			}
			else
			{
				guiMain.DeactivateMenu(guiMain.uiObjects[0]);
				guiMain.CloseMenu();
				ReOpenBuyInventarMenu();
			}
		}
		if (isRobotClean)
		{
			GetComponent<createRobot>().Init(myID);
		}
		StartCoroutine(DestroyUnnoetigeComponents());
	}

	public bool ReOpenBuyInventarMenu()
	{
		FindScripts();
		if (pickObject_.reopenBuyInventarMenu)
		{
			pickObject_.reopenBuyInventarMenu = false;
			guiMain.DROPDOWN_BuyInventar(pickObject_.buyInventar);
			return true;
		}
		return false;
	}

	private void RemoveTilesView()
	{
		Transform transform = base.transform.Find("TilesView");
		if ((bool)transform)
		{
			Object.Destroy(transform.gameObject);
		}
	}

	private IEnumerator DestroyUnnoetigeComponents()
	{
		yield return new WaitForSeconds(2f);
		Animation component = GetComponent<Animation>();
		if ((bool)component)
		{
			Object.Destroy(component);
		}
	}

	public void MouseOver()
	{
		SetOutlineLayer();
		if (aufladungenMax > 0)
		{
			guiMain.ShowObjectTooltip(this);
		}
	}

	public void MouseLeave()
	{
		DisableOutlineLayer();
		guiMain.DisableObjectTooltip();
	}

	public void SetOutlineLayer()
	{
		mCamS_.EnablePostLiner();
		if (!outline)
		{
			outline = true;
			mCamS_.SetOutlineColor(2, 0.3f, 4);
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

	public bool HasAufladungen()
	{
		if (aufladungenMax <= 0)
		{
			return true;
		}
		if (aufladungenAkt > 0)
		{
			return true;
		}
		return false;
	}

	public void SetBesetzt(int i)
	{
		if (!wirdNichtbesetzt)
		{
			besetztCharID = i;
		}
	}

	public void SetUnbesetzt(int i)
	{
		besetztCharID = -1;
	}

	public bool IsUnbesetzt()
	{
		if (wirdNichtbesetzt)
		{
			return true;
		}
		if (besetztCharID == -1)
		{
			return true;
		}
		return false;
	}

	public int GetVerkaufspreis()
	{
		return Mathf.RoundToInt((float)preis * 0.5f);
	}

	public void Monatskosten()
	{
		if (monatsKosten <= 0 || !mS_)
		{
			return;
		}
		int num = monatsKosten;
		if (isServer)
		{
			if (mS_.globalEvent == 9)
			{
				num = monatsKosten * 2;
			}
			if (myRoomID != 1)
			{
				int num2 = Mathf.RoundToInt(base.transform.position.x);
				int num3 = Mathf.RoundToInt(base.transform.position.z);
				roomScript roomScript2 = mapS_.mapRoomScript[num2, num3];
				if ((bool)roomScript2 && roomScript2.serverDown)
				{
					return;
				}
			}
		}
		if (!isServer)
		{
			mS_.Pay(num, 8);
		}
		else
		{
			mS_.Pay(num, 34);
		}
		StartCoroutine(guiMain.MoneyPopEnumerate(num, base.transform.position, green: false));
	}

	public void ConsumeAufladung(int i)
	{
		if (!HasAufladungen())
		{
			return;
		}
		aufladungenAkt -= i;
		if (aufladungenAkt <= 0)
		{
			aufladungenAkt = 0;
			if ((bool)gfxObjectEmpty && !gfxObjectEmpty.activeSelf)
			{
				gfxObjectEmpty.SetActive(value: true);
			}
		}
	}

	public void AddAufladungen()
	{
		if (aufladungenMax > aufladungenAkt)
		{
			aufladungenAkt = aufladungenMax;
			if ((bool)gfxObjectEmpty && gfxObjectEmpty.activeSelf)
			{
				gfxObjectEmpty.SetActive(value: false);
			}
		}
	}

	private string GetTooltip()
	{
		FindScripts();
		string text = "";
		text = "<b>" + tS_.GetObjects(typ) + "</b>";
		text = text + "<br>" + tS_.GetObjectsTooltip(typ) + "<br>";
		if (aufladungenMax > 0)
		{
			string text2 = tS_.GetText(775);
			text2 = text2.Replace("<NUM>", aufladungenAkt + "/" + aufladungenMax);
			text = text + "<br>" + text2;
		}
		return text;
	}

	private string GetQualitatStars(int i)
	{
		string text = "";
		return i switch
		{
			0 => "☆☆☆☆☆", 
			1 => "★☆☆☆☆", 
			2 => "★★☆☆☆", 
			3 => "★★★☆☆", 
			4 => "★★★★☆", 
			5 => "★★★★★", 
			_ => "☆☆☆☆☆", 
		};
	}

	public int GetServerplatz()
	{
		int num = serverplatz;
		int num2 = Mathf.RoundToInt(base.transform.position.x);
		int num3 = Mathf.RoundToInt(base.transform.position.z);
		if (!mapS_.IsInMapLimit(num2, num3))
		{
			return num;
		}
		if ((bool)mapS_.mapRoomScript[num2, num3] && mapS_.mapRoomScript[num2, num3].serverDown)
		{
			mapS_.mapRoomScript[num2, num3].serverOverheat = false;
			if (particle.activeSelf)
			{
				particle.SetActive(value: false);
			}
		}
		if (mS_.globalEvent == 11)
		{
			num = Mathf.RoundToInt((float)num * 0.7f);
		}
		if (mapS_.mapWaerme[num2, num3] > 0.5f)
		{
			if (!particle.activeSelf)
			{
				particle.SetActive(value: true);
			}
			if ((bool)mapS_.mapRoomScript[num2, num3])
			{
				mapS_.mapRoomScript[num2, num3].serverOverheat = true;
			}
			return num / 2;
		}
		if (particle.activeSelf)
		{
			particle.SetActive(value: false);
		}
		return num;
	}

	public int GetRoomID()
	{
		return myRoomID;
	}
}
