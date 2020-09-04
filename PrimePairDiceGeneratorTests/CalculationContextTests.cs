using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using PrimePairDiceGenerator;

namespace PrimePairDiceGeneratorTests
{
    [TestFixture]
    public class CalculationContextTests
    {
        [TestCase(100)]
        [TestCase(2)]
        [TestCase(1)]
        [TestCase(0)]
        public void TestGetPrimes(int max)
        {
            // act 
            var sut = CalculationContext.GetPrimes(max).ToList();

            // assert
            sut.Should().NotBeNull();
            foreach (var prime in sut)
            {
                IsPrime(prime).Should().BeTrue();
            }
            for (var i=2; i<=max; i++)
            {
                if (IsPrime(i))
                {
                    sut.Should().Contain(i);
                }
            }
        }

        [TestCase(100, 4, ExpectedResult = "[(1,7,25,31),(6,12,16,22)]")]
        [TestCase(31, 4, ExpectedResult = "[(1,7,25,31),(6,12,16,22)]")]
        [TestCase(30, 4, ExpectedResult = null)]
        [TestCase(2, 1, ExpectedResult = "[(1),(2)]")]
        [TestCase(1, 1, ExpectedResult = null)]
        [TestCase(0, 4, ExpectedResult = null)]
        [TestCase(-1, 4, ExpectedResult = null)]
        [TestCase(30, 0, ExpectedResult = null)]
        [TestCase(30, -1, ExpectedResult = null)]
        public string? TestGetSmallestDicePair(int max, int diceSides)
        {
            // arrange
            var calculationContext = new CalculationContext(max, diceSides);

            // act 
            var sut = calculationContext.GetSmallestDicePair();

            // assert
            if (sut != null)
            {
                foreach (var odd in sut.OddComb)
                {
                    foreach (var even in sut.EvenComb)
                    {
                        (odd % 2).Should().Be(1);
                        (even % 2).Should().Be(0);
                        IsPrime(odd + even).Should().BeTrue();
                    }
                }
            }

            return sut?.ToString();
        }
 
        private static bool IsPrime(int p)
        {
            if (p < 2)
            {
                return false;
            }
            for (var i = 2; i < p; i++)
            {
                if (p % i == 0)
                {
                    return false;
                }
            }

            return true;
        }
    }
}