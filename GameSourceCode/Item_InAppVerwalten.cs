using UnityEngine;
using UnityEngine.UI;

public class Item_InAppVerwalten : MonoBehaviour
{
	public gameScript game_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public Menu_InAppVerwalten menu_;

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
		if (!game_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
		uiObjects[3].GetComponent<Text>().text = game_.GetReleaseDateString();
		uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
		uiObjects[6].GetComponent<Image>().sprite = game_.GetTypSprite();
		for (int i = 0; i < game_.inAppPurchase.Length; i++)
		{
			if (game_.inAppPurchase[i])
			{
				uiObjects[7 + i].GetComponent<Image>().color = Color.white;
			}
			else
			{
				uiObjects[7 + i].GetComponent<Image>().color = guiMain_.colors[6];
			}
		}
		tooltip_.c = game_.GetTooltip();
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
		guiMain_.ActivateMenu(guiMain_.uiObjects[278]);
		guiMain_.uiObjects[278].GetComponent<Menu_InAppPurchases>().Init(game_, closeMenu_: false);
	}
}
