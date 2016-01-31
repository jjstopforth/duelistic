using UnityEngine;
using System.Collections;

public class TransistionController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		if (Input.anyKey) {
			//SceneManager.LoadScene("Main");
			Debug.Log("Load next scene!");
		
		}
	
	}
}
