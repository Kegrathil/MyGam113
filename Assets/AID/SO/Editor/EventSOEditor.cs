using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace AID
{
    /// <summary>
    /// Adds a listing of the current number of listeners and allows Firing an event while the game is running manually.
    /// </summary>
    [CustomEditor(typeof(EventSO))]
    public class EventSOEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EventSO eventso = target as EventSO;
            EditorGUILayout.Space();
            EditorGUILayout.PrefixLabel("Current Listeners: " + eventso.ListenerCount.ToString());
            if(Application.isPlaying && GUILayout.Button("Fire"))
            {
                eventso.Fire();
            }

            if(GUILayout.Button(new GUIContent("Attempt to Ping all Listeners","Cannot ping Objects that are not monobehaviours")))
            {
                EditorUTIL.PingElements(eventso.Listeners);
            }
        }
    }

}