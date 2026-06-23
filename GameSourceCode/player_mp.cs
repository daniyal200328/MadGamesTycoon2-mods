using System;
using System.Collections.Generic;

[Serializable]
public class player_mp
{
	public publisherScript myPubScript_;

	public float timeout;

	public int playerID;

	public int connectionID;

	public string playerName;

	public long money;

	public int fans;

	public bool playerReady;

	public bool playerPause;

	public List<object_mp> objects;

	public int[,] mapRoomID;

	public int[,] mapRoomTyp;

	public int[,] mapDoors;

	public int[,] mapWindows;

	public bool ready;

	public bool[] forschungSonstiges;

	public bool[] genres;

	public bool[] themes;

	public bool[] engineFeatures;

	public bool[] gameplayFeatures;

	public bool[] hardware;

	public bool[] hardwareFeatures;

	public player_mp(int playerID_)
	{
		myPubScript_ = null;
		timeout = 0f;
		playerID = playerID_;
		connectionID = 0;
		playerName = "";
		money = 0L;
		fans = 0;
		playerReady = false;
		playerPause = false;
		objects = new List<object_mp>();
		mapRoomID = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		mapRoomTyp = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		mapDoors = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		mapWindows = new int[mapScript.mapSizeX, mapScript.mapSizeY];
		ready = false;
		forschungSonstiges = new bool[1];
		genres = new bool[1];
		themes = new bool[1];
		engineFeatures = new bool[1];
		gameplayFeatures = new bool[1];
		hardware = new bool[1];
		hardwareFeatures = new bool[1];
	}
}
