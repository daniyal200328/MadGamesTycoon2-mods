using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Statistics_AllTimeCharts : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private games games_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private int TAB;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			GameObject gameObject = parent_.transform.GetChild(i).gameObject;
			if (gameObject.activeSelf)
			{
				Item_AllTimeCharts component = gameObject.GetComponent<Item_AllTimeCharts>();
				if (component.game_.myID == id_)
				{
					gameObject.name = component.game_.sellsTotal.ToString();
					return true;
				}
			}
		}
		return false;
	}

	private void OnEnable()
	{
		Init();
	}

	public void Init()
	{
		FindScripts();
		TAB_Select(0);
	}

	private void SetData()
	{
		List<gameScript> list = new List<gameScript>();
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].sellsTotal > 0 && ((TAB == 0 && games_.arrayGamesScripts[i].gameTyp != 2 && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_addon && !games_.arrayGamesScripts[i].typ_addonStandalone && !games_.arrayGamesScripts[i].typ_mmoaddon) || (TAB == 1 && games_.arrayGamesScripts[i].gameTyp != 2 && games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_addon && !games_.arrayGamesScripts[i].typ_addonStandalone && !games_.arrayGamesScripts[i].typ_mmoaddon) || (TAB == 2 && games_.arrayGamesScripts[i].gameTyp != 2 && games_.arrayGamesScripts[i].arcade && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_addon && !games_.arrayGamesScripts[i].typ_addonStandalone && !games_.arrayGamesScripts[i].typ_mmoaddon) || (TAB == 3 && games_.arrayGamesScripts[i].gameTyp == 2 && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_addon && !games_.arrayGamesScripts[i].typ_addonStandalone && !games_.arrayGamesScripts[i].typ_mmoaddon) || (TAB == 4 && games_.arrayGamesScripts[i].gameTyp != 2 && games_.arrayGamesScripts[i].typ_budget) || (TAB == 5 && games_.arrayGamesScripts[i].gameTyp != 2 && (games_.arrayGamesScripts[i].typ_bundle || games_.arrayGamesScripts[i].typ_bundleAddon)) || (TAB == 6 && games_.arrayGamesScripts[i].gameTyp != 2 && (games_.arrayGamesScripts[i].typ_addon || games_.arrayGamesScripts[i].typ_addonStandalone || games_.arrayGamesScripts[i].typ_mmoaddon))))
			{
				list.Add(games_.arrayGamesScripts[i]);
			}
		}
		list.Sort((gameScript p1, gameScript p2) => p2.sellsTotal.CompareTo(p1.sellsTotal));
		while (list.Count > 200)
		{
			list.RemoveAt(list.Count - 1);
		}
		for (int num = 0; num < list.Count; num++)
		{
			if ((bool)list[num])
			{
				GameObject obj = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				Item_AllTimeCharts component = obj.GetComponent<Item_AllTimeCharts>();
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.genres_ = genres_;
				component.game_ = list[num];
				obj.name = list[num].sellsTotal.ToString();
			}
		}
		mS_.SortChildrenByFloat(uiObjects[0]);
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void TAB_Select(int t)
	{
		TAB = t;
		sfx_.PlaySound(3, force: true);
		guiMain_.SetTab(uiObjects[4], t);
		uiObjects[0].SetActive(value: false);
		uiObjects[0].SetActive(value: true);
		SetData();
	}
}
