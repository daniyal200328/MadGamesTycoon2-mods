using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_Genre : MonoBehaviour
{
	public int genreArt;

	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public genres genres_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = genres_.GetName(myID);
		uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(myID);
		uiObjects[3].GetComponent<Text>().text = genres_.GetStringBeliebtheit(myID, kurz: true);
		if (mS_.trendGenre == myID)
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[31];
		}
		if (mS_.trendAntiGenre == myID)
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[32];
		}
		guiMain_.DrawStars(uiObjects[2], genres_.genres_LEVEL[myID]);
		uiObjects[5].GetComponent<Image>().sprite = genres_.GetSpriteMarkt(myID);
		tooltip_.c = genres_.GetTooltip(myID) + "\n";
		tooltip tooltip2 = tooltip_;
		tooltip2.c = tooltip2.c + "\n" + tS_.GetText(1380) + ": <color=blue>" + genres_.GetStringBeliebtheit(myID, kurz: false) + "</color>";
		tooltip2 = tooltip_;
		tooltip2.c = tooltip2.c + "\n" + tS_.GetText(1665) + ": <color=blue>" + genres_.GetStringMarktsaettigung(myID) + "</color>";
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		if (genreArt == 0)
		{
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetMainGenre(myID);
		}
		else
		{
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetSubGenre(myID);
		}
		guiMain_.uiObjects[61].GetComponent<Menu_DevGame_Genre>().BUTTON_Close();
	}
}
