using UnityEngine;

public interface IDamageListener
{
    void OnDamage(float damageAmount, float newHealth, float maxHealth, GameObject source);
}