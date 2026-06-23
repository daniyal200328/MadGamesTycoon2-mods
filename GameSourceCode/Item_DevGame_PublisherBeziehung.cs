using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_PublisherBeziehung : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GUI_Main guiMain_;

	public void SetData(string text_, Sprite sprite_, int stars_)
	{
		uiObjects[0].GetComponent<Text>().text = text_;
		uiObjects[1].GetComponent<Image>().sprite = sprite_;
		guiMain_.DrawStarsColor(uiObjects[2], stars_, Color.red);
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
