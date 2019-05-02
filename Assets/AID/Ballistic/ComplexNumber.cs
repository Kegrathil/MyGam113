using System;
using UnityEngine;

namespace AID
{
    //these only appeared in .net in 4.0 and unity doesn't support that fully yet, so, here's a partial complex number class to limp along until it does
    public struct ComplexNumber
    {
        public float r, i;

        public bool IsReal { get { return Mathf.Approximately(i, 0); } }

        public ComplexNumber(float real, float imaginary)
        {
            r = real;
            i = imaginary;
        }

        public static explicit operator ComplexNumber(float real)
        {
            return new ComplexNumber(real, 0);
        }

        public ComplexNumber Sqrt()
        {
            float mod = Mathf.Sqrt(r * r + i * i);
            float y = Mathf.Sqrt((mod - r) / 2);
            float x = i / (2 * y);

            return new ComplexNumber(Mathf.Abs(x), Mathf.Sign(i) * y);
        }

        public static ComplexNumber operator +(ComplexNumber lhs, ComplexNumber rhs)
        {
            return new ComplexNumber(lhs.r + rhs.r, lhs.i + rhs.i);
        }

        public static ComplexNumber operator -(ComplexNumber lhs, ComplexNumber rhs)
        {
            return new ComplexNumber(lhs.r - rhs.r, lhs.i - rhs.i);
        }

        public static ComplexNumber operator -(ComplexNumber unary)
        {
            return new ComplexNumber(-unary.r, -unary.i);
        }

        public static ComplexNumber operator *(ComplexNumber lhs, ComplexNumber rhs)
        {
            return new ComplexNumber(lhs.r * rhs.r - lhs.i * rhs.i, lhs.i * rhs.r + lhs.r * rhs.i);
        }

        public override string ToString()
        {
            return string.Format("[ComplexNumber: r={0}, i={1}]", r, i);
        }

    }
}