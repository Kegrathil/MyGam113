using UnityEngine;
using System.Collections;

/*
	Automatic use of a spline traveler that supports offsets.
*/
namespace AID
{

    public class FollowSpline : ABAlongSpline
    {

        public float distPerSecond = 1;

        public override void Update()
        {
            if (trav.enabled)
            {
                transform.position = trav.Move(distPerSecond * Time.deltaTime) + offsetPosition;
            }

            base.Update();
        }

    }
}