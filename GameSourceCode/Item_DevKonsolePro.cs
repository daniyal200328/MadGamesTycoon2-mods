using UnityEngine;
using UnityEngine.UI;

public class Item_DevKonsolePro : MonoBehaviour
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
			if (!pS_.vomMarktGenommen)
			{
				uiObjects[1].GetComponent<Text>().text = pS_.GetDateString();
			}
			else
			{
				string text = tS_.GetText(1673);
				text = text.Replace("<DATE1>", pS_.GetDateString());
				text = text.Replace("<DATE2>", pS_.GetDateStringEnd());
				uiObjects[1].GetComponent<Text>().color = guiMain_.colors[5];
				uiObjects[1].GetComponent<Text>().text = text;
			}
			pS_.SetPic(uiObjects[2]);
			uiObjects[3].GetComponent<Image>().sprite = pS_.GetTypSprite();
			uiObjects[4].GetComponent<Text>().text = pS_.tech.ToString();
			uiObjects[5].GetComponent<Text>().text = mS_.GetMoney(pS_.performancePoints, showDollar: false);
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
		roomScript rS_ = guiMain_.uiObjects[449].GetComponent<Menu_Dev_KonsoleProSelect>().rS_;
		guiMain_.uiObjects[318].SetActive(value: true);
		guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>().InitPro(rS_, pS_);
		guiMain_.uiObjects[317].SetActive(value: false);
		guiMain_.uiObjects[449].SetActive(value: false);
	}
}
