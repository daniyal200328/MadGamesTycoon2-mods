using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_SelectPublisher : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private themes themes_;

	private Menu_DevGame mDevGame_;

	private genres genres_;

	private gameScript gS_;

	private taskGame task_;

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
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init(gameScript game_, taskGame t_)
	{
		FindScripts();
		gS_ = game_;
		task_ = t_;
		InitDropdowns();
		uiObjects[1].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[6].GetComponent<Image>().sprite = genres_.GetPic(gS_.maingenre);
		uiObjects[8].GetComponent<Text>().text = genres_.GetName(gS_.maingenre);
		if (gS_.subgenre != -1)
		{
			uiObjects[7].GetComponent<Image>().sprite = genres_.GetPic(gS_.subgenre);
			uiObjects[9].GetComponent<Text>().text = genres_.GetName(gS_.subgenre);
		}
		else
		{
			uiObjects[7].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[9].GetComponent<Text>().text = "---";
		}
		publisherScript publisherScript2 = null;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			publisherScript component = array[i].GetComponent<publisherScript>();
			if (!component.Geschlossen() && component.isUnlocked && !component.TochterfirmaGeschlossen() && !component.isPlayer && !component.IsTochterfirmaVonMitspieler() && component.publisher && !component.onlyMobile)
			{
				if ((float)gS_.reviewTotal >= component.GetMinimalReviewPoints() || component.IsMyTochterfirma())
				{
					Item_SelectPublisher component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_SelectPublisher>();
					component2.pS_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
				}
				if (publisherScript2 == null)
				{
					publisherScript2 = component;
				}
				if ((bool)publisherScript2 && publisherScript2.GetMinimalReviewPoints() > component.GetMinimalReviewPoints())
				{
					publisherScript2 = component;
				}
			}
		}
		if (uiObjects[0].transform.childCount <= 0 && (bool)publisherScript2)
		{
			Item_SelectPublisher component3 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_SelectPublisher>();
			component3.pS_ = publisherScript2;
			component3.mS_ = mS_;
			component3.tS_ = tS_;
			component3.sfx_ = sfx_;
			component3.guiMain_ = guiMain_;
			component3.genres_ = genres_;
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
	}

	public void InitDropdowns()
	{
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[5].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(355));
		list.Add(tS_.GetText(434));
		list.Add(tS_.GetText(435));
		list.Add(tS_.GetText(436));
		list.Add(tS_.GetText(437));
		uiObjects[5].GetComponent<Dropdown>().ClearOptions();
		uiObjects[5].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[5].GetComponent<Dropdown>().value = value;
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[5].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[5].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_SelectPublisher component = gameObject.GetComponent<Item_SelectPublisher>();
				switch (value)
				{
				case 0:
					gameObject.name = component.pS_.GetName();
					break;
				case 1:
					gameObject.name = component.pS_.stars.ToString();
					break;
				case 2:
					gameObject.name = component.pS_.GetRelation().ToString();
					break;
				case 3:
					gameObject.name = component.pS_.GetShare().ToString();
					break;
				case 4:
					gameObject.name = component.pS_.fanGenre.ToString();
					break;
				}
			}
		}
		if (value == 0)
		{
			mS_.SortChildrenByName(uiObjects[0]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[0]);
		}
	}

	public void BUTTON_Close()
	{
		if (!task_ || !gS_)
		{
			base.gameObject.SetActive(value: false);
			return;
		}
		gS_.ClearReview();
		guiMain_.ActivateMenu(guiMain_.uiObjects[69]);
		guiMain_.uiObjects[69].GetComponent<Menu_DevGame_Complete>().Init(gS_, task_);
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void SelectPublisher(int id_)
	{
		Object.Destroy(task_.gameObject);
		if (gS_.reviewTotal <= 0)
		{
			gS_.CalcReview(entwicklungsbericht: false);
			if ((bool)mS_)
			{
				mS_.reviewText_.GetReviewText(gS_);
			}
		}
		else
		{
			gS_.date_year = mS_.year;
			gS_.date_month = mS_.month;
			if ((bool)mS_)
			{
				mS_.reviewText_.GetReviewText(gS_);
			}
		}
		gS_.SetVerkaufspreisNPC();
		gS_.SetPublisher(id_);
		gS_.SetOnMarket();
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[71]);
		guiMain_.uiObjects[71].GetComponent<Menu_Dev_XP>().Init(gS_);
	}
}
