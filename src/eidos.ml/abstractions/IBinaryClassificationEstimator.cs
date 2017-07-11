using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.abstractions
{
    public interface IBinaryClassificationEstimator
    {
        Vector<double> Predict(Matrix<double> x);
    }
}
