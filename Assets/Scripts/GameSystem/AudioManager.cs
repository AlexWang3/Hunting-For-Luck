using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    public AudioSource audioSourceForBGM;
    public AudioSource audioSourceForSFX;
    private void Awake() {
        Instance = this;
    }

    public void PlaySFX(string fileName) {
        string path = Path.Join("Audio", "SFXs", fileName);
        var audioClip = Resources.Load<AudioClip>(path);
        audioSourceForSFX.PlayOneShot(audioClip);
    }
    
    public void PlayBGM(string fileName) {
        string path = Path.Join("Audio", "BGMs", fileName);
        var audioClip = Resources.Load<AudioClip>(path);
        audioSourceForBGM.loop = true;
        audioSourceForBGM.PlayOneShot(audioClip);
        audioSourceForBGM.loop = true;
    }
}
