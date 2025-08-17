//Purpose is to syncrhonize the music with the rings that travel from the origin towards the radius of the player
//The rings are drawn using the LineRendererUtility class and the linerenderer is attached to the player
//The audio is tbd, the rhythm pattern is also tbd, for now, lets just keep it a constant 60 bpm

using UnityEngine;
using System.Collections.Generic;
using System;

public class RhythmDisplayer : MonoBehaviour{

    //the audio and rhythm manager class - should only manage the audio playback and holding the rhythm pattern data
    // could separate the audio syncing from the rhythm pattern management - but might overcomplicate things - for now chunk it, separate later
    public float maxRadius = 1.5f; // the player's radius - belongs in the ring previewer class - one that displays the rings as they are about to arrive
        // the beat manager class - this should manage spawning new beats and configuring them, the beats should then be self sufficient for updating and deleting themselves
        // the beat manager class should expose an API for attempting the next beat and getting the result - accuracy as a percentage.

        // the rhythm manager class - has a music sync component - it also has a rhythm sync api exposing when each beat should happen
        
        // the beat class - this stores beat data - like its start and expire time, as well as if and when it was attempted. 

        //As a result of the beat - we want to trigger attacks that scale with the beat accuracy - lore wise- these are continuations, but logically it makes sesne for them to be distinct
        //So the attack class needs an initialization method that accepts the beat accuracy
        // The beat class needs an on expiry method or event that triggers the attack class to get spawned with the right initializations

        // the player attack manager class - handles recieving player input - querrying the rhythm manager for the next beat, and then triggering the appropriate attack based on the game state and the beat accruacy
        // - scope is taking user input and turning it into the appropriate attack - using the rhythm manager's api to get the appropriate beat, and using the beat's api to get the accuracy and timing.


    
    public float beatsPerMinute = 60f;  // part of the audio / rhythm manager  - sync the audio with the rhythm pattern
    public float[] rhythmBeatPatternCounts = {1f, 1.5f, 3.5f, 4f}; // ditto
    public float beatsLengthCount = 4f; // ditto
    public float ringPreviewTimeSeconds = 0.5f; // This goes into the previewer class

    private const int ringSegments = 100; // this also into the previewer class


    private LineRenderer _lineRenderer; // this is the ring previewer class

    private float _secondsPerBeat; // this is the audio / rhythm manager class
    private float _measureDurationSeconds; // this is the audio / rhythm manager class
    private float _measureZeroStartTime; // this is the audio / rhythm manager class

    public class RingInstance{ // this can be converted to a full time gamobject with it's own monobehaviours - it'll need a line renderer and a ring controller to handle the logic of shrinking/ growing on time and stuff...
        //ideally i would want a pool pattern or like an ECS pattern to handle all the rings - 
        public LineRenderer renderer;
        public float spawnTime;
        public float hitTime;

        public float hitPercentage = -1f;

        public event Action<RingInstance> OnHit;

        public float GetPercentageToHitTime(){
            return hitPercentage == -1f ? (Time.time - spawnTime) / (hitTime - spawnTime) : -1f;
        }

        public void triggerHit(){
            OnHit?.Invoke(this);
        }

    }

    private readonly List<RingInstance> _activeRings = new List<RingInstance>();

    private int _nextMeasureIndexToSchedule = 0;
    private int _nextPatternIndexToSchedule = 0;
    private float[] _sortedPattern;



    void Start(){
        _lineRenderer = GetComponent<LineRenderer>();

        _secondsPerBeat = 60f / Mathf.Max(1f, beatsPerMinute);
        _measureDurationSeconds = _secondsPerBeat * Mathf.Max(1f, beatsLengthCount);

        // Align so that the first ring spawns immediately, reaching maxRadius on the first beat
        _measureZeroStartTime = Time.time + ringPreviewTimeSeconds;

        // Work on a sorted copy to ensure temporal order within a measure
        _sortedPattern = (float[])rhythmBeatPatternCounts.Clone();
        System.Array.Sort(_sortedPattern);
    }

    void Update(){
        // Schedule any rings whose spawn time has arrived (looping over measures)
        ScheduleDueRings();

        // Update active rings and retire those that have reached the beat
        for (int i = _activeRings.Count - 1; i >= 0; i--){
            var ring = _activeRings[i];
            float t = Mathf.InverseLerp(ring.spawnTime, ring.hitTime, Time.time);
            float radius = Mathf.Clamp01(t) * maxRadius;
            LineRendererUtility.MakeRing(ring.renderer, radius, ringSegments);

            if (Time.time >= ring.hitTime){
                if (ring.renderer != null){
                    Destroy(ring.renderer.gameObject);
                }
                ring.triggerHit();
                _activeRings.RemoveAt(i);
            }
        }
    }

    //The beat manager - needs to know that if you miss a beat, give a brief window to ignore the next beat, so that you don't too harshly penalized...? or maybe thats feature creep?
    public RingInstance GetBestRing(){
        float bestPercentage = 0f;
        RingInstance bestRing = null;
        foreach (var ring in _activeRings){
            float percentage = ring.GetPercentageToHitTime();
            if (percentage > bestPercentage){
                bestPercentage = percentage;
                bestRing = ring;
            }
        }
        if (bestRing != null){
            bestRing.hitPercentage = bestPercentage;
            bestRing.renderer.material.color = Color.blue;
        }
        return bestRing;
    }

    private void ScheduleDueRings(){
        // Loop to catch up if multiple spawns were crossed in one frame
        while (true){
            float? nextSpawnTime = GetNextSpawnTime();
            if (!nextSpawnTime.HasValue || Time.time < nextSpawnTime.Value){
                break;
            }

            float hitTime = GetAbsoluteBeatTime(_nextMeasureIndexToSchedule, _sortedPattern[_nextPatternIndexToSchedule]);
            SpawnRing(nextSpawnTime.Value, hitTime);

            AdvanceSchedulePointer();
        }
    }

    private float? GetNextSpawnTime(){
        if (_sortedPattern == null || _sortedPattern.Length == 0){
            return null;
        }

        // Validate pattern value and clamp to [1, beatsLengthCount]
        float beatCount = Mathf.Clamp(_sortedPattern[_nextPatternIndexToSchedule], 1f, beatsLengthCount);
        float hitTime = GetAbsoluteBeatTime(_nextMeasureIndexToSchedule, beatCount);
        return hitTime - ringPreviewTimeSeconds;
    }

    private float GetAbsoluteBeatTime(int measureIndex, float beatCount){
        // beatCount is 1-based within the measure
        float measureStart = _measureZeroStartTime + (measureIndex * _measureDurationSeconds);
        float offsetBeats = (beatCount - 1f);
        return measureStart + (offsetBeats * _secondsPerBeat);
    }


    private void AdvanceSchedulePointer(){
        _nextPatternIndexToSchedule++;
        if (_nextPatternIndexToSchedule >= _sortedPattern.Length){
            _nextPatternIndexToSchedule = 0;
            _nextMeasureIndexToSchedule++;
        }
    }

//this will be replaced by the beat manager class
    private void SpawnRing(float spawnTime, float hitTime){
        var lr = CreateRingRenderer();
        // Initialize at radius 0 on spawn; if we're late, we still compute based on time in Update
        LineRendererUtility.MakeRing(lr, 0f, ringSegments);
        _activeRings.Add(new RingInstance{
            renderer = lr,
            spawnTime = spawnTime,
            hitTime = hitTime
        });
    }

//  This won't be needed once the beat 'ring' is a full gameobject with it's own monobehaviour's 
    private LineRenderer CreateRingRenderer(){
        // Clone a lightweight LineRenderer using the attached one as a template
        var go = new GameObject("RhythmRing");
        go.transform.SetParent(transform, false);
        var lr = go.AddComponent<LineRenderer>();

        if (_lineRenderer != null){
            lr.sharedMaterial = _lineRenderer.sharedMaterial;
            lr.loop = true;
            lr.useWorldSpace = false;
            lr.startWidth = _lineRenderer.startWidth;
            lr.endWidth = _lineRenderer.endWidth;
            lr.widthMultiplier = _lineRenderer.widthMultiplier;
            lr.numCornerVertices = _lineRenderer.numCornerVertices;
            lr.numCapVertices = _lineRenderer.numCapVertices;
            lr.textureMode = _lineRenderer.textureMode;
            lr.alignment = _lineRenderer.alignment;
            lr.colorGradient = _lineRenderer.colorGradient;

            // Keep render ordering similar
            lr.sortingLayerID = _lineRenderer.sortingLayerID;
            lr.sortingOrder = _lineRenderer.sortingOrder;
        } else {
            lr.loop = true;
            lr.useWorldSpace = false;
        }

        return lr;
    }
    
}