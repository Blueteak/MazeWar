using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroScreen2 : MonoBehaviour {

	public GameObject introPanel;
	public Text NameLabel;

	void Update()
	{
		if(Input.GetKey(KeyCode.Delete))
			Application.Quit();
	}

	public void SubmitName(string name)
	{
		if(Input.GetButtonDown("Submit"))
		{
			Debug.Log("Name Selected: " + name);
			NameLabel.text = name;
			introPanel.SetActive(false);
        }
	}
}
