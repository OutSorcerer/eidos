using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.metrics
{
    public static class RegressionMetrics
    {
        public static double MeanSquaredError(Vector<double> yTrue, Vector<double> yPred)
        {
            return Distance.MSE(yTrue, yPred);
        }
    }
}
