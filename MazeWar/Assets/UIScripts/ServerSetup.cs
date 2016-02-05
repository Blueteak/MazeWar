using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Net;
using System.Net.Sockets;

public class ServerSetup : MonoBehaviour {

	public string PostString;

	public GameObject IntroScreen;

	// Use this for initialization
	void Start () 
	{
		GetComponent<Text>().text += LocalIPAddress()+"\n"+PostString+"\n";
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
			FindObjectOfType<PlayerDetails>().name = name;
			FindObjectOfType<PlayerDetails>().hasName = true;
			FindObjectOfType<NetworkLogic>().playerName = name;
			FindObjectOfType<NetworkLogic>().SetupServer();
			gameObject.SetActive(false);
			IntroScreen.SetActive(false);
        }
	}

	public string LocalIPAddress()
    {
         IPHostEntry host;
         string localIP = "";
         host = Dns.GetHostEntry(Dns.GetHostName());
         foreach (IPAddress ip in host.AddressList)
         {
             if (ip.AddressFamily == AddressFamily.InterNetwork)
             {
                 localIP = ip.ToString();
                 break;
             }
         }
         return localIP;
    }
}
