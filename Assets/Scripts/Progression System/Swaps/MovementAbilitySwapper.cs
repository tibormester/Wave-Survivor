using UnityEngine;

public class MovementAbilitySwapper : MonoBehaviour {

    public enum MovementAbility{
        Dash,
        Blink,
        Boost,
    }
    
    public void SwapMovementAbility(MovementAbility newAbility){
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        foreach (var ability in player.GetComponents<IMovementAbility>()){ 
            var script = ability as MonoBehaviour;
            script.enabled = false;
        }
        switch (newAbility){
            case MovementAbility.Dash:
                player.GetComponent<DashAbility>().enabled = true;
                break;
            case MovementAbility.Blink:
                player.GetComponent<BlinkAbility>().enabled = true;
                break;
            case MovementAbility.Boost:
                player.GetComponent<BoostAbility>().enabled = true;
                break;
        }
    }
}