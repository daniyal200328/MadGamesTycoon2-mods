using UnityEngine;
using UnityEngine.UI;

public class Item_InventarKaufen : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int typ;

	public Color[] colors;

	public mainScript mS_;

	public textScript tS_;

	public mapScript mapS_;

	public GUI_Main guiMain_;

	public sfxScript sfx_;

	public autoInventarScript autoInventar_;

	private void Start()
	{
		uiObjects[0].GetComponent<Text>().text = tS_.GetObjects(typ);
		CheckUnlock();
		uiObjects[2].GetComponent<Image>().sprite = guiMain_.inventarSprites[typ];
		if (typ != 17 && typ != 129 && typ != 130 && typ != 132 && typ != 133 && typ != 134 && typ != 135 && typ != 142 && typ != 143 && (bool)mapS_.prefabsInventar[typ])
		{
			uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(mapS_.prefabsInventar[typ].GetComponent<objectScript>().preis, showDollar: true);
		}
		SetTooltip();
	}

	private void Update()
	{
		if (autoInventar_.autoBuildEnabled)
		{
			GetComponent<Button>().interactable = false;
		}
		else
		{
			GetComponent<Button>().interactable = true;
		}
		Highlight();
		switch (typ)
		{
		case 17:
			uiObjects[3].GetComponent<Text>().text = mS_.goldeneSchallplatten.ToString();
			if (mS_.goldeneSchallplatten <= 0)
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[8];
			}
			else
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[4];
			}
			break;
		case 129:
			uiObjects[3].GetComponent<Text>().text = mS_.platinSchallplatten.ToString();
			if (mS_.platinSchallplatten <= 0)
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[8];
			}
			else
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[4];
			}
			break;
		case 130:
			uiObjects[3].GetComponent<Text>().text = mS_.diamantSchallplatten.ToString();
			if (mS_.diamantSchallplatten <= 0)
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[8];
			}
			else
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[4];
			}
			break;
		case 132:
			uiObjects[3].GetComponent<Text>().text = mS_.award_GOTY.ToString();
			if (mS_.award_GOTY <= 0)
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[8];
			}
			else
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[4];
			}
			break;
		case 133:
			uiObjects[3].GetComponent<Text>().text = mS_.award_Studio.ToString();
			if (mS_.award_Studio <= 0)
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[8];
			}
			else
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[4];
			}
			break;
		case 134:
			uiObjects[3].GetComponent<Text>().text = mS_.award_Sound.ToString();
			if (mS_.award_Sound <= 0)
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[8];
			}
			else
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[4];
			}
			break;
		case 135:
			uiObjects[3].GetComponent<Text>().text = mS_.award_Grafik.ToString();
			if (mS_.award_Grafik <= 0)
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[8];
			}
			else
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[4];
			}
			break;
		case 142:
			uiObjects[3].GetComponent<Text>().text = mS_.award_Trendsetter.ToString();
			if (mS_.award_Trendsetter <= 0)
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[8];
			}
			else
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[4];
			}
			break;
		case 143:
			uiObjects[3].GetComponent<Text>().text = mS_.award_Publisher.ToString();
			if (mS_.award_Publisher <= 0)
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[8];
			}
			else
			{
				uiObjects[3].GetComponent<Text>().color = guiMain_.colors[4];
			}
			break;
		}
	}

	private void OnDisable()
	{
	}

	private void CheckUnlock()
	{
		objectScript component = mapS_.prefabsInventar[typ].GetComponent<objectScript>();
		if (component.unlockYear != -1 && (!mS_.settings_sandbox || !mS_.sandbox_allItems) && mS_.year < component.unlockYear)
		{
			uiObjects[1].SetActive(value: true);
		}
	}

	public void BUTTON_Click()
	{
		if (!autoInventar_.autoBuildEnabled && !uiObjects[1].activeSelf)
		{
			sfx_.PlaySound(3, force: true);
			if ((bool)mS_.pickedObject)
			{
				Object.Destroy(mS_.pickedObject);
			}
			mapS_.CreateObject(typ, createdWithAutoInventar: false);
			uiObjects[2].GetComponent<Animation>().Play();
		}
	}

	private void Highlight()
	{
		if ((bool)mS_.pickedObject && mS_.pickedObject.GetComponent<objectScript>().typ == typ)
		{
			uiObjects[4].GetComponent<Image>().color = colors[1];
		}
		else
		{
			uiObjects[4].GetComponent<Image>().color = colors[0];
		}
	}

	private void SetTooltip()
	{
		string text = "";
		objectScript component = mapS_.prefabsInventar[typ].GetComponent<objectScript>();
		text = "<b>" + tS_.GetObjects(typ) + "</b>";
		text = text + "<br>" + tS_.GetObjectsTooltip(typ) + "<br>";
		if (component.isServer)
		{
			text = (mS_.settings_sandbox ? text.Replace("<NUM>", mS_.GetMoney(component.serverplatz * mS_.sandbox_server, showDollar: false)) : text.Replace("<NUM>", mS_.GetMoney(component.serverplatz, showDollar: false)));
		}
		if (component.isLager)
		{
			text = (mS_.settings_sandbox ? text.Replace("<NUM>", mS_.GetMoney(component.lagerplatz * mS_.sandbox_lager, showDollar: false)) : text.Replace("<NUM>", mS_.GetMoney(component.lagerplatz, showDollar: false)));
		}
		if (component.qualitaet > 0)
		{
			text = text + "<br>" + tS_.GetText(532);
			text = text + "<br><size=21>" + GetQualitatStars(Mathf.RoundToInt(component.qualitaet)) + "</size><br>";
			if (component.isArbeitsplatz)
			{
				string text2 = tS_.GetText(533);
				text2 = text2.Replace("<NUM>", Mathf.RoundToInt((component.qualitaet - 1) * 10).ToString());
				text = text + "<br>" + text2;
			}
		}
		if (component.ausstattung > 0f)
		{
			text = text + "<br>" + tS_.GetText(311);
			text = text + "<br><size=21>" + GetQualitatStars(Mathf.RoundToInt(component.ausstattung)) + "</size><br>";
		}
		if (component.motivationRegen > 0f)
		{
			text = text + "<br>" + tS_.GetText(313);
			text = text + "<br><size=21>" + GetQualitatStars(Mathf.RoundToInt(component.motivationRegen / 20f)) + "</size><br>";
		}
		if (component.waerme > 0f)
		{
			int num = Mathf.RoundToInt(component.waerme);
			if (num > 5)
			{
				num = 5;
			}
			text = text + "<br>" + tS_.GetText(312);
			text = text + "<br><size=21>" + GetQualitatStars(Mathf.RoundToInt(num)) + "</size><br>";
		}
		if (component.kaelte > 0f)
		{
			text = text + "<br>" + tS_.GetText(510);
			text = text + "<br><size=21>" + GetQualitatStars(Mathf.RoundToInt(component.kaelte)) + "</size><br>";
		}
		if (component.monatsKosten > 0)
		{
			text = text + "<br>" + tS_.GetText(310) + ": <color=blue>" + mS_.GetMoney(component.monatsKosten, showDollar: true) + "</color>";
		}
		if (component.preis > 0)
		{
			text = text + "<br>" + tS_.GetText(218) + ": <color=blue>" + mS_.GetMoney(component.preis, showDollar: true) + "</color>";
		}
		if (component.aufladungenMax > 0)
		{
			string text3 = tS_.GetText(775);
			text3 = text3.Replace("<NUM>", "<color=blue>" + component.aufladungenMax + "</color>");
			text = text + "<br>" + text3;
		}
		if ((!mS_.settings_sandbox || !mS_.sandbox_allItems) && mS_.year < component.unlockYear)
		{
			string text4 = tS_.GetText(535);
			text4 = text4.Replace("<NUM>", component.unlockYear.ToString());
			text = text + "<br><br><color=red><b>" + text4 + "</b></color>";
		}
		if (component.unlockYear == 99999)
		{
			text = tS_.GetText(1217);
		}
		GetComponent<tooltip>().c = text;
	}

	private string GetQualitatStars(int i)
	{
		string text = "";
		return i switch
		{
			0 => "☆☆☆☆☆", 
			1 => "★☆☆☆☆", 
			2 => "★★☆☆☆", 
			3 => "★★★☆☆", 
			4 => "★★★★☆", 
			5 => "★★★★★", 
			_ => "☆☆☆☆☆", 
		};
	}
}
