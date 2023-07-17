using System.Numerics;

namespace system
{
    public class math
    {

        public static readonly double pi = System.Math.PI;
        public static readonly double e = System.Math.E;
        public static readonly double nan = double.NaN;
        public static readonly double inf = double.PositiveInfinity;
        public static readonly double ninf = double.NegativeInfinity;
        public static readonly double epsilon = double.Epsilon;
        public static readonly Complex cnan = Complex.NaN;
        public static readonly Complex cinf = Complex.Infinity;
        public static readonly Complex czero = Complex.Zero;
        public static readonly Complex cone = Complex.One;

        #region Doubles
        public static double acos(double x)
        {
            return System.Math.Acos(x);
        }
        public static double acosh(double x)
        {
            return System.Math.Acosh(x);
        }
        public static double asin(double x)
        {
            return System.Math.Asin(x);
        }
        public static double asinh(double x)
        {
            return System.Math.Asinh(x);
        }
        public static double atan(double x)
        {
            return System.Math.Atan(x);
        }
        public static double atan2(double x, double y)
        {
            return System.Math.Atan2(x, y);
        }
        public static double atanh(double x)
        {
            return System.Math.Atanh(x);
        }
        public static double bitDec(double x)
        {
            return System.Math.BitDecrement(x);
        }
        public static double bitInc(double x)
        {
            return System.Math.BitIncrement(x);
        }
        public static double cbrt(double x)
        {
            return System.Math.Cbrt(x);
        }
        public static double ceil(double x)
        {
            return System.Math.Ceiling(x);
        }
        public static double copySign(double x, double y)
        {
            return System.Math.CopySign(x, y);
        }
        public static double cos(double x)
        {
            return System.Math.Cos(x);
        }
        public static double cosh(double x)
        {
            return System.Math.Cosh(x);
        }
        public static double exp(double x)
        {
            return System.Math.Exp(x);
        }
        public static double floor(double x)
        {
            return System.Math.Floor(x);
        }
        public static double IEEERemainder(double x, double y)
        {
            return System.Math.IEEERemainder(x, y);
        }
        public static double log(double x, double y = double.NaN)
        {
            if (double.IsNaN(y))
                return System.Math.Log(x);
            return System.Math.Log(x, y);
        }
        public static double log10(double x)
        {
            return System.Math.Log10(x);
        }
        public static double log2(double x)
        {
            return System.Math.Log2(x);
        }
        public static double max(double x, double y)
        {
            return System.Math.Max(x, y);
        }
        public static double min(double x, double y)
        {
            return System.Math.Min(x, y);
        }
        public static double pow(double x, double y)
        {
            return System.Math.Pow(x, y);
        }
        public static double sqrt(double x)
        {
            return System.Math.Sqrt(x);
        }
        public static double trunc(double x)
        {
            return System.Math.Truncate(x);
        }

        #endregion

        #region Complex
        public static Complex cacos(Complex value) {
            return Complex.Acos(value);
        }
        public static Complex casin(Complex value)
        {
            return Complex.Asin(value);
        }
        public static Complex catan(Complex value)
        {
            return Complex.Atan(value);
        }
        public static Complex cconj(Complex value)
        {
            return Complex.Conjugate(value);
        }
        public static Complex ccos(Complex value)
        {
            return Complex.Cos(value);
        }
        public static Complex ccosh(Complex value)
        {
            return Complex.Cosh(value);
        }
        public static Complex cexp(Complex value)
        {
            return Complex.Exp(value);
        }
        public static Complex cFromPolar(double magnitude, double phase)
        {
            return Complex.FromPolarCoordinates(magnitude, phase);
        }
        public static bool cIsFinite(Complex value)
        {
            return Complex.IsFinite(value);
        }
        public static bool cIsInf(Complex value)
        {
            return Complex.IsInfinity(value);
        }
        public static bool cIsNan(Complex value)
        {
            return Complex.IsNaN(value);
        }
        public static Complex clog(Complex value,double baseVal = double.NaN)
        {
            if (baseVal != double.NaN)
                return Complex.Log(value, baseVal);
            return Complex.Log(value);
        }
        public static Complex clog10(Complex value)
        {
            return Complex.Log10(value);
        }
        public static Complex cnegate(Complex value)
        {
            return Complex.Negate(value);
        }
        public static Complex cpower(Complex value, object power)
        {
            if (power.GetType().Name == "Complex")
                return Complex.Pow(value, (Complex)power);
             return Complex.Pow(value, (double)power);
        }
        public static Complex creci(Complex value)
        {
            return Complex.Reciprocal(value);
        }
        public static Complex csin(Complex value)
        {
            return Complex.Sin(value);
        }
        public static Complex csinh(Complex value)
        {
            return Complex.Sinh(value);
        }
        public static Complex csqrt(Complex value)
        {
            return Complex.Sqrt(value);
        }
        public static Complex ctan(Complex value)
        {
            return Complex.Tan(value);
        }
        public static Complex ctanh(Complex value)
        {
            return Complex.Tanh(value);
        }
        #endregion

        #region BigInts
        public static BigInteger bpow(BigInteger value, int exp)
        {
            return BigInteger.Pow(value, exp);
        }
        public static double blog(BigInteger value, double baseVal = double.NaN)
        {
            if (double.IsNaN(baseVal))
                return BigInteger.Log(value);
            return BigInteger.Log(value, baseVal);
        }
        public static double blog10(BigInteger value)
        {
            return BigInteger.Log10(value);
        }
        #endregion
    }
}
