using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_ChangeGameplayFeature : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public gameplayFeatures gF_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public Menu_Dev_ChangeGameplayFeatures menu_;

	public int goodBad = 1;

	private Button myButton;

	public gameScript gS_;

	private void Start()
	{
		if (uiObjects[5].activeSelf)
		{
			uiObjects[5].SetActive(value: false);
		}
		SetData();
		FindScripts();
	}

	private void FindScripts()
	{
		if (!myButton)
		{
			myButton = GetComponent<Button>();
		}
		if (!menu_)
		{
			menu_ = guiMain_.uiObjects[348].GetComponent<Menu_Dev_ChangeGameplayFeatures>();
		}
	}

	private void Update()
	{
		if (!menu_)
		{
			menu_ = guiMain_.uiObjects[348].GetComponent<Menu_Dev_ChangeGameplayFeatures>();
		}
		if (menu_.g_GameGameplayFeatures[myID])
		{
			GetComponent<Image>().color = guiMain_.colors[4];
		}
		else
		{
			GetComponent<Image>().color = Color.white;
		}
		SetGoodBadIcon();
		tooltip_.c = gF_.GetTooltip(myID, menu_.g_GameMainGenre, menu_.g_GameSubGenre);
	}

	private void SetData()
	{
		FindScripts();
		uiObjects[0].GetComponent<Text>().text = gF_.GetName(myID);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(gF_.GetDevCosts(myID), showDollar: true);
		uiObjects[2].GetComponent<Image>().sprite = gF_.GetTypSprite(myID);
		guiMain_.DrawStars(uiObjects[3], gF_.gameplayFeatures_LEVEL[myID]);
		if (gF_.gameplayFeatures_INTERNET[myID] && !uiObjects[6].activeSelf)
		{
			uiObjects[6].SetActive(value: true);
		}
		if (!gF_.gameplayFeatures_INTERNET[myID] && uiObjects[6].activeSelf)
		{
			uiObjects[6].SetActive(value: false);
		}
		tooltip_.c = gF_.GetTooltip(myID, menu_.g_GameMainGenre, menu_.g_GameSubGenre);
		SetGoodBadIcon();
		myButton.interactable = true;
		SetPlattformLock();
		if (gS_.gameplayFeatures_DevDone[myID])
		{
			myButton.interactable = false;
			if (!uiObjects[5].activeSelf)
			{
				uiObjects[5].SetActive(value: true);
			}
		}
		if (gS_.gameGameplayFeatures[myID])
		{
			myButton.interactable = false;
			if (!uiObjects[5].activeSelf)
			{
				uiObjects[5].SetActive(value: true);
			}
		}
	}

	public void BUTTON_Click()
	{
		FindScripts();
		SetPlattformLock();
		if (myButton.interactable)
		{
			sfx_.PlaySound(3, force: false);
			if (menu_.SetGameplayFeature(myID))
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			else
			{
				GetComponent<Image>().color = Color.white;
			}
		}
	}

	public void SetGoodBadIcon()
	{
		FindScripts();
		if (gF_.GetBonus(myID, menu_.g_GameMainGenre, menu_.g_GameSubGenre) > 1.1f)
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[36];
			goodBad = 2;
		}
		else if (gF_.GetBonus(myID, menu_.g_GameMainGenre, menu_.g_GameSubGenre) < 0.9f)
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
		if (gS_.arcade && gF_.gameplayFeatures_LOCKPLATFORM[myID, 4])
		{
			myButton.interactable = false;
		}
		else if (gS_.handy && gF_.gameplayFeatures_LOCKPLATFORM[myID, 3])
		{
			myButton.interactable = false;
		}
	}
}
