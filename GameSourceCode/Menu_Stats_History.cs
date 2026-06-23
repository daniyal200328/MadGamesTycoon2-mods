using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_History : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineFeatures eF_;

	private genres genres_;

	public int seite;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private int numEintraege;

	private string searchStringA = "";

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
	}

	private void OnEnable()
	{
		Init();
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
		uiObjects[4].GetComponent<Text>().text = seite + 1 + " / " + (numEintraege / 100 + 1);
	}

	public void Init()
	{
		FindScripts();
		numEintraege = 0;
		uiObjects[1].GetComponent<Text>().text = "";
		for (int i = 0; i < mS_.history.Count; i++)
		{
			string text = mS_.history[i];
			searchStringA = searchStringA.ToLower();
			text = text.ToLower();
			if (uiObjects[6].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA))
			{
				numEintraege++;
				if (numEintraege >= seite * 100 && numEintraege < seite * 100 + 100)
				{
					Text component = uiObjects[1].GetComponent<Text>();
					component.text = component.text + mS_.history[i] + "\n\n";
				}
			}
		}
		if (uiObjects[1].GetComponent<Text>().text.Length <= 0)
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(303);
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Seite(int i)
	{
		sfx_.PlaySound(3, force: true);
		seite += i;
		if (seite < 0)
		{
			seite = 0;
		}
		if (seite > numEintraege / 100)
		{
			seite = numEintraege / 100;
		}
		Init();
	}

	public void BUTTON_ErsteSeite(int i)
	{
		sfx_.PlaySound(3, force: true);
		seite = 0;
		if (seite < 0)
		{
			seite = 0;
		}
		if (seite > numEintraege / 100)
		{
			seite = numEintraege / 100;
		}
		Init();
	}

	public void BUTTON_LetzteSeite(int i)
	{
		sfx_.PlaySound(3, force: true);
		seite = 99999999;
		if (seite < 0)
		{
			seite = 0;
		}
		if (seite > numEintraege / 100)
		{
			seite = numEintraege / 100;
		}
		Init();
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			searchStringA = uiObjects[6].GetComponent<InputField>().text;
			seite = 0;
			Init();
		}
	}
}
