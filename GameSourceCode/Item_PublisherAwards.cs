using UnityEngine;
using UnityEngine.UI;

public class Item_PublisherAwards : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public publisherScript pub_;

	public genres genres_;

	public int sort;

	private void Start()
	{
		SetData();
	}

	public void SetData()
	{
		if ((bool)pub_)
		{
			uiObjects[0].GetComponent<Text>().text = pub_.GetName();
			if (mS_.myID == pub_.myID)
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			else if (pub_.IsMyTochterfirma())
			{
				GetComponent<Image>().color = guiMain_.colors[27];
			}
			if (mS_.multiplayer && (pub_.IsTochterfirmaVonMitspieler() || (pub_.myID != mS_.myID && pub_.myID >= 100000)))
			{
				GetComponent<Image>().color = guiMain_.colors[8];
			}
			switch (sort)
			{
			case 0:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(770) + ": <b>" + pub_.awards[4] + "</b>";
				break;
			case 1:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(768) + ": <b>" + pub_.awards[2] + "</b>";
				break;
			case 2:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(769) + ": <b>" + pub_.awards[3] + "</b>";
				break;
			case 3:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(767) + ": <b>" + pub_.awards[0] + "</b>";
				break;
			case 4:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(766) + ": <b>" + pub_.awards[1] + "</b>";
				break;
			case 5:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(279) + ": <b>" + pub_.awards[8] + "</b>";
				break;
			case 6:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(772) + ": <b>" + pub_.awards[7] + "</b>";
				break;
			case 7:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(1309) + ": <b>" + pub_.awards[10] + "</b>";
				break;
			case 8:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(1310) + ": <b>" + pub_.awards[11] + "</b>";
				break;
			case 9:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(771) + ": <b>" + pub_.awards[5] + "</b>";
				break;
			case 10:
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(280) + ": <b>" + pub_.awards[9] + "</b>";
				break;
			}
			uiObjects[3].GetComponent<Image>().sprite = pub_.GetLogo();
			tooltip_.c = pub_.GetTooltip();
		}
	}

	private void Update()
	{
		uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
	}
}
