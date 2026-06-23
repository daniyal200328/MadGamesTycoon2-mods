using UnityEngine;
using UnityEngine.UI;

public class Item_AngekuendigteSpiele : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	private void Start()
	{
		SetData();
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[3].GetComponent<Image>().sprite = game_.GetDeveloperLogo();
			uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
