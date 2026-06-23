using UnityEngine;
using UnityEngine.UI;

public class Menu_MesseSelect : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	public int standGroesse;

	public gameScript[] games;

	public platformScript[] konsolen;

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
	}

	public void Init(int standgroesse)
	{
		standGroesse = standgroesse;
		for (int i = 0; i < games.Length; i++)
		{
			games[i] = null;
		}
		FindScripts();
		for (int j = 0; j < games.Length; j++)
		{
			SetGame(j, null);
		}
		for (int k = 0; k < konsolen.Length; k++)
		{
			SetKonsole(k, null);
		}
		switch (standgroesse)
		{
		case 0:
			uiObjects[5].GetComponent<Button>().interactable = false;
			uiObjects[6].GetComponent<Button>().interactable = false;
			uiObjects[7].GetComponent<Button>().interactable = false;
			uiObjects[8].GetComponent<Button>().interactable = false;
			break;
		case 1:
			uiObjects[5].GetComponent<Button>().interactable = true;
			uiObjects[6].GetComponent<Button>().interactable = false;
			uiObjects[7].GetComponent<Button>().interactable = true;
			uiObjects[8].GetComponent<Button>().interactable = false;
			break;
		case 2:
			uiObjects[5].GetComponent<Button>().interactable = true;
			uiObjects[6].GetComponent<Button>().interactable = true;
			uiObjects[7].GetComponent<Button>().interactable = true;
			uiObjects[8].GetComponent<Button>().interactable = true;
			break;
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_SelectGame(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[187]);
		guiMain_.uiObjects[187].GetComponent<Menu_MesseSelectGame>().Init(i);
	}

	public void BUTTON_SelectKonsole(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[323]);
		guiMain_.uiObjects[323].GetComponent<Menu_MesseSelectKonsole>().Init(i);
	}

	public void SetGame(int slot_, gameScript game_)
	{
		games[slot_] = game_;
		if ((bool)games[0])
		{
			uiObjects[0].GetComponent<Text>().text = "<b>" + games[0].GetNameWithTag() + "</b>";
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(948);
		}
		if ((bool)games[1])
		{
			uiObjects[1].GetComponent<Text>().text = "<b>" + games[1].GetNameWithTag() + "</b>";
		}
		else
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(948);
		}
		if ((bool)games[2])
		{
			uiObjects[2].GetComponent<Text>().text = "<b>" + games[2].GetNameWithTag() + "</b>";
		}
		else
		{
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(948);
		}
	}

	public void SetKonsole(int slot_, platformScript script_)
	{
		konsolen[slot_] = script_;
		if ((bool)konsolen[0])
		{
			uiObjects[3].GetComponent<Text>().text = "<b>" + konsolen[0].GetName() + "</b>";
		}
		else
		{
			uiObjects[3].GetComponent<Text>().text = tS_.GetText(949);
		}
		if ((bool)konsolen[1])
		{
			uiObjects[9].GetComponent<Text>().text = "<b>" + konsolen[1].GetName() + "</b>";
		}
		else
		{
			uiObjects[9].GetComponent<Text>().text = tS_.GetText(949);
		}
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		if (games[0] == null && games[1] == null && games[2] == null && konsolen[0] == null && konsolen[1] == null)
		{
			guiMain_.MessageBox(tS_.GetText(951), closeMenu: false);
			return;
		}
		Menu_Messe component = guiMain_.uiObjects[185].GetComponent<Menu_Messe>();
		if ((bool)component)
		{
			mS_.Pay(component.GetPrice(standGroesse), 17);
		}
		if ((bool)component)
		{
			int num = component.GetPrice(standGroesse);
			int num2 = 0;
			for (int i = 0; i < games.Length; i++)
			{
				if ((bool)games[i])
				{
					num2++;
				}
			}
			if ((bool)konsolen[0])
			{
				num2++;
			}
			if (num2 > 0)
			{
				num /= num2;
			}
			for (int j = 0; j < games.Length; j++)
			{
				if ((bool)games[j])
				{
					games[j].GetComponent<gameScript>().costs_marketing += num;
				}
			}
			if ((bool)konsolen[0])
			{
				konsolen[0].GetComponent<platformScript>().costs_marketing += num;
			}
		}
		guiMain_.uiObjects[185].SetActive(value: false);
		guiMain_.uiObjects[188].SetActive(value: true);
		guiMain_.uiObjects[188].GetComponent<Menu_MesseErgebnis>().Init();
		base.gameObject.SetActive(value: false);
	}
}
