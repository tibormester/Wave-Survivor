using UnityEngine;
using System.Collections.Generic;

public class WaveAttack : Attack
{
    public float waveSpeed = 1f;
    public float waveMaxSize = 5f;
    public float waveDamage = 1f;

    private HashSet<HealthController> _alreadyHit = new HashSet<HealthController>();
    private LineRenderer _lineRenderer;
    private CircleCollider2D _circleCollider;
    public float currentRadius = 0f;

    const int RING_SEGMENTS = 100;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start(){
        _lineRenderer = GetComponent<LineRenderer>();
        _circleCollider = GetComponent<CircleCollider2D>();
        LineRendererUtility.MakeRing(_lineRenderer, currentRadius, RING_SEGMENTS);
    }

    // Update is called once per frame
    void Update()
    {
        currentRadius += waveSpeed * Time.deltaTime;
        LineRendererUtility.MakeRing(_lineRenderer, currentRadius, RING_SEGMENTS);
        _circleCollider.radius = currentRadius;

        if (currentRadius > waveMaxSize){
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other){
        Debug.Log("WaveAttack: OnTriggerEnter " + other.name);
        if (other == null) return;

        // Ignore source and its hierarchy
        if (SourceObject != null && other.transform.IsChildOf(SourceObject.transform)) return;

        var health = other.GetComponentInParent<HealthController>();
        if (health == null) return;
        if (SourceObject != null && health.transform.IsChildOf(SourceObject.transform)) return;
        if (_alreadyHit.Contains(health)) return;

        health.AdjustHealth(-(int)waveDamage, gameObject);
        _alreadyHit.Add(health);
    }

}
