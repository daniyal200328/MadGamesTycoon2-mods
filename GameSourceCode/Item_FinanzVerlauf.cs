using UnityEngine;
using UnityEngine.UI;

public class Item_FinanzVerlauf : MonoBehaviour
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
		uiObjects[0].GetComponent<Text>().text = (1976 + index).ToString();
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(mS_.finanzVerlaufEinnahmen[index], showDollar: true);
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(mS_.finanzVerlaufAusgaben[index], showDollar: true);
		long num = mS_.finanzVerlaufEinnahmen[index] - mS_.finanzVerlaufAusgaben[index];
		uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		if (num < 0)
		{
			uiObjects[3].GetComponent<Text>().color = guiMain_.colors[5];
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
