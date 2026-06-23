using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class reviewText : MonoBehaviour
{
	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private games games_;

	private List<string> gameplayList = new List<string>();

	private List<string> grafikList = new List<string>();

	private List<string> soundList = new List<string>();

	private List<string> steuerungList = new List<string>();

	private List<string> totalList = new List<string>();

	private string[] data;

	private void Awake()
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
		if (!games_)
		{
			games_ = GetComponent<games>();
		}
	}

	public string GetReviewText(gameScript game_)
	{
		string text = "";
		StreamReader streamReader = new StreamReader(string.Concat(str2: settings_.language switch
		{
			0 => "EN/Review_EN.txt", 
			1 => "GE/Review_GE.txt", 
			2 => "TU/Review_TU.txt", 
			3 => "CH/Review_CH.txt", 
			4 => "FR/Review_FR.txt", 
			5 => "ES/Review_ES.txt", 
			6 => "KO/Review_KO.txt", 
			7 => "PB/Review_PB.txt", 
			8 => "HU/Review_HU.txt", 
			9 => "RU/Review_RU.txt", 
			10 => "CT/Review_CT.txt", 
			11 => "PL/Review_PL.txt", 
			12 => "CZ/Review_CZ.txt", 
			13 => "AR/Review_AR.txt", 
			14 => "IT/Review_IT.txt", 
			15 => "RO/Review_RO.txt", 
			16 => "JA/Review_JA.txt", 
			17 => "UA/Review_UA.txt", 
			18 => "LA/Review_LA.txt", 
			19 => "TH/Review_TH.txt", 
			_ => "EN/Review_EN.txt", 
		}, str0: Application.dataPath, str1: "/Extern/Text/"), Encoding.Unicode);
		string text2 = streamReader.ReadToEnd();
		streamReader.Close();
		data = text2.Split("\n"[0]);
		int num = game_.reviewGameplay / 10 * 10 + 10;
		int num2 = game_.reviewGrafik / 10 * 10 + 10;
		int num3 = game_.reviewSound / 10 * 10 + 10;
		int num4 = game_.reviewSteuerung / 10 * 10 + 10;
		int num5 = game_.reviewTotal / 10 * 10 + 10;
		if (num > 100)
		{
			num = 100;
		}
		if (num2 > 100)
		{
			num2 = 100;
		}
		if (num3 > 100)
		{
			num3 = 100;
		}
		if (num4 > 100)
		{
			num4 = 100;
		}
		if (num5 > 100)
		{
			num5 = 100;
		}
		gameplayList.Clear();
		grafikList.Clear();
		soundList.Clear();
		steuerungList.Clear();
		totalList.Clear();
		for (int i = 0; i < data.Length; i++)
		{
			if (ParseData("[GAMEPLAY_" + num + "]", i))
			{
				GetStrings(i, 0);
			}
			if (ParseData("[GRAPHIC_" + num2 + "]", i))
			{
				GetStrings(i, 1);
			}
			if (ParseData("[SOUND_" + num3 + "]", i))
			{
				GetStrings(i, 2);
			}
			if (ParseData("[CONTROL_" + num4 + "]", i))
			{
				GetStrings(i, 3);
			}
			if (ParseData("[TOTAL_" + num5 + "]", i))
			{
				GetStrings(i, 4);
			}
		}
		if (game_.reviewTotalText == -1)
		{
			game_.reviewGameplayText = Random.Range(0, gameplayList.Count);
			game_.reviewGrafikText = Random.Range(0, grafikList.Count);
			game_.reviewSoundText = Random.Range(0, soundList.Count);
			game_.reviewSteuerungText = Random.Range(0, steuerungList.Count);
			game_.reviewTotalText = Random.Range(0, totalList.Count);
			if (totalList.Count <= game_.reviewTotalText)
			{
				game_.reviewTotalText = totalList.Count - 1;
			}
			if (gameplayList.Count <= game_.reviewGameplayText)
			{
				game_.reviewGameplayText = gameplayList.Count - 1;
			}
			if (grafikList.Count <= game_.reviewGrafikText)
			{
				game_.reviewGrafikText = grafikList.Count - 1;
			}
			if (soundList.Count <= game_.reviewSoundText)
			{
				game_.reviewSoundText = soundList.Count - 1;
			}
			if (steuerungList.Count <= game_.reviewSteuerungText)
			{
				game_.reviewSteuerungText = steuerungList.Count - 1;
			}
		}
		string text3 = "";
		if (game_.exklusiv)
		{
			platformScript platformScript2 = FindPlatformScriptWithID(game_.gamePlatform[0]);
			if ((bool)platformScript2)
			{
				text3 = tS_.GetText(909) + " ";
				text3 = text3.Replace("<NAME>", platformScript2.GetName());
			}
		}
		if (game_.retro)
		{
			platformScript platformScript3 = FindPlatformScriptWithID(game_.gamePlatform[0]);
			if ((bool)platformScript3)
			{
				text3 = tS_.GetText(910) + " ";
				text3 = text3.Replace("<NAME>", platformScript3.GetName());
			}
		}
		if (game_.typ_nachfolger)
		{
			gameScript gameScript2 = FindGameScriptWithID(game_.originalGameID);
			if ((bool)gameScript2)
			{
				text3 = tS_.GetText(1539) + " ";
				text3 = text3.Replace("<NAME>", gameScript2.GetNameWithTag());
			}
		}
		if (game_.typ_spinoff)
		{
			gameScript gameScript3 = FindGameScriptWithID(game_.originalGameID);
			if ((bool)gameScript3)
			{
				text3 = tS_.GetText(1540) + " ";
				text3 = text3.Replace("<NAME>", gameScript3.GetNameWithTag());
			}
		}
		if (game_.portID != -1)
		{
			gameScript gameScript4 = FindGameScriptWithID(game_.portID);
			if ((bool)gameScript4)
			{
				text3 = tS_.GetText(2066) + " ";
				text3 = text3.Replace("<NAME>", gameScript4.GetNameWithTag());
			}
		}
		if (totalList.Count <= game_.reviewTotalText)
		{
			game_.reviewTotalText = totalList.Count - 1;
		}
		if (gameplayList.Count <= game_.reviewGameplayText)
		{
			game_.reviewGameplayText = gameplayList.Count - 1;
		}
		if (grafikList.Count <= game_.reviewGrafikText)
		{
			game_.reviewGrafikText = grafikList.Count - 1;
		}
		if (soundList.Count <= game_.reviewSoundText)
		{
			game_.reviewSoundText = soundList.Count - 1;
		}
		if (steuerungList.Count <= game_.reviewSteuerungText)
		{
			game_.reviewSteuerungText = steuerungList.Count - 1;
		}
		if (gameplayList.Count >= game_.reviewGameplayText)
		{
			text3 += gameplayList[game_.reviewGameplayText];
		}
		if (grafikList.Count >= game_.reviewGrafikText)
		{
			text3 = text3 + " " + grafikList[game_.reviewGrafikText];
		}
		if (soundList.Count >= game_.reviewSoundText)
		{
			text3 = text3 + " " + soundList[game_.reviewSoundText];
		}
		if (steuerungList.Count >= game_.reviewSteuerungText)
		{
			text3 = text3 + " " + steuerungList[game_.reviewSteuerungText];
		}
		if (game_.finanzierung_Grundkosten < 75 || game_.finanzierung_Kontent < 75 || game_.finanzierung_Technology < 75)
		{
			text3 = text3 + " " + tS_.GetText(1193);
		}
		return text3 + " " + totalList[game_.reviewTotalText];
	}

	private platformScript FindPlatformScriptWithID(int id_)
	{
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID == id_)
			{
				return mS_.arrayPlatformsScripts[i];
			}
		}
		return null;
	}

	private gameScript FindGameScriptWithID(int id_)
	{
		if (!games_)
		{
			FindScripts();
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].myID == id_)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private void GetStrings(int i, int what)
	{
		for (int j = i + 1; j < i + 100 && !ParseData("[END]", j); j++)
		{
			switch (what)
			{
			case 0:
				gameplayList.Add(data[j]);
				break;
			case 1:
				grafikList.Add(data[j]);
				break;
			case 2:
				soundList.Add(data[j]);
				break;
			case 3:
				steuerungList.Add(data[j]);
				break;
			case 4:
				totalList.Add(data[j]);
				break;
			}
		}
	}

	private bool ParseData(string c, int i)
	{
		if (data[i].Contains(c))
		{
			return true;
		}
		return false;
	}
}
