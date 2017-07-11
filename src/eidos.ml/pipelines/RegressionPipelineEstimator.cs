using eidos.ml.abstractions;
using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.pipelines
{
    public class RegressionPipelineEstimator<TR, TE> : IRegressionEstimator<RegressionPipeline<TR, TE>, RegressionPipelineEstimator<TR, TE>> 
        where TR : IRegression<TR, TE>
        where TE : IRegressionEstimator<TR, TE>
    {
        private readonly ITransformer _transformer;
        private readonly IRegressionEstimator<TR, TE> _regressionEstimator;

        public RegressionPipeline<TR, TE> Regression { get; }

        public RegressionPipelineEstimator(RegressionPipeline<TR, TE>  regressionPipeline, ITransformer transformer, 
            IRegressionEstimator<TR, TE> regressionEstimator)
        {
            Regression = regressionPipeline;
            _transformer = transformer;
            _regressionEstimator = regressionEstimator;
        }
        
        public Vector<double> Predict(Matrix<double> x)
        {
            return _regressionEstimator.Predict(_transformer.Transform(x));
        }
    }
}
