using System;
using UnityEngine;

public class Beat : MonoBehaviour
{
    //This is set by the player attack manager and read by others
    public bool playerHit = false;

    
    //These are set in the initialization method by the beat manager
    public double hitTime {get; private set;}
    private double spawnTime;
    
    private double initialRadius;
    private double finalRadius;

    public event Action OnExpire;

    private LineRenderer _lineRenderer;

    public void Initialize(float spawnTime, float hitTime, float initialRadius = 1.5f, float finalRadius = 0f){
        this.spawnTime = spawnTime;
        this.hitTime = hitTime;
        this.initialRadius = initialRadius;
        this.finalRadius = finalRadius;
    }

    public float GetPercentageToHitTime(){
        return  (float)((AudioSettings.dspTime - spawnTime) / (hitTime - spawnTime));
    }

    void Start(){
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update(){
        float progressPercentage = GetPercentageToHitTime();
        if (progressPercentage >= 1f){
            Expire();
        } else {
            LineRendererUtility.MakeRing(_lineRenderer, progressPercentage, 100);
        }
    }

    public void Expire(){
        OnExpire?.Invoke();
        Destroy(gameObject);
    }
}
