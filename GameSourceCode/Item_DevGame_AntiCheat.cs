using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_AntiCheat : MonoBehaviour
{
	public antiCheatScript acS_;

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
		uiObjects[0].GetComponent<Text>().text = acS_.GetName();
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(acS_.GetDevCosts(), showDollar: true);
		uiObjects[2].GetComponent<Text>().text = mS_.Round(acS_.effekt, 2) + "%";
		uiObjects[3].GetComponent<Image>().fillAmount = acS_.effekt * 0.01f;
		uiObjects[3].GetComponent<Image>().color = GetValColor(acS_.effekt);
		tooltip_.c = acS_.GetTooltip();
		if (guiMain_.uiObjects[365].activeSelf && guiMain_.uiObjects[365].GetComponent<Menu_Dev_ChangeCopyProtect>().gS_.gameAntiCheat == acS_.myID)
		{
			GetComponent<Image>().color = guiMain_.colors[7];
			uiObjects[1].GetComponent<Text>().text = "$0";
		}
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
		sfx_.PlaySound(3, force: true);
		if (guiMain_.uiObjects[56].activeSelf)
		{
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetAntiCheat(acS_.myID);
			guiMain_.uiObjects[236].GetComponent<Menu_DevGame_AntiCheat>().BUTTON_Close();
		}
		if (guiMain_.uiObjects[365].activeSelf)
		{
			guiMain_.uiObjects[365].GetComponent<Menu_Dev_ChangeCopyProtect>().SetAntiCheat(acS_.myID);
			guiMain_.uiObjects[236].GetComponent<Menu_DevGame_AntiCheat>().BUTTON_Close();
		}
	}
}
