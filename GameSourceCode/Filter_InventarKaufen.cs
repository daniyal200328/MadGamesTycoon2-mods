using UnityEngine;
using UnityEngine.UI;

public class Filter_InventarKaufen : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int filterArrayID;

	public Color[] colors;

	public mainScript mS_;

	public textScript tS_;

	public mapScript mapS_;

	public GUI_Main guiMain_;

	public sfxScript sfx_;

	public bool show = true;

	private void Start()
	{
	}

	private void Update()
	{
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		show = !show;
		guiMain_.uiObjects[20].GetComponent<Menu_BuyInventar>().filter[filterArrayID] = !show;
		if (show)
		{
			uiObjects[0].GetComponent<Image>().sprite = guiMain_.uiSprites[60];
			GetComponent<Image>().color = colors[0];
		}
		else
		{
			uiObjects[0].GetComponent<Image>().sprite = guiMain_.uiSprites[59];
			GetComponent<Image>().color = colors[1];
		}
		for (int i = base.transform.GetSiblingIndex() + 1; i < base.transform.parent.childCount; i++)
		{
			Transform child = base.transform.parent.GetChild(i);
			if ((bool)child)
			{
				if (!child.GetComponent<Item_InventarKaufen>())
				{
					break;
				}
				child.gameObject.SetActive(!child.gameObject.activeSelf);
			}
		}
	}
}
