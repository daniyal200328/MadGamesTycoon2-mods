using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_Verkaufsverlauf : MonoBehaviour
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
		long i = InitBalken();
		uiObjects[0].GetComponent<Text>().text = tS_.GetText(703) + ": " + mS_.GetMoney(i, showDollar: false);
	}

	private long InitBalken()
	{
		long num = 0L;
		float num2 = 400f;
		long num3 = 0L;
		for (int i = 0; i < mS_.verkaufsverlauf.Length; i++)
		{
			num += mS_.verkaufsverlauf[i];
			if (num3 < mS_.verkaufsverlauf[i])
			{
				num3 = mS_.verkaufsverlauf[i];
			}
		}
		float num4 = num2 / (float)num3;
		for (int j = 0; j < mS_.verkaufsverlauf.Length; j++)
		{
			uiBalken[j].GetComponent<Image>().fillAmount = (float)mS_.verkaufsverlauf[j] * num4 / num2;
			if (mS_.verkaufsverlauf[j] > 0)
			{
				uiBalken[j].transform.GetChild(0).GetComponent<Text>().text = mS_.GetMoney(mS_.verkaufsverlauf[j], showDollar: false);
				if (j < mS_.verkaufsverlauf.Length - 1)
				{
					long num5 = mS_.verkaufsverlauf[j] - mS_.verkaufsverlauf[j + 1];
					if (num5 > 0)
					{
						uiBalken[j].transform.GetChild(1).GetComponent<Text>().text = "+" + mS_.GetMoney(num5, showDollar: false);
						uiBalken[j].transform.GetChild(1).GetComponent<Text>().color = guiMain_.colors[4];
					}
					else
					{
						uiBalken[j].transform.GetChild(1).GetComponent<Text>().text = mS_.GetMoney(num5, showDollar: false);
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
		return num;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
