using UnityEngine;
using UnityEngine.UI;

public class Item_GameplayFeatures_GameDesign : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private gameplayFeatures gF_;

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
			main_ = GameObject.Find("Main");
			mS_ = main_.GetComponent<mainScript>();
			tS_ = main_.GetComponent<textScript>();
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = gF_.GetName(myID);
		uiObjects[1].GetComponent<Image>().sprite = gF_.GetTypSprite(myID);
		uiObjects[2].GetComponent<stars>().amount = gF_.gameplayFeatures_LEVEL[myID];
		tooltip_.c = "<b>" + gF_.GetName(myID) + "</b>\n" + gF_.GetDesc(myID) + "\n\n" + tS_.GetText(8) + "\n\n<b><i>" + tS_.GetText(6) + "\n" + mS_.GetMoney(gF_.GetDevCosts(myID), showDollar: true) + "\n\n</i><color=green>" + tS_.GetText(1) + " +" + gF_.GetGameplay(myID, -1, -1) + "</color>\n<color=blue>" + tS_.GetText(2) + " +" + gF_.GetGraphic(myID, -1, -1) + "</color>\n<color=magenta>" + tS_.GetText(3) + " +" + gF_.GetSound(myID, -1, -1) + "</color>\n<color=orange>" + tS_.GetText(74) + " +" + gF_.GetTechnik(myID, -1, -1) + "</color>\n</b>";
		for (int i = 0; i < gF_.gameplayFeatures_LEVEL[myID]; i++)
		{
			tooltip_.c += "<size=22><b><color=orange>★</color></b></size>";
		}
		for (int j = gF_.gameplayFeatures_LEVEL[myID]; j < 5; j++)
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
