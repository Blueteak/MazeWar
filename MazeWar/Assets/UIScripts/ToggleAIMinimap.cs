using UnityEngine;
using System.Collections;

public class ToggleAIMinimap : MonoBehaviour {

	public GameObject[] mapIcons;
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.M))
		{
			foreach(var v in mapIcons)
			{
				v.SetActive(!v.activeSelf);
			}
		}
	}
}
