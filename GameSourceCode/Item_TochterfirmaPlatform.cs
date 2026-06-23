using UnityEngine;
using UnityEngine.UI;

public class Item_TochterfirmaPlatform : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public platformScript pS_;

	public publisherScript pubS_;

	public int slot;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		pS_.SetPic(uiObjects[1]);
		uiObjects[2].GetComponent<Text>().text = pS_.GetDateString();
		uiObjects[3].GetComponent<Text>().text = tS_.GetText(220) + ": " + pS_.GetGames();
		uiObjects[4].GetComponent<Text>().text = tS_.GetText(219) + ": " + pS_.GetMarktanteilString();
		uiObjects[5].GetComponent<Text>().text = mS_.GetMoney(pS_.GetPrice(), showDollar: true);
		uiObjects[6].GetComponent<Text>().text = pS_.tech.ToString();
		if (pS_.internet)
		{
			uiObjects[10].SetActive(value: true);
		}
		else
		{
			uiObjects[10].SetActive(value: false);
		}
		guiMain_.DrawStars(uiObjects[7], pS_.erfahrung);
		uiObjects[9].GetComponent<Image>().sprite = pS_.GetComplexSprite();
		tooltip_.c = pS_.GetTooltip();
		uiObjects[11].GetComponent<Image>().sprite = pS_.GetTypSprite();
		uiObjects[11].GetComponent<tooltip>().c = pS_.GetTypString();
		if (pS_.vomMarktGenommen)
		{
			GetComponent<Image>().color = guiMain_.colors[2];
		}
		else
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
		pubS_.tf_platformFocus[slot] = myID;
		guiMain_.uiObjects[393].GetComponent<Menu_Stats_TochterfirmaSettings>().UpdateData();
		guiMain_.uiObjects[402].GetComponent<Menu_Stats_TochterfirmaPlatform>().BUTTON_Close();
	}
}
