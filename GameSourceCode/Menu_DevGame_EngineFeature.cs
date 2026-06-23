using UnityEngine;
using UnityEngine.UI;

public class Menu_DevGame_EngineFeature : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineFeatures eF_;

	private Menu_DevGame devGame_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private int featureArt;

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
		if (!devGame_)
		{
			devGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init(int featureArt_)
	{
		featureArt = featureArt_;
		FindScripts();
		if ((bool)devGame_.g_GameEngineScript_)
		{
			uiObjects[7].GetComponent<Text>().text = devGame_.g_GameEngineScript_.GetTechLevel().ToString();
		}
		uiObjects[8].GetComponent<Text>().text = devGame_.GetLowestPlatformTechLevel().ToString();
		switch (featureArt_)
		{
		case 0:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(9);
			break;
		case 1:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(10);
			break;
		case 2:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(11);
			break;
		case 3:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(12);
			break;
		}
		CreateItems(featureArt_);
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
	}

	private void CreateItems(int typ_)
	{
		if (!devGame_.g_GameEngineScript_)
		{
			return;
		}
		for (int i = 0; i < devGame_.g_GameEngineScript_.features.Length; i++)
		{
			if (devGame_.g_GameEngineScript_.features[i] && eF_.engineFeatures_TYP[i] == typ_)
			{
				Item_DevGame_EngineFeature component = Object.Instantiate(uiPrefabs[2], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_EngineFeature>();
				component.myID = i;
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.eF_ = eF_;
			}
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
