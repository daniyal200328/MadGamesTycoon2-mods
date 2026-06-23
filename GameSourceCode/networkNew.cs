using Mirror;
using UnityEngine;

public class networkNew : NetworkManager
{
	public mpCalls mpCalls_;

	public GameObject main_;

	public mainScript mS_;

	public int num;

	public new static networkNew singleton { get; private set; }

	private void FindScripts()
	{
		if (!mpCalls_)
		{
			mpCalls_ = GetComponent<mpCalls>();
		}
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_ && (bool)main_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
	}

	public override void Awake()
	{
		base.Awake();
		singleton = this;
	}

	public override void OnServerConnect(NetworkConnectionToClient conn)
	{
		base.OnServerConnect(conn);
		int connectionId = conn.connectionId;
		Debug.Log("OnServerAddPlayer() -> ConnectionID: " + connectionId);
		FindScripts();
		if ((bool)mS_)
		{
			if (!mS_.mpLobbyOpen)
			{
				conn.Disconnect();
			}
			else
			{
				mpCalls_.AddPlayerNew(conn.connectionId);
			}
		}
	}

	public override void OnServerDisconnect(NetworkConnectionToClient conn)
	{
		base.OnServerDisconnect(conn);
		int connectionId = conn.connectionId;
		Debug.Log("OnStopServer() -> ConnectionID: " + connectionId);
		FindScripts();
		if ((bool)mpCalls_)
		{
			mpCalls_.RemovePlayerNew(conn.connectionId);
		}
	}
}
