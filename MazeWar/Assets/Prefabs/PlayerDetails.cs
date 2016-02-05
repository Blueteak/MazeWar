using UnityEngine;
using System.Collections;

public class PlayerDetails : MonoBehaviour {

	public string name;
	public bool hasName;
	bool setName;

	void Update()
	{
		if(GameObject.FindGameObjectWithTag("MyPlayer") != null && !setName && hasName)
		{
			setName = true;
			GameObject.FindGameObjectWithTag("MyPlayer").GetComponent<PlayerControl>().CmdSetName(name);
		}
	}
}
