using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string Name;
    public AudioClip File;
}

public class SEManager : MonoBehaviour {

    [SerializeField]
    static public SEManager instance;
    public Sound[] sounds;
    public AudioClip[] clips;
    // Use this for initialization
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);

        for(int i = 0; i < sounds.Length; i++)
        {
            //source = GetComponent<AudioSource>();
        }
        
        this.Play(0);
    }

    // Update is called once per frame
    void Update ()
    {
        
	}

    public void Play(int track)
    {

    }

    public void Stop()
    {

    }

    public void FadeOut()
    {

    }

    public void FadeIn()
    {

    }

    public void Reverse()
    {
    }
}
