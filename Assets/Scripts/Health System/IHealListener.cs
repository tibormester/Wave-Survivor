using UnityEngine;

public interface IHealListener
{
    void OnHeal(int healAmount, int newHealth, int maxHealth, GameObject source);
}