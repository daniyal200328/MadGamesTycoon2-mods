using UnityEngine;
using UnityEngine.UI;

public class Item_TochterfirmaIpTausch : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	private void Start()
	{
		SetData();
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetIpName();
			guiMain_.DrawIpBekanntheit(uiObjects[1], game_);
			tooltip_.c = game_.GetTooltipIP();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click(bool allIps)
	{
		if (!allIps)
		{
			sfx_.PlaySound(3, force: true);
		}
		Menu_Stats_TochterfirmaIpTausch component = guiMain_.uiObjects[403].GetComponent<Menu_Stats_TochterfirmaIpTausch>();
		if (!component)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		if (!allIps)
		{
			if (!mS_.games_.IsIpFree(game_, messageBox_: true))
			{
				return;
			}
		}
		else if (!mS_.games_.IsIpFree(game_, messageBox_: false))
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			gameScript component2 = array[i].GetComponent<gameScript>();
			if (!component2 || component2.mainIP != game_.mainIP)
			{
				continue;
			}
			if (component2.ownerID == component.GetRightPublisher().myID)
			{
				component2.ipToSell = false;
				component2.ownerS_ = null;
				component2.ownerID = component.GetLeftPublisher().myID;
			}
			else
			{
				component2.ipToSell = false;
				component2.ownerS_ = null;
				component2.ownerID = component.GetRightPublisher().myID;
			}
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_GameOwner(component2);
				}
				else
				{
					mS_.mpCalls_.CLIENT_Send_GameOwner(component2);
				}
			}
		}
		if (!allIps)
		{
			component.SetDataLeft();
			component.SetDataRight();
		}
		RemoveIpFokus(game_.mainIP);
	}

	public void RemoveIpFokus(int ipID)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			publisherScript component = array[i].GetComponent<publisherScript>();
			if (!component)
			{
				continue;
			}
			for (int j = 0; j < component.tf_ipFocus.Length; j++)
			{
				if (component.tf_ipFocus[j] == ipID)
				{
					component.tf_ipFocus[j] = -1;
				}
			}
		}
	}
}
