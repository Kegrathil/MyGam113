using UnityEngine;
using System.Collections;

namespace AID
{
    public abstract class ABResponse : MonoBehaviour
    {

        public float delay = 0; //time until we fire
        public float hold = 0;  //time until we let the next reponse in the chain take over after we fire

        public abstract void Fire(SensorResponseRouter r);

        void Start()
        {

        }
    }

}