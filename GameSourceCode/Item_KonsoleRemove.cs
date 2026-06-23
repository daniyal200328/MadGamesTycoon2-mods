using UnityEngine;
using UnityEngine.UI;

public class Item_KonsoleRemove : MonoBehaviour
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
			uiObjects[3].GetComponent<Image>().sprite = pS_.GetTypSprite();
			uiObjects[4].GetComponent<Text>().text = pS_.tech.ToString();
			if (pS_.proVersion && pS_.GetProName().Length <= 0)
			{
				GetComponent<Image>().color = guiMain_.colors[26];
			}
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
		if (pS_.proVersion && pS_.GetProName().Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(2326), closeMenu: false);
			return;
		}
		guiMain_.uiObjects[332].SetActive(value: true);
		guiMain_.uiObjects[332].GetComponent<Menu_W_KonsoleFromMarket>().Init(pS_);
	}
}
