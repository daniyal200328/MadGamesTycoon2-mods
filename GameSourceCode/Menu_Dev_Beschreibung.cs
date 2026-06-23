using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_Beschreibung : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private Menu_DevGame mDevGame_;

	private Menu_Dev_AddonDo mDevAddon_;

	private Menu_Dev_MMOAddon mDevMMOAddon_;

	private Menu_Dev_GameEntwicklungsbericht mDevEntwicklungsbericht_;

	private void Start()
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
		if (!mDevGame_)
		{
			mDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
		if (!mDevAddon_)
		{
			mDevAddon_ = guiMain_.uiObjects[193].GetComponent<Menu_Dev_AddonDo>();
		}
		if (!mDevMMOAddon_)
		{
			mDevMMOAddon_ = guiMain_.uiObjects[247].GetComponent<Menu_Dev_MMOAddon>();
		}
		if (!mDevEntwicklungsbericht_)
		{
			mDevEntwicklungsbericht_ = guiMain_.uiObjects[73].GetComponent<Menu_Dev_GameEntwicklungsbericht>();
		}
	}

	private void OnEnable()
	{
		Init();
	}

	private void Init()
	{
		FindScripts();
		if (mDevGame_.gameObject.activeSelf)
		{
			uiObjects[0].GetComponent<InputField>().text = mDevGame_.g_Beschreibung;
		}
		if (mDevAddon_.gameObject.activeSelf)
		{
			uiObjects[0].GetComponent<InputField>().text = mDevAddon_.g_Beschreibung;
		}
		if (mDevMMOAddon_.gameObject.activeSelf)
		{
			uiObjects[0].GetComponent<InputField>().text = mDevMMOAddon_.g_Beschreibung;
		}
		if (mDevEntwicklungsbericht_.gameObject.activeSelf)
		{
			uiObjects[0].GetComponent<InputField>().text = mDevEntwicklungsbericht_.GetBeschreibung();
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		if (mDevGame_.gameObject.activeSelf)
		{
			mDevGame_.SetBeschreibung(uiObjects[0].GetComponent<InputField>().text);
		}
		if (mDevAddon_.gameObject.activeSelf)
		{
			mDevAddon_.SetBeschreibung(uiObjects[0].GetComponent<InputField>().text);
		}
		if (mDevMMOAddon_.gameObject.activeSelf)
		{
			mDevMMOAddon_.SetBeschreibung(uiObjects[0].GetComponent<InputField>().text);
		}
		if (mDevEntwicklungsbericht_.gameObject.activeSelf)
		{
			mDevEntwicklungsbericht_.SetBeschreibung(uiObjects[0].GetComponent<InputField>().text);
		}
		base.gameObject.SetActive(value: false);
	}
}
