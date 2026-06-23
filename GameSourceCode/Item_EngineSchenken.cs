using UnityEngine;
using UnityEngine.UI;

public class Item_EngineSchenken : MonoBehaviour
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

	public Menu_MP_EngineSchenken menu_;

	private float updateTimer;

	private void Start()
	{
		SetData();
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
		if (!eS_.sellEngine || eS_.OwnerIsNPC())
		{
			uiObjects[5].SetActive(value: false);
		}
		if (eS_.sellEngine && eS_.myID == mS_.myID)
		{
			uiObjects[5].SetActive(value: true);
		}
		if ((bool)menu_.selectedEngine)
		{
			if (menu_.selectedEngine.myID == eS_.myID)
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			else
			{
				GetComponent<Image>().color = Color.white;
			}
		}
		else
		{
			GetComponent<Image>().color = Color.white;
		}
	}

	private void Update()
	{
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 0.1f))
		{
			updateTimer = 0f;
			SetData();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		menu_.selectedEngine = eS_;
		SetData();
	}
}
