using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AID
{
    /*
        Adds options to the inspector of a Spline instance
    */
    [CustomEditor(typeof(Spline))]
    [CanEditMultipleObjects]
    public class SplineEditor : Editor
    {
        int numToSpawn = 10;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();


            Spline mySpline = target as Spline;

            EditorGUILayout.BeginHorizontal();
            numToSpawn = EditorGUILayout.IntField("", numToSpawn);

            if (GUILayout.Button("Add Nodes"))
            {
                for (int i = 0; i < numToSpawn; ++i)
                    Selection.activeGameObject = mySpline.CreateNewChildNode();
            }
            if (GUILayout.Button("Nodes from Child GOs"))
            {
                mySpline.AttemptToGetNodesFromChildren();
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Reticulate"))
            {
                mySpline.Reticulate();

                //forces it to be flagged as dirty
                EditorUtility.SetDirty(this);
                SceneView.RepaintAll();
            }
            if (GUILayout.Button("Re-Calculate Tangets"))
            {
                mySpline.RecalcTangents();

                //forces it to be flagged as dirty
                EditorUtility.SetDirty(this);
            }
            EditorGUILayout.EndHorizontal();


            //		EditorGUILayout.BeginVertical ();
            //		EditorGUILayout.LabelField("--Metrics");
            //		EditorGUILayout.LabelField("Spline Length: " + Mathf.Round(mySpline.GetSplineLength()).ToString());
            //		
            //		EditorGUILayout.EndVertical ();
        }
    }
}