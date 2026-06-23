using UnityEngine;
using UnityEngine.UI;

public class Item_DevEngine_Feature : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public engineFeatures eF_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public bool activ;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = eF_.GetName(myID);
		uiObjects[1].GetComponent<Image>().sprite = eF_.engineFeatures_PICTYP[eF_.engineFeatures_TYP[myID]];
		uiObjects[3].GetComponent<Text>().text = eF_.engineFeatures_TECH[myID].ToString();
		guiMain_.DrawStars(uiObjects[4], eF_.engineFeatures_LEVEL[myID]);
		tooltip_.c = eF_.GetTooltip(myID);
		SetButtonColor();
		if (!guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>().featuresLock[myID])
		{
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(eF_.GetDevCostsForEngine(myID), showDollar: true);
		}
		else
		{
			uiObjects[2].GetComponent<Text>().text = "$0";
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		Menu_Dev_Engine component = guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>();
		if (!component.featuresLock[myID])
		{
			activ = !activ;
			sfx_.PlaySound(3, force: true);
			component.SetFeature(myID, activ);
			SetButtonColor();
		}
	}

	private void SetButtonColor()
	{
		if (guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>().featuresLock[myID])
		{
			GetComponent<Button>().interactable = false;
		}
		else if (activ)
		{
			uiObjects[5].GetComponent<Image>().color = guiMain_.colors[4];
		}
		else
		{
			uiObjects[5].GetComponent<Image>().color = Color.white;
		}
	}
}
