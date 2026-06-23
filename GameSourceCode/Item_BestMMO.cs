using UnityEngine;
using UnityEngine.UI;

public class Item_BestMMO : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	public int sort;

	private void Start()
	{
		SetData();
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			if (game_.ownerID == mS_.myID || game_.publisherID == mS_.myID)
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			if (mS_.multiplayer && game_.GameFromMitspieler())
			{
				GetComponent<Image>().color = guiMain_.colors[8];
			}
			if (sort == 0)
			{
				uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.abonnements, showDollar: false);
			}
			if (sort == 1)
			{
				uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.bestAbonnements, showDollar: false);
			}
			uiObjects[3].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
			tooltip_.c = game_.GetTooltip();
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
		guiMain_.uiObjects[46].SetActive(value: true);
		guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(game_);
	}
}
