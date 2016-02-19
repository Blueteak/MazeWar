using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
public class ScoreSystem : MonoBehaviour {

	public GameObject nPrefab;
	public Transform holder;

	List<PScore> scores;

	List<GameObject> objs;

	public string CurrentView;

	void Start() { scores = new List<PScore>(); objs = new List<GameObject>();}

	void Update()
	{
		UpdateScores();
	}

	void UpdateScores()
	{
		if(GameObject.FindObjectsOfType<PlayerControl>().Length > 0)
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
				foreach(var v in objs)
					Destroy(v);
				objs = new List<GameObject>();
				foreach(var v in scores)
				{
					GameObject g = (GameObject)Instantiate(nPrefab);
					g.transform.SetParent(holder);
					g.transform.localScale = Vector3.one;
					bool V = CurrentView.Equals(v.name);
					g.GetComponent<NameObj>().Init(v.name, v.score, V);
					objs.Add(g);
				}
			}
		}
		else
		{
			SinglePlayerControl[] players = GameObject.FindObjectsOfType<SinglePlayerControl>();
			if(players.Length > 0)
			{
				scores = new List<PScore>();
				foreach(var p in players)
				{
					PScore n = new PScore(p.name, p.score);
					scores.Add(n);
				}
				scores.Sort((x, y) => y.score.CompareTo(x.score));
				foreach(var v in objs)
					Destroy(v);
				objs = new List<GameObject>();
				foreach(var v in scores)
				{
					GameObject g = (GameObject)Instantiate(nPrefab);
					g.transform.SetParent(holder);
					g.transform.localScale = Vector3.one;
					bool V = CurrentView.Equals(v.name);
					g.GetComponent<NameObj>().Init(v.name, v.score, V);
					objs.Add(g);
				}
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
