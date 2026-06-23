using UnityEngine;
using UnityEngine.UI;

public class Item_Polishing : MonoBehaviour
{
	public gameScript game_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public roomScript rS_;

	private float updateTimer;

	private void Start()
	{
		SetData();
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
				SetData();
			}
		}
	}

	private void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
			uiObjects[2].GetComponent<Image>().sprite = game_.GetTypSprite();
			uiObjects[3].GetComponent<Image>().sprite = game_.GetPlatformTypSprite();
			tooltip_.c = game_.GetTooltip();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.uiObjects[279].GetComponent<Menu_ROOM_Polishing>().StartPolishing(game_);
	}
}
