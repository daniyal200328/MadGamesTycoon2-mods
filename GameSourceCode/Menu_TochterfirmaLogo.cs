using UnityEngine;
using UnityEngine.UI;

public class Menu_TochterfirmaLogo : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private publisherScript pS_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init(publisherScript pubScript_)
	{
		FindScripts();
		pS_ = pubScript_;
		for (int i = 0; i < guiMain_.logoSprites.Length; i++)
		{
			if ((bool)guiMain_.logoSprites[i])
			{
				GameObject gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				Item_FirmenlogoTochterfirma component = gameObject.GetComponent<Item_FirmenlogoTochterfirma>();
				component.myID = i;
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				if (LogoUsed(i))
				{
					gameObject.name = "A";
				}
				else
				{
					gameObject.name = "B";
				}
			}
		}
		mS_.SortChildrenByName(uiObjects[0]);
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
	}

	private bool LogoUsed(int id_)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i] && array[i].GetComponent<publisherScript>().logoID == id_)
			{
				return true;
			}
		}
		return false;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
