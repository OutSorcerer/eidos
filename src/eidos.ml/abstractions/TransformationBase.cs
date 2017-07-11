using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.abstractions
{
    public abstract class TransformationBase : ITransformation
    {
        public virtual ITransformer Fit(Matrix<double> x, Vector<double> y)
        {
            var (transformer, _) = FitTransform(x, y);
            return transformer;
        }
        
        public virtual (ITransformer, Matrix<double>) FitTransform(Matrix<double> x, Vector<double> y)
        {
            var transformer = Fit(x, y);
            var transformedX = transformer.Transform(x);
            return (transformer, transformedX);
        }
    }
}
