using UnityEngine;

public class EnemyStateManager : MonoBehaviour{

    public IEnemyState currentState;

    void Start(){
    }

    void Update(){
        if (currentState != null && currentState.IsValid()){
            currentState.Tick();
        } else{
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
            if (currentState != null){
                currentState.Exit();
                currentState = null;
            }
            if (nextState != null){
                currentState = nextState;
                currentState.Enter();
            }
        }
    }

}