using UnityEngine;
using UnityEngine.UI;

public class Menu_MesseErgebnis : MonoBehaviour
{
	public GameObject[] uiObjects;

	public AnimationCurve curveBesucher;

	public Sprite[] besucherSprites;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private Menu_MesseSelect menu_;

	private float besucherGesamt;

	private float besucherSoll;

	private float besucherIst;

	private int neueFans;

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
		if (!menu_)
		{
			menu_ = guiMain_.uiObjects[186].GetComponent<Menu_MesseSelect>();
		}
	}

	public void Init()
	{
		FindScripts();
		float num = mS_.PassedMonth();
		if (num > 600f)
		{
			num = 600f;
		}
		float num2 = 0f;
		num2 = 350000f * curveBesucher.Evaluate(num / 600f);
		besucherSoll = Mathf.RoundToInt(num2) + 1000 + Random.Range(0, 1000);
		besucherIst = 0f;
		besucherGesamt = besucherSoll * 1.5f * Random.Range(1f, 1.1f);
		switch (menu_.standGroesse)
		{
		case 0:
			besucherSoll *= 0.1f;
			break;
		case 1:
			besucherSoll *= 0.4f;
			break;
		}
		neueFans = Mathf.RoundToInt(besucherSoll * 0.15f);
		uiObjects[2].GetComponent<Text>().text = "";
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).GetComponent<Image>().sprite = besucherSprites[0];
		}
		string text = tS_.GetText(954);
		text = text.Replace("<NUM>", mS_.GetMoney(Mathf.RoundToInt(besucherGesamt), showDollar: false));
		uiObjects[6].GetComponent<Text>().text = text;
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
		if (besucherSoll != besucherIst)
		{
			besucherIst = Mathf.Lerp(besucherIst, besucherSoll, 0.1f);
			if (besucherSoll - besucherIst < 10f)
			{
				besucherIst = besucherSoll;
				uiObjects[2].GetComponent<Text>().text = "+" + mS_.GetMoney(neueFans, showDollar: false) + " " + tS_.GetText(97);
				sfx_.PlaySound(51, force: false);
			}
			int childCount = uiObjects[0].transform.childCount;
			int num = 350000 / childCount;
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				if (besucherIst >= (float)(i * num))
				{
					uiObjects[0].transform.GetChild(uiObjects[0].transform.childCount - 1 - i).GetComponent<Image>().sprite = besucherSprites[1];
				}
			}
		}
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(besucherIst), showDollar: false);
	}

	public void BUTTON_Abbrechen()
	{
		if (mS_.multiplayer && mS_.mpCalls_.isClient)
		{
			mS_.mpCalls_.CLIENT_Send_Command(1);
		}
		sfx_.PlaySound(3, force: true);
		mS_.AddFans(neueFans, -1);
		int num = mS_.difficulty + 1;
		if ((bool)menu_.games[0])
		{
			menu_.games[0].AddHype(Random.Range(50 / num, 100 / num));
		}
		if ((bool)menu_.games[1])
		{
			menu_.games[1].AddHype(Random.Range(50 / num, 100 / num));
		}
		if ((bool)menu_.games[2])
		{
			menu_.games[2].AddHype(Random.Range(50 / num, 100 / num));
		}
		if ((bool)menu_.konsolen[0])
		{
			menu_.konsolen[0].AddHype(Random.Range(50 / num, 100 / num));
		}
		if ((bool)menu_.konsolen[1])
		{
			menu_.konsolen[1].AddHype(Random.Range(50 / num, 100 / num));
		}
		base.gameObject.SetActive(value: false);
		guiMain_.CloseMenu();
	}
}
