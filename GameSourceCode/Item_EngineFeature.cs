using UnityEngine;
using UnityEngine.UI;

public class Item_EngineFeature : MonoBehaviour
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
		uiObjects[1].GetComponent<Image>().sprite = eF_.GetTypPic(myID);
		uiObjects[3].GetComponent<Text>().text = eF_.engineFeatures_TECH[myID].ToString();
		guiMain_.DrawStars(uiObjects[4], eF_.engineFeatures_LEVEL[myID]);
		tooltip_.c = eF_.GetTooltip(myID);
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}
}
