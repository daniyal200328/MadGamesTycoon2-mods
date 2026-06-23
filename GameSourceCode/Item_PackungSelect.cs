using UnityEngine;
using UnityEngine.UI;

public class Item_PackungSelect : MonoBehaviour
{
	public gameScript game_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public roomScript rS_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		DataUpdate();
	}

	private void DataUpdate()
	{
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 1f))
		{
			updateTimer = 0f;
			SetData();
		}
	}

	private void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[1].GetComponent<Image>().sprite = game_.GetScreenshot();
			uiObjects[7].GetComponent<Image>().sprite = game_.GetPlatformTypSprite();
			uiObjects[2].GetComponent<Text>().text = game_.GetLagerbestandString();
			uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(game_.verkaufspreis[0], showDollar: true);
			uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(game_.verkaufspreis[1], showDollar: true);
			uiObjects[5].GetComponent<Text>().text = mS_.GetMoney(game_.verkaufspreis[2], showDollar: true);
			uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(game_.verkaufspreis[3], showDollar: true);
			if (!game_.digitalVersion)
			{
				uiObjects[6].GetComponent<Text>().text = "-";
			}
			if (!game_.retailVersion)
			{
				uiObjects[3].GetComponent<Text>().text = "-";
				uiObjects[4].GetComponent<Text>().text = "-";
				uiObjects[5].GetComponent<Text>().text = "-";
			}
			if (game_.typ_budget)
			{
				uiObjects[4].GetComponent<Text>().text = "-";
				uiObjects[5].GetComponent<Text>().text = "-";
			}
			tooltip_.c = game_.GetTooltip();
			if (game_.arcade)
			{
				uiObjects[4].GetComponent<Text>().text = "-";
				uiObjects[5].GetComponent<Text>().text = "-";
				uiObjects[6].GetComponent<Text>().text = "-";
			}
			if (mS_.multiplayer && !guiMain_.uiObjects[220].GetComponent<Menu_PackungSelect>().CheckGameData(game_))
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		if (game_.handy)
		{
			guiMain_.uiObjects[301].SetActive(value: true);
			guiMain_.uiObjects[301].GetComponent<Menu_HandyPreis>().Init(game_);
		}
		else if (game_.arcade)
		{
			guiMain_.uiObjects[307].SetActive(value: true);
			guiMain_.uiObjects[307].GetComponent<Menu_ArcadePreis>().Init(game_, null);
		}
		else
		{
			guiMain_.uiObjects[218].SetActive(value: true);
			guiMain_.uiObjects[218].GetComponent<Menu_Packung>().Init(game_, null, newGame: false, hideClose: false);
		}
	}
}
