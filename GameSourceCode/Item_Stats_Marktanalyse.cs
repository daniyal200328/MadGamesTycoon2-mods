using UnityEngine;
using UnityEngine.UI;

public class Item_Stats_Marktanalyse : MonoBehaviour
{
	public GameObject[] uiObjects;

	public string myName;

	public int anzGames;

	public int typ;

	public void Init(string text_, string amountGames, Sprite pic, Sprite marktanalyse, int anzGames_, int typ_)
	{
		myName = text_;
		anzGames = anzGames_;
		typ = typ_;
		uiObjects[0].GetComponent<Text>().text = text_;
		uiObjects[1].GetComponent<Text>().text = amountGames;
		uiObjects[2].GetComponent<Image>().sprite = pic;
		uiObjects[3].GetComponent<Image>().sprite = marktanalyse;
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
