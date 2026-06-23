using System.Collections;
using UnityEngine;

public class autoInventarScript : MonoBehaviour
{
	private mainScript mS_;

	private mapScript mapScript_;

	private GUI_Main guiMain;

	private Camera myCamera;

	private mainCameraScript mCamS_;

	private sfxScript sfx_;

	private roomDataScript rdS_;

	private roomScript rS_;

	public GameObject collisionCheckAutoInventar;

	private int[,] bufferKollision_OnGround;

	public bool autoBuildEnabled;

	public int stufe = 5;

	private bool SETTING_Cooler;

	private bool SETTING_Heizung;

	private bool SETTING_DisableArztschrank;

	private bool SETTING_NichtAnWall_XM;

	private bool SETTING_NichtAnWall_XP;

	private bool SETTING_NichtAnWall_YM;

	private bool SETTING_NichtAnWall_YP;

	private bool SETTING_NurAnWall;

	private bool SETTING_NurAnWall_Ignore_XM;

	private bool SETTING_NurAnWall_Ignore_XP;

	private bool SETTING_NurAnWall_Ignore_YM;

	private bool SETTING_NurAnWall_Ignore_YP;

	private bool SETTING_Doppelt;

	private bool SETTING_NichtInEcke;

	private int SETTING_Rotation;

	private int SETTING_TILES_X = 1;

	private int SETTING_TILES_Y = 1;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!mS_)
		{
			mS_ = GetComponent<mainScript>();
		}
		if (!rdS_)
		{
			rdS_ = GetComponent<roomDataScript>();
		}
		if (!mapScript_)
		{
			mapScript_ = GetComponent<mapScript>();
		}
		if (!myCamera)
		{
			myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		if (!guiMain)
		{
			guiMain = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!mCamS_)
		{
			mCamS_ = GameObject.Find("Camera").GetComponent<mainCameraScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
	}

	private void ResetSettings()
	{
		SETTING_Cooler = false;
		SETTING_Heizung = false;
		SETTING_DisableArztschrank = false;
		SETTING_NichtAnWall_XM = false;
		SETTING_NichtAnWall_XP = false;
		SETTING_NichtAnWall_YM = false;
		SETTING_NichtAnWall_YP = false;
		SETTING_NurAnWall = false;
		SETTING_NurAnWall_Ignore_XM = false;
		SETTING_NurAnWall_Ignore_XP = false;
		SETTING_NurAnWall_Ignore_YM = false;
		SETTING_NurAnWall_Ignore_YP = false;
		SETTING_Doppelt = false;
		SETTING_NichtInEcke = false;
		SETTING_Rotation = 0;
		SETTING_TILES_X = 1;
		SETTING_TILES_Y = 1;
	}

	public void FillAutomaticInventar(roomScript script_, int stufe_)
	{
		Debug.Log("FillAutomaticInventar()");
		autoBuildEnabled = true;
		stufe = stufe_;
		if ((bool)script_)
		{
			for (int i = 0; i < script_.listInventar.Count; i++)
			{
				if ((bool)script_.listInventar[i])
				{
					script_.listInventar[i].waerme = 0f;
					script_.listInventar[i].kaelte = 0f;
					script_.listInventar[i].ausstattung = 0f;
					mS_.Earn(script_.listInventar[i].preis, 54);
					Object.Destroy(script_.listInventar[i].gameObject);
				}
			}
		}
		mapScript_.UpdateMapFilter(force: true);
		if ((bool)script_)
		{
			rS_ = script_;
			switch (rS_.typ)
			{
			case 1:
				ROOM_Development();
				break;
			case 2:
				ROOM_Forschung();
				break;
			case 3:
				ROOM_QA();
				break;
			case 4:
				ROOM_Grafikstudio();
				break;
			case 5:
				ROOM_Soundstudio();
				break;
			case 6:
				ROOM_Marketing();
				break;
			case 7:
				ROOM_Support();
				break;
			case 8:
				ROOM_Hardware();
				break;
			case 9:
				ROOM_Lager();
				break;
			case 10:
				ROOM_MotionCapture();
				break;
			case 11:
				ROOM_WC();
				break;
			case 12:
				ROOM_Aufenthalt();
				break;
			case 13:
				ROOM_Training();
				break;
			case 14:
				ROOM_Produktion();
				break;
			case 15:
				ROOM_Server();
				break;
			case 17:
				ROOM_Werkstatt();
				break;
			}
		}
		StartCoroutine(DisableAutoBuild());
	}

	private IEnumerator DisableAutoBuild()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForSeconds(0.5f);
		mS_.FindObjects();
		for (int i = 0; i < mS_.arrayObjectScripts.Length; i++)
		{
			if ((bool)mS_.arrayObjectScripts[i])
			{
				mS_.arrayObjectScripts[i].autoInventarItem = false;
			}
		}
		autoBuildEnabled = false;
	}

	private bool TilesFree(int posX, int posY, objectScript script_, int roomID, bool keinKollsionsCheck)
	{
		if (!mS_.sandbox_allItems && mS_.year < script_.unlockYear)
		{
			return false;
		}
		int sETTING_TILES_X = SETTING_TILES_X;
		int sETTING_TILES_Y = SETTING_TILES_Y;
		for (int i = posX; i < sETTING_TILES_X + posX; i++)
		{
			for (int j = posY; j < sETTING_TILES_Y + posY; j++)
			{
				if (mapScript_.IsInMapLimit(i, j))
				{
					if (mapScript_.mapRoomID[i, j] != roomID || mapScript_.mapDoors[i, j] != 0)
					{
						return false;
					}
					continue;
				}
				return false;
			}
		}
		if (!keinKollsionsCheck)
		{
			for (int k = posX; k < sETTING_TILES_X + posX; k++)
			{
				for (int l = posY; l < sETTING_TILES_Y + posY; l++)
				{
					if (mapScript_.IsInMapLimit(k, l))
					{
						if (bufferKollision_OnGround[k, l] > 0)
						{
							return false;
						}
						continue;
					}
					return false;
				}
			}
		}
		return true;
	}

	private bool CheckNurAnWall(int posX, int posY, objectScript script_, int roomID)
	{
		if (!SETTING_NurAnWall)
		{
			return true;
		}
		if (!SETTING_NurAnWall_Ignore_XP && mapScript_.IsInMapLimit(posX + 1, posY) && mapScript_.mapRoomID[posX + 1, posY] != roomID)
		{
			mS_.objectRotation = 270f;
			return true;
		}
		if (!SETTING_NurAnWall_Ignore_XM && mapScript_.IsInMapLimit(posX - 1, posY) && mapScript_.mapRoomID[posX - 1, posY] != roomID)
		{
			mS_.objectRotation = 90f;
			return true;
		}
		if (!SETTING_NurAnWall_Ignore_YP && mapScript_.IsInMapLimit(posX, posY + 1) && mapScript_.mapRoomID[posX, posY + 1] != roomID)
		{
			mS_.objectRotation = 180f;
			return true;
		}
		if (!SETTING_NurAnWall_Ignore_YM && mapScript_.IsInMapLimit(posX, posY - 1) && mapScript_.mapRoomID[posX, posY - 1] != roomID)
		{
			mS_.objectRotation = 0f;
			return true;
		}
		return false;
	}

	private bool CheckNichtInEcke(int posX, int posY, objectScript script_, int roomID)
	{
		if (!SETTING_NichtInEcke)
		{
			return true;
		}
		int sETTING_TILES_X = SETTING_TILES_X;
		int sETTING_TILES_Y = SETTING_TILES_Y;
		int num = 0;
		for (int i = posX; i < sETTING_TILES_X + posX; i++)
		{
			for (int j = posY; j < sETTING_TILES_Y + posY; j++)
			{
				if (mapScript_.IsInMapLimit(i + 1, j) && mapScript_.mapRoomID[i + 1, j] != roomID)
				{
					num++;
				}
				if (mapScript_.IsInMapLimit(i - 1, j) && mapScript_.mapRoomID[i - 1, j] != roomID)
				{
					num++;
				}
				if (mapScript_.IsInMapLimit(i, j + 1) && mapScript_.mapRoomID[i, j + 1] != roomID)
				{
					num++;
				}
				if (mapScript_.IsInMapLimit(i, j - 1) && mapScript_.mapRoomID[i, j - 1] != roomID)
				{
					num++;
				}
			}
		}
		if (num >= 2)
		{
			return false;
		}
		return true;
	}

	private bool CheckNichtAnWall(int posX, int posY, objectScript script_, int roomID)
	{
		if (!SETTING_NichtAnWall_XP && !SETTING_NichtAnWall_XM && !SETTING_NichtAnWall_YP && !SETTING_NichtAnWall_YM)
		{
			return true;
		}
		int sETTING_TILES_X = SETTING_TILES_X;
		int sETTING_TILES_Y = SETTING_TILES_Y;
		for (int i = posX; i < sETTING_TILES_X + posX; i++)
		{
			for (int j = posY; j < sETTING_TILES_Y + posY; j++)
			{
				if (SETTING_NichtAnWall_XP)
				{
					if (!mapScript_.IsInMapLimit(i + 1, j))
					{
						return false;
					}
					if (mapScript_.mapRoomID[i + 1, j] != roomID)
					{
						return false;
					}
				}
				if (SETTING_NichtAnWall_XM)
				{
					if (!mapScript_.IsInMapLimit(i - 1, j))
					{
						return false;
					}
					if (mapScript_.mapRoomID[i - 1, j] != roomID)
					{
						return false;
					}
				}
				if (SETTING_NichtAnWall_YP)
				{
					if (!mapScript_.IsInMapLimit(i, j + 1))
					{
						return false;
					}
					if (mapScript_.mapRoomID[i, j + 1] != roomID)
					{
						return false;
					}
				}
				if (SETTING_NichtAnWall_YM)
				{
					if (!mapScript_.IsInMapLimit(i, j - 1))
					{
						return false;
					}
					if (mapScript_.mapRoomID[i, j - 1] != roomID)
					{
						return false;
					}
				}
			}
		}
		return true;
	}

	private void SetTilesBelegt(int posX, int posY, objectScript script_, int itemTyp_)
	{
		int sETTING_TILES_X = SETTING_TILES_X;
		int sETTING_TILES_Y = SETTING_TILES_Y;
		for (int i = posX; i < posX + sETTING_TILES_X; i++)
		{
			for (int j = posY; j < posY + sETTING_TILES_Y; j++)
			{
				bufferKollision_OnGround[i, j] = itemTyp_;
			}
		}
	}

	private int CreateItem(int item, Vector3 offset, bool randomRotation, int maxItems, bool rotateFromWall, int alleNumTiles, bool nichtAnFenster, bool keinKollsionsCheck, int randomChance)
	{
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				int num3 = i;
				int num4 = j;
				if (mapScript_.mapRoomID[num3, num4] != rS_.myID || (bufferKollision_OnGround[i, j] > 0 && !keinKollsionsCheck) || mapScript_.mapDoors[num3, num4] > 0 || (nichtAnFenster && (!nichtAnFenster || mapScript_.mapWindows[num3, num4] > 0 || CheckMapWindows(num3, num4))) || (SETTING_Heizung && (!SETTING_Heizung || !(mapScript_.mapWaerme[num3, num4] <= 0.2f))) || (SETTING_Cooler && (!SETTING_Cooler || !(mapScript_.mapWaerme[num3, num4] > 0f))) || !CheckNurAnWall(i, j, mapScript_.prefabsInventar[item].GetComponent<objectScript>(), rS_.myID) || !CheckNichtAnWall(i, j, mapScript_.prefabsInventar[item].GetComponent<objectScript>(), rS_.myID) || !CheckNichtInEcke(i, j, mapScript_.prefabsInventar[item].GetComponent<objectScript>(), rS_.myID) || !TilesFree(i, j, mapScript_.prefabsInventar[item].GetComponent<objectScript>(), rS_.myID, keinKollsionsCheck) || randomChance <= Random.Range(0, 100))
				{
					continue;
				}
				num2++;
				if (num2 < alleNumTiles)
				{
					continue;
				}
				num2 = 0;
				GameObject gameObject = mapScript_.CreateObject(item, createdWithAutoInventar: true);
				GameObject gameObject2 = null;
				if (SETTING_Doppelt)
				{
					gameObject2 = mapScript_.CreateObject(item, createdWithAutoInventar: true);
				}
				if (!gameObject)
				{
					continue;
				}
				objectScript component = gameObject.GetComponent<objectScript>();
				if (!component)
				{
					continue;
				}
				if (!rotateFromWall)
				{
					if (!randomRotation)
					{
						mS_.objectRotation = SETTING_Rotation;
					}
					else
					{
						mS_.objectRotation = Random.Range(0, 4) * 90;
					}
				}
				component.PlatziereObject(new Vector3((float)i + offset.x, 0f + offset.y, (float)j + offset.z), fromSavegame: false, updatePathfinding: false, autoInventar: true, partikel: false);
				if ((bool)gameObject2)
				{
					gameObject2.GetComponent<objectScript>().PlatziereObject(new Vector3((float)i + offset.x, 0f + offset.y, (float)j + offset.z + 1.5f), fromSavegame: false, updatePathfinding: false, autoInventar: true, partikel: false);
				}
				if (!keinKollsionsCheck)
				{
					SetTilesBelegt(i, j, component, item);
				}
				if (SETTING_Heizung || SETTING_Cooler)
				{
					mS_.FindObjects();
				}
				num++;
				if (num >= maxItems)
				{
					return num;
				}
			}
		}
		return num;
	}

	private bool CheckMapWindows(int x, int y)
	{
		if (mapScript_.mapWindows[x + 1, y] == 1180)
		{
			return true;
		}
		if (mapScript_.mapWindows[x - 1, y] == 1000)
		{
			return true;
		}
		if (mapScript_.mapWindows[x, y + 1] == 1090)
		{
			return true;
		}
		if (mapScript_.mapWindows[x, y - 1] == 1270)
		{
			return true;
		}
		return false;
	}

	private void QUALITY_WandObjects()
	{
		int randomChance = 2;
		SETTING_NurAnWall = true;
		CreateItem(39, new Vector3(0f, 0f, 0f), randomRotation: false, 2, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(12, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(13, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(14, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(15, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(16, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(18, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(19, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(20, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(21, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(22, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(83, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(84, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(85, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(86, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(87, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(165, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(166, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(167, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(168, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(169, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(170, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(171, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(172, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(173, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(174, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		ResetSettings();
	}

	private void QUALITY_BilderKlein()
	{
		int randomChance = 2;
		SETTING_NurAnWall = true;
		CreateItem(18, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(19, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(20, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(21, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(22, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(170, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(171, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(172, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(173, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(174, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		ResetSettings();
	}

	private void QUALITY_Schrank()
	{
		int randomChance = 3;
		SETTING_NurAnWall = true;
		CreateItem(108, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(109, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(110, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(136, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(137, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(138, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		CreateItem(31, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, randomChance);
		ResetSettings();
	}

	private void QUALITY_Pflanzen()
	{
		int randomChance = 3;
		SETTING_NurAnWall = true;
		CreateItem(7, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(8, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(9, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(181, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(182, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(183, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		ResetSettings();
	}

	private void QUALITY_Lampen()
	{
		int randomChance = 3;
		SETTING_NurAnWall = true;
		CreateItem(3, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(44, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(49, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(178, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(179, new Vector3(0f, 0f, 0f), randomRotation: true, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(180, new Vector3(0f, 0f, 0f), randomRotation: false, 2, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		ResetSettings();
	}

	private void QUALITY_Tische()
	{
		int randomChance = 2;
		SETTING_NurAnWall = true;
		CreateItem(158, new Vector3(0f, 0f, 0f), randomRotation: true, 1, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(159, new Vector3(0f, 0f, 0f), randomRotation: true, 1, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(160, new Vector3(0f, 0f, 0f), randomRotation: true, 1, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		CreateItem(184, new Vector3(0f, 0f, 0f), randomRotation: true, 1, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, randomChance);
		ResetSettings();
	}

	private void QUALITY_Teppich()
	{
		switch (Random.Range(0, 11))
		{
		case 0:
			SETTING_TILES_X = 3;
			SETTING_TILES_Y = 4;
			CreateItem(93, new Vector3(1f, 0f, 1.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 1:
			SETTING_TILES_X = 3;
			SETTING_TILES_Y = 3;
			CreateItem(94, new Vector3(1f, 0f, 1f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 2:
			SETTING_TILES_X = 2;
			SETTING_TILES_Y = 2;
			CreateItem(95, new Vector3(0.5f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 3:
			SETTING_TILES_X = 3;
			SETTING_TILES_Y = 3;
			CreateItem(96, new Vector3(1f, 0f, 1f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 4:
			SETTING_TILES_X = 2;
			SETTING_TILES_Y = 2;
			CreateItem(97, new Vector3(0.5f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 5:
			SETTING_TILES_X = 2;
			SETTING_TILES_Y = 2;
			CreateItem(98, new Vector3(0.5f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 6:
			SETTING_TILES_X = 3;
			SETTING_TILES_Y = 3;
			CreateItem(139, new Vector3(1f, 0f, 1f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 7:
			SETTING_TILES_X = 2;
			SETTING_TILES_Y = 2;
			CreateItem(140, new Vector3(0.5f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 8:
			SETTING_TILES_X = 2;
			SETTING_TILES_Y = 2;
			CreateItem(141, new Vector3(0.5f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 9:
			SETTING_TILES_X = 3;
			SETTING_TILES_Y = 3;
			CreateItem(175, new Vector3(1f, 0f, 1f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 10:
			SETTING_TILES_X = 3;
			SETTING_TILES_Y = 3;
			CreateItem(176, new Vector3(1f, 0f, 1f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 11:
			SETTING_TILES_X = 2;
			SETTING_TILES_Y = 2;
			CreateItem(177, new Vector3(0.5f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		}
		ResetSettings();
	}

	private void SONDER_Heizung()
	{
		int num = 0;
		num = ((mS_.year < 2010 && (!mS_.settings_sandbox || !mS_.sandbox_allItems)) ? 2 : 185);
		if (num == 185 && stufe < 2)
		{
			num = 2;
		}
		SETTING_Heizung = true;
		SETTING_NurAnWall = true;
		CreateItem(num, new Vector3(0f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
	}

	private void SONDER_Misc()
	{
		int num = 0;
		num = ((mS_.year < 1995 && (!mS_.settings_sandbox || !mS_.sandbox_allItems)) ? 102 : 107);
		if (num == 107 && stufe < 2)
		{
			num = 102;
		}
		SETTING_NurAnWall = true;
		CreateItem(4, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		if (!SETTING_DisableArztschrank)
		{
			CreateItem(72, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, 100);
		}
		CreateItem(5, new Vector3(0f, 0f, 0f), randomRotation: true, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		CreateItem(131, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		CreateItem(num, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
	}

	private void ROOM_Server()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 128 : ((mS_.year >= 2005) ? 127 : ((mS_.year >= 1995) ? 126 : ((mS_.year < 1985) ? 45 : 125))));
		if (num == 128 && stufe < 5)
		{
			num = 127;
		}
		if (num == 127 && stufe < 4)
		{
			num = 126;
		}
		if (num == 126 && stufe < 3)
		{
			num = 125;
		}
		if (num == 125 && stufe < 2)
		{
			num = 45;
		}
		SETTING_NichtAnWall_XM = true;
		SETTING_NichtAnWall_XP = true;
		SETTING_NichtAnWall_YM = true;
		SETTING_NichtAnWall_YP = true;
		SETTING_Rotation = Mathf.RoundToInt(rS_.myDoor.transform.eulerAngles.y + 90f);
		CreateItem(num, new Vector3(0f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		mS_.FindObjects();
		mapScript_.UpdateMapFilter(force: true);
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		num = 0;
		num = ((mS_.year < 2005 && (!mS_.settings_sandbox || !mS_.sandbox_allItems)) ? 46 : 154);
		if (num == 154 && stufe < 2)
		{
			num = 46;
		}
		SETTING_Cooler = true;
		SETTING_NurAnWall = true;
		CreateItem(num, new Vector3(0f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, 100);
		ResetSettings();
	}

	private void ROOM_Lager()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		int num = 0;
		num = ((mS_.year >= 1995 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 80 : ((mS_.year < 1985) ? 47 : 79));
		if (num == 80 && stufe < 3)
		{
			num = 79;
		}
		if (num == 79 && stufe < 2)
		{
			num = 47;
		}
		CreateItem(num, new Vector3(0f, 0f, 0f), randomRotation: true, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
	}

	private void ROOM_Aufenthalt()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_WandObjects();
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		SONDER_Heizung();
		SETTING_NurAnWall = true;
		SETTING_NichtInEcke = true;
		CreateItem(24, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		CreateItem(37, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		CreateItem(38, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		CreateItem(43, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		CreateItem(99, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		CreateItem(72, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		switch (Random.Range(0, 3))
		{
		case 0:
			CreateItem(33, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			CreateItem(34, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			CreateItem(35, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 1:
			CreateItem(40, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			CreateItem(41, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			CreateItem(42, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		case 2:
			CreateItem(69, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			CreateItem(70, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			CreateItem(71, new Vector3(0f, 0f, 0f), randomRotation: false, 3, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			break;
		}
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 2;
		CreateItem(78, new Vector3(0.5f, 0f, 0.5f), randomRotation: true, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 3;
		SETTING_TILES_Y = 1;
		SETTING_Rotation = 90;
		CreateItem(164, new Vector3(0.5f, 0f, 0f), randomRotation: false, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 3;
		SETTING_TILES_Y = 1;
		SETTING_Rotation = 90;
		CreateItem(188, new Vector3(0.5f, 0f, 0f), randomRotation: false, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 2;
		CreateItem(78, new Vector3(0.5f, 0f, 0.5f), randomRotation: true, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 3;
		SETTING_TILES_Y = 1;
		SETTING_Rotation = 90;
		CreateItem(164, new Vector3(0.5f, 0f, 0f), randomRotation: false, 2, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_NurAnWall = true;
		SETTING_NichtInEcke = true;
		CreateItem(24, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		CreateItem(37, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		CreateItem(38, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 3;
		SETTING_TILES_Y = 1;
		SETTING_Rotation = 90;
		CreateItem(164, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_NurAnWall = true;
		SETTING_NichtInEcke = true;
		CreateItem(38, new Vector3(0f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_WC()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		int num = 0;
		num = ((mS_.year < 2010 && (!mS_.settings_sandbox || !mS_.sandbox_allItems)) ? 10 : 186);
		if (num == 186 && stufe < 2)
		{
			num = 10;
		}
		int num2 = 0;
		num2 = ((mS_.year < 2010 && (!mS_.settings_sandbox || !mS_.sandbox_allItems)) ? 11 : 187);
		if (num2 == 187 && stufe < 2)
		{
			num2 = 11;
		}
		int num3 = 0;
		if (rS_.myDoor.transform.eulerAngles.y == 0f || rS_.myDoor.transform.eulerAngles.y == 270f || rS_.myDoor.transform.eulerAngles.y == 90f)
		{
			SETTING_Doppelt = true;
			SETTING_Rotation = 90;
			SETTING_TILES_X = 2;
			SETTING_TILES_Y = 4;
			num3 += CreateItem(num, new Vector3(0f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			ResetSettings();
		}
		if (rS_.myDoor.transform.eulerAngles.y == 180f)
		{
			SETTING_Doppelt = true;
			SETTING_Rotation = 270;
			SETTING_TILES_X = 2;
			SETTING_TILES_Y = 4;
			num3 += CreateItem(num, new Vector3(1f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			ResetSettings();
		}
		if (num3 <= 0)
		{
			if (rS_.myDoor.transform.eulerAngles.y == 0f || rS_.myDoor.transform.eulerAngles.y == 270f || rS_.myDoor.transform.eulerAngles.y == 90f)
			{
				SETTING_Rotation = 90;
				SETTING_TILES_X = 2;
				SETTING_TILES_Y = 2;
				num3 += CreateItem(num, new Vector3(0f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
				ResetSettings();
			}
			if (rS_.myDoor.transform.eulerAngles.y == 180f)
			{
				SETTING_Rotation = 270;
				SETTING_TILES_X = 2;
				SETTING_TILES_Y = 2;
				num3 += CreateItem(num, new Vector3(1f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
				ResetSettings();
			}
		}
		SONDER_Heizung();
		for (int k = 0; k < num3; k++)
		{
			SETTING_NurAnWall = true;
			CreateItem(num2, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			ResetSettings();
			SETTING_NurAnWall = true;
			CreateItem(23, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			ResetSettings();
		}
		num3 = 0;
		if (rS_.myDoor.transform.eulerAngles.y == 0f || rS_.myDoor.transform.eulerAngles.y == 270f || rS_.myDoor.transform.eulerAngles.y == 90f)
		{
			SETTING_Rotation = 90;
			SETTING_TILES_X = 2;
			SETTING_TILES_Y = 2;
			num3 += CreateItem(num, new Vector3(0f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			ResetSettings();
		}
		if (rS_.myDoor.transform.eulerAngles.y == 180f)
		{
			SETTING_Rotation = 270;
			SETTING_TILES_X = 2;
			SETTING_TILES_Y = 2;
			num3 += CreateItem(num, new Vector3(1f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			ResetSettings();
		}
		for (int l = 0; l < num3; l++)
		{
			SETTING_NurAnWall = true;
			CreateItem(num2, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			ResetSettings();
			SETTING_NurAnWall = true;
			CreateItem(23, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
			ResetSettings();
		}
	}

	private void ROOM_Development()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		SETTING_NurAnWall_Ignore_YP = true;
		QUALITY_WandObjects();
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		SONDER_Heizung();
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 53 : ((mS_.year >= 2005) ? 52 : ((mS_.year >= 1995) ? 51 : ((mS_.year < 1985) ? 1 : 50))));
		if (num == 53 && stufe < 5)
		{
			num = 52;
		}
		if (num == 52 && stufe < 4)
		{
			num = 51;
		}
		if (num == 51 && stufe < 3)
		{
			num = 50;
		}
		if (num == 50 && stufe < 2)
		{
			num = 1;
		}
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 3;
		SETTING_Doppelt = true;
		_ = 0 + CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 2;
		CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_Soundstudio()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 120 : ((mS_.year >= 2005) ? 119 : ((mS_.year >= 1995) ? 82 : ((mS_.year < 1985) ? 76 : 81))));
		if (num == 120 && stufe < 5)
		{
			num = 119;
		}
		if (num == 119 && stufe < 4)
		{
			num = 82;
		}
		if (num == 82 && stufe < 3)
		{
			num = 81;
		}
		if (num == 81 && stufe < 2)
		{
			num = 76;
		}
		SETTING_Doppelt = true;
		SETTING_Rotation = 0;
		SETTING_TILES_X = 4;
		SETTING_TILES_Y = 3;
		int num2 = CreateItem(num, new Vector3(2f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 4;
		SETTING_TILES_Y = 2;
		_ = num2 + CreateItem(num, new Vector3(2f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_Rotation = 270;
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 4;
		CreateItem(num, new Vector3(0.5f, 0f, 2f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Heizung();
		SETTING_DisableArztschrank = true;
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_Forschung()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		SETTING_NurAnWall_Ignore_YP = true;
		QUALITY_WandObjects();
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		SONDER_Heizung();
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 68 : ((mS_.year >= 2005) ? 67 : ((mS_.year >= 1995) ? 66 : ((mS_.year < 1985) ? 6 : 56))));
		if (num == 68 && stufe < 5)
		{
			num = 67;
		}
		if (num == 67 && stufe < 4)
		{
			num = 66;
		}
		if (num == 66 && stufe < 3)
		{
			num = 56;
		}
		if (num == 56 && stufe < 2)
		{
			num = 6;
		}
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 3;
		SETTING_Doppelt = true;
		_ = 0 + CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 2;
		CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_QA()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		SETTING_NurAnWall_Ignore_YP = true;
		QUALITY_WandObjects();
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		SONDER_Heizung();
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 91 : ((mS_.year >= 2005) ? 90 : ((mS_.year >= 1995) ? 89 : ((mS_.year < 1985) ? 74 : 88))));
		if (num == 91 && stufe < 5)
		{
			num = 90;
		}
		if (num == 90 && stufe < 4)
		{
			num = 89;
		}
		if (num == 89 && stufe < 3)
		{
			num = 88;
		}
		if (num == 88 && stufe < 2)
		{
			num = 74;
		}
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 3;
		SETTING_Doppelt = true;
		_ = 0 + CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 2;
		CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_Training()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		SETTING_NurAnWall_Ignore_YP = true;
		SETTING_NurAnWall_Ignore_YM = true;
		QUALITY_BilderKlein();
		ResetSettings();
		SETTING_NurAnWall = true;
		SETTING_NurAnWall_Ignore_YP = true;
		SETTING_NurAnWall_Ignore_XP = true;
		SETTING_NurAnWall_Ignore_XM = true;
		SETTING_TILES_X = 1;
		SETTING_TILES_Y = 1;
		CreateItem(55, new Vector3(0f, 0f, 0f), randomRotation: false, 1, rotateFromWall: true, 1, nichtAnFenster: true, keinKollsionsCheck: false, 20);
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		SONDER_Heizung();
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 114 : ((mS_.year >= 2005) ? 113 : ((mS_.year >= 1995) ? 112 : ((mS_.year < 1985) ? 54 : 111))));
		if (num == 114 && stufe < 5)
		{
			num = 113;
		}
		if (num == 113 && stufe < 4)
		{
			num = 112;
		}
		if (num == 112 && stufe < 3)
		{
			num = 111;
		}
		if (num == 111 && stufe < 2)
		{
			num = 54;
		}
		SETTING_TILES_X = 1;
		SETTING_TILES_Y = 3;
		SETTING_Doppelt = true;
		_ = 0 + CreateItem(num, new Vector3(0f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 1;
		SETTING_TILES_Y = 2;
		CreateItem(num, new Vector3(0f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_Support()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		SETTING_NurAnWall_Ignore_YP = true;
		QUALITY_WandObjects();
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		SONDER_Heizung();
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 65 : ((mS_.year >= 2005) ? 64 : ((mS_.year >= 1995) ? 63 : ((mS_.year < 1985) ? 61 : 62))));
		if (num == 65 && stufe < 5)
		{
			num = 64;
		}
		if (num == 64 && stufe < 4)
		{
			num = 63;
		}
		if (num == 63 && stufe < 3)
		{
			num = 62;
		}
		if (num == 62 && stufe < 2)
		{
			num = 61;
		}
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 3;
		SETTING_Doppelt = true;
		_ = 0 + CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 2;
		CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_Grafikstudio()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		SETTING_NurAnWall_Ignore_YP = true;
		QUALITY_WandObjects();
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		SONDER_Heizung();
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 106 : ((mS_.year >= 2005) ? 105 : ((mS_.year >= 1995) ? 104 : ((mS_.year < 1985) ? 75 : 103))));
		if (num == 106 && stufe < 5)
		{
			num = 105;
		}
		if (num == 105 && stufe < 4)
		{
			num = 104;
		}
		if (num == 104 && stufe < 3)
		{
			num = 103;
		}
		if (num == 103 && stufe < 2)
		{
			num = 75;
		}
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 3;
		SETTING_Doppelt = true;
		_ = 0 + CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 2;
		CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_Produktion()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 118 : ((mS_.year >= 2005) ? 117 : ((mS_.year >= 1995) ? 116 : ((mS_.year < 1985) ? 36 : 115))));
		if (num == 118 && stufe < 5)
		{
			num = 117;
		}
		if (num == 117 && stufe < 4)
		{
			num = 116;
		}
		if (num == 116 && stufe < 3)
		{
			num = 115;
		}
		if (num == 115 && stufe < 2)
		{
			num = 36;
		}
		SETTING_TILES_X = 5;
		SETTING_TILES_Y = 3;
		SETTING_Doppelt = true;
		_ = 0;
		CreateItem(num, new Vector3(2f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 5;
		SETTING_TILES_Y = 2;
		_ = 0 + CreateItem(num, new Vector3(2f, 0f, 0.5f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 5;
		SETTING_Rotation = 270;
		CreateItem(num, new Vector3(0.5f, 0f, 2f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
	}

	private void ROOM_Werkstatt()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		SETTING_NurAnWall_Ignore_YP = true;
		QUALITY_WandObjects();
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		SONDER_Heizung();
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 148 : ((mS_.year >= 2005) ? 147 : ((mS_.year >= 1995) ? 146 : ((mS_.year < 1985) ? 144 : 145))));
		if (num == 148 && stufe < 5)
		{
			num = 147;
		}
		if (num == 147 && stufe < 4)
		{
			num = 146;
		}
		if (num == 146 && stufe < 3)
		{
			num = 145;
		}
		if (num == 145 && stufe < 2)
		{
			num = 144;
		}
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 3;
		SETTING_Doppelt = true;
		_ = 0 + CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 2;
		CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_MotionCapture()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		SETTING_NurAnWall_Ignore_YP = true;
		QUALITY_WandObjects();
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 122 : ((mS_.year < 2005) ? 77 : 121));
		if (num == 122 && stufe < 3)
		{
			num = 121;
		}
		if (num == 121 && stufe < 2)
		{
			num = 77;
		}
		SETTING_TILES_X = 4;
		SETTING_TILES_Y = 3;
		CreateItem(num, new Vector3(1f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 3;
		SETTING_TILES_Y = 4;
		SETTING_Rotation = 270;
		CreateItem(num, new Vector3(1.5f, 0f, 1f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Heizung();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_Marketing()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		SETTING_NurAnWall_Ignore_YP = true;
		QUALITY_WandObjects();
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		SONDER_Heizung();
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 60 : ((mS_.year >= 2005) ? 59 : ((mS_.year >= 1995) ? 58 : ((mS_.year < 1985) ? 48 : 57))));
		if (num == 60 && stufe < 5)
		{
			num = 59;
		}
		if (num == 59 && stufe < 4)
		{
			num = 58;
		}
		if (num == 58 && stufe < 3)
		{
			num = 57;
		}
		if (num == 57 && stufe < 2)
		{
			num = 48;
		}
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 3;
		SETTING_Doppelt = true;
		_ = 0 + CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 2;
		CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}

	private void ROOM_Hardware()
	{
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		QUALITY_Teppich();
		bufferKollision_OnGround = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		SETTING_NurAnWall_Ignore_YP = true;
		QUALITY_WandObjects();
		ResetSettings();
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (bufferKollision_OnGround[i, j] != 72)
				{
					bufferKollision_OnGround[i, j] = 0;
				}
			}
		}
		SONDER_Heizung();
		int num = 0;
		num = ((mS_.year >= 2015 || (mS_.settings_sandbox && mS_.sandbox_allItems)) ? 153 : ((mS_.year >= 2005) ? 152 : ((mS_.year >= 1995) ? 151 : ((mS_.year < 1985) ? 149 : 150))));
		if (num == 153 && stufe < 5)
		{
			num = 152;
		}
		if (num == 152 && stufe < 4)
		{
			num = 151;
		}
		if (num == 151 && stufe < 3)
		{
			num = 150;
		}
		if (num == 150 && stufe < 2)
		{
			num = 149;
		}
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 3;
		SETTING_Doppelt = true;
		_ = 0 + CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SETTING_TILES_X = 2;
		SETTING_TILES_Y = 2;
		CreateItem(num, new Vector3(0.5f, 0f, 0f), randomRotation: false, 99999, rotateFromWall: false, 1, nichtAnFenster: false, keinKollsionsCheck: false, 100);
		ResetSettings();
		SONDER_Misc();
		QUALITY_Schrank();
		QUALITY_Tische();
		QUALITY_Pflanzen();
		QUALITY_Lampen();
	}
}
