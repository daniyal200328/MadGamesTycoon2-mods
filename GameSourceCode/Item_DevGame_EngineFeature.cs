using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_EngineFeature : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public engineFeatures eF_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = eF_.GetName(myID);
		uiObjects[1].GetComponent<Image>().sprite = eF_.engineFeatures_PICTYP[eF_.engineFeatures_TYP[myID]];
		uiObjects[3].GetComponent<Text>().text = eF_.engineFeatures_TECH[myID].ToString();
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(eF_.GetDevCosts(myID), showDollar: true);
		guiMain_.DrawStars(uiObjects[4], eF_.engineFeatures_LEVEL[myID]);
		tooltip_.c = eF_.GetTooltip(myID);
		if (guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().g_GameEngineFeature[eF_.engineFeatures_TYP[myID]] == myID)
		{
			GetComponent<Image>().color = guiMain_.colors[7];
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetEngineFeature(eF_.engineFeatures_TYP[myID], myID);
		guiMain_.uiObjects[67].GetComponent<Menu_DevGame_EngineFeature>().BUTTON_Close();
	}
}
