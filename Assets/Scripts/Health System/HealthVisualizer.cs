using UnityEngine;

public class HealthVisualizer : MonoBehaviour, IDamageListener, IHealListener{
    
    // Cached renderer and material property block for per-instance overrides
    private Renderer _renderer;
    private MaterialPropertyBlock _mpb;
    
    // Store base colors so we can scale them by health without mutating the shared material
    private Color _baseColor;
    private Color _emissionColor;
    
    void Awake(){
        _renderer = GetComponent<Renderer>();
        _mpb = new MaterialPropertyBlock();
        
        if (_renderer != null){
            // Read base values from the shared material (do not mutate it)
            var mat = _renderer.sharedMaterial;
            if (mat != null){
                // Prefer URP Lit names, but gracefully fall back to legacy names when needed
                _baseColor = mat.HasProperty("_BaseColor") ? mat.GetColor("_BaseColor") :
                    (mat.HasProperty("_Color") ? mat.GetColor("_Color") : Color.white);
                _emissionColor = mat.HasProperty("_EmissionColor") ? mat.GetColor("_EmissionColor") : Color.black;
            }
        }
        updateVisuals(1, 1);
    }

    public void OnDamage(int damageAmount, int newHealth, int maxHealth, GameObject source){
        Debug.Log("HealthVisualizer: OnDamage called with damageAmount: " + damageAmount + ", newHealth: " + newHealth + ", maxHealth: " + maxHealth);
        updateVisuals(newHealth, maxHealth);
    }

    public void OnHeal(int healAmount, int newHealth, int maxHealth, GameObject source){
        Debug.Log("HealthVisualizer: OnHeal called with healAmount: " + healAmount + ", newHealth: " + newHealth + ", maxHealth: " + maxHealth);
        updateVisuals(newHealth, maxHealth);
    }

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
}