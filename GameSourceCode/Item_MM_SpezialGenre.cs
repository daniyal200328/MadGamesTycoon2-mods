using UnityEngine;
using UnityEngine.UI;

public class Item_MM_SpezialGenre : MonoBehaviour
{
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
		tooltip_.c = genres_.GetTooltip(myID);
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		if (guiMain_.uiObjects[159].activeSelf)
		{
			guiMain_.uiObjects[159].GetComponent<Menu_NewGame>().SetGenre(myID);
		}
		if (guiMain_.uiObjects[425].activeSelf)
		{
			guiMain_.uiObjects[425].GetComponent<Menu_NewGame_Sandbox>().SetGenre(myID);
		}
		guiMain_.uiObjects[298].SetActive(value: false);
	}
}
