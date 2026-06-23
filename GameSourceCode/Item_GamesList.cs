using UnityEngine;
using UnityEngine.UI;

public class Item_GamesList : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	private float updateTimer;

	private void Start()
	{
	}

	private void Update()
	{
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
				SetData(uiObjects[2].GetComponent<Text>().text);
			}
		}
	}

	public void SetData(string c)
	{
		uiObjects[2].GetComponent<Text>().text = c;
		uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
		uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
		uiObjects[3].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
		if (game_.ownerID == mS_.myID || game_.publisherID == mS_.myID)
		{
			GetComponent<Image>().color = guiMain_.colors[4];
		}
		tooltip_.c = game_.GetTooltip();
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[46]);
		guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(game_);
	}
}
