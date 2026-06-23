using UnityEngine;
using UnityEngine.UI;

public class Item_ArchivEngine : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public engineScript eS_;

	private void Start()
	{
		SetData();
	}

	public void SetData()
	{
		if ((bool)eS_)
		{
			uiObjects[0].GetComponent<Text>().text = eS_.GetName();
			uiObjects[2].GetComponent<Text>().text = eS_.GetTechLevel().ToString();
			tooltip_.c = eS_.GetTooltip();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)eS_)
		{
			eS_.archiv_engine = !eS_.archiv_engine;
			Object.Destroy(base.gameObject);
		}
	}
}
