using eidos.ml.abstractions;
using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.linear
{
    public class RidgeRegressionEstimator : IRegressionEstimator<RidgeRegression, RidgeRegressionEstimator>
    {
        public Vector<double> Weights => _weights.Clone();

        private Vector<double> _weights;

        public RidgeRegressionEstimator(RidgeRegression regression, Vector<double> weights)
        {
            Regression = regression;
            _weights = weights;
        }
        
        public RidgeRegression Regression { get; }

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
