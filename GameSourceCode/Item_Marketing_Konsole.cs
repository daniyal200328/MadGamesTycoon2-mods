using UnityEngine;
using UnityEngine.UI;

public class Item_Marketing_Konsole : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public platformScript pS_;

	private float updateTimer;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		if ((bool)pS_ && pS_.vomMarktGenommen)
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

	public void SetData()
	{
		if ((bool)pS_)
		{
			uiObjects[0].GetComponent<Text>().text = pS_.GetName();
			uiObjects[1].GetComponent<Text>().text = Mathf.RoundToInt(pS_.GetHype()).ToString();
			if (pS_.isUnlocked)
			{
				uiObjects[2].GetComponent<Text>().text = pS_.GetDateString();
			}
			else
			{
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(528);
			}
			tooltip_.c = pS_.GetTooltip();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[322].SetActive(value: false);
		guiMain_.uiObjects[321].GetComponent<Menu_Marketing_KonsoleKampagne>().SetKonsole(pS_);
	}
}
