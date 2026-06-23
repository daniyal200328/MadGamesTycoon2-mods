using UnityEngine;
using UnityEngine.UI;

public class Item_GamePass_Platform : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public platformScript pS_;

	public games games_;

	public int kompatibleGames;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		pS_.SetPic(uiObjects[1]);
		string dateString = pS_.GetDateString();
		dateString = dateString + "\n" + tS_.GetText(219) + ": <b>" + pS_.GetMarktanteilString() + "</b>";
		uiObjects[2].GetComponent<Text>().text = dateString;
		uiObjects[5].GetComponent<Text>().text = tS_.GetText(2112) + ": " + pS_.minGamePassGames;
		uiObjects[6].GetComponent<Text>().text = pS_.tech.ToString();
		guiMain_.DrawStars(uiObjects[7], pS_.erfahrung);
		tooltip_.c = pS_.GetTooltip();
		uiObjects[10].GetComponent<Image>().sprite = pS_.GetTypSprite();
		uiObjects[10].GetComponent<tooltip>().c = pS_.GetTypString();
		UpdateKompatibleSpiele();
	}

	public void UpdateKompatibleSpiele()
	{
		if (!mS_)
		{
			return;
		}
		int amountGamePassGames = pS_.GetAmountGamePassGames();
		int gamePass_AmountGames = mS_.gpS_.gamePass_AmountGames;
		kompatibleGames = amountGamePassGames;
		string text = tS_.GetText(2109);
		text = text.Replace("<NUM1>", amountGamePassGames.ToString());
		text = text.Replace("<NUM2>", gamePass_AmountGames.ToString());
		uiObjects[11].GetComponent<Text>().text = text;
		float num = gamePass_AmountGames;
		num = 100f / num;
		num *= (float)amountGamePassGames;
		num *= 0.01f;
		uiObjects[12].GetComponent<Image>().fillAmount = num;
		if (pS_.minGamePassGames > amountGamePassGames)
		{
			uiObjects[5].GetComponent<Text>().color = guiMain_.colors[18];
			uiObjects[12].GetComponent<Image>().color = guiMain_.colors[18];
			GetComponent<Button>().interactable = false;
			if (!uiObjects[3].activeSelf)
			{
				uiObjects[3].SetActive(value: true);
			}
		}
		else
		{
			uiObjects[5].GetComponent<Text>().color = guiMain_.colors[28];
			GetComponent<Button>().interactable = true;
			if (uiObjects[3].activeSelf)
			{
				uiObjects[3].SetActive(value: false);
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click(bool allPlatforms)
	{
		if (!mS_)
		{
			return;
		}
		if (!allPlatforms)
		{
			sfx_.PlaySound(3, force: true);
		}
		if ((bool)pS_)
		{
			if (pS_.inGamePass)
			{
				mS_.gpS_.GAMEPASS_RemovePlatform(pS_);
			}
			else
			{
				mS_.gpS_.GAMEPASS_AddPlatform(pS_);
			}
			if (!allPlatforms)
			{
				guiMain_.uiObjects[418].GetComponent<Menu_GamePass_Platforms>().ResetData();
			}
		}
	}
}
