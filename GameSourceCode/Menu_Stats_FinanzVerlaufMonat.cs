using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_FinanzVerlaufMonat : MonoBehaviour
{
	public GameObject[] uiBalken;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private engineFeatures eF_;

	private engineScript eS_;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
	}

	private void OnEnable()
	{
		Init();
	}

	private void Update()
	{
		if (mS_.multiplayer)
		{
			Init();
		}
	}

	public void Init()
	{
		FindScripts();
		long num = InitBalken();
		if (num >= 0)
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(724) + ": <color=#13C05C>" + mS_.GetMoney(num, showDollar: true) + "</color>";
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(724) + ": <color=#A40000>" + mS_.GetMoney(num, showDollar: true) + "</color>";
		}
	}

	private long InitBalken()
	{
		long num = 0L;
		float num2 = 400f;
		long num3 = 0L;
		for (int i = 0; i < mS_.finanzVerlauf.Length; i++)
		{
			num += mS_.finanzVerlauf[i];
			long num4 = mS_.finanzVerlauf[i];
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
		for (int j = 0; j < mS_.verkaufsverlauf.Length; j++)
		{
			long num6 = mS_.finanzVerlauf[j];
			if (num6 < 0)
			{
				num6 *= -1;
			}
			uiBalken[j].GetComponent<Image>().fillAmount = (float)num6 * num5 / num2;
			if (mS_.finanzVerlauf[j] < 0)
			{
				uiBalken[j].GetComponent<Image>().color = guiMain_.colors[5];
			}
			else
			{
				uiBalken[j].GetComponent<Image>().color = guiMain_.colors[30];
			}
			uiBalken[j].transform.GetChild(0).GetComponent<Text>().text = "";
			uiBalken[j].transform.GetChild(1).GetComponent<Text>().text = mS_.GetMoney(mS_.finanzVerlauf[j], showDollar: true);
		}
		return num;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
