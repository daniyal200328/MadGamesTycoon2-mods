using UnityEngine;
using UnityEngine.UI;

public class Menu_Engine_ShowFeatures : MonoBehaviour
{
	public engineScript eS_;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineFeatures eF_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

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
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
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
				SetData();
			}
		}
	}

	public void Init(engineScript s)
	{
		eS_ = s;
		FindScripts();
		SetData();
	}

	private void SetData()
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		uiObjects[5].GetComponent<Text>().text = tS_.GetText(4) + " " + eS_.GetTechLevel();
		CreateItems(eF_.GetTypGrafik(), "<color=white>" + tS_.GetText(9) + "</color>");
		CreateItems(eF_.GetTypSound(), "<color=white>" + tS_.GetText(10) + "</color>");
		CreateItems(eF_.GetTypKI(), "<color=white>" + tS_.GetText(11) + "</color>");
		CreateItems(eF_.GetTypPhysik(), "<color=white>" + tS_.GetText(12) + "</color>");
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
	}

	private void CreateItems(int typ_, string title_)
	{
		guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>();
		Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).transform.GetChild(0).GetComponent<Text>().text = title_;
		Object.Instantiate(uiPrefabs[1], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
		for (int i = 0; i < eS_.features.Length; i++)
		{
			if (eS_.features[i] && eF_.engineFeatures_TYP[i] == typ_)
			{
				Item_EngineFeature component = Object.Instantiate(uiPrefabs[2], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_EngineFeature>();
				component.myID = i;
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.eF_ = eF_;
			}
		}
		if (uiObjects[0].transform.childCount % 2 != 0)
		{
			Object.Instantiate(uiPrefabs[1], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
