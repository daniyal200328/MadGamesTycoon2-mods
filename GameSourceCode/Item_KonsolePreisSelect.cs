using UnityEngine;
using UnityEngine.UI;

public class Item_KonsolePreisSelect : MonoBehaviour
{
	public platformScript pS_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public Menu_Bundle menu_;

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
		if ((bool)pS_)
		{
			uiObjects[0].GetComponent<Text>().text = pS_.GetName();
			uiObjects[1].GetComponent<Text>().text = pS_.GetDateString();
			pS_.SetPic(uiObjects[2]);
			uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(pS_.verkaufspreis, showDollar: true);
			uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(pS_.price, showDollar: true);
			tooltip_.c = pS_.GetTooltip();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[328].SetActive(value: true);
		guiMain_.uiObjects[328].GetComponent<Menu_Konsolenpreis>().Init(pS_, null);
	}
}
