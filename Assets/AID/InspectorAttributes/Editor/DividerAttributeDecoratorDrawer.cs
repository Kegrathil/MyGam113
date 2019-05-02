using UnityEditor;
using UnityEngine;
using System.Collections;

namespace AID
{
    /*
        Decorator Drawer for creating line breaks with text in them in the unity inspector eg

        ---------------------------<some text>----------------------------
    */

    [CustomPropertyDrawer(typeof(DividerAttribute))]
    public class DividerAttributeDrawer : DecoratorDrawer
    {
        DividerAttribute divider { get { return ((DividerAttribute)attribute); } }

        public override float GetHeight()
        {
            return divider.GetTotalHeight();
        }


        // Override the GUI drawing for this attribute
        public override void OnGUI(Rect position)
        {
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            position.y += divider.leadingSpace;

            var left = divider.TransformRectLeft(position);

            GUI.Box(left, GUIContent.none, GUILayoutHelper.GetTexColGUIStyle(divider.divLineCol));

            if (divider.HasMsg)
            {
                var msgPos = divider.TransformRectMsg(position);
                GUI.Label(msgPos, divider.msg);

                var right = divider.TransformRectRight(position);
                GUI.Box(right, GUIContent.none, GUILayoutHelper.GetTexColGUIStyle(divider.divLineCol));
            }

            EditorGUI.indentLevel = indent;
        }
    }
}