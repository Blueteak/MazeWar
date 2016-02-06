using UnityEngine;
using System.Collections;

public class ClosestSprite : MonoBehaviour {

	public Transform[] Sprites;

	
	// Update is called once per frame
	void Update () 
	{
		float[] D = new float[4];
		D[0] = Vector3.Distance(Sprites[0].transform.position, Camera.main.transform.position);
		D[1] = Vector3.Distance(Sprites[1].transform.position, Camera.main.transform.position);
		D[2] = Vector3.Distance(Sprites[2].transform.position, Camera.main.transform.position);
		D[3] = Vector3.Distance(Sprites[3].transform.position, Camera.main.transform.position);
		float min = D[0];
		int idxMin = 0;
		for(int i=0; i< 4; i++)
		{
			if(D[i] < min)
			{
				idxMin = i;
				min = D[i];
			}
		}
		for(int i=0; i<4 ;i++)
		{
			if(i != idxMin)
				Sprites[i].gameObject.SetActive(false);
			else
				Sprites[i].gameObject.SetActive(true);
		}
	}
}
