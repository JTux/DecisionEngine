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
            var matrixWithAddedColumns = AddColumnTotals(matrixWithSetArrayValues, itemCount);
            var multipliedMatrix = CreateMultipliedNormalizeMatrix(matrixWithAddedColumns, itemCount);
            var matrixWithEigenVectorRows = CalculateEigenVectorRows(multipliedMatrix, itemCount);

            return matrixWithEigenVectorRows;
        }

        private double[,] CalculateEigenVectorRows(double[,] matrix, int itemCount)
        {
            for (int y = 0; y < itemCount; y++)
            {
                var totalRowValue = 0.0;

                for (int x = 0; x < itemCount; x++)
                {
                    var cellValue = matrix[x, y];
                    totalRowValue += cellValue;
                    if (x == (itemCount - 1))
                        matrix[itemCount, y] = totalRowValue / itemCount;
                };
            }

            return matrix;
        }

        private double[,] NormalizeMatrix(double[,] matrix, int itemCount)
        {
            for (int i = 0; i < itemCount; i++)
                matrix[i, i] = (1.0);

            return matrix;
        }

        private double[,] CreateMultipliedNormalizeMatrix(double[,] matrix, int itemCount)
        {
            for (int x = 0; x < itemCount; x++)
            {
                double bottomCell = matrix[x, itemCount];

                for (int y = 0; y < itemCount; y++)
                {
                    var cellValue = matrix[x, y];
                    var newCellValue = cellValue / bottomCell;
                    matrix[x, y] = newCellValue;
                };
            }
            return matrix;
        }

        private double[,] SetValuesOnNormedMatrix(int[] values, double[,] matrix, int itemCount)
        {
            int count = 0;

            for (int x = 0; x < itemCount; x++)
            {
                for (int y = 0; y < itemCount; y++)
                {
                    if (x >= y) continue;
                    int value = values[count];
                    if (value < 0)
                    {
                        var absolute = Math.Abs(value);
                        matrix[x, y] = GetInverse(absolute);
                        matrix[y, x] = absolute;
                    }
                    else
                    {
                        var absolute = Math.Abs(value);
                        matrix[x, y] = absolute;
                        matrix[y, x] = GetInverse(absolute);
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

        private double[,] AddColumnTotals(double[,] matrix, int itemCount)
        {
            for (int x = 0; x < itemCount; x++)
            {
                double columnTotalValue = 0.0;

                for (int y = 0; y < itemCount; y++)
                {
                    columnTotalValue += (matrix[x, y]);

                    if (y == (itemCount - 1))
                        matrix[x, itemCount] = (columnTotalValue);
                };
            };
            return matrix;
        }
    }
}
