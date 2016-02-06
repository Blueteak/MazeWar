using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NameObj : MonoBehaviour {

	public Text nt;
	public Text st;
	public Image ig;
	
	public void Init(string nm, int s, bool inView)
	{
		nt.text = nm;
		st.text = ""+s;
		if(inView)
		{
			nt.color = Color.white;
			st.color = Color.white;
			ig.color = Color.black;
		}
	}
}
