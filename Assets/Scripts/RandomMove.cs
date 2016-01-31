using UnityEngine;
using System.Collections;

public class RandomMove : MonoBehaviour {

	Vector2 _maxOffset, _minOffset;
	Vector2 position;

	// Use this for initialization
	void Start () 
	{

		//_maxOffset = this.GetComponentInParent<Canvas>().Ge;
		//_minOffset = this.GetComponentInParent<Canvas>().offsetMin;
		Debug.Log("The max offset is -> X: [" + _maxOffset.x + "] Y: [" + _maxOffset.y + "]");
		Debug.Log("The min offset is -> X: [" + _minOffset.x + "] Y: [" + _minOffset.y + "]");

		position = new Vector2(Random.Range(-307,332),Random.Range(-100,100));
	
	}
	
	// Update is called once per frame
	void Update ()
	{

		Vector3 newPosition = new Vector3 (position.x, position.y);

		if (PositionInBounds (newPosition)) 
		{

			this.transform.localPosition = newPosition;
			
		}

		position = new Vector2(Random.Range(-307,332),Random.Range(-100,100));
		

	
	}

	bool PositionInBounds (Vector3 pos)
	{

		//if ()

		return true;
		

	}
}
