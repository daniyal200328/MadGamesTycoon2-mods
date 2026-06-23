using UnityEngine;
using UnityEngine.UI;

public class Item_Arcadepreis : MonoBehaviour
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
			uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(game_.verkaufspreis[0], showDollar: true);
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.verkaufspreis[0] - game_.arcadeProdCosts, showDollar: true);
			float num = game_.arcadeCase + game_.arcadeJoystick + game_.arcadeMonitor + game_.arcadeSound + 4;
			num /= 4f;
			guiMain_.DrawStars(uiObjects[3], Mathf.RoundToInt(num));
			GetComponent<tooltip>().c = game_.GetTooltip();
			if (mS_.multiplayer && !guiMain_.uiObjects[309].GetComponent<Menu_Arcadepreise>().CheckGameData(game_))
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
		if (game_.arcade)
		{
			guiMain_.uiObjects[307].SetActive(value: true);
			guiMain_.uiObjects[307].GetComponent<Menu_ArcadePreis>().Init(game_, null);
		}
	}
}
