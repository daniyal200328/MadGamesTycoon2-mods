using UnityEngine;
using UnityEngine.UI;

public class Item_DevAddon_GameplayFeature : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public gameplayFeatures gF_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public int goodBad = 1;

	private Menu_Dev_AddonDo menuDevAddon_;

	private Menu_Dev_MMOAddon menuDevMMOAddon_;

	private void Start()
	{
		FindScripts();
		SetData();
	}

	private void FindScripts()
	{
		if (!menuDevAddon_)
		{
			menuDevAddon_ = guiMain_.uiObjects[193].GetComponent<Menu_Dev_AddonDo>();
		}
		if (!menuDevMMOAddon_)
		{
			menuDevMMOAddon_ = guiMain_.uiObjects[247].GetComponent<Menu_Dev_MMOAddon>();
		}
	}

	private void Update()
	{
		if (guiMain_.uiObjects[193].activeSelf)
		{
			if (!menuDevAddon_)
			{
				menuDevAddon_ = guiMain_.uiObjects[193].GetComponent<Menu_Dev_AddonDo>();
			}
			if (menuDevAddon_.g_GameGameplayFeatures[myID])
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			else
			{
				GetComponent<Image>().color = Color.white;
			}
		}
		if (guiMain_.uiObjects[247].activeSelf)
		{
			if (!menuDevMMOAddon_)
			{
				menuDevMMOAddon_ = guiMain_.uiObjects[247].GetComponent<Menu_Dev_MMOAddon>();
			}
			if (menuDevMMOAddon_.g_GameGameplayFeatures[myID])
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			else
			{
				GetComponent<Image>().color = Color.white;
			}
		}
		if (guiMain_.uiObjects[193].activeSelf)
		{
			tooltip_.c = gF_.GetTooltip(myID, menuDevAddon_.gS_.maingenre, menuDevAddon_.gS_.subgenre);
		}
		if (guiMain_.uiObjects[247].activeSelf)
		{
			tooltip_.c = gF_.GetTooltip(myID, menuDevMMOAddon_.gS_.maingenre, menuDevMMOAddon_.gS_.subgenre);
		}
		SetGoodBadIcon();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = gF_.GetName(myID);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(gF_.GetDevCosts(myID), showDollar: true);
		uiObjects[2].GetComponent<Image>().sprite = gF_.GetTypSprite(myID);
		guiMain_.DrawStars(uiObjects[3], gF_.gameplayFeatures_LEVEL[myID]);
		if (guiMain_.uiObjects[193].activeSelf)
		{
			tooltip_.c = gF_.GetTooltip(myID, menuDevAddon_.gS_.maingenre, menuDevAddon_.gS_.subgenre);
		}
		if (guiMain_.uiObjects[247].activeSelf)
		{
			tooltip_.c = gF_.GetTooltip(myID, menuDevMMOAddon_.gS_.maingenre, menuDevMMOAddon_.gS_.subgenre);
		}
		SetGoodBadIcon();
	}

	public void BUTTON_Click()
	{
		if (GetComponent<Button>().interactable)
		{
			sfx_.PlaySound(3, force: false);
			bool flag = false;
			if (guiMain_.uiObjects[193].activeSelf)
			{
				flag = guiMain_.uiObjects[193].GetComponent<Menu_Dev_AddonDo>().SetGameplayFeature(myID);
			}
			if (guiMain_.uiObjects[247].activeSelf)
			{
				flag = guiMain_.uiObjects[247].GetComponent<Menu_Dev_MMOAddon>().SetGameplayFeature(myID);
			}
			if (flag)
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			else
			{
				GetComponent<Image>().color = Color.white;
			}
		}
	}

	private void SetGoodBadIcon()
	{
		FindScripts();
		if (guiMain_.uiObjects[193].activeSelf)
		{
			if (gF_.GetBonus(myID, menuDevAddon_.gS_.maingenre, menuDevAddon_.gS_.subgenre) > 1.1f)
			{
				uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[36];
				goodBad = 2;
			}
			else if (gF_.GetBonus(myID, menuDevAddon_.gS_.maingenre, menuDevAddon_.gS_.subgenre) < 0.9f)
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
		else if (guiMain_.uiObjects[247].activeSelf)
		{
			if (gF_.GetBonus(myID, menuDevMMOAddon_.gS_.maingenre, menuDevMMOAddon_.gS_.subgenre) > 1.1f)
			{
				uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[36];
				goodBad = 2;
			}
			else if (gF_.GetBonus(myID, menuDevMMOAddon_.gS_.maingenre, menuDevMMOAddon_.gS_.subgenre) < 0.9f)
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
	}
}
