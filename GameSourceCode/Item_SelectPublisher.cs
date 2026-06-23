using UnityEngine;
using UnityEngine.UI;

public class Item_SelectPublisher : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public genres genres_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public publisherScript pS_;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		if ((bool)mS_ && mS_.multiplayer && (bool)pS_)
		{
			if (pS_.IsTochterfirmaVonMitspieler())
			{
				Object.Destroy(base.gameObject);
			}
			else if (pS_.Geschlossen())
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
		uiObjects[2].GetComponent<Image>().sprite = genres_.GetPic(pS_.fanGenre);
		uiObjects[5].GetComponent<Text>().text = "$" + pS_.GetShare();
		guiMain_.DrawStars(uiObjects[3], Mathf.RoundToInt(pS_.stars / 20f));
		guiMain_.DrawStarsColor(uiObjects[4], Mathf.RoundToInt(pS_.GetRelation() / 20f), guiMain_.colors[5]);
		tooltip_.c = pS_.GetTooltip();
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
			if (!uiObjects[7].activeSelf)
			{
				uiObjects[7].SetActive(value: true);
			}
		}
		else if (uiObjects[7].activeSelf)
		{
			uiObjects[7].SetActive(value: false);
		}
		if (!pS_.isPlayer && !pS_.IsTochterfirma())
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
		guiMain_.uiObjects[70].GetComponent<Menu_Dev_SelectPublisher>().SelectPublisher(pS_.myID);
	}
}
