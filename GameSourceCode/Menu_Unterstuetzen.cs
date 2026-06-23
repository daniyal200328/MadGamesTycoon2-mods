using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Menu_Unterstuetzen : MonoBehaviour
{
	private GameObject main_;

	private mainScript mS_;

	private Camera myCamera;

	private sfxScript sfx_;

	private RaycastHit hit;

	public RaycastHit hitOld;

	private RaycastHit hitEmpty;

	private GUI_Main guiMain_;

	private mapScript mapS_;

	private pickObjectScript pOS_;

	private Camera camera_;

	public GameObject[] uiObjects;

	public LayerMask layerMaskFloor;

	public roomScript rS_;

	private roomScript roomOutlineOld;

	private VectorLine line3D;

	private bool initLine;

	private GameObject myLine;

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
		if (!camera_)
		{
			camera_ = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
		}
		if (!pOS_)
		{
			pOS_ = main_.GetComponent<pickObjectScript>();
		}
		if (!myCamera)
		{
			myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void Update()
	{
		DrawLine();
		MouseMovement();
	}

	private void MouseMovement()
	{
		if (!mS_)
		{
			return;
		}
		bool mouseButtonUp = Input.GetMouseButtonUp(0);
		pOS_.disableMouseButton = mouseButtonUp;
		if (Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out var hitInfo, 200f, layerMaskFloor))
		{
			float x = hitInfo.point.x;
			float z = hitInfo.point.z;
			int num = Mathf.RoundToInt(x);
			int num2 = Mathf.RoundToInt(z);
			if (mapS_.mapRoomID[num, num2] != 1)
			{
				if (!mapS_.mapRoomScript[num, num2])
				{
					return;
				}
				if (roomOutlineOld != mapS_.mapRoomScript[num, num2])
				{
					if ((bool)roomOutlineOld)
					{
						roomOutlineOld.DisableOutlineLayer();
					}
					roomOutlineOld = mapS_.mapRoomScript[num, num2];
					if (mapS_.mapRoomScript[num, num2].typ == rS_.typ && mapS_.mapRoomScript[num, num2].myID != rS_.myID)
					{
						mapS_.mapRoomScript[num, num2].SetOutlineLayer();
					}
				}
				if (mouseButtonUp)
				{
					if (mapS_.mapRoomScript[num, num2].typ == rS_.typ && mapS_.mapRoomScript[num, num2].myID != rS_.myID)
					{
						Accept(mapS_.mapRoomScript[num, num2]);
					}
					else
					{
						sfx_.PlaySound(2, force: true);
					}
				}
			}
			else if ((bool)roomOutlineOld)
			{
				roomOutlineOld.DisableOutlineLayer();
				roomOutlineOld = null;
			}
		}
		else if ((bool)roomOutlineOld)
		{
			roomOutlineOld.DisableOutlineLayer();
			roomOutlineOld = null;
		}
	}

	public void BUTTON_Close()
	{
		initLine = false;
		if ((bool)myLine)
		{
			Object.Destroy(myLine);
		}
		sfx_.PlaySound(3, force: true);
		if ((bool)roomOutlineOld)
		{
			roomOutlineOld.DisableOutlineLayer();
			roomOutlineOld = null;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	private void Accept(roomScript script_)
	{
		if (rS_ == script_)
		{
			return;
		}
		if ((bool)rS_ && (bool)script_ && rS_.typ == script_.typ)
		{
			for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
			{
				if ((bool)mS_.arrayRoomScripts[i] && (bool)mS_.arrayRoomScripts[i].taskGameObject)
				{
					taskUnterstuetzen taskUnterstuetzen2 = mS_.arrayRoomScripts[i].GetTaskUnterstuetzen();
					if ((bool)taskUnterstuetzen2 && taskUnterstuetzen2.roomID == rS_.myID)
					{
						taskUnterstuetzen2.Abbrechen();
					}
				}
			}
			if (script_.taskID != -1)
			{
				GameObject gameObject = GameObject.Find("Task_" + script_.taskID);
				if ((bool)gameObject && (bool)gameObject.GetComponent<taskUnterstuetzen>())
				{
					gameObject.GetComponent<taskUnterstuetzen>().Abbrechen();
				}
			}
			taskUnterstuetzen taskUnterstuetzen3 = guiMain_.AddTask_Unterstuetzen();
			taskUnterstuetzen3.Init(fromSavegame: false);
			taskUnterstuetzen3.roomID = script_.myID;
			rS_.taskID = taskUnterstuetzen3.myID;
			rS_.DisableOutlineLayer();
			script_.DisableOutlineLayer();
		}
		BUTTON_Close();
	}

	private void DrawLine()
	{
		if (!rS_ || !mS_.settings_)
		{
			return;
		}
		VectorManager.useDraw3D = true;
		if (!initLine)
		{
			initLine = true;
			line3D = new VectorLine("UntersteutzenLine3D", new List<Vector3>(2), 20f, LineType.Continuous, Joins.Weld);
			line3D.endCap = "VerschiebeTask";
			GameObject gameObject = line3D.rectTransform.gameObject;
			myLine = gameObject;
		}
		if ((bool)myLine && (bool)myLine.GetComponent<MeshRenderer>())
		{
			myLine.GetComponent<MeshRenderer>().material.shader = mS_.shaders[0];
		}
		Vector3 vector = new Vector3(0f, 0f, 0f);
		if (Physics.Raycast(camera_.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out var hitInfo))
		{
			vector = hitInfo.point;
			line3D.color = guiMain_.colors[21];
			line3D.points3[0] = vector;
			line3D.points3[1] = new Vector3(rS_.uiPos.x, rS_.uiPos.y - 0.2f, rS_.uiPos.z);
			line3D.Draw3D();
		}
		else
		{
			initLine = false;
			if ((bool)myLine)
			{
				Object.Destroy(myLine);
			}
		}
	}
}
