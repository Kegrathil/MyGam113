using UnityEngine;
using UnityEditor;
using System.Collections;

namespace AID
{
    [CustomPropertyDrawer(typeof(RampNormalised))]
    public class RampNormalisedDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.PropertyField(position, property, label, true);

            //really don't like the way unity.object has to be ref values

            //get anim curve and clamp
            var curveSP = property.FindPropertyRelative("curve");
            var an = curveSP.animationCurveValue;
            
            NormalisedCurveAttribute.ClampAnimCurve(an);

            curveSP.animationCurveValue = an;

            //keep the min and max in line

            var minSP = property.FindPropertyRelative("remappedMin");
            var maxSP = property.FindPropertyRelative("remappedMax");

            maxSP.floatValue = Mathf.Max(maxSP.floatValue, minSP.floatValue);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property);
        }
    }
}