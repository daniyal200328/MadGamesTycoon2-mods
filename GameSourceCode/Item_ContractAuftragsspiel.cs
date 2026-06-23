using UnityEngine;
using UnityEngine.UI;

public class Item_ContractAuftragsspiel : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public themes themes_;

	public roomScript rS_;

	public games games_;

	public gameScript game_;

	private platformScript platformScript_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		if (!game_)
		{
			Object.Destroy(base.gameObject);
		}
		else if (game_.isOnMarket || !game_.auftragsspiel)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			MultiplayerUpdate();
		}
	}

	private void MultiplayerUpdate()
	{
		if ((bool)platformScript_ && platformScript_.inBesitz && platformScript_.OwnerIsNPC())
		{
			uiObjects[8].GetComponent<Image>().color = Color.white;
		}
		if (!mS_.multiplayer)
		{
			return;
		}
		if ((bool)game_)
		{
			if (game_.auftragsspiel_Blocked)
			{
				GetComponent<Button>().interactable = false;
			}
			else
			{
				GetComponent<Button>().interactable = true;
			}
		}
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 5f))
		{
			updateTimer = 0f;
			SetData();
		}
	}

	private void SetData()
	{
		if (!game_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
		uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
		uiObjects[1].GetComponent<tooltip>().c = genres_.GetTooltip(game_.maingenre);
		uiObjects[2].GetComponent<Image>().sprite = games_.gameSizeSprites[game_.gameSize];
		GameObject gameObject = GameObject.Find("PUB_" + game_.publisherID);
		if ((bool)gameObject)
		{
			uiObjects[3].GetComponent<Image>().sprite = gameObject.GetComponent<publisherScript>().GetLogo();
		}
		uiObjects[4].GetComponent<Text>().text = tS_.GetText(600) + ": <color=#3664B5><b>" + mS_.GetMoney(game_.auftragsspiel_gehalt, showDollar: true) + "</b></color>";
		Text component = uiObjects[4].GetComponent<Text>();
		component.text = component.text + "\n" + tS_.GetText(627) + ": <color=#3664B5><b>" + mS_.GetMoney(game_.auftragsspiel_bonus, showDollar: true) + "</b></color>";
		string text = "\n" + tS_.GetText(605);
		text = text.Replace("<NUM>", game_.auftragsspiel_zeitInWochen.ToString());
		uiObjects[4].GetComponent<Text>().text += text;
		text = "\n" + tS_.GetText(626);
		text = text.Replace("<NUM>", game_.auftragsspiel_mindestbewertung.ToString());
		uiObjects[4].GetComponent<Text>().text += text;
		if (game_.portID != -1)
		{
			uiObjects[4].GetComponent<Text>().text += "\n<color=blue>";
			if (game_.portID != -1 && game_.subgenre != -1)
			{
				Text component2 = uiObjects[4].GetComponent<Text>();
				component2.text = component2.text + "▪" + genres_.GetName(game_.subgenre) + " ";
			}
			if (game_.portID != -1 && game_.gameMainTheme != -1)
			{
				Text component3 = uiObjects[4].GetComponent<Text>();
				component3.text = component3.text + "▪" + tS_.GetThemes(game_.gameMainTheme) + " ";
			}
			if (game_.portID != -1 && game_.gameSubTheme != -1)
			{
				Text component4 = uiObjects[4].GetComponent<Text>();
				component4.text = component4.text + "▪" + tS_.GetThemes(game_.gameSubTheme) + " ";
			}
			uiObjects[4].GetComponent<Text>().text += "</color>";
		}
		if (!mS_.genres_.IsErforscht(game_.maingenre))
		{
			uiObjects[1].GetComponent<Image>().color = Color.red;
		}
		if (game_.gameSize == 1 && !mS_.forschungSonstiges_.IsErforscht(0))
		{
			uiObjects[2].GetComponent<Image>().color = Color.red;
		}
		if (game_.gameSize == 2 && !mS_.forschungSonstiges_.IsErforscht(1))
		{
			uiObjects[2].GetComponent<Image>().color = Color.red;
		}
		if (game_.gameSize == 3 && !mS_.forschungSonstiges_.IsErforscht(2))
		{
			uiObjects[2].GetComponent<Image>().color = Color.red;
		}
		if (game_.gameSize == 4 && !mS_.forschungSonstiges_.IsErforscht(3))
		{
			uiObjects[2].GetComponent<Image>().color = Color.red;
		}
		if ((bool)platformScript_)
		{
			return;
		}
		gameObject = GameObject.Find("PLATFORM_" + game_.gamePlatform[0]);
		if ((bool)gameObject)
		{
			platformScript_ = gameObject.GetComponent<platformScript>();
			platformScript_.SetPic(uiObjects[8]);
			uiObjects[8].GetComponent<tooltip>().c = platformScript_.GetTooltip();
			if (!platformScript_.inBesitz && platformScript_.OwnerIsNPC())
			{
				uiObjects[8].GetComponent<Image>().color = Color.red;
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		if (!game_)
		{
			return;
		}
		if (!mS_.genres_.IsErforscht(game_.maingenre))
		{
			guiMain_.MessageBox(tS_.GetText(1813), closeMenu: false);
		}
		else if (game_.portID != -1 && game_.subgenre != -1 && !mS_.genres_.IsErforscht(game_.subgenre))
		{
			string text = tS_.GetText(2305);
			text = text.Replace("<NAME>", genres_.GetName(game_.subgenre));
			guiMain_.MessageBox(text, closeMenu: false);
		}
		else if (game_.portID != -1 && game_.gameMainTheme != -1 && !themes_.IsErforscht(game_.gameMainTheme))
		{
			string text2 = tS_.GetText(2305);
			text2 = text2.Replace("<NAME>", tS_.GetThemes(game_.gameMainTheme));
			guiMain_.MessageBox(text2, closeMenu: false);
		}
		else if (game_.portID != -1 && game_.gameSubTheme != -1 && !themes_.IsErforscht(game_.gameSubTheme))
		{
			string text3 = tS_.GetText(2305);
			text3 = text3.Replace("<NAME>", tS_.GetThemes(game_.gameSubTheme));
			guiMain_.MessageBox(text3, closeMenu: false);
		}
		else if (game_.gameSize == 1 && !mS_.forschungSonstiges_.IsErforscht(0))
		{
			guiMain_.MessageBox(tS_.GetText(1814), closeMenu: false);
		}
		else if (game_.gameSize == 2 && !mS_.forschungSonstiges_.IsErforscht(1))
		{
			guiMain_.MessageBox(tS_.GetText(1814), closeMenu: false);
		}
		else if (game_.gameSize == 3 && !mS_.forschungSonstiges_.IsErforscht(2))
		{
			guiMain_.MessageBox(tS_.GetText(1814), closeMenu: false);
		}
		else if (game_.gameSize == 4 && !mS_.forschungSonstiges_.IsErforscht(3))
		{
			guiMain_.MessageBox(tS_.GetText(1814), closeMenu: false);
		}
		else if (platformScript_.inBesitz)
		{
			if (game_.auftragsspiel)
			{
				base.gameObject.SetActive(value: false);
				guiMain_.ActivateMenu(guiMain_.uiObjects[56]);
				guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().InitContractGame(rS_, game_);
				guiMain_.uiObjects[99].SetActive(value: false);
			}
			if (mS_.multiplayer && (bool)mS_.mpCalls_)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_BlockContractGame(game_, block_: true);
				}
				if (mS_.mpCalls_.isClient)
				{
					mS_.mpCalls_.CLIENT_Send_BlockContractGame(game_, block_: true);
				}
			}
		}
		else
		{
			guiMain_.MessageBox(tS_.GetText(631), closeMenu: false);
		}
	}

	public void BUTTON_Remove()
	{
		sfx_.PlaySound(3, force: true);
		game_.auftragsspiel_Inivs = true;
		Object.Destroy(base.gameObject);
	}
}
