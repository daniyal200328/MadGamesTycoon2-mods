using UnityEngine;
using UnityEngine.UI;

public class Menu_Trendsetter : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
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
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(gameScript script_)
	{
		FindScripts();
		sfx_.PlaySound(31);
		if ((bool)script_)
		{
			string text = tS_.GetText(760);
			text = text.Replace("<NAME>", script_.GetNameWithTag());
			uiObjects[0].GetComponent<Text>().text = text;
			uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(script_.maingenre);
			uiObjects[2].GetComponent<Text>().text = genres_.GetName(script_.maingenre);
			uiObjects[3].GetComponent<Text>().text = tS_.GetThemes(script_.gameMainTheme);
			script_.trendsetter = true;
			mS_.trendWeeks = Random.Range(50, 100);
			mS_.trendGenre = script_.maingenre;
			int num = 0;
			bool flag = false;
			while (!flag)
			{
				mS_.trendAntiGenre = Random.Range(0, genres_.genres_LEVEL.Length);
				if (genres_.genres_UNLOCK[mS_.trendAntiGenre] && mS_.trendAntiGenre != mS_.trendGenre)
				{
					flag = true;
				}
				num++;
				if (num > 10000)
				{
					flag = true;
				}
			}
			mS_.trendTheme = script_.gameMainTheme;
			mS_.trendAntiTheme = Random.Range(0, themes_.themes_LEVEL.Length);
			if (mS_.trendAntiTheme == mS_.trendTheme)
			{
				if (mS_.trendAntiTheme > 0)
				{
					mS_.trendAntiTheme--;
				}
				else
				{
					mS_.trendAntiTheme++;
				}
			}
			if (!mS_.myPubS_)
			{
				mS_.FindMyPublisherScript();
			}
			mS_.AddAwards(6, mS_.myPubS_);
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_Trend();
				}
				if (mS_.mpCalls_.isClient)
				{
					mS_.mpCalls_.CLIENT_Send_Trend();
				}
			}
		}
		else
		{
			BUTTON_Abbrechen();
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
		guiMain_.CreateTopNewsTrend(genres_.GetName(mS_.trendGenre) + " / " + tS_.GetThemes(mS_.trendTheme), genres_.GetPic(mS_.trendGenre));
	}

	public void BUTTON_Yes()
	{
		BUTTON_Abbrechen();
	}
}
