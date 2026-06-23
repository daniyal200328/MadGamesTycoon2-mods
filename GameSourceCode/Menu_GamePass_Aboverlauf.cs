using UnityEngine;
using UnityEngine.UI;

public class Menu_GamePass_Aboverlauf : MonoBehaviour
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
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
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
		InitBalken();
		long num = gpS_.gamePass_aboVerlaufMonat[0] - gpS_.gamePass_aboVerlaufMonat[23];
		if (num >= 0)
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(700) + ": <color=black>+" + mS_.GetMoney(num, showDollar: false) + "</color>";
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(700) + ": <color=red>" + mS_.GetMoney(num, showDollar: false) + "</color>";
		}
	}

	private void InitBalken()
	{
		float num = 400f;
		long num2 = 0L;
		for (int i = 0; i < gpS_.gamePass_aboVerlaufMonat.Length; i++)
		{
			if (num2 < gpS_.gamePass_aboVerlaufMonat[i])
			{
				num2 = gpS_.gamePass_aboVerlaufMonat[i];
			}
		}
		float num3 = num / (float)num2;
		for (int j = 0; j < gpS_.gamePass_aboVerlaufMonat.Length; j++)
		{
			uiBalken[j].GetComponent<Image>().fillAmount = (float)gpS_.gamePass_aboVerlaufMonat[j] * num3 / num;
			if (gpS_.gamePass_aboVerlaufMonat[j] > 0)
			{
				uiBalken[j].transform.GetChild(0).GetComponent<Text>().text = mS_.GetMoney(gpS_.gamePass_aboVerlaufMonat[j], showDollar: false);
				if (j < gpS_.gamePass_aboVerlaufMonat.Length - 1)
				{
					long num4 = gpS_.gamePass_aboVerlaufMonat[j] - gpS_.gamePass_aboVerlaufMonat[j + 1];
					if (num4 >= 0)
					{
						uiBalken[j].transform.GetChild(1).GetComponent<Text>().text = "+" + mS_.GetMoney(num4, showDollar: false);
						uiBalken[j].transform.GetChild(1).GetComponent<Text>().color = guiMain_.colors[4];
					}
					else
					{
						uiBalken[j].transform.GetChild(1).GetComponent<Text>().text = mS_.GetMoney(num4, showDollar: false);
						uiBalken[j].transform.GetChild(1).GetComponent<Text>().color = guiMain_.colors[8];
					}
				}
			}
			else
			{
				uiBalken[j].transform.GetChild(0).GetComponent<Text>().text = "";
				uiBalken[j].transform.GetChild(1).GetComponent<Text>().text = "";
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
