using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class PlayerIcon : MonoBehaviour {

	public PlayerControl pControl;

	public Vector2 Dimensions;
	public Vector2 pOffset;
	public Vector2 cellDim;

	public Image icon;

	MazeGenerator mGen;
	Maze m;

	void Start()
	{
		Rect r = transform.parent.GetComponent<RectTransform>().rect;
		Dimensions = new Vector2(r.width, r.height);
		mGen = FindObjectOfType<MazeGenerator>();
	}

	void Update () 
	{
		if(m == null)
		{
			m = mGen.currentMaze();
		}
		else if(pControl == null)
		{
			GameObject g = GameObject.FindGameObjectWithTag("MyPlayer");
			if(g != null)
				pControl = g.GetComponent<PlayerControl>();
		}
		else
		{
			pOffset = pixelOffset();
			cellDim = cellDimenstions();
			icon.rectTransform.sizeDelta = new Vector2(cellDimenstions().x, cellDimenstions().y);
			icon.transform.localEulerAngles = new Vector3(0,0,-pControl.transform.localEulerAngles.y + 90);
			Vector2 pPos = pControl.pos();
			float x = pixelOffset().x + (cellDimenstions().x*pPos.x) + (cellDimenstions().x/2);
			float y = pixelOffset().y + (cellDimenstions().y*pPos.y) + (cellDimenstions().x/2);
			Vector2 v = new Vector2(x,y);
			GetComponent<RectTransform>().anchoredPosition = v;
		}
	}

	Vector2 cellDimenstions()
	{
		float X = (Dimensions.x)/m.Dimensions().x;
		float Y = (Dimensions.y)/m.Dimensions().y;
		return new Vector2(Mathf.Min(X,Y),Mathf.Min(X,Y));
	}

	Vector2 pixelOffset()
	{
		Vector2 pS = new Vector2(Dimensions.x, Dimensions.y);
		Vector2 mS = new Vector2(m.Dimensions().x, m.Dimensions().y);
		float dx = 0;
		float dy = 0;
		if(pS.x/pS.y > mS.x/mS.y)
		{
			mS *= pS.y/mS.y;
			dx = pS.x - mS.x;
		}
		else
		{
			mS *= pS.x/mS.x;
			dy = pS.y-mS.y;
		}
		return new Vector2((dx)/2, (dy)/2);
	}
}
