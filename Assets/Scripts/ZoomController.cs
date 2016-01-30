using UnityEngine;
using System.Collections;

public class ZoomController : MonoBehaviour {
    
    [SerializeField]
    public GameObject Player;
    [SerializeField]
    public float MaxZoomSize;
    [SerializeField]
    public float MinZoomSize;


    float _lerpStart;
    float _lerpEnd;
    float _zoomStartFraction;
	bool _fired = false;

	float _timeAccumulator;
	float _deltaTime;

	// Use this for initialization
	void Start ()
	{
		if (this.GetComponentInParent<Camera>().orthographic) 
		{
			MaxZoomSize = this.GetComponentInParent<Camera>().orthographicSize;
		}

		this.GetComponentInParent<Camera>().orthographicSize = MinZoomSize;

		_lerpEnd = MaxZoomSize;
	}
	
	// Update is called once per frame
	void Update ()
	{

		float newOrthoSize = DuelManagerBehaviour.Instance.WalkFraction * MaxZoomSize;

		if ((newOrthoSize > MinZoomSize) && !_fired) 
		{
			_lerpStart = newOrthoSize;
			_zoomStartFraction = DuelManagerBehaviour.Instance.WalkFraction;

			//Debug.Log ("WalkFraction is: " + DuelManagerBehaviour.Instance.WalkFraction);
			_fired = true;

		}

		if (_fired) 
		{

			float t = (DuelManagerBehaviour.Instance.WalkFraction - _zoomStartFraction) / (1f - _zoomStartFraction);
			newOrthoSize = Mathf.Lerp(_lerpStart,_lerpEnd, Easing.Ease(0f,1f,t,EasingType.QuadraticInOut));
			//Debug.Log("The value of t is: " + t);
			//Debug.Log("The easing value is: " + Easing.Ease(0f,1f,t,EasingType.SineInOut));

		}

		if (this.GetComponentInParent<Camera> ().orthographic) {
			this.GetComponentInParent<Camera> ().orthographicSize = ((newOrthoSize > MinZoomSize) ? newOrthoSize : MinZoomSize);


		}
	}

    public void ResetCamera()
    {
        _fired = false;
        GetComponent<Camera>().orthographicSize = MinZoomSize;
    }
}
