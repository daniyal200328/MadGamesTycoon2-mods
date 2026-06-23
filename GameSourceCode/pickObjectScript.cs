using UnityEngine;
using UnityEngine.UI;

public class pickObjectScript : MonoBehaviour
{
	private mainScript mS_;

	private GUI_Main guiMain;

	private mapScript mapS_;

	private Camera myCamera;

	private sfxScript sfx_;

	private pickCharacterScript pcS_;

	private RaycastHit hit;

	private RaycastHit hitOld;

	private RaycastHit hitEmpty;

	public LayerMask layerMask;

	private gummibandScript gummiS_;

	public bool disableMouseButton;

	public Vector3 oldPosition;

	public Vector3 oldRotation;

	private mainCameraScript mCamS_;

	public bool reopenBuyInventarMenu;

	public int buyInventar = -1;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!mS_)
		{
			mS_ = GetComponent<mainScript>();
		}
		if (!pcS_)
		{
			pcS_ = GetComponent<pickCharacterScript>();
		}
		if (!guiMain)
		{
			guiMain = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!myCamera)
		{
			myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		if (!mapS_)
		{
			mapS_ = GetComponent<mapScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!gummiS_)
		{
			gummiS_ = GameObject.Find("CanvasInGameMenu").GetComponent<gummibandScript>();
		}
		if (!mCamS_)
		{
			mCamS_ = myCamera.GetComponent<mainCameraScript>();
		}
	}

	private void Update()
	{
		Pick();
		disableMouseButton = false;
	}

	private void Pick()
	{
		if (gummiS_.isActive || guiMain.uiObjects[3].GetComponent<Toggle>().isOn)
		{
			return;
		}
		if ((bool)pcS_.hitOld.transform)
		{
			Unpick();
			return;
		}
		if ((bool)mS_.pickedObject)
		{
			Unpick();
			return;
		}
		if (guiMain.menuOpen && !guiMain.uiObjects[20].activeSelf)
		{
			Unpick();
			return;
		}
		if (guiMain.IsMouseOverGUI())
		{
			Unpick();
			return;
		}
		if (Physics.Raycast(myCamera.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)), out hit, 200f, layerMask))
		{
			mCamS_.EnablePostLiner();
			if (hit.transform != hitOld.transform)
			{
				if (hit.transform.CompareTag("Object"))
				{
					if ((bool)hitOld.transform)
					{
						hitOld.transform.gameObject.GetComponent<objectScript>().MouseLeave();
						hitOld = hitEmpty;
					}
					hitOld = hit;
					hit.transform.gameObject.GetComponent<objectScript>().MouseOver();
				}
				else
				{
					Unpick();
				}
			}
		}
		else
		{
			Unpick();
		}
		if (!Input.GetMouseButtonUp(0) || !hitOld.transform || disableMouseButton || (!mS_.settings_TutorialOff && guiMain.GetTutorialStep() < 2) || guiMain.uiObjects[3].GetComponent<Toggle>().isOn)
		{
			return;
		}
		if (guiMain.uiObjects[20].activeSelf)
		{
			if (Input.GetKey(KeyCode.LeftControl))
			{
				mapS_.CreateObject(hitOld.transform.gameObject.GetComponent<objectScript>().typ, createdWithAutoInventar: false);
				return;
			}
			reopenBuyInventarMenu = true;
			buyInventar = guiMain.uiObjects[20].GetComponent<Menu_BuyInventar>().buyInventar;
			guiMain.uiObjects[20].GetComponent<Menu_BuyInventar>().BUTTON_CloseSelectInventar(resetScrollbar: false);
		}
		else if (Input.GetKey(KeyCode.LeftControl) && (bool)mS_.guiMain_)
		{
			objectScript component = hitOld.transform.gameObject.GetComponent<objectScript>();
			if ((bool)component)
			{
				GameObject gameObject = GameObject.Find("Room_" + component.GetRoomID());
				if ((bool)gameObject)
				{
					int typ = gameObject.GetComponent<roomScript>().typ;
					mS_.guiMain_.DROPDOWN_BuyInventar(typ);
					mapS_.CreateObject(hitOld.transform.gameObject.GetComponent<objectScript>().typ, createdWithAutoInventar: false);
				}
				else
				{
					mS_.guiMain_.DROPDOWN_BuyInventar(0);
					mapS_.CreateObject(hitOld.transform.gameObject.GetComponent<objectScript>().typ, createdWithAutoInventar: false);
				}
				return;
			}
		}
		Click(hitOld.transform.gameObject);
		hitOld = hitEmpty;
	}

	private void Unpick()
	{
		if ((bool)hitOld.transform)
		{
			hitOld.transform.gameObject.GetComponent<objectScript>().MouseLeave();
			hitOld = hitEmpty;
		}
	}

	public void Click(GameObject go)
	{
		guiMain.OpenMenu(hideChars: false);
		sfx_.PlaySound(8, force: true);
		mS_.objectRotation = go.transform.eulerAngles.y;
		objectScript component = go.transform.gameObject.GetComponent<objectScript>();
		objectScript component2 = mapS_.CreateObject(component.typ, createdWithAutoInventar: false).GetComponent<objectScript>();
		component2.gekauft = true;
		component2.aufladungenAkt = component.aufladungenAkt;
		if (!mS_.settings_TutorialOff && component2.typ == 92)
		{
			guiMain.SetTutorialStep(3);
		}
		oldPosition = go.transform.position;
		oldRotation = go.transform.eulerAngles;
		Object.Destroy(go);
		guiMain.ActivateMenu(guiMain.uiObjects[0]);
	}
}
