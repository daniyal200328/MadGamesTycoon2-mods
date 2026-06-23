using UnityEngine;

public class publishingOffer : MonoBehaviour
{
	public GameObject main_;

	public mainScript mS_;

	public textScript tS_;

	public genres genres_;

	public int myID;

	public int garantiesumme;

	public float gewinnbeteiligung;

	public int developer = -1;

	public int wochenAlsAngebot;

	public float review;

	public string gameName = "";

	public int genre;

	public int gameSize;

	public int[] gamePlatform;

	public float points_grafik;

	public float verhandlung;

	public float verhandlungProzent = 100f;

	public float stimmung = 100f;

	public int gameVorbild = -1;

	public bool retail;

	public bool digital;

	public bool angebotWoche;

	public bool nachfolger;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
	}

	public void Init()
	{
		base.name = "PUBOFFER_" + myID;
	}

	public void Delete()
	{
		Object.Destroy(base.gameObject);
	}

	public string GetGameName()
	{
		return gameName;
	}

	public string GetDeveloperName()
	{
		GameObject gameObject = GameObject.Find("PUB_" + developer);
		if ((bool)gameObject)
		{
			return gameObject.GetComponent<publisherScript>().GetName();
		}
		return "<missing>";
	}

	public Sprite GetDeveloperLogo()
	{
		GameObject gameObject = GameObject.Find("PUB_" + developer);
		if ((bool)gameObject)
		{
			return gameObject.GetComponent<publisherScript>().GetLogo();
		}
		return null;
	}

	public Sprite GetScreenshot()
	{
		return genres_.GetScreenshot(genre, Mathf.RoundToInt(points_grafik));
	}

	public platformScript GetPlattformScript(int i)
	{
		GameObject gameObject = GameObject.Find("PLATFORM_" + gamePlatform[i]);
		if ((bool)gameObject)
		{
			return gameObject.GetComponent<platformScript>();
		}
		return null;
	}

	public string GetRetailDigitalString()
	{
		if ((bool)tS_)
		{
			if (retail && digital)
			{
				return tS_.GetText(1746);
			}
			if (retail && !digital)
			{
				return tS_.GetText(1747);
			}
			if (!retail && digital)
			{
				return tS_.GetText(1748);
			}
		}
		return "<missing>";
	}

	public string GetTooltip()
	{
		string text = "<b>" + GetGameName() + "</b>";
		text = text + "\n<color=black>" + GetDeveloperName() + "</color>";
		text = text + "\n<color=magenta>" + GetRetailDigitalString() + "</color>";
		if (gamePlatform[0] > 0)
		{
			text = text + "\n<color=blue>" + GetPlattformScript(0).GetName() + "</color>";
		}
		if (gamePlatform[1] > 0)
		{
			text = text + "\n<color=blue>" + GetPlattformScript(1).GetName() + "</color>";
		}
		if (gamePlatform[2] > 0)
		{
			text = text + "\n<color=blue>" + GetPlattformScript(2).GetName() + "</color>";
		}
		if (gamePlatform[3] > 0)
		{
			text = text + "\n<color=blue>" + GetPlattformScript(3).GetName() + "</color>";
		}
		text += "\n\n";
		text = text + genres_.GetName(genre) + "\n";
		text = text + tS_.GetText(327) + ": <color=blue>" + tS_.GetText(330 + gameSize - 1) + "</color>\n";
		text = text + tS_.GetText(1730) + ": <color=blue>" + mS_.GetMoney(GetGarantiesumme(), showDollar: true) + "</color>\n";
		text = text + tS_.GetText(1731) + ": <color=blue>" + GetGewinnbeteiligung() + "%</color>\n";
		text += "\n";
		int i = Mathf.RoundToInt(review / 20f);
		text = text + tS_.GetText(1732) + "\n<size=21>" + GetQualitatStars(i) + "</size>\n\n";
		text = text + tS_.GetText(1736) + "\n";
		if (stimmung < 33f)
		{
			text = text + "<color=red><b>" + tS_.GetText(1740) + "</b></color>";
		}
		if (stimmung > 33f && stimmung < 66f)
		{
			text = text + "<color=orange><b>" + tS_.GetText(1741) + "</b></color>";
		}
		if (stimmung > 66f)
		{
			text = text + "<color=green><b>" + tS_.GetText(1742) + "</b></color>";
		}
		return text;
	}

	private string GetQualitatStars(int i)
	{
		string text = "";
		return i switch
		{
			0 => "☆☆☆☆☆", 
			1 => "★☆☆☆☆", 
			2 => "★★☆☆☆", 
			3 => "★★★☆☆", 
			4 => "★★★★☆", 
			5 => "★★★★★", 
			_ => "☆☆☆☆☆", 
		};
	}

	public int GetGarantiesumme()
	{
		float num = verhandlungProzent;
		num *= 0.01f;
		return Mathf.RoundToInt((float)garantiesumme * num);
	}

	public int GetGewinnbeteiligung()
	{
		float num = verhandlungProzent;
		num *= 0.01f;
		return Mathf.RoundToInt(gewinnbeteiligung * num);
	}
}
