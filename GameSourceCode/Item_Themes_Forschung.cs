using UnityEngine;
using UnityEngine.UI;

public class Item_Themes_Forschung : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public Color[] colors;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public themes themes_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public roomScript rS_;

	private float updateTimer;

	private RectTransform myRect_;

	private int frames;

	private bool hasEnabled;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = tS_.GetThemes(myID);
		uiObjects[1].GetComponent<Image>().sprite = themes_.icon;
		if ((float)themes_.RES_POINTS == themes_.themes_RES_POINTS_LEFT[myID])
		{
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(themes_.PRICE, showDollar: true);
		}
		else
		{
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(157);
		}
		string text = tS_.GetText(156);
		text = text.Replace("<NUM>", mS_.Round(themes_.themes_RES_POINTS_LEFT[myID], 2).ToString());
		uiObjects[3].GetComponent<Text>().text = text;
		float prozent = themes_.GetProzent(myID);
		uiObjects[4].GetComponent<Image>().fillAmount = prozent * 0.01f;
		uiObjects[5].GetComponent<Text>().text = mS_.Round(prozent, 1) + "%";
		tooltip_.c = themes_.GetTooltip(myID);
	}

	private void Update()
	{
		if (!hasEnabled)
		{
			frames++;
			if (frames >= 3)
			{
				if (!myRect_)
				{
					myRect_ = GetComponent<RectTransform>();
				}
				if (myRect_.position.y >= 0f && myRect_.position.y <= (float)Screen.height)
				{
					EnableObjects();
				}
			}
		}
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 1f))
		{
			updateTimer = 0f;
			SetData();
		}
	}

	public void EnableObjects()
	{
		if (hasEnabled)
		{
			return;
		}
		hasEnabled = true;
		for (int i = 0; i < uiObjects.Length; i++)
		{
			if ((bool)uiObjects[i] && !uiObjects[i].activeSelf)
			{
				uiObjects[i].SetActive(value: true);
				SetData();
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		Menu_Forschung component = guiMain_.uiObjects[21].GetComponent<Menu_Forschung>();
		if (!themes_.Pay(myID))
		{
			guiMain_.ShowNoMoney();
			return;
		}
		taskForschung taskForschung2 = guiMain_.AddTask_Forschung();
		taskForschung2.Init(fromSavegame: false);
		taskForschung2.typ = 1;
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
