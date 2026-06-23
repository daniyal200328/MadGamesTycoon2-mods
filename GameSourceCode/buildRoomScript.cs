using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;

public class buildRoomScript : MonoBehaviour
{
	private mainScript mS_;

	private mapScript mapScript_;

	private GUI_Main guiMain;

	private Camera myCamera;

	private Seeker seeker;

	private mainCameraScript mCamS_;

	private sfxScript sfx_;

	private roomDataScript rdS_;

	private autoInventarScript autoInventar_;

	private Menu_BuildRoom menuBuildRoom_;

	public bool activ;

	public GameObject[] pointers;

	public GameObject[] pointersInstantiate;

	public GameObject[] prefabRoomElements;

	public GameObject[] prefabParticles;

	public GameObject roomMainObject;

	public int posX;

	public int posY;

	public int modus;

	public bool noPath;

	public int replaceRoomID = -1;

	public int moveRoomID = -1;

	private bool error;

	private bool errorDoor;

	private bool errorWindow;

	public int moneyRedesign;

	public int[,] mapOld;

	private int[,] mapNew;

	public int[,] mapDoors;

	public int[,] mapWindows;

	private int[,] mapRemove;

	private int[,] mapMove;

	private bool[,] doorsPlaced;

	private GameObject[,] mapRoomGO;

	public int roomStartX;

	public int roomStartY;

	private bool makeUpdate;

	private RaycastHit hit;

	private GameObject roomVorschau;

	private Vector3[] endVectors;

	private void Awake()
	{
		mapOld = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		mapNew = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		mapDoors = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		mapWindows = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		mapRemove = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		mapRoomGO = new GameObject[mapScript.mapSizeX, mapScript.mapSizeY];
		doorsPlaced = new bool[mapScript.mapSizeX, mapScript.mapSizeY];
	}

	private void Start()
	{
		FindScripts();
		InitPointers();
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
		if (!seeker)
		{
			seeker = GetComponent<Seeker>();
		}
		if (!mCamS_)
		{
			mCamS_ = GameObject.Find("Camera").GetComponent<mainCameraScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!autoInventar_)
		{
			autoInventar_ = GetComponent<autoInventarScript>();
		}
		if (!menuBuildRoom_)
		{
			menuBuildRoom_ = guiMain.uiObjects[19].GetComponent<Menu_BuildRoom>();
		}
	}

	private void InitPointers()
	{
		for (int i = 0; i < pointersInstantiate.Length; i++)
		{
			if (!pointersInstantiate[i])
			{
				pointersInstantiate[i] = Object.Instantiate(pointers[i]);
			}
		}
		DisableAllPointers();
	}

	public void DisableAllPointers()
	{
		for (int i = 0; i < pointersInstantiate.Length; i++)
		{
			if ((bool)pointersInstantiate[i])
			{
				pointersInstantiate[i].SetActive(value: false);
			}
		}
	}

	private void Update()
	{
		if (activ)
		{
			if (guiMain.IsMouseOverGUI())
			{
				DisableAllPointers();
			}
			GetMousePosition();
			ResizeRoom();
			SetDoor();
			SetWindow();
			CreateRoomVorschau();
			SetPointer();
		}
	}

	public void SetActive()
	{
		activ = true;
		if (!mCamS_.additionalCamera[0].activeSelf)
		{
			mCamS_.additionalCamera[0].SetActive(value: true);
		}
	}

	public void SetInactive()
	{
		activ = false;
		DisableAllPointers();
		DeleteComplete();
		if (mCamS_.additionalCamera[0].activeSelf)
		{
			mCamS_.additionalCamera[0].SetActive(value: false);
		}
	}

	private void GetMousePosition()
	{
		makeUpdate = false;
		if (guiMain.IsMouseOverGUI())
		{
			return;
		}
		int layerMask = 0;
		if (modus == 0 || modus == 1 || modus == 4)
		{
			layerMask = 512;
		}
		if (modus == 2)
		{
			layerMask = 1024;
		}
		if (modus == 3)
		{
			layerMask = 1024;
		}
		if (Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out hit, 200f, layerMask))
		{
			if ((modus == 0 || modus == 1) && Input.GetMouseButton(0) && (posX != Mathf.RoundToInt(hit.transform.position.x) || posY != Mathf.RoundToInt(hit.transform.position.z)))
			{
				makeUpdate = true;
			}
			posX = Mathf.RoundToInt(hit.transform.position.x);
			posY = Mathf.RoundToInt(hit.transform.position.z);
		}
	}

	private void SetPointer()
	{
		if (guiMain.IsMouseOverGUI())
		{
			return;
		}
		for (int i = 0; i < pointersInstantiate.Length; i++)
		{
			if ((bool)pointersInstantiate[i])
			{
				Vector3 position = pointersInstantiate[i].transform.position;
				position = Vector3.Lerp(position, new Vector3(posX, -0.4f, posY), 0.3f);
				pointersInstantiate[i].transform.position = position;
				if ((double)Vector3.Distance(position, new Vector3(posX, -0.4f, posY)) > 2.0)
				{
					pointersInstantiate[i].transform.position = new Vector3(posX, -0.4f, posY);
				}
				if (i == 3)
				{
					pointersInstantiate[i].transform.position = new Vector3(posX, -0.4f, posY);
				}
			}
		}
		switch (modus)
		{
		case 0:
			DisableAllPointers(0, -1, -1);
			if (GetMergedMap(posX, posY) == 0)
			{
				if (!pointersInstantiate[0].activeSelf)
				{
					pointersInstantiate[0].SetActive(value: true);
				}
			}
			else if (pointersInstantiate[0].activeSelf)
			{
				pointersInstantiate[0].SetActive(value: false);
			}
			break;
		case 1:
			DisableAllPointers(1, -1, -1);
			if (!pointersInstantiate[1].activeSelf)
			{
				pointersInstantiate[1].SetActive(value: true);
			}
			break;
		case 2:
			DisableAllPointers(2, 3, -1);
			if ((bool)hit.transform)
			{
				if (!errorDoor)
				{
					mCamS_.SetOutlineColor(0, 1f, 3);
					if (!pointersInstantiate[2].activeSelf)
					{
						pointersInstantiate[2].SetActive(value: true);
					}
					if (pointersInstantiate[3].activeSelf)
					{
						pointersInstantiate[3].SetActive(value: false);
					}
					pointersInstantiate[2].transform.localEulerAngles = new Vector3(90f, hit.transform.localEulerAngles.y, 0f);
				}
				else
				{
					mCamS_.SetOutlineColor(1, 1f, 3);
					if (pointersInstantiate[2].activeSelf)
					{
						pointersInstantiate[2].SetActive(value: false);
					}
					if (!pointersInstantiate[3].activeSelf)
					{
						pointersInstantiate[3].SetActive(value: true);
					}
					pointersInstantiate[3].transform.localEulerAngles = new Vector3(90f, hit.transform.localEulerAngles.y, 0f);
				}
			}
			else
			{
				if (pointersInstantiate[2].activeSelf)
				{
					pointersInstantiate[2].SetActive(value: false);
				}
				if (pointersInstantiate[3].activeSelf)
				{
					pointersInstantiate[3].SetActive(value: false);
				}
			}
			break;
		case 3:
			DisableAllPointers(4, 5, -1);
			if ((bool)hit.transform)
			{
				if (!Input.GetKey(KeyCode.LeftShift))
				{
					if (!errorWindow)
					{
						if (mapWindows[posX, posY] <= 0)
						{
							mCamS_.SetOutlineColor(0, 1f, 3);
							if (!pointersInstantiate[4].activeSelf)
							{
								pointersInstantiate[4].SetActive(value: true);
							}
							if (pointersInstantiate[5].activeSelf)
							{
								pointersInstantiate[5].SetActive(value: false);
							}
							pointersInstantiate[4].transform.localEulerAngles = new Vector3(90f, hit.transform.localEulerAngles.y, 0f);
						}
						else
						{
							if (pointersInstantiate[4].activeSelf)
							{
								pointersInstantiate[4].SetActive(value: false);
							}
							if (pointersInstantiate[5].activeSelf)
							{
								pointersInstantiate[5].SetActive(value: false);
							}
						}
					}
					else
					{
						mCamS_.SetOutlineColor(1, 1f, 3);
						if (pointersInstantiate[4].activeSelf)
						{
							pointersInstantiate[4].SetActive(value: false);
						}
						if (!pointersInstantiate[5].activeSelf)
						{
							pointersInstantiate[5].SetActive(value: true);
						}
						pointersInstantiate[5].transform.localEulerAngles = new Vector3(90f, hit.transform.localEulerAngles.y, 0f);
					}
				}
				else if (mapWindows[posX, posY] > 0)
				{
					mCamS_.SetOutlineColor(1, 1f, 3);
					if (!pointersInstantiate[4].activeSelf)
					{
						pointersInstantiate[4].SetActive(value: true);
					}
					if (pointersInstantiate[5].activeSelf)
					{
						pointersInstantiate[5].SetActive(value: false);
					}
					pointersInstantiate[4].transform.localEulerAngles = new Vector3(90f, hit.transform.localEulerAngles.y, 0f);
					pointersInstantiate[4].transform.position = new Vector3(posX, -0.5f, posY);
				}
				else
				{
					if (pointersInstantiate[4].activeSelf)
					{
						pointersInstantiate[4].SetActive(value: false);
					}
					if (pointersInstantiate[5].activeSelf)
					{
						pointersInstantiate[5].SetActive(value: false);
					}
				}
			}
			else
			{
				if (pointersInstantiate[4].activeSelf)
				{
					pointersInstantiate[4].SetActive(value: false);
				}
				if (pointersInstantiate[5].activeSelf)
				{
					pointersInstantiate[5].SetActive(value: false);
				}
			}
			break;
		}
	}

	private void DisableAllPointers(int a, int b, int c)
	{
		for (int i = 0; i < pointersInstantiate.Length; i++)
		{
			if (i != a && i != b && i != c && pointersInstantiate[i].activeSelf)
			{
				pointersInstantiate[i].SetActive(value: false);
			}
		}
	}

	private void SetDoor()
	{
		if (modus != 2)
		{
			return;
		}
		if (Input.GetKeyUp(KeyCode.Delete))
		{
			sfx_.PlaySound(1, force: true);
			for (int i = 0; i < mapScript.mapSizeX; i++)
			{
				for (int j = 0; j < mapScript.mapSizeY; j++)
				{
					mapDoors[i, j] = 0;
				}
			}
			makeUpdate = true;
		}
		else
		{
			if (!hit.transform || guiMain.IsMouseOverGUI())
			{
				return;
			}
			errorDoor = false;
			if (mapScript_.mapDoors[posX, posY] > 0 || mapScript_.mapWindows[posX, posY] > 0)
			{
				errorDoor = true;
				return;
			}
			switch (Mathf.RoundToInt(hit.transform.eulerAngles.y))
			{
			case 180:
				if (!OutOfMap(posX - 1, posY))
				{
					if (mapScript_.mapRoomID[posX - 1, posY] == 0)
					{
						errorDoor = true;
					}
					if (mapScript_.mapRoomID[posX - 1, posY] > 1)
					{
						errorDoor = true;
					}
				}
				break;
			case 0:
				if (!OutOfMap(posX + 1, posY))
				{
					if (mapScript_.mapRoomID[posX + 1, posY] == 0)
					{
						errorDoor = true;
					}
					if (mapScript_.mapRoomID[posX + 1, posY] > 1)
					{
						errorDoor = true;
					}
				}
				break;
			case 90:
				if (!OutOfMap(posX, posY - 1))
				{
					if (mapScript_.mapRoomID[posX, posY - 1] == 0)
					{
						errorDoor = true;
					}
					if (mapScript_.mapRoomID[posX, posY - 1] > 1)
					{
						errorDoor = true;
					}
				}
				break;
			case 270:
				if (!OutOfMap(posX, posY + 1))
				{
					if (mapScript_.mapRoomID[posX, posY + 1] == 0)
					{
						errorDoor = true;
					}
					if (mapScript_.mapRoomID[posX, posY + 1] > 1)
					{
						errorDoor = true;
					}
				}
				break;
			}
			if (errorDoor || !Input.GetMouseButtonUp(0))
			{
				return;
			}
			sfx_.PlaySound(0, force: true);
			for (int k = 0; k < mapScript.mapSizeX; k++)
			{
				for (int l = 0; l < mapScript.mapSizeY; l++)
				{
					mapDoors[k, l] = 0;
				}
			}
			mapWindows[posX, posY] = 0;
			mapDoors[posX, posY] = Mathf.RoundToInt(hit.transform.localEulerAngles.y + 1000f);
			makeUpdate = true;
			Debug.Log("Set Door: " + posX + "," + posY + " / Rot: " + mapDoors[posX, posY]);
		}
	}

	private void SetWindow()
	{
		if (modus != 3 || !hit.transform || guiMain.IsMouseOverGUI())
		{
			return;
		}
		errorWindow = false;
		if (mapScript_.mapDoors[posX, posY] > 0 || mapScript_.mapWindows[posX, posY] > 0)
		{
			errorWindow = true;
			return;
		}
		switch (Mathf.RoundToInt(hit.transform.eulerAngles.y))
		{
		case 180:
			if (!OutOfMap(posX - 1, posY) && mapScript_.mapRoomID[posX - 1, posY] == 0)
			{
				errorWindow = true;
			}
			break;
		case 0:
			if (!OutOfMap(posX + 1, posY) && mapScript_.mapRoomID[posX + 1, posY] == 0)
			{
				errorWindow = true;
			}
			break;
		case 90:
			if (!OutOfMap(posX, posY - 1) && mapScript_.mapRoomID[posX, posY - 1] == 0)
			{
				errorWindow = true;
			}
			break;
		case 270:
			if (!OutOfMap(posX, posY + 1) && mapScript_.mapRoomID[posX, posY + 1] == 0)
			{
				errorWindow = true;
			}
			break;
		}
		if (errorWindow || !Input.GetMouseButtonUp(0))
		{
			return;
		}
		if (!Input.GetKey(KeyCode.LeftShift))
		{
			if (mapWindows[posX, posY] <= 0)
			{
				sfx_.PlaySound(0, force: true);
				makeUpdate = true;
				mapWindows[posX, posY] = Mathf.RoundToInt(hit.transform.localEulerAngles.y + 1000f);
			}
		}
		else if (mapWindows[posX, posY] > 0)
		{
			sfx_.PlaySound(1, force: true);
			mapWindows[posX, posY] = 0;
			makeUpdate = true;
		}
		Debug.Log("Set Window: " + posX + "," + posY + " / Rot: " + mapWindows[posX, posY]);
	}

	private void RemoveWindow(int i)
	{
		for (int j = 0; j < mapScript.mapSizeX; j++)
		{
			for (int k = 0; k < mapScript.mapSizeY; k++)
			{
				if (mapWindows[j, k] == i)
				{
					Debug.Log("Remove Window: " + mapWindows[j, k]);
					mapWindows[j, k] = 0;
				}
			}
		}
	}

	private void RemoveDoor(int i)
	{
		for (int j = 0; j < mapScript.mapSizeX; j++)
		{
			for (int k = 0; k < mapScript.mapSizeY; k++)
			{
				if (mapDoors[j, k] == i)
				{
					Debug.Log("Remove Door: " + mapDoors[j, k]);
					mapDoors[j, k] = 0;
				}
			}
		}
	}

	private void ResizeRoom()
	{
		if (modus != 0 && modus != 1)
		{
			return;
		}
		if (!guiMain.IsMouseOverGUI())
		{
			if (Input.GetMouseButtonDown(0))
			{
				roomStartX = posX;
				roomStartY = posY;
				makeUpdate = true;
				Debug.Log(roomStartX + ", " + roomStartY + "/ ID: " + mapScript_.mapRoomID[posX, posY] + " DOOR: " + mapScript_.mapDoors[posX, posY] + " WINDOW: " + mapScript_.mapWindows[posX, posY]);
			}
			if (Input.GetMouseButton(0) && roomStartX > 0 && roomStartX < mapScript.mapSizeX && roomStartY > 0 && roomStartY < mapScript.mapSizeY)
			{
				for (int i = 0; i < mapScript.mapSizeX; i++)
				{
					for (int j = 0; j < mapScript.mapSizeY; j++)
					{
						if (modus == 0)
						{
							mapNew[i, j] = 0;
						}
						if (modus == 1)
						{
							mapRemove[i, j] = 0;
						}
					}
				}
				error = false;
				bool flag = ExistRoom();
				bool flag2 = false;
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = 0;
				if (roomStartX < posX)
				{
					num = roomStartX;
					num3 = posX;
				}
				else
				{
					num = posX;
					num3 = roomStartX;
				}
				if (roomStartY < posY)
				{
					num2 = roomStartY;
					num4 = posY;
				}
				else
				{
					num2 = posY;
					num4 = roomStartY;
				}
				for (int k = num; k <= num3; k++)
				{
					for (int l = num2; l <= num4; l++)
					{
						switch (modus)
						{
						case 0:
							mapNew[k, l] = 1;
							if (!mS_.IsMyBuilding(mapScript_.mapBuilding[k, l]))
							{
								error = true;
								Debug.Log("NCIHT MEH");
							}
							if (mapScript_.mapBlock[k, l] != 0)
							{
								error = true;
							}
							if (mapScript_.mapRoomID[k, l] != 1)
							{
								error = true;
							}
							if (mapScript_.mapDoors[k, l] > 0)
							{
								error = true;
							}
							if (!flag2 && flag && (mapOld[k - 1, l] == 1 || mapOld[k + 1, l] == 1 || mapOld[k, l - 1] == 1 || mapOld[k, l + 1] == 1))
							{
								flag2 = true;
							}
							break;
						case 1:
							mapRemove[k, l] = 1;
							break;
						}
					}
				}
				if (modus == 1 && ErrorCut())
				{
					error = true;
				}
				if (modus == 0 && flag && !flag2)
				{
					error = true;
				}
			}
		}
		if (!Input.GetMouseButtonUp(0))
		{
			return;
		}
		if (guiMain.IsMouseOverGUI())
		{
			error = true;
		}
		if (!error)
		{
			if (modus == 0)
			{
				sfx_.PlaySound(0, force: true);
			}
			if (modus == 1)
			{
				sfx_.PlaySound(1, force: true);
			}
		}
		else if (!guiMain.IsMouseOverGUI())
		{
			sfx_.PlaySound(2, force: true);
		}
		for (int m = 0; m < mapScript.mapSizeX; m++)
		{
			for (int n = 0; n < mapScript.mapSizeY; n++)
			{
				switch (modus)
				{
				case 0:
					if (error)
					{
						mapNew[m, n] = 0;
					}
					else if (mapNew[m, n] > 0)
					{
						mapWindows[m, n] = 0;
						mapOld[m, n] = 1;
						mapNew[m, n] = 0;
					}
					break;
				case 1:
					if (error)
					{
						mapRemove[m, n] = 0;
					}
					else if (mapRemove[m, n] == 1)
					{
						if (mapOld[m, n] > 0)
						{
							mapOld[m, n] = 0;
							mapDoors[m, n] = 0;
							mapWindows[m, n] = 0;
						}
						mapRemove[m, n] = 0;
					}
					break;
				}
			}
		}
		error = false;
		makeUpdate = true;
	}

	public void CreateRoomVorschau()
	{
		if (modus == 4 || !makeUpdate)
		{
			return;
		}
		doorsPlaced = new bool[mapScript.mapSizeX, mapScript.mapSizeY];
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if ((bool)mapRoomGO[i, j])
				{
					Object.Destroy(mapRoomGO[i, j]);
				}
				if (mapNew[i, j] <= 0 && mapOld[i, j] <= 0 && mapRemove[i, j] <= 0)
				{
					continue;
				}
				GameObject gameObject = null;
				if (mapRemove[i, j] <= 0)
				{
					if (!error)
					{
						mapRoomGO[i, j] = Object.Instantiate(prefabRoomElements[0], new Vector3(i, 0f, j), Quaternion.identity);
					}
					else
					{
						mapRoomGO[i, j] = Object.Instantiate(prefabRoomElements[7], new Vector3(i, 0f, j), Quaternion.identity);
					}
				}
				if (mapRemove[i, j] > 0 && mapOld[i, j] > 0)
				{
					mapRoomGO[i, j] = Object.Instantiate(prefabRoomElements[4], new Vector3(i, 0f, j), Quaternion.identity);
				}
				if (mapRemove[i, j] > 0 && mapOld[i, j] <= 0)
				{
					mapRoomGO[i, j] = Object.Instantiate(prefabRoomElements[6], new Vector3(i, 0f, j), Quaternion.identity);
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				bool flag4 = false;
				if (mapDoors[i, j] > 0 && mapRemove[i, j] == 0)
				{
					if (mapDoors[i, j] > 0)
					{
						if ((mapDoors[i, j] - 1000 == 180 && mapScript_.mapRoomID[i - 1, j] == mapScript.ID_FLOOR && mapNew[i - 1, j] == 0 && mapOld[i - 1, j] == 0) || (mapDoors[i, j] - 1000 == 0 && mapScript_.mapRoomID[i + 1, j] == mapScript.ID_FLOOR && mapNew[i + 1, j] == 0 && mapOld[i + 1, j] == 0) || (mapDoors[i, j] - 1000 == 270 && mapScript_.mapRoomID[i, j + 1] == mapScript.ID_FLOOR && mapNew[i, j + 1] == 0 && mapOld[i, j + 1] == 0) || (mapDoors[i, j] - 1000 == 90 && mapScript_.mapRoomID[i, j - 1] == mapScript.ID_FLOOR && mapNew[i, j - 1] == 0 && mapOld[i, j - 1] == 0))
						{
							gameObject = Object.Instantiate(prefabRoomElements[9], new Vector3(i, 0f, j), Quaternion.identity);
							gameObject.transform.eulerAngles = new Vector3(0f, mapDoors[i, j] - 1000, 0f);
							gameObject.transform.SetParent(mapRoomGO[i, j].transform);
							doorsPlaced[i, j] = true;
							switch (mapDoors[i, j] - 1000)
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
						else
						{
							mapDoors[i, j] = 0;
							doorsPlaced[i, j] = false;
						}
					}
					if (Input.GetMouseButtonUp(0) && !doorsPlaced[i, j])
					{
						mapDoors[i, j] = 0;
						Debug.Log("RemoveDoor " + Random.Range(0, 10000));
					}
				}
				bool flag5 = false;
				bool flag6 = false;
				bool flag7 = false;
				bool flag8 = false;
				if (mapWindows[i, j] > 0 && mapRemove[i, j] == 0 && mapWindows[i, j] > 0)
				{
					switch (mapWindows[i, j] - 1000)
					{
					case 0:
						if (GetMergedMap(i + 1, j) != 0)
						{
							mapWindows[i, j] = 0;
						}
						break;
					case 90:
						if (GetMergedMap(i, j - 1) != 0)
						{
							mapWindows[i, j] = 0;
						}
						break;
					case 180:
						if (GetMergedMap(i - 1, j) != 0)
						{
							mapWindows[i, j] = 0;
						}
						break;
					case 270:
						if (GetMergedMap(i, j + 1) != 0)
						{
							mapWindows[i, j] = 0;
						}
						break;
					}
					if (mapWindows[i, j] > 0)
					{
						gameObject = Object.Instantiate(prefabRoomElements[10], new Vector3(i, 0f, j), Quaternion.identity);
						gameObject.transform.eulerAngles = new Vector3(0f, mapWindows[i, j] - 1000, 0f);
						gameObject.transform.SetParent(mapRoomGO[i, j].transform);
						switch (mapWindows[i, j] - 1000)
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
				}
				if (GetMergedMap(i, j) > 0)
				{
					if (GetMergedMap(i + 1, j) != GetMergedMap(i, j) && !flag && !flag5)
					{
						if (error)
						{
							gameObject = Object.Instantiate(prefabRoomElements[8], new Vector3(i, 0f, j), Quaternion.identity);
						}
						else
						{
							if (mapRemove[i, j] <= 0)
							{
								gameObject = Object.Instantiate(prefabRoomElements[1], new Vector3(i, 0f, j), Quaternion.identity);
							}
							if (mapRemove[i, j] > 0)
							{
								gameObject = Object.Instantiate(prefabRoomElements[5], new Vector3(i, 0f, j), Quaternion.identity);
							}
						}
						gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
						gameObject.transform.SetParent(mapRoomGO[i, j].transform);
					}
					if (GetMergedMap(i - 1, j) != GetMergedMap(i, j) && !flag3 && !flag7)
					{
						if (error)
						{
							gameObject = Object.Instantiate(prefabRoomElements[8], new Vector3(i, 0f, j), Quaternion.identity);
						}
						else
						{
							if (mapRemove[i, j] <= 0)
							{
								gameObject = Object.Instantiate(prefabRoomElements[1], new Vector3(i, 0f, j), Quaternion.identity);
							}
							if (mapRemove[i, j] > 0)
							{
								gameObject = Object.Instantiate(prefabRoomElements[5], new Vector3(i, 0f, j), Quaternion.identity);
							}
						}
						gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
						gameObject.transform.SetParent(mapRoomGO[i, j].transform);
					}
					if (GetMergedMap(i, j + 1) != GetMergedMap(i, j) && !flag4 && !flag8)
					{
						if (error)
						{
							gameObject = Object.Instantiate(prefabRoomElements[8], new Vector3(i, 0f, j), Quaternion.identity);
						}
						else
						{
							if (mapRemove[i, j] <= 0)
							{
								gameObject = Object.Instantiate(prefabRoomElements[1], new Vector3(i, 0f, j), Quaternion.identity);
							}
							if (mapRemove[i, j] > 0)
							{
								gameObject = Object.Instantiate(prefabRoomElements[5], new Vector3(i, 0f, j), Quaternion.identity);
							}
						}
						gameObject.transform.eulerAngles = new Vector3(0f, 270f, 0f);
						gameObject.transform.SetParent(mapRoomGO[i, j].transform);
					}
					if (GetMergedMap(i, j - 1) != GetMergedMap(i, j) && !flag2 && !flag6)
					{
						if (error)
						{
							gameObject = Object.Instantiate(prefabRoomElements[8], new Vector3(i, 0f, j), Quaternion.identity);
						}
						else
						{
							if (mapRemove[i, j] <= 0)
							{
								gameObject = Object.Instantiate(prefabRoomElements[1], new Vector3(i, 0f, j), Quaternion.identity);
							}
							if (mapRemove[i, j] > 0)
							{
								gameObject = Object.Instantiate(prefabRoomElements[5], new Vector3(i, 0f, j), Quaternion.identity);
							}
						}
						gameObject.transform.eulerAngles = new Vector3(0f, 90f, 0f);
						gameObject.transform.SetParent(mapRoomGO[i, j].transform);
					}
				}
				if (GetMergedMap(i, j) > 0)
				{
					if (GetMergedMap(i, j - 1) == GetMergedMap(i, j) && GetMergedMap(i - 1, j) == GetMergedMap(i, j) && GetMergedMap(i - 1, j - 1) != GetMergedMap(i, j))
					{
						gameObject = Object.Instantiate(prefabRoomElements[3], new Vector3(i, 0f, j), Quaternion.identity);
						gameObject.transform.eulerAngles = new Vector3(0f, 180f, 0f);
						gameObject.transform.SetParent(mapRoomGO[i, j].transform);
					}
					if (GetMergedMap(i, j - 1) == GetMergedMap(i, j) && GetMergedMap(i + 1, j) == GetMergedMap(i, j) && GetMergedMap(i + 1, j - 1) != GetMergedMap(i, j))
					{
						gameObject = Object.Instantiate(prefabRoomElements[3], new Vector3(i, 0f, j), Quaternion.identity);
						gameObject.transform.eulerAngles = new Vector3(0f, 90f, 0f);
						gameObject.transform.SetParent(mapRoomGO[i, j].transform);
					}
					if (GetMergedMap(i, j + 1) == GetMergedMap(i, j) && GetMergedMap(i - 1, j) == GetMergedMap(i, j) && GetMergedMap(i - 1, j + 1) != GetMergedMap(i, j))
					{
						gameObject = Object.Instantiate(prefabRoomElements[3], new Vector3(i, 0f, j), Quaternion.identity);
						gameObject.transform.eulerAngles = new Vector3(0f, 270f, 0f);
						gameObject.transform.SetParent(mapRoomGO[i, j].transform);
					}
					if (GetMergedMap(i, j + 1) == GetMergedMap(i, j) && GetMergedMap(i + 1, j) == GetMergedMap(i, j) && GetMergedMap(i + 1, j + 1) != GetMergedMap(i, j))
					{
						gameObject = Object.Instantiate(prefabRoomElements[3], new Vector3(i, 0f, j), Quaternion.identity);
						gameObject.transform.eulerAngles = new Vector3(0f, 0f, 0f);
						gameObject.transform.SetParent(mapRoomGO[i, j].transform);
					}
				}
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			Debug.Log("MakePathfindUpdate");
			StartCoroutine(UpdatePathfindingNextFrame());
		}
		else
		{
			sfx_.PlaySound(36, force: true);
		}
	}

	private IEnumerator UpdatePathfindingNextFrame()
	{
		noPath = true;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if ((bool)mapScript_.aStar_)
		{
			mapScript_.aStar_.Scan();
		}
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		noPath = false;
		bool flag = true;
		int num = 0;
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mS_.IsMyBuilding(mapScript_.mapBuilding[i, j]) && mapScript_.mapBuilding[i, j] != mapScript.ID_FLOOROUTSIDE)
				{
					if (mapDoors[i, j] > 0)
					{
						flag = false;
					}
					if (mapDoors[i, j] > 0 || mapScript_.mapDoors[i, j] > 0)
					{
						num++;
					}
				}
			}
		}
		if (flag)
		{
			noPath = true;
			yield break;
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("BuildingDoor");
		endVectors = new Vector3[num];
		Debug.Log("AmountDoors: " + endVectors.Length);
		int num2 = 0;
		for (int k = 0; k < mapScript.mapSizeX; k++)
		{
			for (int l = 0; l < mapScript.mapSizeY; l++)
			{
				if (mS_.IsMyBuilding(mapScript_.mapBuilding[k, l]) && mapScript_.mapBuilding[k, l] != mapScript.ID_FLOOROUTSIDE && (mapScript_.mapDoors[k, l] > 0 || mapDoors[k, l] > 0))
				{
					if (mapScript_.mapDoors[k, l] > 0)
					{
						endVectors[num2] = new Vector3(k, 0f, l);
					}
					else
					{
						endVectors[num2] = new Vector3(k, 0f, l);
					}
					num2++;
				}
			}
		}
		seeker.StartMultiTargetPath(gameObject.transform.position, endVectors, pathsForAll: true, OnPathComplete);
	}

	public void OnPathComplete(Path p)
	{
		Debug.Log("Got Callback");
		if (p.error)
		{
			Debug.Log("Ouch, the path returned an error\nError: " + p.errorLog);
			noPath = true;
			return;
		}
		if (!(p is MultiTargetPath multiTargetPath))
		{
			Debug.LogError("The Path was no MultiTargetPath");
			noPath = true;
			return;
		}
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].typ != 16)
			{
				mS_.arrayRoomScripts[i].SetListGameObjectsLayer(15);
			}
		}
		List<Vector3>[] vectorPaths = multiTargetPath.vectorPaths;
		for (int j = 0; j < vectorPaths.Length; j++)
		{
			List<Vector3> list = vectorPaths[j];
			if (list == null)
			{
				Debug.Log("Path number " + j + " could not be found");
				noPath = true;
				continue;
			}
			int num = Mathf.RoundToInt(list[list.Count - 1].x);
			int num2 = Mathf.RoundToInt(list[list.Count - 1].z);
			if ((bool)mapScript_.mapRoomScript[num, num2] && mapScript_.mapRoomScript[num, num2].typ != 16)
			{
				mapScript_.mapRoomScript[num, num2].SetListGameObjectsLayer(0);
			}
		}
	}

	public bool IsDoor()
	{
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapDoors[i, j] > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void CreateRoom(int typ_, int price)
	{
		GameObject gameObject = null;
		int num = Random.Range(100, 999999999);
		if (replaceRoomID == -1)
		{
			gameObject = Object.Instantiate(roomMainObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
			gameObject.name = "Room_" + num;
		}
		else
		{
			gameObject = GameObject.Find("Room_" + replaceRoomID);
			num = replaceRoomID;
		}
		roomScript component = gameObject.GetComponent<roomScript>();
		component.listGameObjects.Clear();
		component.myID = num;
		component.typ = typ_;
		component.uiPos = FindUiPosition();
		gameObject.transform.position = new Vector3(component.uiPos.x, 0f, component.uiPos.z);
		float num2 = 0f;
		Vector3 vector = new Vector3(-1f, -1f, -1f);
		int buildingID = -1;
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapOld[i, j] != 0)
				{
					if (Random.Range(0, 100) > 90 || vector == new Vector3(-1f, -1f, -1f))
					{
						vector = new Vector3(i, 1f, j);
					}
					StartCoroutine(CreateParticle(i, j, num2));
					num2 += 0.005f;
				}
				if (mapOld[i, j] != 0)
				{
					mapScript_.mapRoomID[i, j] = num;
					mapScript_.mapRoomScript[i, j] = component;
					buildingID = mapScript_.mapBuilding[i, j];
				}
				if (mapDoors[i, j] != 0 && mapScript_.mapDoors[i, j] != 99)
				{
					mapScript_.mapDoors[i, j] = mapDoors[i, j];
				}
				if (mapWindows[i, j] != 0 && mapScript_.mapWindows[i, j] != 99)
				{
					mapScript_.mapWindows[i, j] = mapWindows[i, j];
				}
				if (mS_.multiplayer && (mapOld[i, j] != 0 || mapDoors[i, j] != 0 || mapWindows[i, j] != 0))
				{
					mS_.Multiplayer_SendMap(i, j);
				}
			}
		}
		mapScript_.CreateWalls(buildingID);
		guiMain.MoneyPop(price, vector, green: false);
		mS_.Pay(price, 0);
		if ((bool)menuBuildRoom_ && menuBuildRoom_.uiObjects[33].activeSelf && menuBuildRoom_.uiObjects[33].GetComponent<Toggle>().isOn)
		{
			autoInventar_.FillAutomaticInventar(component, menuBuildRoom_.autoInventar_Quality);
		}
		DeleteComplete();
		replaceRoomID = -1;
	}

	public float GetBiggestRoomQuad()
	{
		float num = 0f;
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapOld[i, j] != 0)
				{
					float num2 = QuaderSizeTest(i, j);
					if (num2 > num)
					{
						num = num2;
					}
				}
			}
		}
		return num;
	}

	public Vector3 FindUiPosition()
	{
		float num = 0f;
		float x = 0f;
		float z = 0f;
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapOld[i, j] != 0)
				{
					float num2 = QuaderSizeTest(i, j);
					if (num2 > num)
					{
						num = num2;
						x = (float)i + num2 * 0.5f - 0.5f;
						z = (float)j + num2 * 0.5f - 0.5f;
					}
				}
			}
		}
		return new Vector3(x, -0.5f, z);
	}

	public Vector3 FindUiPositionExtern(int id)
	{
		float num = 0f;
		float x = 0f;
		float z = 0f;
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapScript_.mapRoomID[i, j] == id)
				{
					float num2 = QuaderSizeTestExtern(i, j, id);
					if (num2 > num)
					{
						num = num2;
						x = (float)i + num2 * 0.5f - 0.5f;
						z = (float)j + num2 * 0.5f - 0.5f;
					}
				}
			}
		}
		return new Vector3(x, -0.5f, z);
	}

	private float QuaderSizeTest(int x, int y)
	{
		float result = 0f;
		for (int i = 1; i < 20 && QuaderSize(x, y, i); i++)
		{
			result = i;
		}
		return result;
	}

	private float QuaderSizeTestExtern(int x, int y, int id)
	{
		float result = 0f;
		for (int i = 1; i < 20 && QuaderSizeExtern(x, y, i, id); i++)
		{
			result = i;
		}
		return result;
	}

	private bool QuaderSize(int px, int py, int size)
	{
		for (int i = px; i < px + size; i++)
		{
			for (int j = py; j < py + size; j++)
			{
				if (!OutOfMap(i, j))
				{
					if (mapOld[i, j] == 0)
					{
						return false;
					}
					continue;
				}
				return false;
			}
		}
		return true;
	}

	private bool QuaderSizeExtern(int px, int py, int size, int id)
	{
		for (int i = px; i < px + size; i++)
		{
			for (int j = py; j < py + size; j++)
			{
				if (!OutOfMap(i, j))
				{
					if (mapScript_.mapRoomID[i, j] != id)
					{
						return false;
					}
					continue;
				}
				return false;
			}
		}
		return true;
	}

	private IEnumerator CreateParticle(int x, int y, float t)
	{
		yield return new WaitForSeconds(t);
		Object.Instantiate(prefabParticles[0], new Vector3(x, 0.5f, y), Quaternion.identity);
	}

	public IEnumerator CreateParticleDemolish(int x, int y, float t)
	{
		yield return new WaitForSeconds(t);
		Object.Instantiate(prefabParticles[0], new Vector3(x, 0.5f, y), Quaternion.identity);
	}

	public bool ExistRoom()
	{
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapOld[i, j] > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	private int GetMergedMap(int x, int y)
	{
		if (mapRemove[x, y] > 0)
		{
			return 0;
		}
		if (mapOld[x, y] > 0 || mapNew[x, y] > 0)
		{
			return 1;
		}
		return 0;
	}

	public int AmountTiles()
	{
		int num = 0;
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapRemove[i, j] <= 0 && (mapOld[i, j] > 0 || mapNew[i, j] > 0))
				{
					num++;
				}
			}
		}
		return num;
	}

	private bool ErrorCut()
	{
		bool flag = false;
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (!flag && mapOld[i, j] > 0 && mapRemove[i, j] <= 0)
				{
					mapOld[i, j] = 2;
					flag = true;
					break;
				}
			}
		}
		bool flag2 = false;
		int num = 0;
		while (!flag2)
		{
			bool flag3 = false;
			for (int k = 0; k < mapScript.mapSizeX; k++)
			{
				for (int l = 0; l < mapScript.mapSizeY; l++)
				{
					if (mapOld[k, l] == 2 && mapRemove[k, l] <= 0)
					{
						if (!OutOfMap(k - 1, l) && mapOld[k - 1, l] == 1 && mapRemove[k - 1, l] <= 0)
						{
							mapOld[k - 1, l] = 2;
							flag3 = true;
						}
						if (!OutOfMap(k + 1, l) && mapOld[k + 1, l] == 1 && mapRemove[k + 1, l] <= 0)
						{
							mapOld[k + 1, l] = 2;
							flag3 = true;
						}
						if (!OutOfMap(k, l - 1) && mapOld[k, l - 1] == 1 && mapRemove[k, l - 1] <= 0)
						{
							mapOld[k, l - 1] = 2;
							flag3 = true;
						}
						if (!OutOfMap(k, l + 1) && mapOld[k, l + 1] == 1 && mapRemove[k, l + 1] <= 0)
						{
							mapOld[k, l + 1] = 2;
							flag3 = true;
						}
					}
				}
			}
			if (!flag3)
			{
				flag2 = true;
			}
			num++;
			if (num > 100000)
			{
				flag2 = true;
			}
		}
		bool result = false;
		for (int m = 0; m < mapScript.mapSizeX; m++)
		{
			for (int n = 0; n < mapScript.mapSizeY; n++)
			{
				if (mapOld[m, n] > 0 && mapOld[m, n] != 2 && mapRemove[m, n] <= 0)
				{
					result = true;
				}
				if (mapOld[m, n] > 0)
				{
					mapOld[m, n] = 1;
				}
			}
		}
		return result;
	}

	private bool OutOfMap(int x, int y)
	{
		if (x < 0)
		{
			return true;
		}
		if (x >= mapScript.mapSizeX)
		{
			return true;
		}
		if (y < 0)
		{
			return true;
		}
		if (y >= mapScript.mapSizeY)
		{
			return true;
		}
		return false;
	}

	public void Remove_DeleteMap()
	{
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapRemove[i, j] != 0)
				{
					mapRemove[i, j] = 0;
					if ((bool)mapRoomGO[i, j])
					{
						Object.Destroy(mapRoomGO[i, j]);
					}
				}
			}
		}
		makeUpdate = true;
	}

	private void DeleteComplete()
	{
		Debug.Log("DeleteComplete()");
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				mapOld[i, j] = 0;
				mapNew[i, j] = 0;
				mapDoors[i, j] = 0;
				mapWindows[i, j] = 0;
				mapRemove[i, j] = 0;
				if ((bool)mapRoomGO[i, j])
				{
					Object.Destroy(mapRoomGO[i, j]);
				}
				doorsPlaced[i, j] = false;
			}
		}
	}

	public void CreateOldRoomLayout(roomScript script_)
	{
		moneyRedesign = 0;
		replaceRoomID = script_.myID;
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapScript_.mapRoomID[i, j] == script_.myID)
				{
					moneyRedesign += rdS_.roomData_PRICE[script_.typ];
					mapOld[i, j] = 1;
					if (mapScript_.mapWindows[i, j] > 0)
					{
						mapWindows[i, j] = mapScript_.mapWindows[i, j];
					}
					if (mapScript_.mapDoors[i, j] > 0)
					{
						mapDoors[i, j] = mapScript_.mapDoors[i, j];
					}
				}
			}
		}
		mapScript_.RemoveRoom(script_.myID, particle: false);
		makeUpdate = true;
		CreateRoomVorschau();
	}

	private Vector2 GetAussenrand()
	{
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapMove[i, j] > 0)
				{
					return new Vector2(i, j);
				}
			}
		}
		return new Vector2(0f, 0f);
	}
}
