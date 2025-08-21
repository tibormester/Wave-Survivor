//Purpose is to spawn beats before their time and initilize them so that they will arrive at the correct time independent of frame rate
//Additional purpose is to expose an API for accessing these spawned beats - specifically need to find the next beat that is due to hit

using UnityEngine;
using System;
public class NoteManager : MonoBehaviour{


    public float maxRadius = 1f; // the player's radius - belongs in the ring previewer class - one that displays the rings as they are about to arrive

    public float previewTimeSeconds = 0.5f; // This goes into the previewer class
    public int currentNoteIndex = 0;

    public event Action OnNoteHit;

    //Must be sorted in chronological order - the beats are values indicate what count they appear on
    [SerializeField] private float[] notePattern = {1f, 1.5f, 2f, 3f, 3.75f, 4f};
    [SerializeField] private float patternLength = 4f; 


    [SerializeField] private Note _notePrefab;

    public BeatManager _beatManager {get; private set;}
    void Start(){
        _beatManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<BeatManager>();
        int measureCount = (int)_beatManager.GetSongCount();
    }

    private int _lastSpawnedIndex = -1;
    void Update(){

        double startTime, hitTime;
        while (CanNextNoteBeSpawned(out startTime, out hitTime)){
            _lastSpawnedIndex++;
            SpawnNote((float)startTime, (float)hitTime);
        }

        while( GetNoteHitTimeFromRhythm(currentNoteIndex) <= AudioSettings.dspTime){
            currentNoteIndex++;
            OnNoteHit?.Invoke();
        }
    }

    private bool CanNextNoteBeSpawned(out double startTime, out double hitTime){
        int nextNoteIndex = _lastSpawnedIndex + 1;
        hitTime = GetNoteHitTimeFromRhythm(nextNoteIndex);
        startTime = hitTime - previewTimeSeconds;
        if (AudioSettings.dspTime >= startTime){
            return true;
        }
        return false; 
    }

    public double GetNoteHitTimeFromRhythm(int noteIndexInPattern, bool localToPattern = false){
        int index = noteIndexInPattern % notePattern.Length;
        double note = (double)notePattern[index];
        if (!localToPattern){ 
            int numberOfLoops = (int)(noteIndexInPattern / notePattern.Length);
            note = note + (numberOfLoops * patternLength);
        }
        return _beatManager.GetBeatTime(note);
    }
     //Spawns the beat and configure it with the correct initial and final radius
    private Note SpawnNote(float spawnTime, float hitTime){
        var note = Instantiate(_notePrefab, transform);
        note.Initialize(spawnTime, hitTime, maxRadius, 0f);

        return note;
    }


    // The public api to get the next beat that is due to hit
    public Note GetNextNote(){
        var notes = GetComponentsInChildren<Note>();
        Note bestNote = null;
        foreach (var note in notes){
            if (bestNote == null || note.hitTime < bestNote.hitTime){
                bestNote = note;
            }
        }
        return bestNote;
    }

   

    
}