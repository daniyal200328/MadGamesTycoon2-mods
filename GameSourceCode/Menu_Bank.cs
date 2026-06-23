using UnityEngine;
using UnityEngine.UI;

public class Menu_Bank : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private float updateTimer;

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

	public void Init()
	{
		FindScripts();
		uiObjects[0].GetComponent<Text>().text = mS_.GetMoney(mS_.GetKreditlimit(), showDollar: true);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(mS_.GetKredit(), showDollar: true);
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(mS_.GetKreditZinsen(), showDollar: true);
		uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(25000L, showDollar: true);
		uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(25000L, showDollar: true);
		uiObjects[5].GetComponent<Text>().text = mS_.GetMoney(50000L, showDollar: true);
		uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(50000L, showDollar: true);
		uiObjects[7].GetComponent<Text>().text = mS_.GetMoney(100000L, showDollar: true);
		uiObjects[8].GetComponent<Text>().text = mS_.GetMoney(100000L, showDollar: true);
		uiObjects[9].GetComponent<Text>().text = mS_.GetMoney(250000L, showDollar: true);
		uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(250000L, showDollar: true);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_KreditAufnehmen(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (i == 0)
		{
			long num = mS_.GetKreditlimit() - mS_.kredit;
			mS_.kredit += num;
			mS_.money += num;
			Init();
		}
		else if (mS_.kredit + i <= mS_.GetKreditlimit())
		{
			mS_.kredit += i;
			mS_.money += i;
			Init();
		}
		else
		{
			long num2 = mS_.GetKreditlimit() - mS_.kredit;
			mS_.kredit += num2;
			mS_.money += num2;
			Init();
		}
	}

	public void BUTTON_KreditAbzahlen(int i)
	{
		if (i == 0)
		{
			do
			{
				BUTTON_KreditAbzahlen(25000);
			}
			while (mS_.money >= 25000 && mS_.kredit > 0);
			return;
		}
		sfx_.PlaySound(3, force: true);
		if (mS_.money >= i)
		{
			mS_.kredit -= i;
			mS_.money -= i;
			if (mS_.kredit < 0)
			{
				mS_.money += mS_.kredit * -1;
				mS_.kredit = 0L;
			}
			Init();
		}
	}
}
