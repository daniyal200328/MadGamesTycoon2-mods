using System.Collections;
using UnityEngine;

public class moCapChar : MonoBehaviour
{
	private Animator charAnimation;

	private float timer;

	private GameObject main_;

	private mainScript mS_;

	private clothScript clothScript_;

	public SkinnedMeshRenderer skin;

	public objectScript oS_;

	private roomScript roomS_;

	private mapScript mapS_;

	private Vector3 localPos;

	private bool hided = true;

	private void Start()
	{
		localPos = base.transform.localPosition;
		base.transform.localPosition = new Vector3(5000f, 5000f, 5000f);
		hided = true;
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!clothScript_)
		{
			clothScript_ = main_.GetComponent<clothScript>();
		}
		if (charAnimation == null)
		{
			charAnimation = GetComponent<Animator>();
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
		}
		skin.material = clothScript_.matColor_Skin[Random.Range(0, clothScript_.matColor_Skin.Length)];
	}

	private void Update()
	{
		if (!mS_)
		{
			return;
		}
		if (oS_.picked)
		{
			base.transform.localPosition = new Vector3(5000f, 5000f, 5000f);
			return;
		}
		int num = Mathf.RoundToInt(base.transform.root.transform.position.x);
		int num2 = Mathf.RoundToInt(base.transform.root.transform.position.z);
		if (!mapS_.IsInMapLimit(num, num2))
		{
			return;
		}
		roomS_ = mapS_.mapRoomScript[num, num2];
		if (!roomS_)
		{
			return;
		}
		if (roomS_.taskID == -1 || !oS_.inUse)
		{
			if (!hided)
			{
				hided = true;
				base.transform.parent.GetComponent<Animation>().Play("moCapCharScaleDown");
				StartCoroutine(RemoveChar());
			}
			return;
		}
		if (hided)
		{
			hided = false;
			base.transform.parent.GetComponent<Animation>().Play("moCapCharScaleUp");
			base.transform.localPosition = localPos;
		}
		if (!skin.isVisible)
		{
			return;
		}
		timer -= mS_.GetDeltaTime();
		charAnimation.speed = mS_.GetGameSpeed();
		if (!(timer > 0f))
		{
			timer = 3f;
			switch (Random.Range(0, 14))
			{
			case 0:
				charAnimation.CrossFade("UppercutRight", 0.1f, 0, 0f, 0.4f);
				break;
			case 1:
				charAnimation.CrossFade("IdleLHandPunch", 0.1f, 0, 0f, 0.4f);
				break;
			case 2:
				charAnimation.CrossFade("IdleFrontKick", 0.1f, 0, 0f, 0.4f);
				break;
			case 3:
				charAnimation.CrossFade("FlipKick", 0.1f, 0, 0f, 0.4f);
				break;
			case 4:
				charAnimation.CrossFade("Fight540RoundHouse", 0.1f, 0, 0f, 0.4f);
				break;
			case 5:
				charAnimation.CrossFade("ChargePunch", 0.1f, 0, 0f, 0.4f);
				break;
			case 6:
				charAnimation.CrossFade("Cast", 0.1f, 0, 0f, 0.4f);
				break;
			case 7:
				charAnimation.CrossFade("UseItem", 0.1f, 0, 0f, 0.4f);
				break;
			case 8:
				charAnimation.CrossFade("Katana45DegSwing", 0.1f, 0, 0f, 0.4f);
				break;
			case 9:
				charAnimation.CrossFade("Mutilate", 0.1f, 0, 0f, 0.4f);
				break;
			case 10:
				charAnimation.CrossFade("StaffHeal", 0.1f, 0, 0f, 0.4f);
				break;
			case 11:
				charAnimation.CrossFade("BasketballJumpShot", 0.1f, 0, 0f, 0.4f);
				break;
			case 12:
				charAnimation.CrossFade("Namaste", 0.1f, 0, 0f, 0.4f);
				break;
			case 13:
				charAnimation.CrossFade("Lunges", 0.1f, 0, 0f, 0.4f);
				break;
			}
		}
	}

	private IEnumerator RemoveChar()
	{
		yield return new WaitForSeconds(1f);
		base.transform.localPosition = new Vector3(5000f, 5000f, 5000f);
		skin.material = clothScript_.matColor_Skin[Random.Range(0, clothScript_.matColor_Skin.Length)];
	}
}
