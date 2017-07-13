using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace eidos.ml.metrics
{
    public static class RegressionMetrics
    {
        public static double MeanSquaredError(Vector<double> yTrue, Vector<double> yPred)
        {
            return (yTrue - yPred).PointwisePower(2).Mean();
        }
    }
}
