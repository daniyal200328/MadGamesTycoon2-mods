using UnityEngine;
using UnityEngine.UI;

public class Menu_W_Restbestand : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private games games_;

	private gameScript game_;

	private Menu_LagerSelect menu_LagerSelect;

	private int money;

	private float updateTimer;

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
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!menu_LagerSelect)
		{
			menu_LagerSelect = guiMain_.uiObjects[225].GetComponent<Menu_LagerSelect>();
		}
	}

	private void Update()
	{
		MultiplayerUpdate();
	}

	private void MultiplayerUpdate()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 1f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	public void Init(gameScript gS_)
	{
		FindScripts();
		_ = (bool)gS_;
		game_ = gS_;
		SetData();
	}

	private void SetData()
	{
		if ((bool)game_)
		{
			money = GetSumme(game_);
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(money, showDollar: true);
			return;
		}
		uiObjects[0].GetComponent<Text>().text = "";
		money = 0;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			gameScript component = array[i].GetComponent<gameScript>();
			if ((bool)component && menu_LagerSelect.CheckGameData(component))
			{
				money += GetSumme(component);
			}
		}
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(money, showDollar: true);
		if (money <= 0)
		{
			BUTTON_Abbrechen();
		}
	}

	private int GetSumme(gameScript script_)
	{
		if (!script_)
		{
			return 0;
		}
		_ = script_.reviewTotal;
		_ = 10;
		return Mathf.RoundToInt(0.029000001f * (float)script_.reviewTotal * (float)script_.GetLagerbestand());
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if ((bool)game_)
		{
			mS_.Earn(money, 1);
			game_.umsatzTotal += money;
			for (int i = 0; i < game_.lagerbestand.Length; i++)
			{
				game_.lagerbestand[i] = 0L;
			}
			games_.LagerplatzVerteilen();
			BUTTON_Abbrechen();
			return;
		}
		mS_.Earn(money, 1);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int j = 0; j < array.Length; j++)
		{
			gameScript component = array[j].GetComponent<gameScript>();
			if ((bool)component && menu_LagerSelect.CheckGameData(component))
			{
				component.umsatzTotal += money;
				for (int k = 0; k < component.lagerbestand.Length; k++)
				{
					component.lagerbestand[k] = 0L;
				}
			}
		}
		games_.LagerplatzVerteilen();
		BUTTON_Abbrechen();
	}
}
