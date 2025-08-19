using UnityEngine;

public class FuelVisualizer : MonoBehaviour{
    private FuelManager _fuelManager;
    private LineRenderer _lineRenderer;

    void Start(){
        _fuelManager = GetComponent<FuelManager>();
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update(){
        LineRendererUtility.MakeRing(_lineRenderer, 0.6f, 50, 0f, (_fuelManager.Fuel / _fuelManager.MaxFuel) * 360f);
    }
}