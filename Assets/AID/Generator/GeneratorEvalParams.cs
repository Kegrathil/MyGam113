using UnityEngine;
using System.Collections;

[System.Serializable]
public class GeneratorEvalParams : System.Object
{
    public float increment;
    public float currentVal;
    public Vector2 currentWindow;
    public bool first = true;

    public void Reset(float newIncrement)
    {
        increment = newIncrement;
        currentVal = 0;
        currentWindow = Vector2.zero;
        first = true;
    }
}