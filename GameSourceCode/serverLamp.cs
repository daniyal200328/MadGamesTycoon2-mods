using UnityEngine;

public class serverLamp : MonoBehaviour
{
	public objectScript oS_;

	private mainScript mS_;

	private roomScript rS_;

	public GameObject[] goLamps;

	private Renderer[] goLamps_Renderer;

	public Material[] materials;

	private float timer;

	private void Start()
	{
		FindScripts();
		FindRenderer();
	}

	private void FindScripts()
	{
		if (!mS_)
		{
			mS_ = GameObject.FindWithTag("Main").GetComponent<mainScript>();
		}
		if (!oS_ && mS_.multiplayer && !oS_)
		{
			Object.Destroy(this);
		}
	}

	private void FindRenderer()
	{
		goLamps_Renderer = new Renderer[goLamps.Length];
		for (int i = 0; i < goLamps.Length; i++)
		{
			if ((bool)goLamps[i])
			{
				goLamps_Renderer[i] = goLamps[i].GetComponent<Renderer>();
			}
		}
	}

	private void Update()
	{
		timer += mS_.GetDeltaTime();
		if (timer < 0.1f)
		{
			return;
		}
		timer = 0f;
		if ((bool)goLamps_Renderer[0] && !goLamps_Renderer[0].isVisible)
		{
			return;
		}
		FindRoomScript();
		if (!rS_)
		{
			return;
		}
		if (rS_.serverDown)
		{
			for (int i = 0; i < goLamps.Length; i++)
			{
				if ((bool)goLamps[i] && (bool)goLamps_Renderer[i])
				{
					goLamps_Renderer[i].sharedMaterial = materials[1];
				}
			}
			return;
		}
		for (int j = 0; j < goLamps.Length; j++)
		{
			if ((bool)goLamps[j] && Random.Range(0, 100) > 80 && (bool)goLamps_Renderer[j])
			{
				goLamps_Renderer[j].sharedMaterial = materials[Random.Range(0, materials.Length)];
			}
		}
	}

	private void FindRoomScript()
	{
		if (!rS_ && (bool)mS_ && (bool)mS_.mapScript_)
		{
			int num = Mathf.RoundToInt(oS_.gameObject.transform.position.x);
			int num2 = Mathf.RoundToInt(oS_.gameObject.transform.position.z);
			roomScript roomScript2 = mS_.mapScript_.mapRoomScript[num, num2];
			if ((bool)roomScript2)
			{
				rS_ = roomScript2;
			}
		}
	}
}
