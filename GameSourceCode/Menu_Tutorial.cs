using UnityEngine;
using UnityEngine.UI;

public class Menu_Tutorial : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] arrows;

	public bool[] showNextButton;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	public int step;

	private void Start()
	{
		FindScripts();
		BUTTON_Next(0);
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

	public void BUTTON_Next(int i)
	{
		sfx_.PlaySound(3, force: true);
		SetStep(step + i);
	}

	public void BUTTON_StopTutorial()
	{
		sfx_.PlaySound(3, force: true);
		mS_.settings_TutorialOff = true;
	}

	public void SetStep(int s)
	{
		base.gameObject.GetComponent<Animation>().Play();
		step = s;
		if (step < 0)
		{
			step = 0;
		}
		if (step >= tS_.tutorial_GE.Length)
		{
			mS_.settings_TutorialOff = true;
			base.gameObject.SetActive(value: false);
			DisableAllArrows();
			return;
		}
		if (showNextButton[step])
		{
			uiObjects[2].SetActive(value: true);
		}
		else
		{
			uiObjects[2].SetActive(value: false);
		}
		uiObjects[0].GetComponent<Text>().text = tS_.GetTutorial(step);
		for (int i = 0; i < arrows.Length; i++)
		{
			if ((bool)arrows[i])
			{
				if (arrows[i].activeSelf)
				{
					arrows[i].SetActive(value: false);
				}
				if (i == step && !arrows[i].activeSelf)
				{
					arrows[i].SetActive(value: true);
				}
			}
		}
	}

	private void DisableAllArrows()
	{
		for (int i = 0; i < arrows.Length; i++)
		{
			if ((bool)arrows[i] && arrows[i].activeSelf)
			{
				arrows[i].SetActive(value: false);
			}
		}
	}
}
