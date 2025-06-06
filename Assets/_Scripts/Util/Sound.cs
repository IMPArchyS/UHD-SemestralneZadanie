using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    [SerializeField] private string name;
    [SerializeField] AudioClip clip;

    public string getName()
    {
        return this.name;
    }

    public AudioClip getClip()
    {
        return this.clip;
    }
}
