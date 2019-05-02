using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace AID
{
    public static class GUILayoutHelper
    {

        //adapted from http://answers.unity3d.com/questions/477306/any-simple-examples-for-creating-a-tabbed-menu.html
        public static int Tabs(string[] options, int selected)
        {
            const float DarkGray = 0.4f;
            const float LightGray = 0.9f;
            const float StartSpace = 10;

            GUILayout.Space(StartSpace);
            Color storeColor = GUI.backgroundColor;
            Color highlightCol = new Color(LightGray, LightGray, LightGray);
            Color bgCol = new Color(DarkGray, DarkGray, DarkGray);

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.padding.bottom = 8;

            GUILayout.BeginHorizontal();
            {   //Create a row of buttons
                for (int i = 0; i < options.Length; ++i)
                {
                    GUI.backgroundColor = i == selected ? highlightCol : bgCol;
                    if (GUILayout.Button(options[i], buttonStyle))
                    {
                        selected = i; //Tab click
                    }
                }
            }
            GUILayout.EndHorizontal();
            //Restore color
            GUI.backgroundColor = storeColor;

            return selected;
        }

        private static Texture2D colTex2d;
        private static GUIStyle colTexGuiStyle;

        public static GUIStyle GetTexColGUIStyle(Color col)
        {
            if (colTex2d == null || colTexGuiStyle == null)
            {
                colTex2d = new Texture2D(1, 1, TextureFormat.RGB24, false);

                colTexGuiStyle = new GUIStyle();
                colTexGuiStyle.normal.background = colTex2d;
            }

            colTex2d.SetPixel(0, 0, col);
            colTex2d.Apply();

            return colTexGuiStyle;
        }

        public static void DrawSpacerLine(Color col, int leadingSpace = 10, int lineHeight = 2, int trailingSpace = 10)
        {
            GUILayout.Space(leadingSpace);

            var height = lineHeight;
            var width = Screen.width - GUI.skin.toggle.padding.left * 2;

            GUILayout.Box("", GetTexColGUIStyle(col), GUILayout.Height(height), GUILayout.MinHeight(height), GUILayout.MaxHeight(height), GUILayout.Width(width), GUILayout.MinWidth(width), GUILayout.MaxWidth(width));

            GUILayout.Space(trailingSpace);
        }

        public static void BoolList(ref bool[] b, int spacerMod)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < b.Length; i++)
            {
                int width = 15;
                if (i % spacerMod == spacerMod - 1)
                {
                    width = 25;
                }

                b[i] = GUILayout.Toggle(b[i], "", GUILayout.MaxWidth(width), GUILayout.MinWidth(width));
            }
            GUILayout.EndHorizontal();
        }

        public static void BoolList(ref List<bool> b, int spacerMod)
        {
            GUILayout.BeginHorizontal();
            for (int i = 0; i < b.Count; i++)
            {
                int width = 15;
                if (i % spacerMod == spacerMod - 1)
                {
                    width = 25;
                }

                b[i] = GUILayout.Toggle(b[i], "", GUILayout.MaxWidth(width), GUILayout.MinWidth(width));
            }
            GUILayout.EndHorizontal();
        }
    }
}