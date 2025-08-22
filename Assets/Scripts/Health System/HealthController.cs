using UnityEngine;
using System;
using Game.ProgressionSystem;

//TODO this class is pretty messy, but it basically validates adjusting the health to be within 0 to maxHealth and calls the right components when that happns
public class HealthController : MonoBehaviour, IUpgradeable
{
    
    public float maxHealth = 100;
    public bool isInvincible = false;
    private float _health;

    public event Action healthZeroed;
    
    void Start(){
        _health = maxHealth;
    }

    public void SetHealth(float value){
        var delta = value - _health;
        AdjustHealth(delta, gameObject);
    }

    public void AdjustHealth(float delta, GameObject source){
        if (delta > 0 && _health < maxHealth){
            if (_health + delta > maxHealth){
                Heal(maxHealth - _health, source);
            } else {
                Heal(delta, source);
            }
        } else if (delta < 0 && _health > 0 && !isInvincible) {
            TakeDamage(-delta, source);
        } else{
            return;
        }
    }

    
    private void TakeDamage(float damageAmount, GameObject source)
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

    private void Heal(float healAmount, GameObject source)
    {
        _health += healAmount;
        foreach (var listener in GetComponents<IHealListener>())
        {
            listener.OnHeal(healAmount, _health, maxHealth, source);
        }
    }

    public void Upgrade(UpgradeType upgradeType){
        switch(upgradeType){
            case UpgradeType.MaxHealth:
                maxHealth += 10;
                break;
            case UpgradeType.CurrentHealth:
                _health = maxHealth;
                break;
        }
    }
}
