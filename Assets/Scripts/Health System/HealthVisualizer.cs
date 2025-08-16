using UnityEngine;

public class HealthVisualizer : MonoBehaviour, IDamageListener, IHealListener{

    public void OnDamage(int damageAmount, int newHealth, int maxHealth, GameObject source){
        Debug.Log("HealthVisualizer: OnDamage called with damageAmount: " + damageAmount + ", newHealth: " + newHealth + ", maxHealth: " + maxHealth);
    }

    public void OnHeal(int healAmount, int newHealth, int maxHealth, GameObject source){
        Debug.Log("HealthVisualizer: OnHeal called with healAmount: " + healAmount + ", newHealth: " + newHealth + ", maxHealth: " + maxHealth);
    }

    public void updateVisuals(int newHealth, int maxHealth){
        var healthPercentage = (float)newHealth / maxHealth;
    }
}