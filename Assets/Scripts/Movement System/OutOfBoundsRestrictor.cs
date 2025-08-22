using UnityEngine;

public class OutOfBoundsRestrictor : MonoBehaviour{
    public float viewportPadding = 0.05f;

    private void Update(){
        var cam = Camera.main;
        if (cam == null) return;

        var viewportPoint = cam.WorldToViewportPoint(transform.position);

        float min = 0f + viewportPadding;
        float max = 1f - viewportPadding;

        var clampedViewportPoint = new Vector3(
            Mathf.Clamp(viewportPoint.x, min, max),
            Mathf.Clamp(viewportPoint.y, min, max),
            viewportPoint.z
        );

        if (clampedViewportPoint.x != viewportPoint.x || clampedViewportPoint.y != viewportPoint.y){
            var world = cam.ViewportToWorldPoint(clampedViewportPoint);
            world.z = transform.position.z; // preserve original z plane
            transform.position = world;
        }
    }
}