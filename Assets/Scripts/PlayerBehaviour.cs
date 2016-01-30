﻿using UnityEngine;
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
				_endPosition = Camera.main.GetComponentInParent<ZoomController>().MaxZoomSize;
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
    	Animator _playerAnimator = this.GetComponentInParent<Animator>();

		switch (DuelManagerBehaviour.Instance.State)
		{
			case DuelStates.none:
				break;
			case DuelStates.pistolSelection:
				break;
			case DuelStates.ready:
				break;
			case DuelStates.walk:
				_playerAnimator.ResetTrigger("Shoot"); // Might be able to remove later on or reset all after win/loss/tie.
				_playerAnimator.SetTrigger("Walk");
				UpdatePosition();
				break;
			case DuelStates.turn:
				_playerAnimator.ResetTrigger("Walk");
				_playerAnimator.SetTrigger("Turn");
				break;
			case DuelStates.shoot:
                this.GetComponentInParent<SpriteRenderer>().flipX = !this.GetComponentInParent<SpriteRenderer>().flipX;
				_playerAnimator.ResetTrigger("Turn");
				_playerAnimator.SetTrigger("Shoot");
				break;
			default:
				break;
		
		}
	}

    /// <summary>
    /// Resets stats to 0
    /// </summary>
    public void ResetStats()
    {
        gumption = 0f;
        manners = 0f;
        breeding = 0f;
        bottle = 0f;
    }

    /// <summary>
    /// Makes sure stats are within sane 0-maxFactorValue bounds
    /// </summary>
    public void SanitiseStats()
    {
        gumption = Mathf.Min(Mathf.Max(gumption, 0f), maxFactorValue);
        manners = Mathf.Min(Mathf.Max(manners, 0f), maxFactorValue);
        breeding = Mathf.Min(Mathf.Max(breeding, 0f), maxFactorValue);
        bottle = Mathf.Min(Mathf.Max(bottle, 0f), maxFactorValue);
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
