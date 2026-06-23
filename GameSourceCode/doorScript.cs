using UnityEngine;

public class doorScript : MonoBehaviour
{
	private GameObject main_;

	private mainScript mS_;

	private mapScript mapS_;

	public Animation myAnim;

	public int roomID = -1;

	public int buildingID = -1;

	private bool isOpen;

	private float oldGamespeed;

	public bool buildingDoor;

	private float updateTimer;

	private GameObject oldChar;

	private void Start()
	{
		FindScripts();
		Init();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
			mS_ = main_.GetComponent<mainScript>();
			mapS_ = main_.GetComponent<mapScript>();
		}
	}

	private void Init()
	{
		GameObject gameObject = base.transform.parent.gameObject;
		roomID = mapS_.mapRoomID[Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z)];
		buildingID = mapS_.mapBuilding[Mathf.RoundToInt(gameObject.transform.position.x), Mathf.RoundToInt(gameObject.transform.position.z)];
	}

	private void Update()
	{
		if (oldGamespeed != mS_.GetGameSpeed())
		{
			oldGamespeed = mS_.GetGameSpeed();
			myAnim["doorOpen"].speed = mS_.GetGameSpeed();
			myAnim["doorClose"].speed = mS_.GetGameSpeed();
		}
		updateTimer += Time.deltaTime;
		if (updateTimer < 0.1f)
		{
			return;
		}
		updateTimer = 0f;
		Vector3 position = base.transform.position;
		if ((bool)oldChar && Vector3.Distance(position, oldChar.transform.position) < 2f)
		{
			return;
		}
		for (int i = 0; i < mS_.arrayRobotsForDoors.Count; i++)
		{
			if (!mS_.arrayRobotsForDoors[i] || !(Mathf.Abs(position.x - mS_.arrayRobotsForDoors[i].transform.position.x) < 2f) || !(Vector3.Distance(position, mS_.arrayRobotsForDoors[i].transform.position) < 2f))
			{
				continue;
			}
			robotScript component = mS_.arrayRobotsForDoors[i].GetComponent<robotScript>();
			if (!component || !component.myTarget)
			{
				continue;
			}
			if (isOpen)
			{
				return;
			}
			bool flag = false;
			if (!buildingDoor && ((component.GetPosition_RoomID() == roomID && component.GetTargetPosition_RoomID() != roomID) || (component.GetPosition_RoomID() != roomID && component.GetTargetPosition_RoomID() == roomID)))
			{
				flag = true;
				oldChar = mS_.arrayRobotsForDoors[i];
			}
			if (flag || buildingDoor)
			{
				if (!isOpen && !myAnim.isPlaying)
				{
					isOpen = true;
					myAnim.Play("doorOpen");
				}
				return;
			}
		}
		for (int j = 0; j < mS_.arrayCharactersForDoors.Count; j++)
		{
			if (!mS_.arrayCharactersForDoors[j] || !(Mathf.Abs(position.x - mS_.arrayCharactersForDoors[j].transform.position.x) < 2f) || !(Vector3.Distance(position, mS_.arrayCharactersForDoors[j].transform.position) < 2f))
			{
				continue;
			}
			characterScript component2 = mS_.arrayCharactersForDoors[j].GetComponent<characterScript>();
			if (!component2 || !component2.moveS_ || !component2.moveS_.myTarget || component2.picked)
			{
				continue;
			}
			if (isOpen)
			{
				return;
			}
			bool flag2 = false;
			if (!buildingDoor)
			{
				if ((component2.moveS_.GetPosition_RoomID() == roomID && component2.moveS_.GetTargetPosition_RoomID() != roomID) || (component2.moveS_.GetPosition_RoomID() != roomID && component2.moveS_.GetTargetPosition_RoomID() == roomID))
				{
					flag2 = true;
					oldChar = mS_.arrayCharactersForDoors[j];
				}
			}
			else if ((component2.moveS_.GetPosition_BuildingID() == buildingID && component2.moveS_.GetTargetPosition_BuildingID() != buildingID) || (component2.moveS_.GetPosition_BuildingID() != buildingID && component2.moveS_.GetTargetPosition_BuildingID() == buildingID) || mS_.office == 7)
			{
				flag2 = true;
				oldChar = mS_.arrayCharactersForDoors[j];
			}
			if (flag2)
			{
				if (!isOpen && !myAnim.isPlaying)
				{
					isOpen = true;
					myAnim.Play("doorOpen");
				}
				return;
			}
		}
		if (isOpen)
		{
			oldChar = null;
			if (!myAnim.isPlaying)
			{
				isOpen = false;
				myAnim.Play("doorClose");
			}
		}
	}
}
