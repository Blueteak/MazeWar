using UnityEngine;
using System.Collections;

public class SinglePlayerStart : MonoBehaviour {

	public GameObject IntroScreen;
	public GameObject MazeGen;
	public GameObject SPlayer;

	public void Submit(string name)
	{
		if(Input.GetButtonDown("Submit"))
		{
			IntroScreen.SetActive(false);
			MazeGen.GetComponent<MazeGenerator>().MazeSeed = Random.Range(0, 1000);
			MazeGen.SetActive(true);
			SPlayer.SetActive(true);
			AIPlayer[] plrs = GameObject.FindObjectsOfType<AIPlayer>();
			foreach(var p in plrs)
			{
				p.Init();
			}
		}
	}
}
