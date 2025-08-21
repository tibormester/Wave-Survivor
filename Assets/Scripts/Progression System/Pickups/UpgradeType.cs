namespace Game.ProgressionSystem{
    public enum UpgradeType{

        //MovementController
        PlayerSpeed,

        //HealthManager
        MaxHealth, 
        CurrentHealth, //Consumable pickup to replenish health

        //FuelManager
        MaxFuel,
        CurrentFuel, //Consumable pickup to replenish fuel
        FuelRecharge,


        //PlayerAttackController
        AttackDamage,
        AttackSpeed,
        AttackRange,
        AttackSpread,

    }
}