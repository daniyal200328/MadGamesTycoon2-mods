using UnityEngine;
using UnityEngine.UI;

public class Menu_Firmenstandort : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private string searchStringA = "";

	private void Start()
	{
		FindScripts();
		Init();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if ((bool)main_)
		{
			if (!mS_)
			{
				mS_ = main_.GetComponent<mainScript>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!genres_)
			{
				genres_ = main_.GetComponent<genres>();
			}
			if (!sfx_)
			{
				sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
			}
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			}
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init()
	{
		FindScripts();
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			Transform child = uiObjects[0].transform.GetChild(i);
			child.gameObject.SetActive(value: true);
			Transform child2 = child.transform.GetChild(3);
			Transform child3 = child.transform.GetChild(2);
			child2.GetComponent<Text>().text = tS_.GetCountry(i);
			child3.GetComponent<Image>().sprite = guiMain_.flagSprites[i];
			child.name = tS_.GetCountry(i);
		}
		mS_.SortChildrenByName(uiObjects[0]);
		EnableDisable();
	}

	private void EnableDisable()
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			string text = uiObjects[0].transform.GetChild(i).name;
			searchStringA = searchStringA.ToLower();
			text = text.ToLower();
			if (uiObjects[6].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA))
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: true);
			}
			else
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Country(int i)
	{
		if (guiMain_.uiObjects[159].activeSelf)
		{
			guiMain_.uiObjects[159].GetComponent<Menu_NewGame>().SetCountry(i);
		}
		if (guiMain_.uiObjects[425].activeSelf)
		{
			guiMain_.uiObjects[425].GetComponent<Menu_NewGame_Sandbox>().SetCountry(i);
		}
		BUTTON_Abbrechen();
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[6].GetComponent<InputField>().text;
			EnableDisable();
		}
	}
}
