using UnityEngine;
using UnityEngine.UI;

public class Menu_DevGame_Size : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private Menu_DevGame mDevGame_;

	private unlockScript unlock_;

	private forschungSonstiges fS_;

	private float updateTimer;

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
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!fS_)
		{
			fS_ = main_.GetComponent<forschungSonstiges>();
		}
	}

	private void OnEnable()
	{
		Init();
	}

	private void Update()
	{
		MultiplayerUpdate();
	}

	private void MultiplayerUpdate()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 5f))
			{
				updateTimer = 0f;
				Init();
			}
		}
	}

	private void Init()
	{
		FindScripts();
		uiObjects[18].GetComponent<Text>().text = tS_.GetText(328) + " " + tS_.GetText(2069);
		fS_.Unlock(0, uiObjects[14], uiObjects[10]);
		fS_.Unlock(1, uiObjects[15], uiObjects[11]);
		fS_.Unlock(2, uiObjects[16], uiObjects[12]);
		fS_.Unlock(3, uiObjects[17], uiObjects[13]);
		fS_.Unlock(40, uiObjects[22], uiObjects[21]);
		uiObjects[0].GetComponent<Text>().text = mS_.GetMoney(mDevGame_.costs_gameSize[0], showDollar: true);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(mDevGame_.costs_gameSize[1] * (mS_.difficulty + 1), showDollar: true);
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(mDevGame_.costs_gameSize[2] * (mS_.difficulty + 1), showDollar: true);
		uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(mDevGame_.costs_gameSize[3] * (mS_.difficulty + 1), showDollar: true);
		uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(mDevGame_.costs_gameSize[4] * (mS_.difficulty + 1), showDollar: true);
		uiObjects[19].GetComponent<Text>().text = mS_.GetMoney(mDevGame_.costs_gameSize[5] * (mS_.difficulty + 1), showDollar: true);
		uiObjects[5].GetComponent<Text>().text = tS_.GetText(1722) + ": <color=blue>1 - " + mDevGame_.maxFeatures_gameSize[0] + "</color>";
		uiObjects[6].GetComponent<Text>().text = tS_.GetText(1722) + ": <color=blue>" + mDevGame_.maxFeatures_gameSize[0] / 2 + " - " + mDevGame_.maxFeatures_gameSize[1] + "</color>";
		uiObjects[7].GetComponent<Text>().text = tS_.GetText(1722) + ": <color=blue>" + mDevGame_.maxFeatures_gameSize[1] / 2 + " - " + mDevGame_.maxFeatures_gameSize[2] + "</color>";
		uiObjects[8].GetComponent<Text>().text = tS_.GetText(1722) + ": <color=blue>" + mDevGame_.maxFeatures_gameSize[2] / 2 + " - " + mDevGame_.maxFeatures_gameSize[3] + "</color>";
		uiObjects[9].GetComponent<Text>().text = tS_.GetText(1722) + ": <color=blue>" + mDevGame_.maxFeatures_gameSize[3] / 2 + " - " + mDevGame_.maxFeatures_gameSize[4] + "</color>";
		uiObjects[20].GetComponent<Text>().text = tS_.GetText(1722) + ": <color=blue>" + mDevGame_.maxFeatures_gameSize[4] / 2 + " - " + tS_.GetText(335) + "</color>";
	}

	public void BUTTON_GameSize(int i)
	{
		mDevGame_.SetGameSize(i);
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
