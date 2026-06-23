using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_CharList : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
{
	public GameObject[] uiObjects;

	public Color[] colors;

	public int slot;

	public characterScript cS_;

	public mainScript mS_;

	public GUI_Main guiMain_;

	private void Start()
	{
		Init();
	}

	private void Init()
	{
		if ((bool)cS_)
		{
			uiObjects[0].GetComponent<Text>().text = cS_.GetGroupString("orange") + " " + cS_.myName;
			base.gameObject.GetComponent<tooltip>().c = cS_.GetTooltip();
		}
	}

	private void Update()
	{
		if (!cS_)
		{
			Object.Destroy(base.gameObject);
			guiMain_.uiObjects[15].GetComponent<Menu_PickCharacter>().UpdateData();
		}
		else if (mS_.pickedChars.Count > 0)
		{
			if (mS_.pickedChars[0] == cS_.gameObject)
			{
				uiObjects[0].GetComponent<Text>().text = "• " + cS_.GetGroupString("orange") + " " + cS_.myName;
			}
			else
			{
				uiObjects[0].GetComponent<Text>().text = cS_.GetGroupString("orange") + " " + cS_.myName;
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		uiObjects[0].GetComponent<Text>().color = colors[1];
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		uiObjects[0].GetComponent<Text>().color = colors[0];
	}

	public void BUTTON_Click()
	{
		if (!cS_)
		{
			return;
		}
		for (int i = 0; i < mS_.pickedChars.Count; i++)
		{
			if (mS_.pickedChars[i] == cS_.gameObject)
			{
				mS_.pickedChars.RemoveAt(i);
				break;
			}
		}
		mS_.pickedChars.Insert(0, cS_.gameObject);
		guiMain_.uiObjects[15].GetComponent<Menu_PickCharacter>().UpdateData();
	}
}
