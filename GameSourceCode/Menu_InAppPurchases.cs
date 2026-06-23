using UnityEngine;
using UnityEngine.UI;

public class Menu_InAppPurchases : MonoBehaviour
{
	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private games games_;

	private gameScript gS_;

	public bool[] g_InAppPurchase;

	private bool closeMenu;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
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
	}

	public void Init(gameScript game_, bool closeMenu_)
	{
		closeMenu = closeMenu_;
		FindScripts();
		gS_ = game_;
		SetData();
	}

	private void SetData()
	{
		for (int i = 0; i < gS_.inAppPurchase.Length; i++)
		{
			g_InAppPurchase[i] = gS_.inAppPurchase[i];
		}
		SetMaxVerdienstInApp();
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[1].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[0], 1) + "0";
		uiObjects[2].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[1], 1) + "0";
		uiObjects[3].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[2], 1) + "0";
		uiObjects[4].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[3], 1) + "0";
		uiObjects[5].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[4], 1) + "0";
		uiObjects[6].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[5], 1) + "0";
		uiObjects[8].GetComponent<Text>().text = mS_.GetMoney(gS_.umsatzInApp, showDollar: true);
	}

	public void BUTTON_InAppPurchase(int i)
	{
		sfx_.PlaySound(3, force: true);
		g_InAppPurchase[i] = !g_InAppPurchase[i];
		SetMaxVerdienstInApp();
	}

	public void BUTTON_AlleInAppPurchase()
	{
		sfx_.PlaySound(3, force: true);
		bool flag = g_InAppPurchase[0];
		for (int i = 0; i < 6; i++)
		{
			g_InAppPurchase[i] = !flag;
		}
		SetMaxVerdienstInApp();
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		if (closeMenu)
		{
			guiMain_.CloseMenu();
		}
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		for (int i = 0; i < gS_.inAppPurchase.Length; i++)
		{
			gS_.inAppPurchase[i] = g_InAppPurchase[i];
		}
		guiMain_.uiObjects[277].GetComponent<Menu_InAppVerwalten>().Init();
		sfx_.PlaySound(3, force: true);
		if (closeMenu)
		{
			guiMain_.CloseMenu();
		}
		base.gameObject.SetActive(value: false);
	}

	private float SetMaxVerdienstInApp()
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < 6; i++)
		{
			if (g_InAppPurchase[i])
			{
				num += games_.inAppPurchasePrice[i];
				num2 += games_.inAppPurchaseHate[i];
				uiObjects[10 + i].GetComponent<Image>().color = guiMain_.colors[20];
			}
			else
			{
				uiObjects[10 + i].GetComponent<Image>().color = Color.white;
			}
		}
		uiObjects[7].GetComponent<Text>().text = "$" + mS_.Round(num, 2);
		uiObjects[9].GetComponent<Image>().fillAmount = num2 * 0.01f;
		uiObjects[9].GetComponent<Image>().color = GetValColor(100f - num2);
		return num;
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
}
