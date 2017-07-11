using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.abstractions
{
    // ReSharper disable once TypeParameterCanBeVariant
    public interface IRegression<TR, TE>
        where TR : IRegression<TR, TE>
        where TE : IRegressionEstimator<TR, TE>
    {
        TE Fit(Matrix<double> x, Vector<double> y);
    }
}
