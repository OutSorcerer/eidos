using eidos.ml.abstractions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Statistics;

namespace eidos.ml.metrics
{
    public static class RegressionMetricsExtensions
    {
        public static double MeanSquaredError<TR, TE>(this IRegressionEstimator<TR, TE> estimator, Matrix<double> x, Vector<double> y)
            where TR : IRegression<TR, TE>
            where TE : IRegressionEstimator<TR, TE>
        {
            return RegressionMetrics.MeanSquaredError(y, estimator.Predict(x));
        }
    }
}
