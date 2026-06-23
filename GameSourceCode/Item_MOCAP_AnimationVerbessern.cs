using UnityEngine;
using UnityEngine.UI;

public class Item_MOCAP_AnimationVerbessern : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

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

	public void SetData()
	{
		if (!game_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
		tooltip_.c = game_.GetTooltip();
		for (int i = 0; i < 6; i++)
		{
			if (!game_.motionCaptureStudio[i])
			{
				uiObjects[1 + i].GetComponent<Image>().color = Color.grey;
			}
			else
			{
				uiObjects[1 + i].GetComponent<Image>().color = Color.white;
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[179].SetActive(value: false);
		guiMain_.uiObjects[178].GetComponent<Menu_MOCAP_AnimationVerbessern>().SetGame(game_);
	}
}
