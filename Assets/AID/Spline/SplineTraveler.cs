using UnityEngine;
using System.Collections;
namespace AID
{
    /*
        Allows for the ability to traverse a spline in a number of ways.

        If mode is set to stop, this component is disabled after it reaches the end node.

        Sends following messages
            OnSplineTravelerReachedEnd	- sent when traveler reaches the end, passes SplineTraversalLoopMode
    */

    //TODO
    //	distance along reticulated
    //	percentage
    //		isn't returning pos after calcs, this should be calling a function in node


    public class SplineTraveler : MonoBehaviour
    {
        public Spline spline;
        public int curSplineSegment;    //which nodes we are currently traversing
        public float curPercentage; //percentage traveled along the segment, in non-reticulated mode
        public int curRetSegment;       //which reticulated segment are we currently in
        public float curRetProgression; // how car have we traveled in that reticulated segment
        public bool directionOfTravel = true;
        public SplineTraversalLoopMode loopMode = SplineTraversalLoopMode.Stop;
        public SplineMoveInterpretation moveMode = SplineMoveInterpretation.Distance;
        public Vector3 prevReturnedPosition;
        public Vector3 curReturnedPosition;

        private Vector3 _getReticulatedPosition()
        {
            //return new pos
            ReticulatedSplineSegment seg = spline.GetNode(curSplineSegment).GetReticulatedSegment(curRetSegment);
            return seg.startPos + seg.dir * curRetProgression;
        }

        public SplineReticulatedPosition GetReticulatedPosition()
        {
            SplineReticulatedPosition retval = new SplineReticulatedPosition();
            retval.spline = spline;
            retval.distanceFromRetStart = curRetProgression;
            retval.splineNodeIndex = curSplineSegment;
            retval.retSeg = curRetSegment;
            return retval;
        }

        public void SetReticulatedPosition(SplineReticulatedPosition pos)
        {
            spline = pos.spline;
            curRetProgression = pos.distanceFromRetStart;
            curSplineSegment = pos.splineNodeIndex;
            curRetSegment = pos.retSeg;
        }

        //input is either percentage in segment or distance along reticulated, distance given should be POST any delta time modifications
        public Vector3 Move(float dist)
        {
            if (!enabled)
            {
                print("called move while disabled");
                return Vector3.zero;
            }

            prevReturnedPosition = curReturnedPosition;
            switch (moveMode)
            {
                case SplineMoveInterpretation.Percentage:
                    curReturnedPosition = HandlePercentageMove(dist);

                    break;
                case SplineMoveInterpretation.Distance:
                    curReturnedPosition = HandleDistanceMove(dist);

                    break;
            }

            return curReturnedPosition;
        }

        public Vector3 GetCurrentHeading()
        {
            Vector3 dir = Vector3.zero;

            if (spline != null)
            {
                switch (moveMode)
                {
                    case SplineMoveInterpretation.Percentage:
                        dir = spline.GetPositionInSection(curSplineSegment, curPercentage + 0.0001f) - spline.GetPositionInSection(curSplineSegment, curPercentage);
                        break;
                    case SplineMoveInterpretation.Distance:
                        dir = spline.GetNode(curSplineSegment).GetReticulatedSegment(curRetSegment).dir;    //TODO this might return null tho
                        break;
                }
            }

            return dir;
        }

        public Vector3 HandlePercentageMove(float dist)
        {
            //temp flip direction on negative dist
            if (dist < 0)
            {
                directionOfTravel = !directionOfTravel;
                dist = Mathf.Abs(dist);
            }

            float newPer = curPercentage + dist * (directionOfTravel ? 1.0f : -1.0f);
            int segChange = 0;

            if (newPer >= 1.0f || newPer < 0.0f)
            {
                if (newPer < 0f)
                {
                    newPer -= 1.0f;
                }

                segChange = (int)Mathf.Abs(newPer);
            }

            curPercentage = Mathf.Repeat(newPer, 1.0f);

            //move to new segment
            while (segChange > 0)
            {
                if (directionOfTravel)
                {
                    ++curSplineSegment;
                }
                else
                {
                    --curSplineSegment;
                }

                ProcessSplineNodeChange();
                --segChange;
            }

            return spline.GetPositionInSection(curSplineSegment, curPercentage);
        }

        public Vector3 HandleDistanceMove(float dist)
        {
            if (spline == null)
            {
                return Vector3.zero;
            }

            //temp flip direction on negative dist
            if (dist < 0)
            {
                directionOfTravel = !directionOfTravel;
                Vector3 retval = _handleDistanceMoveForward(Mathf.Abs(dist));
                directionOfTravel = !directionOfTravel;
                return retval;
            }
            else
                return _handleDistanceMoveForward(dist);
        }

        public Vector3 _handleDistanceMoveForward(float dist)
        {
            float distLeft = dist;

            //HACK to prevent inf loops during development
            for (int i = 0; i < 10 && distLeft > 0f; i++)
            //while(distLeft > 0f)
            {
                //handle spline being capped and at end already
                if (loopMode == SplineTraversalLoopMode.Stop)
                {
                    if ((directionOfTravel && curSplineSegment >= spline.GetNumSections() - 1 &&
                         curRetSegment >= spline.GetNode(curSplineSegment).reticulatedSegments.Length - 1 &&
                         curRetProgression >= spline.GetNode(curSplineSegment).reticulatedSegments[spline.GetNode(curSplineSegment).reticulatedSegments.Length - 1].length)
                       ||
                       (!directionOfTravel && curSplineSegment <= 0 && curRetSegment <= 0 && curRetProgression <= 0))
                    {
                        return _getReticulatedPosition();
                    }
                }

                ReticulatedSplineSegment seg = spline.GetNode(curSplineSegment).GetReticulatedSegment(curRetSegment);

                float distLeftInSeg = 0;
                if (seg != null)
                    distLeftInSeg = directionOfTravel ? seg.length - curRetProgression : seg.length - (seg.length - curRetProgression);

                //do we have more left than this segment has
                if (distLeft > distLeftInSeg)
                {
                    distLeft -= distLeftInSeg;

                    //advance
                    if (directionOfTravel)
                    {
                        curRetProgression = 0;
                        ++curRetSegment;
                    }
                    else
                    {
                        --curRetSegment;
                        //be safe
                        if (curRetSegment < spline.GetNode(curSplineSegment).GetNumReticulatedSegments() && curRetSegment >= 0)
                        {
                            curRetProgression = spline.GetNode(curSplineSegment).GetReticulatedSegment(curRetSegment).length;
                        }
                    }

                    //we move the segment
                    bool validRetSeg = curRetSegment < spline.GetNode(curSplineSegment).GetNumReticulatedSegments() && curRetSegment >= 0;

                    if (validRetSeg == false)
                    {
                        //end of spline segment move to next/handle end of spline
                        curSplineSegment += directionOfTravel ? 1 : -1;

                        ProcessSplineNodeChange();
                    }
                }
                else
                {
                    //more space than we need
                    curRetProgression = directionOfTravel ? curRetProgression + distLeft : curRetProgression - distLeft;
                    distLeft = 0;
                    break;  //force break out
                }
            }

            return _getReticulatedPosition();
        }

        private void ProcessSplineNodeChange()
        {
            //is that valid
            if (curSplineSegment > spline.GetNumSections() - 1 || curSplineSegment < 0)
            {
                //ok how to proceed
                switch (loopMode)
                {
                    case SplineTraversalLoopMode.Stop:
                        //enabled = false;
                        if (directionOfTravel)
                        {
                            curSplineSegment = spline.GetNumSections() - 1;
                            curPercentage = 1;
                            curRetSegment = spline.GetNode(curSplineSegment).GetNumReticulatedSegments() - 1;
                            curRetProgression = spline.GetNode(curSplineSegment).GetReticulatedSegment(curRetSegment).length;

                        }
                        else
                        {
                            curSplineSegment = 0;
                            curPercentage = 0;
                            curRetSegment = 0;
                            curRetProgression = 0;
                        }

                        SendMessage("OnSplineTravelerReachedEnd", loopMode, SendMessageOptions.DontRequireReceiver);
                        break;

                    case SplineTraversalLoopMode.Loop:
                        if (directionOfTravel)
                        {
                            curSplineSegment = 0;
                            curPercentage = 0;
                            curRetSegment = 0;
                            curRetProgression = 0;
                        }
                        else
                        {
                            curSplineSegment = spline.GetNumSections() - 1;
                            curPercentage = 1;
                            curRetSegment = spline.GetNode(curSplineSegment).GetNumReticulatedSegments() - 1;
                            curRetProgression = spline.GetNode(curSplineSegment).GetReticulatedSegment(curRetSegment).length;
                        }

                        curSplineSegment = (int)Mathf.Repeat(curSplineSegment, spline.GetNumNodes());
                        SendMessage("OnSplineTravelerReachedEnd", loopMode, SendMessageOptions.DontRequireReceiver);
                        break;

                    case SplineTraversalLoopMode.PingPong:
                        directionOfTravel = !directionOfTravel;
                        curPercentage = 1.0f - curPercentage;
                        curSplineSegment = curSplineSegment < 0 ? 0 : (spline.GetNumSections() - 1);
                        SendMessage("OnSplineTravelerReachedEnd", loopMode, SendMessageOptions.DontRequireReceiver);
                        break;
                }

            }
            else
            {
                if (directionOfTravel)
                {
                    curRetSegment = 0;
                    curRetProgression = 0;
                }
                else
                {
                    curRetSegment = spline.GetNode(curSplineSegment).GetNumReticulatedSegments() - 1;
                    curRetProgression = spline.GetNode(curSplineSegment).GetReticulatedSegment(curRetSegment).length;
                }
            }
        }
    }

    public enum SplineTraversalLoopMode
    {
        Stop,           //stop movement at last node
        Loop,           //wrap index at last node
        PingPong        //reverse direction at last node
    };

    public enum SplineMoveInterpretation
    {
        Distance,
        Percentage
    };
}