using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_EngineFeatures : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineFeatures eF_;

	public int roomID = -1;

	private bool activ_All_Nothing;

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
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
	}

	private void Update()
	{
		Menu_Dev_Engine component = guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>();
		int num = 0;
		int num2 = 0;
		if (uiObjects[0].transform.childCount > 0)
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				if (!(uiObjects[0].transform.GetChild(i).tag != "NoItem"))
				{
					continue;
				}
				Item_DevEngine_Feature component2 = uiObjects[0].transform.GetChild(i).gameObject.GetComponent<Item_DevEngine_Feature>();
				if ((bool)component2 && component2.activ)
				{
					if (!component.featuresLock[component2.myID])
					{
						num += eF_.GetDevCostsForEngine(component2.myID);
					}
					if (num2 < eF_.engineFeatures_TECH[component2.myID])
					{
						num2 = eF_.engineFeatures_TECH[component2.myID];
					}
				}
			}
		}
		uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		uiObjects[5].GetComponent<Text>().text = tS_.GetText(4) + " " + num2;
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init()
	{
		FindScripts();
		InitDropdowns();
		CreateItems(eF_.GetTypGrafik(), "<color=white>" + tS_.GetText(9) + "</color>");
		CreateItems(eF_.GetTypSound(), "<color=white>" + tS_.GetText(10) + "</color>");
		CreateItems(eF_.GetTypKI(), "<color=white>" + tS_.GetText(11) + "</color>");
		CreateItems(eF_.GetTypPhysik(), "<color=white>" + tS_.GetText(12) + "</color>");
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
	}

	public void InitDropdowns()
	{
		int num = 0;
		Menu_Dev_Engine component = guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>();
		if ((bool)component.eSUpdate_)
		{
			num = component.eSUpdate_.GetTechLevel();
		}
		int num2 = 1;
		for (int i = 0; i < eF_.engineFeatures_TECH.Length; i++)
		{
			if (eF_.engineFeatures_RES_POINTS_LEFT[i] <= 0f && num2 < eF_.engineFeatures_TECH[i])
			{
				num2 = eF_.engineFeatures_TECH[i];
			}
		}
		List<string> list = new List<string>();
		list.Add(tS_.GetText(2192));
		for (int j = 1; j <= num2; j++)
		{
			if (num != 0)
			{
				if (num > j)
				{
					list.Add("<color=grey>" + tS_.GetText(4) + ": <b>" + j + "</b></color>");
				}
				else
				{
					list.Add(tS_.GetText(4) + ": <b>" + j + "</b>");
				}
			}
			else
			{
				list.Add(tS_.GetText(4) + ": <b>" + j + "</b>");
			}
		}
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = 0;
	}

	private void CreateItems(int typ_, string title_)
	{
		Menu_Dev_Engine component = guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>();
		Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).transform.GetChild(0).GetComponent<Text>().text = title_;
		Object.Instantiate(uiPrefabs[1], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
		for (int i = 0; i < eF_.engineFeatures_RES_POINTS.Length; i++)
		{
			if (eF_.engineFeatures_UNLOCK[i] && eF_.IsErforscht(i) && eF_.engineFeatures_TYP[i] == typ_)
			{
				Item_DevEngine_Feature component2 = Object.Instantiate(uiPrefabs[2], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevEngine_Feature>();
				component2.myID = i;
				component2.mS_ = mS_;
				component2.tS_ = tS_;
				component2.sfx_ = sfx_;
				component2.guiMain_ = guiMain_;
				component2.eF_ = eF_;
				if (component.features[i] || component.featuresLock[i])
				{
					component2.activ = true;
				}
			}
		}
		if (uiObjects[0].transform.childCount % 2 != 0)
		{
			Object.Instantiate(uiPrefabs[1], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
		}
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_All_Nothing()
	{
		uiObjects[1].GetComponent<Dropdown>().value = 0;
		Menu_Dev_Engine component = guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>();
		sfx_.PlaySound(3, force: true);
		if (uiObjects[0].transform.childCount <= 0)
		{
			return;
		}
		activ_All_Nothing = !activ_All_Nothing;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			if (uiObjects[0].transform.GetChild(i).tag != "NoItem")
			{
				Item_DevEngine_Feature component2 = uiObjects[0].transform.GetChild(i).gameObject.GetComponent<Item_DevEngine_Feature>();
				if ((bool)component2 && !component.featuresLock[component2.myID])
				{
					component2.activ = !activ_All_Nothing;
					component2.BUTTON_Click();
				}
			}
		}
	}

	public void DROWPDOWN_TechStufe()
	{
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[1].name, value);
		if (value <= 0)
		{
			return;
		}
		Menu_Dev_Engine component = guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>();
		if (uiObjects[0].transform.childCount <= 0)
		{
			return;
		}
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			if (!(uiObjects[0].transform.GetChild(i).tag != "NoItem"))
			{
				continue;
			}
			Item_DevEngine_Feature component2 = uiObjects[0].transform.GetChild(i).gameObject.GetComponent<Item_DevEngine_Feature>();
			if ((bool)component2)
			{
				if (!component.featuresLock[component2.myID] && eF_.engineFeatures_TECH[component2.myID] <= value)
				{
					component2.activ = false;
					component2.BUTTON_Click();
				}
				if (!component.featuresLock[component2.myID] && eF_.engineFeatures_TECH[component2.myID] > value)
				{
					component2.activ = true;
					component2.BUTTON_Click();
				}
			}
		}
	}
}
