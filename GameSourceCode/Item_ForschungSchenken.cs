using UnityEngine;
using UnityEngine.UI;

public class Item_ForschungSchenken : MonoBehaviour
{
	public int myID;

	public int art;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public genres genres_;

	public themes themes_;

	public engineFeatures eF_;

	public gameplayFeatures gF_;

	public hardware hardware_;

	public hardwareFeatures hardwareFeature_;

	public forschungSonstiges fS_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public Menu_MP_ForschungSchenken menu_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		switch (art)
		{
		case 0:
			uiObjects[0].GetComponent<Text>().text = genres_.GetName(myID);
			uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(myID);
			tooltip_.c = genres_.GetTooltip(myID);
			GetComponent<Button>().interactable = !mS_.mpCalls_.playersMP[menu_.selectedPlayer].genres[myID];
			break;
		case 1:
			uiObjects[0].GetComponent<Text>().text = tS_.GetThemes(myID);
			uiObjects[1].GetComponent<Image>().sprite = guiMain_.uiSprites[6];
			tooltip_.c = tS_.GetThemes(myID);
			GetComponent<Button>().interactable = !mS_.mpCalls_.playersMP[menu_.selectedPlayer].themes[myID];
			break;
		case 2:
			uiObjects[0].GetComponent<Text>().text = eF_.GetName(myID);
			uiObjects[1].GetComponent<Image>().sprite = eF_.engineFeatures_PICTYP[eF_.engineFeatures_TYP[myID]];
			tooltip_.c = eF_.GetDesc(myID);
			GetComponent<Button>().interactable = !mS_.mpCalls_.playersMP[menu_.selectedPlayer].engineFeatures[myID];
			break;
		case 3:
			uiObjects[0].GetComponent<Text>().text = gF_.GetName(myID);
			uiObjects[1].GetComponent<Image>().sprite = gF_.GetTypSprite(myID);
			tooltip_.c = gF_.GetDesc(myID);
			GetComponent<Button>().interactable = !mS_.mpCalls_.playersMP[menu_.selectedPlayer].gameplayFeatures[myID];
			break;
		case 4:
			uiObjects[0].GetComponent<Text>().text = hardware_.GetName(myID);
			uiObjects[1].GetComponent<Image>().sprite = hardware_.GetTypPic(myID);
			tooltip_.c = hardware_.GetTooltip(myID);
			GetComponent<Button>().interactable = !mS_.mpCalls_.playersMP[menu_.selectedPlayer].hardware[myID];
			break;
		case 5:
			uiObjects[0].GetComponent<Text>().text = fS_.GetName(myID);
			uiObjects[1].GetComponent<Image>().sprite = fS_.GetPic(myID);
			tooltip_.c = fS_.GetTooltip(myID);
			GetComponent<Button>().interactable = !mS_.mpCalls_.playersMP[menu_.selectedPlayer].forschungSonstiges[myID];
			break;
		case 6:
			uiObjects[0].GetComponent<Text>().text = hardwareFeature_.GetName(myID);
			uiObjects[1].GetComponent<Image>().sprite = hardwareFeature_.GetSprite(myID);
			tooltip_.c = hardwareFeature_.GetTooltip(myID);
			GetComponent<Button>().interactable = !mS_.mpCalls_.playersMP[menu_.selectedPlayer].hardwareFeatures[myID];
			break;
		}
		if (menu_.selectedForschung == myID)
		{
			GetComponent<Image>().color = guiMain_.colors[4];
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
		menu_.selectedForschung = myID;
		SetData();
	}
}
