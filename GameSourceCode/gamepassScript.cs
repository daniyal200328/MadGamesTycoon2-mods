using UnityEngine;

public class gamepassScript : MonoBehaviour
{
	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private GUI_Main guiMain_;

	private games games_;

	private themes themes_;

	public bool gamePass_aktiv;

	public string gamePass_name = "";

	public int gamePass_AboPreis = 5;

	public int gamePass_AboPreisOld = 5;

	public long gamePass_UmsatzJahr;

	public long gamePass_Abos;

	public long gamePass_AbosLetzteWoche;

	public long[] gamePass_aboVerlaufWoche = new long[20];

	public long[] gamePass_aboVerlaufMonat = new long[24];

	public long[] gamePass_umsatzVerlaufMonat = new long[24];

	public float gamePass_marktPC;

	public float gamePass_marktKonsole;

	public float gamePass_marktHandheld;

	public long gamePass_userGesamt;

	public int gamePass_AmountGames;

	public int gamePass_AmountGamesAktiv;

	public int gamePass_AmountGamesOnMarket;

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
		if (!games_)
		{
			games_ = GetComponent<games>();
		}
		if (!themes_)
		{
			themes_ = GetComponent<themes>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public string GetName()
	{
		return gamePass_name;
	}

	public void GAMEPASS_AddGame(gameScript script_, bool updateGamesAmount)
	{
		if ((bool)script_)
		{
			script_.inGamePass = true;
			script_.gamePassPlayer = 0;
			if (updateGamesAmount)
			{
				GetAmountGamePassGames();
			}
		}
	}

	public void GAMEPASS_RemoveGame(gameScript script_, bool updateGamesAmount)
	{
		if ((bool)script_)
		{
			script_.inGamePass = false;
			script_.gamePassPlayer = 0;
			if (updateGamesAmount)
			{
				GetAmountGamePassGames();
			}
		}
	}

	public void UpdatePassivePlatforms()
	{
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].inGamePassPassiv)
			{
				mS_.arrayPlatformsScripts[i].inGamePassPassiv = false;
			}
		}
		for (int j = 0; j < mS_.arrayPlatformsScripts.Length; j++)
		{
			if (!mS_.arrayPlatformsScripts[j] || !mS_.arrayPlatformsScripts[j].inGamePass)
			{
				continue;
			}
			for (int k = 0; k < mS_.arrayPlatformsScripts[j].platformCompatible.Length; k++)
			{
				if (mS_.arrayPlatformsScripts[j].platformCompatible[k] > 0)
				{
					if (!mS_.arrayPlatformsScripts[j].platformCompatibleScript[k])
					{
						mS_.arrayPlatformsScripts[j].FindMyPlatformsCompatible();
					}
					if ((bool)mS_.arrayPlatformsScripts[j].platformCompatibleScript[k])
					{
						mS_.arrayPlatformsScripts[j].platformCompatibleScript[k].inGamePassPassiv = true;
					}
				}
			}
		}
	}

	public void GAMEPASS_AddPlatform(platformScript script_)
	{
		if ((bool)script_)
		{
			script_.inGamePass = true;
			UpdatePassivePlatforms();
			GAMEPASS_UpdateMarketshare();
			GetAmountGamePassGames();
		}
	}

	public void GAMEPASS_RemovePlatform(platformScript script_)
	{
		if ((bool)script_)
		{
			script_.inGamePass = false;
			UpdatePassivePlatforms();
			GAMEPASS_UpdateMarketshare();
			GetAmountGamePassGames();
		}
	}

	public void GAMEPASS_UpdateMarketshare()
	{
		gamePass_marktPC = 0f;
		gamePass_marktKonsole = 0f;
		gamePass_marktHandheld = 0f;
		gamePass_userGesamt = 0L;
		if (!gamePass_aktiv)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].inGamePass && mS_.arrayPlatformsScripts[i].CanBeInGamePass(ignoreGames: false))
			{
				gamePass_userGesamt += mS_.arrayPlatformsScripts[i].GetAktiveNutzer();
				switch (mS_.arrayPlatformsScripts[i].typ)
				{
				case 0:
					gamePass_marktPC += mS_.arrayPlatformsScripts[i].GetMarktanteil();
					break;
				case 1:
					gamePass_marktKonsole += mS_.arrayPlatformsScripts[i].GetMarktanteil();
					break;
				case 2:
					gamePass_marktHandheld += mS_.arrayPlatformsScripts[i].GetMarktanteil();
					break;
				}
			}
		}
	}

	public float GAMEPASS_GetAktualitaet()
	{
		return (float)gamePass_AmountGamesOnMarket * 0.2f;
	}

	public string GetMostPlayedGame()
	{
		gameScript gameScript2 = null;
		int num = 0;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].inGamePass && games_.arrayGamesScripts[i].gamePassPlayer > num)
			{
				num = games_.arrayGamesScripts[i].gamePassPlayer;
				gameScript2 = games_.arrayGamesScripts[i];
			}
		}
		if ((bool)gameScript2)
		{
			return gameScript2.GetNameWithTag();
		}
		return "---";
	}

	private string GetStarsString(int i)
	{
		if (i > 5)
		{
			return "★★★★★";
		}
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

	public string GetTooltip()
	{
		if (!mS_)
		{
			FindScripts();
		}
		string text = "<b><size=18>" + GetName() + "</size></b>";
		text = text + "\n<b><color=blue>" + tS_.GetText(1243) + "</color></b>\n";
		text = text + "\n" + tS_.GetText(1236) + ": <color=blue>" + mS_.GetMoney(gamePass_Abos, showDollar: false) + "</color>";
		text = ((gamePass_AbosLetzteWoche < 0) ? (text + "\n" + tS_.GetText(1229) + ": <color=red>" + mS_.GetMoney(gamePass_AbosLetzteWoche, showDollar: false) + "</color>") : (text + "\n" + tS_.GetText(483) + ": <color=blue>+" + mS_.GetMoney(gamePass_AbosLetzteWoche, showDollar: false) + "</color>"));
		text = text + "\n" + tS_.GetText(1229) + ": <color=blue>" + mS_.GetMoney(gamePass_AboPreis, showDollar: true) + "</color>";
		text += "\n";
		text = text + "\n" + tS_.GetText(2136) + ": <color=blue>" + mS_.GetMoney(gamePass_UmsatzJahr, showDollar: true) + "</color>";
		text = text + "\n" + tS_.GetText(220) + ": <color=blue>" + mS_.GetMoney(gamePass_AmountGamesAktiv, showDollar: false) + " / " + mS_.GetMoney(gamePass_AmountGames, showDollar: false) + "</color>";
		text = text + "\n\n" + tS_.GetText(2137);
		text = text + "\n<color=blue>" + GetMostPlayedGame() + "</color>";
		text += "\n";
		text = text + "\n" + tS_.GetText(2131);
		text = text + "\n<color=blue><size=18>" + GetStarsString(Mathf.RoundToInt(GAMEPASS_GetAktualitaet() * 5f)) + "</size></color>";
		text += "\n";
		text = text + "\n" + tS_.GetText(2138) + ": <color=blue>" + mS_.Round((float)gamePass_userGesamt / 1000000f, 1) + " " + tS_.GetText(1483) + "</color>";
		text = text + "\n" + tS_.GetText(1479) + ": <color=blue>" + mS_.Round(gamePass_marktPC, 1) + "%</color>";
		text = text + "\n" + tS_.GetText(1480) + ": <color=blue>" + mS_.Round(gamePass_marktKonsole, 1) + "%</color>";
		text = text + "\n" + tS_.GetText(1481) + ": <color=blue>" + mS_.Round(gamePass_marktHandheld, 1) + "%</color>";
		text += "\n";
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].inGamePass && mS_.arrayPlatformsScripts[i].CanBeInGamePass(ignoreGames: false))
			{
				text = text + "\n<color=blue>" + mS_.arrayPlatformsScripts[i].GetName() + "</color>";
			}
		}
		return text;
	}

	public void VerteileAbosAufServer()
	{
		if (!gamePass_aktiv)
		{
			gamePass_Abos = 0L;
			return;
		}
		long num = gamePass_Abos;
		float num2 = gamePass_Abos;
		switch (mS_.difficulty)
		{
		case 0:
			num2 = num2 * 0.01f + (float)Random.Range(1, 20);
			break;
		case 1:
			num2 = num2 * 0.015f + (float)Random.Range(5, 20);
			break;
		case 2:
			num2 = num2 * 0.02f + (float)Random.Range(10, 20);
			break;
		case 3:
			num2 = num2 * 0.025f + (float)Random.Range(15, 20);
			break;
		case 4:
			num2 = num2 * 0.03f + (float)Random.Range(18, 25);
			break;
		case 5:
			num2 = num2 * 0.035f + (float)Random.Range(20, 25);
			break;
		}
		if ((float)gamePass_Abos > 10000000f)
		{
			num2 *= 1.2f;
		}
		if ((float)gamePass_Abos > 50000000f)
		{
			num2 *= 1.2f;
		}
		if ((float)gamePass_Abos > 100000000f)
		{
			num2 *= 1.2f;
		}
		if ((float)gamePass_Abos > 150000000f)
		{
			num2 *= 1.2f;
		}
		if ((float)gamePass_Abos > 200000000f)
		{
			num2 *= 1.2f;
		}
		if ((float)gamePass_Abos > 250000000f)
		{
			num2 *= 1.2f;
		}
		if ((float)gamePass_Abos > 300000000f)
		{
			num2 *= 1.2f;
		}
		gamePass_Abos -= Mathf.RoundToInt(num2);
		switch (gamePass_AboPreis - gamePass_AboPreisOld)
		{
		case 1:
			gamePass_Abos -= gamePass_Abos / 10;
			break;
		case 2:
			gamePass_Abos -= gamePass_Abos / 9;
			break;
		case 3:
			gamePass_Abos -= gamePass_Abos / 8;
			break;
		case 4:
			gamePass_Abos -= gamePass_Abos / 7;
			break;
		case 5:
			gamePass_Abos -= gamePass_Abos / 6;
			break;
		case 6:
			gamePass_Abos -= gamePass_Abos / 5;
			break;
		case 7:
			gamePass_Abos -= gamePass_Abos / 4;
			break;
		case 8:
			gamePass_Abos -= gamePass_Abos / 3;
			break;
		case 9:
			gamePass_Abos -= gamePass_Abos / 2;
			break;
		case 10:
			gamePass_Abos -= gamePass_Abos / 2;
			break;
		}
		gamePass_AboPreisOld = gamePass_AboPreis;
		gamePass_Abos += gamePass_AbosLetzteWoche;
		if (gamePass_Abos > gamePass_userGesamt)
		{
			gamePass_Abos = gamePass_userGesamt;
		}
		long num3 = gamePass_Abos;
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].typ == 15 && (mS_.arrayRoomScripts[i].serverReservieren == 0 || mS_.arrayRoomScripts[i].serverReservieren == 3))
			{
				num3 = mS_.arrayRoomScripts[i].SetAbos(num3);
				if (num3 <= 0)
				{
					break;
				}
			}
		}
		gamePass_Abos -= num3;
		if (gamePass_Abos < 0)
		{
			gamePass_Abos = 0L;
		}
		gamePass_AbosLetzteWoche = gamePass_Abos - num;
	}

	public void GetAmountGamePassGames()
	{
		gamePass_AmountGames = 0;
		gamePass_AmountGamesAktiv = 0;
		gamePass_AmountGamesOnMarket = 0;
		if (!gamePass_aktiv)
		{
			return;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i] || !games_.arrayGamesScripts[i].inGamePass)
			{
				continue;
			}
			if (gamePass_Abos < games_.arrayGamesScripts[i].gamePassPlayer)
			{
				games_.arrayGamesScripts[i].gamePassPlayer = (int)gamePass_Abos;
			}
			gamePass_AmountGames++;
			if (games_.arrayGamesScripts[i].MinEinePlattformIstImGamePass())
			{
				gamePass_AmountGamesAktiv++;
				if (games_.arrayGamesScripts[i].isOnMarket)
				{
					gamePass_AmountGamesOnMarket++;
				}
			}
		}
	}

	public void GetAmountGamePassGamesFast(gameScript script_)
	{
		if (!script_)
		{
			return;
		}
		if (gamePass_Abos < script_.gamePassPlayer)
		{
			script_.gamePassPlayer = (int)gamePass_Abos;
		}
		gamePass_AmountGames++;
		if (script_.MinEinePlattformIstImGamePass())
		{
			gamePass_AmountGamesAktiv++;
			if (script_.isOnMarket)
			{
				gamePass_AmountGamesOnMarket++;
			}
		}
	}

	public void UpdateGamesFormMarket(gameScript script_, float interested_, float gamePass_AboPreis_)
	{
		if ((bool)script_ && !script_.isOnMarket && !script_.inDevelopment)
		{
			script_.gamePassPlayer = 0;
			if (script_.MinEinePlattformIstImGamePass())
			{
				float num = Random.Range(script_.reviewTotal / 2, script_.reviewTotal);
				num *= interested_;
				num *= gamePass_AboPreis_;
				float num2 = Mathf.RoundToInt(gamePass_Abos / 1000000);
				script_.gamePassPlayer = 1 + Mathf.RoundToInt(num) + Mathf.RoundToInt(num * num2);
				gamePass_AbosLetzteWoche += Mathf.RoundToInt(num);
			}
		}
	}
}
