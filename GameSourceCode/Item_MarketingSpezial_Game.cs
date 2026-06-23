using UnityEngine;
using UnityEngine.UI;

public class Item_MarketingSpezial_Game : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		if ((bool)game_ && !game_.inDevelopment && !game_.isOnMarket && !game_.schublade)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			MultiplayerUpdate();
		}
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

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[1].GetComponent<Text>().text = Mathf.RoundToInt(game_.GetHype()).ToString();
			uiObjects[2].GetComponent<Text>().text = game_.GetReleaseDateString();
			if (game_.specialMarketing[0] == 0)
			{
				uiObjects[3].GetComponent<Image>().color = guiMain_.colors[6];
			}
			if (game_.specialMarketing[1] == 0)
			{
				uiObjects[4].GetComponent<Image>().color = guiMain_.colors[6];
			}
			if (game_.specialMarketing[2] == 0)
			{
				uiObjects[5].GetComponent<Image>().color = guiMain_.colors[6];
			}
			if (game_.specialMarketing[3] == 0)
			{
				uiObjects[6].GetComponent<Image>().color = guiMain_.colors[6];
			}
			if (game_.specialMarketing[4] == 0)
			{
				uiObjects[7].GetComponent<Image>().color = guiMain_.colors[6];
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
		guiMain_.uiObjects[295].SetActive(value: false);
		guiMain_.uiObjects[294].GetComponent<Menu_MarketingSpezial>().SetGame(game_);
	}
}
