using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.abstractions
{
    // TODO: 1. consider a generic version.
    // TODO: 2. consider a generic and non-generic version for every interface.
    public interface ITransformer
    {
        Matrix<double> Transform(Matrix<double> x);
    }
}
