using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_Fanbriefe : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private gameScript gS_;

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

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		string text = tS_.GetText(668);
		text = text.Replace("<NAME>", gS_.GetNameWithTag());
		uiObjects[1].GetComponent<Text>().text = text;
		for (int i = 0; i < game_.fanbrief.Length; i++)
		{
			if (game_.fanbrief[i])
			{
				GameObject obj = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				text = tS_.GetFanLetter(i);
				text = text.Replace("<NAME>", game_.GetNameWithTag());
				obj.transform.GetChild(0).GetComponent<Text>().text = text;
			}
		}
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
		uiObjects[6].GetComponent<Button>().interactable = false;
		uiObjects[6].SetActive(value: false);
		if (uiPrefabs[0].transform.childCount > 0)
		{
			if (guiMain_.uiObjects[56].activeSelf)
			{
				uiObjects[6].SetActive(value: true);
				uiObjects[6].GetComponent<Button>().interactable = true;
			}
			if (guiMain_.uiObjects[247].activeSelf)
			{
				uiObjects[6].SetActive(value: true);
				uiObjects[6].GetComponent<Button>().interactable = true;
			}
			if (guiMain_.uiObjects[193].activeSelf)
			{
				uiObjects[6].SetActive(value: true);
				uiObjects[6].GetComponent<Button>().interactable = true;
			}
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Anheften()
	{
		sfx_.PlaySound(3, force: false);
		GameObject gameObject = null;
		if (guiMain_.uiObjects[56].activeSelf)
		{
			gameObject = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().uiObjects[227];
		}
		if (guiMain_.uiObjects[247].activeSelf)
		{
			gameObject = guiMain_.uiObjects[247].GetComponent<Menu_Dev_MMOAddon>().uiObjects[79];
		}
		if (guiMain_.uiObjects[193].activeSelf)
		{
			gameObject = guiMain_.uiObjects[193].GetComponent<Menu_Dev_AddonDo>().uiObjects[79];
		}
		for (int i = 0; i < gameObject.transform.childCount; i++)
		{
			gameObject.transform.GetChild(i).gameObject.SetActive(value: false);
		}
		List<Transform> list = new List<Transform>();
		for (int j = 0; j < uiObjects[0].transform.childCount; j++)
		{
			list.Add(uiObjects[0].transform.GetChild(j));
		}
		for (int k = 0; k < list.Count; k++)
		{
			list[k].SetParent(gameObject.transform);
		}
		guiMain_.uiObjects[111].GetComponent<Menu_Dev_FanbriefeSelect>().BUTTON_Close();
		base.gameObject.SetActive(value: false);
	}
}
