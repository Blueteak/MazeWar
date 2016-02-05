using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class ScoreSystem : MonoBehaviour {

	public Text Names;
	public Text Scores;

	List<PScore> scores;

	void Start() { scores = new List<PScore>(); }

	void Update()
	{
		UpdateScores();
	}

	void UpdateScores()
	{
		PlayerControl[] players = GameObject.FindObjectsOfType<PlayerControl>();
		if(players.Length > 0)
		{
			scores = new List<PScore>();
			foreach(var p in players)
			{
				PScore n = new PScore(p.name, p.score);
				scores.Add(n);
			}
			scores.Sort((x, y) => y.score.CompareTo(x.score));
			Names.text = "";
			Scores.text = "";
			foreach(var v in scores)
			{
				Names.text += v.name + "\n";
				Scores.text += v.score + "\n";
			}
		}
	}

}

[System.Serializable]
public class PScore
{
	public string name;
	public int score;

	public PScore(string n, int s)
	{
		name = n;
		score = s;
	}
}
