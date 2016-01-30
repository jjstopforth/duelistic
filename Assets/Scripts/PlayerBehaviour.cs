using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

	//For different screen sizes
	float _startPosition;
	float _endPosition;

    //The pseudo factors:
    public float gumption, manners, breeding, bottle = 0f;
    public static float maxFactorValue = 100;

	// Use this for initialization
	void Start ()
	{
		_startPosition = this.transform.position.x;
		//Debug.Log ("The start position is: " + _startPosition);

		if (Camera.main != null) 
		{
			//Debug.Log ("The Main camera half-width is: " + Camera.main.orthographicSize);
			if (Camera.main.orthographic) 
			{
				_endPosition = Camera.main.orthographicSize;
				_endPosition -= (_endPosition / 8f);

				if (_startPosition < 0)
				{
					// The player has to move towards the left endpoint
					_endPosition *= -1;
				}
			}
		}
    	
	}
	
	// Update is called once per frame
	void Update()
    {

		switch (DuelManagerBehaviour.Instance.State)
		{
			case DuelStates.none:
				break;
			case DuelStates.pistolSelection:
				//Debug.Log("Pistol selection taking place");
				break;
			case DuelStates.ready:
				//Debug.Log("Working up the courage.");
				break;
			case DuelStates.walk:
				Debug.Log("Walking");
				UpdatePosition();
				break;
			case DuelStates.turn:
				//Debug.Log("Turning slowly");
				break;
			case DuelStates.shoot:
				//Debug.Log("Pew pew!");
				break;

			default:
				break;
		
		}


	}



	private void UpdatePosition ()
	{

		if (this.transform != null) {
			float newX = (DuelManagerBehaviour.Instance.WalkFraction * _endPosition) + _startPosition;

			Vector3 newPosition = new Vector3 (newX, this.transform.position.y, this.transform.position.z);
			this.transform.position = newPosition;
			
		} 

	}

}
