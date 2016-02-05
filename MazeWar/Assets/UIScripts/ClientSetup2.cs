using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ClientSetup2 : MonoBehaviour {

	public string IP;

	public string PostStr;

	public GameObject IntroScreen;


	public void Load()
	{
		GetComponent<Text>().text += IP + "\n"+PostStr;
	}

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
			FindObjectOfType<NetworkLogic>().SetupClient(IP);
			FindObjectOfType<NetworkLogic>().playerName = name;
			gameObject.SetActive(false);
			IntroScreen.SetActive(false);
        }
	}
}
