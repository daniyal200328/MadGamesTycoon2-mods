using UnityEngine;
using UnityEngine.UI;

public class Item_GotySelect : MonoBehaviour
{
	public gameScript game_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public Menu_GOTYSelect menu_;

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
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(275) + ": " + mS_.GetMoney(game_.sellsTotal, showDollar: false);
			uiObjects[5].GetComponent<Text>().text = tS_.GetText(277) + ": " + Mathf.RoundToInt(game_.reviewTotal) + "%";
			uiObjects[2].GetComponent<Image>().sprite = game_.GetScreenshot();
			uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
			uiObjects[6].GetComponent<Image>().sprite = game_.GetTypSprite();
			float num = game_.hype * 0.5f;
			num -= (float)(mS_.year - game_.date_year);
			if (num < 0f)
			{
				num = 0f;
			}
			uiObjects[7].GetComponent<Text>().text = Mathf.RoundToInt(num).ToString();
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
		if (!menu_.CheckGameData(game_))
		{
			Object.Destroy(base.gameObject);
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component && component.isOnMarket && component.typ_budget && component.originalGameID == game_.myID)
				{
					guiMain_.MessageBox(tS_.GetText(1404), closeMenu: false);
					return;
				}
			}
		}
		guiMain_.ActivateMenu(guiMain_.uiObjects[275]);
		guiMain_.uiObjects[275].GetComponent<Menu_GOTYGamename>().Init(game_);
	}
}
