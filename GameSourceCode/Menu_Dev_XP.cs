using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_XP : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private themes themes_;

	private Menu_DevGame mDevGame_;

	private genres genres_;

	private engineFeatures eF_;

	private gameplayFeatures gF_;

	private gameScript gS_;

	private float time_ = 0.1f;

	private bool disableOkButton = true;

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
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!mDevGame_)
		{
			mDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(15);
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		if (game_.gameTyp == 1 && game_.aboPreis <= 0)
		{
			guiMain_.uiObjects[242].SetActive(value: true);
			guiMain_.uiObjects[242].GetComponent<Menu_Abo_Preis>().Init(game_);
			base.gameObject.SetActive(value: false);
		}
		else if (game_.pubOffer)
		{
			disableOkButton = false;
			gS_ = game_;
			BUTTON_Close();
		}
		else
		{
			disableOkButton = true;
			gS_ = game_;
			StartCoroutine(CreateItems());
			guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
		}
	}

	private IEnumerator CreateItems()
	{
		if (gS_.publisherID != -1)
		{
			gS_.FindMyPublisher();
			if ((bool)gS_.pS_ && gS_.pS_.GetRelation() < 100f && !gS_.pS_.IsMyTochterfirma() && !gS_.pS_.isPlayer && gS_.reviewTotal >= 20 && ((!gS_.typ_addon && !gS_.typ_addonStandalone && !gS_.typ_mmoaddon) || Random.Range(0, 100) < 30) && Random.Range(0, 40 + mS_.difficulty * 6) < gS_.reviewTotal)
			{
				gS_.pS_.relation += 20f;
				Item_DevGame_PublisherBeziehung component = Object.Instantiate(uiPrefabs[1], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_PublisherBeziehung>();
				component.guiMain_ = guiMain_;
				component.SetData(gS_.pS_.GetName(), gS_.pS_.GetLogo(), Mathf.RoundToInt(gS_.pS_.GetRelation() / 20f));
				if (gS_.pS_.GetRelation() >= 100f)
				{
					gS_.pS_.relation = 100f;
					if ((bool)mS_.achScript_)
					{
						mS_.achScript_.SetAchivement(47);
					}
				}
				if (time_ > 0f)
				{
					sfx_.PlaySound(38, force: true);
					yield return new WaitForSeconds(time_);
				}
			}
		}
		if (Random.Range(0, 100) > 90)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					continue;
				}
				publisherScript component2 = array[i].GetComponent<publisherScript>();
				if ((bool)component2 && component2.myID != gS_.publisherID && !component2.IsMyTochterfirma() && !component2.isPlayer && component2.GetRelation() > 0f)
				{
					component2.relation -= 20f;
					Item_DevGame_PublisherBeziehung component3 = Object.Instantiate(uiPrefabs[2], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_PublisherBeziehung>();
					component3.guiMain_ = guiMain_;
					component3.SetData(component2.GetName(), component2.GetLogo(), Mathf.RoundToInt(component2.GetRelation() / 20f));
					if (time_ > 0f)
					{
						sfx_.PlaySound(24, force: true);
						yield return new WaitForSeconds(time_);
					}
					break;
				}
			}
		}
		if (genres_.genres_LEVEL[gS_.maingenre] < 5 && GetChance(genres_.genres_LEVEL[gS_.maingenre]))
		{
			int maingenre = gS_.maingenre;
			genres_.genres_LEVEL[maingenre]++;
			Item_DevGame_XP component4 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_XP>();
			component4.guiMain_ = guiMain_;
			component4.SetData(genres_.GetName(maingenre), genres_.GetPic(maingenre), genres_.genres_LEVEL[maingenre]);
			if (time_ > 0f)
			{
				sfx_.PlaySound(38, force: true);
				yield return new WaitForSeconds(time_);
			}
		}
		if (gS_.subgenre != -1 && genres_.genres_LEVEL[gS_.subgenre] < 5 && GetChance(genres_.genres_LEVEL[gS_.subgenre]))
		{
			int subgenre = gS_.subgenre;
			genres_.genres_LEVEL[subgenre]++;
			Item_DevGame_XP component5 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_XP>();
			component5.guiMain_ = guiMain_;
			component5.SetData(genres_.GetName(subgenre), genres_.GetPic(subgenre), genres_.genres_LEVEL[subgenre]);
			if (time_ > 0f)
			{
				sfx_.PlaySound(38, force: true);
				yield return new WaitForSeconds(time_);
			}
		}
		if (themes_.themes_LEVEL[gS_.gameMainTheme] < 5 && GetChance(themes_.themes_LEVEL[gS_.gameMainTheme]))
		{
			int gameMainTheme = gS_.gameMainTheme;
			themes_.themes_LEVEL[gameMainTheme]++;
			Item_DevGame_XP component6 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_XP>();
			component6.guiMain_ = guiMain_;
			component6.SetData(tS_.GetThemes(gameMainTheme), guiMain_.uiSprites[6], themes_.themes_LEVEL[gameMainTheme]);
			if (time_ > 0f)
			{
				sfx_.PlaySound(38, force: true);
				yield return new WaitForSeconds(time_);
			}
		}
		if (gS_.gameSubTheme != -1 && themes_.themes_LEVEL[gS_.gameSubTheme] < 5 && GetChance(themes_.themes_LEVEL[gS_.gameSubTheme]))
		{
			int gameSubTheme = gS_.gameSubTheme;
			themes_.themes_LEVEL[gameSubTheme]++;
			Item_DevGame_XP component7 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_XP>();
			component7.guiMain_ = guiMain_;
			component7.SetData(tS_.GetThemes(gameSubTheme), guiMain_.uiSprites[6], themes_.themes_LEVEL[gameSubTheme]);
			if (time_ > 0f)
			{
				sfx_.PlaySound(38, force: true);
				yield return new WaitForSeconds(time_);
			}
		}
		for (int j = 0; j < gS_.gamePlatform.Length; j++)
		{
			if (gS_.gamePlatform[j] == -1)
			{
				continue;
			}
			platformScript component8 = GameObject.Find("PLATFORM_" + gS_.gamePlatform[j]).GetComponent<platformScript>();
			if (component8.erfahrung < 5 && GetChance(component8.erfahrung))
			{
				component8.erfahrung++;
				Item_DevGame_XP component9 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_XP>();
				component9.guiMain_ = guiMain_;
				component9.SetData(component8.GetName(), null, component8.erfahrung);
				component8.SetPic(component9.uiObjects[1]);
				if (time_ > 0f)
				{
					sfx_.PlaySound(38, force: true);
					yield return new WaitForSeconds(time_);
				}
			}
		}
		for (int j = 0; j < gS_.gameEngineFeature.Length; j++)
		{
			if (gS_.gameEngineFeature[j] == -1)
			{
				continue;
			}
			int num = gS_.gameEngineFeature[j];
			if (eF_.engineFeatures_LEVEL[num] < 5 && GetChance(eF_.engineFeatures_LEVEL[num]))
			{
				eF_.engineFeatures_LEVEL[num]++;
				Item_DevGame_XP component10 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_XP>();
				component10.guiMain_ = guiMain_;
				component10.SetData(eF_.GetName(num), eF_.GetTypPic(num), eF_.engineFeatures_LEVEL[num]);
				if (time_ > 0f)
				{
					sfx_.PlaySound(38, force: true);
					yield return new WaitForSeconds(time_);
				}
			}
		}
		for (int j = 0; j < gS_.gameGameplayFeatures.Length; j++)
		{
			if (gS_.gameGameplayFeatures[j] && gF_.gameplayFeatures_LEVEL[j] < 5 && GetChance(gF_.gameplayFeatures_LEVEL[j]))
			{
				gF_.gameplayFeatures_LEVEL[j]++;
				Item_DevGame_XP component11 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_XP>();
				component11.guiMain_ = guiMain_;
				component11.SetData(gF_.GetName(j), gF_.GetTypSprite(j), gF_.gameplayFeatures_LEVEL[j]);
				if (time_ > 0f)
				{
					sfx_.PlaySound(38, force: true);
					yield return new WaitForSeconds(time_);
				}
			}
		}
		disableOkButton = false;
		time_ = 0.1f;
	}

	private bool GetChance(int level_)
	{
		if (gS_.reviewTotal <= 1 && level_ > 0)
		{
			return false;
		}
		if (gS_.reviewTotal <= 5 + mS_.difficulty && level_ > 1)
		{
			return false;
		}
		if (gS_.reviewTotal <= 15 + mS_.difficulty && level_ > 2)
		{
			return false;
		}
		int num = 35 + level_ * 5;
		num += mS_.difficulty * 2;
		if (gS_.typ_addon || gS_.typ_addonStandalone || gS_.typ_bundleAddon)
		{
			if (Random.Range(0, 100) > 50 && Random.Range(gS_.reviewTotal, 100) > num)
			{
				return true;
			}
		}
		else if (Random.Range(gS_.reviewTotal, 100) > num)
		{
			return true;
		}
		return false;
	}

	public void BUTTON_Close()
	{
		if (disableOkButton)
		{
			time_ = 0f;
			return;
		}
		guiMain_.ActivateMenu(guiMain_.uiObjects[72]);
		guiMain_.uiObjects[72].GetComponent<Menu_Dev_MarketAnalyse>().Init(gS_);
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
