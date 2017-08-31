using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace eidos.ml.test
{
    public class FloatingPointAssertions
    {
        protected const double DefaultPrecision = 1e-5;
        
        public void Equal(double a, double b, double precision = DefaultPrecision)
        {
            Assert.True(Math.Abs(a-b) < precision);
        }

        public void Greater(double a, double b, double precision = DefaultPrecision)
        {
            Assert.True(a - b > precision);
        }

        public void GreaterOrEqual(double a, double b, double precision = DefaultPrecision)
        {
            Assert.True(a - b > -precision);
        }

        public void Less(double a, double b, double precision = DefaultPrecision)
        {
            Assert.True(b - a > precision);
        }

        public void LessOrEqual(double a, double b, double precision = DefaultPrecision)
        {
            Assert.True(b - a > -precision);
        }
    }
}
