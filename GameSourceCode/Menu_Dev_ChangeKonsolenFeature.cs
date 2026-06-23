using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_ChangeKonsolenFeature : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] uiPrefabs;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private licences licences_;

	private engineFeatures eF_;

	private cameraMovementScript cmS_;

	private unlockScript unlock_;

	private gameplayFeatures gF_;

	private games games_;

	private platforms platforms_;

	private hardwareFeatures hardwareFeatures_;

	private Menu_Dev_Konsole menuDevKonsole_;

	public platformScript pS_;

	public bool[] hwFeatures;

	private string searchStringA = "";

	private bool allFeatures;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
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
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!hardwareFeatures_)
		{
			hardwareFeatures_ = main_.GetComponent<hardwareFeatures>();
		}
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
		if (!menuDevKonsole_)
		{
			menuDevKonsole_ = guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>();
		}
	}

	private void Update()
	{
		if (uiObjects[7].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[8].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init(platformScript plat_)
	{
		FindScripts();
		pS_ = plat_;
		allFeatures = false;
		uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(0L, showDollar: true);
		CopyKonsolenData();
		InitDropdowns_HardwareFeatures();
		Init_KonsolenFeatures();
	}

	public int GetKomplexitaetVerteuerung()
	{
		int result = 0;
		if ((bool)pS_)
		{
			switch (pS_.complex)
			{
			case 1:
				result = 2;
				break;
			case 2:
				result = 3;
				break;
			}
		}
		return result;
	}

	private void CopyKonsolenData()
	{
		hwFeatures = (bool[])pS_.hwFeatures.Clone();
	}

	public void InitDropdowns_HardwareFeatures()
	{
		int value = PlayerPrefs.GetInt(uiObjects[2].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(6));
		uiObjects[2].GetComponent<Dropdown>().ClearOptions();
		uiObjects[2].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[2].GetComponent<Dropdown>().value = value;
	}

	private void Init_KonsolenFeatures()
	{
		FindScripts();
		if (hwFeatures.Length == 0)
		{
			hwFeatures = new bool[hardwareFeatures_.hardFeat_UNLOCK.Length];
		}
		for (int i = 0; i < uiObjects[3].transform.childCount; i++)
		{
			Object.Destroy(uiObjects[3].transform.GetChild(i).gameObject);
		}
		for (int j = 2; j < hardwareFeatures_.hardFeat_UNLOCK.Length; j++)
		{
			if (hardwareFeatures_.hardFeat_UNLOCK[j] && hardwareFeatures_.IsErforscht(j))
			{
				string text = hardwareFeatures_.GetName(j);
				searchStringA = searchStringA.ToLower();
				text = text.ToLower();
				if (uiObjects[4].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA))
				{
					Item_DevKonsole_ChangeHardwareFeature component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[3].transform).GetComponent<Item_DevKonsole_ChangeHardwareFeature>();
					component.myID = j;
					component.pS_ = pS_;
					component.hardwareFeatures_ = hardwareFeatures_;
					component.mS_ = mS_;
					component.tS_ = tS_;
					component.sfx_ = sfx_;
					component.guiMain_ = guiMain_;
					component.menu_ = this;
					component.BUTTON_Click();
					component.BUTTON_Click();
				}
			}
		}
		DROPDOWN_SortKonsoleneatures();
		guiMain_.KeinEintrag(uiObjects[3], uiObjects[5]);
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[3].transform.childCount; i++)
			{
				uiObjects[3].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[4].GetComponent<InputField>().text;
			Init_KonsolenFeatures();
		}
	}

	public void DROPDOWN_SortKonsoleneatures()
	{
		int value = uiObjects[2].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[2].name, value);
		int childCount = uiObjects[3].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[3].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_DevKonsole_ChangeHardwareFeature component = gameObject.GetComponent<Item_DevKonsole_ChangeHardwareFeature>();
				switch (value)
				{
				case 0:
					gameObject.name = hardwareFeatures_.GetName(component.myID);
					break;
				case 1:
					gameObject.name = hardwareFeatures_.GetDevCosts(component.myID).ToString();
					break;
				}
			}
		}
		if (value == 0)
		{
			mS_.SortChildrenByName(uiObjects[3]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[3]);
		}
	}

	public bool SetKonsolenFeature(int i)
	{
		hwFeatures[i] = !hwFeatures[i];
		CalcDevCosts();
		return hwFeatures[i];
	}

	private int CalcDevCosts()
	{
		int num = 0;
		for (int i = 0; i < hwFeatures.Length; i++)
		{
			if (hwFeatures[i] && !pS_.hwFeatures[i])
			{
				num += hardwareFeatures_.GetDevCosts(i);
			}
		}
		num += num * GetKomplexitaetVerteuerung();
		uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(num), showDollar: true);
		return num;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.CloseMenu();
	}

	public void BUTTON_AllKonsolenFeatures()
	{
		allFeatures = !allFeatures;
		if (!allFeatures)
		{
			DisableAllKonsolenFeatures();
			return;
		}
		for (int i = 0; i < uiObjects[3].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[3].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				gameObject.GetComponent<Item_DevKonsole_ChangeHardwareFeature>().BUTTON_Click();
			}
		}
	}

	public void DisableAllKonsolenFeatures()
	{
		for (int i = 0; i < hwFeatures.Length; i++)
		{
			if (!pS_.hwFeatures[i])
			{
				hwFeatures[i] = false;
			}
		}
		CalcDevCosts();
		sfx_.PlaySound(3, force: true);
	}

	public void BUTTON_Ok()
	{
		int num = CalcDevCosts();
		if (num <= 0)
		{
			BUTTON_Close();
			return;
		}
		mS_.Pay(num, 10);
		pS_.dev_costs += num;
		for (int i = 0; i < hwFeatures.Length; i++)
		{
			if (!pS_.hwFeatures[i] && hwFeatures[i])
			{
				pS_.hwFeatures[i] = true;
				int workPoints = hardwareFeatures_.GetWorkPoints(i);
				workPoints += workPoints * GetKomplexitaetVerteuerung();
				pS_.devPointsStart += workPoints;
				pS_.devPoints += workPoints;
			}
		}
		BUTTON_Close();
	}
}
