using System;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    [SerializeField] private Sound[] music, sfx;
    [SerializeField] private AudioSource musicSrc, sfxSrc;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }
    private void Start()
    {
        playMusic("BGMusic");
    }
    
    public void playMusic(string name)
    {
        Sound s = Array.Find(music, m => m.getName().Equals(name));

        if(s != null)
        {
            this.musicSrc.clip = s.getClip();
            this.musicSrc.Play();
        }
    }

    public void playSfx(string name)
    {
        Sound s = Array.Find(sfx, m => m.getName().Equals(name));

        if(s != null)
            this.sfxSrc.PlayOneShot(s.getClip());
    }

    public void adjustMusic(float value)
    {
        this.musicSrc.volume = value;
        if(this.musicSrc.volume == 0)
            this.musicSrc.mute = true;
        else 
            this.musicSrc.mute = false;
    }

    public void adjustSfx(float value)
    {
        this.sfxSrc.volume = value;
        if(this.musicSrc.volume == 0)
            this.musicSrc.mute = true;
        else 
            this.musicSrc.mute = false;
    }
}
