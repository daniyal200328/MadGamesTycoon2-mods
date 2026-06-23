using UnityEngine;
using UnityEngine.UI;

public class Item_AboPreis : MonoBehaviour
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
		if (!(updateTimer < 0.5f))
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
			uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(game_.abonnements, showDollar: false);
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.aboPreis, showDollar: true);
			tooltip_.c = game_.GetTooltip();
			if (mS_.multiplayer && !guiMain_.uiObjects[245].GetComponent<Menu_AboPreis_Select>().CheckGameData(game_))
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
		guiMain_.uiObjects[242].SetActive(value: true);
		guiMain_.uiObjects[242].GetComponent<Menu_Abo_Preis>().Init(game_);
	}
}
