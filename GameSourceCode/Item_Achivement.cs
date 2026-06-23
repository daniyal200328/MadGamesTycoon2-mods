using UnityEngine;
using UnityEngine.UI;

public class Item_Achivement : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public GUI_Main guiMain_;

	private int myID;

	public void SetData(int i)
	{
		myID = i;
		if ((bool)guiMain_)
		{
			uiObjects[0].GetComponent<Image>().sprite = guiMain_.iconAchivements[i];
			if (mS_.achivements[i])
			{
				uiObjects[0].GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}
			uiObjects[1].GetComponent<Text>().text = tS_.GetAchivementName(i);
			uiObjects[2].GetComponent<Text>().text = tS_.GetAchivementDesc(i);
			uiObjects[3].GetComponent<Text>().text = GetBonusText(mS_.achivementsBonus[i]);
		}
	}

	public string GetBonusText(int i)
	{
		int num = 1;
		string text = "";
		switch (i)
		{
		case 0:
			text = tS_.GetText(1801);
			break;
		case 1:
			text = tS_.GetText(1802);
			break;
		case 2:
			text = tS_.GetText(1803);
			break;
		case 3:
			text = tS_.GetText(1804);
			break;
		case 4:
			text = tS_.GetText(1805);
			break;
		case 5:
			text = tS_.GetText(1806);
			break;
		case 6:
			text = tS_.GetText(1807);
			break;
		case 7:
			text = tS_.GetText(1808);
			break;
		case 8:
			text = tS_.GetText(1809);
			break;
		case 9:
			text = tS_.GetText(1810);
			break;
		}
		return text.Replace("<NUM>", num.ToString());
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
