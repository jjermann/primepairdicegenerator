using System;
using System.Collections.Generic;
using System.Linq;

namespace PrimePairDiceGenerator
{
    public class DicePair
    {
        public int DiceSides { get; }
        public IList<int> OddComb { get; }
        public IList<int> EvenComb { get; }

        public DicePair(IList<int> oddComb, IList<int> evenComb)
        {
            if (oddComb.Count < 1 || evenComb.Count != oddComb.Count)
            {
                throw new ArgumentException("Invalid combination pair!");
            }
            DiceSides = oddComb.Count;
            OddComb = oddComb;
            EvenComb = evenComb;
        }

        public int GetMax()
        {
            return OddComb.Union(EvenComb).Max();
        }

        public int GetMaxPrime()
        {
            return OddComb.Max() + EvenComb.Max();
        }

        public int GetSum()
        {
            return OddComb.Sum() + EvenComb.Sum();
        }

        public override string ToString()
        {
            var oddString = string.Join(",", OddComb);
            var evenString = string.Join(",", EvenComb);
            return $"[({oddString}),({evenString})]";
        }
    }
}
