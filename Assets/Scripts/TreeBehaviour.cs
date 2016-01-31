using UnityEngine;
using System.Collections;

public class TreeBehaviour : MonoBehaviour {

    protected ZoomController _zoomController;
    protected Vector3 _startPos, _pos;

	// Use this for initialization
	void Start () {
        _startPos = transform.position;
        _zoomController = Camera.main.GetComponent<ZoomController>();
	}
	
	// Update is called once per frame
	void Update () {
        _pos = _startPos;
        _pos.x += Mathf.Sign(_pos.x) * (_zoomController.MaxZoomSize - Camera.main.orthographicSize);
        transform.position = _pos;
	}
}
