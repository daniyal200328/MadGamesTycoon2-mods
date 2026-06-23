using System.IO;
using System.Text;
using UnityEngine;

public class themes : MonoBehaviour
{
	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private genres genres_;

	public Sprite icon;

	public int RES_POINTS;

	public int PRICE;

	public Sprite[] spriteMarkt;

	public float[] themes_RES_POINTS_LEFT;

	public int[] themes_LEVEL;

	public bool[,] themes_FITGENRE;

	public int[] themes_MGSR;

	public int[] themes_MARKT;

	public int[] themes_USES;

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
		if (!genres_)
		{
			genres_ = GetComponent<genres>();
		}
	}

	public void AddUses(int i)
	{
		if (i >= 0 && i < themes_USES.Length)
		{
			themes_USES[i]++;
		}
	}

	public void Init()
	{
		FindScripts();
		InitArrays(tS_.themes_EN.Length);
	}

	public bool[] Return1DimensionArray_FITGENRE()
	{
		int num = themes_LEVEL.Length;
		int num2 = genres_.genres_UNLOCK.Length;
		bool[] array = new bool[num * num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				array[i * num2 + j] = themes_FITGENRE[i, j];
			}
		}
		return array;
	}

	public void Copy2DimensionArray_FITGENRE(bool[] arr)
	{
		int num = themes_LEVEL.Length;
		int num2 = arr.Length / num;
		themes_FITGENRE = new bool[num, num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				themes_FITGENRE[i, j] = arr[i * num2 + j];
			}
		}
	}

	public void InitArrays(int length_)
	{
		FindScripts();
		themes_RES_POINTS_LEFT = new float[length_];
		themes_LEVEL = new int[length_];
		themes_MARKT = new int[length_];
		themes_USES = new int[length_];
		for (int i = 0; i < themes_RES_POINTS_LEFT.Length; i++)
		{
			themes_RES_POINTS_LEFT[i] = RES_POINTS;
			themes_LEVEL[i] = 0;
		}
	}

	private string OpenFile(string filename)
	{
		StreamReader streamReader = new StreamReader(Application.dataPath + "/Extern/Text/" + filename, Encoding.Unicode);
		string result = streamReader.ReadToEnd();
		streamReader.Close();
		return result;
	}

	public void Load_THEMES_MGSR(string filename)
	{
		_ = genres_.genres_LEVEL.Length;
		string[] data = OpenFile(filename).Split("\n"[0]);
		data = tS_.RemoveComments(data);
		themes_MGSR = new int[data.Length];
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i].Contains("<M1>"))
			{
				themes_MGSR[i] = 1;
			}
			if (data[i].Contains("<M2>"))
			{
				themes_MGSR[i] = 2;
			}
			if (data[i].Contains("<M3>"))
			{
				themes_MGSR[i] = 3;
			}
			if (data[i].Contains("<M4>"))
			{
				themes_MGSR[i] = 4;
			}
			if (data[i].Contains("<M5>"))
			{
				themes_MGSR[i] = 5;
			}
		}
	}

	public void Load_FITGENRE(string filename)
	{
		int num = genres_.genres_LEVEL.Length;
		string[] data = OpenFile(filename).Split("\n"[0]);
		data = tS_.RemoveComments(data);
		themes_FITGENRE = new bool[data.Length, num];
		for (int i = 0; i < data.Length; i++)
		{
			for (int j = 0; j < num; j++)
			{
				if (data[i].Contains("<" + j + ">"))
				{
					themes_FITGENRE[i, j] = true;
				}
			}
		}
	}

	public bool IsThemesFitWithGenre(int theme_, int genre_)
	{
		if (theme_ < 0 || genre_ < 0)
		{
			return false;
		}
		if (theme_ > themes_LEVEL.Length)
		{
			return false;
		}
		return themes_FITGENRE[theme_, genre_];
	}

	public int GetPrice(int i)
	{
		return PRICE;
	}

	public bool IsErforscht(int i)
	{
		if (i > themes_RES_POINTS_LEFT.Length)
		{
			return false;
		}
		if (themes_RES_POINTS_LEFT[i] <= 0f)
		{
			return true;
		}
		return false;
	}

	public float GetProzent(int i)
	{
		if (i > themes_RES_POINTS_LEFT.Length)
		{
			return 0f;
		}
		return 100f / (float)RES_POINTS * ((float)RES_POINTS - themes_RES_POINTS_LEFT[i]);
	}

	public bool ForschungGestartet(int i)
	{
		if (i > themes_RES_POINTS_LEFT.Length)
		{
			return false;
		}
		if (themes_RES_POINTS_LEFT[i] == (float)RES_POINTS)
		{
			return false;
		}
		return true;
	}

	public bool Pay(int i)
	{
		if (!ForschungGestartet(i))
		{
			if (mS_.NotEnoughMoney(PRICE))
			{
				return false;
			}
			mS_.Pay(PRICE, 2);
		}
		return true;
	}

	public bool BereitsInAnderenRaumAktiv(int s)
	{
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].typ == 2 && (bool)mS_.arrayRoomScripts[i].taskGameObject)
			{
				taskForschung taskForschung2 = mS_.arrayRoomScripts[i].GetTaskForschung();
				if ((bool)taskForschung2 && taskForschung2.slot == s && taskForschung2.typ == 1)
				{
					return true;
				}
			}
		}
		return false;
	}

	public string GetTooltip(int i)
	{
		if (i > themes_LEVEL.Length)
		{
			return "";
		}
		return "<b>" + tS_.GetThemes(i) + "</b>";
	}

	public Sprite GetSpriteMarkt(int i)
	{
		if (i > themes_MARKT.Length)
		{
			return spriteMarkt[0];
		}
		int num = themes_MARKT[i];
		if (num <= 2)
		{
			return spriteMarkt[0];
		}
		if (num > 2 && num <= 4)
		{
			return spriteMarkt[1];
		}
		if (num > 4)
		{
			return spriteMarkt[2];
		}
		return spriteMarkt[0];
	}

	public void ResearchAll()
	{
		for (int i = 0; i < themes_RES_POINTS_LEFT.Length; i++)
		{
			themes_RES_POINTS_LEFT[i] = 0f;
		}
	}

	public void MaxLevelAll()
	{
		for (int i = 0; i < themes_LEVEL.Length; i++)
		{
			themes_LEVEL[i] = 5;
		}
	}
}
