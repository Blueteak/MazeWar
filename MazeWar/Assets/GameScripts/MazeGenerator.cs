using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;

public class MazeGenerator : NetworkBehaviour {

	Maze cMaze;
	public int Width;
	public int Height;

	[Range (0,100)]
	public int SparsePercent;

	public GameObject WallPrefab;

	List<GameObject> WallObjects;

	[SyncVar]
	public int MazeSeed;

	void Start()
	{
		NewMaze();
	}

	public void NewMaze()
	{
		ClearOld();
		WallObjects = new List<GameObject>();
		cMaze = new Maze(Width, Height, 100-SparsePercent, MazeSeed);
		Cell[,] cells = cMaze.GetCells();
		MakeMaze(cells);
	}

	public Maze currentMaze()
	{
		return cMaze;
	}

	void MakeMaze(Cell[,] cells)
	{
		ClearOld();
		for(int x =0; x<cells.GetLength(0); x++)
		{
			for(int y=0; y<cells.GetLength(1); y++)
			{
				if(cells[x,y].isWall)
				{
					GameObject newObj = (GameObject)Instantiate(WallPrefab);
					newObj.transform.position = new Vector3(x*1.5f, 0, y*1.5f);
					newObj.transform.SetParent(transform);
					WallObjects.Add(newObj);
				}
			}
		}
	}

	void ClearOld()
	{
		if(WallObjects != null)
		{
			foreach(var v in WallObjects)
			Destroy(v);
		}
		WallObjects = new List<GameObject>();
	}

}
