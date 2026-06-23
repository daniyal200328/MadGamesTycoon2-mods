using UnityEngine;
using UnityEngine.UI;

public class Item_DevEngine_Genre : MonoBehaviour
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
		guiMain_.DrawStars(uiObjects[2], genres_.genres_LEVEL[myID]);
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>().SetSpezialgenre(myID);
		guiMain_.uiObjects[38].GetComponent<Menu_Dev_Engine_Genre>().BUTTON_Close();
	}
}
