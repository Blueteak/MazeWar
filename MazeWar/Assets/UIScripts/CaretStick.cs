using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

 public class CaretStick:MonoBehaviour, ISelectHandler
 {
     private bool alreadyFixed;
     public float upp;
     public float rightt;

     bool isFixed;
     
     public void OnSelect(BaseEventData eventData)
     {  	
     	if(!isFixed)
     	{
			StartCoroutine("FixCaret");
			isFixed = true;
     	}
			
     }

      IEnumerator FixCaret()
      {
      	yield return true;
      
        	string nm = gameObject.name+" Input Caret";
         	RectTransform caretRT = (RectTransform)transform.FindChild(nm);
         
         	Vector2 cpos = caretRT.anchoredPosition;
         
			cpos.y = cpos.y + upp;
			cpos.x = cpos.x + rightt;
         
			caretRT.anchoredPosition = cpos;
      }
}