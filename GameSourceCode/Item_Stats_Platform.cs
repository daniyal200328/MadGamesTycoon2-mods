using UnityEngine;
using UnityEngine.UI;

public class Item_Stats_Platform : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public platformScript pS_;

	private RectTransform myRect_;

	private int frames;

	private bool hasEnabled;

	private float updateTimer;

	private void Start()
	{
	}

	private void Update()
	{
		frames++;
		if (frames >= 3)
		{
			if (!myRect_)
			{
				myRect_ = GetComponent<RectTransform>();
			}
			if (myRect_.position.y >= 0f && myRect_.position.y <= (float)Screen.height)
			{
				EnableObjects();
				MultiplayerUpdate();
			}
		}
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

	public void EnableObjects()
	{
		if (hasEnabled)
		{
			return;
		}
		hasEnabled = true;
		for (int i = 0; i < uiObjects.Length; i++)
		{
			if ((bool)uiObjects[i] && !uiObjects[i].activeSelf)
			{
				uiObjects[i].SetActive(value: true);
				SetData();
			}
		}
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		pS_.SetPic(uiObjects[1]);
		uiObjects[2].GetComponent<Text>().text = pS_.GetDateString();
		uiObjects[3].GetComponent<Text>().text = tS_.GetText(220) + ": " + pS_.GetGames();
		uiObjects[4].GetComponent<Text>().text = tS_.GetText(219) + ": " + pS_.GetMarktanteilString();
		uiObjects[5].GetComponent<Text>().text = mS_.GetMoney(pS_.GetPrice(), showDollar: true);
		uiObjects[6].GetComponent<Text>().text = pS_.tech.ToString();
		guiMain_.DrawStars(uiObjects[7], pS_.erfahrung);
		uiObjects[9].GetComponent<Image>().sprite = pS_.GetComplexSprite();
		if (pS_.internet)
		{
			uiObjects[10].SetActive(value: true);
		}
		else
		{
			uiObjects[10].SetActive(value: false);
		}
		tooltip_.c = pS_.GetTooltip();
		if (pS_.inBesitz)
		{
			uiObjects[5].GetComponent<Text>().text = "<color=black>" + tS_.GetText(682) + "</color>";
			GetComponent<Image>().color = guiMain_.colors[7];
		}
		uiObjects[11].GetComponent<Image>().sprite = pS_.GetTypSprite();
		uiObjects[11].GetComponent<tooltip>().c = pS_.GetTypString();
		if (pS_.vomMarktGenommen)
		{
			GetComponent<Image>().color = guiMain_.colors[2];
		}
		else
		{
			uiObjects[8].SetActive(value: false);
		}
		if (pS_.IsAngekuendigt())
		{
			uiObjects[2].GetComponent<Text>().text = pS_.GetManufacturer();
			uiObjects[3].GetComponent<Text>().text = "";
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(217);
			uiObjects[5].GetComponent<Text>().text = "<color=#164165>" + pS_.date_year + " " + tS_.GetText(pS_.date_month + 220) + "</color>";
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
