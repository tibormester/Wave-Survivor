using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class WaveAttack : MonoBehaviour
{
    public float waveSpeed = 1f;
    public float waveMaxSize = 5f;
    public float waveDamage = 1f;

    public float minAngle = 0f;
    public float maxAngle = 45f;

    public float waveWidth = 0.5f;
    

    public GameObject sourceObject;

    private HashSet<HealthController> _alreadyHit = new HashSet<HealthController>();
    private LineRenderer _lineRenderer;
    public float currentRadius = 0f;

    const int RING_SEGMENTS = 100;

    public void InitializeAttack(float damage, float speed, float range, float spread, float centerAngle){
        waveDamage = damage;
        waveSpeed = speed;
        waveMaxSize = range;
        minAngle = centerAngle - spread / 2f;
        maxAngle = centerAngle + spread / 2f;
    }

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
        var innerRadius = (currentRadius - waveWidth < 0.01f) ? 0f : currentRadius - waveWidth;
        var innerColliders = Physics2D.OverlapCircleAll(transform.position, innerRadius);
        var waveColliders = allColliders.Where(collider => 
            !innerColliders.Contains(collider) &&
            IsInArc(collider.transform.position)
        ).ToList();

        foreach (var collider in waveColliders){
            OnCollision(collider);
        }
    }

    private bool IsInArc(Vector3 point){
        var localPoint = transform.InverseTransformPoint(point);
        var angle = Mathf.Atan2(localPoint.y, localPoint.x) * Mathf.Rad2Deg;
        //want to make the angle more forgiving since the midpoint might be outside but the collider overlaps, but the extra angle buffer needs to decrease as the wave grows
        var angleBuffer = Mathf.Lerp(0f, 10f, currentRadius / waveMaxSize);
        return angle >= (minAngle - angleBuffer) && angle <= (maxAngle + angleBuffer);
    }

    private void OnCollision(Collider2D other){
        if (other == null) return;

        // Ignore source and its hierarchy
        if (sourceObject != null && other.transform.IsChildOf(sourceObject.transform)) return;

        var health = other.GetComponentInParent<HealthController>();
        if (health == null) return;
        if (sourceObject != null && health.transform.IsChildOf(sourceObject.transform)) return;
        if (_alreadyHit.Contains(health)) return;

        health.AdjustHealth(-(int)waveDamage, gameObject);
        _alreadyHit.Add(health);
    }

}
