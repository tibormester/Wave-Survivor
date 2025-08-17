using UnityEngine;

public interface IEnemyState{

    void Enter();
    void Tick();
    void Exit();

    bool IsValid();
    int GetPriority();
}
