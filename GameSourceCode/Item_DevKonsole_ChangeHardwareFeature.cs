using UnityEngine;
using UnityEngine.UI;

public class Item_DevKonsole_ChangeHardwareFeature : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public hardwareFeatures hardwareFeatures_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public Menu_Dev_ChangeKonsolenFeature menu_;

	public platformScript pS_;

	private void Start()
	{
		if (uiObjects[5].activeSelf)
		{
			uiObjects[5].SetActive(value: false);
		}
		SetData();
		FindScripts();
	}

	private void FindScripts()
	{
	}

	private void Update()
	{
		if (menu_.hwFeatures[myID])
		{
			GetComponent<Image>().color = guiMain_.colors[4];
		}
		else
		{
			GetComponent<Image>().color = Color.white;
		}
		int devCosts = hardwareFeatures_.GetDevCosts(myID);
		devCosts += devCosts * menu_.GetKomplexitaetVerteuerung();
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(devCosts, showDollar: true);
	}

	private void SetData()
	{
		GetComponent<Button>().interactable = true;
		uiObjects[0].GetComponent<Text>().text = hardwareFeatures_.GetName(myID);
		if (hardwareFeatures_.hardFeat_NEEDINTERNET[myID])
		{
			uiObjects[2].SetActive(value: true);
			if (!pS_.internet)
			{
				GetComponent<Button>().interactable = false;
				menu_.hwFeatures[myID] = false;
			}
		}
		if (hardwareFeatures_.hardFeat_ONLYSTATIONARY[myID])
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
			if (pS_.typ == 2)
			{
				GetComponent<Button>().interactable = false;
				menu_.hwFeatures[myID] = false;
			}
		}
		if (hardwareFeatures_.hardFeat_ONLYHANDHELD[myID])
		{
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
			if (pS_.typ == 1)
			{
				GetComponent<Button>().interactable = false;
				menu_.hwFeatures[myID] = false;
			}
		}
		if (pS_.hwFeatures[myID])
		{
			GetComponent<Button>().interactable = false;
			if (!uiObjects[5].activeSelf)
			{
				uiObjects[5].SetActive(value: true);
			}
		}
		tooltip_.c = hardwareFeatures_.GetTooltip(myID);
	}

	public void BUTTON_Click()
	{
		FindScripts();
		if (GetComponent<Button>().interactable)
		{
			sfx_.PlaySound(3, force: false);
			if (menu_.SetKonsolenFeature(myID))
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			else
			{
				GetComponent<Image>().color = Color.white;
			}
		}
	}
}
