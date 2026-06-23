using UnityEngine;
using UnityEngine.UI;

public class Menu_Messe : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int[] price;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void Init()
	{
		FindScripts();
		guiMain_.OpenMenu(hideChars: false);
		if (mS_.multiplayer && mS_.mpCalls_.isServer)
		{
			guiMain_.BUTTON_GameSpeed(0f);
			mS_.mpCalls_.SetPlayersUnready();
		}
		uiObjects[0].GetComponent<Text>().text = mS_.GetMoney(GetPrice(0), showDollar: true);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(GetPrice(1), showDollar: true);
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(GetPrice(2), showDollar: true);
		Menu_MesseErgebnis component = guiMain_.uiObjects[188].GetComponent<Menu_MesseErgebnis>();
		float num = mS_.PassedMonth();
		if (num > 600f)
		{
			num = 600f;
		}
		int num2 = Mathf.RoundToInt((float)(Mathf.RoundToInt(350000f * component.curveBesucher.Evaluate(num / 600f)) + 1000 + Random.Range(0, 1000)) * 1.5f);
		num2 = num2 / 1000 * 1000;
		string text = tS_.GetText(953);
		text = text.Replace("<NUM>", mS_.GetMoney(Mathf.RoundToInt(num2), showDollar: false));
		uiObjects[3].GetComponent<Text>().text = text;
		if ((bool)mS_.settings_ && mS_.settings_.hideConvention)
		{
			BUTTON_Abbrechen();
		}
		else
		{
			sfx_.PlaySound(50, force: false);
		}
	}

	private void Update()
	{
		if (base.gameObject.activeSelf && !guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public int GetPrice(int i)
	{
		int num = mS_.year - 1975;
		if (num > 50)
		{
			num = 50;
		}
		return price[i] * num + 5000;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
		if (mS_.multiplayer && mS_.mpCalls_.isClient)
		{
			mS_.mpCalls_.CLIENT_Send_Command(1);
		}
	}

	public void BUTTON_Stand(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[186]);
		guiMain_.uiObjects[186].GetComponent<Menu_MesseSelect>().Init(i);
	}
}
