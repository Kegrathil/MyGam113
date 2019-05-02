using UnityEngine;
using System.Collections;
using System;

namespace AID
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class DividerAttribute : PropertyAttribute
    {
        public int leadingSpace = 10;
        public int divLineHeight = 2;
        public int trailingSpace = 10;
        public int padLeft = 15;
        public int padRight = 5;
        public Color divLineCol = new Color(0.5f, 0.5f, 0.5f);
        public string msg = string.Empty;

        public float GetTotalHeight()
        {
            return divLineHeight + leadingSpace + trailingSpace;
        }

        public DividerAttribute() { }
        public DividerAttribute(string a_msg) { msg = a_msg; }

        public DividerAttribute(string a_msg, float red, float green, float blue, int a_leadingSpace, int a_divLineHeight, int a_trailingSpace, int a_padLeft, int a_padRight)
        {
            msg = a_msg;
            leadingSpace = a_leadingSpace;
            divLineHeight = a_divLineHeight;
            trailingSpace = a_trailingSpace;
            padLeft = a_padLeft;
            padRight = a_padRight;
            divLineCol = new Color(red, green, blue);
        }

        public float MsgWidth()
        {
            return GUI.skin.label.CalcSize(new GUIContent(msg)).x + GUI.skin.label.CalcSize(new GUIContent(" ")).x * 2;
        }

        public bool HasMsg
        {
            get { return msg.Length != 0; }
        }

        public Rect TransformRectLeft(Rect position)
        {
            var ret = new Rect(position);

            //ret the line correctl
            //ret.y += leadingSpace;
            ret.x = padLeft;
            if (HasMsg)
            {
                ret.width = Screen.width - padLeft - padRight - MsgWidth();
                ret.width /= 2;
            }
            else
            {
                ret.width = Screen.width - padLeft - padRight;
            }
            ret.height = divLineHeight;

            return ret;
        }


        public Rect TransformRectMsg(Rect position)
        {
            var ret = new Rect(position);
            //position the line correctly
            ret.x = padLeft;
            if (HasMsg)
            {
                var msgw = MsgWidth();
                ret.width = Screen.width - padLeft - padRight - msgw - padRight;
                ret.width /= 2;

                ret.x += ret.width + GUI.skin.label.CalcSize(new GUIContent(" ")).x;
                ret.width = msgw;
            }

            ret.height = GUI.skin.label.lineHeight + 2;
            ret.y -= ret.height / 2;

            return ret;
        }

        public Rect TransformRectRight(Rect position)
        {
            var ret = new Rect(position);
            //ret the line correctly
            ret.x = padLeft;
            if (HasMsg)
            {
                var msgw = MsgWidth();
                ret.width = Screen.width - padLeft - padRight - msgw;
                ret.width /= 2;

                ret.x += ret.width + msgw;
            }

            ret.height = divLineHeight;

            return ret;
        }
    }
}