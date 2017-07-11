using eidos.ml.abstractions;
using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.pipelines
{
    public class RegressionPipeline<TR, TE> : IRegression<RegressionPipeline<TR, TE>, RegressionPipelineEstimator<TR, TE>>
        where TR : IRegression<TR, TE>
        where TE : IRegressionEstimator<TR, TE>
    {
        private readonly ITransformation _transformation;
        private readonly IRegression<TR, TE> _regression;

        public RegressionPipeline(ITransformation transformation, IRegression<TR, TE> regression)
        {
            _transformation = transformation;
            _regression = regression;
        }

        public RegressionPipelineEstimator<TR, TE> Fit(Matrix<double> x, Vector<double> y)
        {
            var (transformer, transformedX) = _transformation.FitTransform(x, y);
            var regressionEstimator = _regression.Fit(transformedX, y);
            return new RegressionPipelineEstimator<TR, TE>(this, transformer, regressionEstimator);
        }
    }
}
