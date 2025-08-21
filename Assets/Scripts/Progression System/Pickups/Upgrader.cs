using UnityEngine;
using Game.ProgressionSystem;

[RequireComponent(typeof(Pickupable))]
//Determines the type of upgrade, and responds by upgrading all upgradable components with the upgrade type
public class Upgrader : MonoBehaviour
{
    [SerializeField] private UpgradeType upgradeType;
    void Start(){
        GetComponent<Pickupable>().OnPickup += Upgrade;
    }

    public void Upgrade(GameObject player){
        foreach(var upgradable in player.GetComponents<IUpgradeable>()){
            upgradable.Upgrade(upgradeType);
        }
    }
}
