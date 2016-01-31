using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroySound : MonoBehaviour {

    public List<AudioClip> soundClips;
    AudioSource audio;
    private bool started = false;
    // Use this for initialization
    void Start()
    {
        audio = GetComponent<AudioSource>();
        started = true;
    }


    // Update is called once per frame
    void Update()
    {
        if (started && !audio.isPlaying)
        {
            Destroy(gameObject);
        }
    }
}
