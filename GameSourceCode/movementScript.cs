using System.Collections;
using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

public class movementScript : MonoBehaviour
{
	public GameObject myTarget;

	public GameObject main_;

	public mainScript mS_;

	public GameObject charGFX;

	public characterScript cS_;

	public sfxScript sfx_;

	public clipScript clipS_;

	public mapScript mapS_;

	private IAstarAI aStar;

	public Animator charAnimation;

	public Animator charAnimationLOD;

	public int currentAnimation;

	private int audioTyping = -1;

	private Seeker seeker;

	public float waitForceAnimation;

	private float randomDeskEmoteTime;

	private float waitOnFloor;

	private float randomWaitDistance = 1f;

	private bool charArbeitsmarkt;

	private float movementTimer;

	private float movementGameSpeed;

	private float speed = 1f;

	private float walkDistance;

	public int trysToFindPath;

	private float BUFFER_distanceToObject = 9999999f;

	private objectScript BUFFER_object;

	private objectScript aktObjectToTest;

	private void Start()
	{
		randomDeskEmoteTime = Random.Range(5f, 30f);
		randomWaitDistance = Random.Range(1.5f, 2.5f);
		FindScripts();
		Init();
	}

	private void Init()
	{
		if (!cS_)
		{
			cS_ = GetComponent<characterScript>();
		}
		if (charGFX == null)
		{
			charGFX = base.transform.GetChild(0).gameObject;
		}
		if (charAnimation == null)
		{
			charAnimation = charGFX.GetComponent<Animator>();
		}
		InitPathfinding();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!clipS_)
		{
			clipS_ = main_.GetComponent<clipScript>();
		}
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
		}
		if (!seeker)
		{
			seeker = GetComponent<Seeker>();
		}
	}

	public void InitUpdate()
	{
		FindScripts();
		FindTarget();
		if ((bool)myTarget && Vector3.Distance(base.transform.position, myTarget.transform.position) < 2f)
		{
			TargetReached(teleport: true);
			SetAnimation();
			charAnimation.speed = 100f;
			charAnimation.Update(5f);
		}
	}

	public void UpdateMe()
	{
		if (!charArbeitsmarkt)
		{
			Move();
			FindTarget();
			SetAnimation();
		}
	}

	private void InitPathfinding()
	{
		if (aStar == null)
		{
			aStar = GetComponent<IAstarAI>();
		}
	}

	private void Move()
	{
		if (!cS_ || cS_.picked || waitForceAnimation > 0f || cS_.objectUsingID != -1 || mS_.GetGameSpeed() <= 0f)
		{
			return;
		}
		if ((bool)cS_.objectBelegtS_ && cS_.objectBelegtS_.checkWaitDistance && cS_.objectBelegtS_.inUse && Vector3.Distance(base.transform.position, cS_.objectBelegtS_.gameObject.transform.position) < randomWaitDistance)
		{
			waitOnFloor = 0.1f;
			return;
		}
		if (waitOnFloor > 0f)
		{
			waitOnFloor -= mS_.GetDeltaTime();
			return;
		}
		movementGameSpeed += mS_.GetDeltaTime();
		movementTimer += Time.deltaTime;
		if (cS_.IsVisible())
		{
			if (movementTimer < 0.016f)
			{
				return;
			}
		}
		else if (movementTimer < 0.05f)
		{
			return;
		}
		movementTimer = 0f;
		if (cS_.krank > 0 && (bool)cS_.objectBelegtS_ && !cS_.objectBelegtS_.isMedizinSchrank && FindObjectInRoom(11, null, onlyInRoom: false))
		{
			return;
		}
		if (speed < 2.9f)
		{
			if (cS_.perks[7])
			{
				if (cS_.krank > 0)
				{
					speed = 0.5f;
				}
				else
				{
					speed = 1.5f;
				}
			}
			else if (cS_.krank > 0)
			{
				speed = 0.5f;
			}
			else
			{
				speed = 1f;
			}
			if ((bool)cS_.objectBelegtS_ && (cS_.objectBelegtS_.isGhostWC || (cS_.objectBelegtS_.isWC && aStar.remainingDistance > 15f)))
			{
				speed = 3f;
			}
		}
		base.transform.position = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
		if ((bool)mS_)
		{
			aStar.MovementUpdate(movementGameSpeed * speed, out var nextPosition, out var nextRotation);
			aStar.FinalizeMovement(nextPosition, nextRotation);
		}
		movementGameSpeed = 0f;
		TargetReached(teleport: false);
	}

	public void SetAnimationForce(int hash, AnimationClip clip)
	{
		if ((bool)cS_ && (bool)cS_.myRenderer)
		{
			PlayAnimation(hash);
			waitForceAnimation = clip.length;
		}
	}

	private void SetAnimation()
	{
		if (!mS_ || !charAnimation)
		{
			return;
		}
		charAnimation.speed = mS_.GetGameSpeed();
		if ((bool)charAnimationLOD)
		{
			charAnimationLOD.speed = mS_.GetGameSpeed();
		}
		if (cS_.picked)
		{
			cS_.HideAddObjects();
			charAnimation.speed = 1f;
			if ((bool)charAnimationLOD)
			{
				charAnimationLOD.speed = 1f;
			}
			waitOnFloor = 0f;
			PlayAnimation(clipS_.hash_picked);
		}
		else if (waitForceAnimation > 0f)
		{
			waitForceAnimation -= mS_.GetDeltaTime();
			if (!(waitForceAnimation < 0f))
			{
				return;
			}
			cS_.HideAddObjects();
			if (currentAnimation == clipS_.hash_standDrink)
			{
				cS_.ResetDurst(erfuellt: true);
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_wc)
			{
				if ((bool)cS_.objectUsingS_)
				{
					cS_.objectUsingS_.gfxAnimation.Play("wcKabine1Auf");
				}
				cS_.ResetWC(erfuellt: true, cS_.objectUsingS_.motivationRegen);
				cS_.RemoveObjectUsing();
				DeleteTarget();
				FindObjectInRoom(5, null, onlyInRoom: true);
			}
			if (currentAnimation == clipS_.hash_sink)
			{
				if ((bool)cS_.objectUsingS_)
				{
					cS_.objectUsingS_.gfxShow.SetActive(value: false);
				}
				cS_.ResetWaschbecken(erfuellt: true, cS_.objectUsingS_.motivationRegen);
				cS_.RemoveObjectUsing();
				DeleteTarget();
				FindObjectInRoom(6, null, onlyInRoom: true);
			}
			if (currentAnimation == clipS_.hash_handtrockner)
			{
				if ((bool)cS_.objectUsingS_)
				{
					cS_.objectUsingS_.gfxShow.SetActive(value: false);
				}
				cS_.ResetMotivation(erfuellt: true, cS_.objectUsingS_.motivationRegen);
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_arcade)
			{
				if ((bool)cS_.objectUsingS_)
				{
					cS_.objectUsingS_.gfxShow.SetActive(value: false);
					cS_.objectUsingS_.gfxHide.SetActive(value: true);
					cS_.ResetMotivation(erfuellt: true, cS_.objectUsingS_.motivationRegen);
				}
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_motSit1)
			{
				if ((bool)cS_.objectUsingS_)
				{
					cS_.ResetMotivation(erfuellt: true, cS_.objectUsingS_.motivationRegen);
				}
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_dart)
			{
				if ((bool)cS_.objectUsingS_)
				{
					cS_.objectUsingS_.gfxShow.SetActive(value: false);
					cS_.ResetMotivation(erfuellt: true, cS_.objectUsingS_.motivationRegen);
				}
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_golf)
			{
				if ((bool)cS_.objectUsingS_)
				{
					cS_.objectUsingS_.gfxShow.SetActive(value: false);
					cS_.ResetMotivation(erfuellt: true, cS_.objectUsingS_.motivationRegen);
				}
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_laufband)
			{
				if ((bool)cS_.objectUsingS_)
				{
					cS_.objectUsingS_.gfxShow.SetActive(value: false);
					cS_.ResetMotivation(erfuellt: true, cS_.objectUsingS_.motivationRegen);
				}
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_piano)
			{
				if ((bool)cS_.objectUsingS_)
				{
					cS_.objectUsingS_.gfxShow.SetActive(value: false);
					cS_.ResetMotivation(erfuellt: true, cS_.objectUsingS_.motivationRegen);
				}
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_arztSchrank)
			{
				cS_.ResetKrank();
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_muell)
			{
				cS_.ResetMuell(erfuellt: true);
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_muellFloor)
			{
				cS_.ResetMuell(erfuellt: false);
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_waterCan)
			{
				cS_.ResetGiessen(erfuellt: true);
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_ghostPlant)
			{
				cS_.ResetGiessen(erfuellt: false);
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_ghostDrink)
			{
				cS_.ResetDurst(erfuellt: false);
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_ghostWC)
			{
				cS_.ResetWC(erfuellt: false, 0f);
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation == clipS_.hash_ghostSink)
			{
				cS_.ResetWaschbecken(erfuellt: false, 0f);
				cS_.RemoveObjectUsing();
				DeleteTarget();
			}
			if (currentAnimation != clipS_.hash_pause1 && currentAnimation != clipS_.hash_pause2 && currentAnimation != clipS_.hash_pause3 && currentAnimation != clipS_.hash_pause4 && currentAnimation != clipS_.hash_pauseSit1 && currentAnimation != clipS_.hash_freezer && currentAnimation != clipS_.hash_tv && currentAnimation != clipS_.hash_dance)
			{
				return;
			}
			if ((bool)cS_.objectUsingS_)
			{
				if ((bool)cS_.objectUsingS_.gfxShow)
				{
					cS_.objectUsingS_.gfxShow.SetActive(value: false);
				}
				if ((bool)cS_.objectUsingS_.gfxHide)
				{
					cS_.objectUsingS_.gfxHide.SetActive(value: true);
				}
			}
			cS_.ResetPause(erfuellt: true);
			cS_.RemoveObjectUsing();
			DeleteTarget();
			switch (Random.Range(0, 3))
			{
			case 0:
				if (!cS_.perks[13])
				{
					FindObjectInRoom(4, null, onlyInRoom: false);
				}
				break;
			case 1:
				FindObjectInRoom(1, null, onlyInRoom: false);
				break;
			}
		}
		else if (cS_.objectUsingID != -1)
		{
			if (cS_.IsVisible())
			{
				randomDeskEmoteTime -= mS_.GetDeltaTime();
				if ((double)randomDeskEmoteTime <= 0.0)
				{
					cS_.HideAddObjects();
					randomDeskEmoteTime = Random.Range(10f, 30f);
					if (cS_.roomS_.typ != 17)
					{
						switch (Random.Range(0, 8))
						{
						case 0:
							PlayAnimation(clipS_.hash_sitAngry2);
							waitForceAnimation = clipS_.clip_sitAngry2.length;
							break;
						case 1:
							PlayAnimation(clipS_.hash_sitGesture);
							waitForceAnimation = clipS_.clip_sitGesture.length;
							break;
						case 2:
							PlayAnimation(clipS_.hash_sitClap3);
							waitForceAnimation = clipS_.clip_sitClap3.length;
							break;
						case 3:
							PlayAnimation(clipS_.hash_sitFistPump);
							waitForceAnimation = clipS_.clip_sitFistPump.length;
							break;
						case 4:
							PlayAnimation(clipS_.hash_sitVictory);
							waitForceAnimation = clipS_.clip_sitVictory.length;
							break;
						case 5:
							PlayAnimation(clipS_.hash_sitDisbelief);
							waitForceAnimation = clipS_.clip_sitDisbelief.length;
							break;
						case 6:
							PlayAnimation(clipS_.hash_sitYell2);
							waitForceAnimation = clipS_.clip_sitYell2.length;
							break;
						case 7:
							PlayAnimation(clipS_.hash_sitCheer);
							waitForceAnimation = clipS_.clip_sitCheer.length;
							break;
						}
					}
					else
					{
						switch (Random.Range(0, 5))
						{
						case 0:
							PlayAnimation(clipS_.hash_yawn);
							waitForceAnimation = clipS_.clip_sitYawn.length;
							break;
						case 1:
							PlayAnimation(clipS_.hash_IdleArmGesture);
							waitForceAnimation = clipS_.clip_IdleArmGesture.length;
							break;
						case 2:
							PlayAnimation(clipS_.hash_IdleBriefCase);
							waitForceAnimation = clipS_.clip_IdleBriefCase.length;
							break;
						case 3:
							PlayAnimation(clipS_.hash_IdleMouthWipe);
							waitForceAnimation = clipS_.clip_IdleMouthWipe.length;
							break;
						case 4:
							PlayAnimation(clipS_.hash_IdleSandCover);
							waitForceAnimation = clipS_.clip_IdleSandCover.length;
							break;
						}
					}
					return;
				}
			}
			if (!cS_.roomS_)
			{
				return;
			}
			if (cS_.iDoWork)
			{
				switch (cS_.roomS_.typ)
				{
				case 1:
					PlayAnimation(clipS_.hash_sitTyping);
					break;
				case 2:
					PlayAnimation(clipS_.hash_sitTyping);
					break;
				case 6:
					PlayAnimation(clipS_.hash_sitTyping);
					break;
				case 7:
					PlayAnimation(clipS_.hash_sitTyping);
					break;
				case 3:
					PlayAnimation(clipS_.hash_gaming);
					cS_.ShowAddObject(6);
					break;
				case 4:
					PlayAnimation(clipS_.hash_writing);
					cS_.ShowAddObject(7);
					break;
				case 5:
					PlayAnimation(clipS_.hash_piano);
					break;
				case 10:
					PlayAnimation(clipS_.hash_sitTyping);
					break;
				case 13:
					PlayAnimation(clipS_.hash_training);
					cS_.ShowAddObject(3);
					break;
				case 17:
					PlayAnimation(clipS_.hash_prodArcade);
					cS_.ShowAddObject(8);
					break;
				case 8:
					PlayAnimation(clipS_.hash_writing);
					cS_.ShowAddObject(9);
					break;
				}
				if (cS_.IsVisible() && !cS_.hided && (audioTyping == -1 || (audioTyping > 0 && !sfx_.GetAudioSource(audioTyping).isPlaying)))
				{
					switch (Random.Range(0, 5))
					{
					case 0:
						sfx_.PlaySound(19, force: false);
						audioTyping = 19;
						break;
					case 1:
						sfx_.PlaySound(20, force: false);
						audioTyping = 20;
						break;
					case 2:
						sfx_.PlaySound(21, force: false);
						audioTyping = 21;
						break;
					case 3:
						sfx_.PlaySound(22, force: false);
						audioTyping = 22;
						break;
					case 4:
						sfx_.PlaySound(23, force: false);
						audioTyping = 23;
						break;
					}
				}
			}
			else
			{
				if (cS_.IsVisible())
				{
					cS_.HideAddObjects();
				}
				switch (cS_.roomS_.typ)
				{
				case 1:
					PlayAnimation(clipS_.hash_sitIdle);
					DisableAnim();
					break;
				case 2:
					PlayAnimation(clipS_.hash_sitIdle);
					DisableAnim();
					break;
				case 6:
					PlayAnimation(clipS_.hash_sitIdle);
					DisableAnim();
					break;
				case 7:
					PlayAnimation(clipS_.hash_sitIdle);
					DisableAnim();
					break;
				case 3:
					PlayAnimation(clipS_.hash_sitIdle);
					DisableAnim();
					break;
				case 4:
					PlayAnimation(clipS_.hash_sitIdle);
					DisableAnim();
					break;
				case 5:
					PlayAnimation(clipS_.hash_sitIdle);
					DisableAnim();
					break;
				case 10:
					PlayAnimation(clipS_.hash_sitIdle);
					DisableAnim();
					break;
				case 13:
					PlayAnimation(clipS_.hash_sitIdle);
					DisableAnim();
					break;
				case 17:
					PlayAnimation(clipS_.hash_idle);
					break;
				case 8:
					PlayAnimation(clipS_.hash_sitIdle);
					DisableAnim();
					break;
				case 9:
				case 11:
				case 12:
				case 14:
				case 15:
				case 16:
					break;
				}
			}
		}
		else
		{
			if (!cS_.IsVisible())
			{
				return;
			}
			if (waitOnFloor > 0f)
			{
				if (cS_.IsVisible())
				{
					cS_.HideAddObjects();
				}
				PlayAnimation(clipS_.hash_idle);
			}
			else
			{
				if (mS_.GetGameSpeed() <= 0f)
				{
					return;
				}
				if (Mathf.Abs(aStar.velocity.x) > 0f || Mathf.Abs(aStar.velocity.y) > 0f || Mathf.Abs(aStar.velocity.z) > 0f)
				{
					cS_.HideAddObjects();
					bool flag = false;
					if (speed > 1.5f)
					{
						flag = true;
					}
					if (!flag)
					{
						if (cS_.male)
						{
							if (cS_.krank > 0)
							{
								PlayAnimation(clipS_.hash_walk_krank);
							}
							else
							{
								PlayAnimation(clipS_.hash_walk_m);
							}
						}
						else if (cS_.krank > 0)
						{
							PlayAnimation(clipS_.hash_walk_krank);
						}
						else
						{
							PlayAnimation(clipS_.hash_walk_f);
						}
						if (cS_.krank == 0 && cS_.perks[7])
						{
							charAnimation.speed *= 1.5f;
							if ((bool)charAnimationLOD)
							{
								charAnimationLOD.speed *= 1.5f;
							}
						}
					}
					else
					{
						PlayAnimation(clipS_.hash_run);
					}
				}
				else
				{
					cS_.HideAddObjects();
					PlayAnimation(clipS_.hash_idle);
				}
			}
		}
	}

	private bool IsForceAnimation(int hash)
	{
		if (currentAnimation == hash && waitForceAnimation > 0f)
		{
			return true;
		}
		return false;
	}

	private void PlayAnimation(int hash)
	{
		if (currentAnimation != hash)
		{
			charAnimation.CrossFade(hash, 0.1f, 0, 0f, 0.4f);
			if ((bool)charAnimationLOD)
			{
				charAnimationLOD.CrossFade(hash, 0.1f, 0, 0f, 0.4f);
			}
			currentAnimation = hash;
			if (!charAnimation.enabled)
			{
				charAnimation.enabled = true;
			}
			if ((bool)charAnimationLOD && !charAnimationLOD.enabled)
			{
				charAnimationLOD.enabled = true;
			}
		}
	}

	private void DisableAnim()
	{
	}

	public void TargetReached(bool teleport)
	{
		if (!myTarget || (!teleport && aStar.pathPending))
		{
			return;
		}
		if (walkDistance <= 0f)
		{
			walkDistance = 0.2f;
			objectScript component = myTarget.GetComponent<objectScript>();
			if ((bool)component)
			{
				if (component.isGhost)
				{
					if (aStar.remainingDistance > 20f)
					{
						walkDistance = Random.Range(aStar.remainingDistance - 10f, aStar.remainingDistance - 15f);
					}
					else
					{
						walkDistance = Random.Range(0f, aStar.remainingDistance);
					}
				}
			}
			else if (myTarget.CompareTag("Floor"))
			{
				walkDistance = aStar.remainingDistance * 0.5f;
			}
		}
		if (!(Vector3.Distance(base.transform.position, myTarget.transform.position) < 0.2f) && !(aStar.remainingDistance < 0.2f) && !aStar.reachedEndOfPath && !(aStar.remainingDistance < walkDistance) && !teleport)
		{
			return;
		}
		walkDistance = -1f;
		speed = 1f;
		if (myTarget.CompareTag("Object"))
		{
			objectScript component2 = myTarget.GetComponent<objectScript>();
			Vector3 newPosition;
			Vector3 vEulerMale;
			if (cS_.male)
			{
				newPosition = component2.vPointMale;
				vEulerMale = component2.vEulerMale;
			}
			else
			{
				newPosition = component2.vPointFemale;
				vEulerMale = component2.vEulerMale;
			}
			if (component2.exitPointMale || component2.exitPointFemale)
			{
				aStar.Teleport(newPosition);
				base.transform.eulerAngles = new Vector3(0f, vEulerMale.y, -180f);
			}
			else
			{
				base.transform.LookAt(myTarget.transform);
				base.transform.eulerAngles = new Vector3(0f, base.transform.eulerAngles.y, -180f);
			}
			cS_.objectUsingID = component2.myID;
			cS_.objectUsingS_ = component2;
			if (component2.isArbeitsplatz)
			{
				randomDeskEmoteTime = Random.Range(5f, 15f);
			}
			if (component2.canDrink)
			{
				PlayAnimation(clipS_.hash_standDrink);
				waitForceAnimation = clipS_.clip_standDrink.length;
				cS_.ShowAddObject(0);
				if (cS_.IsVisible())
				{
					sfx_.Play3DSound(14, 1.09f, force: true, base.transform.position);
				}
			}
			if (component2.isGhostDrink)
			{
				PlayAnimation(clipS_.hash_ghostDrink);
				waitForceAnimation = clipS_.clip_ghostDrink.length;
				cS_.ShowAddObject(0);
			}
			if (component2.isMuelleimer)
			{
				PlayAnimation(clipS_.hash_muell);
				waitForceAnimation = clipS_.clip_muell.length;
				cS_.ShowAddObject(1);
				if (cS_.IsVisible())
				{
					sfx_.Play3DSound(17, 0.9f, force: true, base.transform.position);
				}
			}
			if (component2.isWC)
			{
				PlayAnimation(clipS_.hash_wc);
				waitForceAnimation = clipS_.clip_wc.length * 10f;
				if (cS_.IsVisible())
				{
					sfx_.Play3DSound(34, 11f, force: false, base.transform.position);
				}
				component2.gfxAnimation.Play("wcKabine1Zu");
			}
			if (component2.isSink)
			{
				PlayAnimation(clipS_.hash_sink);
				waitForceAnimation = clipS_.clip_sink.length;
				if ((bool)component2.gfxShow)
				{
					component2.gfxShow.SetActive(value: true);
				}
			}
			if (component2.isHandtrockner)
			{
				PlayAnimation(clipS_.hash_handtrockner);
				waitForceAnimation = clipS_.clip_handtrockner.length;
				if ((bool)component2.gfxShow)
				{
					component2.gfxShow.SetActive(value: true);
				}
			}
			if (component2.isArcade)
			{
				PlayAnimation(clipS_.hash_arcade);
				waitForceAnimation = clipS_.clip_arcade.length * 2f;
				if ((bool)component2.gfxShow)
				{
					component2.gfxShow.SetActive(value: true);
				}
				if ((bool)component2.gfxHide)
				{
					component2.gfxHide.SetActive(value: false);
				}
			}
			if (component2.isDart)
			{
				PlayAnimation(clipS_.hash_dart);
				waitForceAnimation = clipS_.clip_dart.length * 10f;
				cS_.ShowAddObject(5);
				if ((bool)component2.gfxShow)
				{
					component2.gfxShow.SetActive(value: true);
				}
			}
			if (component2.isMinigolf)
			{
				PlayAnimation(clipS_.hash_golf);
				waitForceAnimation = clipS_.clip_Minigolf.length * 10f;
				cS_.ShowAddObject(10);
				if ((bool)component2.gfxShow)
				{
					component2.gfxShow.SetActive(value: true);
				}
			}
			if (component2.isLaufband)
			{
				PlayAnimation(clipS_.hash_laufband);
				waitForceAnimation = clipS_.clip_Laufband.length * 33f;
				if ((bool)component2.gfxShow)
				{
					component2.gfxShow.SetActive(value: true);
				}
			}
			if (component2.isPiano)
			{
				PlayAnimation(clipS_.hash_piano);
				waitForceAnimation = clipS_.clip_piano.length * 2f;
				if ((bool)component2.gfxShow)
				{
					component2.gfxShow.SetActive(value: true);
				}
			}
			if (component2.isMedizinSchrank)
			{
				PlayAnimation(clipS_.hash_arztSchrank);
				waitForceAnimation = clipS_.clip_arztSchrank.length * 1f;
				if (cS_.IsVisible())
				{
					sfx_.Play3DSound(47, 0f, force: false, base.transform.position);
				}
				component2.gfxAnimation.Play("arztSchrank");
			}
			if (component2.isFreezer)
			{
				PlayAnimation(clipS_.hash_freezer);
				waitForceAnimation = clipS_.clip_freezer.length * 1f;
				if (cS_.IsVisible())
				{
					sfx_.Play3DSound(52, 0f, force: false, base.transform.position);
				}
				component2.gfxAnimation.Play("freezer1");
			}
			if (component2.isTV)
			{
				PlayAnimation(clipS_.hash_tv);
				waitForceAnimation = clipS_.clip_tv.length * 5f;
				if ((bool)component2.gfxShow)
				{
					component2.gfxShow.SetActive(value: true);
				}
				if ((bool)component2.gfxHide)
				{
					component2.gfxHide.SetActive(value: false);
				}
			}
			if (component2.isRadio)
			{
				PlayAnimation(clipS_.hash_dance);
				waitForceAnimation = clipS_.clip_dance.length * 5f;
				if ((bool)component2.gfxShow)
				{
					component2.gfxShow.SetActive(value: true);
				}
				if ((bool)component2.gfxHide)
				{
					component2.gfxHide.SetActive(value: false);
				}
			}
			if (component2.isGhostSink)
			{
				PlayAnimation(clipS_.hash_ghostSink);
				waitForceAnimation = clipS_.clip_ghostSink.length * 2f;
			}
			if (component2.isGhostMuelleimer)
			{
				PlayAnimation(clipS_.hash_muellFloor);
				waitForceAnimation = clipS_.clip_muellFloor.length;
				cS_.ShowAddObject(1);
				if (cS_.IsVisible())
				{
					sfx_.Play3DSound(18, 0.9f, force: true, base.transform.position);
				}
			}
			if (component2.isPlant)
			{
				PlayAnimation(clipS_.hash_waterCan);
				waitForceAnimation = clipS_.clip_waterCan.length;
				cS_.ShowAddObject(2);
				if (cS_.IsVisible())
				{
					sfx_.Play3DSound(26, 0f, force: true, base.transform.position);
				}
			}
			if (component2.isGhostPlant)
			{
				PlayAnimation(clipS_.hash_ghostPlant);
				waitForceAnimation = clipS_.clip_ghostPlant.length;
				cS_.ShowAddObject(2);
			}
			if (component2.isGhostWC)
			{
				PlayAnimation(clipS_.hash_ghostWC);
				waitForceAnimation = clipS_.clip_ghostWC.length * 2f;
			}
			if (component2.isGhostPause1)
			{
				PlayAnimation(clipS_.hash_pause1);
				waitForceAnimation = clipS_.clip_pause1.length * 1f;
			}
			if (component2.isGhostPause2)
			{
				PlayAnimation(clipS_.hash_pause2);
				waitForceAnimation = clipS_.clip_pause2.length * 1f;
			}
			if (component2.isGhostPause3)
			{
				PlayAnimation(clipS_.hash_pause3);
				waitForceAnimation = clipS_.clip_pause3.length * 9f;
				cS_.ShowAddObject(3);
			}
			if (component2.isGhostPause4)
			{
				PlayAnimation(clipS_.hash_pause4);
				waitForceAnimation = clipS_.clip_pause4.length * 4f;
				cS_.ShowAddObject(4);
			}
			if (component2.isSeat)
			{
				PlayAnimation(clipS_.hash_pauseSit1);
				waitForceAnimation = clipS_.clip_pauseSit1.length * 15f;
			}
			if (component2.isSeatAufenthalt)
			{
				PlayAnimation(clipS_.hash_motSit1);
				waitForceAnimation = clipS_.clip_motSit1.length * 15f;
			}
		}
		else if (waitOnFloor <= 0f)
		{
			waitOnFloor = Random.Range(3f, 6f);
		}
		DeleteTarget();
	}

	private bool HasScannedMap()
	{
		if (!mapS_)
		{
			return false;
		}
		if (!mapS_.aStar_)
		{
			return false;
		}
		if (mapS_.aStar_.lastScanTime <= 0f)
		{
			return false;
		}
		return true;
	}

	private void FindTarget()
	{
		if (!cS_ || cS_.objectUsingID != -1 || cS_.picked || !HasScannedMap() || aStar == null || (aStar != null && aStar.pathPending) || waitOnFloor > 0f)
		{
			return;
		}
		if (!aStar.hasPath && (bool)myTarget)
		{
			objectScript component = myTarget.GetComponent<objectScript>();
			if (trysToFindPath > 10)
			{
				Debug.Log("Teleport Char");
				trysToFindPath = 0;
				if ((bool)component)
				{
					if (component.existWaypoint)
					{
						base.transform.position = component.vWaypoint;
					}
				}
				else
				{
					base.transform.position = myTarget.transform.position;
				}
			}
			else if ((bool)component && component.existWaypoint)
			{
				aStar.destination = component.vWaypoint;
				aStar.SearchPath();
				trysToFindPath++;
				Debug.Log("Hat keinen Path, aber ein Target -> Also nochmal den Weg berechnen (mit Waypoint)");
			}
			else
			{
				aStar.destination = myTarget.transform.position;
				aStar.SearchPath();
				trysToFindPath++;
				Debug.Log("Hat keinen Path, aber ein Target -> Also nochmal den Weg berechnen (ohne Waypoint)");
			}
		}
		else
		{
			if ((bool)myTarget)
			{
				return;
			}
			if (cS_.roomID <= -1)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("Floor");
				if (array.Length == 0)
				{
					return;
				}
				int num = 0;
				while (num < 10000)
				{
					num++;
					myTarget = array[Random.Range(0, array.Length)];
					int num2 = Mathf.RoundToInt(myTarget.transform.position.x);
					int num3 = Mathf.RoundToInt(myTarget.transform.position.z);
					if (mapS_.IsInMapLimit(num2, num3) && mapS_.mapRoomID[num2, num3] == mapScript.ID_FLOOR && (mapS_.mapAusstattung[num2, num3] > 0f || mapS_.mapWaerme[num2, num3] > 0f))
					{
						aStar.destination = myTarget.transform.position;
						aStar.SearchPath();
						return;
					}
				}
				num = 0;
				while (num < 10000)
				{
					num++;
					myTarget = array[Random.Range(0, array.Length)];
					int num4 = Mathf.RoundToInt(myTarget.transform.position.x);
					int num5 = Mathf.RoundToInt(myTarget.transform.position.z);
					if (mapS_.IsInMapLimit(num4, num5) && mapS_.mapRoomID[num4, num5] == mapScript.ID_FLOOR)
					{
						aStar.destination = myTarget.transform.position;
						aStar.SearchPath();
						return;
					}
				}
				myTarget = array[Random.Range(0, array.Length)];
				aStar.destination = myTarget.transform.position;
				aStar.SearchPath();
			}
			else if (!cS_.roomS_ || !FindObjectInRoom(0, null, onlyInRoom: false))
			{
				FindRandomFloorInRoom();
			}
		}
	}

	public bool FindObjectInRoom(int what, GameObject forceObject, bool onlyInRoom)
	{
		Init();
		objectScript objectScript2 = null;
		GameObject gameObject = null;
		List<GameObject> list = new List<GameObject>();
		bool flag = false;
		if (!forceObject)
		{
			if (what == 0 && (bool)cS_.mainArbeitsplatzS_)
			{
				if (cS_.mainArbeitsplatzS_.besetztCharID == -1)
				{
					gameObject = cS_.mainArbeitsplatzS_.gameObject;
					objectScript2 = cS_.mainArbeitsplatzS_;
				}
				else
				{
					cS_.mainArbeitsplatzS_ = null;
				}
			}
			if ((what == 0 || what == 1 || what == 2 || what == 3 || what == 11 || what == 14 || what == 15) && !gameObject && (bool)cS_.roomS_)
			{
				for (int i = 0; i < cS_.roomS_.listInventar.Count; i++)
				{
					if (!cS_.roomS_.listInventar[i])
					{
						continue;
					}
					objectScript2 = cS_.roomS_.listInventar[i];
					if (objectScript2.picked || !objectScript2.gekauft)
					{
						continue;
					}
					switch (what)
					{
					case 0:
						if (objectScript2.isArbeitsplatz && objectScript2.IsUnbesetzt())
						{
							list.Add(cS_.roomS_.listInventar[i].gameObject);
						}
						break;
					case 1:
						if (objectScript2.canDrink && objectScript2.IsUnbesetzt() && objectScript2.HasAufladungen())
						{
							list.Add(cS_.roomS_.listInventar[i].gameObject);
						}
						break;
					case 2:
						if (objectScript2.isMuelleimer && objectScript2.IsUnbesetzt() && objectScript2.HasAufladungen())
						{
							list.Add(cS_.roomS_.listInventar[i].gameObject);
						}
						break;
					case 3:
						if (objectScript2.isPlant && objectScript2.IsUnbesetzt() && objectScript2.HasAufladungen())
						{
							list.Add(cS_.roomS_.listInventar[i].gameObject);
						}
						break;
					case 11:
						if (objectScript2.isMedizinSchrank && objectScript2.IsUnbesetzt() && objectScript2.HasAufladungen())
						{
							list.Add(cS_.roomS_.listInventar[i].gameObject);
						}
						break;
					case 14:
						if (objectScript2.isTV && objectScript2.IsUnbesetzt() && objectScript2.HasAufladungen())
						{
							list.Add(cS_.roomS_.listInventar[i].gameObject);
						}
						break;
					case 15:
						if (objectScript2.isRadio && objectScript2.IsUnbesetzt() && objectScript2.HasAufladungen())
						{
							list.Add(cS_.roomS_.listInventar[i].gameObject);
						}
						break;
					}
				}
			}
			int num = Mathf.RoundToInt(base.transform.position.x);
			int num2 = Mathf.RoundToInt(base.transform.position.z);
			if (!gameObject && mapS_.IsInMapLimit(num, num2))
			{
				roomScript roomScript2 = mapS_.mapRoomScript[num, num2];
				if ((bool)roomScript2 && roomScript2.typ != mapScript.ID_FLOOR)
				{
					List<objectScript> list2 = new List<objectScript>();
					switch (what)
					{
					case 1:
						list2 = new List<objectScript>(mS_.object_canDrink);
						break;
					case 2:
						list2 = new List<objectScript>(mS_.object_isMuelleimer);
						break;
					case 3:
						list2 = new List<objectScript>(mS_.object_isPlant);
						break;
					case 4:
						if (roomScript2.typ == 11)
						{
							list2 = new List<objectScript>(mS_.object_isWC);
						}
						break;
					case 5:
						if (roomScript2.typ == 11)
						{
							list2 = new List<objectScript>(mS_.object_isSink);
						}
						break;
					case 6:
						if (roomScript2.typ == 11)
						{
							list2 = new List<objectScript>(mS_.object_isHandtrockner);
						}
						break;
					case 7:
						list2 = new List<objectScript>(mS_.object_isSeat);
						break;
					case 8:
						if (roomScript2.typ == 12)
						{
							list2 = new List<objectScript>(mS_.object_isArcade);
						}
						break;
					case 9:
						if (roomScript2.typ == 12)
						{
							list2 = new List<objectScript>(mS_.object_isSeatAufenthalt);
						}
						break;
					case 10:
						if (roomScript2.typ == 12)
						{
							list2 = new List<objectScript>(mS_.object_isDart);
						}
						break;
					case 11:
						list2 = new List<objectScript>(mS_.object_isMedizinSchrank);
						break;
					case 12:
						if (roomScript2.typ == 12)
						{
							list2 = new List<objectScript>(mS_.object_isPiano);
						}
						break;
					case 13:
						if (roomScript2.typ == 12)
						{
							list2 = new List<objectScript>(mS_.object_isFreezer);
						}
						break;
					case 14:
						list2 = new List<objectScript>(mS_.object_isTV);
						break;
					case 15:
						list2 = new List<objectScript>(mS_.object_isRadio);
						break;
					case 16:
						if (roomScript2.typ == 12)
						{
							list2 = new List<objectScript>(mS_.object_isMinigolf);
						}
						break;
					case 17:
						if (roomScript2.typ == 12)
						{
							list2 = new List<objectScript>(mS_.object_Aufenthalsraum);
						}
						break;
					case 18:
						if (roomScript2.typ == 12)
						{
							list2 = new List<objectScript>(mS_.object_isLaufband);
						}
						break;
					}
					for (int j = 0; j < list2.Count; j++)
					{
						objectScript2 = list2[j];
						if ((bool)objectScript2 && objectScript2.GetRoomID() == roomScript2.myID && !objectScript2.picked && objectScript2.gekauft && objectScript2.IsUnbesetzt() && objectScript2.HasAufladungen())
						{
							list.Add(objectScript2.gameObject);
						}
					}
				}
			}
			num = Mathf.RoundToInt(base.transform.position.x);
			num2 = Mathf.RoundToInt(base.transform.position.z);
			if (!onlyInRoom && mapS_.IsInMapLimit(num, num2) && list.Count <= 0 && !gameObject)
			{
				flag = true;
				List<objectScript> list3 = new List<objectScript>();
				switch (what)
				{
				case 1:
					list3 = new List<objectScript>(mS_.object_canDrink);
					break;
				case 2:
					list3 = new List<objectScript>(mS_.object_isMuelleimer);
					break;
				case 3:
					list3 = new List<objectScript>(mS_.object_isPlant);
					break;
				case 4:
					list3 = new List<objectScript>(mS_.object_isWC);
					break;
				case 5:
					list3 = new List<objectScript>(mS_.object_isSink);
					break;
				case 6:
					list3 = new List<objectScript>(mS_.object_isHandtrockner);
					break;
				case 7:
					list3 = new List<objectScript>(mS_.object_isSeat);
					break;
				case 8:
					list3 = new List<objectScript>(mS_.object_isArcade);
					break;
				case 9:
					list3 = new List<objectScript>(mS_.object_isSeatAufenthalt);
					break;
				case 10:
					list3 = new List<objectScript>(mS_.object_isDart);
					break;
				case 11:
					list3 = new List<objectScript>(mS_.object_isMedizinSchrank);
					break;
				case 12:
					list3 = new List<objectScript>(mS_.object_isPiano);
					break;
				case 13:
					list3 = new List<objectScript>(mS_.object_isFreezer);
					break;
				case 14:
					list3 = new List<objectScript>(mS_.object_isTV);
					break;
				case 15:
					list3 = new List<objectScript>(mS_.object_isRadio);
					break;
				case 16:
					list3 = new List<objectScript>(mS_.object_isMinigolf);
					break;
				case 17:
					list3 = new List<objectScript>(mS_.object_Aufenthalsraum);
					break;
				case 18:
					list3 = new List<objectScript>(mS_.object_isLaufband);
					break;
				}
				for (int k = 0; k < list3.Count; k++)
				{
					objectScript2 = list3[k];
					if ((bool)objectScript2 && !objectScript2.picked && objectScript2.gekauft)
					{
						GameObject gameObject2 = objectScript2.gameObject;
						int num3 = Mathf.RoundToInt(gameObject2.transform.position.x);
						int num4 = Mathf.RoundToInt(gameObject2.transform.position.z);
						if (mapS_.IsInMapLimit(num3, num4) && mapS_.mapBuilding[num, num2] == mapS_.mapBuilding[num3, num4] && objectScript2.IsUnbesetzt() && objectScript2.HasAufladungen())
						{
							list.Add(gameObject2);
						}
					}
				}
			}
			if (!onlyInRoom && !mS_.personal_dontLeaveBuilding && list.Count <= 0 && !gameObject)
			{
				flag = true;
				List<objectScript> list4 = new List<objectScript>();
				switch (what)
				{
				case 1:
					list4 = new List<objectScript>(mS_.object_canDrink);
					break;
				case 2:
					list4 = new List<objectScript>(mS_.object_isMuelleimer);
					break;
				case 3:
					list4 = new List<objectScript>(mS_.object_isPlant);
					break;
				case 4:
					list4 = new List<objectScript>(mS_.object_isWC);
					break;
				case 5:
					list4 = new List<objectScript>(mS_.object_isSink);
					break;
				case 6:
					list4 = new List<objectScript>(mS_.object_isHandtrockner);
					break;
				case 7:
					list4 = new List<objectScript>(mS_.object_isSeat);
					break;
				case 8:
					list4 = new List<objectScript>(mS_.object_isArcade);
					break;
				case 9:
					list4 = new List<objectScript>(mS_.object_isSeatAufenthalt);
					break;
				case 10:
					list4 = new List<objectScript>(mS_.object_isDart);
					break;
				case 11:
					list4 = new List<objectScript>(mS_.object_isMedizinSchrank);
					break;
				case 12:
					list4 = new List<objectScript>(mS_.object_isPiano);
					break;
				case 13:
					list4 = new List<objectScript>(mS_.object_isFreezer);
					break;
				case 14:
					list4 = new List<objectScript>(mS_.object_isTV);
					break;
				case 15:
					list4 = new List<objectScript>(mS_.object_isRadio);
					break;
				case 16:
					list4 = new List<objectScript>(mS_.object_isMinigolf);
					break;
				case 17:
					list4 = new List<objectScript>(mS_.object_Aufenthalsraum);
					break;
				case 18:
					list4 = new List<objectScript>(mS_.object_isLaufband);
					break;
				}
				for (int l = 0; l < list4.Count; l++)
				{
					objectScript2 = list4[l];
					if ((bool)objectScript2 && !objectScript2.picked && objectScript2.gekauft)
					{
						GameObject item = objectScript2.gameObject;
						if (objectScript2.IsUnbesetzt() && objectScript2.HasAufladungen())
						{
							list.Add(item);
						}
					}
				}
			}
			if (list.Count > 0)
			{
				gameObject = ShortestWay(list);
				objectScript2 = gameObject.GetComponent<objectScript>();
				if (what == 0)
				{
					cS_.mainArbeitsplatzS_ = objectScript2;
				}
				else if ((bool)mS_ && (bool)mS_.settings_ && flag)
				{
					StartCoroutine(iShortestWay(list));
				}
			}
		}
		else
		{
			gameObject = forceObject;
			objectScript2 = forceObject.GetComponent<objectScript>();
		}
		if ((bool)gameObject)
		{
			cS_.RemoveObjectUsing();
			cS_.objectBelegtID = objectScript2.myID;
			cS_.objectBelegtS_ = objectScript2;
			objectScript2.SetBesetzt(cS_.myID);
			myTarget = gameObject;
			aStar.destination = objectScript2.vWaypoint;
			if (HasScannedMap())
			{
				aStar.SearchPath();
			}
			return true;
		}
		return false;
	}

	public IEnumerator iShortestWay(List<GameObject> objects)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		BUFFER_distanceToObject = 9999999f;
		BUFFER_object = null;
		aktObjectToTest = null;
		for (int i = 0; i < objects.Count; i++)
		{
			if ((bool)objects[i])
			{
				objectScript component = objects[i].GetComponent<objectScript>();
				if (!component.picked && component.gekauft && (component.IsUnbesetzt() || component.besetztCharID == cS_.myID) && component.HasAufladungen())
				{
					aktObjectToTest = component;
					Path path = seeker.StartPath(base.transform.position, component.vWaypoint, OnPathComplete);
					yield return StartCoroutine(path.WaitForPath());
				}
			}
		}
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if ((bool)BUFFER_object)
		{
			aStar.destination = BUFFER_object.vWaypoint;
			aStar.SearchPath();
		}
		else if ((bool)cS_.objectBelegtS_)
		{
			aStar.destination = cS_.objectBelegtS_.vWaypoint;
			aStar.SearchPath();
		}
	}

	public void OnPathComplete(Path p)
	{
		if (!p.error && (float)p.vectorPath.Count < BUFFER_distanceToObject + 20f && (BUFFER_distanceToObject > 99999f || Random.Range(0, 100) > 50) && (bool)aktObjectToTest && !aktObjectToTest.picked && aktObjectToTest.gekauft && (aktObjectToTest.IsUnbesetzt() || aktObjectToTest.besetztCharID == cS_.myID) && aktObjectToTest.HasAufladungen())
		{
			if (BUFFER_distanceToObject > (float)p.vectorPath.Count)
			{
				BUFFER_distanceToObject = p.vectorPath.Count;
			}
			BUFFER_object = aktObjectToTest;
			cS_.RemoveObjectUsing();
			cS_.objectBelegtID = BUFFER_object.myID;
			cS_.objectBelegtS_ = BUFFER_object;
			BUFFER_object.SetBesetzt(cS_.myID);
			myTarget = BUFFER_object.gameObject;
		}
	}

	public void FindRandomFloorInRoom()
	{
		if (!cS_.roomS_)
		{
			return;
		}
		int num = 0;
		if (cS_.roomS_.listGameObjects.Count > 0)
		{
			num = Random.Range(0, cS_.roomS_.listGameObjects.Count);
			if ((bool)cS_.roomS_.listGameObjects[num])
			{
				cS_.RemoveObjectUsing();
				myTarget = cS_.roomS_.listGameObjects[num];
				aStar.destination = myTarget.transform.position;
				aStar.SearchPath();
			}
		}
	}

	private GameObject ShortestWay(List<GameObject> objects)
	{
		int index = 0;
		float num = 9999999f;
		for (int i = 0; i < objects.Count; i++)
		{
			if ((bool)objects[i])
			{
				float num2 = Vector3.Distance(base.transform.position, objects[i].transform.position);
				if (num2 < num)
				{
					num = num2;
					index = i;
				}
			}
		}
		return objects[index];
	}

	public void RecalculatePath()
	{
		aStar.SearchPath();
	}

	public int GetPosition_RoomID()
	{
		int num = Mathf.RoundToInt(base.transform.position.x);
		int num2 = Mathf.RoundToInt(base.transform.position.z);
		if ((bool)mapS_ && mapS_.IsInMapLimit(num, num2))
		{
			return mapS_.mapRoomID[num, num2];
		}
		return -1;
	}

	public int GetTargetPosition_RoomID()
	{
		if (!myTarget)
		{
			return -1;
		}
		int num = Mathf.RoundToInt(myTarget.transform.position.x);
		int num2 = Mathf.RoundToInt(myTarget.transform.position.z);
		if ((bool)mapS_ && mapS_.IsInMapLimit(num, num2))
		{
			return mapS_.mapRoomID[num, num2];
		}
		return -1;
	}

	public int GetPosition_BuildingID()
	{
		int num = Mathf.RoundToInt(base.transform.position.x);
		int num2 = Mathf.RoundToInt(base.transform.position.z);
		if ((bool)mapS_ && mapS_.IsInMapLimit(num, num2))
		{
			return mapS_.mapBuilding[num, num2];
		}
		return -1;
	}

	public int GetTargetPosition_BuildingID()
	{
		if (!myTarget)
		{
			return -1;
		}
		int num = Mathf.RoundToInt(myTarget.transform.position.x);
		int num2 = Mathf.RoundToInt(myTarget.transform.position.z);
		if ((bool)mapS_ && mapS_.IsInMapLimit(num, num2))
		{
			return mapS_.mapBuilding[num, num2];
		}
		return -1;
	}

	public void DeleteTarget()
	{
		myTarget = null;
	}
}
