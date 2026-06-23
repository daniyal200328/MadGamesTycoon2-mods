using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_Theme : MonoBehaviour
{
	public int themeArt;

	public int myID;

	public int fitGenre;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public themes themes_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public bool debug;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = tS_.GetThemes(myID);
		if (themes_.themes_MGSR[myID] != 0)
		{
			uiObjects[1].GetComponent<Image>().sprite = mS_.games_.gamePEGI[themes_.themes_MGSR[myID]];
		}
		uiObjects[3].GetComponent<Image>().sprite = themes_.GetSpriteMarkt(myID);
		switch (fitGenre)
		{
		case 1:
			GetComponent<Image>().color = guiMain_.colors[7];
			break;
		case -1:
			GetComponent<Image>().color = guiMain_.colors[8];
			break;
		}
		guiMain_.DrawStars(uiObjects[2], themes_.themes_LEVEL[myID]);
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		if (themeArt == 0)
		{
			if (guiMain_.uiObjects[56].activeSelf)
			{
				guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetMainTheme(myID);
			}
		}
		else
		{
			if (guiMain_.uiObjects[56].activeSelf)
			{
				guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetSubTheme(myID);
			}
			if (guiMain_.uiObjects[193].activeSelf)
			{
				guiMain_.uiObjects[193].GetComponent<Menu_Dev_AddonDo>().SetSubTheme(myID);
			}
			if (guiMain_.uiObjects[247].activeSelf)
			{
				guiMain_.uiObjects[247].GetComponent<Menu_Dev_MMOAddon>().SetSubTheme(myID);
			}
		}
		guiMain_.uiObjects[62].GetComponent<Menu_DevGame_Theme>().BUTTON_Close();
	}
}
