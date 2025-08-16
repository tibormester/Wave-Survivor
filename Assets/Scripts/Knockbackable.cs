using UnityEngine;

public class Knockbackable : MonoBehaviour, IDamageListener, IMovementAffector {


    public float knockbackDamageMultiplier = 1f;
    public float knockbackSpeed = 1f;

    private Vector3 queuedKnockbackDistance = Vector3.zero;

    public void OnDamage(int damageAmount, int newHealth, int maxHealth, GameObject source){
        var knockbackDirection = (transform.position - source.transform.position).normalized;
        queuedKnockbackDistance += knockbackDirection * damageAmount * knockbackDamageMultiplier;
    }

    public float ModifyBaseSpeed(float baseSpeed){
        return baseSpeed;
    }

    public Vector3 ModifyMovement(Vector3 movement){
        float knockbackMagnitude = queuedKnockbackDistance.magnitude;
        if (knockbackMagnitude > 0f){
            float maxKnockbackThisFrame = knockbackSpeed * Time.deltaTime;
            Vector3 knockbackThisFrame;
            if (knockbackMagnitude > maxKnockbackThisFrame){
                knockbackThisFrame = queuedKnockbackDistance.normalized * maxKnockbackThisFrame;
                queuedKnockbackDistance -= knockbackThisFrame;
            } else {
                knockbackThisFrame = queuedKnockbackDistance;
                queuedKnockbackDistance = Vector3.zero;
            }
            movement += knockbackThisFrame;
        }
        return movement;
    }

}