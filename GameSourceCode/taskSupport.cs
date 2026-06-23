using UnityEngine;

public class taskSupport : MonoBehaviour
{
	public int myID = -1;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private void Awake()
	{
		base.transform.position = new Vector3(200f, 0f, 0f);
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
		if (!mS_)
		{
			FindScripts();
		}
		return mS_.GetAnrufe100Prozent();
	}

	public Sprite GetPic()
	{
		return guiMain_.uiObjects[89].GetComponent<Menu_Marketing_GameKampagne>().sprites[0];
	}

	public void Work(float f, Vector3 pos)
	{
		if ((bool)mS_ && mS_.anrufe > 0)
		{
			int num = 15 + Mathf.RoundToInt(f * 1.5f);
			mS_.anrufe -= num;
			if (mS_.anrufe <= 0)
			{
				mS_.anrufe = 0;
			}
			if (mS_.support_kostenpflichtig)
			{
				int num2 = num * 2;
				mS_.Earn(num2, 14);
				StartCoroutine(guiMain_.MoneyPopEnumerate(num2, pos, green: true));
			}
		}
	}

	public void Abbrechen()
	{
		Object.Destroy(base.gameObject);
	}
}
