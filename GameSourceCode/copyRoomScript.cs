using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;
using UnityEngine.UI;

public class copyRoomScript : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] prefabs;

	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private GUI_Main guiMain;

	private Camera myCamera;

	private mainCameraScript mCamS_;

	private sfxScript sfx_;

	private buildRoomScript brS_;

	public mapScript mapS_;

	public Menu_BuildRoom menuBuildRoom_;

	public roomDataScript rdS_;

	private Seeker seeker;

	public GameObject copyRoomParent_;

	public roomScript rS_;

	public LayerMask layerMask;

	private RaycastHit hit;

	public int posX;

	public int posY;

	public int rotation;

	public Material[] myMaterials;

	public List<GameObject> tiles = new List<GameObject>();

	public List<GameObject> windows = new List<GameObject>();

	public List<GameObject> objects = new List<GameObject>();

	public GameObject myDoor;

	private MeshRenderer[] renderers;

	private BoxCollider[] colliders;

	private int modus;

	private int aktMaterial = -1;

	private bool coroutineRunning;

	private bool noPath;

	private Vector3[] endVectors;

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
		if (!tS_)
		{
			tS_ = GetComponent<textScript>();
		}
		if (!mapS_)
		{
			mapS_ = GetComponent<mapScript>();
		}
		if (!settings_)
		{
			settings_ = GetComponent<settingsScript>();
		}
		if (!brS_)
		{
			brS_ = GetComponent<buildRoomScript>();
		}
		if (!rdS_)
		{
			rdS_ = GetComponent<roomDataScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!myCamera)
		{
			myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		if (!mCamS_)
		{
			mCamS_ = GameObject.Find("Camera").GetComponent<mainCameraScript>();
		}
		if (!guiMain)
		{
			guiMain = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!menuBuildRoom_)
		{
			menuBuildRoom_ = guiMain.uiObjects[19].GetComponent<Menu_BuildRoom>();
		}
		if (!seeker)
		{
			seeker = GetComponent<Seeker>();
		}
	}

	private void Update()
	{
		if (!guiMain.uiObjects[428].activeSelf && !guiMain.uiObjects[429].activeSelf)
		{
			return;
		}
		if (mS_.multiplayer && !guiMain.menuOpen)
		{
			guiMain.menuOpen = true;
		}
		if (guiMain.IsMouseOverGUI())
		{
			copyRoomParent_.transform.position = new Vector3(-5000f, 0f, -5000f);
		}
		else
		{
			if (coroutineRunning || copyRoomParent_.transform.childCount <= 0)
			{
				return;
			}
			GetMousePosition();
			if (copyRoomParent_.transform.position.x < -1000f)
			{
				copyRoomParent_.transform.position = new Vector3(posX, 0f, posY);
			}
			copyRoomParent_.transform.position = Vector3.Lerp(copyRoomParent_.transform.position, new Vector3(posX, 0.1f, posY), 15f * Time.deltaTime);
			if (Input.GetKeyDown(KeyCode.Period) || Input.GetKeyDown(KeyCode.F))
			{
				rotation += 90;
				sfx_.PlaySound(6, force: true);
			}
			if (Input.GetKeyDown(KeyCode.Comma) || Input.GetKeyDown(KeyCode.R))
			{
				rotation -= 90;
				sfx_.PlaySound(6, force: true);
			}
			if (Input.GetMouseButton(1))
			{
				if (Input.mouseScrollDelta.y > 0f)
				{
					rotation -= 90;
				}
				if (Input.mouseScrollDelta.y < 0f)
				{
					rotation += 90;
				}
			}
			float num = 0f;
			num = Mathf.LerpAngle(copyRoomParent_.transform.eulerAngles.y, rotation, 15f * Time.deltaTime);
			copyRoomParent_.transform.eulerAngles = new Vector3(0f, num, 0f);
			if (CheckRoomCollision())
			{
				SetMaterial(1);
				return;
			}
			SetMaterial(0);
			if (Input.GetMouseButtonUp(0))
			{
				copyRoomParent_.transform.position = new Vector3(posX, 0f, posY);
				copyRoomParent_.transform.eulerAngles = new Vector3(0f, rotation, 0f);
				StartCoroutine(UpdatePathfindingNextFrame());
			}
		}
	}

	public void Clear()
	{
		tiles.Clear();
		windows.Clear();
		objects.Clear();
		if (copyRoomParent_.transform.childCount > 0)
		{
			for (int i = 0; i < copyRoomParent_.transform.childCount; i++)
			{
				Object.Destroy(copyRoomParent_.transform.GetChild(i).gameObject);
			}
		}
	}

	public void InitCopyRoom(roomScript script_)
	{
		modus = 0;
		if (!script_)
		{
			return;
		}
		rS_ = script_;
		guiMain.uiObjects[428].SetActive(value: true);
		Clear();
		uiObjects[3].SetActive(value: false);
		if (!mCamS_.additionalCamera[0].activeSelf)
		{
			mCamS_.additionalCamera[0].SetActive(value: true);
		}
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].typ != 16)
			{
				mS_.arrayRoomScripts[i].SetListGameObjectsLayer(0);
			}
		}
		copyRoomParent_.transform.position = new Vector3(Mathf.RoundToInt(rS_.uiPos.x), 0f, Mathf.RoundToInt(rS_.uiPos.z));
		for (int j = 0; j < rS_.listInventar.Count; j++)
		{
			if ((bool)rS_.listInventar[j] && rS_.listInventar[j].typ != -1)
			{
				GameObject gameObject = Object.Instantiate(rS_.listInventar[j].gameObject, copyRoomParent_.transform, worldPositionStays: true);
				gameObject.GetComponent<objectScript>().enabled = false;
				gameObject.tag = "Untagged";
				objects.Add(gameObject);
			}
		}
		Animation[] componentsInChildren = copyRoomParent_.GetComponentsInChildren<Animation>();
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			componentsInChildren[k].enabled = false;
		}
		AudioSource[] componentsInChildren2 = copyRoomParent_.GetComponentsInChildren<AudioSource>();
		for (int k = 0; k < componentsInChildren2.Length; k++)
		{
			componentsInChildren2[k].enabled = false;
		}
		colliders = copyRoomParent_.GetComponentsInChildren<BoxCollider>();
		BoxCollider[] array = colliders;
		for (int k = 0; k < array.Length; k++)
		{
			array[k].enabled = false;
		}
		for (int l = 0; l < mapS_.ROOMS.transform.childCount; l++)
		{
			Transform child = mapS_.ROOMS.transform.GetChild(l);
			if ((bool)child && child.gameObject.CompareTag("HideWall") && child.gameObject.layer == 17 && mapS_.mapRoomID[Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.z)] == rS_.myID)
			{
				if (mapS_.mapWindows[Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.z)] > 0 && (bool)child.GetComponent<identifyWindow>())
				{
					GameObject gameObject2 = Object.Instantiate(prefabs[3], copyRoomParent_.transform, worldPositionStays: true);
					gameObject2.transform.position = new Vector3(child.position.x, 0f, child.position.z);
					gameObject2.transform.rotation = child.rotation;
					gameObject2.tag = "Untagged";
					windows.Add(gameObject2);
				}
				else
				{
					GameObject obj = Object.Instantiate(prefabs[1], copyRoomParent_.transform, worldPositionStays: true);
					obj.transform.position = new Vector3(child.position.x, 0f, child.position.z);
					obj.transform.rotation = child.rotation;
					obj.tag = "Untagged";
				}
			}
		}
		myDoor = Object.Instantiate(prefabs[2], copyRoomParent_.transform, worldPositionStays: true);
		myDoor.transform.position = rS_.myDoor.transform.position;
		myDoor.transform.rotation = rS_.myDoor.transform.rotation;
		for (int m = 0; m < rS_.listGameObjects.Count; m++)
		{
			if ((bool)rS_.listGameObjects[m])
			{
				GameObject gameObject3 = Object.Instantiate(prefabs[0], copyRoomParent_.transform, worldPositionStays: true);
				gameObject3.transform.position = rS_.listGameObjects[m].transform.position;
				tiles.Add(gameObject3);
				if (Mathf.RoundToInt(gameObject3.transform.position.x) == Mathf.RoundToInt(myDoor.transform.position.x) && Mathf.RoundToInt(gameObject3.transform.position.z) == Mathf.RoundToInt(myDoor.transform.position.z))
				{
					gameObject3.GetComponent<BoxCollider>().enabled = false;
				}
			}
		}
		renderers = copyRoomParent_.GetComponentsInChildren<MeshRenderer>();
		aktMaterial = -1;
		SetMaterial(0);
		uiObjects[0].GetComponent<Image>().sprite = rdS_.roomData_SPRITE[rS_.typ];
		uiObjects[1].GetComponent<Text>().text = rdS_.GetName(rS_.typ);
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(GetRoomPrice() + GetObjectPrice(), showDollar: true);
	}

	public void InitMoveRoom(roomScript script_)
	{
		modus = 1;
		if (!script_)
		{
			return;
		}
		rS_ = script_;
		guiMain.uiObjects[429].SetActive(value: true);
		Clear();
		uiObjects[3].SetActive(value: false);
		uiObjects[7].SetActive(value: false);
		if (!mCamS_.additionalCamera[0].activeSelf)
		{
			mCamS_.additionalCamera[0].SetActive(value: true);
		}
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].typ != 16)
			{
				mS_.arrayRoomScripts[i].SetListGameObjectsLayer(0);
			}
		}
		copyRoomParent_.transform.position = new Vector3(Mathf.RoundToInt(rS_.uiPos.x), 0f, Mathf.RoundToInt(rS_.uiPos.z));
		for (int j = 0; j < rS_.listInventar.Count; j++)
		{
			if ((bool)rS_.listInventar[j] && rS_.listInventar[j].typ != -1)
			{
				GameObject gameObject = Object.Instantiate(rS_.listInventar[j].gameObject, copyRoomParent_.transform, worldPositionStays: true);
				gameObject.GetComponent<objectScript>().enabled = false;
				gameObject.tag = "Untagged";
				objects.Add(gameObject);
			}
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Object");
		if (array.Length != 0)
		{
			for (int k = 0; k < array.Length; k++)
			{
				if (!array[k])
				{
					continue;
				}
				bool flag = false;
				int num = Mathf.RoundToInt(array[k].transform.position.x);
				int num2 = Mathf.RoundToInt(array[k].transform.position.z);
				if (!mapS_.IsInMapLimit(num, num2) || !mapS_.IsInMapLimit(num + 1, num2) || !mapS_.IsInMapLimit(num - 1, num2) || !mapS_.IsInMapLimit(num, num2 + 1) || !mapS_.IsInMapLimit(num, num2 - 1))
				{
					continue;
				}
				int num3 = Mathf.RoundToInt(array[k].transform.eulerAngles.y);
				if (num3 == 90 && mapS_.mapRoomID[num - 1, num2] == rS_.myID)
				{
					flag = true;
				}
				else if (num3 == 270 && mapS_.mapRoomID[num + 1, num2] == rS_.myID)
				{
					flag = true;
				}
				else if (num3 == 0 && mapS_.mapRoomID[num, num2 - 1] == rS_.myID)
				{
					flag = true;
				}
				else if (num3 == 180 && mapS_.mapRoomID[num, num2 + 1] == rS_.myID)
				{
					flag = true;
				}
				if (flag)
				{
					objectScript component = array[k].GetComponent<objectScript>();
					if ((bool)component && component.typ != -1 && component.GetRoomID() == 1 && component.wallObject)
					{
						GameObject gameObject2 = Object.Instantiate(array[k], copyRoomParent_.transform, worldPositionStays: true);
						gameObject2.GetComponent<objectScript>().enabled = false;
						gameObject2.tag = "FloorObject";
						objects.Add(gameObject2);
						Object.Destroy(array[k]);
					}
				}
			}
		}
		Animation[] componentsInChildren = copyRoomParent_.GetComponentsInChildren<Animation>();
		for (int l = 0; l < componentsInChildren.Length; l++)
		{
			componentsInChildren[l].enabled = false;
		}
		AudioSource[] componentsInChildren2 = copyRoomParent_.GetComponentsInChildren<AudioSource>();
		for (int l = 0; l < componentsInChildren2.Length; l++)
		{
			componentsInChildren2[l].enabled = false;
		}
		colliders = copyRoomParent_.GetComponentsInChildren<BoxCollider>();
		BoxCollider[] array2 = colliders;
		for (int l = 0; l < array2.Length; l++)
		{
			array2[l].enabled = false;
		}
		for (int m = 0; m < mapS_.ROOMS.transform.childCount; m++)
		{
			Transform child = mapS_.ROOMS.transform.GetChild(m);
			if ((bool)child && child.gameObject.CompareTag("HideWall") && child.gameObject.layer == 17 && mapS_.mapRoomID[Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.z)] == rS_.myID)
			{
				if (mapS_.mapWindows[Mathf.RoundToInt(child.position.x), Mathf.RoundToInt(child.position.z)] > 0 && (bool)child.GetComponent<identifyWindow>())
				{
					GameObject gameObject3 = Object.Instantiate(prefabs[3], copyRoomParent_.transform, worldPositionStays: true);
					gameObject3.transform.position = new Vector3(child.position.x, 0f, child.position.z);
					gameObject3.transform.rotation = child.rotation;
					gameObject3.tag = "Untagged";
					windows.Add(gameObject3);
				}
				else
				{
					GameObject obj = Object.Instantiate(prefabs[1], copyRoomParent_.transform, worldPositionStays: true);
					obj.transform.position = new Vector3(child.position.x, 0f, child.position.z);
					obj.transform.rotation = child.rotation;
					obj.tag = "Untagged";
				}
			}
		}
		myDoor = Object.Instantiate(prefabs[2], copyRoomParent_.transform, worldPositionStays: true);
		myDoor.transform.position = rS_.myDoor.transform.position;
		myDoor.transform.rotation = rS_.myDoor.transform.rotation;
		for (int n = 0; n < rS_.listGameObjects.Count; n++)
		{
			if ((bool)rS_.listGameObjects[n])
			{
				GameObject gameObject4 = Object.Instantiate(prefabs[0], copyRoomParent_.transform, worldPositionStays: true);
				gameObject4.transform.position = rS_.listGameObjects[n].transform.position;
				tiles.Add(gameObject4);
				if (Mathf.RoundToInt(gameObject4.transform.position.x) == Mathf.RoundToInt(myDoor.transform.position.x) && Mathf.RoundToInt(gameObject4.transform.position.z) == Mathf.RoundToInt(myDoor.transform.position.z))
				{
					gameObject4.GetComponent<BoxCollider>().enabled = false;
				}
			}
		}
		renderers = copyRoomParent_.GetComponentsInChildren<MeshRenderer>();
		aktMaterial = -1;
		SetMaterial(0);
		uiObjects[5].GetComponent<Image>().sprite = rdS_.roomData_SPRITE[rS_.typ];
		uiObjects[6].GetComponent<Text>().text = rdS_.GetName(rS_.typ);
		for (int num4 = 0; num4 < rS_.listInventar.Count; num4++)
		{
			if ((bool)rS_.listInventar[num4])
			{
				Object.Destroy(rS_.listInventar[num4].gameObject);
			}
		}
		mapS_.RemoveRoom(rS_.myID, particle: false);
	}

	private void GetMousePosition()
	{
		if (!guiMain.IsMouseOverGUI() && Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out hit, 200f, layerMask))
		{
			posX = Mathf.RoundToInt(hit.transform.position.x);
			posY = Mathf.RoundToInt(hit.transform.position.z);
		}
	}

	private bool CheckRoomCollision()
	{
		for (int i = 0; i < tiles.Count; i++)
		{
			int num = Mathf.RoundToInt(tiles[i].transform.position.x);
			int num2 = Mathf.RoundToInt(tiles[i].transform.position.z);
			if (!mapS_.IsInMapLimit(num, num2))
			{
				return true;
			}
			if (mapS_.mapBuilding[num, num2] <= 0)
			{
				return true;
			}
			if (mapS_.mapBlock[num, num2] > 0 || mapS_.mapBlockDoor[num, num2] > 0)
			{
				return true;
			}
			if (mapS_.mapRoomID[num, num2] != mapScript.ID_FLOOR)
			{
				return true;
			}
			if (mapS_.mapDoors[num, num2] > 0)
			{
				return true;
			}
			if (!myDoor)
			{
				continue;
			}
			int num3 = Mathf.RoundToInt(myDoor.transform.position.x);
			int num4 = Mathf.RoundToInt(myDoor.transform.position.z);
			if (Mathf.RoundToInt(myDoor.transform.eulerAngles.y) == 180)
			{
				num3--;
			}
			if (Mathf.RoundToInt(myDoor.transform.eulerAngles.y) == 0)
			{
				num3++;
			}
			if (Mathf.RoundToInt(myDoor.transform.eulerAngles.y) == 90)
			{
				num4--;
			}
			if (Mathf.RoundToInt(myDoor.transform.eulerAngles.y) == 270)
			{
				num4++;
			}
			if (mapS_.IsInMapLimit(num3, num4))
			{
				if (mapS_.mapBuilding[num3, num4] <= 0)
				{
					return true;
				}
				if (mapS_.mapRoomID[num3, num4] != mapScript.ID_FLOOR)
				{
					return true;
				}
				if (mapS_.mapDoors[num3, num4] > 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void SetMaterial(int i)
	{
		if (aktMaterial == i)
		{
			return;
		}
		aktMaterial = i;
		Debug.Log("SetMaterial() " + i);
		MeshRenderer[] array = renderers;
		foreach (MeshRenderer meshRenderer in array)
		{
			if ((bool)meshRenderer && meshRenderer.gameObject.layer != 10)
			{
				meshRenderer.material = myMaterials[i];
			}
		}
	}

	private void CreateRoom_CopyRoom()
	{
		int num = 0;
		int num2 = 0;
		copyRoomParent_.transform.position = new Vector3(posX, 0f, posY);
		copyRoomParent_.transform.eulerAngles = new Vector3(0f, rotation, 0f);
		for (int i = 0; i < tiles.Count; i++)
		{
			num = Mathf.RoundToInt(tiles[i].transform.position.x);
			num2 = Mathf.RoundToInt(tiles[i].transform.position.z);
			if (!mapS_.IsInMapLimit(num, num2))
			{
				return;
			}
			brS_.mapOld[num, num2] = 1;
		}
		for (int j = 0; j < windows.Count; j++)
		{
			num = Mathf.RoundToInt(windows[j].transform.position.x);
			num2 = Mathf.RoundToInt(windows[j].transform.position.z);
			if (!mapS_.IsInMapLimit(num, num2))
			{
				return;
			}
			bool flag = false;
			int num3 = Mathf.RoundToInt(windows[j].transform.eulerAngles.y);
			if (num3 == 180 && mapS_.mapBuilding[num - 1, num2] != mapS_.mapBuilding[num, num2])
			{
				flag = true;
			}
			if (num3 == 0 && mapS_.mapBuilding[num + 1, num2] != mapS_.mapBuilding[num, num2])
			{
				flag = true;
			}
			if (num3 == 90 && mapS_.mapBuilding[num, num2 - 1] != mapS_.mapBuilding[num, num2])
			{
				flag = true;
			}
			if (num3 == 270 && mapS_.mapBuilding[num, num2 + 1] != mapS_.mapBuilding[num, num2])
			{
				flag = true;
			}
			if (!flag)
			{
				brS_.mapWindows[num, num2] = Mathf.RoundToInt(windows[j].transform.eulerAngles.y) + 1000;
			}
		}
		num = Mathf.RoundToInt(myDoor.transform.position.x);
		num2 = Mathf.RoundToInt(myDoor.transform.position.z);
		if (mapS_.IsInMapLimit(num, num2))
		{
			brS_.mapDoors[num, num2] = Mathf.RoundToInt(myDoor.transform.eulerAngles.y) + 1000;
		}
		int roomPrice = GetRoomPrice();
		bool isOn = menuBuildRoom_.uiObjects[33].GetComponent<Toggle>().isOn;
		menuBuildRoom_.uiObjects[33].GetComponent<Toggle>().isOn = false;
		brS_.CreateRoom(rS_.typ, roomPrice);
		menuBuildRoom_.uiObjects[33].GetComponent<Toggle>().isOn = isOn;
		for (int k = 0; k < objects.Count; k++)
		{
			if ((bool)objects[k])
			{
				objectScript component = objects[k].GetComponent<objectScript>();
				if ((bool)component)
				{
					objectScript component2 = mapS_.CreateObject(component.typ, createdWithAutoInventar: false).GetComponent<objectScript>();
					mS_.objectRotation = Mathf.RoundToInt(objects[k].transform.eulerAngles.y);
					component2.PlatziereObject(objects[k].transform.position, fromSavegame: false, updatePathfinding: false, autoInventar: true, partikel: false);
				}
			}
		}
	}

	private void CreateRoom_MoveRoom()
	{
		int num = 0;
		int num2 = 0;
		copyRoomParent_.transform.position = new Vector3(posX, 0f, posY);
		copyRoomParent_.transform.eulerAngles = new Vector3(0f, rotation, 0f);
		for (int i = 0; i < tiles.Count; i++)
		{
			num = Mathf.RoundToInt(tiles[i].transform.position.x);
			num2 = Mathf.RoundToInt(tiles[i].transform.position.z);
			if (mapS_.IsInMapLimit(num, num2))
			{
				mapS_.mapRoomID[num, num2] = rS_.myID;
				mapS_.mapRoomScript[num, num2] = rS_;
			}
		}
		for (int j = 0; j < windows.Count; j++)
		{
			num = Mathf.RoundToInt(windows[j].transform.position.x);
			num2 = Mathf.RoundToInt(windows[j].transform.position.z);
			if (mapS_.IsInMapLimit(num, num2))
			{
				bool flag = false;
				int num3 = Mathf.RoundToInt(windows[j].transform.eulerAngles.y);
				if (num3 == 180 && mapS_.mapBuilding[num - 1, num2] != mapS_.mapBuilding[num, num2])
				{
					flag = true;
				}
				if (num3 == 0 && mapS_.mapBuilding[num + 1, num2] != mapS_.mapBuilding[num, num2])
				{
					flag = true;
				}
				if (num3 == 90 && mapS_.mapBuilding[num, num2 - 1] != mapS_.mapBuilding[num, num2])
				{
					flag = true;
				}
				if (num3 == 270 && mapS_.mapBuilding[num, num2 + 1] != mapS_.mapBuilding[num, num2])
				{
					flag = true;
				}
				if (!flag)
				{
					mapS_.mapWindows[num, num2] = Mathf.RoundToInt(windows[j].transform.eulerAngles.y) + 1000;
				}
			}
		}
		num = Mathf.RoundToInt(myDoor.transform.position.x);
		num2 = Mathf.RoundToInt(myDoor.transform.position.z);
		if (mapS_.IsInMapLimit(num, num2))
		{
			mapS_.mapDoors[num, num2] = Mathf.RoundToInt(myDoor.transform.eulerAngles.y) + 1000;
		}
		if (mS_.multiplayer)
		{
			for (int k = 0; k < tiles.Count; k++)
			{
				num = Mathf.RoundToInt(tiles[k].transform.position.x);
				num2 = Mathf.RoundToInt(tiles[k].transform.position.z);
				if (mapS_.IsInMapLimit(num, num2))
				{
					mS_.Multiplayer_SendMap(num, num2);
				}
			}
		}
		mapS_.CreateWalls(-1);
		rS_.uiPos = brS_.FindUiPositionExtern(rS_.myID);
		rS_.transform.position = new Vector3(rS_.uiPos.x, 0f, rS_.uiPos.z);
		for (int l = 0; l < objects.Count; l++)
		{
			if (!objects[l])
			{
				continue;
			}
			if (objects[l].CompareTag("Untagged"))
			{
				objectScript component = objects[l].GetComponent<objectScript>();
				Debug.Log("Name: " + component.name);
				if ((bool)component)
				{
					objectScript component2 = mapS_.CreateObject(component.typ, createdWithAutoInventar: false).GetComponent<objectScript>();
					component2.gekauft = true;
					mS_.objectRotation = Mathf.RoundToInt(objects[l].transform.eulerAngles.y);
					component2.PlatziereObject(objects[l].transform.position, fromSavegame: false, updatePathfinding: false, autoInventar: true, partikel: false);
				}
				continue;
			}
			int num4 = Mathf.RoundToInt(objects[l].transform.position.x);
			int num5 = Mathf.RoundToInt(objects[l].transform.position.z);
			if (!mapS_.IsInMapLimit(num4, num5))
			{
				continue;
			}
			objectScript component3 = objects[l].GetComponent<objectScript>();
			Debug.Log("Name: " + component3.name);
			if (mapS_.mapRoomID[num4, num5] == 1)
			{
				if ((bool)component3)
				{
					objectScript component4 = mapS_.CreateObject(component3.typ, createdWithAutoInventar: false).GetComponent<objectScript>();
					component4.gekauft = true;
					mS_.objectRotation = Mathf.RoundToInt(objects[l].transform.eulerAngles.y);
					component4.PlatziereObject(objects[l].transform.position, fromSavegame: false, updatePathfinding: false, autoInventar: true, partikel: false);
				}
			}
			else
			{
				mS_.Earn(component3.GetVerkaufspreis(), 0);
			}
		}
		for (int m = 0; m < mS_.arrayCharactersScripts.Length; m++)
		{
			if ((bool)mS_.arrayCharactersScripts[m] && mS_.arrayCharactersScripts[m].roomID == rS_.myID && mS_.arrayCharactersScripts[m].objectBelegtID == -1)
			{
				mS_.arrayCharactersScripts[m].transform.position = new Vector3(rS_.uiPos.x + Random.Range(-1f, 1f), 0f, rS_.uiPos.z + Random.Range(-1f, 1f));
			}
		}
		Close();
	}

	private int GetRoomPrice()
	{
		if (!mS_)
		{
			FindScripts();
		}
		int count = tiles.Count;
		return rdS_.GetPrice(rS_.typ) * count;
	}

	private int GetObjectPrice()
	{
		int num = 0;
		for (int i = 0; i < objects.Count; i++)
		{
			if ((bool)objects[i])
			{
				objectScript component = objects[i].GetComponent<objectScript>();
				if ((bool)component)
				{
					num += component.preis;
				}
			}
		}
		return num;
	}

	public void Close()
	{
		guiMain.uiObjects[428].SetActive(value: false);
		guiMain.uiObjects[429].SetActive(value: false);
		if (mCamS_.additionalCamera[0].activeSelf)
		{
			mCamS_.additionalCamera[0].SetActive(value: false);
		}
		guiMain.CloseMenu();
		Clear();
		StartCoroutine(mS_.UpdatePathfindingNextFrame());
	}

	private IEnumerator UpdatePathfindingNextFrame()
	{
		coroutineRunning = true;
		noPath = true;
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if ((bool)mapS_.aStar_)
		{
			mapS_.aStar_.Scan();
		}
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		int num = 1;
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mS_.IsMyBuilding(mapS_.mapBuilding[i, j]) && mapS_.mapBuilding[i, j] != mapScript.ID_FLOOROUTSIDE && mapS_.mapDoors[i, j] > 0)
				{
					num++;
				}
			}
		}
		GameObject gameObject = GameObject.FindGameObjectWithTag("BuildingDoor");
		endVectors = new Vector3[num];
		Debug.Log("AmountDoors: " + endVectors.Length);
		int num2 = 0;
		for (int k = 0; k < mapScript.mapSizeX; k++)
		{
			for (int l = 0; l < mapScript.mapSizeY; l++)
			{
				if (mS_.IsMyBuilding(mapS_.mapBuilding[k, l]) && mapS_.mapBuilding[k, l] != mapScript.ID_FLOOROUTSIDE && mapS_.mapDoors[k, l] > 0)
				{
					endVectors[num2] = new Vector3(k, 0f, l);
					num2++;
				}
			}
		}
		endVectors[num2] = myDoor.transform.position;
		seeker.StartMultiTargetPath(gameObject.transform.position, endVectors, pathsForAll: true, OnPathComplete);
	}

	public void OnPathComplete(Path p)
	{
		Debug.Log("Got Callback");
		coroutineRunning = false;
		if (p.error)
		{
			Debug.Log("Ouch, the path returned an error\nError: " + p.errorLog);
			noPath = true;
			sfx_.PlaySound(2, force: true);
			return;
		}
		if (!(p is MultiTargetPath multiTargetPath))
		{
			Debug.LogError("The Path was no MultiTargetPath");
			noPath = true;
			sfx_.PlaySound(2, force: true);
			return;
		}
		noPath = false;
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
				sfx_.PlaySound(2, force: true);
				continue;
			}
			int num = Mathf.RoundToInt(list[list.Count - 1].x);
			int num2 = Mathf.RoundToInt(list[list.Count - 1].z);
			if ((bool)mapS_.mapRoomScript[num, num2] && mapS_.mapRoomScript[num, num2].typ != 16)
			{
				mapS_.mapRoomScript[num, num2].SetListGameObjectsLayer(0);
			}
		}
		if (!noPath)
		{
			if (modus == 0)
			{
				CreateRoom_CopyRoom();
			}
			else
			{
				CreateRoom_MoveRoom();
			}
			guiMain.TOGGLE_Walls();
			return;
		}
		if (modus == 0)
		{
			if (!uiObjects[3].activeSelf)
			{
				uiObjects[3].SetActive(value: true);
			}
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(73);
		}
		if (modus == 1)
		{
			if (!uiObjects[7].activeSelf)
			{
				uiObjects[7].SetActive(value: true);
			}
			uiObjects[8].GetComponent<Text>().text = tS_.GetText(73);
		}
	}
}
