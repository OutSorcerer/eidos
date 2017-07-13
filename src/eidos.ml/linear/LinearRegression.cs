using eidos.ml.abstractions;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearRegression;

namespace eidos.ml.linear
{
    public class LinearRegression : IRegression<LinearRegression, LinearRegressionEstimator>
    {
        private readonly bool _intercept;

        public LinearRegression(bool intercept = true)
        {
            _intercept = intercept;
        }

        public LinearRegressionEstimator Fit(Matrix<double> x, Vector<double> y)
        {
            if (_intercept)
            {
                x = x.InsertColumn(0, Vector<double>.Build.Dense(x.RowCount, Vector<double>.One));
            }
            var w = MultipleRegression.NormalEquations(x, y);
            // TODO: what if matrix is sparse? other methods?
            return new LinearRegressionEstimator(this, w);
        }
    }
}
