using UnityEngine;
using UnityEngine.UI;

public class AmountContracts : MonoBehaviour
{
	public int contractTyp;

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
		base.gameObject.GetComponent<Text>().text = "[" + mS_.GetAmountContracts(contractTyp) + "] ";
	}

	private void Update()
	{
		Init();
	}
}
