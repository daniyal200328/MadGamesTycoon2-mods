using UnityEngine;
using UnityEngine.UI;

public class Menu_TooltipCharacter : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private characterScript charS_;

	private mapScript mapS_;

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
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
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

	private void Update()
	{
		if (!charS_)
		{
			base.gameObject.SetActive(value: false);
		}
	}

	public void Init(characterScript script_)
	{
		charS_ = script_;
		if ((bool)charS_)
		{
			FindScripts();
			UpdateData();
		}
	}

	public void UpdateData()
	{
		uiObjects[0].GetComponent<Text>().text = charS_.myName;
		uiObjects[16].GetComponent<Text>().text = tS_.GetText(137 + charS_.beruf);
		uiObjects[9].GetComponent<Text>().text = mS_.GetMoney(charS_.GetGehalt(), showDollar: true);
		guiMain_.SetBalkenEmployee(uiObjects[1], charS_.s_gamedesign, 0, charS_);
		guiMain_.SetBalkenEmployee(uiObjects[2], charS_.s_programmieren, 1, charS_);
		guiMain_.SetBalkenEmployee(uiObjects[3], charS_.s_grafik, 2, charS_);
		guiMain_.SetBalkenEmployee(uiObjects[4], charS_.s_sound, 3, charS_);
		guiMain_.SetBalkenEmployee(uiObjects[5], charS_.s_pr, 4, charS_);
		guiMain_.SetBalkenEmployee(uiObjects[6], charS_.s_gametests, 5, charS_);
		guiMain_.SetBalkenEmployee(uiObjects[7], charS_.s_technik, 6, charS_);
		guiMain_.SetBalkenEmployee(uiObjects[8], charS_.s_forschen, 7, charS_);
		guiMain_.CreatePerkIcons(charS_, uiObjects[10].transform);
		uiObjects[12].GetComponent<Image>().fillAmount = charS_.s_motivation * 0.01f;
		uiObjects[13].GetComponent<Text>().text = mS_.Round(charS_.s_motivation, 0).ToString();
		uiObjects[12].GetComponent<Image>().color = GetValColor(charS_.s_motivation);
		uiObjects[19].SetActive(value: false);
		uiObjects[20].SetActive(value: false);
		uiObjects[21].SetActive(value: false);
		uiObjects[22].SetActive(value: false);
		uiObjects[23].SetActive(value: false);
		if (charS_.krank > 0)
		{
			uiObjects[19].SetActive(value: true);
		}
		int num = Mathf.RoundToInt(charS_.transform.position.x);
		int num2 = Mathf.RoundToInt(charS_.transform.position.z);
		if (!mapS_.IsInMapLimit(num, num2))
		{
			return;
		}
		if (!charS_.perks[16])
		{
			if (mapS_.mapMuell[num, num2] > 0f)
			{
				uiObjects[22].SetActive(value: true);
			}
			else
			{
				uiObjects[22].SetActive(value: false);
			}
		}
		if (mapS_.mapRoomID[num, num2] == 0)
		{
			return;
		}
		if (!charS_.perks[11])
		{
			if (mapS_.mapWaerme[num, num2] <= 0.2f)
			{
				uiObjects[21].SetActive(value: true);
			}
			else
			{
				uiObjects[21].SetActive(value: false);
			}
		}
		if (!charS_.perks[12])
		{
			if (mapS_.mapAusstattung[num, num2] <= 0.2f)
			{
				uiObjects[23].SetActive(value: true);
			}
			else
			{
				uiObjects[23].SetActive(value: false);
			}
		}
		if (!charS_.perks[17])
		{
			if (charS_.IsUeberfuellt())
			{
				uiObjects[20].SetActive(value: true);
			}
			else
			{
				uiObjects[20].SetActive(value: false);
			}
		}
	}

	private Color GetValColor(float val)
	{
		if (val < 30f)
		{
			return guiMain_.colorsBalken[0];
		}
		if (val >= 30f && val < 70f)
		{
			return guiMain_.colorsBalken[1];
		}
		if (val >= 70f)
		{
			return guiMain_.colorsBalken[2];
		}
		return guiMain_.colorsBalken[0];
	}
}
