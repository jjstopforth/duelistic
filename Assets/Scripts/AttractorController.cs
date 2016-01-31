using UnityEngine;
using System.Collections;

public class AttractorController : MonoBehaviour {

	Vector2 _maxOffset, _minOffset;
	Vector3 _targetPosition;

	bool _roaming;
	bool _hasTarget;

	// Use this for initialization
	void Start () 
	{
		Reset();

	}
	
	// Update is called once per frame
	void Update ()
	{

		Vector3 newPosition = _targetPosition;

		if (PositionInBounds (newPosition)) {

			
			
		}

		if (_roaming) 
		{
			_targetPosition = new Vector3 (Random.Range (-307, 332), Random.Range (-100, 100));
		} 
		else if (!_hasTarget && !_roaming) 
		{
			// Move towards the centre
			_targetPosition = Vector3.zero;
		}


		this.transform.localPosition = newPosition;
	
	}

	bool PositionInBounds (Vector3 pos)
	{

		//if ()

		return true;


	}

	public void SetTarget (Vector3 position)
	{	

		if (!_hasTarget) 
		{ 	
			SetPosition (position);
			_hasTarget = true;
			_roaming = false;

			// The hand has to snap to the target
			this.GetComponent<SpringJoint2D>().frequency = 1f;
		} 

	}

	void SetPosition (Vector3 targetPosition)
	{
		_targetPosition = targetPosition;
		this.transform.localPosition = targetPosition;

	}

	public bool HasTarget {
		get { return _hasTarget; }
	}

	public Vector3 TargetPosition 
	{
		get { return _targetPosition; }
	}

	public void Reset ()
	{
		_roaming = true;
		_hasTarget = false;

		_targetPosition = new Vector3(Random.Range(-307,332),Random.Range(-100,100));

		
	}

}
