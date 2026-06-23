using UnityEngine;

public class taskForschungWait : MonoBehaviour
{
	public int myID = -1;

	public int typ = -1;

	private float waitTimer = 10f;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private Menu_Forschung menuForschung_;

	private unlockScript unlock_;

	private genres genres_;

	private themes themes_;

	private engineFeatures eF_;

	private gameplayFeatures gF_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeature_;

	private forschungSonstiges fS_;

	private void Awake()
	{
		base.transform.position = new Vector3(280f, 0f, 0f);
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
			if (!menuForschung_)
			{
				menuForschung_ = guiMain_.uiObjects[21].GetComponent<Menu_Forschung>();
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
			if (!hardwareFeature_)
			{
				hardwareFeature_ = main_.GetComponent<hardwareFeatures>();
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

	private void Update()
	{
		AutomaticWait();
	}

	private void AutomaticWait()
	{
		waitTimer += Time.deltaTime;
		if (waitTimer < 5f)
		{
			return;
		}
		waitTimer = 0f;
		if (!menuForschung_ || !guiMain_)
		{
			return;
		}
		int amountForschung = menuForschung_.GetAmountForschung(typ, getUnerforschtesObjekt: true);
		if (amountForschung < 0)
		{
			return;
		}
		switch (typ)
		{
		case 0:
			if (!genres_.Pay(amountForschung))
			{
				return;
			}
			break;
		case 1:
			if (!themes_.Pay(amountForschung))
			{
				return;
			}
			break;
		case 2:
			if (!eF_.Pay(amountForschung))
			{
				return;
			}
			break;
		case 3:
			if (!gF_.Pay(amountForschung))
			{
				return;
			}
			break;
		case 4:
			if (!hardware_.Pay(amountForschung))
			{
				return;
			}
			break;
		case 5:
			if (!fS_.Pay(amountForschung))
			{
				return;
			}
			break;
		case 6:
			if (!hardwareFeature_.Pay(amountForschung))
			{
				return;
			}
			break;
		}
		taskForschung taskForschung2 = guiMain_.AddTask_Forschung();
		taskForschung2.Init(fromSavegame: false);
		taskForschung2.typ = typ;
		taskForschung2.slot = amountForschung;
		taskForschung2.automatic = true;
		taskForschung2.automaticWait = true;
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && (bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].taskID == myID)
			{
				mS_.arrayRoomScripts[i].taskID = taskForschung2.myID;
			}
		}
		Object.Destroy(base.gameObject);
	}

	public void Abbrechen()
	{
		Object.Destroy(base.gameObject);
	}
}
