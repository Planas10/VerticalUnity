using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SetVolume : MonoBehaviour
{
    public AudioMixer audioMixer;
    private void Awake()
    {
        
    }
    public void SetVolumeMaster(float v) 
    {
        float newValue = Mathf.Log10(v) * 20;
        if (v == 0)
            newValue = -80;
        audioMixer.SetFloat("MasterV", newValue);
    }
    public void SetVolumeEffect(float v)
    {
         float newValue = Mathf.Log10(v) * 20;
        if (v == 0)
            newValue = -80;
        audioMixer.SetFloat("EffectV", newValue);
    }
    public void SetVolumeMusic(float v)
    {
        float newValue = Mathf.Log10(v) * 20;
        if (v == 0)
            newValue = -80;
        audioMixer.SetFloat("MusicV", newValue);
    }
}
