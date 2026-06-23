using UnityEngine;
using UnityEngine.UI;

public class Menu_AwardsVerlauf : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private games games_;

	public gameScript bestGrafik;

	public gameScript bestSound;

	public publisherScript bestStudio;

	public publisherScript bestPublisher;

	public gameScript bestGame;

	public gameScript badGame;

	public int seite;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
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

	public void Init()
	{
		FindScripts();
		if (seite < 0)
		{
			seite = 0;
		}
		if (seite > mS_.madGamesCon_BestGrafik.Count - 1)
		{
			seite = mS_.madGamesCon_BestGrafik.Count - 1;
		}
		if (mS_.madGamesCon_BestGrafik.Count > 0)
		{
			if (mS_.madGamesCon_BestGrafik.Count > seite)
			{
				FindWinners(mS_.madGamesCon_BestGrafik[seite], mS_.madGamesCon_BestSound[seite], mS_.madGamesCon_BestStudio[seite], mS_.madGamesCon_BestPublisher[seite], mS_.madGamesCon_BestGame[seite], mS_.madGamesCon_BadGame[seite]);
			}
			ShowAwards();
		}
	}

	public void FindWinners(int IDbestGrafik, int IDbestSound, int IDbestStudio, int IDbestPublisher, int IDbestGame, int IDbadGame)
	{
		bestGrafik = null;
		bestSound = null;
		bestStudio = null;
		bestPublisher = null;
		bestGame = null;
		badGame = null;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if (IDbestGrafik == component.myID)
				{
					bestGrafik = component;
				}
				if (IDbestSound == component.myID)
				{
					bestSound = component;
				}
				if (IDbestGame == component.myID)
				{
					bestGame = component;
				}
				if (IDbadGame == component.myID)
				{
					badGame = component;
				}
			}
		}
		array = GameObject.FindGameObjectsWithTag("Publisher");
		if (array.Length == 0)
		{
			return;
		}
		for (int j = 0; j < array.Length; j++)
		{
			publisherScript component2 = array[j].GetComponent<publisherScript>();
			if (IDbestStudio == component2.myID)
			{
				bestStudio = component2;
			}
			if (IDbestPublisher == component2.myID)
			{
				bestPublisher = component2;
			}
		}
	}

	private void ShowAwards()
	{
		if (!bestGrafik)
		{
			uiObjects[0].GetComponent<Text>().text = "-";
			uiObjects[1].GetComponent<Text>().text = "-";
			uiObjects[2].GetComponent<Text>().text = "-";
			uiObjects[3].GetComponent<Text>().text = "-";
			uiObjects[4].GetComponent<Text>().text = "-";
			uiObjects[5].GetComponent<Text>().text = "-";
			uiObjects[6].GetComponent<Text>().text = "-";
			uiObjects[7].GetComponent<Text>().text = "-";
			return;
		}
		uiObjects[6].GetComponent<Text>().text = mS_.madGamesCon_Jahr[seite].ToString();
		uiObjects[7].GetComponent<Text>().text = seite + 1 + " / " + mS_.madGamesCon_Jahr.Count;
		gameScript gameScript2 = null;
		gameScript2 = null;
		gameScript2 = bestGrafik;
		if ((bool)gameScript2)
		{
			if (gameScript2.ownerID == mS_.myID || gameScript2.publisherID == mS_.myID)
			{
				uiObjects[0].GetComponent<Text>().text = "<color=blue>" + gameScript2.GetNameWithTag() + "</color>";
			}
			else
			{
				uiObjects[0].GetComponent<Text>().text = gameScript2.GetNameWithTag();
				if (mS_.multiplayer && gameScript2.GameFromMitspieler())
				{
					uiObjects[0].GetComponent<Text>().text = "<color=magenta>" + gameScript2.GetNameWithTag() + "</color>";
				}
			}
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = "-";
		}
		gameScript2 = null;
		gameScript2 = bestSound;
		if ((bool)gameScript2)
		{
			if (gameScript2.ownerID == mS_.myID || gameScript2.publisherID == mS_.myID)
			{
				uiObjects[1].GetComponent<Text>().text = "<color=blue>" + gameScript2.GetNameWithTag() + "</color>";
			}
			else
			{
				uiObjects[1].GetComponent<Text>().text = gameScript2.GetNameWithTag();
				if (mS_.multiplayer && gameScript2.GameFromMitspieler())
				{
					uiObjects[1].GetComponent<Text>().text = "<color=magenta>" + gameScript2.GetNameWithTag() + "</color>";
				}
			}
		}
		else
		{
			uiObjects[1].GetComponent<Text>().text = "-";
		}
		publisherScript publisherScript2 = bestStudio;
		if ((bool)publisherScript2)
		{
			if (publisherScript2.myID != mS_.myID)
			{
				uiObjects[2].GetComponent<Text>().text = publisherScript2.GetName();
				if (publisherScript2.isPlayer)
				{
					uiObjects[2].GetComponent<Text>().text = "<color=magenta>" + publisherScript2.GetName() + "</color>";
				}
			}
			else
			{
				uiObjects[2].GetComponent<Text>().text = "<color=blue>" + publisherScript2.GetName() + "</color>";
			}
			uiObjects[12].GetComponent<Button>().interactable = true;
		}
		publisherScript2 = bestPublisher;
		if ((bool)publisherScript2)
		{
			if (publisherScript2.myID != mS_.myID)
			{
				uiObjects[3].GetComponent<Text>().text = publisherScript2.GetName();
				if (publisherScript2.isPlayer)
				{
					uiObjects[3].GetComponent<Text>().text = "<color=magenta>" + publisherScript2.GetName() + "</color>";
				}
			}
			else
			{
				uiObjects[3].GetComponent<Text>().text = "<color=blue>" + publisherScript2.GetName() + "</color>";
			}
			uiObjects[13].GetComponent<Button>().interactable = true;
		}
		gameScript2 = null;
		gameScript2 = bestGame;
		if ((bool)gameScript2)
		{
			if (gameScript2.ownerID == mS_.myID || gameScript2.publisherID == mS_.myID)
			{
				uiObjects[4].GetComponent<Text>().text = "<color=blue>" + gameScript2.GetNameWithTag() + "</color>";
			}
			else
			{
				uiObjects[4].GetComponent<Text>().text = gameScript2.GetNameWithTag();
				if (mS_.multiplayer && gameScript2.GameFromMitspieler())
				{
					uiObjects[4].GetComponent<Text>().text = "<color=magenta>" + gameScript2.GetNameWithTag() + "</color>";
				}
			}
		}
		else
		{
			uiObjects[4].GetComponent<Text>().text = "-";
		}
		gameScript2 = null;
		gameScript2 = badGame;
		if ((bool)gameScript2)
		{
			if (gameScript2.ownerID == mS_.myID || gameScript2.publisherID == mS_.myID)
			{
				uiObjects[5].GetComponent<Text>().text = "<color=blue>" + gameScript2.GetNameWithTag() + "</color>";
				return;
			}
			uiObjects[5].GetComponent<Text>().text = gameScript2.GetNameWithTag();
			if (mS_.multiplayer && gameScript2.GameFromMitspieler())
			{
				uiObjects[5].GetComponent<Text>().text = "<color=magenta>" + gameScript2.GetNameWithTag() + "</color>";
			}
		}
		else
		{
			uiObjects[5].GetComponent<Text>().text = "-";
		}
	}

	public void BUTTON_Seite(int i)
	{
		sfx_.PlaySound(3, force: true);
		seite += i;
		if (seite < 0)
		{
			seite = 0;
		}
		if (seite > mS_.madGamesCon_BestGrafik.Count - 1)
		{
			seite = mS_.madGamesCon_BestGrafik.Count - 1;
		}
		Init();
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_ShowGame(int i)
	{
		sfx_.PlaySound(3, force: true);
		switch (i)
		{
		case 0:
			if ((bool)bestGrafik)
			{
				guiMain_.uiObjects[46].SetActive(value: true);
				guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(bestGrafik);
			}
			break;
		case 1:
			if ((bool)bestSound)
			{
				guiMain_.uiObjects[46].SetActive(value: true);
				guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(bestSound);
			}
			break;
		case 2:
			if ((bool)bestGame)
			{
				guiMain_.uiObjects[46].SetActive(value: true);
				guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(bestGame);
			}
			break;
		case 3:
			if ((bool)badGame)
			{
				guiMain_.uiObjects[46].SetActive(value: true);
				guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(badGame);
			}
			break;
		}
	}

	public void BUTTON_ShowStudio()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)bestStudio)
		{
			guiMain_.uiObjects[359].SetActive(value: true);
			guiMain_.uiObjects[359].GetComponent<Menu_Stats_Developer_Main>().Init(bestStudio);
		}
	}

	public void BUTTON_ShowPublisher()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)bestStudio)
		{
			guiMain_.uiObjects[373].SetActive(value: true);
			guiMain_.uiObjects[373].GetComponent<Menu_Stats_Publisher_Main>().Init(bestPublisher);
		}
	}
}
