using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Item_AllTimeCharts : MonoBehaviour
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
			if (guiMain_.uiObjects[375].activeSelf)
			{
				uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.umsatzTotal, showDollar: true);
			}
			else
			{
				uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.sellsTotal, showDollar: false);
			}
			uiObjects[3].GetComponent<Image>().sprite = game_.GetTypSprite();
			StartCoroutine(iSetTooltip());
		}
	}

	private void Update()
	{
		uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
		if (mS_.multiplayer)
		{
			if (guiMain_.uiObjects[375].activeSelf)
			{
				uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.umsatzTotal, showDollar: true);
				base.gameObject.name = game_.umsatzTotal.ToString();
			}
			else
			{
				uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.sellsTotal, showDollar: false);
				base.gameObject.name = game_.sellsTotal.ToString();
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
		guiMain_.uiObjects[46].SetActive(value: true);
		guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(game_);
	}

	private IEnumerator iSetTooltip()
	{
		yield return new WaitForSeconds(Random.Range(0.1f, 1f));
		if ((bool)game_)
		{
			tooltip_.c = game_.GetTooltip();
		}
	}
}
