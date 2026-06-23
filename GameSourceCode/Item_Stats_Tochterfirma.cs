using UnityEngine;
using UnityEngine.UI;

public class Item_Stats_Tochterfirma : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public genres genres_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public publisherScript pS_;

	public int playerID = -1;

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
		if (!pS_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
		guiMain_.DrawStars(uiObjects[3], Mathf.RoundToInt(pS_.stars / 20f));
		uiObjects[2].GetComponent<Text>().text = pS_.GetAmountGames().ToString();
		uiObjects[4].GetComponent<Text>().text = pS_.GetFirmenwertString();
		uiObjects[11].GetComponent<Text>().text = mS_.GetMoney(pS_.GetVerwaltungskosten(), showDollar: true);
		uiObjects[12].GetComponent<Image>().sprite = genres_.GetPic(pS_.fanGenre);
		uiObjects[13].GetComponent<Text>().text = mS_.GetMoney(pS_.tf_umsatz_allTime, showDollar: true);
		uiObjects[14].GetComponent<Text>().text = mS_.GetMoney(pS_.GetTochterfirmaUmsatz24Monate(), showDollar: true);
		uiObjects[7].GetComponent<Text>().text = pS_.GetDeveloperPublisherString();
		tooltip_.c = pS_.GetTooltip();
		if (pS_.Geschlossen())
		{
			base.gameObject.GetComponent<Image>().color = guiMain_.colors[25];
		}
		if (pS_.Geschlossen())
		{
			if (!uiObjects[5].activeSelf)
			{
				uiObjects[5].SetActive(value: true);
			}
		}
		else if (uiObjects[5].activeSelf)
		{
			uiObjects[5].SetActive(value: false);
		}
		if (pS_.developer)
		{
			uiObjects[8].GetComponent<Image>().fillAmount = pS_.GetEntwicklungsFortschritt();
			gameScript gameScript2 = pS_.FindGameInDevelopment();
			if ((bool)gameScript2)
			{
				uiObjects[10].GetComponent<Text>().text = gameScript2.GetNameWithTag();
				if (pS_.newGameInWeeks > 2)
				{
					uiObjects[9].GetComponent<Text>().text = tS_.GetText(1944) + ": " + Mathf.RoundToInt(pS_.GetEntwicklungsFortschritt() * 100f) + "%";
					return;
				}
				uiObjects[9].GetComponent<Text>().text = tS_.GetText(1947);
				if ((bool)gameScript2 && gameScript2.HasUnreleasedPlattform())
				{
					uiObjects[9].GetComponent<Text>().text = tS_.GetText(2316);
				}
				uiObjects[8].GetComponent<Image>().fillAmount = 1f;
			}
			else
			{
				uiObjects[10].GetComponent<Text>().text = tS_.GetText(1949);
				uiObjects[9].GetComponent<Text>().text = "0%";
				uiObjects[8].GetComponent<Image>().fillAmount = 0f;
			}
		}
		else
		{
			uiObjects[9].GetComponent<Text>().text = tS_.GetText(1949);
			uiObjects[8].GetComponent<Image>().fillAmount = 0f;
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[387]);
		guiMain_.uiObjects[387].GetComponent<Menu_Stats_Tochterfirma_Main>().Init(pS_);
	}
}
