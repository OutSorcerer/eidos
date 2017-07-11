using System.Collections.Generic;
using System.Collections.Immutable;
using eidos.ml.abstractions;
using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.pipelines
{
    public class CompositeTransformation : TransformationBase
    {
        private readonly IImmutableList<ITransformation> _transformations;

        public CompositeTransformation(IImmutableList<ITransformation> transformations)
        {
            _transformations = transformations;
        }
        
        public override (ITransformer, Matrix<double>) FitTransform(Matrix<double> x, Vector<double> y)
        {
            var transformers = new List<ITransformer>();
            var transformedX = x;
            foreach (var transformation in _transformations)
            {
                ITransformer transformer;
                (transformer, transformedX) = transformation.FitTransform(transformedX, y);
                transformers.Add(transformer);
            }
            var compositeTransformer = new CompositeTransformer(transformers.ToImmutableList());
            return (compositeTransformer, transformedX);
        }
    }
}
