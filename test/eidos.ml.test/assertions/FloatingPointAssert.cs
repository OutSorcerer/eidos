using System;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;
using Xunit;

namespace eidos.ml.test.assertions
{
    public class FloatingPointAssert
    {
        protected const double DefaultPrecision = 1e-5;

        public static void Equal(double a, double b, double precision = DefaultPrecision)
        {
            var absoluteDifference = Math.Abs(a - b);
            Assert.True(absoluteDifference < precision,
                $"Expected approximately equal values with precision {precision} but found {a} and {b} which difference is {absoluteDifference}.");
        }

        public static void Greater(double a, double b, double precision = DefaultPrecision)
        {
            Assert.True(a - b > precision);
        }

        public static void GreaterOrEqual(double a, double b, double precision = DefaultPrecision)
        {
            Assert.True(a - b > -precision);
        }

        public static void Less(double a, double b, double precision = DefaultPrecision)
        {
            Assert.True(b - a > precision);
        }

        public static void LessOrEqual(double a, double b, double precision = DefaultPrecision)
        {
            Assert.True(b - a > -precision);
        }

        public static void Equal(Vector<double> a, Vector<double> b, double precision = DefaultPrecision,
            Func<Vector<double>, Vector<double>, double> distanceDelegate = null)
        {
            if (distanceDelegate == null)
                distanceDelegate = Distance.MSE;
            var d = distanceDelegate(a, b);
            Assert.True(d < precision,
                $"Expected approximately equal vectors with precision {precision} and {distanceDelegate.Method.Name} distanceDelegate but found vectors with actual distanceDelegate of {d}.");
        }
    }
}