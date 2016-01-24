using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

 public class CaretStick:MonoBehaviour, ISelectHandler
 {
     private bool alreadyFixed;
     public float upp;
     public float rightt;
     
     public void OnSelect(BaseEventData eventData)
     {  	
		StartCoroutine("FixCaret");
      }

      IEnumerator FixCaret()
      {
      	yield return true;
      
        	string nm = gameObject.name+" Input Caret";
         	RectTransform caretRT = (RectTransform)transform.FindChild(nm);
         
         	Vector2 cpos = caretRT.anchoredPosition;
         
         	Debug.Log("here's the ap .. " +cpos);
         
			cpos.y = cpos.y + upp;
			cpos.x = cpos.x + rightt;
         
			caretRT.anchoredPosition = cpos;
      }
}