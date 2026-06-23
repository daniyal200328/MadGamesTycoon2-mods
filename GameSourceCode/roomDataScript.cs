using UnityEngine;

public class roomDataScript : MonoBehaviour
{
	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	public int[] roomData_PRICE;

	public Sprite[] roomData_SPRITE;

	public const int floor = 0;

	public const int entwicklung = 1;

	public const int forschung = 2;

	public const int qa = 3;

	public const int grafikstudio = 4;

	public const int soundstudio = 5;

	public const int marketing = 6;

	public const int support = 7;

	public const int hardware = 8;

	public const int lager = 9;

	public const int motion = 10;

	public const int wc = 11;

	public const int aufenthalt = 12;

	public const int training = 13;

	public const int produktion = 14;

	public const int server = 15;

	public const int free = 16;

	public const int werkstatt = 17;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
			mS_ = main_.GetComponent<mainScript>();
			tS_ = main_.GetComponent<textScript>();
		}
	}

	public int GetPrice(int i)
	{
		return roomData_PRICE[i];
	}

	public string GetName(int i)
	{
		return i switch
		{
			1 => tS_.GetText(19), 
			2 => tS_.GetText(20), 
			3 => tS_.GetText(21), 
			4 => tS_.GetText(22), 
			5 => tS_.GetText(23), 
			6 => tS_.GetText(24), 
			7 => tS_.GetText(25), 
			8 => tS_.GetText(26), 
			9 => tS_.GetText(27), 
			10 => tS_.GetText(28), 
			11 => tS_.GetText(29), 
			12 => tS_.GetText(30), 
			13 => tS_.GetText(31), 
			14 => tS_.GetText(32), 
			15 => tS_.GetText(33), 
			17 => tS_.GetText(1508), 
			_ => "<Missing>", 
		};
	}

	public bool KeineMitarbeiter(int roomTyp)
	{
		bool result = false;
		switch (roomTyp)
		{
		case 0:
			result = true;
			break;
		case 9:
			result = true;
			break;
		case 11:
			result = true;
			break;
		case 12:
			result = true;
			break;
		case 14:
			result = true;
			break;
		case 15:
			result = true;
			break;
		case 16:
			result = true;
			break;
		}
		return result;
	}
}
