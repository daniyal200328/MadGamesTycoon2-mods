using UnityEngine;
using UnityEngine.UI;

public class Menu_DemolishRoom : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	public int money;

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
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
	}

	public void Init(roomScript script_)
	{
		FindScripts();
		cmS_.disableMovement = true;
		money = 0;
		if (!script_)
		{
			return;
		}
		rS_ = script_;
		if (rS_.taskID == -1)
		{
			for (int i = 0; i < rS_.listInventar.Count; i++)
			{
				if ((bool)rS_.listInventar[i])
				{
					money += rS_.listInventar[i].GetComponent<objectScript>().GetVerkaufspreis();
				}
			}
			string text = tS_.GetText(153);
			text = text.Replace("<NUM>", mS_.GetMoney(money, showDollar: true));
			uiObjects[0].GetComponent<Text>().text = text;
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(638);
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
		cmS_.disableMovement = false;
	}

	public void BUTTON_Yes()
	{
		if ((bool)rS_ && rS_.taskID == -1)
		{
			mS_.Earn(money, 0);
			rS_.Demolish();
			sfx_.PlaySound(25, force: true);
			guiMain_.ShowWalls(guiMain_.uiObjects[241].GetComponent<Toggle>().isOn);
		}
		BUTTON_Abbrechen();
	}
}
