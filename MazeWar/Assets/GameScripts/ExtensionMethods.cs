using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ExtensionMethods {

	public static void Shuffle<T>(this IList<T> list, System.Random r)  
	{  
	    int n = list.Count;  
	    while (n > 1) {  
	        n--;  
	        int k = r.Next(0, n+1);//Random.Range(0,n + 1);  
	        T value = list[k];  
	        list[k] = list[n];  
	        list[n] = value;  
	    }  
	}
}
