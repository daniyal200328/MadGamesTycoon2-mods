using UnityEngine;
using UnityEngine.UI;

public class Item_Stats_Publisher : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public genres genres_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public publisherScript pS_;

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
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
		uiObjects[2].GetComponent<Image>().sprite = genres_.GetPic(pS_.fanGenre);
		uiObjects[5].GetComponent<Text>().text = "$" + mS_.Round(pS_.share, 1);
		uiObjects[11].GetComponent<Text>().text = pS_.GetFirmenwertString();
		guiMain_.DrawStars(uiObjects[3], Mathf.RoundToInt(pS_.stars / 20f));
		guiMain_.DrawStarsColor(uiObjects[4], Mathf.RoundToInt(pS_.GetRelation() / 20f), guiMain_.colors[5]);
		tooltip_.c = pS_.GetTooltip();
		if (pS_.KaufangebotFree())
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
		if (pS_.IsTochterfirma())
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
			if (!uiObjects[7].activeSelf)
			{
				uiObjects[7].SetActive(value: true);
			}
		}
		else if (uiObjects[7].activeSelf)
		{
			uiObjects[7].SetActive(value: false);
		}
		if (pS_.Geschlossen())
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
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[373]);
		guiMain_.uiObjects[373].GetComponent<Menu_Stats_Publisher_Main>().Init(pS_);
	}
}
