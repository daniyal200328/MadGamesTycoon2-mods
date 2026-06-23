using UnityEngine;
using UnityEngine.UI;

public class LeftNews : MonoBehaviour
{
	public GameObject[] uiObjects;

	private float timer;

	public int roomID = -1;

	private void Start()
	{
	}

	public void Init(int roomID_, Sprite sprite_, string tooltip_, Sprite smallSprite_)
	{
		roomID = roomID_;
		uiObjects[0].GetComponent<Image>().sprite = sprite_;
		uiObjects[1].GetComponent<Image>().sprite = smallSprite_;
		base.gameObject.GetComponent<tooltip>().c = tooltip_;
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (timer > 30f)
		{
			Remove();
		}
	}

	public void BUTTON_Click()
	{
		sfxScript component = GameObject.Find("SFX").GetComponent<sfxScript>();
		if ((bool)component)
		{
			component.PlaySound(3, force: true);
		}
		if (roomID != -1)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
			for (int i = 0; i < array.Length; i++)
			{
				roomScript component2 = array[i].GetComponent<roomScript>();
				if ((bool)component2 && component2.myID == roomID)
				{
					GameObject gameObject = GameObject.Find("CamMovement");
					if ((bool)gameObject)
					{
						gameObject.transform.position = new Vector3(component2.uiPos.x, gameObject.transform.position.y, component2.uiPos.z);
					}
					break;
				}
			}
		}
		Remove();
	}

	private void Remove()
	{
		Object.Destroy(base.gameObject);
	}
}
