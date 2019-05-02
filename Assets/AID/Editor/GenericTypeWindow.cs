using UnityEngine;
using System.Collections;
using UnityEditor;

namespace AID
{

    public abstract class GenericTypeWindow<T> : EditorWindow where T : UnityEngine.Object
    {
        public static GenericTypeWindow<T> instance;


        public T data;
        public Editor ed;
        public Vector2 scrollPos;


        public static void InternalInit(System.Type ct)
        {
            if (instance == null)
            {
                // Get existing open window or if none, make a new one:
                var window = EditorWindow.GetWindow(ct);
                instance = (GenericTypeWindow<T>)window;


                //window = window;
            }


            instance.Show();
            instance.Focus();
        }

        void OnGUI()
        {
            var res = AssetDatabase.FindAssets("t:" + typeof(T).Name);

            T newData = null;

            //if there is only 1 then just load it
            if (res.Length == 1)
            {
                newData = AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(res[0]));
            }
            else
            {
                newData = (T)EditorGUILayout.ObjectField((string)"Target", data, typeof(T), true, null);

                GUILayoutHelper.DrawSpacerLine(Color.black);
            }


            if (newData != data)
            {
                data = newData;

                if (newData != null)
                    ed = Editor.CreateEditor(data);
                else
                    ed = null;

                scrollPos = Vector2.zero;
            }

            if (data != null && ed == null)
            {
                ed = Editor.CreateEditor(data);
                scrollPos = Vector2.zero;
            }

            if (ed != null)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
                ed.OnInspectorGUI();
                EditorGUILayout.EndScrollView();
            }
        }
    }
}