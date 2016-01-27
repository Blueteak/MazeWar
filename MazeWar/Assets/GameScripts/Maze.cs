using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Maze{

	Cell[,] MazeCells;

	// Use this for initialization
	public Maze (int W, int H, int percent, int seed)
	{
		W = Mathf.Max(W, 3);
		H = Mathf.Max(H, 3);
		if(W%2 == 0)
			W++;
		if(H%2 == 0)
			H++;
		MazeCells = new Cell[W,H];
		Generate(percent, seed);
	}

	public void Generate(int HolePercentage, int seed)
	{
		ResetMaze();
		//Code to fill in Cells
		List<Cell> Visited = new List<Cell>();
		System.Random r = new System.Random(seed);
		CarvePassage(1,1,Visited, r);
		RandomHoles(HolePercentage, r);
	}

	void CarvePassage(int x, int y, List<Cell> visited, System.Random r)
	{
		visited.Add(MazeCells[x,y]);
		List<Vector2> Dir = new List<Vector2>(); 
		Dir.Add(new Vector2(0,1)); Dir.Add(new Vector2(0,-1)); Dir.Add(new Vector2(1,0)); Dir.Add(new Vector2(-1,0));
		Dir.Shuffle(r);
		foreach(var i in Dir)
		{
			int nx = (int)(x+(2*i.x));
			int ny = (int)(y+(2*i.y));
			if(nx > 0 && nx < MazeCells.GetLength(0) && ny > 0 && ny < MazeCells.GetLength(1))
			{
				if(!visited.Contains(MazeCells[nx,ny]))
				{
					MazeCells[(int)(x+i.x), (int)(y+i.y)].isWall = false;
					CarvePassage(nx, ny, visited, r);
				}
			}
		}
	}

	void RandomHoles(int percent, System.Random r)
	{
		List<Cell> UsedCells = new List<Cell>();
		for(int x = 1; x < MazeCells.GetLength(0)-1; x++)
		{
			for(int y = 1; y<MazeCells.GetLength(1)-1; y++)
			{
				if( MazeCells[x,y].isWall && !UsedCells.Contains(MazeCells[x,y]))
				{
					List<Cell> nB = MazeCells[x,y].Neighbors;
					int adj = 0;
					Cell n1 = null;
					Cell n2 = null;
					foreach(var v in nB)
					{
						if(v.isWall)
						{
							adj++;
							if(adj == 1)
								n1 = v;
							else if(adj == 2)
								n2 = v;
						}
							
					}
					if(adj == 2 && ((n1.x == n2.x) || (n1.y == n2.y)))
					{
						int rN = r.Next(0,100);
						if(rN > percent)
						{
							UsedCells.Add(n1);
							UsedCells.Add(n2);
							//Debug.Log("Want to Remove ("+x+","+y+")\nNeighbors: ("+nB[0].x+","+nB[0].y+") - ("+nB[1].x+","+nB[1].y+")");
							MazeCells[x,y].isWall = false;
						}

					}
				}
			}
		}
	}

	void ResetMaze()
	{
		//Set all cells to new Cell
		for(int x=0; x<MazeCells.GetLength(0); x++)
		{
			for(int y=0; y<MazeCells.GetLength(1); y++)
			{
				MazeCells[x,y] = new Cell(true, x, y);
			}
		}
		//Do grid Layout
		for(int x = 1; x < MazeCells.GetLength(0)-1; x++)
		{
			for(int y = 1; y<MazeCells.GetLength(1)-1; y++)
			{
				if(x%2 == 1 && y%2 == 1)
				{
					MazeCells[x,y].isWall = false;
				}
			}
		}
		//Add all neighbors
		for(int x=0; x<MazeCells.GetLength(0); x++)
		{
			for(int y=0; y<MazeCells.GetLength(1); y++)
			{
				if(x > 0)
					MazeCells[x,y].AddNeighbor(MazeCells[x-1, y]);
				if(x < MazeCells.GetLength(0)-1)
					MazeCells[x,y].AddNeighbor(MazeCells[x+1, y]);
				if(y > 0)
				{
					MazeCells[x,y].AddNeighbor(MazeCells[x, y-1]);
					if(x > 0)
						MazeCells[x,y].AddNeighbor(MazeCells[x-1, y-1]);
					if(x < MazeCells.GetLength(0)-1)
						MazeCells[x,y].AddNeighbor(MazeCells[x+1, y-1]);
				}
				if(y < MazeCells.GetLength(1)-1)
				{
					MazeCells[x,y].AddNeighbor(MazeCells[x, y+1]);
					if(x > 0)
						MazeCells[x,y].AddNeighbor(MazeCells[x-1, y+1]);
					if(x < MazeCells.GetLength(0)-1)
						MazeCells[x,y].AddNeighbor(MazeCells[x+1, y+1]);
				}
			}
		}
	}

	public Cell[,] GetCells()
	{
		return MazeCells;
	}

	public Vector2 Dimensions()
	{
		return new Vector2(MazeCells.GetLength(0), MazeCells.GetLength(1));
	}

	public Cell GetCell(int x, int y)
	{
		if(x >= 0 && x < MazeCells.GetLength(0))
		{
			if(y >= 0 && y < MazeCells.GetLength(1))
			{
				return MazeCells[x,y];
			}
		}
		Debug.LogError("Invalid Maze Cell (Out of Range)");
		return null;
	}

}

public class Cell
{
	public bool isWall;
	public int x = 0;
	public int y = 0;
	public List<Cell> Neighbors;

	public Cell(bool wall, int newX, int newY)
	{
		x = newX;
		y = newY;
		isWall = wall;
		Neighbors = new List<Cell>();
	}

	public void AddNeighbor(Cell c)
	{
		if(!Neighbors.Contains(c))
		{
			Neighbors.Add(c);
		}
	}

	public int wallsSurround()
	{
		int nW = 0;
		foreach(var v in Neighbors)
		{
			if(v.isWall)
				nW++;
		}
		return nW;
	}
}


