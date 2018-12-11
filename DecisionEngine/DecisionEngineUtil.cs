using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionEngine
{
    public static class DecisionEngineUtil
    {
        public static (double, double, double)[,] TrackDecision(int[] values, int interestLevel)
        {
            var tfnMatrix = CreateTrigonometricFuzzyNumberMatrix();

            var comparisonMatrix = new(double, double, double)[7, 7];

            var normedMatrix = NormalizeMatrix(comparisonMatrix);

            var matrixWithSetArrayValues = SetValuesOnNormedMatrix(values, tfnMatrix, normedMatrix, interestLevel);

            var multipliedMatrix = CreateMultipliedMatrix(matrixWithSetArrayValues);

            var finalComparison = FinalizedMatrix(multipliedMatrix);

            return finalComparison;
        }

        //1
        private static (double, double, double)[,] CreateTrigonometricFuzzyNumberMatrix()
        {
            #region TRIGONOMETRIC FUZZY NUMBER
            /*
                Trigonometric Fuzzy Number: 
            */
            #endregion
            (double, double, double)[,] tfnMatrix =
              {
                    {
                        (1, 1, 1), (1, 2, 3), (2, 3, 4),
                        (3, 4, 5), (4, 5, 6), (5, 6, 7),
                        (6, 7, 8), (7, 8, 9), (9, 9, 9)
                    },
                    {
                        (1, 1, 1), (1.5, 2, 2.5), (2.5, 3, 3.5),
                        (3.5, 4, 4.5), (4.5, 5, 5.5), (5.5, 6, 6.5),
                        (6.5, 7, 7.5), (7.5, 8, 8.5), (9, 9, 9)
                    },
                    {
                        (1, 1, 1), (2, 2, 2), (3, 3, 3),
                        (4, 4, 4), (5, 5, 5), (6, 6, 6),
                        (7, 7, 7), (8, 8, 8), (9, 9, 9)
                    }
                };

            return tfnMatrix;
        }

        //2
        private static (double, double, double)[,] NormalizeMatrix((double, double, double)[,] unNormalizedComparisonMatrix)
        {
            #region NORMALIZATION
            /*
                Normalization: We need to set the comparison of two like objects to 1. We can't compare
                if we like Apple over Apple. In the matrix, this will create a diagonal row of 1s. 
                So when we compare one thing to itself, it's a 1.
                Like this:
        
                        A   B   C   D   E   F
                    A   1   0   0   0   0   0
                    B   0   1   0   0   0   0
                    C   0   0   1   0   0   0
                    D   0   0   0   1   0   0
                    E   0   0   0   0   1   0
                    F   0   0   0   0   0   1

                The loop below does this:
             */
            #endregion
            for (int i = 0; i <= 6; i++)
                unNormalizedComparisonMatrix[i, i] = (1.0, 1.0, 1.0);

            return unNormalizedComparisonMatrix;
        }


        //3
        private static (double, double, double)[,] SetValuesOnNormedMatrix(int[] values, (double, double, double)[,] tfnMatrix, (double, double, double)[,] normedMatrix, int interestLevel)
        {
            //Set the values from the incoming array.
            for (int i = 0; i < 6; i++)
            {
                if (values[i] < 0)
                    normedMatrix[i, i + 1] = tfnMatrix[interestLevel, -1 - values[i]];
                else if (values[i] > 0)
                    normedMatrix[i, i + 1] = tfnMatrix[interestLevel, -1 + values[i]].GetInverse();
                else
                    normedMatrix[i, i + 1] = (1, 1, 1);
            }

            return normedMatrix; 
        }

        //4
        private static (double, double, double)[,] CreateMultipliedMatrix((double, double, double)[,] matrixWithSetArrayValues)
        {
            for (int j = 4; j >= 0; j--)
            {
                for (int k = 0; k <= j; k++)
                    matrixWithSetArrayValues[k, k + 5 - j] = matrixWithSetArrayValues[k, k + 4 - j]
                        .ExecuteFuzzyMultiplication(matrixWithSetArrayValues[k + 4 - j, k + 5 - j]);
            }

            return matrixWithSetArrayValues;
        }

        //5
        private static (double, double, double)[,] FinalizedMatrix((double, double, double)[,] multipliedMatrix)
        {
            // Finally, ij = 1 / ji
            for (int r = 1; r <= 5; r++)
                for (int c = 0; c < r; c++)
                    multipliedMatrix[r, c] = multipliedMatrix[c, r].GetInverse();

            return multipliedMatrix;
        }


        private static (double, double, double) GetInverse(this (double, double, double) a)
        {
            #region INVERSE
            /*
             When we ask a user for a comparison we have the following range of values:
                9  7  5  3  1  1/3 1/5 1/7 1/9
             
            What do you like better. Apples or Oranges?

            Apples                                                                          Oranges
            Totally  A lot  somewhat  a little  same    a little    somewhat    a lot       Totally
               9        7      5        3        1         1/3          1/5        1/7      1/9

            So we use this function if it is on the right part of the scale.
            */
            #endregion
            var inverse = (1.0 / a.Item1, 1.0 / a.Item2, 1.0 / a.Item3);
            return inverse;
        }


        private static (double, double, double) ExecuteFuzzyMultiplication(this (double, double, double) a, (double, double, double) b)
        {
            double[] Items = { a.Item1 * b.Item1, a.Item1 * b.Item3, a.Item3 * b.Item1, a.Item3 * b.Item3 };
            // a1b1 a1b3 a3b1 a3b3
            double Left = Items[0], Middle = a.Item2 * b.Item2, Right = Items[0];

            for (int i = 0; i < 4; i++)
            {
                if (Items[i] < Left)
                    Left = Items[i];
                if (Items[i] > Right)
                    Right = Items[i];
            }

            return (Left, Middle, Right);
        }
    }
}
