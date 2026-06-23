using UnityEngine;
using UnityEngine.UI;

public class Item_History : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public int index;

	private void Start()
	{
		SetData();
	}

	public void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = mS_.history[index];
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
