using UnityEngine;

public class antiCheatScript : MonoBehaviour
{
	public GameObject main_;

	public mainScript mS_;

	public settingsScript settings_;

	public textScript tS_;

	public int myID;

	public int date_year;

	public int date_month;

	public int price;

	public int dev_costs;

	public string name_EN = "";

	public string name_GE = "";

	public string name_TU = "";

	public string name_CH = "";

	public string name_FR = "";

	public string name_CT = "";

	public string name_RU = "";

	public string name_IT = "";

	public string name_JA = "";

	public string name_UA = "";

	public string name_TH = "";

	public bool isUnlocked;

	public bool inBesitz;

	public float effekt = 100f;

	public bool neverLooseEffect;

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
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
	}

	public void Init()
	{
		base.name = "ANTICHEAT_" + myID;
	}

	public string GetName()
	{
		string text = "";
		text = settings_.language switch
		{
			0 => name_EN, 
			1 => name_GE, 
			2 => name_TU, 
			3 => name_CH, 
			4 => name_FR, 
			9 => name_RU, 
			10 => name_CT, 
			14 => name_IT, 
			16 => name_JA, 
			17 => name_UA, 
			_ => name_EN, 
		};
		if (text == null)
		{
			return name_EN;
		}
		if (text.Length <= 0)
		{
			return name_EN;
		}
		return text;
	}

	public int GetPrice()
	{
		return price;
	}

	public int GetDevCosts()
	{
		return dev_costs;
	}

	public string GetDateString()
	{
		return date_year + " " + tS_.GetText(date_month + 220);
	}

	public string GetTooltip()
	{
		string text = "<b>" + GetName() + "</b>";
		text = text + "\n<color=magenta>" + tS_.GetText(286) + ": " + mS_.Round(effekt, 2) + "%</color>";
		text += "\n";
		text = text + "\n" + tS_.GetText(218) + ": " + mS_.GetMoney(GetPrice(), showDollar: true);
		return text + "\n" + tS_.GetText(6) + ": " + mS_.GetMoney(GetDevCosts(), showDollar: true);
	}

	public bool IsVerfuegbar()
	{
		if (!isUnlocked)
		{
			return false;
		}
		return true;
	}

	public void EffektVerschlechtern()
	{
		if (IsVerfuegbar() && !neverLooseEffect)
		{
			effekt -= 0.2f;
			if (effekt < 0f)
			{
				effekt = 0f;
			}
		}
	}
}
