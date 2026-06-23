using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_TochterfirmaUmsatz : MonoBehaviour
{
	public GameObject[] uiBalken;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private publisherScript pS_;

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

	public void Init(publisherScript script_)
	{
		FindScripts();
		pS_ = script_;
		if ((bool)pS_)
		{
			long num = InitBalken();
			if (num >= 0)
			{
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(698) + ": <color=green><b>" + mS_.GetMoney(num, showDollar: true) + "</b></color>";
			}
			else
			{
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(698) + ": <color=red><b>" + mS_.GetMoney(num, showDollar: true) + "</b></color>";
			}
			long tf_umsatz_allTime = pS_.tf_umsatz_allTime;
			if (tf_umsatz_allTime >= 0)
			{
				uiObjects[1].GetComponent<Text>().text = tS_.GetText(724) + ": <color=green><b>" + mS_.GetMoney(tf_umsatz_allTime, showDollar: true) + "</b></color>";
			}
			else
			{
				uiObjects[1].GetComponent<Text>().text = tS_.GetText(724) + ": <color=red><b>" + mS_.GetMoney(tf_umsatz_allTime, showDollar: true) + "</b></color>";
			}
		}
	}

	private long InitBalken()
	{
		long num = 0L;
		float num2 = 400f;
		long num3 = 0L;
		for (int i = 0; i < pS_.tf_umsatz.Length; i++)
		{
			num += pS_.tf_umsatz[i];
			long num4 = pS_.tf_umsatz[i];
			if (num4 < 0)
			{
				num4 *= -1;
			}
			if (num3 < num4)
			{
				num3 = num4;
			}
		}
		float num5 = num2 / (float)num3;
		for (int j = 0; j < pS_.tf_umsatz.Length; j++)
		{
			long num6 = pS_.tf_umsatz[j];
			if (num6 < 0)
			{
				num6 *= -1;
			}
			uiBalken[j].GetComponent<Image>().fillAmount = (float)num6 * num5 / num2;
			if (pS_.tf_umsatz[j] < 0)
			{
				uiBalken[j].GetComponent<Image>().color = guiMain_.colors[5];
			}
			else
			{
				uiBalken[j].GetComponent<Image>().color = guiMain_.colors[7];
			}
			uiBalken[j].transform.GetChild(0).GetComponent<Text>().text = mS_.GetMoney(pS_.tf_umsatz[j], showDollar: true);
			uiBalken[j].transform.GetChild(1).GetComponent<Text>().text = "";
		}
		return num;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
