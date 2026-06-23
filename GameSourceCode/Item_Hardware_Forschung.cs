using UnityEngine;
using UnityEngine.UI;

public class Item_Hardware_Forschung : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public Color[] colors;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public hardware hardware_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public roomScript rS_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = hardware_.GetName(myID);
		uiObjects[1].GetComponent<Image>().sprite = hardware_.GetTypPic(myID);
		uiObjects[7].GetComponent<Text>().text = hardware_.hardware_TECH[myID].ToString();
		if ((float)hardware_.hardware_RES_POINTS[myID] == hardware_.hardware_RES_POINTS_LEFT[myID])
		{
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(hardware_.GetPrice(myID), showDollar: true);
		}
		else
		{
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(157);
		}
		if (!hardware_.IsTechComponent(myID))
		{
			uiObjects[6].SetActive(value: false);
			uiObjects[12].SetActive(value: true);
		}
		if (hardware_.hardware_ONLYSTATIONARY[myID])
		{
			uiObjects[11].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
		}
		if (hardware_.hardware_ONLYHANDHELD[myID])
		{
			uiObjects[10].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
		}
		string text = tS_.GetText(156);
		text = text.Replace("<NUM>", mS_.Round(hardware_.hardware_RES_POINTS_LEFT[myID], 2).ToString());
		uiObjects[3].GetComponent<Text>().text = text;
		float prozent = hardware_.GetProzent(myID);
		uiObjects[4].GetComponent<Image>().fillAmount = prozent * 0.01f;
		uiObjects[5].GetComponent<Text>().text = mS_.Round(prozent, 1) + "%";
		tooltip_.c = hardware_.GetTooltip(myID);
	}

	private void Update()
	{
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 1f))
		{
			updateTimer = 0f;
			SetData();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		Menu_Forschung component = guiMain_.uiObjects[21].GetComponent<Menu_Forschung>();
		if (!hardware_.Pay(myID))
		{
			guiMain_.ShowNoMoney();
			return;
		}
		taskForschung taskForschung2 = guiMain_.AddTask_Forschung();
		taskForschung2.Init(fromSavegame: false);
		taskForschung2.typ = 4;
		taskForschung2.slot = myID;
		taskForschung2.automatic = component.uiObjects[4].GetComponent<Toggle>().isOn;
		if (taskForschung2.automatic)
		{
			taskForschung2.automaticWait = component.uiObjects[8].GetComponent<Toggle>().isOn;
		}
		GameObject gameObject = GameObject.Find("Room_" + component.roomID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskForschung2.myID;
		}
		sfx_.PlaySound(3, force: true);
		component.BUTTON_Close();
	}
}
