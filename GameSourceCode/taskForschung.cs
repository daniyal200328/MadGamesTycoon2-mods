using UnityEngine;

public class taskForschung : MonoBehaviour
{
	public int myID = -1;

	public int typ = -1;

	public bool[] kategorie = new bool[7];

	public int slot = -1;

	public bool automatic;

	public bool automaticWait;

	public bool autoForschung;

	private GameObject main_;

	private mainScript mS_;

	private genres genres_;

	private themes themes_;

	private engineFeatures eF_;

	private gameplayFeatures gF_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private unlockScript unlock_;

	private forschungSonstiges fS_;

	private void Awake()
	{
		base.transform.position = new Vector3(10f, 0f, 0f);
	}

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			if (!main_)
			{
				main_ = GameObject.FindGameObjectWithTag("Main");
			}
			if (!mS_)
			{
				mS_ = main_.GetComponent<mainScript>();
			}
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
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
			if (!hardware_)
			{
				hardware_ = main_.GetComponent<hardware>();
			}
			if (!hardwareFeatures_)
			{
				hardwareFeatures_ = main_.GetComponent<hardwareFeatures>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!rdS_)
			{
				rdS_ = main_.GetComponent<roomDataScript>();
			}
			if (!unlock_)
			{
				unlock_ = main_.GetComponent<unlockScript>();
			}
			if (!fS_)
			{
				fS_ = main_.GetComponent<forschungSonstiges>();
			}
		}
	}

	public void Init(bool fromSavegame)
	{
		if (!fromSavegame)
		{
			myID = Random.Range(1, 100000000);
		}
		base.name = "Task_" + myID;
	}

	public float GetProzent()
	{
		FindScripts();
		return typ switch
		{
			0 => genres_.GetProzent(slot), 
			1 => themes_.GetProzent(slot), 
			2 => eF_.GetProzent(slot), 
			3 => gF_.GetProzent(slot), 
			4 => hardware_.GetProzent(slot), 
			5 => fS_.GetProzent(slot), 
			6 => hardwareFeatures_.GetProzent(slot), 
			_ => -1f, 
		};
	}

	public void Work(float f)
	{
		FindScripts();
		switch (typ)
		{
		case 0:
			if (float.IsNaN(genres_.genres_RES_POINTS_LEFT[slot]))
			{
				Complete();
			}
			if (genres_.genres_RES_POINTS_LEFT[slot] > 0f)
			{
				genres_.genres_RES_POINTS_LEFT[slot] -= f;
				if (genres_.genres_RES_POINTS_LEFT[slot] <= 0f)
				{
					genres_.genres_RES_POINTS_LEFT[slot] = 0f;
					Complete();
				}
			}
			break;
		case 1:
			if (float.IsNaN(themes_.themes_RES_POINTS_LEFT[slot]))
			{
				Complete();
			}
			if (themes_.themes_RES_POINTS_LEFT[slot] > 0f)
			{
				themes_.themes_RES_POINTS_LEFT[slot] -= f;
				if (themes_.themes_RES_POINTS_LEFT[slot] <= 0f)
				{
					themes_.themes_RES_POINTS_LEFT[slot] = 0f;
					Complete();
				}
			}
			break;
		case 2:
			if (float.IsNaN(eF_.engineFeatures_RES_POINTS_LEFT[slot]))
			{
				Complete();
			}
			if (eF_.engineFeatures_RES_POINTS_LEFT[slot] > 0f)
			{
				eF_.engineFeatures_RES_POINTS_LEFT[slot] -= f;
				if (eF_.engineFeatures_RES_POINTS_LEFT[slot] <= 0f)
				{
					eF_.engineFeatures_RES_POINTS_LEFT[slot] = 0f;
					Complete();
				}
			}
			break;
		case 3:
			if (float.IsNaN(gF_.gameplayFeatures_RES_POINTS_LEFT[slot]))
			{
				Complete();
			}
			if (gF_.gameplayFeatures_RES_POINTS_LEFT[slot] > 0f)
			{
				gF_.gameplayFeatures_RES_POINTS_LEFT[slot] -= f;
				if (gF_.gameplayFeatures_RES_POINTS_LEFT[slot] <= 0f)
				{
					gF_.gameplayFeatures_RES_POINTS_LEFT[slot] = 0f;
					Complete();
				}
			}
			break;
		case 4:
			if (float.IsNaN(hardware_.hardware_RES_POINTS_LEFT[slot]))
			{
				Complete();
			}
			if (hardware_.hardware_RES_POINTS_LEFT[slot] > 0f)
			{
				hardware_.hardware_RES_POINTS_LEFT[slot] -= f;
				if (hardware_.hardware_RES_POINTS_LEFT[slot] <= 0f)
				{
					hardware_.hardware_RES_POINTS_LEFT[slot] = 0f;
					Complete();
				}
			}
			break;
		case 5:
			if (float.IsNaN(fS_.RES_POINTS_LEFT[slot]))
			{
				Complete();
			}
			if (fS_.RES_POINTS_LEFT[slot] > 0f)
			{
				fS_.RES_POINTS_LEFT[slot] -= f;
				if (fS_.RES_POINTS_LEFT[slot] <= 0f)
				{
					fS_.RES_POINTS_LEFT[slot] = 0f;
					Complete();
				}
			}
			break;
		case 6:
			if (float.IsNaN(hardwareFeatures_.hardFeat_RES_POINTS_LEFT[slot]))
			{
				Complete();
			}
			if (hardwareFeatures_.hardFeat_RES_POINTS_LEFT[slot] > 0f)
			{
				hardwareFeatures_.hardFeat_RES_POINTS_LEFT[slot] -= f;
				if (hardwareFeatures_.hardFeat_RES_POINTS_LEFT[slot] <= 0f)
				{
					hardwareFeatures_.hardFeat_RES_POINTS_LEFT[slot] = 0f;
					Complete();
				}
			}
			break;
		}
	}

	private void Complete()
	{
		roomScript roomScript2 = FindMyRoomWithTask();
		if (!roomScript2)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		int roomID_ = roomScript2.myID;
		string text = "";
		switch (typ)
		{
		case 0:
			text = tS_.GetText(165) + "\n<b>" + genres_.GetName(slot) + "</b>";
			guiMain_.CreateLeftNews(roomID_, genres_.GetPic(slot), text, rdS_.roomData_SPRITE[2]);
			break;
		case 1:
			text = tS_.GetText(165) + "\n<b>" + tS_.GetThemes(slot) + "</b>";
			guiMain_.CreateLeftNews(roomID_, themes_.icon, text, rdS_.roomData_SPRITE[2]);
			break;
		case 2:
			text = tS_.GetText(165) + "\n<b>" + eF_.GetName(slot) + "</b>";
			guiMain_.CreateLeftNews(roomID_, eF_.GetTypPic(slot), text, rdS_.roomData_SPRITE[2]);
			break;
		case 3:
			text = tS_.GetText(165) + "\n<b>" + gF_.GetName(slot) + "</b>";
			guiMain_.CreateLeftNews(roomID_, gF_.GetTypSprite(slot), text, rdS_.roomData_SPRITE[2]);
			break;
		case 4:
			text = tS_.GetText(165) + "\n<b>" + hardware_.GetName(slot) + "</b>";
			guiMain_.CreateLeftNews(roomID_, hardware_.GetTypPic(slot), text, rdS_.roomData_SPRITE[2]);
			break;
		case 5:
			text = tS_.GetText(165) + "\n<b>" + fS_.GetName(slot) + "</b>";
			guiMain_.CreateLeftNews(roomID_, fS_.RES_SPRITE[slot], text, rdS_.roomData_SPRITE[2]);
			break;
		case 6:
			text = tS_.GetText(165) + "\n<b>" + hardwareFeatures_.GetName(slot) + "</b>";
			guiMain_.CreateLeftNews(roomID_, hardwareFeatures_.GetSprite(slot), text, rdS_.roomData_SPRITE[2]);
			break;
		}
		unlock_.CheckUnlock(showMessage: true);
		if (mS_.multiplayer && (bool)mS_.mpCalls_)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Forschung(mS_.myID);
			}
			else
			{
				mS_.mpCalls_.CLIENT_Send_Forschung();
			}
		}
		if (DoAutomatic())
		{
			return;
		}
		if (automatic && automaticWait)
		{
			taskForschungWait taskForschungWait2 = guiMain_.AddTask_ForschungWait();
			taskForschungWait2.Init(fromSavegame: false);
			taskForschungWait2.typ = typ;
			roomScript2.taskID = taskForschungWait2.myID;
			Debug.Log("ForschungWait");
		}
		if (!automatic && !automaticWait && autoForschung)
		{
			taskAutoForschung taskAutoForschung2 = guiMain_.AddTask_AutoForschung();
			taskAutoForschung2.Init(fromSavegame: false);
			for (int i = 0; i < kategorie.Length; i++)
			{
				taskAutoForschung2.kategorie[i] = kategorie[i];
			}
			roomScript2.taskID = taskAutoForschung2.myID;
			Debug.Log("AutoForschung");
		}
		Object.Destroy(base.gameObject);
	}

	private roomScript FindMyRoomWithTask()
	{
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].taskID == myID)
			{
				return mS_.arrayRoomScripts[i];
			}
		}
		return null;
	}

	private bool DoAutomatic()
	{
		if (!automatic)
		{
			return false;
		}
		bool flag = false;
		int num = 0;
		switch (typ)
		{
		case 0:
			for (num = 0; num < genres_.genres_RES_POINTS_LEFT.Length; num++)
			{
				if (genres_.genres_UNLOCK[num])
				{
					if (genres_.genres_RES_POINTS_LEFT[num] > 0f)
					{
						flag = true;
					}
					if (genres_.genres_RES_POINTS_LEFT[num] > 0f && genres_.Pay(num) && !genres_.BereitsInAnderenRaumAktiv(num))
					{
						slot = num;
						return true;
					}
				}
			}
			break;
		case 1:
			for (num = 0; num < themes_.themes_RES_POINTS_LEFT.Length; num++)
			{
				if (themes_.themes_RES_POINTS_LEFT[num] > 0f)
				{
					flag = true;
				}
				if (themes_.themes_RES_POINTS_LEFT[num] > 0f && themes_.Pay(num) && !themes_.BereitsInAnderenRaumAktiv(num))
				{
					slot = num;
					return true;
				}
			}
			break;
		case 2:
			for (num = 0; num < eF_.engineFeatures_RES_POINTS_LEFT.Length; num++)
			{
				if (eF_.engineFeatures_UNLOCK[num])
				{
					if (eF_.engineFeatures_RES_POINTS_LEFT[num] > 0f)
					{
						flag = true;
					}
					if (eF_.engineFeatures_RES_POINTS_LEFT[num] > 0f && eF_.Pay(num) && !eF_.BereitsInAnderenRaumAktiv(num))
					{
						slot = num;
						return true;
					}
				}
			}
			break;
		case 3:
			for (num = 0; num < gF_.gameplayFeatures_RES_POINTS_LEFT.Length; num++)
			{
				if (gF_.gameplayFeatures_UNLOCK[num])
				{
					if (gF_.gameplayFeatures_RES_POINTS_LEFT[num] > 0f)
					{
						flag = true;
					}
					if (gF_.gameplayFeatures_RES_POINTS_LEFT[num] > 0f && gF_.Pay(num) && !gF_.BereitsInAnderenRaumAktiv(num))
					{
						slot = num;
						return true;
					}
				}
			}
			break;
		case 4:
			for (num = 0; num < hardware_.hardware_RES_POINTS_LEFT.Length; num++)
			{
				if (hardware_.hardware_UNLOCK[num])
				{
					if (hardware_.hardware_RES_POINTS_LEFT[num] > 0f)
					{
						flag = true;
					}
					if (hardware_.hardware_RES_POINTS_LEFT[num] > 0f && hardware_.Pay(num) && !hardware_.BereitsInAnderenRaumAktiv(num))
					{
						slot = num;
						return true;
					}
				}
			}
			break;
		case 5:
		{
			int amountForschung = guiMain_.uiObjects[21].GetComponent<Menu_Forschung>().GetAmountForschung(5, getUnerforschtesObjekt: true);
			if (amountForschung != -1 && !fS_.BereitsInAnderenRaumAktiv(amountForschung))
			{
				if (fS_.RES_POINTS_LEFT[amountForschung] > 0f)
				{
					flag = true;
				}
				if (fS_.RES_POINTS_LEFT[amountForschung] > 0f && fS_.Pay(amountForschung))
				{
					slot = amountForschung;
					return true;
				}
			}
			break;
		}
		case 6:
			for (num = 0; num < hardwareFeatures_.hardFeat_RES_POINTS_LEFT.Length; num++)
			{
				if (hardwareFeatures_.hardFeat_UNLOCK[num])
				{
					if (hardwareFeatures_.hardFeat_RES_POINTS_LEFT[num] > 0f)
					{
						flag = true;
					}
					if (hardwareFeatures_.hardFeat_RES_POINTS_LEFT[num] > 0f && hardwareFeatures_.Pay(num) && !hardwareFeatures_.BereitsInAnderenRaumAktiv(num))
					{
						slot = num;
						return true;
					}
				}
			}
			break;
		}
		if (flag)
		{
			LeftNews(tS_.GetText(728), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[2]);
		}
		else
		{
			LeftNews(tS_.GetText(1376), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[2]);
		}
		return false;
	}

	private void LeftNews(string c, Sprite icon, Sprite iconRoom)
	{
		int roomID_ = -1;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
		for (int i = 0; i < array.Length; i++)
		{
			roomScript component = array[i].GetComponent<roomScript>();
			if ((bool)component && component.taskID == myID)
			{
				roomID_ = component.myID;
				break;
			}
		}
		guiMain_.CreateLeftNews(roomID_, icon, c, iconRoom);
	}

	public int GetRueckgeld()
	{
		int result = 0;
		switch (typ)
		{
		case 0:
			if (!genres_.ForschungGestartet(slot))
			{
				result = genres_.GetPrice(slot);
			}
			break;
		case 1:
			if (!themes_.ForschungGestartet(slot))
			{
				result = themes_.GetPrice(slot);
			}
			break;
		case 2:
			if (!eF_.ForschungGestartet(slot))
			{
				result = eF_.GetPrice(slot);
			}
			break;
		case 3:
			if (!gF_.ForschungGestartet(slot))
			{
				result = gF_.GetPrice(slot);
			}
			break;
		case 4:
			if (!hardware_.ForschungGestartet(slot))
			{
				result = hardware_.GetPrice(slot);
			}
			break;
		case 5:
			if (!fS_.ForschungGestartet(slot))
			{
				result = fS_.GetPrice(slot);
			}
			break;
		case 6:
			if (!hardwareFeatures_.ForschungGestartet(slot))
			{
				result = hardwareFeatures_.GetPrice(slot);
			}
			break;
		}
		return result;
	}

	public void Abbrechen()
	{
		int rueckgeld = GetRueckgeld();
		if (rueckgeld > 0)
		{
			mS_.Earn(rueckgeld, 1);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
			for (int i = 0; i < array.Length; i++)
			{
				roomScript component = array[i].GetComponent<roomScript>();
				if ((bool)component && component.taskID == myID)
				{
					guiMain_.MoneyPop(rueckgeld, new Vector3(component.uiPos.x, component.uiPos.y + 3f, component.uiPos.z), green: true);
					break;
				}
			}
		}
		Object.Destroy(base.gameObject);
	}
}
