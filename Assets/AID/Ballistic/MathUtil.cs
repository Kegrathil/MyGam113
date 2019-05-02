

using UnityEngine;

namespace AID
{
    static partial class UTIL
    {

        static public readonly float SQRT2f = Mathf.Sqrt(2.0f);
        static public readonly float SQRT3f = Mathf.Sqrt(3.0f);

        public static QuadraticResult QuadraticEquation(float a, float b, float c)
        {
            QuadraticResult retval = new QuadraticResult();

            float b4ac = b * b - 4 * a * c;

            if (b4ac < 0)
            {
                retval.numResults = 0;
            }
            else if (b4ac == 0)
            {
                //Mathf.Sqrt(0) is 0 so ignore it
                retval.numResults = 1;
                retval.r1 = -b / (2 * a);
            }
            else
            {
                float sq = Mathf.Sqrt(b4ac);
                retval.numResults = 2;
                retval.r1 = (-b + sq) / (2 * a);
                retval.r2 = (-b - sq) / (2 * a);
            }

            return retval;
        }

        public static CubicResult CubicEquation(float a, float b, float c, float d)
        {
            //if a is zero???

            CubicResult res = new CubicResult();
            res.SetAll(-1);

            //tried http://easycalculation.com/algebra/learn-cubic-equation.php, ugly due to complex numbers

            //trying http://www.1728.org/cubic2.htm
            float f = ((3 * c / a) - ((b * b) / (a * a))) / 3;
            float g1 = 2 * Mathf.Pow(b, 3) / Mathf.Pow(a, 3);
            float g2 = 9 * b * c / (Mathf.Pow(a, 2));
            float g3 = 27 * d / a;
            float g = (g1 - g2 + g3) / 27;

            float h = (Mathf.Pow(g, 2) / 4) + (Mathf.Pow(f, 3) / 27);

            if (Mathf.Approximately(h, 0) && Mathf.Approximately(g, 0) && Mathf.Approximately(f, 0))
            {
                float da = d / a;
                res.SetAll(-1f * Mathf.Sign(da) * Mathf.Pow(Mathf.Abs(da), 1f / 3f));
            }
            else if (h > 0)
            {
                //1 root
                float hsqrt = Mathf.Sqrt(h);
                float r = (g / -2) + hsqrt;
                float s = Mathf.Pow(r, 1f / 3f);
                float t = (g / -2) - hsqrt;
                //we only want the REAL roots, thx tho
                float u = Mathf.Sign(t) * Mathf.Pow(Mathf.Abs(t), 1f / 3f);

                res.r1.r = (s + u) - (b / (3 * a));
                float realPart = -(s + u) / 2 - (b / (3 * a));
                float iPart = (s - u) * SQRT3f / 2;
                res.r2.r = realPart;
                res.r2.i = iPart;
                res.r3.r = realPart;
                res.r3.i = -iPart;

            }
            else
            {
                //3 roots
                float i = Mathf.Sqrt((Mathf.Pow(g, 2) / 4) - h);
                float j = Mathf.Pow(i, 1.0f / 3.0f);
                float k = Mathf.Acos(-(g / (2 * i)));
                float k3 = k / 3;
                float l = -j;
                float m = Mathf.Cos(k3);
                float n = SQRT3f * Mathf.Sin(k3);
                float p = (b / (-3 * a));

                res.r1.r = 2 * j * m + p;
                res.r2.r = l * (m + n) + p;
                res.r3.r = l * (m - n) + p;
            }


            return res;
        }

        //http://www.1728.org/quartic2.htm
        static public QuarticResult QuarticEquation(float a, float b, float c, float d, float e)
        {
            QuarticResult res = new QuarticResult();
            res.SetAll(-1);

            //divide through by a
            b /= a;
            c /= a;
            d /= a;
            e /= a;
            a = 1;

            float f = c - (3 * b * b / 8);
            float g = d + (b * b * b / 8) - (b * c / 2);
            float h = e - (3 * b * b * b * b / 256) + (b * b * c / 16) - (b * d / 4);

            //determine coeffs for cubic
            float acubic = 1;
            float bcubic = f / 2;
            float ccubic = ((f * f - 4 * h) / 16);
            float dcubic = -1 * g * g / 64;

            CubicResult cubicRes = CubicEquation(acubic, bcubic, ccubic, dcubic);

            int numReals = cubicRes.GetNumReals();
            if (numReals == 3)
            {
                //this method means we get 2 non zero roots, unless all 3 are 0 intersects
                //try to grab first if not then second
                float r1 = Mathf.Approximately(cubicRes.r1.r, 0) ? cubicRes.r2.r : cubicRes.r1.r;
                //try to grab 3rd if not then second
                float r2 = Mathf.Approximately(cubicRes.r3.r, 0) ? cubicRes.r2.r : cubicRes.r3.r;

                float p = Mathf.Sqrt(r1);
                float q = Mathf.Sqrt(r2);
                float r = -g / (8 * p * q);
                float s = b / (4 * a);

                res.r1.r = p + q + r - s;
                res.r2.r = p - q - r - s;
                res.r3.r = -p + q - r - s;
                res.r4.r = -p - q + r - s;
            }
            else
            {
                //this method means we get 2 img results
                //try to grab first if not then second
                ComplexNumber cr1 = cubicRes.r1.IsReal ? cubicRes.r2 : cubicRes.r1;
                //try to grab 3rd if not then second
                ComplexNumber cr2 = cubicRes.r3.IsReal ? cubicRes.r2 : cubicRes.r3;

                ComplexNumber p = cr1.Sqrt();
                ComplexNumber q = cr2.Sqrt();

                // complex mul by real number zeros the complex part
                ComplexNumber pq = p * q;
                float rReal = -g / (8 * pq.r);
                float sReal = b / (4 * a);

                ComplexNumber r = (ComplexNumber)rReal;
                ComplexNumber s = (ComplexNumber)sReal;

                res.r1 = p + q + r - s;
                res.r2 = p - q - r - s;
                res.r3 = -p + q - r - s;
                res.r4 = -p - q + r - s;
            }

            return res;
        }
    }
}
