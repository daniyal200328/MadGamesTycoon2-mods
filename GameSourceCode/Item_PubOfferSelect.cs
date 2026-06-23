using UnityEngine;
using UnityEngine.UI;

public class Item_PubOfferSelect : MonoBehaviour
{
	public gameScript game_;

	public GameObject[] uiObjects;

	public Sprite[] iconStimmung;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public games games_;

	private float stimmungOLD;

	public int review;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		if (!game_)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (game_.isOnMarket || !game_.pubAngebot)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		if (game_.pubAngebot_Stimmung <= 0f || game_.pubAnbgebot_Inivs)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		uiObjects[5].GetComponent<Text>().text = tS_.GetText(1730) + ": <color=#BE0000>" + mS_.GetMoney(game_.PUBOFFER_GetGarantiesumme(), showDollar: true) + "</color>";
		uiObjects[6].GetComponent<Text>().text = tS_.GetText(1731) + ": <color=#BE0000>" + Mathf.RoundToInt(game_.PUBOFFER_GetGewinnbeteiligung()) + "%</color>";
		if (stimmungOLD != game_.pubAngebot_Stimmung)
		{
			stimmungOLD = game_.pubAngebot_Stimmung;
			if (game_.pubAngebot_Stimmung < 33f)
			{
				uiObjects[9].GetComponent<Image>().sprite = iconStimmung[2];
			}
			if (game_.pubAngebot_Stimmung > 33f && game_.pubAngebot_Stimmung < 66f)
			{
				uiObjects[9].GetComponent<Image>().sprite = iconStimmung[1];
			}
			if (game_.pubAngebot_Stimmung > 66f)
			{
				uiObjects[9].GetComponent<Image>().sprite = iconStimmung[0];
			}
		}
	}

	private void SetData()
	{
		if (!game_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
		uiObjects[2].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
		uiObjects[3].GetComponent<Image>().sprite = games_.gameSizeSprites[game_.gameSize];
		uiObjects[4].GetComponent<Image>().sprite = game_.GetScreenshot();
		uiObjects[10].GetComponent<Text>().text = game_.PUBOFFER_GetRetailDigitalString();
		uiObjects[15].GetComponent<Text>().text = mS_.Round(game_.GetIpBekanntheit(), 1).ToString();
		uiObjects[7].GetComponent<Image>().sprite = game_.GetDeveloperLogo();
		game_.FindMyPlatforms();
		for (int i = 0; i < game_.gamePlatform.Length; i++)
		{
			platformScript platformScript2 = game_.gamePlatformScript[i];
			if ((bool)platformScript2)
			{
				platformScript2.SetPic(uiObjects[11 + i]);
			}
			else
			{
				uiObjects[11 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			}
		}
		UpdateReview();
		tooltip_.c = game_.PUBOFFER_GetTooltip(review);
	}

	public void UpdateReview()
	{
		review = game_.reviewTotal;
		if (review <= 0)
		{
			game_.CalcReview(entwicklungsbericht: true);
			review = game_.reviewTotal;
			game_.ClearReview();
		}
		guiMain_.DrawStars(uiObjects[8], Mathf.RoundToInt(review / 20));
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		if ((bool)game_ && !game_.isOnMarket && game_.pubAngebot)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[350]);
			guiMain_.uiObjects[350].GetComponent<MenuPublishingOfferVerhandlung>().Init(game_);
		}
	}

	public void BUTTON_Delete()
	{
		sfx_.PlaySound(3, force: true);
		game_.pubAnbgebot_Inivs = true;
		mS_.publishingOfferMain_.amountPublishingOffers--;
		Object.Destroy(base.gameObject);
	}
}
