using UnityEngine;

public class contractWork : MonoBehaviour
{
	public GameObject main_;

	public mainScript mS_;

	public textScript tS_;

	public int myID;

	public bool angenommen;

	public int typ;

	public int gehalt;

	public int strafe;

	public int auftraggeberID = -1;

	public int zeitInWochen;

	public int wochenAlsAngebot;

	public float points;

	public int art;

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
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
	}

	public void Init()
	{
		base.name = "CONTRACTWORK_" + myID;
	}

	public string GetName()
	{
		if (art == 6)
		{
			return tS_.GetText(1560);
		}
		if (art == 5)
		{
			return tS_.GetText(1130);
		}
		return tS_.GetContractWork(typ);
	}

	public int GetGehalt()
	{
		return gehalt;
	}

	public int GetStrafe()
	{
		return strafe;
	}

	public int GetWochen()
	{
		return zeitInWochen;
	}

	public float GetArbeitsaufwand()
	{
		return points;
	}

	public float GetAuftragsansehen()
	{
		if (art == 5)
		{
			return 0.1f;
		}
		return points * 0.001f;
	}

	public string GetTooltip()
	{
		return "<b>" + GetName() + "</b>";
	}

	public bool IsAngenommen()
	{
		return angenommen;
	}
}
