using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pickCharacterScript : MonoBehaviour
{
	private mainScript mS_;

	private GUI_Main guiMain;

	private Camera myCamera;

	private sfxScript sfx_;

	private RaycastHit hit;

	public RaycastHit hitOld;

	private RaycastHit hitEmpty;

	private gummibandScript gummiS_;

	private GUI_Main guiMain_;

	private mapScript mapS_;

	private roomDataScript rdS_;

	private pickObjectScript pOS_;

	private mainCameraScript mCamS_;

	public LayerMask layerMaskChar;

	public LayerMask layerMaskFloor;

	private int oldRoomID = -1;

	private Vector3 oldPosition;

	private roomScript roomOutlineOld;

	private bool lastFrameESC;

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
		if (!guiMain)
		{
			guiMain = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!myCamera)
		{
			myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!gummiS_)
		{
			gummiS_ = GameObject.Find("CanvasInGameMenu").GetComponent<gummibandScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!mapS_)
		{
			mapS_ = GetComponent<mapScript>();
		}
		if (!rdS_)
		{
			rdS_ = GetComponent<roomDataScript>();
		}
		if (!pOS_)
		{
			pOS_ = GetComponent<pickObjectScript>();
		}
		if (!mCamS_)
		{
			mCamS_ = myCamera.GetComponent<mainCameraScript>();
		}
	}

	private void Update()
	{
		Pick();
		MouseMovement();
	}

	private void Pick()
	{
		if (gummiS_.isActive || (guiMain.menuOpen && !guiMain.uiObjects[15].activeSelf))
		{
			return;
		}
		PickupGroupWithKey();
		if (guiMain.IsMouseOverGUI())
		{
			if ((bool)hitOld.transform)
			{
				hitOld.transform.gameObject.GetComponent<characterScript>().MouseLeave();
				hitOld = hitEmpty;
			}
			return;
		}
		int layerMask = 4096;
		if (Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out hit, 200f, layerMask))
		{
			mCamS_.EnablePostLiner();
			if (hit.transform != hitOld.transform)
			{
				if ((bool)hitOld.transform)
				{
					hitOld.transform.gameObject.GetComponent<characterScript>().MouseLeave();
					hitOld = hitEmpty;
				}
				hitOld = hit;
				hit.transform.gameObject.GetComponent<characterScript>().MouseOver();
			}
		}
		else if ((bool)hitOld.transform)
		{
			hitOld.transform.gameObject.GetComponent<characterScript>().MouseLeave();
			hitOld = hitEmpty;
		}
		if (!guiMain.uiObjects[2].GetComponent<Toggle>().isOn && Input.GetMouseButtonUp(0) && (bool)hitOld.transform && !guiMain.uiObjects[2].GetComponent<Toggle>().isOn)
		{
			StartCoroutine(PickChar(hitOld.transform.gameObject));
		}
	}

	public void PickFromExternObject(GameObject go)
	{
		StartCoroutine(PickChar(go));
	}

	public IEnumerator PickChar(GameObject go)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (!guiMain_.menuOpen || guiMain.uiObjects[15].activeSelf)
		{
			characterScript component = go.GetComponent<characterScript>();
			if (!component.picked)
			{
				oldRoomID = component.roomID;
				oldPosition = go.transform.position;
				guiMain.OpenMenu(hideChars: false);
				guiMain_.disableRoomGUI = false;
				sfx_.PlaySound(9, force: true);
				component.PickUp();
				guiMain.ActivateMenu(guiMain.uiObjects[15]);
			}
		}
		else
		{
			Debug.Log("Picking abbrechen!");
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

	public bool ESC_DropChar()
	{
		if (mS_.pickedChars.Count == 1 && oldRoomID != -1 && oldPosition.x != (float)Mathf.RoundToInt(9999f))
		{
			characterScript component = mS_.pickedChars[0].GetComponent<characterScript>();
			component.DropChar(oldPosition);
			oldPosition = new Vector3(9999f, 9999f, 9999f);
			component.roomID = oldRoomID;
			oldRoomID = -1;
			if ((bool)roomOutlineOld)
			{
				roomOutlineOld.DisableOutlineLayer();
				roomOutlineOld = null;
			}
			if (mCamS_.additionalCamera[1].activeSelf)
			{
				mCamS_.additionalCamera[1].SetActive(value: false);
			}
			return true;
		}
		return false;
	}

	private void MouseMovement()
	{
		if (!mS_ || mS_.pickedChars.Count <= 0 || guiMain_.IsMouseOverGUI())
		{
			return;
		}
		bool flag = Input.GetMouseButtonUp(0);
		pOS_.disableMouseButton = flag;
		bool flag2 = false;
		if (lastFrameESC && ESC_DropChar())
		{
			lastFrameESC = false;
			return;
		}
		if (Input.GetKeyUp(KeyCode.Escape))
		{
			lastFrameESC = true;
		}
		else
		{
			lastFrameESC = false;
		}
		if (Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out var hitInfo, 200f, layerMaskChar))
		{
			flag = false;
			flag2 = true;
		}
		if (Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out hitInfo, 200f, layerMaskFloor))
		{
			float x = hitInfo.point.x;
			float z = hitInfo.point.z;
			Vector3 vector = new Vector3(x, 0.3f, z);
			for (int i = 0; i < mS_.pickedChars.Count; i++)
			{
				if (!mS_.pickedChars[i])
				{
					continue;
				}
				if (i == 0)
				{
					mS_.pickedChars[i].transform.position = Vector3.Lerp(mS_.pickedChars[i].transform.position, vector, 0.3f);
					if (mS_.pickedChars[i].transform.position.y > 100f)
					{
						mS_.pickedChars[i].transform.position = vector;
					}
					if (mS_.pickedChars[i].transform.GetChild(0).gameObject.layer != 16)
					{
						SetLayer(16, mS_.pickedChars[i].transform.GetChild(0));
					}
					if (!mCamS_.additionalCamera[1].activeSelf)
					{
						mCamS_.additionalCamera[1].SetActive(value: true);
					}
				}
				else
				{
					mS_.pickedChars[i].transform.position = new Vector3(0f, 5000f, 0f);
				}
			}
			mCamS_.SetOutlineColor(2, 0.3f, 4);
			int num = Mathf.RoundToInt(vector.x);
			int num2 = Mathf.RoundToInt(vector.z);
			if (mapS_.mapRoomID[num, num2] != 1 && !flag2)
			{
				if ((bool)mapS_.mapRoomScript[num, num2])
				{
					if (!rdS_.KeineMitarbeiter(mapS_.mapRoomScript[num, num2].typ))
					{
						if (roomOutlineOld != mapS_.mapRoomScript[num, num2])
						{
							if ((bool)roomOutlineOld)
							{
								roomOutlineOld.DisableOutlineLayer();
							}
							roomOutlineOld = mapS_.mapRoomScript[num, num2];
							mapS_.mapRoomScript[num, num2].SetOutlineLayer();
						}
					}
					else if ((bool)roomOutlineOld)
					{
						roomOutlineOld.DisableOutlineLayer();
						roomOutlineOld = null;
					}
				}
			}
			else if ((bool)roomOutlineOld)
			{
				roomOutlineOld.DisableOutlineLayer();
				roomOutlineOld = null;
			}
			if (!flag || !mS_.IsMyBuilding(mapS_.mapBuilding[num, num2]))
			{
				return;
			}
			if ((bool)roomOutlineOld)
			{
				roomOutlineOld.DisableOutlineLayer();
				roomOutlineOld = null;
			}
			if (mCamS_.additionalCamera[1].activeSelf)
			{
				mCamS_.additionalCamera[1].SetActive(value: false);
			}
			List<GameObject> list = new List<GameObject>();
			for (int j = 0; j < mS_.pickedChars.Count; j++)
			{
				if ((bool)mS_.pickedChars[j])
				{
					list.Add(mS_.pickedChars[j]);
				}
			}
			for (int k = 0; k < list.Count; k++)
			{
				if ((bool)list[k])
				{
					list[k].GetComponent<characterScript>().DropChar(vector);
				}
			}
			return;
		}
		for (int l = 0; l < mS_.pickedChars.Count; l++)
		{
			if ((bool)mS_.pickedChars[l])
			{
				mS_.pickedChars[l].transform.position = new Vector3(0f, 9999f, 0f);
			}
		}
		if ((bool)roomOutlineOld)
		{
			roomOutlineOld.DisableOutlineLayer();
			roomOutlineOld = null;
		}
	}

	private void PickupGroupWithKey()
	{
		if (Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKeyUp(KeyCode.F1))
			{
				SelectGroup(1);
			}
			if (Input.GetKeyUp(KeyCode.F2))
			{
				SelectGroup(2);
			}
			if (Input.GetKeyUp(KeyCode.F3))
			{
				SelectGroup(3);
			}
			if (Input.GetKeyUp(KeyCode.F4))
			{
				SelectGroup(4);
			}
			if (Input.GetKeyUp(KeyCode.F5))
			{
				SelectGroup(5);
			}
			if (Input.GetKeyUp(KeyCode.F6))
			{
				SelectGroup(6);
			}
			if (Input.GetKeyUp(KeyCode.F7))
			{
				SelectGroup(7);
			}
			if (Input.GetKeyUp(KeyCode.F8))
			{
				SelectGroup(8);
			}
			if (Input.GetKeyUp(KeyCode.F9))
			{
				SelectGroup(9);
			}
			if (Input.GetKeyUp(KeyCode.F10))
			{
				SelectGroup(10);
			}
			if (Input.GetKeyUp(KeyCode.F11))
			{
				SelectGroup(11);
			}
			if (Input.GetKeyUp(KeyCode.F12))
			{
				SelectGroup(12);
			}
		}
	}

	private void SelectGroup(int g)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Character");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				characterScript component = array[i].GetComponent<characterScript>();
				if ((bool)component && component.group == g)
				{
					StartCoroutine(PickChar(array[i]));
				}
			}
		}
	}
}
