using UnityEngine;
using UnityEngine.UI;

public class Menu_GamePass_Umsatzverlauf : MonoBehaviour
{
	public GameObject[] uiBalken;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private gamepassScript gpS_;

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
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
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
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(2127) + ": <color=green>" + mS_.GetMoney(num, showDollar: true) + "</color>";
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(2127) + ": <color=red>" + mS_.GetMoney(num, showDollar: true) + "</color>";
		}
	}

	private long InitBalken()
	{
		long num = 0L;
		float num2 = 400f;
		long num3 = 0L;
		for (int i = 0; i < gpS_.gamePass_umsatzVerlaufMonat.Length; i++)
		{
			num += gpS_.gamePass_umsatzVerlaufMonat[i];
			long num4 = gpS_.gamePass_umsatzVerlaufMonat[i];
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
		for (int j = 0; j < gpS_.gamePass_umsatzVerlaufMonat.Length; j++)
		{
			long num6 = gpS_.gamePass_umsatzVerlaufMonat[j];
			if (num6 < 0)
			{
				num6 *= -1;
			}
			uiBalken[j].GetComponent<Image>().fillAmount = (float)num6 * num5 / num2;
			if (gpS_.gamePass_umsatzVerlaufMonat[j] < 0)
			{
				uiBalken[j].GetComponent<Image>().color = guiMain_.colors[5];
			}
			else
			{
				uiBalken[j].GetComponent<Image>().color = guiMain_.colors[13];
			}
			uiBalken[j].transform.GetChild(0).GetComponent<Text>().text = mS_.GetMoney(gpS_.gamePass_umsatzVerlaufMonat[j], showDollar: true);
			uiBalken[j].transform.GetChild(1).GetComponent<Text>().text = "";
		}
		return num;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		if (!guiMain_.uiObjects[415].activeSelf)
		{
			guiMain_.CloseMenu();
		}
	}
}
