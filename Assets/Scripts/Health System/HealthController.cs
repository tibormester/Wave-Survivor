using UnityEngine;


//TODO this class is pretty messy, but it basically validates adjusting the health to be within 0 to maxHealth and calls the right components when that happns
public class HealthController : MonoBehaviour
{
    
    public int maxHealth = 100;
    private int _health;
    
    void Start(){
        _health = maxHealth;
    }

    public void SetHealth(int value){
        var delta = value - _health;
        AdjustHealth(delta);
    }

    public void AdjustHealth(int delta){
        if (delta > 0 && _health < maxHealth){
            if (_health + delta > maxHealth){
                Heal(maxHealth - _health);
            } else {
                Heal(delta);
            }
        } else if (delta < 0 && _health > 0) {
            if (_health + delta < 0){
                if (_health < 0){
                    TakeDamage(-_health);
                } else {
                    TakeDamage(-delta);
                }
            } else {
                TakeDamage(-delta);
            }
        } else{
            Debug.LogWarning("HealthController: AdjustHealth called with invalid delta");
            return;
        }
    }

    
    private void TakeDamage(int damageAmount)
    {
        _health -= damageAmount;
        foreach (var listener in GetComponents<IDamageListener>())
        {
            listener.OnDamage(damageAmount, _health, maxHealth);
        }
    }

    private void Heal(int healAmount)
    {
        _health += healAmount;
        foreach (var listener in GetComponents<IHealListener>())
        {
            listener.OnHeal(healAmount, _health, maxHealth);
        }
    }

}
