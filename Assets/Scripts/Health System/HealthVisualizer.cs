using UnityEngine;

public class HealthVisualizer : MonoBehaviour, IDamageListener, IHealListener{
    
    private LineRenderer _lineRenderer;
    
    void Start(){
        _lineRenderer = GetComponent<LineRenderer>();
        updateVisuals(1, 1);
    }

    public void OnDamage(float damageAmount, float newHealth, float maxHealth, GameObject source){
        updateVisuals(newHealth, maxHealth);
    }

    public void OnHeal(float healAmount, float newHealth, float maxHealth, GameObject source){
        updateVisuals(newHealth, maxHealth);
    }
    private void updateVisuals(float newHealth, float maxHealth){
        LineRendererUtility.MakeRing(_lineRenderer, 0.55f, 50, 0f, (newHealth / maxHealth) * 360f);
    }


    /**
    public void updateVisuals(int newHealth, int maxHealth){
        if (_renderer == null){
            return;
        }

        var healthPercentage = Mathf.Clamp01(maxHealth > 0 ? (float)newHealth / maxHealth : 0f);

        // Map health -> alpha and emission intensity
        // Example mapping: lower health = lower alpha and lower emission
        float alpha = Mathf.Lerp(0.2f, 1f, healthPercentage);
        float emissionMultiplier = healthPercentage; // scale emission linearly by health

        // Prepare colors
        var baseColorWithAlpha = new Color(_baseColor.r, _baseColor.g, _baseColor.b, alpha);
        var emissionScaled = _emissionColor * emissionMultiplier;

        // Apply using MaterialPropertyBlock for per-instance changes
        _renderer.GetPropertyBlock(_mpb);

        // Prefer URP Lit property names, with fallbacks for shaders that use _Color
        if (_renderer.sharedMaterial != null && _renderer.sharedMaterial.HasProperty("_BaseColor")){
            _mpb.SetColor("_BaseColor", baseColorWithAlpha);
        } else {
            _mpb.SetColor("_Color", baseColorWithAlpha);
        }

        if (_renderer.sharedMaterial != null && _renderer.sharedMaterial.HasProperty("_EmissionColor")){
            _mpb.SetColor("_EmissionColor", emissionScaled);
        }

        _renderer.SetPropertyBlock(_mpb);
    }
    **/
}