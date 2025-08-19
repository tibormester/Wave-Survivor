using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    protected GameObject SourceObject;

    public void Initialize(GameObject source)
    {
        SourceObject = source;
    }
}
