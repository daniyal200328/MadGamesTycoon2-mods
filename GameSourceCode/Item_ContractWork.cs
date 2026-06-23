using UnityEngine;
using UnityEngine.UI;

public class Item_ContractWork : MonoBehaviour
{
	public contractWork contract_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	private publisherScript pS_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		if (!contract_)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			MultiplayerUpdate();
		}
	}

	private void MultiplayerUpdate()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 5f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	private void SetData()
	{
		if (contract_.art != 5 && contract_.art != 6)
		{
			uiObjects[0].GetComponent<Text>().text = contract_.GetName();
			uiObjects[6].GetComponent<Text>().text = tS_.GetText(603) + ": " + mS_.Round(contract_.GetArbeitsaufwand() * 0.1f, 2);
		}
		if (contract_.art == 6)
		{
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1560);
			uiObjects[6].GetComponent<Text>().text = tS_.GetText(603) + ": " + mS_.Round(contract_.GetArbeitsaufwand() * 0.1f, 2);
		}
		if (contract_.art == 5)
		{
			uiObjects[0].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(contract_.points), showDollar: false);
		}
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(contract_.GetGehalt(), showDollar: true);
		uiObjects[4].GetComponent<Text>().text = tS_.GetText(601) + ": " + mS_.GetMoney(contract_.GetStrafe(), showDollar: true);
		string text = tS_.GetText(605);
		text = text.Replace("<NUM>", contract_.GetWochen().ToString());
		uiObjects[5].GetComponent<Text>().text = text;
		if (!pS_)
		{
			GameObject gameObject = GameObject.Find("PUB_" + contract_.auftraggeberID);
			if ((bool)gameObject)
			{
				pS_ = gameObject.GetComponent<publisherScript>();
			}
		}
		if ((bool)pS_)
		{
			uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
			uiObjects[3].GetComponent<Text>().text = pS_.GetName();
		}
		tooltip_.c = contract_.GetTooltip();
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Remove()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)contract_)
		{
			Object.Destroy(contract_.gameObject);
			Object.Destroy(base.gameObject);
		}
	}

	public void BUTTON_Click()
	{
		Menu_Dev_AuftragSelect component = guiMain_.uiObjects[96].GetComponent<Menu_Dev_AuftragSelect>();
		contract_.angenommen = true;
		taskContractWork taskContractWork2 = guiMain_.AddTask_ContractWork();
		taskContractWork2.Init(fromSavegame: false);
		taskContractWork2.contractID = contract_.myID;
		taskContractWork2.points = contract_.GetArbeitsaufwand();
		taskContractWork2.pointsLeft = contract_.GetArbeitsaufwand();
		taskContractWork2.automatic = component.uiObjects[4].GetComponent<Toggle>().isOn;
		if (taskContractWork2.automatic)
		{
			taskContractWork2.automaticWait = component.uiObjects[7].GetComponent<Toggle>().isOn;
		}
		component.rS_.taskID = taskContractWork2.myID;
		sfx_.PlaySound(3, force: true);
		component.BUTTON_Close();
		guiMain_.uiObjects[95].SetActive(value: false);
		guiMain_.CloseMenu();
	}
}
