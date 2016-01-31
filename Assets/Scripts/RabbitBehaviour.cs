using UnityEngine;
using System.Collections;

public class RabbitBehaviour : MonoBehaviour {

    protected Vector3 pos;

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(Random.Range(-2f, 2f), -Camera.main.orthographicSize, -1f);
	}
	
	// Update is called once per frame
	void Update () {
        pos = transform.position;
        pos.y = -Camera.main.orthographicSize;
        transform.position = pos;
	}
}
