//Purpose is to syncrhonize the music with the rings that travel from the origin towards the radius of the player
//The rings are drawn using the LineRendererUtility class and the linerenderer is attached to the player
//The audio is tbd, the rhythm pattern is also tbd, for now, lets just keep it a constant 60 bpm

using UnityEngine;
using System.Collections.Generic;
using System;

public class RhythmDisplayer : MonoBehaviour{

    public float maxRadius = 1.5f;
    public float beatsPerMinute = 60f;
    public float[] rhythmBeatPatternCounts = {1f, 2f, 3f, 4f};
    public float beatsLengthCount = 4f;
    public float ringPreviewTimeSeconds = 0.5f;

    private const int ringSegments = 100;


    private LineRenderer _lineRenderer;

    private float _secondsPerBeat;
    private float _measureDurationSeconds;
    private float _measureZeroStartTime;

    public class RingInstance{
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