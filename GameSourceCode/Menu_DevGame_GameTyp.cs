using UnityEngine;
using UnityEngine.UI;

public class Menu_DevGame_GameTyp : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private Menu_DevGame mDevGame_;

	private unlockScript unlock_;

	private float updateTimer;

	private void Start()
	{
		FindScripts();
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
	}

	private void OnEnable()
	{
		FindScripts();
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
		Unlock(21, uiObjects[3], uiObjects[5]);
		Unlock(22, uiObjects[4], uiObjects[6]);
		uiObjects[0].GetComponent<Text>().text = mS_.GetMoney(mDevGame_.costs_gameTyp[0], showDollar: true);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(mDevGame_.costs_gameTyp[1] * (mS_.difficulty + 1), showDollar: true);
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(mDevGame_.costs_gameTyp[2] * (mS_.difficulty + 1), showDollar: true);
	}

	private void Unlock(int id_, GameObject lock_, GameObject button_)
	{
		if (unlock_.unlock[id_])
		{
			button_.GetComponent<Button>().interactable = true;
			lock_.SetActive(value: false);
		}
		else
		{
			button_.GetComponent<Button>().interactable = false;
			lock_.SetActive(value: true);
		}
	}

	public void BUTTON_GameTyp(int i)
	{
		mDevGame_.SetGameTyp(i);
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
