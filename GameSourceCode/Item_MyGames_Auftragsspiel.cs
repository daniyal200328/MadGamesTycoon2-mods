using UnityEngine;
using UnityEngine.UI;

public class Item_MyGames_Auftragsspiel : MonoBehaviour
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

	private void Update()
	{
		uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
		if (mS_.multiplayer)
		{
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(1291) + ": " + game_.GetUserReviewPercent() + "%  " + tS_.GetText(1292) + ": " + game_.reviewTotal + "%";
			base.gameObject.name = game_.reviewTotal.ToString();
		}
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(1291) + ": " + game_.GetUserReviewPercent() + "%  " + tS_.GetText(1292) + ": " + game_.reviewTotal + "%";
			if (!game_.pS_)
			{
				game_.FindMyPublisher();
			}
			if ((bool)game_.pS_)
			{
				uiObjects[3].GetComponent<Image>().sprite = game_.pS_.GetLogo();
			}
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
		guiMain_.uiObjects[46].SetActive(value: true);
		guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(game_);
	}
}
