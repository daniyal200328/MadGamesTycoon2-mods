using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_GamePass_Games : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	public Button button_;

	private RectTransform myRect_;

	private void Start()
	{
	}

	private void Update()
	{
		SetActivIcon();
	}

	public void EnableObjects()
	{
		for (int i = 0; i < uiObjects.Length; i++)
		{
			if ((bool)uiObjects[i] && !uiObjects[i].activeSelf)
			{
				uiObjects[i].SetActive(value: true);
			}
		}
		SetData();
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[1].GetComponent<Text>().text = game_.reviewTotal + "%";
			uiObjects[2].GetComponent<Text>().text = game_.GetReleaseDateString();
			uiObjects[3].GetComponent<Text>().text = game_.GetDeveloperName();
			SetActivIcon();
			if (game_.IsRemovedFromMarket())
			{
				GetComponent<Image>().color = guiMain_.colors[26];
			}
		}
	}

	private void SetActivIcon()
	{
		if (!game_.gamePlatformScript[0])
		{
			game_.FindMyPlatforms();
		}
		for (int i = 0; i < game_.gamePlatformScript.Length; i++)
		{
			if ((bool)game_.gamePlatformScript[i] && (game_.gamePlatformScript[i].inGamePass || game_.gamePlatformScript[i].inGamePassPassiv))
			{
				if (!uiObjects[5].activeSelf)
				{
					uiObjects[5].SetActive(value: true);
				}
				return;
			}
		}
		if (uiObjects[5].activeSelf)
		{
			uiObjects[5].SetActive(value: false);
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click(bool allGames)
	{
		if (!mS_)
		{
			return;
		}
		if (!allGames)
		{
			sfx_.PlaySound(3, force: true);
		}
		if (!game_)
		{
			return;
		}
		if (game_.inGamePass)
		{
			mS_.gpS_.GAMEPASS_RemoveGame(game_, !allGames);
		}
		else
		{
			mS_.gpS_.GAMEPASS_AddGame(game_, !allGames);
		}
		if (!allGames)
		{
			Menu_GamePass_Games component = guiMain_.uiObjects[417].GetComponent<Menu_GamePass_Games>();
			if (!game_.inGamePass)
			{
				base.gameObject.transform.SetParent(component.uiObjects[4].transform);
				component.DROPDOWN_Sort_OutPass();
			}
			else
			{
				base.gameObject.transform.SetParent(component.uiObjects[0].transform);
				component.DROPDOWN_Sort_InPass();
			}
			component.MESSAGEBOX_PlatformRemoved();
			guiMain_.KeinEintrag(component.uiObjects[4], component.uiObjects[8]);
			guiMain_.KeinEintrag(component.uiObjects[0], component.uiObjects[7]);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		if ((bool)game_)
		{
			tooltip_.c = game_.GetTooltip();
			if (!tooltip_.enabled)
			{
				tooltip_.enabled = true;
			}
			if (!button_.enabled)
			{
				button_.enabled = true;
			}
		}
	}
}
