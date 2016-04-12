using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using System.IO;

public class AIPlayer : MonoBehaviour {

	bool online;
	Maze m;
	SinglePlayerControl control;

	bool hasPath;

	public float MoveSpeed;
	float mCD;
	public float ReflexSpeed;
	float rCD;

	public TextAsset t;
	string s;


	public void Init()
	{
		control = GetComponent<SinglePlayerControl>();
		control.enabled = true;
		control.RandomPlacement();
		MoveQ = new Stack<Direction>();
		StartCoroutine("afterFrame");
	}

	IEnumerator afterFrame()
	{
		yield return true;
		online = true;
		m = GameObject.FindObjectOfType<MazeGenerator>().currentMaze();
		wantPos = randomPos();
	}

	IntVector randomPos()
	{
		IntVector nVec = new IntVector(UnityEngine.Random.Range(0,(int)m.Dimensions().x), UnityEngine.Random.Range(0, (int)m.Dimensions().y));
		while(m.GetCell(nVec.x, nVec.y).isWall)
			nVec = new IntVector(UnityEngine.Random.Range(0,(int)m.Dimensions().x), UnityEngine.Random.Range(0, (int)m.Dimensions().y));
		return nVec;
	}

	void Update()
	{
		if(online && m != null)
		{
			CheckShoot();
			if(!hasPath || control.justDied)
			{
				GetPath();
				following = false;
				control.justDied = false;
			}
			else
			{
				MoveOnPath();	
			}
		}
	}

	bool[,] visited;
	int[,] weights;
	public IntVector wantPos;
	public Stack<Direction> MoveQ;

	bool printed = false;

	void GetPath()
	{
		s += "Getting Path to: " + wantPos.x + " , " + wantPos.y + "\n\n";
		MoveQ = new Stack<Direction>();
		visited = new bool[(int)m.Dimensions().y, (int)m.Dimensions().x];
		weights = new int[(int)m.Dimensions().y, (int)m.Dimensions().x];
		for(int y = weights.GetLength(0)-1; y >= 0; y--)
		{
			for(int x = 0; x < weights.GetLength(1); x++)
			{
				if(m.GetCell(x,y).isWall)
				{
					weights[y,x] = -1;
					visited[y,x] = true;
				}
				else
				{
					weights[y,x] = int.MaxValue;
					visited[y,x] = false;
				}
					
			}
		}
		recursiveSolve((int)control.pos().x, (int)control.pos().y, 0);
		if(!printed)
		{
			Debug.Log("AI Weight Path: " + gameObject.name);
			printed = true;
			for(int y = weights.GetLength(0)-1; y >= 0; y--)
			{
				string mapLine = "";
				for(int x = 0; x < weights.GetLength(1); x++)
				{
					if(x == wantPos.x && y == wantPos.y)
					{
						mapLine += "X  ";
					}
					else if(weights[y,x] != -1)
					{
						if(weights[y,x] > 9)
							mapLine += weights[y,x] + " ";
						else
							mapLine += weights[y,x] + "  ";
					}
					else
						mapLine += "#  ";
				}
				s += mapLine+"\n";
			}
		}
		GetPathAStar();
		/*
		File.WriteAllText(Application.dataPath + "/AItest.text", s);
        AssetDatabase.Refresh();
        */
        s = "";
		hasPath = true;
	}

	void recursiveSolve(int x, int y, int curStep)
	{
		weights[y,x] = curStep;
		if(weights[y+1,x] != -1 && weights[y+1,x] > curStep)
			recursiveSolve(x, y+1, curStep+1);
		if(weights[y-1,x] != -1 && weights[y-1,x] > curStep)
			recursiveSolve(x, y-1, curStep+1);
		if(weights[y,x-1] != -1 && weights[y,x-1] > curStep)
			recursiveSolve(x-1, y, curStep+1);
		if(weights[y,x+1] != -1 && weights[y,x+1] > curStep)
			recursiveSolve(x+1, y, curStep+1);

	}

	void GetPathAStar()
	{
		IntVector curPos = wantPos;
		IntVector startP = new IntVector((int)control.pos().x, (int)control.pos().y);
		s += "\nRead top to bottom\n\nPath To: " + curPos.x + "," +curPos.y+"\n"; 
		int steps = 0;
		while(weights[curPos.y, curPos.x] != 0 && steps < 150)
		{
			int curWeight = weights[curPos.y, curPos.x];
			Direction d = Direction.wait;
			int mw = int.MaxValue;
			int mx = 0; int my = 0;
			if(curPos.y < weights.GetLength(0)-1 && weights[curPos.y+1, curPos.x] != -1 && !visited[curPos.y+1, curPos.x] && weights[curPos.y+1, curPos.x] <= mw)
			{
				d = Direction.down;
				mw = weights[curPos.y+1, curPos.x];
				mx = curPos.y+1; my = curPos.x;
			}
			if(curPos.y > 0 && weights[curPos.y-1, curPos.x] <= mw && weights[curPos.y-1, curPos.x] != -1 && !visited[curPos.y-1, curPos.x])
			{
				mw = weights[curPos.y -1, curPos.x]; mx = curPos.y-1; my = curPos.x;
				d = Direction.up;
			}
			if(curPos.x > 0 && weights[curPos.y, curPos.x-1] <= mw && weights[curPos.y, curPos.x-1] != -1 && !visited[curPos.y, curPos.x-1])
			{
				mw = weights[curPos.y, curPos.x-1]; mx = curPos.y; my = curPos.x-1;
				d = Direction.right;
			}
			if(curPos.x < weights.GetLength(1)-1 && weights[curPos.y, curPos.x+1] <= mw && weights[curPos.y, curPos.x+1] != -1 && !visited[curPos.y, curPos.x+1])
			{
				mw = weights[curPos.y, curPos.x+1]; mx = curPos.y; my = curPos.x+1;
				d = Direction.left;
			}
			if(d != Direction.wait)
			{
				s += "\n"+steps+ " : " + d + " to " + my + "," + mx + " -- Weight: " + weights[mx, my];
				MoveQ.Push(d);
				visited[curPos.y, curPos.x] = true;
				curPos.y = mx; curPos.x = my;
			}
			steps++;
		}
		s+= "\n\nFrom: " + startP.x + "," +startP.y;
	}


	/*
	bool recursiveSolve(int x, int y)
	{
		if (x == wantPos.x && y == wantPos.y) return true;
		if (m.GetCell(x,y).isWall || visited[x,y]) return false;
		visited[x,y] = true;
		if (x > 0){ // Checks if not on left edge
	        if (recursiveSolve(x-1, y)) { // Recalls method one to the left
	            MoveQ.Push(Direction.left);
	            if(Random.Range(0, 100) < 15)
	            	MoveQ.Push(Direction.wait);
	            return true;
	        }
        }
	    if (x < m.Dimensions().x - 1){ // Checks if not on right edge
	        if (recursiveSolve(x+1, y)) { // Recalls method one to the right
	            //correctPath[x][y] = true;
				MoveQ.Push(Direction.right);
				if(Random.Range(0, 100) < 15)
	            	MoveQ.Push(Direction.wait);
	            return true;
	        }
	    }
	    if (y > 0){  // Checks if not on top edge
	        if (recursiveSolve(x, y-1)) { // Recalls method one up
	            //correctPath[x][y] = true;
				MoveQ.Push(Direction.down);
				if(Random.Range(0, 100) < 15)
	            	MoveQ.Push(Direction.wait);
	            return true;
	        }
	    }
	    if (y < m.Dimensions().y - 1){ // Checks if not on bottom edge
	        if (recursiveSolve(x, y+1)) { // Recalls method one down
	            //correctPath[x][y] = true;
				MoveQ.Push(Direction.up);
				if(Random.Range(0, 100) < 15)
	            	MoveQ.Push(Direction.wait);
	            return true;
	        }
	    }
		return false;
	}
	*/

	Direction interm = Direction.wait;

	void MoveOnPath()
	{
		if(mCD <= 0)
		{	
			if(interm == Direction.up)
			{
				//Debug.Log("Moving Forward");
				control.CmdDoMovement(true, false ,false, false);
				interm = Direction.wait;
				if(following)
					mCD = MoveSpeed/2f;
				else
					mCD =  MoveSpeed;
			}
			else if(MoveQ.Count > 0)
			{
				Direction d = MoveQ.Pop();
				if(d == getDirection()) //Move Forward
				{
					//Debug.Log(MoveQ.Count + "| Dir to Move: " + d + " - Facing: " + getDirection() + " - Moving Forward");
					control.CmdDoMovement(true, false ,false, false);
					if(following)
						mCD = MoveSpeed/2f;
					else
						mCD =  MoveSpeed;
				}
				else if(d == Direction.wait)
				{
					mCD = Random.Range(mCD/2, mCD);
				}
				else if(reverse(d, getDirection()))
				{
					//Debug.Log(MoveQ.Count + "| Dir to Move: " + d + " - Facing: " + getDirection() + " - Turning Around");
					control.CmdDoMovement(false, false ,true, false);
					control.CmdDoMovement(false, false ,true, false);
					interm = Direction.up;
					if(following)
						mCD = MoveSpeed/2f;
					else
						mCD =  MoveSpeed/3.5f;
				}
				else
				{
					if(leftTurn(getDirection(), d)) //Turn left
					{
						//Debug.Log(MoveQ.Count + "| Dir to Move: " + d + " - Facing: " + getDirection() + " - Turning Left");
						control.CmdDoMovement(false, false ,true, false);
						interm = Direction.up;
						if(following)
							mCD = MoveSpeed/2f;
						else
							mCD =  MoveSpeed/3.5f;
					}
					else //Turn right
					{
						//Debug.Log(MoveQ.Count + "| Dir to Move: " + d + " - Facing: " + getDirection() + " - Turning Right");
						control.CmdDoMovement(false, false ,false, true);
						interm = Direction.up;
						if(following)
							mCD = MoveSpeed/2f;
						else
							mCD =  MoveSpeed/3.5f;
					}
				}
			}
			else
			{
				if(following)
				{
					following = false;
					Debug.Log("Ended follow (lost or killed)");
				}
				hasPath = false;
				wantPos = randomPos();
				mCD =  MoveSpeed/2;
			}

		}
		else
			mCD -= Time.deltaTime;
	}

	bool reverse(Direction o, Direction g)
	{
		return ((o == Direction.up && g == Direction.down) || (o == Direction.left && g == Direction.right) ||
				(o == Direction.down && g == Direction.up) || (o == Direction.right && g == Direction.left));
	}

	bool leftTurn(Direction o, Direction g)
	{
		return ((o==Direction.up && g == Direction.left) || (o == Direction.left && g == Direction.down) ||
				(o == Direction.down && g == Direction.right) || (o == Direction.right && g == Direction.up));
	}

	Direction getDirection()
	{
		if(transform.localEulerAngles.y < 45 || transform.localEulerAngles.y > 315)
			return Direction.up;
		else if(transform.localEulerAngles.y < 135)
			return Direction.right;
		else if(transform.localEulerAngles.y < 225)
			return Direction.down;
		else 
			return Direction.left;
	}

	SinglePlayerControl hc;
	public bool following;
	void CheckShoot()
	{
		Ray r = new Ray(transform.position+(Vector3.up*1.38f), transform.TransformDirection(Vector3.forward)*50);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, 50))
		{
			if(hit.collider.tag == "OtherPlayer" || hit.collider.tag == "MyPlayer")
			{
				hc = hit.collider.GetComponent<SinglePlayerControl>();
				if(!following)
				{
					Debug.Log("Attempting to follow new target");
					Invoke("SetPos", 0.5f);
				}
				following = true;
				rCD -= Time.deltaTime;
				if(rCD <= 0)
				{
					hc = null;
					following = false;
					control.CmdShoot();
					rCD = ReflexSpeed;
				}
			}
			else
				rCD = ReflexSpeed;
		}
	}

	void SetPos()
	{
		if(hc != null)
		{
			MoveQ.Push(Direction.wait);
			Debug.Log("Locked onto new Target");
			wantPos = new IntVector((int)hc.pos().x, (int)hc.pos().y);
			GetPath();
		}
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

[System.Serializable]
public enum Direction
{
	up, down, left, right, wait
}
