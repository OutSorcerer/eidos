using System.Collections.Immutable;
using eidos.ml.abstractions;
using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.pipelines
{
    public class CompositeTransformer : ITransformer
    {
        private readonly IImmutableList<ITransformer> _transformers;

        public CompositeTransformer(IImmutableList<ITransformer> transformers)
        {
            _transformers = transformers;
        }

        public Matrix<double> Transform(Matrix<double> x)
        {
            var result = x;
            foreach (var transformer in _transformers)
            {
                result = transformer.Transform(result);
            }
            return result;
        }
    }
}
