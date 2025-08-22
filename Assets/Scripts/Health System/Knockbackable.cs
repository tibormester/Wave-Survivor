using UnityEngine;

public class Knockbackable : MonoBehaviour, IDamageListener, IMovementDecorator {


    public float knockbackDamageMultiplier = 1f;
    public float knockbackSpeed = 1f;
    public float maxKnobackTime = 1.5f;


    private Vector3 queuedKnockbackDistance = Vector3.zero;
    private float knockbackTimer = 0f;

    public void OnDamage(float damageAmount, float newHealth, float maxHealth, GameObject source){
        var knockbackDirection = (transform.position - source.transform.position).normalized;
        queuedKnockbackDistance += knockbackDirection * damageAmount * knockbackDamageMultiplier;
        knockbackTimer = 0f;
    }

    public float ModifyBaseSpeed(float baseSpeed){
        return baseSpeed;
    }

    public Vector3 ModifyMovement(Vector3 movement){
        float knockbackMagnitude = queuedKnockbackDistance.magnitude;
        if (knockbackMagnitude > 0f){
            float maxKnockbackThisFrame = knockbackSpeed * Time.deltaTime * (knockbackMagnitude / 5f);
            Vector3 knockbackThisFrame;
            if (knockbackMagnitude > maxKnockbackThisFrame){
                knockbackThisFrame = queuedKnockbackDistance.normalized * maxKnockbackThisFrame;
                queuedKnockbackDistance -= knockbackThisFrame;
            } else {
                knockbackThisFrame = queuedKnockbackDistance;
                queuedKnockbackDistance = Vector3.zero;
            }
            movement += knockbackThisFrame;
            knockbackTimer += Time.deltaTime;
            if (knockbackTimer >= maxKnobackTime){
                queuedKnockbackDistance = Vector3.zero;
            }
        }
        return movement;
    }

}