using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//TODO this and collision sensor are so similar make this a macro with 2 stringification vars
namespace AID
{
    public class TriggerSensor : ABSensor
    {

        public List<string> tagsToFireOn = new List<string>();

        public float onStayRate;

        public bool fireOnEnter = true, fireOnExit = false;

        void OnTriggerEnter(Collider col)
        {
            if (fireOnEnter && gameObject.CompareTagCollection(col.gameObject, tagsToFireOn))
            {
                DetectionOccured();
            }
        }

        void OnTriggerStay(Collider col)
        {
            if (gameObject.CompareTagCollection(col.gameObject, tagsToFireOn))
            {
                DetectionOccuredLimited(onStayRate);
            }
        }

        void OnTriggerExit(Collider col)
        {
            if (fireOnExit && gameObject.CompareTagCollection(col.gameObject, tagsToFireOn))
            {
                DetectionOccured();
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (fireOnEnter && gameObject.CompareTagCollection(col.gameObject, tagsToFireOn))
            {
                DetectionOccured();
            }
        }

        void OnTriggerStay2D(Collider2D col)
        {
            if (gameObject.CompareTagCollection(col.gameObject, tagsToFireOn))
            {
                DetectionOccuredLimited(onStayRate);
            }
        }

        void OnTriggerExit2D(Collider2D col)
        {
            if (fireOnExit && gameObject.CompareTagCollection(col.gameObject, tagsToFireOn))
            {
                DetectionOccured();
            }
        }
    };

}