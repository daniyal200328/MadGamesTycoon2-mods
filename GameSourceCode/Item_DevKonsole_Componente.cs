using UnityEngine;
using UnityEngine.UI;

public class Item_DevKonsole_Componente : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public hardware hardware_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = hardware_.GetName(myID);
		uiObjects[1].GetComponent<Image>().sprite = hardware_.GetTypPic(myID);
		uiObjects[3].GetComponent<Text>().text = hardware_.hardware_TECH[myID].ToString();
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(hardware_.GetDevCosts(myID), showDollar: true);
		uiObjects[4].GetComponent<Text>().text = tS_.GetText(1604) + ": <color=blue>" + mS_.GetMoney(hardware_.GetPerformance(myID), showDollar: false) + "</color>";
		tooltip_.c = hardware_.GetTooltip(myID);
		if (!hardware_.IsTechComponent(myID))
		{
			uiObjects[5].SetActive(value: false);
			uiObjects[6].SetActive(value: true);
		}
		Menu_Dev_Konsole component = guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>();
		if (component.component_cpu == myID || component.component_gfx == myID || component.component_ram == myID || component.component_hdd == myID || component.component_sfx == myID || component.component_cooling == myID || component.component_disc == myID || component.component_controller == myID || component.component_case == myID || component.component_monitor == myID)
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
		guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>().SetComponent(hardware_.hardware_TYP[myID], myID);
		guiMain_.uiObjects[319].GetComponent<Menu_Dev_KonsoleComponent>().BUTTON_Close();
	}
}
