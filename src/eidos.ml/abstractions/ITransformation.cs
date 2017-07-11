using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.abstractions
{
    public interface ITransformation
    {
        ITransformer Fit(Matrix<double> x, Vector<double> y);

        (ITransformer, Matrix<double>) FitTransform(Matrix<double> x, Vector<double> y);
    }
}
