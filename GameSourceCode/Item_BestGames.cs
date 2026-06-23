using UnityEngine;
using UnityEngine.UI;

public class Item_BestGames : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	private RectTransform myRect_;

	private int frames;

	private bool hasEnabled;

	private void Start()
	{
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
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(1291) + ": " + game_.GetUserReviewPercent() + "%  " + tS_.GetText(1292) + ": " + game_.reviewTotal + "%";
			uiObjects[3].GetComponent<Image>().sprite = game_.GetTypSprite();
			tooltip_.c = game_.GetTooltip();
		}
	}

	private void Update()
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
		uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
		if (mS_.multiplayer)
		{
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(1291) + ": " + game_.GetUserReviewPercent() + "%  " + tS_.GetText(1292) + ": " + game_.reviewTotal + "%";
			base.gameObject.name = game_.reviewTotal.ToString();
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
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[46].SetActive(value: true);
		guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(game_);
	}
}
