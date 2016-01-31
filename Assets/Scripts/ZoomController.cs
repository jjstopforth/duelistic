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
    protected Vector3 _pos;
    protected Camera _camera;

	// Use this for initialization
	void Start ()
	{
        _camera = GetComponentInParent<Camera>();

        if (_camera.orthographic) 
		{
			MaxZoomSize = _camera.orthographicSize;
		}

        _camera.orthographicSize = MinZoomSize;

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

		if (_camera.orthographic) {
            _camera.orthographicSize = ((newOrthoSize > MinZoomSize) ? newOrthoSize : MinZoomSize);

            _pos = transform.position;
            _pos.y = (_camera.orthographicSize - MinZoomSize);
            transform.position = _pos;
        }
	}

    public void ResetCamera()
    {
        _fired = false;
        GetComponent<Camera>().orthographicSize = MinZoomSize;
    }
}
