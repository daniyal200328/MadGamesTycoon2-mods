using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mapScript : MonoBehaviour
{
	private mainScript mS_;

	private buildRoomScript brS_;

	private roomDataScript rdS_;

	private GUI_Main guiMain_;

	public static int mapSizeX = 100;

	public static int mapSizeY = 100;

	public static int ID_FLOOR = 1;

	public static int ID_FLOOROUTSIDE = 29;

	public int[,] mapRoomID = new int[mapSizeX, mapSizeY];

	public roomScript[,] mapRoomScript = new roomScript[mapSizeX, mapSizeY];

	public int[,] mapDoors = new int[mapSizeX, mapSizeY];

	public int[,] mapWindows = new int[mapSizeX, mapSizeY];

	public int[,] mapBlock = new int[mapSizeX, mapSizeY];

	public int[,] mapBlockDoor = new int[mapSizeX, mapSizeY];

	public int[,] mapBuilding = new int[mapSizeX, mapSizeY];

	public int[,] mapRoomID_LAYOUT = new int[mapSizeX, mapSizeY];

	public int[,] mapRoomTyp_LAYOUT = new int[mapSizeX, mapSizeY];

	public int[,] mapDoors_LAYOUT = new int[mapSizeX, mapSizeY];

	public int[,] mapWindows_LAYOUT = new int[mapSizeX, mapSizeY];

	public GameObject ROOMS_MP;

	public GameObject[] prefabsWalls;

	public GameObject[] prefabsInventar;

	public AstarPath aStar_;

	private bool[,] doorsPlaced = new bool[mapSizeX, mapSizeY];

	public GameObject ROOMS;

	public float[,] mapMuell = new float[mapSizeX, mapSizeY];

	public float[,] mapWaerme = new float[mapSizeX, mapSizeY];

	public float[,] mapAusstattung = new float[mapSizeX, mapSizeY];

	private float muellTimer;

	private float updateMapFilterTimer;

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
		if (!brS_)
		{
			brS_ = GetComponent<buildRoomScript>();
		}
		if (!rdS_)
		{
			rdS_ = GetComponent<roomDataScript>();
		}
		if (!ROOMS)
		{
			ROOMS = GameObject.Find("ROOMS");
		}
		if (!ROOMS_MP)
		{
			ROOMS_MP = GameObject.Find("ROOMS_MP");
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void Update()
	{
		UpdateMapMuell(force: false);
	}

	public void InitBuilding(bool fromSavegame)
	{
		Debug.Log("InitBuilding()");
		GameObject[] array = GameObject.FindGameObjectsWithTag("BlockFloor");
		if (array.Length == 0)
		{
			return;
		}
		GameObject[] array2 = array;
		foreach (GameObject gameObject in array2)
		{
			mapBlock[Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z)] = 1;
		}
		array2 = array;
		for (int i = 0; i < array2.Length; i++)
		{
			Object.Destroy(array2[i]);
		}
		GameObject[] array3 = GameObject.FindGameObjectsWithTag("BlockDoor");
		if (array3.Length == 0)
		{
			return;
		}
		array2 = array3;
		foreach (GameObject gameObject2 in array2)
		{
			mapBlock[Mathf.RoundToInt(gameObject2.transform.position.x), Mathf.RoundToInt(gameObject2.transform.position.z)] = 1;
			mapBlockDoor[Mathf.RoundToInt(gameObject2.transform.position.x), Mathf.RoundToInt(gameObject2.transform.position.z)] = 1;
		}
		array2 = array3;
		for (int i = 0; i < array2.Length; i++)
		{
			Object.Destroy(array2[i]);
		}
		GameObject[] array4 = GameObject.FindGameObjectsWithTag("BuildingFloor");
		Debug.Log("MAP QM: " + array4.Length);
		if (array4.Length == 0)
		{
			return;
		}
		array2 = array4;
		foreach (GameObject gameObject3 in array2)
		{
			buildingFloor component = gameObject3.GetComponent<buildingFloor>();
			if ((bool)component)
			{
				mapRoomID[Mathf.RoundToInt(gameObject3.transform.position.x), Mathf.RoundToInt(gameObject3.transform.position.z)] = ID_FLOOR;
				mapBuilding[Mathf.RoundToInt(gameObject3.transform.position.x), Mathf.RoundToInt(gameObject3.transform.position.z)] = component.buildingID;
			}
		}
		array2 = array4;
		for (int i = 0; i < array2.Length; i++)
		{
			Object.Destroy(array2[i]);
		}
		array2 = GameObject.FindGameObjectsWithTag("BuildingFloorOutside");
		foreach (GameObject gameObject4 in array2)
		{
			mapRoomID[Mathf.RoundToInt(gameObject4.transform.position.x), Mathf.RoundToInt(gameObject4.transform.position.z)] = ID_FLOOROUTSIDE;
			mapBuilding[Mathf.RoundToInt(gameObject4.transform.position.x), Mathf.RoundToInt(gameObject4.transform.position.z)] = ID_FLOOROUTSIDE;
		}
		array2 = GameObject.FindGameObjectsWithTag("BuildingWindow");
		foreach (GameObject gameObject5 in array2)
		{
			int num = Mathf.RoundToInt(gameObject5.transform.position.x);
			int num2 = Mathf.RoundToInt(gameObject5.transform.position.z);
			mapWindows[num, num2] = Mathf.RoundToInt(gameObject5.transform.eulerAngles.y) + 1000;
		}
		array2 = GameObject.FindGameObjectsWithTag("BuildingDoor");
		foreach (GameObject gameObject6 in array2)
		{
			Debug.Log("BuildingDoor Name: " + gameObject6.name);
			int num3 = Mathf.RoundToInt(gameObject6.transform.position.x);
			int num4 = Mathf.RoundToInt(gameObject6.transform.position.z);
			mapDoors[num3, num4] = 99;
		}
		if (mS_.multiplayer)
		{
			for (int j = 0; j < mapSizeX; j++)
			{
				for (int k = 0; k < mapSizeY; k++)
				{
					mapRoomID_LAYOUT[j, k] = mapRoomID[j, k];
					mapWindows_LAYOUT[j, k] = mapWindows[j, k];
					mapDoors_LAYOUT[j, k] = mapDoors[j, k];
				}
			}
			if (!fromSavegame)
			{
				for (int l = 0; l < mS_.mpCalls_.playersMP.Count; l++)
				{
					mS_.mpCalls_.playersMP[l].mapRoomID = (int[,])mapRoomID_LAYOUT.Clone();
					mS_.mpCalls_.playersMP[l].mapWindows = (int[,])mapWindows_LAYOUT.Clone();
					mS_.mpCalls_.playersMP[l].mapDoors = (int[,])mapDoors_LAYOUT.Clone();
				}
			}
		}
		Debug.Log("CreateRoomsForBuildingsToBuy()");
		if (!fromSavegame)
		{
			CreateRoomsForBuildingsToBuy();
		}
		Debug.Log("CreateWalls()");
		CreateWalls(-1);
		SANDBOX_UnlockAllBuildings();
	}

	private void SANDBOX_UnlockAllBuildings()
	{
		if (!mS_.settings_sandbox || !mS_.sandbox_allBuildings)
		{
			return;
		}
		for (int i = 2; i < mS_.buildings.Length; i++)
		{
			if (mS_.buildings[i])
			{
				continue;
			}
			mS_.buildings[i] = true;
			GameObject gameObject = GameObject.Find("Room_" + i);
			if ((bool)gameObject)
			{
				roomScript component = gameObject.GetComponent<roomScript>();
				if ((bool)component)
				{
					component.Demolish();
				}
			}
		}
	}

	public void CreateRoomsForBuildingsToBuy()
	{
		roomScript roomScript2 = null;
		bool flag = false;
		for (int i = 2; i < mS_.buildings.Length; i++)
		{
			if ((bool)roomScript2)
			{
				roomScript2.uiPos = brS_.FindUiPositionExtern(roomScript2.myID);
			}
			flag = false;
			roomScript2 = null;
			for (int j = 0; j < mapSizeX; j++)
			{
				for (int k = 0; k < mapSizeY; k++)
				{
					if (mapBuilding[j, k] == i)
					{
						int num = i;
						mapRoomID[j, k] = num;
						if (!flag)
						{
							flag = true;
							GameObject obj = Object.Instantiate(brS_.roomMainObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
							roomScript2 = obj.GetComponent<roomScript>();
							obj.name = "Room_" + num;
							roomScript2.myID = num;
							roomScript2.typ = 16;
							roomScript2.taskID = -1;
							roomScript2.myName = "";
							roomScript2.pause = false;
							roomScript2.lockKI = false;
							obj.transform.position = new Vector3(roomScript2.uiPos.x, 0f, roomScript2.uiPos.z);
						}
						mapRoomScript[j, k] = roomScript2;
					}
				}
			}
		}
	}

	public void RemoveRoom(int id_, bool particle)
	{
		Debug.Log("RemoveRoom()");
		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				if (mapRoomID[i, j] == id_)
				{
					mapRoomID[i, j] = ID_FLOOR;
					mapRoomScript[i, j] = null;
					mapDoors[i, j] = 0;
					mapWindows[i, j] = 0;
					if (particle)
					{
						StartCoroutine(brS_.CreateParticleDemolish(i, j, Random.Range(0f, 0.2f)));
					}
					mS_.Multiplayer_SendMap(i, j);
				}
			}
		}
		CreateWalls(-1);
	}

	public void UpdatePathfindingInstant()
	{
		StartCoroutine(UpdatePathfindingInstantNextFrame());
	}

	public void UpdatePathfinding()
	{
		StartCoroutine(UpdatePathfindingNextFrame());
	}

	private IEnumerator UpdatePathfindingNextFrame()
	{
		Debug.Log("mapScript -> UpdatePathfindingNextFrame() Wait");
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Debug.Log("mapScript -> UpdatePathfindingNextFrame()");
		if (!aStar_)
		{
			aStar_ = GameObject.Find("AStar").GetComponent<AstarPath>();
		}
		if ((bool)aStar_)
		{
			aStar_.Scan();
		}
	}

	private IEnumerator UpdatePathfindingInstantNextFrame()
	{
		Debug.Log("mapScript -> UpdatePathfindingInstantNextFrame() Wait");
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Debug.Log("mapScript -> UpdatePathfindingInstantNextFrame()");
		if (!aStar_)
		{
			aStar_ = GameObject.Find("AStar").GetComponent<AstarPath>();
		}
		if ((bool)aStar_)
		{
			aStar_.Scan();
		}
	}

	public void CreateWalls(int buildingID)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i] && (mapBuilding[Mathf.RoundToInt(array[i].transform.position.x), Mathf.RoundToInt(array[i].transform.position.z)] == buildingID || buildingID == -1))
			{
				array[i].GetComponent<roomScript>().listGameObjects.Clear();
			}
		}
		for (int j = 0; j < ROOMS.transform.childCount; j++)
		{
			Transform child = ROOMS.transform.GetChild(j);
			if (mapBuilding[Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.z)] == buildingID || buildingID == -1)
			{
				Object.Destroy(child.gameObject);
			}
		}
		doorsPlaced = new bool[mapSizeX, mapSizeY];
		GameObject gameObject = null;
		roomScript script_ = null;
		int num = -1;
		for (int k = 0; k < mapSizeX; k++)
		{
			for (int l = 0; l < mapSizeY; l++)
			{
				if ((mapBuilding[k, l] != buildingID && buildingID != -1) || mapRoomID[k, l] <= 0)
				{
					continue;
				}
				if (num != mapRoomID[k, l])
				{
					script_ = null;
					num = mapRoomID[k, l];
					gameObject = GameObject.Find("Room_" + num);
					if ((bool)gameObject)
					{
						script_ = gameObject.GetComponent<roomScript>();
					}
				}
				InstantiateMap(k, l, gameObject, script_);
			}
		}
		UpdatePathfinding();
		guiMain_.filterToggles = -1;
	}

	public void CreateWalls_Multiplayer(int playerID_)
	{
		player_mp player_mp2 = mS_.mpCalls_.FindPlayer(playerID_);
		for (int i = 0; i < ROOMS_MP.transform.childCount; i++)
		{
			Transform child = ROOMS_MP.transform.GetChild(i);
			if ((bool)child)
			{
				Object.Destroy(child.gameObject);
			}
		}
		doorsPlaced = new bool[mapSizeX, mapSizeY];
		for (int j = 0; j < mapSizeX; j++)
		{
			for (int k = 0; k < mapSizeY; k++)
			{
				if (player_mp2.mapRoomID[j, k] > 0)
				{
					InstantiateMap_Multiplayer(player_mp2, j, k, player_mp2.mapRoomID[j, k], player_mp2.mapRoomTyp[j, k]);
				}
			}
		}
		guiMain_.filterToggles = -1;
	}

	private void InstantiateMap(int x, int y, GameObject room, roomScript script_)
	{
		GameObject gameObject = null;
		if (mapBlockDoor[x, y] != 0 || mapRoomID[x, y] == ID_FLOOROUTSIDE)
		{
			return;
		}
		int roomTyp = 0;
		if (!room)
		{
			roomTyp = 0;
		}
		else if ((bool)script_)
		{
			roomTyp = script_.typ;
		}
		gameObject = Object.Instantiate(prefabsWalls[GetFloorPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
		if (mapBlock[x, y] != 0)
		{
			gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().material = mS_.specialMaterials[4];
			gameObject.GetComponent<floorScript>().materials[0] = mS_.specialMaterials[4];
		}
		SetRoomParent(gameObject);
		if ((bool)script_)
		{
			script_.listGameObjects.Add(gameObject);
		}
		if (mapDoors[x, y] != 99 && mapDoors[x, y] > 0 && mapRoomID[x, y] == 1)
		{
			mapDoors[x, y] = 0;
		}
		if (mapDoors[x, y] == 1 && mapRoomID[x, y] >= 1)
		{
			mapDoors[x, y] = 0;
			guiMain_.OpenMenu(hideChars: false);
			guiMain_.MessageBox("Your savegame is out of date. You need to reset the doors of your rooms.", closeMenu: true);
		}
		if (mapWindows[x, y] != 99 && mapWindows[x, y] > 0 && mapRoomID[x, y] == 1)
		{
			mapDoors[x, y] = 0;
		}
		if (mapWindows[x, y] == 1 && mapWindows[x, y] >= 1)
		{
			mapWindows[x, y] = 0;
			guiMain_.OpenMenu(hideChars: false);
			guiMain_.MessageBox("Your savegame is out of date. The windows of your buildings may not be displayed.", closeMenu: true);
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (mapRoomID[x, y] == ID_FLOOR)
		{
			if (mapDoors[x + 1, y] == 1180)
			{
				flag = true;
			}
			if (mapDoors[x - 1, y] == 1000)
			{
				flag3 = true;
			}
			if (mapDoors[x, y + 1] == 1090)
			{
				flag4 = true;
			}
			if (mapDoors[x, y - 1] == 1270)
			{
				flag2 = true;
			}
		}
		if (mapDoors[x, y] > 0 && mapDoors[x, y] > 0 && mapRoomID[x, y] != 0 && !doorsPlaced[x, y] && mapDoors[x, y] != 99)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetDoorPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, mapDoors[x, y] - 1000, 0f);
			SetRoomParent(gameObject);
			script_.myDoor = gameObject;
			switch (mapDoors[x, y] - 1000)
			{
			case 0:
				flag = true;
				break;
			case 90:
				flag2 = true;
				break;
			case 180:
				flag3 = true;
				break;
			case 270:
				flag4 = true;
				break;
			}
		}
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		if (mapRoomID[x, y] > 0)
		{
			if (mapWindows[x + 1, y] == 1180)
			{
				flag5 = true;
				gameObject = Object.Instantiate(prefabsWalls[GetWindowPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
				gameObject.transform.eulerAngles = new Vector3(0f, mapWindows[x + 1, y] - 1000 + 180, 0f);
				SetRoomParent(gameObject);
			}
			if (mapWindows[x - 1, y] == 1000)
			{
				flag7 = true;
				gameObject = Object.Instantiate(prefabsWalls[GetWindowPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
				gameObject.transform.eulerAngles = new Vector3(0f, mapWindows[x - 1, y] - 1000 + 180, 0f);
				SetRoomParent(gameObject);
			}
			if (mapWindows[x, y + 1] == 1090)
			{
				flag8 = true;
				gameObject = Object.Instantiate(prefabsWalls[GetWindowPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
				gameObject.transform.eulerAngles = new Vector3(0f, mapWindows[x, y + 1] - 1000 + 180, 0f);
				SetRoomParent(gameObject);
			}
			if (mapWindows[x, y - 1] == 1270)
			{
				flag6 = true;
				gameObject = Object.Instantiate(prefabsWalls[GetWindowPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
				gameObject.transform.eulerAngles = new Vector3(0f, mapWindows[x, y - 1] - 1000 + 180, 0f);
				SetRoomParent(gameObject);
			}
		}
		if (mapWindows[x, y] > 0 && mapWindows[x, y] > 0 && mapRoomID[x, y] != 0 && mapWindows[x, y] != 99)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetWindowPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, mapWindows[x, y] - 1000, 0f);
			SetRoomParent(gameObject);
			switch (mapWindows[x, y] - 1000)
			{
			case 0:
				flag5 = true;
				break;
			case 90:
				flag6 = true;
				break;
			case 180:
				flag7 = true;
				break;
			case 270:
				flag8 = true;
				break;
			}
		}
		if (mapRoomID[x + 1, y] != mapRoomID[x, y] && !flag && !flag5)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetWallPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			SetRoomParent(gameObject);
		}
		if (mapRoomID[x - 1, y] != mapRoomID[x, y] && !flag3 && !flag7)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetWallPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			SetRoomParent(gameObject);
		}
		if (mapRoomID[x, y + 1] != mapRoomID[x, y] && !flag4 && !flag8)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetWallPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 270f, 0f);
			SetRoomParent(gameObject);
		}
		if (mapRoomID[x, y - 1] != mapRoomID[x, y] && !flag2 && !flag6)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetWallPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 90f, 0f);
			SetRoomParent(gameObject);
		}
		if (mapRoomID[x, y - 1] == mapRoomID[x, y] && mapRoomID[x - 1, y] == mapRoomID[x, y] && mapRoomID[x - 1, y - 1] != mapRoomID[x, y])
		{
			gameObject = Object.Instantiate(prefabsWalls[GetEdgeOutPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			SetRoomParent(gameObject);
		}
		if (mapRoomID[x, y - 1] == mapRoomID[x, y] && mapRoomID[x + 1, y] == mapRoomID[x, y] && mapRoomID[x + 1, y - 1] != mapRoomID[x, y])
		{
			gameObject = Object.Instantiate(prefabsWalls[GetEdgeOutPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 90f, 0f);
			SetRoomParent(gameObject);
		}
		if (mapRoomID[x, y + 1] == mapRoomID[x, y] && mapRoomID[x - 1, y] == mapRoomID[x, y] && mapRoomID[x - 1, y + 1] != mapRoomID[x, y])
		{
			gameObject = Object.Instantiate(prefabsWalls[GetEdgeOutPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 270f, 0f);
			SetRoomParent(gameObject);
		}
		if (mapRoomID[x, y + 1] == mapRoomID[x, y] && mapRoomID[x + 1, y] == mapRoomID[x, y] && mapRoomID[x + 1, y + 1] != mapRoomID[x, y])
		{
			gameObject = Object.Instantiate(prefabsWalls[GetEdgeOutPrefab(roomTyp)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			SetRoomParent(gameObject);
		}
	}

	private void InstantiateMap_Multiplayer(player_mp p, int x, int y, int roomID_MP, int roomTYP_MP)
	{
		GameObject gameObject = null;
		if (mapBlockDoor[x, y] != 0 || roomID_MP == ID_FLOOROUTSIDE)
		{
			return;
		}
		gameObject = Object.Instantiate(prefabsWalls[GetFloorPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
		if (mapBlock[x, y] != 0)
		{
			gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().material = mS_.specialMaterials[4];
			Object.Destroy(gameObject.GetComponent<floorScript>());
		}
		SetRoomParent_Multiplayer(gameObject);
		if (p.mapDoors[x, y] != 99 && p.mapDoors[x, y] > 0 && p.mapRoomID[x, y] == 1)
		{
			p.mapDoors[x, y] = 0;
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (p.mapRoomID[x, y] == ID_FLOOR)
		{
			if (p.mapDoors[x + 1, y] == 1180)
			{
				flag = true;
			}
			if (p.mapDoors[x - 1, y] == 1000)
			{
				flag3 = true;
			}
			if (p.mapDoors[x, y + 1] == 1090)
			{
				flag4 = true;
			}
			if (p.mapDoors[x, y - 1] == 1270)
			{
				flag2 = true;
			}
		}
		if (p.mapDoors[x, y] > 0 && p.mapDoors[x, y] > 0 && p.mapRoomID[x, y] != 0 && !doorsPlaced[x, y] && p.mapDoors[x, y] != 99)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetDoorPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, p.mapDoors[x, y] - 1000, 0f);
			SetRoomParent_Multiplayer(gameObject);
			switch (p.mapDoors[x, y] - 1000)
			{
			case 0:
				flag = true;
				break;
			case 90:
				flag2 = true;
				break;
			case 180:
				flag3 = true;
				break;
			case 270:
				flag4 = true;
				break;
			}
		}
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		bool flag8 = false;
		if (p.mapRoomID[x, y] > 0)
		{
			if (p.mapWindows[x + 1, y] == 1180)
			{
				flag5 = true;
				gameObject = Object.Instantiate(prefabsWalls[GetWindowPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
				gameObject.transform.eulerAngles = new Vector3(0f, p.mapWindows[x + 1, y] - 1000 + 180, 0f);
				SetRoomParent_Multiplayer(gameObject);
			}
			if (p.mapWindows[x - 1, y] == 1000)
			{
				flag7 = true;
				gameObject = Object.Instantiate(prefabsWalls[GetWindowPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
				gameObject.transform.eulerAngles = new Vector3(0f, p.mapWindows[x - 1, y] - 1000 + 180, 0f);
				SetRoomParent_Multiplayer(gameObject);
			}
			if (p.mapWindows[x, y + 1] == 1090)
			{
				flag8 = true;
				gameObject = Object.Instantiate(prefabsWalls[GetWindowPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
				gameObject.transform.eulerAngles = new Vector3(0f, p.mapWindows[x, y + 1] - 1000 + 180, 0f);
				SetRoomParent_Multiplayer(gameObject);
			}
			if (p.mapWindows[x, y - 1] == 1270)
			{
				flag6 = true;
				gameObject = Object.Instantiate(prefabsWalls[GetWindowPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
				gameObject.transform.eulerAngles = new Vector3(0f, p.mapWindows[x, y - 1] - 1000 + 180, 0f);
				SetRoomParent_Multiplayer(gameObject);
			}
		}
		if (p.mapWindows[x, y] > 0 && p.mapWindows[x, y] > 0 && p.mapRoomID[x, y] != 0 && p.mapWindows[x, y] != 99)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetWindowPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, p.mapWindows[x, y] - 1000, 0f);
			SetRoomParent_Multiplayer(gameObject);
			switch (p.mapWindows[x, y] - 1000)
			{
			case 0:
				flag5 = true;
				break;
			case 90:
				flag6 = true;
				break;
			case 180:
				flag7 = true;
				break;
			case 270:
				flag8 = true;
				break;
			}
		}
		if (p.mapRoomID[x + 1, y] != p.mapRoomID[x, y] && !flag && !flag5)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetWallPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			SetRoomParent_Multiplayer(gameObject);
		}
		if (p.mapRoomID[x - 1, y] != p.mapRoomID[x, y] && !flag3 && !flag7)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetWallPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			SetRoomParent_Multiplayer(gameObject);
		}
		if (p.mapRoomID[x, y + 1] != p.mapRoomID[x, y] && !flag4 && !flag8)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetWallPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 270f, 0f);
			SetRoomParent_Multiplayer(gameObject);
		}
		if (p.mapRoomID[x, y - 1] != p.mapRoomID[x, y] && !flag2 && !flag6)
		{
			gameObject = Object.Instantiate(prefabsWalls[GetWallPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 90f, 0f);
			SetRoomParent_Multiplayer(gameObject);
		}
		if (p.mapRoomID[x, y - 1] == p.mapRoomID[x, y] && p.mapRoomID[x - 1, y] == p.mapRoomID[x, y] && p.mapRoomID[x - 1, y - 1] != p.mapRoomID[x, y])
		{
			gameObject = Object.Instantiate(prefabsWalls[GetEdgeOutPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
			SetRoomParent_Multiplayer(gameObject);
		}
		if (p.mapRoomID[x, y - 1] == p.mapRoomID[x, y] && p.mapRoomID[x + 1, y] == p.mapRoomID[x, y] && p.mapRoomID[x + 1, y - 1] != p.mapRoomID[x, y])
		{
			gameObject = Object.Instantiate(prefabsWalls[GetEdgeOutPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 90f, 0f);
			SetRoomParent_Multiplayer(gameObject);
		}
		if (p.mapRoomID[x, y + 1] == p.mapRoomID[x, y] && p.mapRoomID[x - 1, y] == p.mapRoomID[x, y] && p.mapRoomID[x - 1, y + 1] != p.mapRoomID[x, y])
		{
			gameObject = Object.Instantiate(prefabsWalls[GetEdgeOutPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 270f, 0f);
			SetRoomParent_Multiplayer(gameObject);
		}
		if (p.mapRoomID[x, y + 1] == p.mapRoomID[x, y] && p.mapRoomID[x + 1, y] == p.mapRoomID[x, y] && p.mapRoomID[x + 1, y + 1] != p.mapRoomID[x, y])
		{
			gameObject = Object.Instantiate(prefabsWalls[GetEdgeOutPrefab(roomTYP_MP)], new Vector3(x, 0f, y), Quaternion.identity);
			gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
			SetRoomParent_Multiplayer(gameObject);
		}
	}

	private int GetFloorPrefab(int roomTyp)
	{
		return roomTyp switch
		{
			0 => 3, 
			1 => 1, 
			2 => 12, 
			6 => 15, 
			11 => 20, 
			12 => 25, 
			14 => 30, 
			15 => 35, 
			9 => 40, 
			13 => 45, 
			7 => 50, 
			3 => 55, 
			4 => 60, 
			5 => 65, 
			10 => 70, 
			16 => 75, 
			17 => 80, 
			8 => 85, 
			_ => 3, 
		};
	}

	private int GetWallPrefab(int roomTyp)
	{
		return roomTyp switch
		{
			0 => 2, 
			1 => 0, 
			2 => 5, 
			6 => 14, 
			11 => 19, 
			12 => 24, 
			14 => 29, 
			15 => 34, 
			9 => 39, 
			13 => 44, 
			7 => 49, 
			3 => 54, 
			4 => 59, 
			5 => 64, 
			10 => 69, 
			16 => 74, 
			17 => 79, 
			8 => 84, 
			_ => 2, 
		};
	}

	private int GetEdgeOutPrefab(int roomTyp)
	{
		return roomTyp switch
		{
			0 => 6, 
			1 => 7, 
			2 => 13, 
			6 => 16, 
			11 => 21, 
			12 => 26, 
			14 => 31, 
			15 => 36, 
			9 => 41, 
			13 => 46, 
			7 => 51, 
			3 => 56, 
			4 => 61, 
			5 => 66, 
			10 => 71, 
			16 => 76, 
			17 => 81, 
			8 => 86, 
			_ => 6, 
		};
	}

	private int GetDoorPrefab(int roomTyp)
	{
		return roomTyp switch
		{
			1 => 9, 
			2 => 4, 
			6 => 17, 
			11 => 22, 
			12 => 27, 
			14 => 32, 
			15 => 37, 
			9 => 42, 
			13 => 47, 
			7 => 52, 
			3 => 57, 
			4 => 62, 
			5 => 67, 
			10 => 72, 
			16 => 77, 
			17 => 82, 
			8 => 87, 
			_ => 8, 
		};
	}

	private int GetWindowPrefab(int roomTyp)
	{
		return roomTyp switch
		{
			0 => 10, 
			1 => 11, 
			2 => 8, 
			6 => 18, 
			11 => 23, 
			12 => 28, 
			14 => 33, 
			15 => 38, 
			9 => 43, 
			13 => 48, 
			7 => 53, 
			3 => 58, 
			4 => 63, 
			5 => 68, 
			10 => 73, 
			16 => 78, 
			17 => 83, 
			8 => 88, 
			_ => 10, 
		};
	}

	private void SetRoomParent(GameObject go)
	{
		go.transform.SetParent(ROOMS.transform, worldPositionStays: true);
	}

	private void SetRoomParent_Multiplayer(GameObject go)
	{
		go.transform.SetParent(ROOMS_MP.transform, worldPositionStays: true);
		go.transform.GetChild(0).tag = "Untagged";
		go.transform.GetChild(0).transform.parent = ROOMS_MP.transform;
		Object.Destroy(go);
	}

	public int GetAmountRooms(int t)
	{
		int num = 0;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
		for (int i = 0; i < array.Length; i++)
		{
			roomScript component = array[i].GetComponent<roomScript>();
			if (t == component.typ)
			{
				num++;
			}
		}
		return num;
	}

	public GameObject CreateObject(int t, bool createdWithAutoInventar)
	{
		Debug.Log("CreateObject: " + t);
		GameObject obj = Object.Instantiate(prefabsInventar[t]);
		obj.transform.position = new Vector3(0f, 9999f, 0f);
		objectScript component = obj.GetComponent<objectScript>();
		if (createdWithAutoInventar)
		{
			component.autoInventarItem = true;
		}
		component.InitNewObject(t);
		return obj;
	}

	public Vector2 FindRandomFloorInMyBuilding(int id)
	{
		List<Vector2> list = new List<Vector2>();
		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				if (mapRoomID[i, j] == ID_FLOOR && mapBuilding[i, j] == id)
				{
					list.Add(new Vector2(i, j));
				}
			}
		}
		return list[Random.Range(0, list.Count)];
	}

	public Vector2 FindRandomFloor()
	{
		List<Vector2> list = new List<Vector2>();
		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				if (mapRoomID[i, j] == ID_FLOOR)
				{
					list.Add(new Vector2(i, j));
				}
			}
		}
		return list[Random.Range(0, list.Count)];
	}

	public bool IsInMapLimit(int x, int y)
	{
		if (x < 0 || x >= mapSizeX)
		{
			return false;
		}
		if (y < 0 || y >= mapSizeY)
		{
			return false;
		}
		return true;
	}

	public void UpdateMapMuell(bool force)
	{
		if (!force)
		{
			muellTimer += Time.deltaTime;
			if (muellTimer < 1f)
			{
				return;
			}
			muellTimer = 0f;
		}
		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				mapMuell[i, j] = 0f;
			}
		}
		for (int k = 0; k < mS_.arrayMuell.Length; k++)
		{
			if (!mS_.arrayMuell[k])
			{
				continue;
			}
			int num = Mathf.RoundToInt(mS_.arrayMuell[k].transform.position.x);
			int num2 = Mathf.RoundToInt(mS_.arrayMuell[k].transform.position.z);
			if (IsInMapLimit(num, num2))
			{
				mapMuell[num, num2] = 1f;
			}
			for (int l = num - 4; l < num + 4; l++)
			{
				for (int m = num2 - 4; m < num2 + 4; m++)
				{
					float num3 = Vector2.Distance(new Vector2(num, num2), new Vector2(l, m));
					if (num3 < 8f && IsInMapLimit(l, m) && IsInMapLimit(num, num2) && mapRoomID[num, num2] == mapRoomID[l, m])
					{
						mapMuell[l, m] += 1f / (num3 + 0.1f);
					}
				}
			}
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Muell_InUse");
		if (array.Length == 0)
		{
			return;
		}
		for (int n = 0; n < array.Length; n++)
		{
			if (!array[n])
			{
				continue;
			}
			int num4 = Mathf.RoundToInt(array[n].transform.position.x);
			int num5 = Mathf.RoundToInt(array[n].transform.position.z);
			if (IsInMapLimit(num4, num5))
			{
				mapMuell[num4, num5] = 1f;
			}
			for (int num6 = num4 - 4; num6 < num4 + 4; num6++)
			{
				for (int num7 = num5 - 4; num7 < num5 + 4; num7++)
				{
					float num8 = Vector2.Distance(new Vector2(num4, num5), new Vector2(num6, num7));
					if (num8 < 8f && num8 > 0f && IsInMapLimit(num6, num7) && IsInMapLimit(num4, num5) && mapRoomID[num4, num5] == mapRoomID[num6, num7])
					{
						mapMuell[num6, num7] += 1f / (num8 + 0.1f);
					}
				}
			}
		}
	}

	public void UpdateMapFilter(bool force)
	{
		if (!force)
		{
			updateMapFilterTimer += Time.deltaTime;
			if (updateMapFilterTimer < 1f)
			{
				return;
			}
			updateMapFilterTimer = 0f;
		}
		for (int i = 0; i < mapSizeX; i++)
		{
			for (int j = 0; j < mapSizeY; j++)
			{
				mapWaerme[i, j] = 0f;
				mapAusstattung[i, j] = 0f;
			}
		}
		for (int k = 0; k < mapSizeX; k++)
		{
			for (int l = 0; l < mapSizeY; l++)
			{
				if (mapWindows[k, l] <= 0)
				{
					continue;
				}
				int num = k;
				int num2 = l;
				if (mapBuilding[k, l] == 0)
				{
					if (mapWindows[k, l] == 1180)
					{
						num--;
					}
					if (mapWindows[k, l] == 1000)
					{
						num++;
					}
					if (mapWindows[k, l] == 1090)
					{
						num2--;
					}
					if (mapWindows[k, l] == 1270)
					{
						num2++;
					}
				}
				if (IsInMapLimit(num, num2) && mS_.IsMyBuilding(mapBuilding[num, num2]))
				{
					mapAusstattung[num, num2] += 0.4f;
				}
				for (int m = num - 3; m < num + 3; m++)
				{
					for (int n = num2 - 3; n < num2 + 3; n++)
					{
						float num3 = Vector2.Distance(new Vector2(num, num2), new Vector2(m, n));
						if (num3 < 8f && num3 > 0f && IsInMapLimit(m, n) && IsInMapLimit(k, l) && mapRoomID[num, num2] == mapRoomID[m, n] && mapBuilding[num, num2] == mapBuilding[m, n] && mS_.IsMyBuilding(mapBuilding[m, n]))
						{
							mapAusstattung[m, n] += 0.4f / (num3 + 0.1f);
						}
					}
				}
			}
		}
		for (int num4 = 0; num4 < mS_.arrayObjectScripts.Length; num4++)
		{
			if (!mS_.arrayObjectScripts[num4] || (!(mS_.arrayObjectScripts[num4].kaelte > 0f) && !(mS_.arrayObjectScripts[num4].ausstattung > 0f) && !(mS_.arrayObjectScripts[num4].waerme > 0f)))
			{
				continue;
			}
			objectScript objectScript2 = mS_.arrayObjectScripts[num4];
			if (!(objectScript2.transform.position.y < 1000f))
			{
				continue;
			}
			if (objectScript2.waerme > 0f || objectScript2.kaelte > 0f)
			{
				float num5 = objectScript2.waerme - objectScript2.kaelte;
				if (objectScript2.kaelte == 5f)
				{
					num5 -= 3f;
				}
				int num6 = 8;
				if (objectScript2.waerme == 20f)
				{
					num6 = 16;
				}
				int num7 = Mathf.RoundToInt(mS_.arrayObjects[num4].transform.position.x);
				int num8 = Mathf.RoundToInt(mS_.arrayObjects[num4].transform.position.z);
				if (IsInMapLimit(num7, num8))
				{
					mapWaerme[num7, num8] += num5 * 0.2f;
				}
				for (int num9 = num7 - num6; num9 < num7 + num6; num9++)
				{
					for (int num10 = num8 - num6; num10 < num8 + num6; num10++)
					{
						float num11 = Vector2.Distance(new Vector2(num7, num8), new Vector2(num9, num10));
						if (num11 < (float)num6 && num11 > 0f && IsInMapLimit(num9, num10) && IsInMapLimit(num7, num8) && mapRoomID[num7, num8] == mapRoomID[num9, num10] && mapBuilding[num7, num8] == mapBuilding[num9, num10])
						{
							mapWaerme[num9, num10] += num5 * 0.2f / (num11 + 0.1f);
						}
					}
				}
			}
			if (!(objectScript2.ausstattung > 0f))
			{
				continue;
			}
			float ausstattung = objectScript2.ausstattung;
			int num12 = Mathf.RoundToInt(mS_.arrayObjects[num4].transform.position.x);
			int num13 = Mathf.RoundToInt(mS_.arrayObjects[num4].transform.position.z);
			if (IsInMapLimit(num12, num13))
			{
				mapAusstattung[num12, num13] += ausstattung * 0.2f;
			}
			for (int num14 = num12 - 8; num14 < num12 + 8; num14++)
			{
				for (int num15 = num13 - 8; num15 < num13 + 8; num15++)
				{
					float num16 = Vector2.Distance(new Vector2(num12, num13), new Vector2(num14, num15));
					if (num16 < 8f && num16 > 0f && IsInMapLimit(num14, num15) && IsInMapLimit(num12, num13) && mapRoomID[num12, num13] == mapRoomID[num14, num15] && mapBuilding[num12, num13] == mapBuilding[num14, num15])
					{
						mapAusstattung[num14, num15] += ausstattung * 0.2f / (num16 + 0.1f);
					}
				}
			}
		}
	}
}
