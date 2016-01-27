using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
public class InputSetup : MonoBehaviour 
{	
	int frm;

	void Update () 
	{
		if(EventSystem.current.currentSelectedGameObject != gameObject && frm > 2)
		{
			EventSystem.current.SetSelectedGameObject(gameObject);
			frm = 0;
		}
		else
			frm++;
			
	}
}
