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
            var comparisonMatrix = new double[5, 5];
            var normedMatrix = NormalizeMatrix(comparisonMatrix, itemCount);
            var matrixWithSetArrayValues = SetValuesOnNormedMatrix(values, normedMatrix);
            double[,] matrixWithoutNegatives = RemoveNegativesInMatrix(matrixWithSetArrayValues, itemCount);
            var matrixWithAddedColumns = AddColumnTotals(matrixWithoutNegatives, itemCount);
            var multipliedMatrix = CreateMultipliedNormalizeMatrix(matrixWithAddedColumns, itemCount);
            var matrixWithEigenVectorRows = CalculateEigenVectorRows(multipliedMatrix, itemCount);

            return matrixWithEigenVectorRows;
        }

        private double[,] RemoveNegativesInMatrix(double[,] matrix, int length)
        {
            double cellValue = 0.0;

            for (int i = 0; i < length; i++)
            {
                int k = 0;

                for (int j = i; k < length; k++)
                {
                    (cellValue) = matrix[j, k];
                    var newCellValue = Math.Abs(cellValue);
                    matrix[j, k] = newCellValue;
                };
            }
            return matrix;
        }

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
                    var cellValue = matrix[j, k];
                    var newCellValue = cellValue / bottomCell;
                    matrix[j, k] = newCellValue;
                };
            }
            return matrix;
        }
        
        private double[,] SetValuesOnNormedMatrix(int[] values, double[,] matrix)
        {
            int value;

            for (int i = 0; i < values.Count(); i++)
            {
                value = values[i];

                switch (i)
                {
                    case 0:
                        if (value < 0)
                        {
                            matrix[0, 1] = GetInverse(value);
                            matrix[1, 0] = (value);
                        }
                        else
                        {
                            matrix[0, 1] = (value);
                            matrix[1, 0] = GetInverse(value);
                        }
                        break;
                    case 1:
                        if (value < 0)
                        {
                            matrix[0, 2] = GetInverse(value);
                            matrix[2, 0] = (value);
                        }
                        else
                        {
                            matrix[0, 2] = (value);
                            matrix[2, 0] = GetInverse(value);
                        }
                        break;
                    case 2:
                        if (value < 0)
                        {
                            matrix[0, 3] = GetInverse(value);
                            matrix[3, 0] = (value);
                        }
                        else
                        {
                            matrix[0, 3] = (value);
                            matrix[3, 0] = GetInverse(value);
                        }
                        break;
                    case 3:
                        if (value < 0)
                        {
                            matrix[1, 2] = GetInverse(value);
                            matrix[2, 1] = (value);
                        }
                        else
                        {
                            matrix[1, 2] = (value);
                            matrix[2, 1] = GetInverse(value);
                        }
                        break;
                    case 4:
                        if (value < 0)
                        {
                            matrix[1, 3] = GetInverse(value);
                            matrix[3, 1] = (value);
                        }
                        else
                        {
                            matrix[1, 3] = (value);
                            matrix[3, 1] = GetInverse(value);
                        }
                        break;
                    case 5:
                        if (value < 0)
                        {
                            matrix[2, 3] = GetInverse(value);
                            matrix[3, 2] = (value);
                        }
                        else
                        {
                            matrix[2, 3] = (value);
                            matrix[3, 2] = GetInverse(value);
                        }
                        break;
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
                    double cellValue;
                    (cellValue) = Math.Abs(matrix[k, j]);
                    columnTotalValue += (cellValue);

                    if (k == (length - 1))
                    {
                        matrix[length, j] = (columnTotalValue);
                        break;
                    }
                };
            };
            return matrix;
        }
    }
}
