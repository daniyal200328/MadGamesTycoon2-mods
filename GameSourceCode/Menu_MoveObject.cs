using UnityEngine;
using UnityEngine.UI;

public class Menu_MoveObject : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private mapScript mapS_;

	private unlockScript unlock_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void Update()
	{
		mS_.snapObject = uiObjects[0].GetComponent<Toggle>().isOn;
		mS_.snapRotation = uiObjects[4].GetComponent<Toggle>().isOn;
	}

	public void BUTTON_Sell()
	{
		bool flag = false;
		if ((bool)mS_.pickedObject)
		{
			objectScript component = mS_.pickedObject.GetComponent<objectScript>();
			int num = Mathf.RoundToInt(component.GetVerkaufspreis());
			mS_.Earn(num, 0);
			if (!mS_.settings_TutorialOff && component.typ == 92)
			{
				guiMain_.SetTutorialStep(4);
			}
			flag = mS_.pickedObject.GetComponent<objectScript>().ReOpenBuyInventarMenu();
			Object.Destroy(mS_.pickedObject);
			mS_.pickedObject = null;
			mS_.ResetAllColliderLayer();
		}
		sfx_.PlaySound(11, force: true);
		mS_.UpdatePathfindingNextFrameExtern();
		guiMain_.DeactivateMenu(guiMain_.uiObjects[0]);
		if (!flag)
		{
			guiMain_.CloseMenu();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		uiObjects[0].GetComponent<Toggle>().isOn = mS_.snapObject;
		uiObjects[4].GetComponent<Toggle>().isOn = mS_.snapRotation;
		if ((bool)mS_.pickedObject)
		{
			objectScript component = mS_.pickedObject.GetComponent<objectScript>();
			uiObjects[1].GetComponent<Text>().text = tS_.GetObjects(component.typ);
			if (component.preis <= 0)
			{
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(2416);
				uiObjects[6].GetComponent<Text>().text = "";
				uiObjects[7].GetComponent<Text>().text = tS_.GetText(2417);
			}
			else
			{
				uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)component.preis * 0.5f), showDollar: true);
				uiObjects[6].GetComponent<Text>().text = tS_.GetText(88);
				uiObjects[7].GetComponent<Text>().text = tS_.GetText(85);
			}
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.inventarSprites[component.typ];
			uiObjects[5].GetComponent<tooltip>().c = SetTooltip(component.typ);
		}
	}

	private void OnDisable()
	{
		if ((bool)mS_)
		{
			mS_.UpdatePathfindingNextFrameExtern();
		}
	}

	private string SetTooltip(int typ)
	{
		string text = "";
		objectScript component = mapS_.prefabsInventar[typ].GetComponent<objectScript>();
		text = "<b>" + tS_.GetObjects(typ) + "</b>";
		text = text + "\n" + tS_.GetObjectsTooltip(typ) + "\n";
		if (component.qualitaet > 0)
		{
			text = text + "\n" + tS_.GetText(532);
			text = text + "\n<size=21>" + GetQualitatStars(Mathf.RoundToInt(component.qualitaet)) + "</size>\n";
			if (component.isArbeitsplatz)
			{
				string text2 = tS_.GetText(533);
				text2 = text2.Replace("<NUM>", Mathf.RoundToInt((component.qualitaet - 1) * 10).ToString());
				text = text + "\n" + text2;
			}
		}
		if (component.ausstattung > 0f)
		{
			text = text + "\n" + tS_.GetText(311);
			text = text + "\n<size=21>" + GetQualitatStars(Mathf.RoundToInt(component.ausstattung)) + "</size>\n";
		}
		if (component.motivationRegen > 0f)
		{
			text = text + "\n" + tS_.GetText(313);
			text = text + "\n<size=21>" + GetQualitatStars(Mathf.RoundToInt(component.motivationRegen / 20f)) + "</size>\n";
		}
		if (component.waerme > 0f)
		{
			text = text + "\n" + tS_.GetText(312);
			text = text + "\n<size=21>" + GetQualitatStars(Mathf.RoundToInt(component.waerme)) + "</size>\n";
		}
		if (component.kaelte > 0f)
		{
			text = text + "\n" + tS_.GetText(510);
			text = text + "\n<size=21>" + GetQualitatStars(Mathf.RoundToInt(component.kaelte)) + "</size>\n";
		}
		if (component.monatsKosten > 0)
		{
			text = text + "\n" + tS_.GetText(310) + ": " + mS_.GetMoney(component.monatsKosten, showDollar: true);
		}
		if (component.preis > 0)
		{
			text = text + "\n" + tS_.GetText(218) + ": " + mS_.GetMoney(component.preis, showDollar: true);
		}
		if (component.aufladungenMax > 0)
		{
			string text3 = tS_.GetText(775);
			text3 = text3.Replace("<NUM>", component.aufladungenMax.ToString());
			text = text + "\n" + text3;
		}
		if (mS_.year < component.unlockYear)
		{
			string text4 = tS_.GetText(535);
			text4 = text4.Replace("<NUM>", component.unlockYear.ToString());
			text = text + "\n\n<color=red><b>" + text4 + "</b></color>";
		}
		return text;
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
