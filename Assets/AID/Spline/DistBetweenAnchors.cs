using UnityEngine;
using System.Collections;

namespace AID
{
    public class DistBetweenAnchors : MonoBehaviour
    {

        public SplineAnchor a, b;
        public float dist;

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

            dist = (float)a.retPRes.retPos.DistanceBetween(b.retPRes.retPos);

        }
    }
}