using UnityEngine;
using UnityEngine.UI;

public class Item_EngineFeatures_GameDesign : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private engineFeatures eF_;

	public tooltip tooltip_;

	private float updateTimer;

	private void Start()
	{
		FindScripts();
		SetData();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
			mS_ = main_.GetComponent<mainScript>();
			tS_ = main_.GetComponent<textScript>();
			eF_ = main_.GetComponent<engineFeatures>();
		}
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = eF_.GetName(myID);
		uiObjects[1].GetComponent<Image>().sprite = eF_.GetTypPic(myID);
		uiObjects[2].GetComponent<Text>().text = eF_.engineFeatures_TECH[myID].ToString();
		uiObjects[3].GetComponent<stars>().amount = eF_.engineFeatures_LEVEL[myID];
		tooltip_.c = "<b>" + eF_.GetName(myID) + "</b>\n" + eF_.GetDesc(myID) + "\n\n" + tS_.GetText(8) + "\n\n<b><i>" + tS_.GetText(6) + "\n" + mS_.GetMoney(eF_.GetDevCosts(myID), showDollar: true) + "\n\n</i></b><b><color=grey>" + tS_.GetText(4) + " " + eF_.engineFeatures_TECH[myID] + "</color>\n<color=green>" + tS_.GetText(1) + " +" + eF_.GetGameplay(myID) + "</color>\n<color=blue>" + tS_.GetText(2) + " +" + eF_.GetGraphic(myID) + "</color>\n<color=magenta>" + tS_.GetText(3) + " +" + eF_.GetSound(myID) + "</color>\n<color=orange>" + tS_.GetText(74) + " +" + eF_.GetTechnik(myID) + "</color>\n</b>";
		for (int i = 0; i < eF_.engineFeatures_LEVEL[myID]; i++)
		{
			tooltip_.c += "<size=22><b><color=orange>★</color></b></size>";
		}
		for (int j = eF_.engineFeatures_LEVEL[myID]; j < 5; j++)
		{
			tooltip_.c += "<size=22><b><color=black>★</color></b></size>";
		}
	}

	private void Update()
	{
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 1f))
		{
			updateTimer = 0f;
			SetData();
		}
	}
}
