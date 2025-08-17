//Purpose is to spawn beats before their time and initilize them so that they will arrive at the correct time independent of frame rate
//Additional purpose is to expose an API for accessing these spawned beats - specifically need to find the next beat that is due to hit

using UnityEngine;
using System.Collections.Generic;

public class BeatManager : MonoBehaviour{


    public float maxRadius = 1.5f; // the player's radius - belongs in the ring previewer class - one that displays the rings as they are about to arrive

    public float previewTimeSeconds = 0.5f; // This goes into the previewer class


    [SerializeField] private Beat _beatPrefab;

    private RhythmManager _rhythmManager;

    private Dictionary<int, Beat> _beats = new Dictionary<int, Beat>();
    void Start(){
        _rhythmManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<RhythmManager>();
    }

    private int _lastSpawnedIndex = -1;
    void Update(){
        double startTime, hitTime;
        while (CanNextBeatBeSpawned(out startTime, out hitTime)){
            _lastSpawnedIndex++;
            SpawnBeat((float)startTime, (float)hitTime, _lastSpawnedIndex);
        }

    }

    private bool CanNextBeatBeSpawned(out double startTime, out double hitTime){
        int nextBeatIndex = _lastSpawnedIndex + 1;
        hitTime = _rhythmManager.GetPatternBeatHitTime(nextBeatIndex);
        startTime = hitTime - previewTimeSeconds;
        if (AudioSettings.dspTime >= startTime){
            return true;
        }
        return false; 
    }
     //Spawns the beat and configure it with the correct initial and final radius
    private Beat SpawnBeat(float spawnTime, float hitTime, int index){
        var beat = Instantiate(_beatPrefab, transform);
        beat.Initialize(spawnTime, hitTime, maxRadius, 0f);
        
        _beats[index] = beat;
        beat.OnExpire += () => {
            _beats.Remove(index);
        };

        return beat;
    }


    // The public api to get the next beat that is due to hit, filtering out any beats that have already been hit
    public Beat GetNextBeat(){
        var beats = GetComponentsInChildren<Beat>();
        Beat bestBeat = null;
        foreach (var beat in beats){
            if (beat.hitTime > Time.time){
                return beat;
            }
        }
        return bestBeat;
    }

   

    
}