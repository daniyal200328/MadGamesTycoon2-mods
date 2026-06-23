using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_Platform : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public platformScript pS_;

	public Menu_DevGame devGame_;

	public Menu_Dev_ChangePlatform changePlatform_;

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
		uiObjects[5].GetComponent<Text>().text = tS_.GetText(375) + ": " + mS_.GetMoney(pS_.GetDevCosts(), showDollar: true);
		if (pS_.OwnerIsNPC())
		{
			uiObjects[11].GetComponent<Text>().text = tS_.GetText(1926) + ": -" + Mathf.RoundToInt(pS_.GetExklusivBonus()) + "%";
		}
		else if (pS_.ownerID == mS_.myID)
		{
			uiObjects[11].GetComponent<Text>().text = "<color=blue>" + tS_.GetText(2379) + "</color>";
		}
		else
		{
			uiObjects[11].GetComponent<Text>().text = "";
		}
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
		if ((bool)devGame_ && (devGame_.g_GamePlatform[0] == pS_.myID || devGame_.g_GamePlatform[1] == pS_.myID || devGame_.g_GamePlatform[2] == pS_.myID || devGame_.g_GamePlatform[3] == pS_.myID))
		{
			uiObjects[3].SetActive(value: true);
			tooltip obj = tooltip_;
			obj.c = obj.c + "\n\n<color=red><b>" + tS_.GetText(379) + "</b></color>";
			base.gameObject.GetComponent<Button>().interactable = false;
			return;
		}
		if ((bool)changePlatform_ && (changePlatform_.g_GamePlatform[0] == pS_.myID || changePlatform_.g_GamePlatform[1] == pS_.myID || changePlatform_.g_GamePlatform[2] == pS_.myID || changePlatform_.g_GamePlatform[3] == pS_.myID))
		{
			uiObjects[3].SetActive(value: true);
			tooltip obj2 = tooltip_;
			obj2.c = obj2.c + "\n\n<color=red><b>" + tS_.GetText(379) + "</b></color>";
			base.gameObject.GetComponent<Button>().interactable = false;
			return;
		}
		if (pS_.ownerID == mS_.myID && !pS_.isUnlocked)
		{
			uiObjects[3].SetActive(value: true);
			tooltip obj3 = tooltip_;
			obj3.c = obj3.c + "\n\n<color=red><b>" + tS_.GetText(1633) + "</b></color>";
		}
		if ((bool)devGame_)
		{
			devGame_.GetEngineTechLevel();
		}
		if ((bool)changePlatform_)
		{
			changePlatform_.GetEngineTechLevel();
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
		if ((bool)devGame_)
		{
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetPlatform(guiMain_.uiObjects[66].GetComponent<Menu_DevGame_Platform>().platformNR, myID);
		}
		if ((bool)changePlatform_)
		{
			guiMain_.uiObjects[102].GetComponent<Menu_Dev_ChangePlatform>().SetPlatform(guiMain_.uiObjects[66].GetComponent<Menu_DevGame_Platform>().platformNR, myID, init: false);
		}
		guiMain_.uiObjects[66].GetComponent<Menu_DevGame_Platform>().BUTTON_Close();
	}

	private bool IsExclusivGame()
	{
		if (guiMain_.uiObjects[56].activeSelf && devGame_.uiObjects[146].GetComponent<Dropdown>().value == 1)
		{
			return true;
		}
		return false;
	}
}
