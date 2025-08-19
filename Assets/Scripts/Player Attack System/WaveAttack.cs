using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WaveAttack : Attack
{
    public float waveSpeed = 1f;
    public float waveMaxSize = 5f;
    public float waveDamage = 1f;

    public float waveWidth = 0.2f;
    public float minAngle = 0f;
    public float maxAngle = 45f;

    private HashSet<HealthController> _alreadyHit = new HashSet<HealthController>();
    private LineRenderer _lineRenderer;
    public float currentRadius = 0f;

    const int RING_SEGMENTS = 100;

    void Start(){
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.startWidth = waveWidth;
        _lineRenderer.endWidth = waveWidth;
        LineRendererUtility.MakeRing(_lineRenderer, currentRadius, RING_SEGMENTS, minAngle, maxAngle);
    }

    // Update is called once per frame
    void Update()
    {
        currentRadius += waveSpeed * Time.deltaTime;
        LineRendererUtility.MakeRing(_lineRenderer, currentRadius, RING_SEGMENTS, minAngle, maxAngle);
        CheckForCollisions();
        if (currentRadius > waveMaxSize){
            Destroy(gameObject);
        }
    }

    private void CheckForCollisions(){
        var allColliders = Physics2D.OverlapCircleAll(transform.position, currentRadius);
        var innerColliders = Physics2D.OverlapCircleAll(transform.position, currentRadius - waveWidth);
        var waveColliders = allColliders.Where(collider => 
            !innerColliders.Contains(collider) &&
            IsInArc(collider.transform.position)
        ).ToList();

        foreach (var collider in waveColliders){
            OnCollision(collider);
        }
    }

    private bool IsInArc(Vector3 point){
        var angle = Mathf.Atan2(point.y - transform.position.y, point.x - transform.position.x) * Mathf.Rad2Deg;
        return angle >= minAngle && angle <= maxAngle;
    }

    private void OnCollision(Collider2D other){
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
