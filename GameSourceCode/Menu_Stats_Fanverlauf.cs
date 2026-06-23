using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_Fanverlauf : MonoBehaviour
{
	public GameObject[] uiPrefabs;

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

	private float updateTimer;

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
		MultiplayerUpdate();
	}

	private void MultiplayerUpdate()
	{
		if (!mS_.multiplayer)
		{
			return;
		}
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 5f))
		{
			updateTimer = 0f;
			for (int i = 0; i < uiObjects[1].transform.childCount; i++)
			{
				uiObjects[1].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			Init();
		}
	}

	public void Init()
	{
		FindScripts();
		InitBalken();
		long num = mS_.fansverlauf[0] - mS_.fansverlauf[23];
		if (num >= 0)
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(700) + ": <color=black>+" + mS_.GetMoney(num, showDollar: false) + "</color>";
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(700) + ": <color=red>" + mS_.GetMoney(num, showDollar: false) + "</color>";
		}
		for (int i = 0; i < genres_.genres_FANS.Length; i++)
		{
			if (genres_.genres_FANS[i] > 0)
			{
				GameObject gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[1].transform);
				gameObject.transform.GetChild(0).GetComponent<Text>().text = genres_.GetName(i) + "\n<color=blue>" + tS_.GetText(97) + ": " + mS_.GetMoney(genres_.genres_FANS[i], showDollar: false) + "</color>";
				gameObject.transform.GetChild(1).GetComponent<Image>().sprite = genres_.GetPic(i);
				gameObject.name = genres_.genres_FANS[i].ToString();
			}
		}
		mS_.SortChildrenByFloat(uiObjects[1]);
		uiObjects[2].GetComponent<Text>().text = tS_.GetText(1162) + ": <color=blue>" + mS_.GetMoney(genres_.GetAmountFans(), showDollar: false) + "</color>";
	}

	private void InitBalken()
	{
		float num = 400f;
		long num2 = 0L;
		for (int i = 0; i < mS_.fansverlauf.Length; i++)
		{
			if (num2 < mS_.fansverlauf[i])
			{
				num2 = mS_.fansverlauf[i];
			}
		}
		float num3 = num / (float)num2;
		for (int j = 0; j < mS_.fansverlauf.Length; j++)
		{
			uiBalken[j].GetComponent<Image>().fillAmount = (float)mS_.fansverlauf[j] * num3 / num;
			if (mS_.fansverlauf[j] > 0)
			{
				uiBalken[j].transform.GetChild(0).GetComponent<Text>().text = mS_.GetMoney(mS_.fansverlauf[j], showDollar: false);
				if (j < mS_.fansverlauf.Length - 1)
				{
					long num4 = mS_.fansverlauf[j] - mS_.fansverlauf[j + 1];
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
