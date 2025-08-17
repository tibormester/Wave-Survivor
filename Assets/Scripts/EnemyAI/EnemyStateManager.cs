using UnityEngine;

public class EnemyStateManager : MonoBehaviour{

    public IEnemyState currentState;

    void Start(){
    }

    void Update(){
        int highestPriority = -999999999;
        IEnemyState nextState = null;
        foreach (var state in GetComponents<IEnemyState>()){
            if (state.IsValid()){
                int priority = state.GetPriority();
                if (highestPriority == -999999999 || priority > highestPriority){
                    nextState = state;
                    highestPriority = state.GetPriority();
                }
            }
        }
        // Exit the current state if it is not the next state
        if (currentState != null && (currentState != nextState || nextState == null)){
            currentState.Exit();
            currentState = null;
        }
        // Enter the next state if it is not the current state
        if (nextState != null){
            if (currentState != nextState){
                currentState = nextState;
                currentState.Enter();
            }
        }
        // Tick the current state
        if (currentState != null){
            currentState.Tick();
        }   
    }
}
