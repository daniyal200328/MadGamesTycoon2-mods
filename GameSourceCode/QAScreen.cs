using UnityEngine;

public class QAScreen : MonoBehaviour
{
	public MeshRenderer renderer;

	public Material[] mat;

	private Material newMat;

	private float timer;

	private GameObject main_;

	private roomScript roomS_;

	private mapScript mapS_;

	private mainScript mS_;

	private objectScript oS_;

	private games games_;

	private Transform myRoot;

	private float timerFindRoom;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!myRoot)
		{
			myRoot = base.transform.root.transform;
		}
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
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
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
			if (!oS_.enabled || oS_.picked || !renderer.isVisible)
			{
				return;
			}
			if ((bool)myRoot)
			{
				timerFindRoom += Time.deltaTime;
				if (timerFindRoom > 1f)
				{
					timerFindRoom = 0f;
					int num = Mathf.RoundToInt(myRoot.position.x);
					int num2 = Mathf.RoundToInt(myRoot.position.z);
					if (!mapS_.IsInMapLimit(num, num2))
					{
						return;
					}
					roomS_ = mapS_.mapRoomScript[num, num2];
				}
			}
			if (!roomS_)
			{
				return;
			}
			if (roomS_.taskID == -1 || !oS_.inUse || roomS_.pause)
			{
				renderer.sharedMaterial = mat[0];
				return;
			}
			timer += mS_.GetDeltaTime();
			if (!(timer > 2f))
			{
				return;
			}
			timer = 0f;
			if (!newMat)
			{
				newMat = new Material(mat[0]);
			}
			if ((bool)roomS_.taskGameObject)
			{
				taskGameplayVerbessern taskGameplayVerbessern2 = roomS_.GetTaskGameplayVerbessern();
				if ((bool)taskGameplayVerbessern2 && (bool)taskGameplayVerbessern2.gS_ && (bool)newMat)
				{
					newMat.mainTexture = taskGameplayVerbessern2.gS_.GetScreenshotTexture2D();
					renderer.sharedMaterial = newMat;
					return;
				}
				taskBugfixing taskBugfixing2 = roomS_.GetTaskBugfixing();
				if ((bool)taskBugfixing2 && (bool)taskBugfixing2.gS_ && (bool)newMat)
				{
					newMat.mainTexture = taskBugfixing2.gS_.GetScreenshotTexture2D();
					renderer.sharedMaterial = newMat;
					return;
				}
				taskSpielbericht taskSpielbericht2 = roomS_.GetTaskSpielbericht();
				if ((bool)taskSpielbericht2 && (bool)taskSpielbericht2.gS_ && (bool)newMat)
				{
					newMat.mainTexture = taskSpielbericht2.gS_.GetScreenshotTexture2D();
					renderer.sharedMaterial = newMat;
					return;
				}
				taskPolishing taskPolishing2 = roomS_.GetTaskPolishing();
				if ((bool)taskPolishing2 && (bool)taskPolishing2.gS_ && (bool)newMat)
				{
					newMat.mainTexture = taskPolishing2.gS_.GetScreenshotTexture2D();
					renderer.sharedMaterial = newMat;
					return;
				}
			}
			newMat.mainTexture = games_.arrayGamesScripts[Random.Range(0, games_.arrayGamesScripts.Length)].GetScreenshotTexture2D();
			renderer.sharedMaterial = newMat;
		}
	}
}
