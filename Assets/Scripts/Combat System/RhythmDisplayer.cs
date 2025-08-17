//Purpose is to syncrhonize the music with the rings that travel from the origin towards the radius of the player
//The rings are drawn using the LineRendererUtility class and the linerenderer is attached to the player
//The audio is tbd, the rhythm pattern is also tbd, for now, lets just keep it a constant 60 bpm

using UnityEngine;

public class RhythmDisplayer : MonoBehaviour{

    private LineRenderer _lineRenderer;

    void Start(){
        _lineRenderer = GetComponent<LineRenderer>();
    }

    void Update(){
        LineRendererUtility.MakeRing(_lineRenderer, 10f, 100);
    }
}