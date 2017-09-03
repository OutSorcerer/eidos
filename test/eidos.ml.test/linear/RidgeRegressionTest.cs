using System;
using eidos.ml.linear;
using eidos.ml.test.assertions;
using eidos.ml.tuning;
using Xunit;
using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.test
{
    public class RidgeRegressionTest : EidosTestBase
    {
        // TODO: add trivial tests

        // TODO: add tests for invalid data

        // TODO: test with sparse matrices

        [Fact]
        public void FitPredictLSQR_WithInterceptAndWithRandomData_ReturnsCorrectResult()
        {
            var (x, y, theta) = Generate(intercept: true);
            var (xTrain, yTrain, xTest, yTest) = ModelValidation.TrainTestSplit(x, y, 0.5);

            var estimator = new RidgeRegression(intercept: true, method: RidgeRegressionMethod.LSQR).Fit(xTrain, yTrain);
            var yTrainPred = estimator.Predict(xTrain);
            var yTestPred = estimator.Predict(xTest);
            
            FloatingPointAssert.Equal(theta, estimator.Weights, 1e-3);
            FloatingPointAssert.Equal(yTrain, yTrainPred, 1e-2);
            FloatingPointAssert.Equal(yTest, yTestPred, 1e-2);
        }

        [Fact]
        public void FitPredictLSQR_WithoutInterceptAndWithRandomData_ReturnsCorrectResult()
        {
            var (x, y, theta) = Generate(intercept: false);
            var (xTrain, yTrain, xTest, yTest) = ModelValidation.TrainTestSplit(x, y, 0.5);

            var estimator = new RidgeRegression(intercept: false, method: RidgeRegressionMethod.LSQR).Fit(x, y);
            var yTrainPred = estimator.Predict(xTrain);
            var yTestPred = estimator.Predict(xTest);

            FloatingPointAssert.Equal(theta, estimator.Weights, 1e-3);
            FloatingPointAssert.Equal(yTrain, yTrainPred, 1e-2);
            FloatingPointAssert.Equal(yTest, yTestPred, 1e-2);
        }

        private static (Matrix<double> x, Vector<double> y, Vector<double> theta) Generate(bool intercept)
        {
            var n = 200;
            var m = 10;
            var random = new Random(42);
            var x = Matrix<double>.Build.Dense(n, m, (i, j) => random.NextDouble() * 2 - 1);
            var theta = Vector<double>.Build.Dense(intercept ? m + 1 : m, i => random.NextDouble() * 2 - 1);
            var noise = Vector<double>.Build.Dense(n, i => random.NextDouble() * 0.2 - 0.1);
            var y_clean = (intercept ? x * theta.SubVector(1, m) + theta[0] : x * theta);
            var y = y_clean + noise;
            return (x, y, theta);
        }
    }
}
