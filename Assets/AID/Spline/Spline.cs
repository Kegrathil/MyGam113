using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO
// stale only reticulate so we can move splines while the game is runnings
namespace AID
{
    public class Spline : MonoBehaviour {

	public delegate void DrawLineDelegate(Vector3 start, Vector3 end);
	
	public List<SplineNode> nodes = new List<SplineNode>();
	public bool isTangentsCalced  = false;
	public bool isReticulated = false;
	public bool leadIn = false;
	public bool leadOut = false;
	public bool drawTangets = true;
	public int resolution = 5;
	public Bounds bounds;
	public float lengthOfSpline;
	private bool showSplineInGame = false;

	
	public int GetNumNodes()
	{
		int retval = nodes.Count;
		if(leadIn)
		{
			retval -= 1;
		}
		
		if(leadOut)
		{
			retval -= 1;
		}
		
		return retval;
	}
	
	public int GetNumSections()
	{
		return GetNumNodes()-1;
	}
	
	public SplineNode GetNode(int n)
	{
		if(leadIn)
		{
			n++;
		}
		
		return nodes[n];
	}
	
	
	//section starts at 0, t is the percent from the start to end of that section
	public Vector3 GetPositionInSection(int section, float t)
	{
		SplineNode p1 = GetNode(section);
		SplineNode p2 = GetNode(section+1);
		
		Vector3 t1 = p1.inFragment + p1.outFragment;
		Vector3 t2 = p2.inFragment + p2.outFragment;
		
		return UTIL.CubicHermite(t1, p1.transform.position, p2.transform.position, t2, t);
	}
	
	public void Reticulate()
	{
		if(!isTangentsCalced)
			RecalcTangents();
		
		for(int i = 0; i < GetNumSections(); ++i)
		{
			nodes[i].Reticulate(nodes[i+1], resolution);
		}

		//zero out the last node in case it had bad data
		nodes[nodes.Count-1].ZeroReticulatedData();

		for(int i = 1; i < GetNumNodes(); i++)
		{
			nodes[i].distanceFromStartOfSpline = nodes[i-1].distanceFromStartOfSpline + nodes[i-1].distanceToNextNode;
		}

		lengthOfSpline = nodes[GetNumSections()].distanceFromStartOfSpline;

		isReticulated = true;
		CalcBounds();
	}
	
	public void RecalcTangents()
	{
		for(int i = 0; i < nodes.Count; ++i)
		{
			nodes[i].CalcuateTangentInfo(nodes[Mathf.Max(0,i-1)], nodes[Mathf.Min(nodes.Count-1, i+1)]);
		}
		
		isTangentsCalced = true;
	}

	public void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.yellow;
		OnDrawGizmos();
	}
	
	public void OnDrawGizmos()
	{
		DrawSplineInternal(Vector3.zero, DrawLineWithGizmo);


		Gizmos.color = Color.cyan;
		for(int i = 0; i < nodes.Count; i++)
		{
			Gizmos.DrawWireSphere(nodes[i].transform.position, 0.5f);
		}


		//leads
		if(leadIn)
		{
			Gizmos.DrawLine(nodes[0].transform.position , nodes[1].transform.position);
		}
		if(leadOut)
		{
			Gizmos.DrawLine(nodes[nodes.Count-1].transform.position , nodes[nodes.Count-2].transform.position);
		}
		Gizmos.color = Color.white;
	}
	
	public void DrawSplineInternal(Vector3 offset, DrawLineDelegate dld)
	{
		if(nodes == null || nodes.Count == 0)
		{
			return;
		}
		
		for(int i = leadIn ? 1 : 0; i < nodes.Count - (leadOut ? 2 : 1); ++i)
		{			
			if(!isTangentsCalced && !isReticulated)
			{
				dld(nodes[i].transform.position + offset, nodes[i+1].transform.position + offset);
			}
			else
			{
				for(int j = 0; j < nodes[i].reticulationPoints.Length-1; ++j)
				{
					dld(nodes[i].reticulationPoints[j] + offset, nodes[i].reticulationPoints[j+1] + offset);
				}
			}
		}
	}
	
	public void AttemptToGetNodesFromChildren()
	{
		List<SplineNode> newNodes = new List<SplineNode>();
		
		for(int i = 0; i < transform.childCount; ++i)
		{
			SplineNode spNode = transform.GetChild(i).GetComponent<SplineNode>();
			if(spNode != null)
			{
				newNodes.Add(spNode);
			}
		}
		
		newNodes.Sort(CompareByGOName);
		
		nodes = newNodes;
	}
	
	public GameObject CreateNewChildNode()
	{
		GameObject newNode = new GameObject(nodes.Count.ToString());
		SplineNode sp = newNode.AddComponent<SplineNode>();
		newNode.transform.parent = gameObject.transform;
		nodes.Add(sp);
		
		isReticulated = false;
		isTangentsCalced = false;
		
		return newNode;
	}
	
	public int CompareByGOName(SplineNode lhs, SplineNode rhs)
	{
		double lhsAsNum = 0, rhsAsNum = 0;
		
		if(double.TryParse(lhs.gameObject.name, out lhsAsNum) && double.TryParse(rhs.gameObject.name, out rhsAsNum))
		{
			return lhsAsNum > rhsAsNum ? 1 : (lhsAsNum == rhsAsNum ? 0 : -1);
		}
		
		return lhs.gameObject.name.CompareTo(rhs.gameObject.name);
	}

	public void CalcBounds()
	{
		for(int i = 0; i < nodes.Count-1; ++i)
		{
			bounds.Encapsulate( nodes[i].bounds);
		}
	}


	public ClosestPointToRetSplineResult CalcClosestPointOnRetSpline(Vector3 position)
	{
		ClosestPointToRetSplineResult retval = new ClosestPointToRetSplineResult();
		CalcClosestPointOnRetSpline(position, retval);
		return retval;
	}

	public ClosestPointToRetSplineResult CalcClosestPointOnRetSpline(Vector3 position, ClosestPointToRetSplineResult retval)
	{
		Vector3 temp = Vector3.zero, tempDif = Vector3.zero;
		float dist = float.MaxValue;

		for(int i = 0; i < GetNumNodes(); i++)
		{
			for(int j = 0; j < GetNode(i).GetNumReticulatedSegments(); j++)
			{
				ReticulatedSplineSegment seg = GetNode(i).GetReticulatedSegment(j);
				temp = AID.UTIL.ClosestPointToLine(seg.startPos, seg.endPos,position, true);
				tempDif = temp - position;
				if(dist > tempDif.sqrMagnitude)
				{
					//retval = temp;
					dist = tempDif.sqrMagnitude;
					retval.closestPoint = temp;
					retval.distSqrFromTarget = dist;
					retval.retPos = new SplineReticulatedPosition();

					retval.retPos.splineNodeIndex = i;// = GetNode(i);
					retval.retPos.retSeg = j;// = GetNode(i).GetReticulatedSegment(j);
					retval.retPos.spline = this;
					retval.retPos.distanceFromRetStart = (temp - GetNode(i).reticulationPoints[j]).magnitude;
				}
			}
		}

		return retval;
	}


	void Update()
	{
		if(showSplineInGame)
			DrawSplineInternal(Vector3.zero, DrawLineWithDebug);
	}


	public static void DrawLineWithGizmo(Vector3 s, Vector3 e)
	{
		Gizmos.DrawLine(s,e);
	}

	public static void DrawLineWithDebug(Vector3 s, Vector3 e)
	{
		Debug.DrawLine(s,e);
	}
}
}