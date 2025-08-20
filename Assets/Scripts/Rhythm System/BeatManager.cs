using UnityEngine;
using System;

public class BeatManager : MonoBehaviour{
    
    //triggers when the beat is hit and carriers -int beat count, doublehit time
    public event Action<int, double> onBeat;

    [SerializeField] private float beatsPerMinute = 120f;  

    [SerializeField] private double START_TIME_OFFSET_SECONDS = 0f;

    private AudioSource _audioSource;
    private double _startTime;


    public int _currentBeatIndex = 0;

    void Update(){
        while (AudioSettings.dspTime >= GetBeatTime(_currentBeatIndex)){
            onBeat?.Invoke(_currentBeatIndex, GetBeatTime(_currentBeatIndex));
            _currentBeatIndex++;
        }
    }
    void Start(){
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
        _startTime = AudioSettings.dspTime;
    }

    public double GetSecondsPerBeat(){
        return 60f / beatsPerMinute;
    }

    //returns which beat we are on since the song started
    public double GetSongCount(){
        double songPositionSeconds = AudioSettings.dspTime - _startTime;
        double offsetAdjustedPositionSeconds = songPositionSeconds - START_TIME_OFFSET_SECONDS;
        double beatCount = offsetAdjustedPositionSeconds /  GetSecondsPerBeat();
        return beatCount;
        //based on the current time, return the time converted to beats in the measure
    }

    //returns what time the beat should hit on based on the start time
    public double GetBeatTime(double beat){
        double beatHitTime = _startTime + START_TIME_OFFSET_SECONDS + (beat *  GetSecondsPerBeat());
        return beatHitTime;
    }

    



    
}