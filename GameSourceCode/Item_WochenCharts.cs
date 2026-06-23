using UnityEngine;
using UnityEngine.UI;

public class Item_WochenCharts : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	private void Start()
	{
		SetData();
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			if (game_.IsMyGame())
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			else if (game_.GetPublisherOrDeveloperIsTochterfirma())
			{
				GetComponent<Image>().color = guiMain_.colors[27];
			}
			if (mS_.multiplayer && game_.GameFromMitspieler())
			{
				GetComponent<Image>().color = guiMain_.colors[8];
			}
			uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.sellsPerWeek[0], showDollar: false);
			uiObjects[3].GetComponent<Image>().sprite = game_.GetTypSprite();
			tooltip_.c = game_.GetTooltip();
		}
	}

	private void Update()
	{
		int siblingIndex = base.gameObject.transform.GetSiblingIndex();
		uiObjects[1].GetComponent<Text>().text = (siblingIndex + 1).ToString();
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.sellsPerWeek[0], showDollar: false);
		base.gameObject.name = game_.sellsTotal.ToString();
		if (game_.lastChartPosition < siblingIndex)
		{
			uiObjects[4].GetComponent<Text>().text = "<color=red>▼</color>";
		}
		if (game_.lastChartPosition > siblingIndex)
		{
			uiObjects[4].GetComponent<Text>().text = "<color=green>▲</color>";
		}
		if (game_.lastChartPosition == siblingIndex)
		{
			uiObjects[4].GetComponent<Text>().text = "<color=black>●</color>";
		}
		if (game_.lastChartPosition == -1)
		{
			uiObjects[4].GetComponent<Text>().text = "<color=blue>◆</color>";
		}
		if (mS_.multiplayer && !game_.isOnMarket)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[46].SetActive(value: true);
		guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(game_);
	}
}
