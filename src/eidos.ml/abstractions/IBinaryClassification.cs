using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.abstractions
{
    public interface IBinaryClassification
    {
        IBinaryClassificationEstimator Fit(Matrix<double> x, Vector<double> y);
    }
}
