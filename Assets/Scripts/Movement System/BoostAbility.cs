using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(FuelManager))]
public class BoostAbility : MonoBehaviour, IMovementDecorator, IMovementAbility
{
    public float boostMultiplier = 1.85f;
    public float boostFuelCostSecond = 25f;

    private bool isBoosting = false;
    private float _noteAccuracy = 0f;
    private FuelManager _fuelManager;
    private NoteManager _noteManager;
    void Start(){
        _fuelManager = GetComponent<FuelManager>();
        _noteManager = GetComponent<NoteManager>();
    }

    public float ModifyBaseSpeed(float baseSpeed)
    {
        if (isBoosting && _fuelManager.TryUseFuel(boostFuelCostSecond * (1 - (Mathf.Max(_noteAccuracy -0.8f, 0f) * 3f)) * Time.deltaTime)){
            return baseSpeed * (boostMultiplier + (Mathf.Max(_noteAccuracy -0.8f, 0f) * 3f));
        }
        else{
            return baseSpeed;
        }
    }

    public Vector3 ModifyMovement(Vector3 movement)
    {
        return movement;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.LeftShift)){
            isBoosting = true;
            Note note = _noteManager.GetNextNote();
            _noteAccuracy = note?.GetPercentageToHitTime() ?? 0f;
        }
        if (!Input.GetKey(KeyCode.LeftShift)){
            isBoosting = false;
        }
    }


}