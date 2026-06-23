using UnityEngine;
using UnityEngine.UI;

public class Item_LizenzVerschenken : MonoBehaviour
{
	public int myID = -1;

	public licences licences_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public Menu_MP_LizenzSchenken menu_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = licences_.GetName(myID);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(licences_.GetSellPrice(myID), showDollar: true);
		guiMain_.DrawStars(uiObjects[2], Mathf.RoundToInt(licences_.licence_QUALITY[myID] / 20f));
		string text = tS_.GetText(297);
		text = text.Replace("<NUM>", licences_.licence_GEKAUFT[myID].ToString());
		uiObjects[3].GetComponent<Text>().text = text;
		uiObjects[4].GetComponent<Text>().text = licences_.GetTypString(myID);
		tooltip_.c = licences_.GetTooltip(myID);
		if (menu_.selectedLizenz == myID)
		{
			GetComponent<Image>().color = guiMain_.colors[4];
		}
		else
		{
			GetComponent<Image>().color = Color.white;
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 0.1f))
		{
			updateTimer = 0f;
			SetData();
		}
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		menu_.selectedLizenz = myID;
		SetData();
	}
}
