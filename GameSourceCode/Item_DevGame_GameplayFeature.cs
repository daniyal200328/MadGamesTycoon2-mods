using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_GameplayFeature : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public gameplayFeatures gF_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public Menu_DevGame menuDevGame_;

	public int goodBad = 1;

	private Button myButton;

	private void Start()
	{
		SetData();
		FindScripts();
	}

	private void FindScripts()
	{
		if (!myButton)
		{
			myButton = GetComponent<Button>();
		}
		if (!menuDevGame_)
		{
			menuDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
	}

	private void Update()
	{
		if (!menuDevGame_)
		{
			menuDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
		if (menuDevGame_.g_GameGameplayFeatures[myID])
		{
			GetComponent<Image>().color = guiMain_.colors[4];
		}
		else
		{
			GetComponent<Image>().color = Color.white;
		}
		SetPlattformLock();
		SetGoodBadIcon();
		tooltip_.c = gF_.GetTooltip(myID, menuDevGame_.g_GameMainGenre, menuDevGame_.g_GameSubGenre);
	}

	private void SetData()
	{
		FindScripts();
		uiObjects[0].GetComponent<Text>().text = gF_.GetName(myID);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(gF_.GetDevCosts(myID), showDollar: true);
		uiObjects[2].GetComponent<Image>().sprite = gF_.GetTypSprite(myID);
		guiMain_.DrawStars(uiObjects[3], gF_.gameplayFeatures_LEVEL[myID]);
		if (gF_.gameplayFeatures_INTERNET[myID] && !uiObjects[5].activeSelf)
		{
			uiObjects[5].SetActive(value: true);
		}
		if (!gF_.gameplayFeatures_INTERNET[myID] && uiObjects[5].activeSelf)
		{
			uiObjects[5].SetActive(value: false);
		}
		tooltip_.c = gF_.GetTooltip(myID, menuDevGame_.g_GameMainGenre, menuDevGame_.g_GameSubGenre);
		SetGoodBadIcon();
	}

	public void BUTTON_Click()
	{
		FindScripts();
		SetPlattformLock();
		if (!myButton.interactable)
		{
			GetComponent<Image>().color = Color.white;
			menuDevGame_.DisableGameplayFeature(myID);
			return;
		}
		sfx_.PlaySound(3, force: false);
		if (menuDevGame_.SetGameplayFeature(myID))
		{
			GetComponent<Image>().color = guiMain_.colors[4];
		}
		else
		{
			GetComponent<Image>().color = Color.white;
		}
	}

	private void SetGoodBadIcon()
	{
		if (gF_.GetBonus(myID, menuDevGame_.g_GameMainGenre, menuDevGame_.g_GameSubGenre) > 1.1f)
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[36];
			goodBad = 2;
		}
		else if (gF_.GetBonus(myID, menuDevGame_.g_GameMainGenre, menuDevGame_.g_GameSubGenre) < 0.9f)
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[38];
			goodBad = 0;
		}
		else
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[37];
			goodBad = 1;
		}
	}

	private void SetPlattformLock()
	{
		int value = menuDevGame_.uiObjects[146].GetComponent<Dropdown>().value;
		if (value == 4 && gF_.gameplayFeatures_LOCKPLATFORM[myID, 4])
		{
			myButton.interactable = false;
		}
		else if (value == 5 && gF_.gameplayFeatures_LOCKPLATFORM[myID, 3])
		{
			myButton.interactable = false;
		}
		else
		{
			myButton.interactable = true;
		}
	}
}
