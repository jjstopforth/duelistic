﻿using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {

	//For different screen sizes
	public float _startPosition;
	float _endPosition;

	Animator _playerAnimator;
	string[] _animationBools = {"isReady","isWalking", "isTurning", "isShooting", "isWinner"};

	bool _hasFlipped = false;

    //The pseudo factors:
    public float gumption, manners, breeding, bottle = 0f;
    public static float maxFactorValue = 100;

	// Use this for initialization
	void Start ()
	{
		_playerAnimator = this.GetComponentInParent<Animator>();

		//_startPosition = this.transform.position.x;

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
	void Update ()
	{
		if (_playerAnimator == null) {

			return;
			
		}

		switch (DuelManagerBehaviour.Instance.State) {
		case DuelStates.none:
			break;
		case DuelStates.pistolSelection:
			break;
		case DuelStates.ready:
			break;
		case DuelStates.walk:
			ResetAnimationBools ();
			_playerAnimator.SetBool ("isWalking", true);
			UpdatePosition ();
			break;
		case DuelStates.turn:
			if (!_hasFlipped) 
			{
				this.GetComponentInParent<SpriteRenderer>().flipX = !this.GetComponentInParent<SpriteRenderer>().flipX;
				_hasFlipped = true;

			}
				ResetAnimationBools();
				_playerAnimator.SetBool("isTurning",true);
				break;
			case DuelStates.shoot:
				ResetAnimationBools();
				_playerAnimator.SetBool("isShooting",true);
				break;
			default:
				break;
		
		}
	}

    /// <summary>
    /// Resets this entire object back to start state...
    /// </summary>
    public void ResetStats()
    {
        gumption = 0f;
        manners = 0f;
        breeding = 0f;
        bottle = 0f;

        //Undo whatever flip happened...
        this.GetComponentInParent<SpriteRenderer>().flipX = (transform.position.x < 0f);
        _hasFlipped = false;

        //Reset our positions...
        Vector3 startPos = transform.position;
        startPos.x = _startPosition;
        transform.position = startPos;

        //Reset animations
        if (_playerAnimator != null)
        {
            SetAnimationBool("isReady", true);
        }
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

	void ResetAnimationBools ()
	{
		
		foreach (string animatorBoolean in _animationBools)
		{
			_playerAnimator.SetBool(animatorBoolean, false);	

		}

	}

    public void SetAnimationBool(string _boolName, bool _value, bool _resetOtherBools = false)
    {
        if (_resetOtherBools) ResetAnimationBools(); //Probably don't do this... erm...
        _playerAnimator.SetBool(_boolName, _value);
    }
}
