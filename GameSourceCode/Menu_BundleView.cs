using UnityEngine;
using UnityEngine.UI;

public class Menu_BundleView : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private games games_;

	public gameScript gS_;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
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

	public void Init(gameScript script_)
	{
		FindScripts();
		gS_ = script_;
		for (int i = 0; i < gS_.bundleID.Length; i++)
		{
			SetGame(i, gS_.GetBundleGame(i));
		}
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[27].GetComponent<Text>().text = mS_.GetMoney(gS_.sellsTotal, showDollar: false);
		if (gS_.GetGesamtGewinn() >= 0)
		{
			uiObjects[28].GetComponent<Text>().text = mS_.GetMoney(gS_.GetGesamtGewinn(), showDollar: true);
		}
		else
		{
			uiObjects[28].GetComponent<Text>().text = "<color=red>" + mS_.GetMoney(gS_.GetGesamtGewinn(), showDollar: true) + "</color>";
		}
	}

	public void SetGame(int slot, gameScript script_)
	{
		if (!script_)
		{
			uiObjects[22 + slot].GetComponent<tooltip>().c = "";
			uiObjects[2 + slot].GetComponent<Text>().text = "";
			uiObjects[7 + slot].GetComponent<Text>().text = "";
			uiObjects[12 + slot].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			uiObjects[17 + slot].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			uiObjects[29 + slot].GetComponent<Text>().text = "";
		}
		else
		{
			uiObjects[22 + slot].GetComponent<tooltip>().c = script_.GetTooltip();
			uiObjects[2 + slot].GetComponent<Text>().text = "<b>" + script_.GetNameWithTag() + "</b>";
			uiObjects[7 + slot].GetComponent<Text>().text = script_.GetReleaseDateString();
			uiObjects[12 + slot].GetComponent<Image>().sprite = genres_.GetPic(script_.maingenre);
			uiObjects[17 + slot].GetComponent<Image>().sprite = guiMain_.uiSprites[30];
			uiObjects[29 + slot].GetComponent<Text>().text = Mathf.RoundToInt(script_.reviewTotal) + "%";
		}
		guiMain_.DrawStarsColor(uiObjects[1], Mathf.RoundToInt(GetQuality()), Color.white);
	}

	public float GetQuality()
	{
		return gS_.reviewTotal / 20;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
