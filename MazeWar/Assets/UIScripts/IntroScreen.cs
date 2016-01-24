using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class IntroScreen : MonoBehaviour {

	public Text LoadText;
	public GameObject NextScene;
	public InputField inpt;
	void Start () 
	{
		StartCoroutine("LoadActions");
	}
	
	IEnumerator LoadActions()
	{
		for(int i=0; i< 3; i++)
		{
			LoadText.text = "";
			yield return new WaitForSeconds(0.4f);
			LoadText.text = ".";
			yield return new WaitForSeconds(0.4f);
			LoadText.text = "..";
			yield return new WaitForSeconds(0.4f);
			LoadText.text = "...";
			yield return new WaitForSeconds(0.4f);
		}
		NextScene.SetActive(true);
		EventSystem.current.SetSelectedGameObject(inpt.gameObject);
		gameObject.SetActive(false);

	}
}
