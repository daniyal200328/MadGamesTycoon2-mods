using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_Remaster : MonoBehaviour
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

	public games games_;

	public themes themes_;

	public unlockScript unlock_;

	private float updateTimer;

	private void Start()
	{
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
			uiObjects[6].GetComponent<Image>().sprite = games_.gameSizeSprites[game_.gameSize];
			uiObjects[7].GetComponent<Image>().sprite = game_.GetPlatformTypSprite();
			uiObjects[8].GetComponent<Text>().text = mS_.Round(game_.GetIpBekanntheit(), 1).ToString();
			tooltip_.c = game_.GetTooltip();
			if (game_.maingenre != -1 && !genres_.IsErforscht(game_.maingenre))
			{
				GetComponent<Button>().interactable = false;
				uiObjects[3].GetComponent<Text>().text = "<color=yellow>" + tS_.GetText(2017) + "</color>";
			}
			if (game_.subgenre != -1 && !genres_.IsErforscht(game_.subgenre))
			{
				GetComponent<Button>().interactable = false;
				uiObjects[3].GetComponent<Text>().text = "<color=yellow>" + tS_.GetText(2017) + "</color>";
			}
			if (game_.gameMainTheme != -1 && !themes_.IsErforscht(game_.gameMainTheme))
			{
				GetComponent<Button>().interactable = false;
				uiObjects[3].GetComponent<Text>().text = "<color=yellow>" + tS_.GetText(2017) + "</color>";
			}
			if (game_.gameSubTheme != -1 && !themes_.IsErforscht(game_.gameSubTheme))
			{
				GetComponent<Button>().interactable = false;
				uiObjects[3].GetComponent<Text>().text = "<color=yellow>" + tS_.GetText(2017) + "</color>";
			}
			if ((game_.gameTyp == 1 && !unlock_.Get(21)) || (game_.gameTyp == 2 && !unlock_.Get(22)))
			{
				GetComponent<Button>().interactable = false;
				uiObjects[3].GetComponent<Text>().text = "<color=yellow>" + tS_.GetText(2018) + "</color>";
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[441]);
		guiMain_.uiObjects[441].GetComponent<Menu_Dev_Platform_Remaster>().Init(rS_, game_);
		guiMain_.uiObjects[98].SetActive(value: false);
	}
}
