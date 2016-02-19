using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class AIPlayer : MonoBehaviour {

	bool online;
	Maze m;
	SinglePlayerControl control;

	public IntVector wantPos;
	public Stack<Direction> MoveQ;
	bool hasPath;

	public float MoveSpeed;
	float mCD;
	public float ReflexSpeed;
	float rCD;

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
		IntVector nVec = new IntVector(Random.Range(0,(int)m.Dimensions().x), Random.Range(0, (int)m.Dimensions().y));
		while(m.GetCell(nVec.x, nVec.y).isWall)
			nVec = new IntVector(Random.Range(0,(int)m.Dimensions().x), Random.Range(0, (int)m.Dimensions().y));
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

	void GetPath()
	{
		Debug.Log("Getting Path");
		MoveQ = new Stack<Direction>();
		visited = new bool[(int)m.Dimensions().x, (int)m.Dimensions().y];
		bool path = recursiveSolve((int)control.pos().x, (int)control.pos().y);
		hasPath = path;
	}

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
				following = true;
				Invoke("SetPos", 0.5f);
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
			Debug.Log("Attempting to follow new target");
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
