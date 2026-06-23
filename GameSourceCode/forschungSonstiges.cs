using UnityEngine;
using UnityEngine.UI;

public class forschungSonstiges : MonoBehaviour
{
	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private unlockScript unlock_;

	public float[] RES_POINTS;

	public float[] RES_POINTS_LEFT;

	public int[] RES_PRICE;

	public Sprite[] RES_SPRITE;

	public const int gameSizeBplus = 0;

	public const int gameSizeA = 1;

	public const int gameSizeAA = 2;

	public const int gameSizeAAA = 3;

	public const int grafikStudio1 = 4;

	public const int grafikStudio2 = 5;

	public const int grafikStudio3 = 6;

	public const int grafikStudio4 = 7;

	public const int grafikStudio5 = 8;

	public const int grafikStudio6 = 9;

	public const int soundStudio1 = 10;

	public const int soundStudio2 = 11;

	public const int soundStudio3 = 12;

	public const int soundStudio4 = 13;

	public const int soundStudio5 = 14;

	public const int soundStudio6 = 15;

	public const int motionCapture1 = 16;

	public const int motionCapture2 = 17;

	public const int motionCapture3 = 18;

	public const int motionCapture4 = 19;

	public const int motionCapture5 = 20;

	public const int motionCapture6 = 21;

	public const int gameplayStudio1 = 22;

	public const int gameplayStudio2 = 23;

	public const int gameplayStudio3 = 24;

	public const int gameplayStudio4 = 25;

	public const int gameplayStudio5 = 26;

	public const int gameplayStudio6 = 27;

	public const int room_qa = 28;

	public const int room_support = 29;

	public const int room_marketing = 30;

	public const int room_grafikstudio = 31;

	public const int room_soundstudio = 32;

	public const int room_produktion = 33;

	public const int room_training = 34;

	public const int subgenre = 35;

	public const int subtheme = 36;

	public const int copyProtect = 37;

	public const int room_werkstatt = 38;

	public const int room_hardware = 39;

	public const int gameSizeAAAA = 40;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!mS_)
		{
			mS_ = GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = GetComponent<textScript>();
		}
		if (!settings_)
		{
			settings_ = GetComponent<settingsScript>();
		}
		if (!unlock_)
		{
			unlock_ = GetComponent<unlockScript>();
		}
	}

	public void Unlock(int id_, GameObject lock_, GameObject button_)
	{
		FindScripts();
		if (IsErforscht(id_))
		{
			if ((bool)button_)
			{
				button_.GetComponent<Button>().interactable = true;
			}
			if ((bool)lock_)
			{
				lock_.SetActive(value: false);
			}
		}
		else
		{
			if ((bool)button_)
			{
				button_.GetComponent<Button>().interactable = false;
			}
			if ((bool)lock_)
			{
				lock_.SetActive(value: true);
			}
		}
	}

	public bool IsErforscht(int i)
	{
		if (RES_POINTS_LEFT[i] <= 0f)
		{
			return true;
		}
		return false;
	}

	public string GetName(int i)
	{
		string result = "";
		switch (i)
		{
		case 0:
			result = tS_.GetText(330);
			break;
		case 1:
			result = tS_.GetText(331);
			break;
		case 2:
			result = tS_.GetText(332);
			break;
		case 3:
			result = tS_.GetText(333);
			break;
		case 4:
			result = tS_.GetText(571);
			break;
		case 5:
			result = tS_.GetText(572);
			break;
		case 6:
			result = tS_.GetText(573);
			break;
		case 7:
			result = tS_.GetText(574);
			break;
		case 8:
			result = tS_.GetText(575);
			break;
		case 9:
			result = tS_.GetText(576);
			break;
		case 10:
			result = tS_.GetText(577);
			break;
		case 11:
			result = tS_.GetText(578);
			break;
		case 12:
			result = tS_.GetText(579);
			break;
		case 13:
			result = tS_.GetText(580);
			break;
		case 14:
			result = tS_.GetText(581);
			break;
		case 15:
			result = tS_.GetText(582);
			break;
		case 16:
			result = tS_.GetText(583);
			break;
		case 17:
			result = tS_.GetText(584);
			break;
		case 18:
			result = tS_.GetText(585);
			break;
		case 19:
			result = tS_.GetText(586);
			break;
		case 20:
			result = tS_.GetText(587);
			break;
		case 21:
			result = tS_.GetText(588);
			break;
		case 22:
			result = tS_.GetText(589);
			break;
		case 23:
			result = tS_.GetText(590);
			break;
		case 24:
			result = tS_.GetText(591);
			break;
		case 25:
			result = tS_.GetText(592);
			break;
		case 26:
			result = tS_.GetText(593);
			break;
		case 27:
			result = tS_.GetText(594);
			break;
		case 28:
			result = tS_.GetText(21);
			break;
		case 29:
			result = tS_.GetText(25);
			break;
		case 30:
			result = tS_.GetText(24);
			break;
		case 31:
			result = tS_.GetText(22);
			break;
		case 32:
			result = tS_.GetText(23);
			break;
		case 33:
			result = tS_.GetText(32);
			break;
		case 34:
			result = tS_.GetText(31);
			break;
		case 35:
			result = tS_.GetText(344);
			break;
		case 36:
			result = tS_.GetText(353);
			break;
		case 37:
			result = tS_.GetText(381);
			break;
		case 38:
			result = tS_.GetText(1508);
			break;
		case 39:
			result = tS_.GetText(26);
			break;
		case 40:
			result = tS_.GetText(2193);
			break;
		}
		return result;
	}

	public int GetPrice(int i)
	{
		return RES_PRICE[i];
	}

	public Sprite GetPic(int i)
	{
		return RES_SPRITE[i];
	}

	public float GetProzent(int i)
	{
		return 100f / RES_POINTS[i] * (RES_POINTS[i] - RES_POINTS_LEFT[i]);
	}

	public bool ForschungGestartet(int i)
	{
		if (RES_POINTS_LEFT[i] == RES_POINTS[i])
		{
			return false;
		}
		return true;
	}

	public bool Pay(int i)
	{
		if (!ForschungGestartet(i))
		{
			if (mS_.NotEnoughMoney(RES_PRICE[i]))
			{
				return false;
			}
			mS_.Pay(RES_PRICE[i], 2);
		}
		return true;
	}

	public bool BereitsInAnderenRaumAktiv(int s)
	{
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].typ == 2 && (bool)mS_.arrayRoomScripts[i].taskGameObject)
			{
				taskForschung component = mS_.arrayRoomScripts[i].taskGameObject.GetComponent<taskForschung>();
				if ((bool)component && component.slot == s && component.typ == 5)
				{
					return true;
				}
			}
		}
		return false;
	}

	public string GetTooltip(int i)
	{
		string result = "";
		switch (i)
		{
		case 0:
			result = tS_.GetText(328);
			break;
		case 1:
			result = tS_.GetText(328);
			break;
		case 2:
			result = tS_.GetText(328);
			break;
		case 3:
			result = tS_.GetText(328);
			break;
		case 28:
			result = tS_.GetText(36);
			break;
		case 29:
			result = tS_.GetText(40);
			break;
		case 30:
			result = tS_.GetText(39);
			break;
		case 31:
			result = tS_.GetText(37);
			break;
		case 32:
			result = tS_.GetText(38);
			break;
		case 33:
			result = tS_.GetText(47);
			break;
		case 34:
			result = tS_.GetText(46);
			break;
		case 38:
			result = tS_.GetText(1512);
			break;
		case 39:
			result = tS_.GetText(41);
			break;
		}
		return result;
	}
}
