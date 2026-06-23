using UnityEngine;
using UnityEngine.UI;

public class publishingOfferMain : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private roomDataScript rdS_;

	private forschungSonstiges fS_;

	private genres genres_;

	private unlockScript unlock_;

	private forschungSonstiges forschungSonstiges_;

	private platforms platforms_;

	private games games_;

	public int amountPublishingOffers;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!rdS_)
		{
			rdS_ = main_.GetComponent<roomDataScript>();
		}
		if (!fS_)
		{
			fS_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
	}

	public publishingOffer CreatePublishingOffer()
	{
		publishingOffer component = Object.Instantiate(uiPrefabs[0]).GetComponent<publishingOffer>();
		component.main_ = main_;
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.genres_ = genres_;
		return component;
	}

	public void UpdateGUI()
	{
		if (amountPublishingOffers > 0 && forschungSonstiges_.IsErforscht(33))
		{
			if (!uiObjects[0].activeSelf)
			{
				uiObjects[0].SetActive(value: true);
			}
			uiObjects[0].transform.GetChild(0).GetComponent<Text>().text = amountPublishingOffers.ToString();
		}
		else if (uiObjects[0].activeSelf)
		{
			uiObjects[0].SetActive(value: false);
		}
	}

	public void UpdatePublishingOffer(bool forceNewPublishingOffer)
	{
	}

	private int GetPlatform()
	{
		int result = 0;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			platformScript component = array[i].GetComponent<platformScript>();
			if ((bool)component && component.isUnlocked && !component.vomMarktGenommen && component.typ != 3 && component.typ != 4)
			{
				result = component.myID;
				if (Random.Range(0, 100) > 70)
				{
					return component.myID;
				}
			}
		}
		return result;
	}

	private int GetRandomDeveloperID()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		if (array.Length != 0)
		{
			bool flag = false;
			while (!flag)
			{
				int num = Random.Range(0, array.Length);
				if ((bool)array[num])
				{
					publisherScript component = array[num].GetComponent<publisherScript>();
					if ((bool)component && component.isUnlocked && component.developer && !component.publisher && !component.onlyMobile)
					{
						flag = true;
						return component.myID;
					}
				}
			}
		}
		return 0;
	}
}
