using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.abstractions
{
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IRegressionEstimator<TR, TE>
        where TR : IRegression<TR, TE>
        where TE : IRegressionEstimator<TR, TE>
    {
        TR Regression { get; }

        Vector<double> Predict(Matrix<double> x);
    }
}
