using UnityEngine;

public interface IHealListener
{
    void OnHeal(float healAmount, float newHealth, float maxHealth, GameObject source);
}