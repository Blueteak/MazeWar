using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroScreen2 : MonoBehaviour {

	public GameObject introPanel;

	public GameObject SelectPanelServer;
	public GameObject SelectPanelClient;

	void Update()
	{
		if(Input.GetKey(KeyCode.Delete))
			Application.Quit();
	}

	public void SubmitChoice(string Choice)
	{
		if(Input.GetButtonDown("Submit"))
		{
			if(Choice.ToUpper().Equals("C"))
			{
				Debug.Log("Creating New Server");
				SelectPanelServer.SetActive(true);
				gameObject.SetActive(false);
			}
			else if(Choice.ToUpper().Equals("J"))
			{
				Debug.Log("Creating New Client");
				SelectPanelClient.SetActive(true);
				gameObject.SetActive(false);
			}

        }
	}

}
