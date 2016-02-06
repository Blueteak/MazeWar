using UnityEngine;
using System.Collections;

public class InvertColorsEffect : MonoBehaviour {

	public Material m;
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnRenderImage(RenderTexture t, RenderTexture d)
	{
		Graphics.Blit(t, d, m);
	}

}
