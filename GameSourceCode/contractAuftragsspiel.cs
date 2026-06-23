using UnityEngine;

public class contractAuftragsspiel : MonoBehaviour
{
	public GameObject main_;

	public mainScript mS_;

	public textScript tS_;

	public int myID;

	public bool angenommen;

	public int gehalt;

	public int bonus;

	public int auftraggeberID = -1;

	public int zeitInWochen;

	public int wochenAlsAngebot;

	public bool zeitAbgelaufen;

	public int mindestbewertung;

	public string gameName = "";

	public int genre;

	public int gameSize;

	public int platform;

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
		base.name = "CONTRACTGAME_" + myID;
	}

	public string GetName()
	{
		return gameName;
	}

	public int GetGehalt()
	{
		return gehalt;
	}

	public int GetBonus()
	{
		return bonus;
	}

	public int GetWochen()
	{
		return zeitInWochen;
	}

	public float GetAuftragsansehen()
	{
		return (float)mindestbewertung * 0.01f;
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
