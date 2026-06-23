using UnityEngine;
using UnityEngine.UI;

public class Component_Aufwertungen : MonoBehaviour
{
	public GameObject[] uiObjects;

	public Sprite[] uiSprites;

	public Color[] colors;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private licences licences_;

	private engineFeatures eF_;

	private cameraMovementScript cmS_;

	private unlockScript unlock_;

	private gameplayFeatures gF_;

	private games games_;

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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
	}

	public void Init(gameScript gS_)
	{
		if (!gS_)
		{
			return;
		}
		for (int i = 0; i < gS_.gameplayStudio.Length; i++)
		{
			if (gS_.gameplayStudio[i])
			{
				uiObjects[i].GetComponent<Image>().color = colors[0];
			}
			else
			{
				uiObjects[i].GetComponent<Image>().color = colors[1];
			}
		}
		for (int j = 0; j < gS_.grafikStudio.Length; j++)
		{
			if (gS_.grafikStudio[j])
			{
				uiObjects[12 + j].GetComponent<Image>().color = colors[0];
			}
			else
			{
				uiObjects[12 + j].GetComponent<Image>().color = colors[1];
			}
		}
		for (int k = 0; k < gS_.soundStudio.Length; k++)
		{
			if (gS_.soundStudio[k])
			{
				uiObjects[6 + k].GetComponent<Image>().color = colors[0];
			}
			else
			{
				uiObjects[6 + k].GetComponent<Image>().color = colors[1];
			}
		}
		if (!gS_.motionCaptureStudio[0])
		{
			uiObjects[18].GetComponent<Image>().sprite = uiSprites[18];
			uiObjects[18].GetComponent<Image>().color = colors[1];
		}
		if (gS_.motionCaptureStudio[0])
		{
			uiObjects[18].GetComponent<Image>().sprite = uiSprites[18];
			uiObjects[18].GetComponent<Image>().color = colors[0];
		}
		if (gS_.motionCaptureStudio[1])
		{
			uiObjects[18].GetComponent<Image>().sprite = uiSprites[19];
			uiObjects[18].GetComponent<Image>().color = colors[0];
		}
		if (gS_.motionCaptureStudio[2])
		{
			uiObjects[18].GetComponent<Image>().sprite = uiSprites[20];
			uiObjects[18].GetComponent<Image>().color = colors[0];
		}
		if (!gS_.motionCaptureStudio[3])
		{
			uiObjects[19].GetComponent<Image>().sprite = uiSprites[21];
			uiObjects[19].GetComponent<Image>().color = colors[1];
		}
		if (gS_.motionCaptureStudio[3])
		{
			uiObjects[19].GetComponent<Image>().sprite = uiSprites[21];
			uiObjects[19].GetComponent<Image>().color = colors[0];
		}
		if (gS_.motionCaptureStudio[4])
		{
			uiObjects[19].GetComponent<Image>().sprite = uiSprites[22];
			uiObjects[19].GetComponent<Image>().color = colors[0];
		}
		if (gS_.motionCaptureStudio[5])
		{
			uiObjects[19].GetComponent<Image>().sprite = uiSprites[23];
			uiObjects[19].GetComponent<Image>().color = colors[0];
		}
	}

	public void InitNpcGame(gameScript gS_, float fortschritt)
	{
		if (!gS_)
		{
			return;
		}
		for (int i = 0; i < gS_.gameplayStudio.Length; i++)
		{
			if (gS_.gameplayStudio[i] && Mathf.RoundToInt(fortschritt * 10f) > i)
			{
				uiObjects[i].GetComponent<Image>().color = colors[0];
			}
			else
			{
				uiObjects[i].GetComponent<Image>().color = colors[1];
			}
		}
		for (int j = 0; j < gS_.grafikStudio.Length; j++)
		{
			if (gS_.grafikStudio[j] && Mathf.RoundToInt(fortschritt * 8f) > j)
			{
				uiObjects[12 + j].GetComponent<Image>().color = colors[0];
			}
			else
			{
				uiObjects[12 + j].GetComponent<Image>().color = colors[1];
			}
		}
		for (int k = 0; k < gS_.soundStudio.Length; k++)
		{
			if (gS_.soundStudio[k] && Mathf.RoundToInt(fortschritt * 7f) > k)
			{
				uiObjects[6 + k].GetComponent<Image>().color = colors[0];
			}
			else
			{
				uiObjects[6 + k].GetComponent<Image>().color = colors[1];
			}
		}
		uiObjects[18].GetComponent<Image>().sprite = uiSprites[18];
		uiObjects[18].GetComponent<Image>().color = colors[1];
		if (fortschritt > 0.2f && gS_.motionCaptureStudio[0])
		{
			uiObjects[18].GetComponent<Image>().sprite = uiSprites[18];
			uiObjects[18].GetComponent<Image>().color = colors[0];
		}
		if (fortschritt > 0.5f && gS_.motionCaptureStudio[1])
		{
			uiObjects[18].GetComponent<Image>().sprite = uiSprites[19];
			uiObjects[18].GetComponent<Image>().color = colors[0];
		}
		if (fortschritt > 0.7f && gS_.motionCaptureStudio[2])
		{
			uiObjects[18].GetComponent<Image>().sprite = uiSprites[20];
			uiObjects[18].GetComponent<Image>().color = colors[0];
		}
		uiObjects[19].GetComponent<Image>().sprite = uiSprites[21];
		uiObjects[19].GetComponent<Image>().color = colors[1];
		if (fortschritt > 0.3f && gS_.motionCaptureStudio[3])
		{
			uiObjects[19].GetComponent<Image>().sprite = uiSprites[21];
			uiObjects[19].GetComponent<Image>().color = colors[0];
		}
		if (fortschritt > 0.6f && gS_.motionCaptureStudio[4])
		{
			uiObjects[19].GetComponent<Image>().sprite = uiSprites[22];
			uiObjects[19].GetComponent<Image>().color = colors[0];
		}
		if (fortschritt > 0.8f && gS_.motionCaptureStudio[5])
		{
			uiObjects[19].GetComponent<Image>().sprite = uiSprites[23];
			uiObjects[19].GetComponent<Image>().color = colors[0];
		}
	}
}
