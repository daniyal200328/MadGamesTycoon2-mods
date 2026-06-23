using UnityEngine;
using UnityEngine.UI;

public class Item_KonsoleKostenreduktion : MonoBehaviour
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
		if ((bool)pS_)
		{
			if (pS_.kostenreduktion >= 100f)
			{
				GetComponent<Button>().interactable = false;
				uiObjects[2].GetComponent<Text>().color = Color.red;
			}
			if (pS_.vomMarktGenommen)
			{
				Object.Destroy(base.gameObject);
				return;
			}
		}
		MultiplayerUpdate();
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
			uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(pS_.GetAktuellProductionsCosts(), showDollar: true);
			uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(pS_.kostenreduktion) + "%";
			uiObjects[3].GetComponent<Image>().sprite = pS_.GetTypSprite();
			uiObjects[5].GetComponent<Text>().text = pS_.GetDateString();
			pS_.SetPic(uiObjects[4]);
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
		guiMain_.uiObjects[451].SetActive(value: false);
		guiMain_.uiObjects[450].GetComponent<Menu_Dev_KonsoleKostenreduktion>().SetKonsole(pS_);
	}
}
