using UnityEngine;

public class BGMManager : MonoBehaviour {
    static public BGMManager Instance;
    public AudioClip[] Clips;
    private AudioSource _source;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        _source = GetComponent<AudioSource>();
    }

    public void Play(int track)
    {
        _source.clip = Clips[track];
        _source.Play();
    }

    public void Play(string name)
    {
        for(var i = 0; i < Clips.Length; i++)
        {
            if (Clips[i].name.Equals(name))
            {
                Play(i);
            }
        }
    }

    public void Stop()
    {

    }

    public void Resume()
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
        _source.pitch = -1;
    }
}
