using UnityEngine;
using UnityEngine.UI;

public class Item_GameConcept_GameplayFeature : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public gameplayFeatures gF_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript gS_;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = gF_.GetName(myID);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(gF_.GetDevCosts(myID), showDollar: true);
		uiObjects[2].GetComponent<Image>().sprite = gF_.GetTypSprite(myID);
		guiMain_.DrawStars(uiObjects[3], gF_.gameplayFeatures_LEVEL[myID]);
		tooltip_.c = gF_.GetTooltip(myID, gS_.maingenre, gS_.subgenre);
		SetGoodBadIcon();
	}

	private void SetGoodBadIcon()
	{
		if (gF_.GetBonus(myID, gS_.maingenre, gS_.subgenre) > 1.1f)
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[36];
		}
		else if (gF_.GetBonus(myID, gS_.maingenre, gS_.subgenre) < 0.9f)
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[38];
		}
		else
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[37];
		}
	}
}
