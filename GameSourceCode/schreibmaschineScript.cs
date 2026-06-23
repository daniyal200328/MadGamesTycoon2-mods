using UnityEngine;

public class schreibmaschineScript : MonoBehaviour
{
	public Animation myAnimation;

	private objectScript oS_;

	public MeshRenderer renderer;

	private GameObject main_;

	private roomScript roomS_;

	private mapScript mapS_;

	private mainScript mS_;

	private float oldGamespeed;

	private void Start()
	{
		FindScripts();
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
		if (!oS_)
		{
			FindScripts();
		}
		else
		{
			if (oS_.picked || !renderer.isVisible)
			{
				return;
			}
			roomS_ = mapS_.mapRoomScript[Mathf.RoundToInt(base.transform.root.transform.position.x), Mathf.RoundToInt(base.transform.root.transform.position.z)];
			if (!roomS_)
			{
				return;
			}
			if (oldGamespeed != mS_.GetGameSpeed())
			{
				oldGamespeed = mS_.GetGameSpeed();
				myAnimation["schreibmaschine1"].speed = mS_.GetGameSpeed();
			}
			if (roomS_.taskID == -1 || !oS_.inUse)
			{
				if (myAnimation.isPlaying)
				{
					myAnimation.Stop();
				}
			}
			else if (!myAnimation.isPlaying)
			{
				myAnimation.Play();
			}
		}
	}
}
