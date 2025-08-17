using UnityEngine;
using System;

public class RhythmManager : MonoBehaviour{
    
    //Must be sorted in chronological order - the beats are values indicate what count they appear on
    [SerializeField] private float[] beatPattern = {1f, 1.5f,  1f, 2f, 3f, 3.75f, 4f};
    [SerializeField] private float patternLength = 4f; 
    [SerializeField] private float beatsPerMinute = 120f;  

    [SerializeField] private double START_TIME_OFFSET_SECONDS = 0f;

    private AudioSource _audioSource;
    private double _startTime;

    
    void Start(){
        _audioSource = GetComponent<AudioSource>();
        _audioSource.Play();
        _startTime = AudioSettings.dspTime;
    }

    //returns which beat we are on since the song started
    public double GetSongCount(){
        double songPositionSeconds = AudioSettings.dspTime - _startTime;
        double offsetAdjustedPositionSeconds = songPositionSeconds - START_TIME_OFFSET_SECONDS;
        double secondsPerBeat = 60f / beatsPerMinute;
        double beatCount = offsetAdjustedPositionSeconds / secondsPerBeat;
        return beatCount;
        //based on the current time, return the time converted to beats in the measure
    }
    //returns which beat we are on relative to the pattern
    public double GetMeasureCount(){
        double songCount = GetSongCount();
        double measureCount = songCount / patternLength;
        return measureCount;
    }

    //returns what time the beat should hit on based on the start time
    public double GetBeatTime(double beat){
        double secondsPerBeat = 60f / beatsPerMinute;
        double beatHitTime = _startTime + START_TIME_OFFSET_SECONDS + (beat * secondsPerBeat);
        return beatHitTime;
    }

    public double GetPatternBeatHitTime(int beatIndexInPattern, bool localToPattern = false){
        int index = beatIndexInPattern % beatPattern.Length;
        double beat = (double)beatPattern[index];
        if (!localToPattern){ 
            int numberOfLoops = (int)(beatIndexInPattern / beatPattern.Length);
            beat = beat + (numberOfLoops * patternLength);
        }
        return GetBeatTime(beat);
    }



    
}