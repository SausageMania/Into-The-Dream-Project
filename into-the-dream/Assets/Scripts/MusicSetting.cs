using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicSetting : MonoBehaviour{

    public Slider SetVolume;
    public AudioSource audioSetting;

    public float setVol = 0.5f;

    void Start()
    {
        SoundManager.instance.PlaySound("bgm", 0);
        audioSetting = GetComponent<AudioSource>();

        setVol              = PlayerPrefs.GetFloat("setvol", 0.5f);
        SetVolume.value     = setVol;
        audioSetting.volume = SetVolume.value;
    }

    // Update is called once per frame
    void Update()
    {
        SoundSlider();
    }

    public void SoundSlider()
    {
        audioSetting.volume = SetVolume.value;
        setVol = SetVolume.value;
        PlayerPrefs.SetFloat("setvol", setVol);
        PlayerPrefs.Save();
    }
}
