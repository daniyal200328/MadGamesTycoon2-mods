using UnityEngine;
using UnityEngine.UI;

public class Item_BudgetSelect : MonoBehaviour
{
	public gameScript game_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public Menu_BudgetSelect menu_;

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
			if (!(updateTimer < 1f))
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
			uiObjects[6].GetComponent<Text>().text = Mathf.RoundToInt(game_.reviewTotal) + "%";
			uiObjects[2].GetComponent<Image>().sprite = game_.GetScreenshot();
			uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
			uiObjects[1].GetComponent<tooltip>().c = genres_.GetName(game_.maingenre);
			float num = game_.hype * 0.5f;
			num -= (float)(mS_.year - game_.date_year);
			if (num < 0f)
			{
				num = 0f;
			}
			uiObjects[7].GetComponent<Text>().text = Mathf.RoundToInt(num).ToString();
			if (game_.freigabeBudget > 0)
			{
				string text = tS_.GetText(1151);
				text = text.Replace("<NUM>", game_.freigabeBudget.ToString());
				uiObjects[5].GetComponent<Text>().text = text;
			}
			else
			{
				uiObjects[5].GetComponent<Text>().text = tS_.GetText(1158);
			}
			tooltip_.c = game_.GetTooltip();
			if (game_.freigabeBudget > 0)
			{
				base.gameObject.GetComponent<Button>().interactable = false;
			}
			else
			{
				base.gameObject.GetComponent<Button>().interactable = true;
			}
			if (mS_.multiplayer && !menu_.CheckGameData(game_))
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
				if ((bool)component && component.isOnMarket && component.typ_goty && component.originalGameID == game_.myID)
				{
					guiMain_.MessageBox(tS_.GetText(1405), closeMenu: false);
					return;
				}
			}
		}
		guiMain_.ActivateMenu(guiMain_.uiObjects[228]);
		guiMain_.uiObjects[228].GetComponent<Menu_BudgetGamename>().Init(game_);
	}
}
