using UnityEngine;
using UnityEngine.UI;

public class Item_DevKonsole_HardwareFeature : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public hardwareFeatures hardwareFeatures_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public Menu_Dev_Konsole menu_;

	private void Start()
	{
		SetData();
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
		uiObjects[0].GetComponent<Text>().text = hardwareFeatures_.GetName(myID);
		if (hardwareFeatures_.hardFeat_NEEDINTERNET[myID])
		{
			uiObjects[2].SetActive(value: true);
			if (!menu_.uiObjects[53].GetComponent<Toggle>().isOn)
			{
				GetComponent<Button>().interactable = false;
				menu_.hwFeatures[myID] = false;
			}
		}
		if (hardwareFeatures_.hardFeat_ONLYSTATIONARY[myID])
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
			if (menu_.platformTyp == 2)
			{
				GetComponent<Button>().interactable = false;
				menu_.hwFeatures[myID] = false;
			}
		}
		if (hardwareFeatures_.hardFeat_ONLYHANDHELD[myID])
		{
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
			if (menu_.platformTyp == 1)
			{
				GetComponent<Button>().interactable = false;
				menu_.hwFeatures[myID] = false;
			}
		}
		tooltip_.c = hardwareFeatures_.GetTooltip(myID);
	}

	public void BUTTON_Click()
	{
		if (GetComponent<Button>().interactable)
		{
			sfx_.PlaySound(3, force: true);
			menu_.hwFeatures[myID] = !menu_.hwFeatures[myID];
			menu_.UpdateGUI();
		}
	}
}
