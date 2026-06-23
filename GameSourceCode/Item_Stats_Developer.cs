using UnityEngine;
using UnityEngine.UI;

public class Item_Stats_Developer : MonoBehaviour
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
		if ((bool)pS_)
		{
			uiObjects[0].GetComponent<Text>().text = pS_.GetName();
			uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
			guiMain_.DrawStars(uiObjects[3], Mathf.RoundToInt(pS_.stars / 20f));
			uiObjects[2].GetComponent<Text>().text = pS_.GetAmountGames().ToString();
			uiObjects[4].GetComponent<Text>().text = pS_.GetFirmenwertString();
			uiObjects[7].GetComponent<Text>().text = pS_.GetDeveloperPublisherString();
			tooltip_.c = pS_.GetTooltip();
			if (pS_.KaufangebotFree())
			{
				if (!uiObjects[11].activeSelf)
				{
					uiObjects[11].SetActive(value: true);
				}
			}
			else if (uiObjects[11].activeSelf)
			{
				uiObjects[11].SetActive(value: false);
			}
			if (pS_.IsTochterfirma())
			{
				if (!uiObjects[6].activeSelf)
				{
					uiObjects[6].SetActive(value: true);
				}
			}
			else if (uiObjects[6].activeSelf)
			{
				uiObjects[6].SetActive(value: false);
			}
			if (pS_.isPlayer)
			{
				if (!uiObjects[9].activeSelf)
				{
					uiObjects[9].SetActive(value: true);
				}
			}
			else if (uiObjects[9].activeSelf)
			{
				uiObjects[9].SetActive(value: false);
			}
			if (!pS_.isPlayer && !pS_.IsTochterfirma())
			{
				if (!uiObjects[10].activeSelf)
				{
					uiObjects[10].SetActive(value: true);
				}
			}
			else if (uiObjects[10].activeSelf)
			{
				uiObjects[10].SetActive(value: false);
			}
			if (pS_.Geschlossen())
			{
				if (!uiObjects[8].activeSelf)
				{
					uiObjects[8].SetActive(value: true);
				}
			}
			else if (uiObjects[8].activeSelf)
			{
				uiObjects[8].SetActive(value: false);
			}
		}
		else if (mS_.multiplayer && playerID != -1)
		{
			base.gameObject.transform.SetAsFirstSibling();
			base.gameObject.GetComponent<Button>().interactable = false;
			uiObjects[0].GetComponent<Text>().text = mS_.mpCalls_.GetCompanyName(playerID);
			uiObjects[1].GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(mS_.mpCalls_.GetLogo(playerID));
			guiMain_.DrawStars(uiObjects[3], Mathf.RoundToInt(5f));
			uiObjects[2].GetComponent<Text>().text = "---";
			uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(mS_.mpCalls_.GetMoney(playerID), showDollar: true);
			uiObjects[7].GetComponent<Text>().text = tS_.GetText(432) + " & " + tS_.GetText(274);
			tooltip_.c = "";
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[359]);
		guiMain_.uiObjects[359].GetComponent<Menu_Stats_Developer_Main>().Init(pS_);
	}
}
