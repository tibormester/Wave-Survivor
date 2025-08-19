using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MovementController))]
[RequireComponent(typeof(FuelManager))]
public class BlinkAbility : MonoBehaviour, IMovementDecorator, IMovementAbility
{
    public float blinkDuration = 0.1f;
    public float blinkFuelCost = 100f;

    private bool _isBlinking = false;
    private Vector3 _queuedBlink = Vector3.zero;
    private float _noteAccuracy = 0f;

    private FuelManager _fuelManager;
    private NoteManager _noteManager;
    private MovementController _movementController;
    private HealthController _healthController;
    void Start(){
        _fuelManager = GetComponent<FuelManager>();
        _noteManager = GetComponent<NoteManager>();
        _movementController = GetComponent<MovementController>();
        _healthController = GetComponent<HealthController>();
    }

    public float ModifyBaseSpeed(float baseSpeed)
    {
        return _isBlinking ? 0 : baseSpeed;
    }

//TODO: This logic could be improved, its just copy and pasted from the knockbackable component
    public Vector3 ModifyMovement(Vector3 movement)
    {
        return _isBlinking ? Vector3.zero : movement;
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.LeftShift) && _fuelManager.TryUseFuel(blinkFuelCost)){
            StartCoroutine(Blink());
        }
    }
    private IEnumerator Blink(){
        _isBlinking = true;
        _queuedBlink = GetBlinkDirection();
        _healthController.isInvincible = true;
        yield return new WaitForSeconds(blinkDuration);

        transform.position = _queuedBlink;
        _isBlinking = false;
        _healthController.isInvincible = false;
    }


    private Vector3 GetBlinkDirection(){
        var mousePosition = Input.mousePosition;
        // Distance from camera to z=0 plane in world space (orthographic)
        mousePosition.z = -Camera.main.transform.position.z;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePosition);
        worldPos.z = 0f; // keep on 2D plane

        return worldPos;
    }

}