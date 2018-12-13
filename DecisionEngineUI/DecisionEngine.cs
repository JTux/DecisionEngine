using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionEngineUI
{
    public class DecisionEngine
    {
        public double[,] TrackDecision(int[] values, int itemCount)
        {
            var comparisonMatrix = new double[(itemCount + 1), (itemCount + 1)];
            var normedMatrix = NormalizeMatrix(comparisonMatrix, itemCount);
            var matrixWithSetArrayValues = SetValuesOnNormedMatrix(values, normedMatrix, itemCount);
            //double[,] matrixWithoutNegatives = RemoveNegativesInMatrix(matrixWithSetArrayValues, itemCount);
            var matrixWithAddedColumns = AddColumnTotals(matrixWithSetArrayValues, itemCount);
            var multipliedMatrix = CreateMultipliedNormalizeMatrix(matrixWithAddedColumns, itemCount);
            var matrixWithEigenVectorRows = CalculateEigenVectorRows(multipliedMatrix, itemCount);

            return matrixWithEigenVectorRows;
        }

        //private double[,] RemoveNegativesInMatrix(double[,] matrix, int length)
        //{
        //    double cellValue = 0.0;

        //    for (int i = 0; i < length; i++)
        //    {
        //        int k = 0;

        //        for (int j = i; k < length; k++)
        //        {
        //            (cellValue) = matrix[j, k];
        //            var newCellValue = Math.Abs(cellValue);
        //            matrix[j, k] = newCellValue;
        //        };
        //    }
        //    return matrix;
        //}

        private double[,] CalculateEigenVectorRows(double[,] matrix, int length)
        {
            for (int i = 0; i < length; i++)
            {
                double lastCell = matrix[i, length];
                var totalCellValue = 0.0;

                int k = 0;

                for (int j = i; k < length; k++)
                {
                    var cellValue = matrix[j, k];
                    totalCellValue += cellValue;
                    if (k == (length - 1))
                    {
                        lastCell = totalCellValue / length;
                        matrix[i, length] = lastCell;
                    }
                };
            }

            return matrix;
        }

        private double[,] NormalizeMatrix(double[,] matrix, int length)
        {
            for (int i = 0; i < length; i++)
                matrix[i, i] = (1.0);

            return matrix;
        }

        private double[,] CreateMultipliedNormalizeMatrix(double[,] matrix, int length)
        {
            for (int i = 0; i < length; i++)
            {
                double bottomCell = matrix[length, i];

                int k = 0;

                for (int j = i; k < length; k++)
                {
                    var cellValue = matrix[k, j];
                    var newCellValue = cellValue / bottomCell;
                    matrix[k, j] = newCellValue;
                };
            }
            return matrix;
        }

        private double[,] SetValuesOnNormedMatrix(int[] values, double[,] matrix, int itemCount)
        {
            int count = 0;

            for (int i = 0; i < itemCount; i++)
            {
                for (int j = 0; j < itemCount; j++)
                {
                    if (i >= j) continue;
                    int value = values[count];
                    if (value < 0)
                    {
                        var absolute = Math.Abs(value);
                        matrix[i, j] = GetInverse(absolute);
                        matrix[j, i] = absolute;
                    }
                    else
                    {
                        var absolute = Math.Abs(value);
                        matrix[i, j] = absolute;
                        matrix[j, i] = GetInverse(absolute);
                    }
                    count++;
                }
            }
            return matrix;
        }

        private double GetInverse(int a)
        {
            var inverse = (1.0 / a);
            return inverse;
        }

        private double[,] AddColumnTotals(double[,] matrix, int length)
        {
            for (int i = 0; i < length; i++)
            {
                int k = 0;
                double columnTotalValue = 0.0;

                for (int j = i; k < length; k++)
                {
                    //double cellValue = matrix[k, j];
                    //(cellValue) = Math.Abs(matrix[k, j]);
                    columnTotalValue += (matrix[k, j]);

                    if (k == (length - 1))
                    {
                        matrix[length, j] = (columnTotalValue);
                        //break;
                    }
                };
            };
            return matrix;
        }
    }
}
