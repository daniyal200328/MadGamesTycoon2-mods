using UnityEngine;
using UnityEngine.UI;

public class OptionenForschung : MonoBehaviour
{
	public int forschungTyp;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private void Start()
	{
		FindScripts();
		Init();
	}

	private void OnEnable()
	{
		FindScripts();
		Init();
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
	}

	private void Init()
	{
		if (forschungTyp != 99)
		{
			base.gameObject.GetComponent<Text>().text = "[" + guiMain_.uiObjects[21].GetComponent<Menu_Forschung>().GetAmountForschung(forschungTyp, getUnerforschtesObjekt: false) + "] ";
			return;
		}
		Menu_Forschung component = guiMain_.uiObjects[21].GetComponent<Menu_Forschung>();
		int num = 0;
		num += component.GetAmountForschung(0, getUnerforschtesObjekt: false);
		num += component.GetAmountForschung(1, getUnerforschtesObjekt: false);
		num += component.GetAmountForschung(2, getUnerforschtesObjekt: false);
		num += component.GetAmountForschung(3, getUnerforschtesObjekt: false);
		num += component.GetAmountForschung(4, getUnerforschtesObjekt: false);
		num += component.GetAmountForschung(5, getUnerforschtesObjekt: false);
		num += component.GetAmountForschung(6, getUnerforschtesObjekt: false);
		base.gameObject.GetComponent<Text>().text = "[" + num + "] ";
	}
}
