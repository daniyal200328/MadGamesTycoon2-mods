using UnityEngine;
using UnityEngine.UI;

public class Fill_Window : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

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
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void Init()
	{
		GetComponent<Image>().material = guiMain_.matFill_Window;
		Object.Destroy(this);
	}
}
