using UnityEngine;
using UnityEngine.UI;

public class Menu_Statistics_WochenCharts : MonoBehaviour
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

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			GameObject gameObject = parent_.transform.GetChild(i).gameObject;
			if (gameObject.activeSelf)
			{
				Item_WochenCharts component = gameObject.GetComponent<Item_WochenCharts>();
				if (component.game_.myID == id_)
				{
					gameObject.name = component.game_.sellsPerWeek[0].ToString();
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
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i])
			{
				gameScript gameScript2 = games_.arrayGamesScripts[i];
				if ((bool)gameScript2 && gameScript2.sellsPerWeek[0] > 0 && gameScript2.isOnMarket && !gameScript2.inDevelopment && ((TAB == 0 && games_.arrayGamesScripts[i].gameTyp != 2 && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade) || (TAB == 1 && games_.arrayGamesScripts[i].gameTyp != 2 && games_.arrayGamesScripts[i].handy) || (TAB == 2 && games_.arrayGamesScripts[i].gameTyp != 2 && games_.arrayGamesScripts[i].arcade) || (TAB == 3 && games_.arrayGamesScripts[i].gameTyp == 2)) && !Exists(uiObjects[0], gameScript2.myID))
				{
					GameObject obj = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
					Item_WochenCharts component = obj.GetComponent<Item_WochenCharts>();
					component.mS_ = mS_;
					component.tS_ = tS_;
					component.sfx_ = sfx_;
					component.guiMain_ = guiMain_;
					component.genres_ = genres_;
					component.game_ = gameScript2;
					obj.name = gameScript2.sellsPerWeek[0].ToString();
				}
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
