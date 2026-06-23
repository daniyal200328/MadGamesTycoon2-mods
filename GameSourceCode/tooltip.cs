using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class tooltip : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	private textScript tS_;

	private settingsScript settings_;

	private GameObject main_;

	private mainScript mS_;

	private GUI_Main guiMain_;

	public int textID = -1;

	public string textArray = "";

	public string c = "";

	public KeyCode shortcut;

	private GUI_Tooltip guiTooltip;

	public Camera mainCamera;

	private RaycastHit raycastHit;

	private float middleMouseTimer;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if ((bool)main_)
		{
			return;
		}
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
			if (!mS_.guiMain_)
			{
				mS_.FindScripts();
			}
		}
		if (!tS_)
		{
			tS_ = mS_.tS_;
		}
		if (!settings_)
		{
			settings_ = mS_.settings_;
		}
		if (!guiMain_)
		{
			guiMain_ = mS_.guiMain_;
		}
	}

	private void Update()
	{
		if (guiMain_.selectInputField || shortcut == KeyCode.None || Input.GetKey(KeyCode.LeftShift))
		{
			return;
		}
		bool flag = false;
		if (settings_.middleMouseClose && shortcut == KeyCode.Escape)
		{
			if (Input.GetMouseButton(1))
			{
				middleMouseTimer += Time.deltaTime;
			}
			if (Input.GetMouseButtonUp(1))
			{
				if (middleMouseTimer < 0.15f)
				{
					flag = true;
				}
				middleMouseTimer = 0f;
			}
		}
		if (Input.GetKeyUp(shortcut) || flag)
		{
			if ((bool)GetComponent<Button>() && GetComponent<Button>().interactable)
			{
				FindScripts();
				guiMain_.SetUIHotkey(base.gameObject);
			}
			else if ((bool)base.transform.parent.GetComponent<Toggle>() && base.transform.parent.GetComponent<Toggle>().interactable)
			{
				base.transform.parent.GetComponent<Toggle>().isOn = !base.transform.parent.GetComponent<Toggle>().isOn;
			}
			else if ((bool)GetComponent<Toggle>() && GetComponent<Toggle>().interactable)
			{
				GetComponent<Toggle>().isOn = !GetComponent<Toggle>().isOn;
			}
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		FindScripts();
		if (!guiTooltip)
		{
			guiTooltip = base.transform.root.GetComponent<GUI_Tooltip>();
		}
		if (!guiTooltip)
		{
			return;
		}
		if (textArray.Length > 0 && textID > -1)
		{
			string text = textArray;
			if (!(text == "text"))
			{
				if (text == "country")
				{
					c = tS_.GetCountry(textID);
				}
			}
			else
			{
				c = tS_.GetText(textID);
			}
		}
		if (shortcut != KeyCode.None)
		{
			c = c + "<br><br><color=purple><i>" + tS_.GetText(87) + " <" + shortcut.ToString() + "></i></color>";
		}
		guiTooltip.SetActive(c);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		if ((bool)guiTooltip)
		{
			guiTooltip.SetInactive();
		}
	}

	public void OnDisable()
	{
		if ((bool)guiTooltip)
		{
			guiTooltip.SetInactive();
		}
	}
}
