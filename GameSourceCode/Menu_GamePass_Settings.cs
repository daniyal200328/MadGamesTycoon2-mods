using UnityEngine;
using UnityEngine.UI;

public class Menu_GamePass_Settings : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

	private forschungSonstiges forschungSonstiges_;

	private gamepassScript gpS_;

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
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
		}
	}

	public void OnEnable()
	{
		FindScripts();
		uiObjects[0].GetComponent<Toggle>().isOn = gpS_.gamePass_aktiv;
		uiObjects[1].GetComponent<InputField>().text = gpS_.gamePass_name;
		uiObjects[2].GetComponent<Slider>().value = gpS_.gamePass_AboPreis;
		UpdateData();
	}

	public void UpdateData()
	{
		FindScripts();
		int num = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Ok()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		gpS_.gamePass_aktiv = uiObjects[0].GetComponent<Toggle>().isOn;
		gpS_.gamePass_name = uiObjects[1].GetComponent<InputField>().text;
		gpS_.gamePass_AboPreis = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		if (gpS_.gamePass_name.Length <= 0)
		{
			gpS_.gamePass_name = tS_.GetText(1243);
		}
	}

	public void BUTTON_Preis_Plus()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[2].GetComponent<Slider>().value++;
	}

	public void BUTTON_Preis_Minus()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[2].GetComponent<Slider>().value--;
	}

	public void SLIDER_AboPreis()
	{
		UpdateData();
	}
}
