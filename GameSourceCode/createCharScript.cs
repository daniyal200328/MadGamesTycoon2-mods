using UnityEngine;

public class createCharScript : MonoBehaviour
{
	private mainScript mS_;

	private Camera camera_;

	private mainCameraScript mCamS_;

	private GameObject main_;

	private settingsScript settings_;

	private GUI_Tooltip guiTooltip;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private clipScript clipS_;

	private roomDataScript rdS_;

	public movementScript moveS_;

	private mapScript mapS_;

	public GameObject charMainObject;

	public GameObject[] charGfxMales;

	public GameObject[] charGfxFemales;

	public int DEBUG_ForceMesh = -1;

	public int DEBUG_Sex;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			if (!main_)
			{
				main_ = GameObject.FindWithTag("Main");
			}
			if (!mS_)
			{
				mS_ = main_.GetComponent<mainScript>();
			}
			if (!rdS_)
			{
				rdS_ = main_.GetComponent<roomDataScript>();
			}
			if (!clipS_)
			{
				clipS_ = main_.GetComponent<clipScript>();
			}
			if (!settings_)
			{
				settings_ = main_.GetComponent<settingsScript>();
			}
			if (!camera_)
			{
				camera_ = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
			}
			if (!mCamS_)
			{
				mCamS_ = GameObject.FindWithTag("MainCamera").GetComponent<mainCameraScript>();
			}
			if (!guiTooltip)
			{
				guiTooltip = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Tooltip>();
			}
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			}
			if (!sfx_)
			{
				sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
			}
			if (!moveS_)
			{
				moveS_ = GetComponent<movementScript>();
			}
			if (!mapS_)
			{
				mapS_ = main_.GetComponent<mapScript>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
		}
	}

	public characterScript CreateCharacter(int id_, bool male, int forceModel)
	{
		FindScripts();
		GameObject gameObject = Object.Instantiate(charMainObject);
		characterScript component = gameObject.GetComponent<characterScript>();
		movementScript component2 = gameObject.GetComponent<movementScript>();
		component.main_ = main_;
		component.mS_ = mS_;
		component.rdS_ = rdS_;
		component.clipS_ = clipS_;
		component.settings_ = settings_;
		component.camera_ = camera_;
		component.mCamS_ = mCamS_;
		component.guiTooltip = guiTooltip;
		component.guiMain_ = guiMain_;
		component.sfx_ = sfx_;
		component.moveS_ = component2;
		component.mapS_ = mapS_;
		component.tS_ = tS_;
		component2.main_ = main_;
		component2.mS_ = mS_;
		component2.cS_ = component;
		component2.sfx_ = sfx_;
		component2.clipS_ = clipS_;
		component2.mapS_ = mapS_;
		component.myID = id_;
		GameObject gameObject2 = null;
		if (DEBUG_ForceMesh != -1)
		{
			gameObject2 = ((DEBUG_Sex != 0) ? Object.Instantiate(charGfxFemales[DEBUG_ForceMesh], gameObject.transform) : Object.Instantiate(charGfxMales[DEBUG_ForceMesh], gameObject.transform));
		}
		else if (male)
		{
			if (forceModel == -1)
			{
				int num = Random.Range(0, charGfxMales.Length);
				gameObject2 = Object.Instantiate(charGfxMales[num], gameObject.transform);
				component.model_body = num;
			}
			else
			{
				if (forceModel >= charGfxMales.Length)
				{
					forceModel = 0;
				}
				gameObject2 = Object.Instantiate(charGfxMales[forceModel], gameObject.transform);
			}
		}
		else if (forceModel == -1)
		{
			int num2 = Random.Range(0, charGfxFemales.Length);
			gameObject2 = Object.Instantiate(charGfxFemales[num2], gameObject.transform);
			component.model_body = num2;
		}
		else
		{
			if (forceModel >= charGfxFemales.Length)
			{
				forceModel = 0;
			}
			gameObject2 = Object.Instantiate(charGfxFemales[forceModel], gameObject.transform);
		}
		if (forceModel == -1)
		{
			gameObject2.GetComponent<characterGFXScript>().Init(forcedClothes: false);
		}
		gameObject2.transform.localPosition = new Vector3(0f, 0.5f, 0f);
		gameObject2.transform.localEulerAngles = new Vector3(0f, 0f, -180f);
		component.Init();
		return component;
	}
}
