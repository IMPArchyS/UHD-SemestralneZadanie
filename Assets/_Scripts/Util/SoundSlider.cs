using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private void Awake()
    {
        this.slider = GetComponent<Slider>();
        if(SoundManager.instance)
        {
            if(this.slider.gameObject.name.Equals("SoundSlider"))
                this.slider.onValueChanged.AddListener(SoundManager.instance.adjustSfx);

            if(this.slider.gameObject.name.Equals("MusicSlider"))
                this.slider.onValueChanged.AddListener(SoundManager.instance.adjustMusic);

        }
    }
}
