using eidos.ml.abstractions;
using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.linear
{
    public class LinearRegressionEstimator : IRegressionEstimator<LinearRegression, LinearRegressionEstimator>
    {
        public LinearRegression Regression { get; }

        public Vector<double> Weights => _weights.Clone();

        private readonly Vector<double> _weights;

        public LinearRegressionEstimator(LinearRegression linearRegression, Vector<double> weights)
        {
            Regression = linearRegression;
            _weights = weights;
        }

        public Vector<double> Predict(Matrix<double> x)
        {
            if (Regression.Intercept)
            {
                x = x.InsertColumn(0, Vector<double>.Build.Dense(x.RowCount, Vector<double>.One));
            }
            return x * _weights;
        }
    }
}
