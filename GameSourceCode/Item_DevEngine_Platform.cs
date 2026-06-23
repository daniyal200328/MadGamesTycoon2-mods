using UnityEngine;
using UnityEngine.UI;

public class Item_DevEngine_Platform : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public platformScript pS_;

	public gameplayFeatures gF_;

	public Menu_Dev_Engine menuDevEngine_;

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
		dateString = dateString + "\n" + tS_.GetText(219) + ": " + pS_.GetMarktanteilString();
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
		dateString = "";
		if (pS_.needFeatures[0] != -1)
		{
			dateString = gF_.GetName(pS_.needFeatures[0]);
		}
		if (pS_.needFeatures[1] != -1)
		{
			dateString = dateString + "\n" + gF_.GetName(pS_.needFeatures[1]);
		}
		if (pS_.needFeatures[2] != -1)
		{
			dateString = dateString + "\n" + gF_.GetName(pS_.needFeatures[2]);
		}
		uiObjects[5].GetComponent<Text>().text = dateString;
		tooltip_.c = pS_.GetTooltip();
		uiObjects[10].GetComponent<Image>().sprite = pS_.GetTypSprite();
		uiObjects[10].GetComponent<tooltip>().c = pS_.GetTypString();
		if (pS_.vomMarktGenommen)
		{
			uiObjects[3].SetActive(value: true);
		}
		else
		{
			uiObjects[8].SetActive(value: false);
		}
		if (pS_.tech < menuDevEngine_.techLevel)
		{
			uiObjects[3].SetActive(value: true);
			tooltip obj = tooltip_;
			obj.c = obj.c + "\n\n<color=red><b>" + tS_.GetText(378) + "</b></color>";
			base.gameObject.GetComponent<Button>().interactable = false;
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
		menuDevEngine_.SetSpezialplatform(myID);
		guiMain_.uiObjects[237].GetComponent<Menu_Dev_EnginePlatform>().BUTTON_Close();
	}
}
