using UnityEngine;

public class taskMitarbeitersuche : MonoBehaviour
{
	public int myID = -1;

	public int beruf = -1;

	public int berufserfahrung;

	public int perk;

	public bool automatic;

	public float points;

	public float pointsLeft;

	public int geschlecht;

	public bool noBadPerks;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	public gameScript gS_;

	public platformScript pS_;

	private arbeitsmarkt arbeitsmarkt_;

	private void Awake()
	{
		base.transform.position = new Vector3(80f, 0f, 0f);
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
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!rdS_)
			{
				rdS_ = main_.GetComponent<roomDataScript>();
			}
			if (!arbeitsmarkt_)
			{
				arbeitsmarkt_ = main_.GetComponent<arbeitsmarkt>();
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
		return 100f / points * (points - pointsLeft);
	}

	public Sprite GetPic()
	{
		FindScripts();
		return guiMain_.uiSprites[44];
	}

	public void Work(float f)
	{
		if (pointsLeft > 0f)
		{
			pointsLeft -= f;
			if (pointsLeft <= 0f)
			{
				pointsLeft = 0f;
				Complete();
			}
		}
	}

	private void Complete()
	{
		if (mS_.multiplayer && guiMain_.menuOpen)
		{
			pointsLeft = 0.1f;
			return;
		}
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
		float chance = guiMain_.uiObjects[344].GetComponent<Menu_Mitarbeitersuche>().GetChance(berufserfahrung);
		if (Random.Range(0f, 100f) < chance)
		{
			guiMain_.CreateLeftNews(roomID_, GetPic(), tS_.GetText(1719), rdS_.roomData_SPRITE[6]);
			CheckZuVieleMitarbeiterDurchMitarbeitersuche();
			charArbeitsmarkt charArbeitsmarkt2 = arbeitsmarkt_.CreateArbeitsmarktItem();
			if ((bool)charArbeitsmarkt2)
			{
				charArbeitsmarkt2.Create(this);
			}
			guiMain_.uiObjects[345].SetActive(value: true);
			guiMain_.uiObjects[345].GetComponent<Menu_MitarbeitersucheResult>().Init(charArbeitsmarkt2);
			guiMain_.OpenMenu(hideChars: false);
		}
		else
		{
			guiMain_.CreateLeftNews(roomID_, guiMain_.uiSprites[48], tS_.GetText(1718), rdS_.roomData_SPRITE[6]);
		}
		if (!DoAutomatic())
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void CheckZuVieleMitarbeiterDurchMitarbeitersuche()
	{
		int num = 0;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Arbeitsmarkt");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			charArbeitsmarkt component = array[i].GetComponent<charArbeitsmarkt>();
			if ((bool)component && component.mitarbeitersuche)
			{
				num++;
				if (num >= 15)
				{
					array[Random.Range(0, array.Length)].GetComponent<charArbeitsmarkt>().RemoveFromArbeitsmarkt(eingestellt: false);
					Debug.Log("Remove Mitarbeiter " + Random.Range(0, 10000));
					break;
				}
			}
		}
	}

	private bool DoAutomatic()
	{
		if (!automatic)
		{
			return false;
		}
		Menu_Mitarbeitersuche component = guiMain_.uiObjects[344].GetComponent<Menu_Mitarbeitersuche>();
		if (mS_.money < component.price[berufserfahrung])
		{
			LeftNews(tS_.GetText(728), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[6]);
			return false;
		}
		mS_.Pay(component.price[berufserfahrung], 24);
		pointsLeft = points;
		return true;
	}

	private void LeftNews(string c, Sprite icon, Sprite iconRoom)
	{
		int roomID_ = FindMyRoomWithTask();
		guiMain_.CreateLeftNews(roomID_, icon, c, iconRoom);
	}

	private int FindMyRoomWithTask()
	{
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].taskID == myID)
			{
				return mS_.arrayRoomScripts[i].myID;
			}
		}
		return -1;
	}

	public int GetRueckgeld()
	{
		return Mathf.RoundToInt((float)guiMain_.uiObjects[344].GetComponent<Menu_Mitarbeitersuche>().price[berufserfahrung] * ((100f - GetProzent()) * 0.01f));
	}

	public void Abbrechen()
	{
		int rueckgeld = GetRueckgeld();
		if (rueckgeld > 0)
		{
			mS_.Earn(Mathf.RoundToInt(rueckgeld), 1);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
			for (int i = 0; i < array.Length; i++)
			{
				roomScript component = array[i].GetComponent<roomScript>();
				if ((bool)component && component.taskID == myID)
				{
					guiMain_.MoneyPop(Mathf.RoundToInt(rueckgeld), new Vector3(component.uiPos.x, component.uiPos.y + 3f, component.uiPos.z), green: true);
					break;
				}
			}
		}
		Object.Destroy(base.gameObject);
	}
}
