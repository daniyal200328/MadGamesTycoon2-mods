using UnityEngine;

public class animationSpeed : MonoBehaviour
{
	public string animationString;

	private Animation myAnimation;

	private mainScript mS_;

	private GameObject main_;

	private float oldGamespeed;

	private void Start()
	{
		myAnimation = GetComponent<Animation>();
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		SetAnimationSpeed();
	}

	private void Update()
	{
		if (oldGamespeed != mS_.GetGameSpeed())
		{
			oldGamespeed = mS_.GetGameSpeed();
			SetAnimationSpeed();
		}
	}

	private void SetAnimationSpeed()
	{
		myAnimation[animationString].speed = mS_.GetGameSpeed();
	}
}
