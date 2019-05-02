using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Linq;

namespace AID
{
    /*
        Attribute Drawer for strings in unity inspector to force them to be of hex characters only
    */

    [CustomPropertyDrawer(typeof(HexStringAttribute))]
    public class HexStringAttributeDrawer : PropertyDrawer
    {
        readonly static string validChars = "0123456789abcdef";

        // Draw the property inside the given rect
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            string prevVal = property.stringValue;

            if (!IsValidHex(prevVal))
                prevVal = "";

            //draw it
            var content = new GUIContent(label.text, "Can only contain " + validChars);
            EditorGUI.PropertyField(position, property, content);

            string newVal = property.stringValue;



            if (!IsValidHex(newVal))
                property.stringValue = prevVal;

            EditorGUI.EndProperty();
        }

        bool IsValidHex(string s)
        {
            bool isValid = true;
            for (int i = 0; i < s.Length; i++)
            {
                if (validChars.IndexOf(s[i]) == -1)
                {
                    isValid = false;
                    break;
                }
            }

            return isValid;
        }
    }
}