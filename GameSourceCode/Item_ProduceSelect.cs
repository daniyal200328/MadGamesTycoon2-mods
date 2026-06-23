using UnityEngine;
using UnityEngine.UI;

public class Item_ProduceSelect : MonoBehaviour
{
	public gameScript game_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public roomScript rS_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		DataUpdate();
	}

	private void DataUpdate()
	{
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 1f))
		{
			updateTimer = 0f;
			SetData();
		}
	}

	private void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[1].GetComponent<Image>().sprite = game_.GetScreenshot();
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.GetLagerbestand(), showDollar: false);
			uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(game_.lagerbestand[0], showDollar: false);
			uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(game_.lagerbestand[1], showDollar: false);
			uiObjects[5].GetComponent<Text>().text = mS_.GetMoney(game_.lagerbestand[2], showDollar: false);
			tooltip_.c = game_.GetTooltip();
			if (mS_.multiplayer && !guiMain_.uiObjects[221].GetComponent<Menu_ProductionSelect>().CheckGameData(game_))
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		if (!mS_.multiplayer || guiMain_.uiObjects[221].GetComponent<Menu_ProductionSelect>().CheckGameData(game_))
		{
			guiMain_.uiObjects[222].SetActive(value: true);
			guiMain_.uiObjects[222].GetComponent<Menu_Production>().Init(rS_, game_);
		}
	}
}
