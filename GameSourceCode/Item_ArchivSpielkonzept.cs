using UnityEngine;
using UnityEngine.UI;

public class Item_ArchivSpielkonzept : MonoBehaviour
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
			uiObjects[2].GetComponent<Text>().text = game_.GetReleaseDateString();
			tooltip_.c = game_.GetTooltip();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)game_)
		{
			game_.archiv_spielkonzept = !game_.archiv_spielkonzept;
			Object.Destroy(base.gameObject);
		}
	}
}
