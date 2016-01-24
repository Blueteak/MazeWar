using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class InputSetup : MonoBehaviour 
{	
	// Update is called once per frame
	void Update () 
	{
		if(EventSystem.current.currentSelectedGameObject != gameObject)
			EventSystem.current.SetSelectedGameObject(gameObject);
	}
}
