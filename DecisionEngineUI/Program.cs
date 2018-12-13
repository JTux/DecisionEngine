using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DecisionEngineUI
{
    class Program
    {
        static void Main(string[] args)
        {
            #region CRITERIA
            /*Hypothetical Criteria for: 
                In lieu of a console app with questions:
                    What do you like better: 
                        0 Apples or oranges?  -9
                        1 Apples or grapes?   -9
                        2 Apples or candy?    -9
                        3 Oranges or Grapes    9
                        4 Oranges or candy?   -9
                        5 Grapes or candy?    -9

            */
            #endregion
            int itemCount = 4;
            int[] itemValues = { -9, -9, -9, 9, -9, -9 }; // apples > grapes > oranges > candy


            var decisionEngine = new DecisionEngine();
            var heavyApples = decisionEngine.TrackDecision(itemValues, itemCount);
            int k = 0;
            for (int i = 0; i < itemValues.Count() - 1; i++)
            {
                for (int j = 0; j < itemValues.Count() - 1; j++)
                {
                    Console.WriteLine(heavyApples[j, i]);
                }
                Console.WriteLine();
            }

            Console.WriteLine();
            /*
             * 
             *        -9   -7    -5    -3     1    3    5   7   9
             *  Breakdown of the above hierarchy
             *  
                    APPLES ORANGES GRAPES CANDY
            APPLES    1      1/9     1/9    1/9
            ORANGES   9        1       9    1/9
            GRAPES    9      1/9       1    1/9
            CANDY     9        9       9     1
            Eigen V   28     10.2   19.1   1.3


            */



        }
    }
}
