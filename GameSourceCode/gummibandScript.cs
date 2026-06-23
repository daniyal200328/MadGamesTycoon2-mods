using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class gummibandScript : MonoBehaviour
{
	private RectTransform rT;

	public GameObject myObject;

	private Vector2 start;

	public bool isActive;

	private Image myImage;

	private GUI_Main guiMain_;

	private mainScript mS_;

	private Camera camera_;

	private settingsScript settings_;

	private pickCharacterScript pcS_;

	private Vector2 vPos;

	private Vector2 vSize;

	private void Start()
	{
		if (!mS_)
		{
			mS_ = GameObject.FindGameObjectWithTag("Main").GetComponent<mainScript>();
		}
		if (!pcS_)
		{
			pcS_ = GameObject.FindGameObjectWithTag("Main").GetComponent<pickCharacterScript>();
		}
		if (!settings_)
		{
			settings_ = GameObject.FindGameObjectWithTag("Main").GetComponent<settingsScript>();
		}
		if (!camera_)
		{
			camera_ = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		guiMain_ = GetComponent<GUI_Main>();
		rT = myObject.GetComponent<RectTransform>();
		myImage = myObject.GetComponent<Image>();
		myImage.enabled = false;
	}

	private void Update()
	{
		UpdateInput();
		UpdateGFX();
	}

	private void UpdateGFX()
	{
		if (myImage.enabled)
		{
			vPos = new Vector2(0f, 0f);
			vSize = new Vector2(Mathf.Abs(Input.mousePosition.x - start.x), Mathf.Abs(Input.mousePosition.y - start.y));
			if (Input.mousePosition.x - start.x >= 0f)
			{
				vPos = new Vector2(start.x, vPos.y);
			}
			if (Input.mousePosition.y - start.y >= 0f)
			{
				vPos = new Vector2(vPos.x, start.y);
			}
			if (Input.mousePosition.x - start.x < 0f)
			{
				vPos = new Vector2(start.x - vSize.x, vPos.y);
			}
			if (Input.mousePosition.y - start.y < 0f)
			{
				vPos = new Vector2(vPos.x, start.y - vSize.y);
			}
			rT.anchoredPosition = vPos / settings_.uiScale;
			rT.sizeDelta = vSize / settings_.uiScale;
		}
	}

	private void UpdateInput()
	{
		if (isActive && guiMain_.menuOpen)
		{
			if (myImage.enabled)
			{
				myImage.enabled = false;
			}
			isActive = false;
		}
		if (guiMain_.uiObjects[2].GetComponent<Toggle>().isOn)
		{
			return;
		}
		if (!guiMain_.IsMouseOverGUI() && !isActive && mS_.pickedChars.Count == 0 && !guiMain_.menuOpen)
		{
			if (Input.GetMouseButtonDown(0))
			{
				start = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
				vSize = new Vector2(0f, 0f);
				if (!myImage.enabled)
				{
					myImage.enabled = true;
				}
			}
			if (Input.GetMouseButton(0) && vSize.x > 32f && vSize.y > 32f)
			{
				isActive = true;
			}
		}
		if (Input.GetMouseButtonUp(0))
		{
			if (myImage.enabled)
			{
				myImage.enabled = false;
			}
			if (isActive)
			{
				SelectCharacters();
				StartCoroutine(DisableEndOfFrame());
			}
		}
	}

	private void SelectCharacters()
	{
		for (int i = 0; i < mS_.arrayCharacters.Length; i++)
		{
			if ((bool)mS_.arrayCharacters[i])
			{
				Vector3 position = mS_.arrayCharacters[i].transform.position;
				position.y += 0.5f;
				Vector2 vector = camera_.WorldToScreenPoint(position);
				if (vector.x >= vPos.x && vector.x <= vPos.x + vSize.x && vector.y >= vPos.y && vector.y <= vPos.y + vSize.y)
				{
					StartCoroutine(pcS_.PickChar(mS_.arrayCharacters[i]));
				}
			}
		}
	}

	private IEnumerator DisableEndOfFrame()
	{
		yield return new WaitForEndOfFrame();
		isActive = false;
	}
}
