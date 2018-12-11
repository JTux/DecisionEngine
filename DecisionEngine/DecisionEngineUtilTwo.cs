using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionEngine
{
    public static class DecisionEngineUtilTwo
    {
        public static double[,] TrackDecision(int[] values)
        {
            var comparisonMatrix = new double[5, 5];

            var normedMatrix = NormalizeMatrix(comparisonMatrix);

            var matrixWithSetArrayValues = SetValuesOnNormedMatrix(values, normedMatrix);

            var matrixWithAddedColumns = AddColumnTotals(matrixWithSetArrayValues);

            var multipliedMatrix = CreateMultipliedNormalizeMatrix(matrixWithAddedColumns);

            var matrixWithEigenVectorRows = CalculateEigenVectorRows(multipliedMatrix);

            return matrixWithEigenVectorRows;
        }

        private static double[,] CalculateEigenVectorRows(double[,] multipliedMatrix)
        {
            var rowTotal = 0.0;
            for(int i = 0; i <= 3; i++)
            {
                double totalForRow = 0.0;
                double lastCell = multipliedMatrix[i, 4];
                var totalCellValue = 0.0;

                int k = 0;

                for (int j = i; k < 4; k++)
                {
                    var cellValue = multipliedMatrix[j, k];
                    totalCellValue += cellValue;
                    if(k == 3)
                    {
                     lastCell = totalCellValue / 4;
                        multipliedMatrix[i, 4] = lastCell;
                    }
                };
            }

            return multipliedMatrix;

        }

        private static double[,] NormalizeMatrix(double[,] unNormalizedComparisonMatrix)
        {
            for (int i = 0; i < 4; i++)
                unNormalizedComparisonMatrix[i, i] = (1.0);

            return unNormalizedComparisonMatrix;
        }

        private static double[,] CreateMultipliedNormalizeMatrix(double[,] matrixWithColumns)
        {
            for (int i = 0; i < 4; i++)
            {
                double bottomCell = matrixWithColumns[4, i];

                int k = 0;

                for (int j = i; k < 4; k++)
                {
                    var cellValue = matrixWithColumns[j, k];
                    var newCellValue = cellValue / bottomCell;
                    matrixWithColumns[j, k] = newCellValue;
                };
            }
            return matrixWithColumns;
        }



        private static double[,] SetValuesOnNormedMatrix(int[] values, double[,] normedMatrix)
        {
            int value;

            for (int i = 0; i < 6; i++)
            {
                switch (i)
                {
                    case 0:
                        value = values[0];
                        if (value < 0)
                        {
                            normedMatrix[0, 1] = GetInverse(value);
                            normedMatrix[1, 0] = (value);
                        } else
                        {
                            normedMatrix[0, 1] = (value);
                            normedMatrix[1, 0] = GetInverse(value);
                        }
                        break;
                    case 1:
                        value = values[1];
                        if (value < 0)
                        {
                            normedMatrix[0, 2] = GetInverse(value);
                            normedMatrix[2, 0] = (value);
                        }
                        else
                        {
                            normedMatrix[0, 2] = (value);
                            normedMatrix[2, 0] = GetInverse(value);
                        }
                        break;
                    case 2:
                        value = values[2];
                        if (value < 0)
                        {
                            normedMatrix[0, 3] = GetInverse(value);
                            normedMatrix[3, 0] = (value);
                        }
                        else
                        {
                            normedMatrix[0, 3] = (value);
                            normedMatrix[3, 0] = GetInverse(value);
                        }
                        break;
                    case 3:
                        value = values[3];
                        if (value < 0)
                        {
                            normedMatrix[1, 2] = GetInverse(value);
                            normedMatrix[2, 1] = Math.Abs((value));
                        }
                        else
                        {
                            normedMatrix[1, 2] = (value);
                            normedMatrix[2, 1] = GetInverse(value);
                        }
                        break;
                    case 4:
                        value = values[4];
                        if (value < 0)
                        {
                            normedMatrix[1, 3] = GetInverse(value);
                            normedMatrix[3, 1] = (value);
                        }
                        else
                        {
                            normedMatrix[1, 3] = (value);
                            normedMatrix[3, 1] = GetInverse(value);
                        }
                        break;
                    case 5:
                        value = values[5];
                        if (value < 0)
                        {
                            normedMatrix[2, 3] = GetInverse(value);
                            normedMatrix[3, 2] = (value);
                        }
                        else
                        {
                            normedMatrix[2, 3] = (value);
                            normedMatrix[3, 2] = GetInverse(value);
                        }
                        break;
                }
            }
            return normedMatrix;
        }

        private static double GetInverse(int a)
        {
            var inverse = Math.Abs((1.0 / a));
            return inverse;
        }

        private static double[,] AddColumnTotals(double[,] matrix)
        {
            for(int i = 0; i < 4; i++)
            {
                int k = 0;
                double columnTotalValue = 0.0;

                for (int j = i; k < 4; k++)
                {
                    double cellValue;
                    (cellValue) = Math.Abs(matrix[k, j]);
                    columnTotalValue += (cellValue);

                    if(k == 3)
                    {
                        matrix[4, j] = (columnTotalValue);
                        break;
                    }
                };
            };
            return matrix;
        }
    }
}
