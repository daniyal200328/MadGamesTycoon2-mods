using UnityEngine;
using UnityEngine.UI;

public class Item_Dev_CopyProtectAddon : MonoBehaviour
{
	public copyProtectScript cpS_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

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
		uiObjects[0].GetComponent<Text>().text = cpS_.GetName();
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(cpS_.GetDevCosts(), showDollar: true);
		uiObjects[2].GetComponent<Text>().text = mS_.Round(cpS_.effekt, 2) + "%";
		uiObjects[3].GetComponent<Image>().fillAmount = cpS_.effekt * 0.01f;
		uiObjects[3].GetComponent<Image>().color = GetValColor(cpS_.effekt);
		tooltip_.c = cpS_.GetTooltip();
	}

	private Color GetValColor(float val)
	{
		if (val < 30f)
		{
			return guiMain_.colorsBalken[0];
		}
		if (val >= 30f && val < 70f)
		{
			return guiMain_.colorsBalken[1];
		}
		if (val >= 70f)
		{
			return guiMain_.colorsBalken[2];
		}
		return guiMain_.colorsBalken[0];
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		if (guiMain_.uiObjects[193].activeSelf)
		{
			guiMain_.uiObjects[193].GetComponent<Menu_Dev_AddonDo>().SetCopyProtect(cpS_.myID);
		}
		if (guiMain_.uiObjects[247].activeSelf)
		{
			guiMain_.uiObjects[247].GetComponent<Menu_Dev_MMOAddon>().SetCopyProtect(cpS_.myID);
		}
		guiMain_.uiObjects[194].GetComponent<Menu_Dev_CopyProtectAddon>().BUTTON_Close();
	}
}
