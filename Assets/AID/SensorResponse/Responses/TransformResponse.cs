using UnityEngine;
using System.Collections;


namespace AID
{
    public class TransformResponse : ABResponse
    {

        public Transform target;
        public Transform dataSource;

        public bool local, rotation = true, position = true, scale;


        public override void Fire(SensorResponseRouter r)
        {
            if (!target || !dataSource) return;

            if (local)
            {
                if (position) target.localPosition = dataSource.localPosition;
                if (rotation) target.localRotation = dataSource.localRotation;
            }
            else
            {
                if (position) target.position = dataSource.position;
                if (rotation) target.rotation = dataSource.rotation;
            }

            //scale is special in unity :S

            if (scale) target.localScale = dataSource.localScale;
        }
    }
}