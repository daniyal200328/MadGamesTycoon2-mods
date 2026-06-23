using UnityEngine;
using UnityEngine.UI;

public class Menu_KonsolenReview : MonoBehaviour
{
	public GameObject[] uiObjects;

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

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	private platforms platforms_;

	private platformScript pS_;

	private float techDifference;

	private float techDifferenceShow;

	private float reviewPoints;

	private float reviewPointsShow;

	private float proKonsoleReviewPoints;

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
		if (!hardware_)
		{
			hardware_ = main_.GetComponent<hardware>();
		}
		if (!hardwareFeatures_)
		{
			hardwareFeatures_ = main_.GetComponent<hardwareFeatures>();
		}
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
	}

	public void Init(platformScript plat_)
	{
		FindScripts();
		pS_ = plat_;
		techDifferenceShow = -5f;
		techDifference = pS_.GetTechDifference();
		proKonsoleReviewPoints = 0f;
		if (pS_.proVersion)
		{
			proKonsoleReviewPoints = pS_.review;
		}
		reviewPointsShow = 0f;
		reviewPoints = GetGesamtbewertung();
		pS_.review = reviewPoints;
		if (pS_.proVersion && pS_.review < proKonsoleReviewPoints)
		{
			reviewPoints = proKonsoleReviewPoints;
			pS_.review = proKonsoleReviewPoints;
		}
		if (pS_.proVersion)
		{
			string text = tS_.GetText(2324);
			text = text.Replace("<NAME>", "<color=blue>" + pS_.GetProName() + "</color>");
			uiObjects[4].GetComponent<Text>().text = text;
		}
		else
		{
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(1650);
		}
		SetData();
	}

	private void Update()
	{
		reviewPointsShow = Mathf.Lerp(reviewPointsShow, reviewPoints, 0.04f);
		techDifferenceShow = Mathf.Lerp(techDifferenceShow, techDifference, 0.04f);
		uiObjects[3].GetComponent<Text>().text = mS_.Round(reviewPointsShow, 1) + " / 10";
		if (techDifferenceShow < 0f)
		{
			int value = Mathf.RoundToInt(techDifferenceShow);
			switch (Mathf.Abs(value))
			{
			case 0:
				guiMain_.DrawStars(uiObjects[2], 4);
				break;
			case 1:
				guiMain_.DrawStars(uiObjects[2], 3);
				break;
			case 2:
				guiMain_.DrawStars(uiObjects[2], 2);
				break;
			case 3:
				guiMain_.DrawStars(uiObjects[2], 1);
				break;
			case 4:
				guiMain_.DrawStars(uiObjects[2], 0);
				break;
			}
		}
		else
		{
			guiMain_.DrawStars(uiObjects[2], 5);
		}
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		pS_.SetPic(uiObjects[1]);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		if (Mathf.RoundToInt(techDifferenceShow) != Mathf.RoundToInt(techDifference) || Mathf.RoundToInt(reviewPointsShow) != Mathf.RoundToInt(reviewPoints))
		{
			techDifferenceShow = pS_.GetTechDifference();
			reviewPointsShow = pS_.review;
		}
		else
		{
			base.gameObject.SetActive(value: false);
			guiMain_.CloseMenu();
		}
	}

	private float GetGesamtbewertung()
	{
		float num = pS_.GetTechDifference();
		float num2 = 0f;
		num2 = ((!(num >= 0f)) ? (9f - Mathf.Abs(num) * 2f) : (9f + num));
		for (int i = 0; i < hardware_.hardware_UNLOCK.Length; i++)
		{
			if (!hardware_.hardware_UNLOCK[i])
			{
				continue;
			}
			if (pS_.typ == 1)
			{
				if (hardware_.hardware_TYP[i] == 0 && !hardware_.hardware_ONLYHANDHELD[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_cpu])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 1 && !hardware_.hardware_ONLYHANDHELD[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_gfx])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 2 && !hardware_.hardware_ONLYHANDHELD[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_ram])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 3 && !hardware_.hardware_ONLYHANDHELD[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_hdd])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 4 && !hardware_.hardware_ONLYHANDHELD[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_sfx])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 5 && !hardware_.hardware_ONLYHANDHELD[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_cooling])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 6 && !hardware_.hardware_ONLYHANDHELD[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_disc])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 7 && !hardware_.hardware_ONLYHANDHELD[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_controller])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 8 && !hardware_.hardware_ONLYHANDHELD[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_case])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 9 && !hardware_.hardware_ONLYHANDHELD[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_monitor])
				{
					num2 -= 0.1f;
				}
			}
			else
			{
				if (hardware_.hardware_TYP[i] == 0 && !hardware_.hardware_ONLYSTATIONARY[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_cpu])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 1 && !hardware_.hardware_ONLYSTATIONARY[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_gfx])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 2 && !hardware_.hardware_ONLYSTATIONARY[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_ram])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 3 && !hardware_.hardware_ONLYSTATIONARY[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_hdd])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 4 && !hardware_.hardware_ONLYSTATIONARY[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_sfx])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 5 && !hardware_.hardware_ONLYSTATIONARY[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_cooling])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 6 && !hardware_.hardware_ONLYSTATIONARY[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_disc])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 7 && !hardware_.hardware_ONLYSTATIONARY[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_controller])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 8 && !hardware_.hardware_ONLYSTATIONARY[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_case])
				{
					num2 -= 0.1f;
				}
				if (hardware_.hardware_TYP[i] == 9 && !hardware_.hardware_ONLYSTATIONARY[i] && hardware_.hardware_RES_POINTS[i] > hardware_.hardware_RES_POINTS[pS_.component_monitor])
				{
					num2 -= 0.1f;
				}
			}
		}
		for (int j = 1; j < pS_.hwFeatures.Length; j++)
		{
			if (!hardwareFeatures_.hardFeat_UNLOCK[j])
			{
				continue;
			}
			if (pS_.typ == 1)
			{
				if (!hardwareFeatures_.hardFeat_ONLYHANDHELD[j] && !pS_.hwFeatures[j])
				{
					num2 -= hardwareFeatures_.hardFeat_QUALITY[j] * 0.1f;
				}
			}
			else if (!hardwareFeatures_.hardFeat_ONLYSTATIONARY[j] && !pS_.hwFeatures[j])
			{
				num2 -= hardwareFeatures_.hardFeat_QUALITY[j] * 0.1f;
			}
		}
		if (pS_.typ == 1)
		{
			switch (pS_.anzController)
			{
			case 1:
				num2 -= 0.6f;
				break;
			case 2:
				num2 -= 0.4f;
				break;
			case 3:
				num2 -= 0.2f;
				break;
			}
		}
		if (pS_.gameID == -1)
		{
			num2 -= 0.1f;
		}
		if (!pS_.internet && hardwareFeatures_.hardFeat_UNLOCK[0] && platforms_.ExistInternetReadyConsole())
		{
			num2 -= 2f;
		}
		switch (mS_.difficulty)
		{
		case 0:
			num2 += 1.5f;
			break;
		case 1:
			num2 += 1f;
			break;
		case 2:
			num2 += 0.7f;
			break;
		case 3:
			num2 += 0.4f;
			break;
		case 4:
			num2 += 0.2f;
			break;
		case 5:
			num2 += 0f;
			break;
		}
		if (num2 > 10f)
		{
			num2 = 10f;
		}
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		return num2;
	}
}
