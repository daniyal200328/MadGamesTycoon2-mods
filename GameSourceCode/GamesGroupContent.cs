using UnityEngine;
using UnityEngine.UI;

public class GamesGroupContent : MonoBehaviour
{
	public GameObject[] uiTabs;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	public float timer;

	private int oldAmount;

	private bool noStandardGame = true;

	private bool noHandy = true;

	private bool noEigeneKonsole = true;

	private bool noArcade = true;

	private bool noSchublade = true;

	private bool noGamePass = true;

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

	private void OnEnable()
	{
		FindScripts();
	}

	private void Update()
	{
		if (!mS_)
		{
			FindScripts();
		}
		timer += Time.deltaTime;
		if (base.gameObject.transform.childCount != oldAmount)
		{
			timer = 10f;
			oldAmount = base.gameObject.transform.childCount;
		}
		if (timer < 1f)
		{
			return;
		}
		timer = 0f;
		noStandardGame = true;
		noHandy = true;
		noArcade = true;
		noEigeneKonsole = true;
		noSchublade = true;
		noGamePass = true;
		for (int i = 0; i < base.gameObject.transform.childCount; i++)
		{
			GameObject gameObject = base.gameObject.transform.GetChild(i).gameObject;
			gameTab component = gameObject.GetComponent<gameTab>();
			if ((bool)component && (bool)component.gS_)
			{
				if (component.gS_.schublade)
				{
					noSchublade = false;
				}
				if (component.gS_.arcade && !component.gS_.schublade)
				{
					noArcade = false;
				}
				if (component.gS_.handy && !component.gS_.schublade)
				{
					noHandy = false;
				}
				if (!component.gS_.arcade && !component.gS_.handy && !component.gS_.schublade)
				{
					noStandardGame = false;
				}
				SetTab(gameObject, component);
				continue;
			}
			konsoleTab component2 = gameObject.GetComponent<konsoleTab>();
			if ((bool)component2 && (bool)component2.pS_)
			{
				noEigeneKonsole = false;
				SetTabKonsole(gameObject, component2);
				continue;
			}
			gamePassTab component3 = gameObject.GetComponent<gamePassTab>();
			if ((bool)component3)
			{
				noGamePass = false;
				SetTabGamePass(gameObject, component3);
			}
		}
		if (noStandardGame)
		{
			if (uiTabs[1].activeSelf)
			{
				uiTabs[1].SetActive(value: false);
			}
		}
		else if (!uiTabs[1].activeSelf)
		{
			uiTabs[1].SetActive(value: true);
		}
		if (noArcade)
		{
			if (uiTabs[2].activeSelf)
			{
				uiTabs[2].SetActive(value: false);
			}
		}
		else if (!uiTabs[2].activeSelf)
		{
			uiTabs[2].SetActive(value: true);
		}
		if (noHandy)
		{
			if (uiTabs[3].activeSelf)
			{
				uiTabs[3].SetActive(value: false);
			}
		}
		else if (!uiTabs[3].activeSelf)
		{
			uiTabs[3].SetActive(value: true);
		}
		if (noEigeneKonsole)
		{
			if (uiTabs[4].activeSelf)
			{
				uiTabs[4].SetActive(value: false);
			}
		}
		else if (!uiTabs[4].activeSelf)
		{
			uiTabs[4].SetActive(value: true);
		}
		if (noSchublade)
		{
			if (uiTabs[5].activeSelf)
			{
				uiTabs[5].SetActive(value: false);
			}
		}
		else if (!uiTabs[5].activeSelf)
		{
			uiTabs[5].SetActive(value: true);
		}
		if (noStandardGame && noArcade && noHandy && noEigeneKonsole && noSchublade)
		{
			if (uiTabs[0].activeSelf)
			{
				uiTabs[0].SetActive(value: false);
			}
		}
		else if (!uiTabs[0].activeSelf)
		{
			uiTabs[0].SetActive(value: true);
		}
		if (noGamePass)
		{
			if (uiTabs[6].activeSelf)
			{
				uiTabs[6].SetActive(value: false);
			}
		}
		else if (!uiTabs[6].activeSelf)
		{
			uiTabs[6].SetActive(value: true);
		}
		if (uiTabs[1].GetComponent<Toggle>().isOn && !uiTabs[1].activeSelf)
		{
			uiTabs[1].GetComponent<Toggle>().isOn = false;
			uiTabs[0].GetComponent<Toggle>().isOn = true;
		}
		if (uiTabs[2].GetComponent<Toggle>().isOn && !uiTabs[2].activeSelf)
		{
			uiTabs[2].GetComponent<Toggle>().isOn = false;
			uiTabs[0].GetComponent<Toggle>().isOn = true;
		}
		if (uiTabs[3].GetComponent<Toggle>().isOn && !uiTabs[3].activeSelf)
		{
			uiTabs[3].GetComponent<Toggle>().isOn = false;
			uiTabs[0].GetComponent<Toggle>().isOn = true;
		}
		if (uiTabs[4].GetComponent<Toggle>().isOn && !uiTabs[4].activeSelf)
		{
			uiTabs[4].GetComponent<Toggle>().isOn = false;
			uiTabs[0].GetComponent<Toggle>().isOn = true;
		}
		if (uiTabs[5].GetComponent<Toggle>().isOn && !uiTabs[5].activeSelf)
		{
			uiTabs[5].GetComponent<Toggle>().isOn = false;
			uiTabs[0].GetComponent<Toggle>().isOn = true;
		}
		if (uiTabs[6].GetComponent<Toggle>().isOn && !uiTabs[6].activeSelf)
		{
			uiTabs[6].GetComponent<Toggle>().isOn = false;
			uiTabs[0].GetComponent<Toggle>().isOn = true;
		}
	}

	private void SetTabGamePass(GameObject go, gamePassTab script_)
	{
		if (uiTabs[0].GetComponent<Toggle>().isOn || uiTabs[6].GetComponent<Toggle>().isOn)
		{
			if (!go.activeSelf)
			{
				go.SetActive(value: true);
			}
		}
		else if (go.activeSelf)
		{
			go.SetActive(value: false);
		}
	}

	private void SetTabKonsole(GameObject go, konsoleTab script_)
	{
		if (uiTabs[0].GetComponent<Toggle>().isOn || uiTabs[4].GetComponent<Toggle>().isOn)
		{
			if (!go.activeSelf)
			{
				go.SetActive(value: true);
			}
		}
		else if (go.activeSelf)
		{
			go.SetActive(value: false);
		}
	}

	private void SetTab(GameObject go, gameTab script_)
	{
		if (!uiTabs[0].GetComponent<Toggle>().isOn)
		{
			if (!script_.gS_.arcade && !script_.gS_.handy && !script_.gS_.schublade && !uiTabs[1].GetComponent<Toggle>().isOn)
			{
				if (go.activeSelf)
				{
					go.SetActive(value: false);
				}
				return;
			}
			if (script_.gS_.arcade && !script_.gS_.schublade && !uiTabs[2].GetComponent<Toggle>().isOn)
			{
				if (go.activeSelf)
				{
					go.SetActive(value: false);
				}
				return;
			}
			if (script_.gS_.handy && !script_.gS_.schublade && !uiTabs[3].GetComponent<Toggle>().isOn)
			{
				if (go.activeSelf)
				{
					go.SetActive(value: false);
				}
				return;
			}
			if (script_.gS_.schublade && !uiTabs[5].GetComponent<Toggle>().isOn)
			{
				if (go.activeSelf)
				{
					go.SetActive(value: false);
				}
				return;
			}
		}
		bool flag = false;
		if (script_.gS_.developerID != mS_.myID && mS_.gameTabFilter[11])
		{
			flag = true;
		}
		if (script_.gS_.gameTyp == 1 && !mS_.gameTabFilter[12])
		{
			flag = true;
		}
		if (script_.gS_.gameTyp == 2 && !mS_.gameTabFilter[13])
		{
			flag = true;
		}
		if (script_.gS_.gameTyp == 0)
		{
			if (script_.gS_.retro && !mS_.gameTabFilter[14])
			{
				flag = true;
			}
			if (script_.gS_.typ_bundle && !mS_.gameTabFilter[9])
			{
				flag = true;
			}
			if (script_.gS_.typ_bundleAddon && !mS_.gameTabFilter[10])
			{
				flag = true;
			}
			if (script_.gS_.typ_remaster && !mS_.gameTabFilter[2])
			{
				flag = true;
			}
			if (script_.gS_.typ_spinoff && !mS_.gameTabFilter[3])
			{
				flag = true;
			}
			if (script_.gS_.typ_addon && !mS_.gameTabFilter[4])
			{
				flag = true;
			}
			if (script_.gS_.typ_addonStandalone && !mS_.gameTabFilter[5])
			{
				flag = true;
			}
			if (script_.gS_.typ_mmoaddon && !mS_.gameTabFilter[6])
			{
				flag = true;
			}
			if (script_.gS_.typ_budget && !mS_.gameTabFilter[7])
			{
				flag = true;
			}
			if (script_.gS_.typ_goty && !mS_.gameTabFilter[8])
			{
				flag = true;
			}
			if (script_.gS_.typ_nachfolger && !mS_.gameTabFilter[1])
			{
				flag = true;
			}
			if (script_.gS_.typ_standard && !mS_.gameTabFilter[0])
			{
				flag = true;
			}
		}
		if (go.activeSelf && flag)
		{
			go.SetActive(value: false);
		}
		else if (!go.activeSelf && !flag)
		{
			go.SetActive(value: true);
		}
	}
}
