using System;
using System.Linq;
using eidos.ml.exception;
using MathNet.Numerics;
using MathNet.Numerics.LinearAlgebra;

namespace eidos.ml.tuning
{
    public class ModelValidation
    {
        public static (Matrix<double> xTrain, Vector<double> yTrain, Matrix<double> xTest, Vector<double> yTest) TrainTestSplit(
            Matrix<double> x, Vector<double> y, double trainRatio, bool shuffle = true, int? randomSeed = null)
        {
            if (x.RowCount != y.Count)
            {
                throw new EidosException($"Dimensions of data matrix ({x.RowCount}x{x.ColumnCount}) and target vector ({y.Count}) do not match.");
            }
            var rowCount = x.RowCount;
            var columnCount = x.ColumnCount;
            var trainRowCount = (int) (rowCount * trainRatio);
            var testRowCount = rowCount - trainRowCount;
            var permutation = shuffle
                ? Combinatorics.GeneratePermutation(
                    rowCount,
                    randomSeed.HasValue ? new Random(randomSeed.Value) : null)
                : Enumerable.Range(0, rowCount).ToArray();
            var xTrain = Matrix<double>.Build.Dense(trainRowCount, columnCount);
            var yTrain = Vector<double>.Build.Dense(trainRowCount);
            var xTest = Matrix<double>.Build.Dense(testRowCount, columnCount);
            var yTest = Vector<double>.Build.Dense(testRowCount);
            for (int i = 0; i < trainRowCount; i++)
            {
                var iSource = permutation[i];
                for (int j = 0; j < columnCount; j++)
                {
                    xTrain[i, j] = x[iSource, j];
                    yTrain[i] = y[iSource];
                }
            }
            for (int i = 0; i < testRowCount; i++)
            {
                var iSource = permutation[i + trainRowCount];
                for (int j = 0; j < columnCount; j++)
                {
                    xTest[i, j] = x[iSource, j];
                    yTest[i] = y[iSource];
                }
            }
            return (xTrain, yTrain, xTest, yTest);
        }
    }
}
