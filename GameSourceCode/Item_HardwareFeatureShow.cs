using UnityEngine;
using UnityEngine.UI;

public class Item_HardwareFeatureShow : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public hardwareFeatures hardwareFeatures_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = hardwareFeatures_.GetName(myID);
		tooltip_.c = hardwareFeatures_.GetTooltip(myID);
	}
}
