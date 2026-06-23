using UnityEngine;

public class taskFanshop : MonoBehaviour
{
	public int myID = -1;

	public int[] bestellungen;

	public int verdienst;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	public Menu_Fanshop menuFanshop_;

	private void Awake()
	{
		base.transform.position = new Vector3(270f, 0f, 0f);
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
			if (!menuFanshop_)
			{
				menuFanshop_ = guiMain_.uiObjects[367].GetComponent<Menu_Fanshop>();
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
		return 0f;
	}

	public void Work(int artikel, int amount, int v)
	{
		if ((bool)mS_)
		{
			bestellungen[artikel] += amount;
			verdienst += v;
		}
	}

	public void Abbrechen()
	{
		Object.Destroy(base.gameObject);
	}

	public void ResetData()
	{
		verdienst = 0;
		for (int i = 0; i < bestellungen.Length; i++)
		{
			bestellungen[i] = 0;
		}
	}
}
