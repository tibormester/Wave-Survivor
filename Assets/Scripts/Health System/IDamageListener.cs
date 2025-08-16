using UnityEngine;

public interface IDamageListener
{
    void OnDamage(int damageAmount, int newHealth, int maxHealth);
}