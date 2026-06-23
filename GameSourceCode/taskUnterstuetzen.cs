using UnityEngine;

public class taskUnterstuetzen : MonoBehaviour
{
	public int myID = -1;

	public int roomID = -1;

	public roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private engineFeatures eF_;

	private gameplayFeatures gF_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private sfxScript sfx_;

	private void Awake()
	{
		base.transform.position = new Vector3(250f, 0f, 0f);
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
			if (!eF_)
			{
				eF_ = main_.GetComponent<engineFeatures>();
			}
			if (!gF_)
			{
				gF_ = main_.GetComponent<gameplayFeatures>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!rdS_)
			{
				rdS_ = main_.GetComponent<roomDataScript>();
			}
			if (!sfx_)
			{
				sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
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
		FindMyRoom();
	}

	private void FindMyRoom()
	{
		if (!rS_)
		{
			GameObject gameObject = GameObject.Find("Room_" + roomID);
			if ((bool)gameObject)
			{
				rS_ = gameObject.GetComponent<roomScript>();
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public bool IsCrunchtime()
	{
		if ((bool)rS_)
		{
			return rS_.IsCrunchtimeRead();
		}
		return false;
	}

	public void Work(float f, int what)
	{
	}

	private void CompleteFeature()
	{
	}

	private void Complete()
	{
	}

	public void Abbrechen()
	{
		FindScripts();
		Object.Destroy(base.gameObject);
	}
}
