using UnityEngine;
using System.Collections;

//TODO
//needs generator visitor
//seeds, random or saved
//better perlin
//	multifractal?
//	b


[System.Serializable]
public class Generator : System.Object 
{
    public enum CombineOp
    {
        Add,
        Sub,
        Mul,
        Div,
        Min,
        Max,
        Mid
    }

    public enum GenMethod
    {
        None,
        SineWave,
        TriangleWave,
        SquareWave,
        PerlinNoise,
        AnimCurve,
        Random
    }

    //all unaltered single generators MUST return a value in this range
	readonly static public Vector2 DefaultWindow = new Vector2(-1,1); 
	readonly static public float perlinPeriod = 100; 
    public Vector2 generationWindow = DefaultWindow;
    public bool rerangeOutGoing = false;
    public Vector2 outgoingWindow = DefaultWindow;
	//public float period = 1;
	[Range(0,1)]
    public float shift = 0;
	public bool clampToOutputRange = false;
    //public float gatePercentPivot = 0;
    //public float gatePercentMin = 0;
	//public float scale = 1;
	//public int seed;
    public CombineOp combineOp = CombineOp.Add;
    public GenMethod genMethod = GenMethod.SineWave;
    public float perlinOffset = 0;
    public AnimationCurve curve;    //set up curve so that it runs from 0-1 and will ping pong
    private float time = 0;
    public float speed = 1;

    //is fed the previous generate if one is available, if not curVal is mid of outgoing window
    public void Evaluate(GeneratorEvalParams p)//float time, float curVal, Vector2 incomingWindow)
    {
        //Vector2 expandedWindow = new Vector2(Mathf.Min(p.currentWindow.x, outgoingWindow.x),Mathf.Max(p.currentWindow.y, outgoingWindow.y));
        float curTime = IncrementAndWrap(p.increment);

        float ourVal = Generate(curTime);

        Combine(ourVal, p);

		if(p.first && genMethod != GenMethod.None)
		{
			p.currentWindow = generationWindow;
			p.first = false;
		}

        if(rerangeOutGoing)
        {
            p.currentVal = AID.UTIL.ReRange(p.currentVal, p.currentWindow.x, p.currentWindow.y, outgoingWindow.x, outgoingWindow.y);
        }

        //do we clamp, we still need to clamp even if rerange as we cannot trust others to have clamped also
        if(clampToOutputRange) p.currentVal = Mathf.Clamp(p.currentVal, outgoingWindow.x, outgoingWindow.y);

        //TODO do we gate
    }

    float Generate(float time)
    {
		float period = GetPeriod();
		time += shift*period;
		time %= period;
        float retval = 0f;
        //TODO take into account gen window
        switch(genMethod)
        {
	    case GenMethod.None:
	       // return 0f;//(generationWindow.x + generationWindow.y) * 0.5f;
	        break;
	    case GenMethod.AnimCurve:
			retval = curve.Evaluate(Mathf.PingPong(time*2,period));
	        retval *= 2f;
	        retval -= 1;
	        break;
	    case GenMethod.PerlinNoise:
	        //TODO replace with better perlin source
			const float perlinRadius = 100;
			//float x = Mathf.Sin(time/period*Mathf.PI*2),y = Mathf.Cos(time/period*Mathf.PI*2);
			retval = AID.UTIL.Perlin1DLoop(time, period, perlinRadius,perlinOffset);
			//retval = Mathf.PerlinNoise(x*perlinRadius, y*perlinRadius)*2 -1f;
			retval = retval*2 -1f;
	        break;
	    case GenMethod.SineWave:
	        retval = Mathf.Sin(time*2*Mathf.PI);
	        break;
	    case GenMethod.SquareWave:
	        retval = Mathf.Sin(time*2*Mathf.PI) > 0 ? 1f : -1f ;
	        break;
	    case GenMethod.TriangleWave:
	        //shifted to start at 0 rising
	        time += 0.25f;
	        time %= 1f;
	        retval = time *4f - 1f;
	        if(retval > 1f)
	            retval = 1f- (retval-1);
	        break;
	    case GenMethod.Random:
	        retval = Random.value;
	        break;
	    default:
	        Debug.LogError("Unhandled generate method: " + genMethod.ToString());
	        break;
        }

        //all forms gen from -1 to 1, so rerange to gen window

        return AID.UTIL.ReRange(retval,-1f,1f,generationWindow.x, generationWindow.y);
    }

    void Combine(float ourVal, GeneratorEvalParams p)
    {
		if(genMethod == GenMethod.None)
			return;

        float sign = 1;
        switch(combineOp)
        {
            case CombineOp.Div:
                p.currentVal /= ourVal;
				if(!p.first)
				{
	                // if there is any neg we must maintain it
	                if(generationWindow.x < 0|| generationWindow.y < 0 || p.currentWindow.x < 0 || p.currentWindow.y < 0)
	                    sign = -1;
	                p.currentWindow.x /= generationWindow.x * sign;
	                p.currentWindow.y /= generationWindow.y;
				}
                break;
            case CombineOp.Mul:
                p.currentVal *= ourVal;
                //if there is any neg we must maintain it
			if(!p.first)
			{
				if(generationWindow.x < 0|| generationWindow.y < 0 || p.currentWindow.x < 0 || p.currentWindow.y < 0)
                    sign = -1;
                p.currentWindow.x *= generationWindow.x * sign;
				p.currentWindow.y *= generationWindow.y;
			}
                break;
            case CombineOp.Add:
                p.currentVal += ourVal;
			if(!p.first)
			{
				p.currentWindow.x += generationWindow.x;
				p.currentWindow.y += generationWindow.y;
			}
                break;
            case CombineOp.Sub:
                p.currentVal -= ourVal;
			if(!p.first)
			{
				p.currentWindow.x -= generationWindow.x;
				p.currentWindow.y -= generationWindow.y;
			}
                break;
            case CombineOp.Min:
                p.currentVal = Mathf.Min(p.currentVal, ourVal);
			if(!p.first)
			{
				p.currentWindow.x = Mathf.Min(p.currentWindow.x, generationWindow.x);
				p.currentWindow.y = Mathf.Min(p.currentWindow.y, generationWindow.y);
			}
                break;
            case CombineOp.Max:
                p.currentVal = Mathf.Max(p.currentVal, ourVal);
			if(!p.first)
			{
				p.currentWindow.x = Mathf.Max(p.currentWindow.x, generationWindow.x);
				p.currentWindow.y = Mathf.Max(p.currentWindow.y, generationWindow.y);
			}
                break;
            case CombineOp.Mid:
                p.currentVal = (p.currentVal + ourVal)*0.5f;
			if(!p.first)
			{
				p.currentWindow.x = (p.currentWindow.x + generationWindow.x)*0.5f;
				p.currentWindow.y = (p.currentWindow.y + generationWindow.y)*0.5f;
			}
                break;
            default:
                Debug.LogError("Unhandled combine op: " + combineOp.ToString());
                break;
        }
    }
    
    
    public float IncrementAndWrap(float increment)
    {
        time += increment * speed;
        
        float retval = time;
        
		if(time > GetPeriod())
        {
			time %= GetPeriod();
            retval = time;
        }
        
        return retval;
    }

	public void Reset()
	{
		time = 0;
	}

	private float GetPeriod()
	{
		if(genMethod == GenMethod.PerlinNoise)
			return perlinPeriod;
		return 1;
	}
}

//TODO
//  allow integer speed ups of each generator, find lcm of all to find new period
//  alternatively we have to give each gen its own time counter
/*
 * static int gcf(int a, int b)
{
    while (b != 0)
    {
        int temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

static int lcm(int a, int b)
{
    return (a / gcf(a, b)) * b;
}
 */
