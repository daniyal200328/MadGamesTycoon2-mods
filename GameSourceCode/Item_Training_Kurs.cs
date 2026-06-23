using UnityEngine;
using UnityEngine.UI;

public class Item_Training_Kurs : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private sfxScript sfx_;

	private GUI_Main guiMain_;

	private Menu_Training_Select menuTraining_;

	private void Start()
	{
		FindScripts();
		SetData();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!menuTraining_)
		{
			menuTraining_ = guiMain_.uiObjects[92].GetComponent<Menu_Training_Select>();
		}
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = tS_.GetText(538 + myID);
		uiObjects[1].GetComponent<Image>().sprite = menuTraining_.trainingSprites[myID];
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(menuTraining_.trainingCosts[myID], showDollar: true);
		string text = tS_.GetText(562);
		text = text.Replace("<NUM>", mS_.Round(menuTraining_.trainingMaxLearn[myID], 2).ToString());
		uiObjects[3].GetComponent<Text>().text = text;
		uiObjects[4].GetComponent<Text>().text = tS_.GetText(563 + menuTraining_.trainingEffekt[myID]);
	}

	public void BUTTON_Click()
	{
		if ((bool)menuTraining_.rS_)
		{
			mS_.Pay(menuTraining_.trainingCosts[myID], 13);
			taskTraining taskTraining2 = guiMain_.AddTask_Training();
			taskTraining2.Init(fromSavegame: false);
			taskTraining2.slot = myID;
			taskTraining2.points = menuTraining_.workPoints[myID];
			taskTraining2.pointsLeft = menuTraining_.workPoints[myID];
			taskTraining2.automatic = menuTraining_.uiObjects[5].GetComponent<Toggle>().isOn;
			GameObject gameObject = GameObject.Find("Room_" + menuTraining_.rS_.myID);
			if ((bool)gameObject)
			{
				gameObject.GetComponent<roomScript>().taskID = taskTraining2.myID;
			}
			sfx_.PlaySound(3, force: true);
			menuTraining_.BUTTON_Close();
		}
	}
}
