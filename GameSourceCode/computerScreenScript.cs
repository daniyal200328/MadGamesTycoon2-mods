using UnityEngine;

public class computerScreenScript : MonoBehaviour
{
	public MeshRenderer myRenderer;

	public Material[] mat;

	public bool force;

	private float timer;

	private float rnd;

	private GameObject main_;

	private roomScript roomS_;

	private mapScript mapS_;

	private mainScript mS_;

	private objectScript oS_;

	private Transform myRoot;

	private int aktuellesMaterial = -1;

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
		if (!myRenderer)
		{
			myRenderer = base.gameObject.GetComponent<MeshRenderer>();
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
		if (!force)
		{
			if (!oS_)
			{
				FindScripts();
				return;
			}
			if (!oS_.enabled || oS_.picked || !myRenderer.isVisible)
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
				if (aktuellesMaterial != 0)
				{
					myRenderer.sharedMaterial = mat[0];
					aktuellesMaterial = 0;
				}
				return;
			}
			timer += mS_.GetDeltaTime();
		}
		else
		{
			timer += Time.deltaTime;
		}
		if (timer > rnd)
		{
			timer = 0f;
			rnd = Random.Range(0.5f, 1.5f);
			int num3 = Random.Range(1, mat.Length);
			if (aktuellesMaterial != num3)
			{
				myRenderer.sharedMaterial = mat[num3];
				aktuellesMaterial = num3;
			}
		}
	}
}
