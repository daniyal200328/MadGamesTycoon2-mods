using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_Aboverlauf : MonoBehaviour
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
		InitBalken();
		long num = mS_.aboverlauf[0] - mS_.aboverlauf[23];
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
		for (int i = 0; i < mS_.aboverlauf.Length; i++)
		{
			if (num2 < mS_.aboverlauf[i])
			{
				num2 = mS_.aboverlauf[i];
			}
		}
		float num3 = num / (float)num2;
		for (int j = 0; j < mS_.aboverlauf.Length; j++)
		{
			uiBalken[j].GetComponent<Image>().fillAmount = (float)mS_.aboverlauf[j] * num3 / num;
			if (mS_.aboverlauf[j] > 0)
			{
				uiBalken[j].transform.GetChild(0).GetComponent<Text>().text = mS_.GetMoney(mS_.aboverlauf[j], showDollar: false);
				if (j < mS_.aboverlauf.Length - 1)
				{
					long num4 = mS_.aboverlauf[j] - mS_.aboverlauf[j + 1];
					if (num4 > 0)
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
