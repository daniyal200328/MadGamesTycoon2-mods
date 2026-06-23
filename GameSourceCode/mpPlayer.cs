using Mirror;
using UnityEngine;

public class mpPlayer : NetworkBehaviour
{
	public mpCalls mpCalls_;

	public int playerID = -1;

	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	public override void OnStartServer()
	{
		Debug.Log("OnStartServer()");
		FindScripts();
	}

	public override void OnStopServer()
	{
		Debug.Log("OnStopServer()");
		FindScripts();
	}

	private void MirrorProcessed()
	{
	}
}
