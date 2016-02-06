using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class PlayerControl : NetworkBehaviour {

	MazeGenerator gen;
	Maze m;

	[SyncVar]
	int pX;
	[SyncVar]
	int pY;

	[SyncVar]
	public int score;
	[SyncVar]
	public string name;

	public LineRenderer r;

	public GameObject SpriteObj;
	public GameObject CamHolder;

	bool setup;

	void Start () 
	{
		gen = FindObjectOfType<MazeGenerator>();
	}

	public Vector2 pos()
	{
		return new Vector2(pX, pY);
	}

	[Command]
	public void CmdSetName(string nm)
	{
		name = nm;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(m == null)
		{
			m = gen.currentMaze();
			RandomPlacement();
		}
		if(isLocalPlayer)
		{
			if(!setup)
			{
				gameObject.tag = "MyPlayer";
				SpriteObj.SetActive(false);
				CamHolder.SetActive(true);
			}
			bool up = Input.GetKeyDown(KeyCode.UpArrow);
			bool down = Input.GetKeyDown(KeyCode.DownArrow);
			bool left = Input.GetKeyDown(KeyCode.LeftArrow);
			bool right = Input.GetKeyDown(KeyCode.RightArrow);
			if(up || down || left || right)
			{
				Debug.Log("Move Command");
				CmdDoMovement(up, down, left, right);
			}
			CheckSeen();

		}
		else
		{
			if(!setup)
			{
				gameObject.tag = "OtherPlayer";
				SpriteObj.SetActive(true);
			}
		}
			
		if(isLocalPlayer && Input.GetKeyDown(KeyCode.Space))
		{
			StopCoroutine("LineShow");
			StartCoroutine("LineShow");
			CmdShoot();
		}
	}

	void CheckSeen()
	{
		Ray r = new Ray(transform.position+(Vector3.up*1.38f), transform.TransformDirection(Vector3.forward)*50);
		Debug.DrawRay(transform.position+(Vector3.up*1.38f), transform.TransformDirection(Vector3.forward)*50,Color.blue,1);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, 50))
		{
			if(hit.collider.tag == "OtherPlayer" || hit.collider.tag == "MyPlayer")
			{
				PlayerControl opc = hit.collider.GetComponentInParent<PlayerControl>();
				FindObjectOfType<ScoreSystem>().CurrentView = opc.name;
			}
			else
				FindObjectOfType<ScoreSystem>().CurrentView = "";
		}
		else
			FindObjectOfType<ScoreSystem>().CurrentView = "";
	}

	[Command]
	void CmdShoot()
	{
		Debug.Log("Player Shooting");
		StopCoroutine("LineShow");
		StartCoroutine("LineShow");
		Ray r = new Ray(transform.position+(Vector3.up*1.38f), transform.TransformDirection(Vector3.forward)*50);
		Debug.DrawRay(transform.position+(Vector3.up*1.38f), transform.TransformDirection(Vector3.forward)*50,Color.blue,1);
		RaycastHit hit;
		if(Physics.Raycast(r, out hit, 50))
		{
			if(hit.collider.tag == "OtherPlayer" || hit.collider.tag == "MyPlayer")
			{
				PlayerControl opc = hit.collider.GetComponentInParent<PlayerControl>();
				changeScore(10);
				opc.RpcKill();
				opc.kill();
			}
		}
	}

	[ClientRpc]
    public void RpcKill()
    {
		if(GetComponentInChildren<InvertColorsEffect>() != null)
		{
			GetComponentInChildren<InvertColorsEffect>().enabled = true;
        	Invoke("HideEffect", 0.15f);
		}
    }

    void HideEffect()
    {
		GetComponentInChildren<InvertColorsEffect>().enabled = false;
    }

	public void changeScore(int x)
	{
		score += x;
	}

	public void kill()
	{
		RandomPlacement();
	}

	IEnumerator LineShow()
	{
		r.enabled = true;
		yield return new WaitForSeconds(0.1f);
		r.enabled = false;
		Debug.Log("Hiding Line");
	}

	public void RandomPlacement()
	{
		StopCoroutine("Place");
		StartCoroutine("Place");
	}

	IEnumerator Place()
	{
		while(m == null)
			yield return true;
		
		int x = 0;
		int y =0;
		while(m.GetCell(x,y).isWall)
		{
			x = Random.Range(1, (int)m.Dimensions().x);
			y = Random.Range(1, (int)m.Dimensions().y);
		}
		pX = x;
		pY = y;
		transform.position = new Vector3(x*1.5f, 0, y*1.5f);
		transform.localEulerAngles = new Vector3(0,Random.Range(0,4)*90, 0);
	}

	[Command]
	void CmdDoMovement(bool up, bool back, bool left, bool right)
	{
		Vector2 fDir = new Vector2(0,1);
		float ang = transform.localEulerAngles.y;
		if(ang < 100 && ang > 80)
			fDir = new Vector2(1,0);
		else if(ang < 190 && ang > 170)
			fDir = new Vector2(0, -1);
		else if(ang < 280 && ang > 260)
			fDir = new Vector2(-1,0);

		if(up)
		{
			int nX = pX + (int)fDir.x;
			int nY = pY + (int)fDir.y;
			if(!m.GetCell(nX, nY).isWall)
			{
				pX = nX;
				pY = nY;
				transform.position = new Vector3(nX*1.5f, 0, nY*1.5f);
			}	
		}
		else if(back)
		{
			int nX = pX - (int)fDir.x;
			int nY = pY - (int)fDir.y;
			if(!m.GetCell(nX, nY).isWall)
			{
				pX = nX;
				pY = nY;
				transform.position = new Vector3(nX*1.5f, 0, nY*1.5f);
			}
		}
		else if(left)
		{
			transform.localEulerAngles += new Vector3(0,-90,0);
		}
		else if(right)
		{
			transform.localEulerAngles += new Vector3(0, 90, 0);
		}
	}

	/*
	void OnDestroy()
	{
		transform.GetComponentInChildren<Camera>().transform.SetParent(null);
	}
	*/
}
