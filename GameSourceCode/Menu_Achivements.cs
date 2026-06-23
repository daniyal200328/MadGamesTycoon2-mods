using UnityEngine;
using UnityEngine.UI;

public class Menu_Achivements : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

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
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	private void OnEnable()
	{
		Init();
	}

	public void Init()
	{
		FindScripts();
		SetData();
	}

	private void SetData()
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			Object.Destroy(uiObjects[0].transform.GetChild(i).gameObject);
		}
		int num = 0;
		int num2 = 0;
		bool isOn = uiObjects[6].GetComponent<Toggle>().isOn;
		for (int j = 0; j < mS_.achivements.Length; j++)
		{
			if (mS_.achivementsDisabled[j])
			{
				continue;
			}
			num++;
			if (mS_.achivements[j])
			{
				num2++;
			}
			if ((bool)guiMain_.iconAchivements[j] && (!isOn || !mS_.achivements[j]))
			{
				GameObject gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				Item_Achivement component = gameObject.GetComponent<Item_Achivement>();
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.guiMain_ = guiMain_;
				component.SetData(j);
				if (mS_.achivements[j])
				{
					gameObject.name = "1";
				}
				else
				{
					gameObject.name = "0";
				}
			}
		}
		mS_.SortChildrenByFloat(uiObjects[0]);
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
		string text = tS_.GetText(1800);
		text = text.Replace("<NUM1>", num2.ToString());
		text = text.Replace("<NUM2>", num.ToString());
		uiObjects[1].GetComponent<Text>().text = text;
		text = "";
		if (mS_.GetAchivementBonus(0) > 0)
		{
			text = tS_.GetText(1801) + "\n";
			text = text.Replace("<NUM>", mS_.GetAchivementBonus(0).ToString());
		}
		if (mS_.GetAchivementBonus(1) > 0)
		{
			text = text + tS_.GetText(1802) + "\n";
			text = text.Replace("<NUM>", mS_.GetAchivementBonus(1).ToString());
		}
		if (mS_.GetAchivementBonus(2) > 0)
		{
			text = text + tS_.GetText(1803) + "\n";
			text = text.Replace("<NUM>", mS_.GetAchivementBonus(2).ToString());
		}
		if (mS_.GetAchivementBonus(3) > 0)
		{
			text = text + tS_.GetText(1804) + "\n";
			text = text.Replace("<NUM>", mS_.GetAchivementBonus(3).ToString());
		}
		if (mS_.GetAchivementBonus(4) > 0)
		{
			text = text + tS_.GetText(1805) + "\n";
			text = text.Replace("<NUM>", mS_.GetAchivementBonus(4).ToString());
		}
		if (mS_.GetAchivementBonus(5) > 0)
		{
			text = text + tS_.GetText(1806) + "\n";
			text = text.Replace("<NUM>", mS_.GetAchivementBonus(5).ToString());
		}
		if (mS_.GetAchivementBonus(6) > 0)
		{
			text = text + tS_.GetText(1807) + "\n";
			text = text.Replace("<NUM>", mS_.GetAchivementBonus(6).ToString());
		}
		if (mS_.GetAchivementBonus(7) > 0)
		{
			text = text + tS_.GetText(1808) + "\n";
			text = text.Replace("<NUM>", mS_.GetAchivementBonus(7).ToString());
		}
		if (mS_.GetAchivementBonus(8) > 0)
		{
			text = text + tS_.GetText(1809) + "\n";
			text = text.Replace("<NUM>", mS_.GetAchivementBonus(8).ToString());
		}
		if (mS_.GetAchivementBonus(9) > 0)
		{
			text = text + tS_.GetText(1810) + "\n";
			text = text.Replace("<NUM>", mS_.GetAchivementBonus(9).ToString());
		}
		uiObjects[7].GetComponent<Text>().text = text;
	}

	public void TOGGLE_Ausblenden()
	{
		SetData();
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
