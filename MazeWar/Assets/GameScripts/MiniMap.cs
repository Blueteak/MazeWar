using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour {

	Image im;
	MazeGenerator mGen;
	Maze m;
	// Use this for initialization
	void Start () 
	{
		im = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update () 
	{
		if(mGen == null)
			mGen = FindObjectOfType<MazeGenerator>();
		else if(m == null && mGen != null)
		{
			m = mGen.currentMaze();
			if(m != null)
			{
				Texture2D t = new Texture2D((int)m.Dimensions().x, (int)m.Dimensions().y, TextureFormat.ARGB4444, false);
				for(int x=0; x<t.width; x++)
				{
					for(int y=0; y<t.height; y++)
					{
						if(m.GetCell(x,y).isWall)
							t.SetPixel(x,y,Color.black);
						else
							t.SetPixel(x,y,Color.white);
					}
				}
				t.anisoLevel = 0;
				t.filterMode = FilterMode.Point;
				t.Apply();
				im.sprite = Sprite.Create(t, new Rect(0,0,t.width,t.height), Vector2.one/2);
			}
		}
	}
}
