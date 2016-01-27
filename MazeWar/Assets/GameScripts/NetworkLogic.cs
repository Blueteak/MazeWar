using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class NetworkLogic : NetworkManager 
{
	public string playerName;
	NetworkClient myClient;

	public GameObject playerPrefab;

	public void SetupServer () 
	{
		FindObjectOfType<MazeGenerator>().MazeSeed = 0; //Needs to change for all clients
        StartHost();
	}

	public void SetupClient (string IP) 
	{
		networkAddress = IP;
		StartClient(); 
	}

	public override void OnClientConnect (NetworkConnection conn)
	{
		base.OnClientConnect (conn);
		FindObjectOfType<MazeGenerator>().NewMaze();
	}

}
