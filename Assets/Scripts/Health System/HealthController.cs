using UnityEngine;
using System;

//TODO this class is pretty messy, but it basically validates adjusting the health to be within 0 to maxHealth and calls the right components when that happns
public class HealthController : MonoBehaviour
{
    
    public int maxHealth = 100;
    private int _health;

    public event Action healthZeroed;
    
    void Start(){
        _health = maxHealth;
    }

    public void SetHealth(int value){
        var delta = value - _health;
        AdjustHealth(delta, gameObject);
    }

    public void AdjustHealth(int delta, GameObject source){
        if (delta > 0 && _health < maxHealth){
            if (_health + delta > maxHealth){
                Heal(maxHealth - _health, source);
            } else {
                Heal(delta, source);
            }
        } else if (delta < 0 && _health > 0) {
            TakeDamage(-delta, source);
        } else{
            Debug.LogWarning("HealthController: AdjustHealth called with invalid delta");
            return;
        }
    }

    
    private void TakeDamage(int damageAmount, GameObject source)
    {
        _health -= damageAmount;
        foreach (var listener in GetComponents<IDamageListener>())
        {
            listener.OnDamage(damageAmount, _health, maxHealth, source);
        }
        if (_health <= 0){
            healthZeroed?.Invoke();
        }
    }

    private void Heal(int healAmount, GameObject source)
    {
        _health += healAmount;
        foreach (var listener in GetComponents<IHealListener>())
        {
            listener.OnHeal(healAmount, _health, maxHealth, source);
        }
    }

}
