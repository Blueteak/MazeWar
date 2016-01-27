using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ClientSetup1 : MonoBehaviour 
{

	public GameObject CSetup2;

	void Update () 
	{
		if(Input.GetKey(KeyCode.Delete))
			Application.Quit();
	}

	public void SubmitIP(string ip)
	{
		if(Input.GetButtonDown("Submit"))
		{
			Debug.Log("IP Address Selected: " + ip);
			CSetup2.SetActive(true);
			CSetup2.GetComponent<ClientSetup2>().IP = ip;
			CSetup2.GetComponent<ClientSetup2>().Load();
			gameObject.SetActive(false);
        }
	}
}
