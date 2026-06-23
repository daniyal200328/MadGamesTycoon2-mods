using UnityEngine;

public class arcadeProduktionScript : MonoBehaviour
{
	public Animation myAnimation;

	public bool force;

	public GameObject saegeblatt;

	public GameObject saegeplattPartikel;

	private GameObject main_;

	private roomScript roomS_;

	private mapScript mapS_;

	private mainScript mS_;

	private objectScript oS_;

	private characterScript charS_;

	private AnimationState animState;

	private void Start()
	{
		FindScripts();
		DisableAllChilds();
		animState = myAnimation["arcadeProduction"];
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
		}
		if (!oS_)
		{
			oS_ = base.transform.root.gameObject.GetComponent<objectScript>();
			if (mS_.multiplayer && !oS_)
			{
				Object.Destroy(this);
			}
		}
	}

	private void Update()
	{
		if (force)
		{
			return;
		}
		if (!oS_)
		{
			FindScripts();
			return;
		}
		if (oS_.picked)
		{
			if (myAnimation.isPlaying)
			{
				myAnimation.Stop();
				DisableAllChilds();
			}
			return;
		}
		roomS_ = mapS_.mapRoomScript[Mathf.RoundToInt(base.transform.root.transform.position.x), Mathf.RoundToInt(base.transform.root.transform.position.z)];
		if (!roomS_)
		{
			return;
		}
		if (roomS_.taskID == -1 || !oS_.inUse || roomS_.pause || oS_.picked)
		{
			if (!myAnimation.isPlaying)
			{
				return;
			}
			myAnimation.Stop();
			DisableAllChilds();
			if ((bool)saegeblatt)
			{
				saegeblatt.GetComponent<Animation>().Stop();
				if ((bool)saegeplattPartikel && saegeplattPartikel.activeSelf)
				{
					saegeplattPartikel.SetActive(value: false);
				}
			}
			return;
		}
		if (!charS_ && oS_.besetztCharID != -1)
		{
			GameObject gameObject = GameObject.Find("CHAR_" + oS_.besetztCharID);
			if ((bool)gameObject)
			{
				charS_ = gameObject.GetComponent<characterScript>();
			}
		}
		if (!charS_)
		{
			return;
		}
		if (charS_.myID != oS_.besetztCharID)
		{
			charS_ = null;
		}
		else
		{
			if (!charS_.moveS_)
			{
				return;
			}
			if ((charS_.moveS_.waitForceAnimation <= 0f && roomS_.WERK_GameHasBestellungen()) || (charS_.moveS_.waitForceAnimation <= 0f && (bool)roomS_.GetTaskContractWork()))
			{
				if (!myAnimation.isPlaying)
				{
					myAnimation.Play();
					if ((bool)saegeblatt)
					{
						saegeblatt.GetComponent<Animation>().Play();
						if ((bool)saegeplattPartikel && !saegeplattPartikel.activeSelf)
						{
							saegeplattPartikel.SetActive(value: true);
						}
					}
				}
				animState.speed = mS_.gameSpeed;
			}
			else
			{
				animState.speed = 0f;
			}
		}
	}

	private void DisableAllChilds()
	{
		for (int i = 0; i < base.transform.childCount; i++)
		{
			if (base.transform.GetChild(i).gameObject.activeSelf)
			{
				base.transform.GetChild(i).gameObject.SetActive(value: false);
			}
		}
	}
}
