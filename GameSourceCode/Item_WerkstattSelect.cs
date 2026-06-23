using UnityEngine;
using UnityEngine.UI;

public class Item_WerkstattSelect : MonoBehaviour
{
	public gameScript game_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public Menu_ProductionArcadeSelect menu_;

	public roomScript rS_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		MultiplayerUpdate();
	}

	private void MultiplayerUpdate()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 5f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	private void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[3].GetComponent<Text>().text = game_.GetReleaseDateString();
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(1511) + ": " + mS_.GetMoney(game_.sellsTotal, showDollar: false);
			uiObjects[7].GetComponent<Text>().text = tS_.GetText(1125) + ": " + mS_.GetMoney(game_.vorbestellungen, showDollar: false);
			uiObjects[5].GetComponent<Text>().text = Mathf.RoundToInt(game_.reviewTotal) + "%";
			uiObjects[2].GetComponent<Image>().sprite = game_.GetScreenshot();
			uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
			uiObjects[6].GetComponent<Image>().sprite = game_.GetTypSprite();
			tooltip_.c = game_.GetTooltip();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		if (!mS_.multiplayer || menu_.CheckGameData(game_))
		{
			taskArcadeProduction taskArcadeProduction2 = guiMain_.AddTask_ArcadeProduction();
			taskArcadeProduction2.Init(fromSavegame: false);
			taskArcadeProduction2.targetID = game_.myID;
			taskArcadeProduction2.points = 25f;
			taskArcadeProduction2.pointsLeft = 25f;
			GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
			if ((bool)gameObject)
			{
				gameObject.GetComponent<roomScript>().taskID = taskArcadeProduction2.myID;
			}
			guiMain_.CloseMenu();
			guiMain_.uiObjects[304].SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
	}
}
