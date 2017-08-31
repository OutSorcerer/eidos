using System;
using eidos.ml.linear;
using eidos.ml.metrics;
using Xunit;
using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.test
{
    public class RidgeRegressionTest : EidosTestBase
    {
        [Fact]
        public void FitPredict_WithRandomData_ReturnsCorrectResult()
        {
            var (x, y, theta) = Generate();
            var ridgeRegressionEstimator = new RidgeRegression().Fit(x, y);
            var y_pred = ridgeRegressionEstimator.Predict(x);
            var meanSquaredError = RegressionMetrics.MeanSquaredError(y, y_pred);
            FloatingPointAssert().Equal(meanSquaredError, 0.002978013970196411, 1e-10);
        }

        private static (Matrix<double> x, Vector<double> y, Vector<double> theta) Generate()
        {
            var n = 1000;
            var m = 100;
            var random = new Random(42);
            var x = Matrix<double>.Build.Dense(n, m, (i, j) => random.NextDouble() * 2 - 1);
            var theta = Vector<double>.Build.Dense(m + 1, i => random.NextDouble() * 2 - 1);
            var y = x * theta.SubVector(1, m) + theta[0] + Vector<double>.Build.Dense(n, i => random.NextDouble() * 0.2 - 0.1);
            return (x, y, theta);
        }

    }
}
