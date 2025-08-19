using UnityEngine;

public class FuelManager : MonoBehaviour{


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

}
