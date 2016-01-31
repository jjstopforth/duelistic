using UnityEngine;
using System.Collections;

public class AttractorTarget : MonoBehaviour {

	
	public AttractorController Attractor;
	
	// Use this for initialization
	void Start () {
		Attractor = null;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Reset ()
	{
		Attractor = null;
	}

	public bool IsAvailable ()
	{

		return (Attractor == null);

	}
}
