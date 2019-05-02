using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace AID
{
    public class GOActivateResponse : ABResponse
    {
        public List<GameObject> enableThese = new List<GameObject>();
        public List<GameObject> disableThese = new List<GameObject>();
        public List<GameObject> toggleThese = new List<GameObject>();

        public override void Fire(SensorResponseRouter r)
        {
            gameObject.ActivateAll(enableThese);
            gameObject.DeactivateAll(disableThese);
            gameObject.ToggleActive(toggleThese);
        }
    }
}