﻿using System;
using System.Collections.Generic;
using System.Linq;
using Combinatorics.Collections;

namespace PrimePairDiceGenerator
{
    public class CalculationContext
    {
        public int Max { get; }
        public int DiceSides { get; }
        public bool ShowProgress { get; set; }
        private List<int> OddList { get; }
        private List<int> Primes { get; }

        public CalculationContext(int max, int diceSides, bool showProgress = false)
        {
            ShowProgress = showProgress;
            Max = max;
            DiceSides = diceSides;
            var maxIsEven = max % 2 == 0;
            OddList = Enumerable.Range(0, maxIsEven ? max / 2 : (max + 1) / 2).Select(i => 2 * i + 1).ToList();
            Primes = GetPrimes(max*2).ToList();
        }
        
        public static IEnumerable<int> GetPrimes(int max)
        {
            if (max <= 1)
            {
                yield break;
            }
            var remainingSieve = Enumerable.Range(1, max).Skip(1).ToList();
            while (remainingSieve.Any()) 
            {
                var prime = remainingSieve.First();
                yield return prime;
                remainingSieve.RemoveAll(i => i % prime == 0);
            }
        }

        public IEnumerable<DicePair> GetDicePairs()
        {
            var oddCombinations = new Combinations<int>(OddList, DiceSides, GenerateOption.WithoutRepetition);
            var index = 0;
            var lastProgress = 0;
            if (ShowProgress)
            {
                DrawTextProgressBar(0, 100);
            }
            foreach (var oddComb in oddCombinations)
            {
                var evenComb = GetFirstMatchingEvenCombination(oddComb);
                index++;
                if (ShowProgress)
                {
                    var currentProgress = (int) (index * 100 / oddCombinations.Count);
                    if (currentProgress != lastProgress)
                    {
                        DrawTextProgressBar(currentProgress, 100);
                        lastProgress = currentProgress;
                    }
                }
                if (evenComb != null)
                {
                    yield return new DicePair(oddComb, evenComb);
                }
            }
        }

        public DicePair? GetSmallestDicePair()
        {
            var oddCombinations = new Combinations<int>(OddList, DiceSides, GenerateOption.WithoutRepetition);
            var currentMax = -1;
            var currentSum = -1;
            var currentMaxPrime = -1;
            var index = 0;
            DicePair? currentDicePair = null;
            var lastProgress = 0;
            if (ShowProgress)
            {
                DrawTextProgressBar(0, 100);
            }
            foreach (var oddComb in oddCombinations)
            {
                if (ShowProgress)
                {
                    var currentProgress = (int)(index * 100 / oddCombinations.Count);
                    if (currentProgress != lastProgress)
                    {
                        DrawTextProgressBar(currentProgress, 100);
                        lastProgress = currentProgress;
                    }
                }
                index++;
                if (currentMax > 0 && oddComb.Max() > currentMax)
                {
                    continue;
                }
                var evenComb = GetFirstMatchingEvenCombination(oddComb);
                if (evenComb != null)
                {
                    var dicePair = new DicePair(oddComb, evenComb);
                    var newMax = dicePair.GetMax();
                    var newMaxPrime = dicePair.GetMaxPrime();
                    var newSum = dicePair.GetSum();
                    var isImprovement = currentMax < 0
                                        || newMax < currentMax
                                        || (newMax == currentMax && newMaxPrime < currentMaxPrime)
                                        || (newMax == currentMax && newMaxPrime == currentMaxPrime && newSum < currentSum);
                    if (isImprovement)
                    {
                        currentMax = newMax;
                        currentMaxPrime = newMaxPrime;
                        currentSum = newSum;
                        currentDicePair = dicePair;
                    }
                }
            }
            if (ShowProgress)
            {
                DrawTextProgressBar(100, 100);
            }

            return currentDicePair;
        }

        private List<int>? GetFirstMatchingEvenCombination(IList<int> oddCombination)
        {
            if (!oddCombination.Any())
            {
                return null;
            }
            var candidateSet = GetCandidateSet(oddCombination);
            if (candidateSet.Count < DiceSides)
            {
                return null;
            }

            var evenComb = candidateSet.OrderBy(n => n).Take(DiceSides).ToList();
            return evenComb;
        }

        private HashSet<int> GetCandidateSet(IList<int> oddCombination)
        {
            var candidateSet = new HashSet<int>(GetCandidatesForEntry(oddCombination.First()));
            foreach (var num in oddCombination.Skip(1))
            {
                candidateSet.IntersectWith(GetCandidatesForEntry(num));
            }

            return candidateSet;
        }

        private IEnumerable<int> GetCandidatesForEntry(int num)
        {
            // a candidate needs to be even and > 0 and <= Max
            return Primes
                .Where(p => p > 2 && p > num && p <= Max + num)
                .Select(p => p - num);
        }

        private static void DrawTextProgressBar(int progress, int total)
        {
            int progressSize = 30;
            Console.CursorLeft = 0;
            Console.Write("[");
            Console.CursorLeft = progressSize + 2;
            Console.Write("]");
            Console.CursorLeft = 1;
            var oneChunk = (float)progressSize / total;

            int position = 1;
            for (var i = 0; i < oneChunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.White;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            for (var i = position; i < progressSize + 2; i++)
            {
                Console.BackgroundColor = ConsoleColor.Black;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = progressSize + 5;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write($"{progress}%");
        }
        public void ResetTextProgressBar()
        {
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
        }
    }
}
