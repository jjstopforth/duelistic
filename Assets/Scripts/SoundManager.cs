using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SoundManager : MonoBehaviour
{
    //public AudioClip walk;
    //public AudioClip music;
    public List<AudioClip> footSteps;
    public GameObject TempSoundPrefab;
    AudioSource audio;

    //public AudioSource walkAudio;
    //public AudioSource musicAudio;
    public AudioClip walk;
    public AudioClip music;

	// Use this for initialization
	void Start ()
    {
        audio = GetComponent<AudioSource>();
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void PlayWalk()
    {
        //walkAudio.Play();
        //musicAudio.Stop();
        audio.clip = walk;
        audio.Play();
        audio.loop = false;
    }

    public void PlayMusic()
    {
        //walkAudio.Stop();
        //musicAudio.Play();
        audio.clip = music;
        audio.Play();
        audio.loop = false;
    }
    public void PlayFootStep(GameObject from)
    {
        GameObject footstep = (GameObject)Instantiate(TempSoundPrefab,from.transform.position,from.transform.rotation);
        AudioSource audioSource = footstep.GetComponent<AudioSource>();
        audioSource.clip = footSteps[Random.Range(0,footSteps.Count)];
        audioSource.Play();
    }
}
