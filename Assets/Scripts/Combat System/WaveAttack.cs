using UnityEngine;
using System.Collections.Generic;

public class WaveAttack : Attack
{
    public float waveSpeed = 1f;
    public float waveMaxSize = 10f;
    public float waveDamage = 1f;

    private HashSet<HealthController> _alreadyHit = new HashSet<HealthController>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localScale += new Vector3(1f, 1f, 0f) * waveSpeed * Time.deltaTime;
        if (transform.localScale.x > waveMaxSize){
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
