using UnityEngine;
using System.Collections;

namespace AID
{
    /*
        Helper for controlling random rotation on the y axis of an object on start
    */
    public class RotatePrefabController : MonoBehaviour
    {

        [Tooltip("Y axis rotation set when object is spawned")]
        [Range(0, 360)]
        public float startingYRot = 0;
        [Tooltip("Y axis random angle added to intial to create slight jitter")]
        [Range(0, 10)]
        public float startingYRandomAddition = 0;
        [Tooltip("Number of cuts made in the circle, 1 means it is not cut at all, 2 means it is at startingYRot and startingYRot+180, 3 means startYRot, or startingYRot + 120 or startYRot + 240, etc.")]
        [Range(1, 36)]
        public int numSteps = 1;

        public void Rotate(float randomPercentStarting, float randomPercentSteps)
        {
            var cur = transform.localEulerAngles;
            var startingOff = -startingYRandomAddition + startingYRandomAddition*2*randomPercentStarting;
            int desiredSteps = Mathf.RoundToInt( numSteps * randomPercentSteps );

            cur.y = startingYRot + startingOff + (360 / numSteps) * desiredSteps;

            transform.localEulerAngles = cur;

        }

        void Start()
        {
            //Rotate(Random.value, Random.value);
        }
    }
}