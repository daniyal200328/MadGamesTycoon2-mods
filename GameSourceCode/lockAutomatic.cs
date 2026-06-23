using UnityEngine;
using UnityEngine.UI;

public class lockAutomatic : MonoBehaviour
{
	public GameObject uiRoom;

	public GameObject lockGameObject;

	private void OnEnable()
	{
		if (!uiRoom)
		{
			return;
		}
		roomScript rS_ = uiRoom.GetComponent<roomButtonScript>().rS_;
		if (!rS_ || !rS_.taskGameObject)
		{
			return;
		}
		bool flag = false;
		if ((bool)rS_.taskGameObject.GetComponent<taskForschung>())
		{
			flag = rS_.taskGameObject.GetComponent<taskForschung>().automatic;
			if ((bool)rS_.mS_ && rS_.taskGameObject.GetComponent<taskForschung>().autoForschung)
			{
				if (lockGameObject.activeSelf)
				{
					lockGameObject.SetActive(value: false);
				}
				base.gameObject.GetComponent<Button>().interactable = true;
				base.gameObject.transform.GetChild(0).GetComponent<Text>().text = rS_.mS_.tS_.GetText(2422);
				return;
			}
		}
		if ((bool)rS_.taskGameObject.GetComponent<taskMarketing>())
		{
			flag = rS_.taskGameObject.GetComponent<taskMarketing>().automatic;
		}
		if ((bool)rS_.taskGameObject.GetComponent<taskTraining>())
		{
			flag = rS_.taskGameObject.GetComponent<taskTraining>().automatic;
		}
		if ((bool)rS_.taskGameObject.GetComponent<taskContractWork>())
		{
			flag = rS_.taskGameObject.GetComponent<taskContractWork>().automatic;
		}
		if ((bool)rS_.taskGameObject.GetComponent<taskUpdate>())
		{
			flag = rS_.taskGameObject.GetComponent<taskUpdate>().automatic;
		}
		if ((bool)rS_.taskGameObject.GetComponent<taskFankampagne>())
		{
			flag = rS_.taskGameObject.GetComponent<taskFankampagne>().automatic;
		}
		if ((bool)rS_.taskGameObject.GetComponent<taskSpielbericht>())
		{
			flag = rS_.taskGameObject.GetComponent<taskSpielbericht>().automatic;
		}
		if ((bool)rS_.taskGameObject.GetComponent<taskProduction>())
		{
			flag = rS_.taskGameObject.GetComponent<taskProduction>().automatic;
		}
		if ((bool)rS_.taskGameObject.GetComponent<taskF2PUpdate>())
		{
			flag = rS_.taskGameObject.GetComponent<taskF2PUpdate>().automatic;
		}
		if ((bool)rS_.taskGameObject.GetComponent<taskMitarbeitersuche>())
		{
			flag = rS_.taskGameObject.GetComponent<taskMitarbeitersuche>().automatic;
		}
		if (flag && (bool)rS_.mS_)
		{
			if (lockGameObject.activeSelf)
			{
				lockGameObject.SetActive(value: false);
			}
			base.gameObject.GetComponent<Button>().interactable = true;
			base.gameObject.transform.GetChild(0).GetComponent<Text>().text = rS_.mS_.tS_.GetText(168);
		}
		else
		{
			if (lockGameObject.activeSelf)
			{
				lockGameObject.SetActive(value: false);
			}
			base.gameObject.GetComponent<Button>().interactable = true;
			base.gameObject.transform.GetChild(0).GetComponent<Text>().text = rS_.mS_.tS_.GetText(1403);
		}
	}
}
