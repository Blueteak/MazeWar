using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ServerSetup : MonoBehaviour {

	public string PostString;

	public GameObject IntroScreen;

	// Use this for initialization
	void Start () 
	{
		GetComponent<Text>().text += Network.player.ipAddress+"\n"+PostString+"\n";
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKey(KeyCode.Delete))
			Application.Quit();
	}

	public void SubmitName(string name)
	{
		if(Input.GetButtonDown("Submit"))
		{
			Debug.Log("Name Selected: " + name);
			FindObjectOfType<NetworkLogic>().playerName = name;
			FindObjectOfType<NetworkLogic>().SetupServer();
			gameObject.SetActive(false);
			IntroScreen.SetActive(false);
        }
	}
}
