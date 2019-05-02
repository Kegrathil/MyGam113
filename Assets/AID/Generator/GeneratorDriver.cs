using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GeneratorDriver : MonoBehaviour {

    public List<Generator> generators = new List<Generator>();
    private GeneratorEvalParams p = new GeneratorEvalParams();

	public void Reset()
	{
		for(int i = 0; i < generators.Count; i++)
		{
			generators[i].Reset();
		}
	}

    public float GenerateIncrement(float increment)
    {
        p.Reset(increment);
        
        for(int i = 0; i < generators.Count; i++)
        {
            generators[i].Evaluate(p);
        }
        
        return p.currentVal;
    }
}
