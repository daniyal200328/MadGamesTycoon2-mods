using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_Engine : MonoBehaviour
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

	public Menu_DevGame mDevGame_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = eS_.GetName();
		uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(eS_.spezialgenre);
		eS_.SetSpezialPlatformSprite(uiObjects[4]);
		uiObjects[3].GetComponent<Text>().text = eS_.GetTechLevel().ToString();
		tooltip_.c = eS_.GetTooltip();
		string text = tS_.GetText(160) + ": " + eS_.GetFeaturesAmount();
		if (eS_.ownerID == mS_.myID)
		{
			text = text + "\n" + tS_.GetText(262);
			uiObjects[0].GetComponent<Text>().color = Color.green;
		}
		else
		{
			text = text + "\n" + tS_.GetText(260) + ": " + eS_.gewinnbeteiligung + "%";
		}
		uiObjects[2].GetComponent<Text>().text = text;
		if (eS_.myID == guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().g_GameEngine)
		{
			GetComponent<Image>().color = guiMain_.colors[4];
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	private void Update()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 1f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetEngine(eS_.myID);
		guiMain_.uiObjects[65].GetComponent<Menu_DevGame_Engine>().BUTTON_Close();
	}
}
