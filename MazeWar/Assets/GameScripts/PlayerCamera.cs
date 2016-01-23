using UnityEngine;
using System.Collections;

public class PlayerCamera : MonoBehaviour {

	RectTransform c;
	RectTransform r;
	public Vector2 dim;
	public Vector2 offset;

	// Use this for initialization
	void Start () 
	{
		c = FindObjectOfType<Canvas>().GetComponent<RectTransform>();
		r = GameObject.FindGameObjectWithTag("PlayerWindow").GetComponent<RectTransform>();
		dim = new Vector2(r.sizeDelta.x/c.rect.width, r.sizeDelta.y/c.rect.height);
		offset = new Vector2(((c.rect.width-r.sizeDelta.x)/2)/c.rect.width,(c.rect.height-(c.rect.height*0.0247f)-r.sizeDelta.y)/c.rect.height);
		GetComponent<Camera>().rect = new Rect(offset.x, offset.y, dim.x, dim.y);
		dim = new Vector2(dim.x*c.rect.width, dim.y*c.rect.height);
		offset = new Vector2(offset.x*c.rect.width, offset.y*c.rect.height);
	}


}
