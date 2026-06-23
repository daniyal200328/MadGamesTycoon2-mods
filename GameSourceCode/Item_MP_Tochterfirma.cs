using UnityEngine;
using UnityEngine.UI;

public class Item_MP_Tochterfirma : MonoBehaviour
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
		if (!pS_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
		guiMain_.DrawStars(uiObjects[3], Mathf.RoundToInt(pS_.stars / 20f));
		uiObjects[2].GetComponent<Text>().text = pS_.GetAmountGames().ToString();
		uiObjects[4].GetComponent<Text>().text = pS_.GetFirmenwertString();
		uiObjects[7].GetComponent<Text>().text = pS_.GetDeveloperPublisherString();
		tooltip_.c = pS_.GetTooltip();
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
