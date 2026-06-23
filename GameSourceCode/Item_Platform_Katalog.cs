using UnityEngine;
using UnityEngine.UI;

public class Item_Platform_Katalog : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public platformScript pS_;

	public int kompatibleGames;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		pS_.SetPic(uiObjects[1]);
		string dateString = pS_.GetDateString();
		dateString = dateString + "\n" + tS_.GetText(219) + ": <b>" + pS_.GetMarktanteilString() + "</b>";
		uiObjects[2].GetComponent<Text>().text = dateString;
		uiObjects[6].GetComponent<Text>().text = pS_.tech.ToString();
		guiMain_.DrawStars(uiObjects[7], pS_.erfahrung);
		tooltip_.c = pS_.GetTooltip();
		uiObjects[10].GetComponent<Image>().sprite = pS_.GetTypSprite();
		uiObjects[10].GetComponent<tooltip>().c = pS_.GetTypString();
		UpdateKompatibleSpiele();
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void UpdateKompatibleSpiele()
	{
		if ((bool)mS_)
		{
			int amountGamePassGames = pS_.GetAmountGamePassGames();
			kompatibleGames = amountGamePassGames;
			string text = tS_.GetText(2119);
			text = text.Replace("<NUM>", kompatibleGames.ToString());
			uiObjects[5].GetComponent<Text>().text = text;
		}
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[420].SetActive(value: true);
		guiMain_.uiObjects[420].GetComponent<Menu_GamePass_ShowKatalog>().Init(pS_);
	}
}
