using UnityEngine;
using UnityEditor;
using System.Collections;

namespace AID
{
    [CustomEditor(typeof(SensorResponseRouter))]
    [CanEditMultipleObjects]
    public class SensorResponseRouterInspector : Editor
    {
        public override void OnInspectorGUI()
        {
            SensorResponseRouter my = (SensorResponseRouter)target;

            if (my.anyOfTheseChains.Count == 0)
            {
                SensorChain sc = my.GetComponent<SensorChain>();

                if (sc != null)
                    my.anyOfTheseChains.Add(sc);
            }

            if (my.responses.Count == 0)
            {
                ResponseChain r = my.GetComponent<ResponseChain>();

                if (r != null)
                    my.responses.Add(r);
            }

            //EditorGUIUtility.LookLikeControls();
            DrawDefaultInspector();
        }
    }
}