using System;

namespace PrimePairDiceGenerator
{
    public class Program
    {
        public static void Main()
        {
            var calculationContext = new CalculationContext(100, 6, true);
            var smallestPair = GetSmallestDicePair(calculationContext);
            if (smallestPair != null)
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("Winner:");
                Console.WriteLine($"{smallestPair} (Max = {smallestPair.GetMax()}, Sum = {smallestPair.GetSum()})");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("No pair found!");
            }
        }

        private static DicePair? GetSmallestDicePair(CalculationContext calculationContext)
        {
            var dicePairs = calculationContext.GetDicePairs();
            var currentMax = -1;
            var currentSum = -1;
            DicePair? currentDicePair = null;
            foreach (var dicePair in dicePairs)
            {
                var newMax = dicePair.GetMax();
                var newSum = dicePair.GetSum();
                var isImprovement = currentMax < 0
                                    || newMax < currentMax
                                    || (newMax == currentMax && newSum < currentSum);
                if (isImprovement)
                {
                    calculationContext.ResetTextProgressBar();
                    Console.WriteLine($"{dicePair} (Max = {dicePair.GetMax()}, Sum = {dicePair.GetSum()})");
                    currentMax = newMax;
                    currentSum = newSum;
                    currentDicePair = dicePair;
                }
            }

            return currentDicePair;
        }
    }
}
