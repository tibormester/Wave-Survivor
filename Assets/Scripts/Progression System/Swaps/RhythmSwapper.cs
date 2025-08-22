using UnityEngine;
using System.Collections.Generic;
using System;

public class RhythmSwapper : MonoBehaviour {

    public Rhythm currentRhythm;

    public event Action OnRhythmSwapped;


    public void SwapRhythm(Rhythm newRhythm){
        currentRhythm = newRhythm;
        GetComponent<BeatManager>().PlayAudioSource(newRhythm.audioSourceIndex);
        OnRhythmSwapped?.Invoke();
    }
}