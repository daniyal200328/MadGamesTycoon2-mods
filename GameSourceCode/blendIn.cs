using UnityEngine;
using UnityEngine.UI;

public class blendIn : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

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

	private void OnEnable()
	{
		uiObjects[0].GetComponent<Image>().fillAmount = 1f;
	}

	private void Update()
	{
		if (uiObjects[0].GetComponent<Image>().fillAmount > 0f)
		{
			uiObjects[0].GetComponent<Image>().fillAmount -= Time.deltaTime * 2f;
			if (uiObjects[0].GetComponent<Image>().fillAmount <= 0f)
			{
				uiObjects[0].GetComponent<Image>().fillAmount = 1f;
				base.gameObject.SetActive(value: false);
			}
		}
	}
}
