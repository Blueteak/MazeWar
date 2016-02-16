using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AIPlayer : MonoBehaviour {

	bool online;
	Maze m;
	SinglePlayerControl control;

	IntVector wantPos;
	Queue<Direction> MoveQ;
	bool hasPath;

	public float MoveSpeed;
	float mCD;
	public float ReflexSpeed;
	float rCD;


	public void Init()
	{
		online = true;
		m = GameObject.FindObjectOfType<MazeGenerator>().currentMaze();
		control = GetComponent<SinglePlayerControl>();
		control.RandomPlacement();
		MoveQ = new Queue<Direction>();
		wantPos = randomPos();
	}

	IntVector randomPos()
	{
		IntVector nVec = new IntVector(Random.Range(0,(int)m.Dimensions().x), Random.Range(0, (int)m.Dimensions().y));
		while(m.GetCell(nVec.x, nVec.y).isWall)
			nVec = new IntVector(Random.Range(0,(int)m.Dimensions().x), Random.Range(0, (int)m.Dimensions().y));
		return nVec;
	}

	void Update()
	{
		CheckShoot();
		if(!hasPath)
		{
			GetPath();
		}
		else
		{
			MoveOnPath();	
		}
	}

	void GetPath()
	{
		//Get path for Movement
		hasPath = true;
	}

	void MoveOnPath()
	{
		if(MoveQ.Count > 0)
		{
			Direction d = MoveQ.Dequeue();
			if(d == Direction.up)
				control.CmdDoMovement(true, false ,false, false);
			else if(d == Direction.down)
				control.CmdDoMovement(false, true ,false, false);
			else if(d == Direction.left)
				control.CmdDoMovement(false, false ,true, false);
			else
				control.CmdDoMovement(false, false ,false, true);
		}
		else
			hasPath = false;
	}

	void CheckShoot()
	{
		if(true) //Check Raycast
		{
			rCD -= Time.deltaTime;
			if(rCD <= 0)
			{
				control.CmdShoot();
				rCD = ReflexSpeed;
			}
		}
		else
			rCD = ReflexSpeed;
	}


}

[System.Serializable]
public class IntVector
{
	public int x;
	public int y;

	public IntVector(int X, int Y)
	{
		x = X;
		y = Y;
	}
}

public enum Direction
{
	up, down, left, right
}
