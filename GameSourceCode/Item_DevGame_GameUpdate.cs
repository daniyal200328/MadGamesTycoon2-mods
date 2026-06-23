using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_GameUpdate : MonoBehaviour
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
			if (game_.pubOffer)
			{
				uiObjects[0].GetComponent<Text>().color = guiMain_.colors[23];
			}
			uiObjects[3].GetComponent<Text>().text = game_.GetReleaseDateString();
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(275) + ": " + mS_.GetMoney(game_.sellsTotal, showDollar: false);
			uiObjects[5].GetComponent<Text>().text = tS_.GetText(277) + ": " + Mathf.RoundToInt(game_.reviewTotal) + "%";
			uiObjects[2].GetComponent<Image>().sprite = game_.GetScreenshot();
			uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
			uiObjects[1].GetComponent<tooltip>().c = genres_.GetName(game_.maingenre);
			uiObjects[6].GetComponent<Text>().text = game_.amountUpdates.ToString();
			uiObjects[7].GetComponent<Image>().sprite = game_.GetTypSprite();
			uiObjects[8].GetComponent<Image>().sprite = game_.GetPlatformTypSprite();
			uiObjects[10].GetComponent<Text>().text = game_.points_bugs.ToString();
			if (game_.points_bugs <= 0f && uiObjects[9].activeSelf)
			{
				uiObjects[9].SetActive(value: false);
			}
			GetComponent<tooltip>().c = game_.GetTooltip();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[108]);
		guiMain_.uiObjects[108].GetComponent<Menu_Dev_Update>().Init(rS_, game_);
		guiMain_.uiObjects[105].SetActive(value: false);
	}
}
