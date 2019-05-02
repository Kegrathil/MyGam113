using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehWFloatVar : MonoBehaviour
{
    public AID.FloatVar f;
    public AID.IntVar i, anotherInt;
    public AID.StringVar s;
    public AID.EventSOParamInt intEvent;
    public AID.AnimCurveVar curve = new AID.AnimCurveVar(AnimationCurve.Linear(0, 0, 1, 1));
    public AID.Vector3Var v3;

    // Use this for initialization
    IEnumerator Start()
    {
        intEvent.Fire(10);

        yield return new WaitForSeconds(0.5f);
        f.Value = 5;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
