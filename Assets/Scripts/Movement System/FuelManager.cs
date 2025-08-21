using UnityEngine;
using Game.ProgressionSystem;
public class FuelManager : MonoBehaviour, IUpgradeable{


    public float MaxFuel = 100f;
    [SerializeField] private float RefuelRate = 15f;
    
    public float Fuel {get; private set;}


    void Start(){
        Fuel = MaxFuel;
    }

    void Update(){
        Fuel += RefuelRate * Time.deltaTime;
        if (Fuel > MaxFuel){
            Fuel = MaxFuel;
        }
    }

    public bool TryUseFuel(float amount){
        if (Fuel >= amount){
            Fuel -= amount;
            return true;
        }
        return false;
    }

    public void Upgrade(UpgradeType upgradeType){
        switch(upgradeType){
            case UpgradeType.MaxFuel:
                MaxFuel += 10f;
                break;
            case UpgradeType.FuelRecharge:
                RefuelRate += 1f;
                break;
            case UpgradeType.CurrentFuel:
                Fuel = MaxFuel;
                break;
        }
    }

}
