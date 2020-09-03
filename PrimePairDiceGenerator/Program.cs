using System;

namespace PrimePairDiceGenerator
{
    public class Program
    {
        public static void Main()
        {
            var calculationContext = new CalculationContext(100, 6, true);
            var smallestPair = calculationContext.GetSmallestDicePair();
            if (smallestPair != null)
            {
                Console.WriteLine();
                Console.WriteLine($"{smallestPair} (Max = {smallestPair.GetMax()}, Sum = {smallestPair.GetSum()})");
            }
            else
            {
                Console.WriteLine();
                Console.WriteLine("No pair found!");
            }
        }
    }
}
