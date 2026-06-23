using Pathfinding;
using UnityEngine;

public class robotScript : MonoBehaviour
{
	public int stationID = -1;

	public GameObject ladestation;

	private GameObject main_;

	private mainScript mS_;

	private mapScript mapS_;

	private settingsScript settings_;

	private IAstarAI aStar;

	private Seeker seeker;

	public GameObject myTarget;

	private Animation cleanAnimation;

	public GameObject[] myObjects;

	private AudioSource sound;

	private bool isAufLadestation = true;

	private ParticleSystem particleSys;

	private ParticleSystem.EmissionModule emission;

	private AudioSource audioParticle;

	private float findTimer;

	private int trysToFindPath;

	private void Start()
	{
		FindScripts();
		InitPathfinding();
		particleSys = myObjects[1].GetComponent<ParticleSystem>();
		if ((bool)particleSys)
		{
			emission = particleSys.emission;
			emission.enabled = false;
		}
		audioParticle = myObjects[1].GetComponent<AudioSource>();
		if ((bool)audioParticle)
		{
			audioParticle.Stop();
		}
		mS_.findRobots = true;
	}

	private void OnDestroy()
	{
		if ((bool)mS_)
		{
			mS_.findRobots = true;
		}
	}

	private void FindScripts()
	{
		if (!sound)
		{
			sound = GetComponent<AudioSource>();
		}
		if (!cleanAnimation)
		{
			cleanAnimation = myObjects[0].GetComponent<Animation>();
		}
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
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

	private void InitPathfinding()
	{
		aStar = GetComponent<IAstarAI>();
	}

	public void UpdateMe()
	{
		FindTarget();
		if ((bool)myTarget)
		{
			Move();
			TargetReached();
		}
	}

	public void RecalculatePath()
	{
		aStar.SearchPath();
	}

	private void Move()
	{
		float num = 1.2f;
		base.transform.position = new Vector3(base.transform.position.x, 0f, base.transform.position.z);
		aStar.MovementUpdate(mS_.GetDeltaTime() * num, out var nextPosition, out var nextRotation);
		aStar.FinalizeMovement(nextPosition, nextRotation);
	}

	private void FindTarget()
	{
		findTimer += Time.deltaTime;
		if (findTimer < 1f)
		{
			return;
		}
		findTimer = 0f;
		if (cleanAnimation.isPlaying || aStar.pathPending)
		{
			return;
		}
		if (!aStar.hasPath && (bool)myTarget)
		{
			if (trysToFindPath > 10)
			{
				trysToFindPath = 0;
				base.transform.position = myTarget.transform.position;
			}
			else
			{
				aStar.destination = myTarget.transform.position;
				aStar.SearchPath();
				trysToFindPath++;
			}
		}
		else
		{
			if ((bool)myTarget)
			{
				return;
			}
			if (!ladestation)
			{
				GetLadestation();
			}
			if (mS_.arrayMuell.Length != 0 && (bool)ladestation)
			{
				int num = Mathf.RoundToInt(ladestation.transform.position.x);
				int num2 = Mathf.RoundToInt(ladestation.transform.position.z);
				float num3 = 100000000f;
				int num4 = -1;
				for (int i = 0; i < mS_.arrayMuell.Length; i++)
				{
					if (!mS_.arrayMuell[i] || !mS_.arrayMuell[i].CompareTag("Muell"))
					{
						continue;
					}
					int num5 = Mathf.RoundToInt(mS_.arrayMuell[i].transform.position.x);
					int num6 = Mathf.RoundToInt(mS_.arrayMuell[i].transform.position.z);
					if (mapS_.IsInMapLimit(num5, num6) && (mapS_.mapBuilding[num, num2] == mapS_.mapBuilding[num5, num6] || !mS_.personal_RobotDontLeaveBuilding))
					{
						float num7 = Vector3.Distance(base.transform.position, mS_.arrayMuell[i].transform.position);
						if (num3 > num7 && !IsAnotherRobotNaeher(num7, i, mapS_.mapBuilding[num5, num6]))
						{
							num3 = num7;
							num4 = i;
						}
					}
				}
				if (num4 != -1)
				{
					myTarget = mS_.arrayMuell[num4];
					myTarget.tag = "Muell_InUse";
					aStar.destination = myTarget.transform.position;
					aStar.SearchPath();
					if ((bool)particleSys)
					{
						emission.enabled = false;
					}
					if ((bool)audioParticle)
					{
						audioParticle.Stop();
					}
					if (!settings_.disableRobotSound)
					{
						sound.Play();
					}
					isAufLadestation = false;
					return;
				}
			}
			if (isAufLadestation)
			{
				return;
			}
			myTarget = GetLadestation();
			if ((bool)ladestation)
			{
				aStar.destination = myTarget.transform.position;
				aStar.SearchPath();
				if ((bool)particleSys)
				{
					emission.enabled = false;
				}
				if ((bool)audioParticle)
				{
					audioParticle.Stop();
				}
				if (!settings_.disableRobotSound)
				{
					sound.Play();
				}
			}
		}
	}

	public GameObject GetLadestation()
	{
		if ((bool)ladestation)
		{
			return ladestation;
		}
		ladestation = GameObject.Find("O_" + stationID);
		return ladestation;
	}

	private void TargetReached()
	{
		if (!myTarget || aStar.pathPending || (!((double)Vector3.Distance(base.transform.position, myTarget.transform.position) < 0.2) && !((double)aStar.remainingDistance < 0.2) && !aStar.reachedEndOfPath))
		{
			return;
		}
		if (myTarget.CompareTag("Muell_InUse"))
		{
			cleanAnimation.Play();
			if ((bool)particleSys)
			{
				emission.enabled = true;
			}
			if ((bool)audioParticle)
			{
				audioParticle.Play();
			}
			Object.Destroy(myTarget);
			myTarget = null;
		}
		else
		{
			myTarget = null;
			isAufLadestation = true;
			if ((bool)ladestation)
			{
				base.transform.position = ladestation.transform.position;
				base.transform.eulerAngles = new Vector3(ladestation.transform.eulerAngles.x, ladestation.transform.eulerAngles.y, ladestation.transform.eulerAngles.z + 180f);
			}
		}
	}

	public int GetPosition_RoomID()
	{
		return mapS_.mapRoomID[Mathf.RoundToInt(base.transform.position.x), Mathf.RoundToInt(base.transform.position.z)];
	}

	public int GetTargetPosition_RoomID()
	{
		if (!myTarget)
		{
			return -1;
		}
		return mapS_.mapRoomID[Mathf.RoundToInt(myTarget.transform.position.x), Mathf.RoundToInt(myTarget.transform.position.z)];
	}

	private bool IsAnotherRobotNaeher(float myDist, int slotMuell, int buildingID)
	{
		for (int i = 0; i < mS_.arrayRobots.Length; i++)
		{
			if (!mS_.arrayRobots[i] || !(mS_.arrayRobots[i] != base.gameObject) || !mS_.arrayRobotsScripts[i])
			{
				continue;
			}
			if (!mS_.arrayRobotsScripts[i].ladestation)
			{
				mS_.arrayRobotsScripts[i].GetLadestation();
			}
			int num = 0;
			int num2 = 0;
			if ((bool)mS_.arrayRobotsScripts[i].ladestation)
			{
				num = Mathf.RoundToInt(mS_.arrayRobotsScripts[i].ladestation.transform.position.x);
				num2 = Mathf.RoundToInt(mS_.arrayRobotsScripts[i].ladestation.transform.position.z);
			}
			if ((mapS_.mapBuilding[num, num2] == buildingID || !mS_.personal_RobotDontLeaveBuilding) && Vector3.Distance(mS_.arrayRobots[i].transform.position, mS_.arrayMuell[slotMuell].transform.position) < myDist)
			{
				robotScript component = mS_.arrayRobots[i].GetComponent<robotScript>();
				if ((bool)component && !component.myTarget)
				{
					return true;
				}
			}
		}
		return false;
	}
}
