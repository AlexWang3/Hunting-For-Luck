using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DG.Tweening;
public class AudioManager : MonoBehaviour {
    public static AudioManager Instance;
    public AudioSource audioSourceForBGM;
    public AudioSource audioSourceForSFX;
    private string currentBGM;
    private Sequence oldSequence;
    private void Awake() {
        Instance = this;
    }

    public void PlaySFX(string fileName) {
        string path = Path.Join("Audio", "SFXs", fileName);
        var audioClip = Resources.Load<AudioClip>(path);
        audioSourceForSFX.PlayOneShot(audioClip);
    }
    
    public void PlayBGM(string fileName) {
        oldSequence.Kill();
        Sequence sequence = DOTween.Sequence();
        if (fileName == "") {
            sequence.Insert(0,audioSourceForBGM.DOFade(0, 1f));
            sequence.InsertCallback(2f, () => {
                audioSourceForBGM.Stop();
                audioSourceForBGM.clip = null;
            });
            sequence.Insert(2f,audioSourceForBGM.DOFade(1, 0));
        } else {
            if (currentBGM == fileName) {
                return;
            }
            currentBGM = fileName;
            string path = Path.Join("Audio", "BGMs", fileName);
            var audioClip = Resources.Load<AudioClip>(path);
            audioSourceForBGM.loop = true;
            sequence.Insert(0,audioSourceForBGM.DOFade(0, 0.5f));
            sequence.InsertCallback(0.5f, () => { 
                audioSourceForBGM.clip = audioClip;
                audioSourceForBGM.Play(); 
            });
            sequence.Insert(0.5f,audioSourceForBGM.DOFade(1, 0.5f));
        }
        sequence.Play();
        oldSequence = sequence;
    }
}
