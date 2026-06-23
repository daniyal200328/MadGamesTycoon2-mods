using UnityEngine;
using UnityEngine.UI;

public class Item_DevKonsole_Platform : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public platformScript pS_;

	public Menu_Dev_Konsole menu_;

	public Menu_Dev_KonsolePlatforms menuSelectPlatform_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		pS_.SetPic(uiObjects[1]);
		string dateString = pS_.GetDateString();
		dateString = dateString + "\n" + tS_.GetText(220) + ": <b>" + pS_.GetGames() + "</b>";
		dateString = dateString + "\n" + tS_.GetText(219) + ": <b>" + pS_.GetMarktanteilString() + "</b>";
		uiObjects[2].GetComponent<Text>().text = dateString;
		uiObjects[6].GetComponent<Text>().text = pS_.tech.ToString();
		guiMain_.DrawStars(uiObjects[7], pS_.erfahrung);
		uiObjects[9].GetComponent<Image>().sprite = pS_.GetComplexSprite();
		if (pS_.internet)
		{
			uiObjects[4].SetActive(value: true);
		}
		else
		{
			uiObjects[4].SetActive(value: false);
		}
		tooltip_.c = pS_.GetTooltip();
		uiObjects[10].GetComponent<Image>().sprite = pS_.GetTypSprite();
		uiObjects[10].GetComponent<tooltip>().c = pS_.GetTypString();
		if (pS_.vomMarktGenommen)
		{
			uiObjects[8].SetActive(value: true);
		}
		else
		{
			uiObjects[8].SetActive(value: false);
		}
	}

	private void Update()
	{
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 1f))
		{
			updateTimer = 0f;
			SetData();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: false);
		if ((bool)menu_)
		{
			menu_.SetPlatform(menuSelectPlatform_.platformNR, myID);
		}
		menuSelectPlatform_.BUTTON_Close();
	}
}
