using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_ShowConcept : MonoBehaviour
{
	public gameScript game_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	private float updateTimer;

	private void Start()
	{
		if ((bool)game_)
		{
			uiObjects[7].GetComponent<Toggle>().isOn = game_.spielbericht_favorit;
		}
		SetData();
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
			if (!(updateTimer < 5f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	private void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[3].GetComponent<Text>().text = game_.GetReleaseDateString();
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(275) + ": " + mS_.GetMoney(game_.sellsTotal, showDollar: false);
			uiObjects[5].GetComponent<Text>().text = tS_.GetText(277) + ": " + Mathf.RoundToInt(game_.reviewTotal) + "%";
			uiObjects[2].GetComponent<Image>().sprite = game_.GetScreenshot();
			uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
			uiObjects[1].GetComponent<tooltip>().c = genres_.GetName(game_.maingenre);
			uiObjects[6].GetComponent<Image>().sprite = game_.GetTypSprite();
			if (game_.subgenre == -1)
			{
				uiObjects[8].GetComponent<Text>().text = game_.GetGenreString();
			}
			else
			{
				uiObjects[8].GetComponent<Text>().text = game_.GetGenreString() + " / " + game_.GetSubGenreString();
			}
			tooltip_.c = game_.GetTooltip();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[110]);
		guiMain_.uiObjects[110].GetComponent<Menu_Dev_ShowConcept>().Init(game_);
	}

	public void TOGGLE_Favorit()
	{
		if ((bool)game_)
		{
			game_.spielbericht_favorit = uiObjects[7].GetComponent<Toggle>().isOn;
		}
	}
}
