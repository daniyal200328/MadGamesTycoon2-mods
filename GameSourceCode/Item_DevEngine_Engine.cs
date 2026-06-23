using UnityEngine;
using UnityEngine.UI;

public class Item_DevEngine_Engine : MonoBehaviour
{
	public engineScript eS_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public engineFeatures eF_;

	public genres genres_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	private float updateTimer;

	private void Start()
	{
		SetData();
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
				SetData();
			}
		}
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = eS_.GetName();
		uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(eS_.spezialgenre);
		eS_.SetSpezialPlatformSprite(uiObjects[6]);
		uiObjects[3].GetComponent<Text>().text = eS_.GetTechLevel().ToString();
		tooltip_.c = eS_.GetTooltip();
		string text = tS_.GetText(160) + ": " + eS_.GetFeaturesAmount();
		text = text + "\n" + tS_.GetText(261) + ": " + eS_.GetGamesAmount();
		uiObjects[2].GetComponent<Text>().text = text;
		uiObjects[4].GetComponent<Text>().text = "";
		if (!eS_.sellEngine)
		{
			uiObjects[5].SetActive(value: false);
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[37]);
		guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>().InitUpdateEngine(guiMain_.uiObjects[41].GetComponent<Menu_Dev_Engine_SelectOld>().rS_, eS_);
		guiMain_.uiObjects[41].SetActive(value: false);
	}
}
