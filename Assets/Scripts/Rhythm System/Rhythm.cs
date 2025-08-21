using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Plain Beat", menuName = "Rhythm/Plain Beat")]
public class Rhythm : ScriptableObject{
    public float[] notePattern = {1f, 2f, 3f, 4f};
    public float patternLength = 4f;

}