using Task1;
using System;
using NUnit.Framework;
using System.Globalization;

namespace Task1Tests
{
    [TestFixture]
    public class PolynomialTests
    {
        const double ACCURACY = 1E-8;

        [TestCase(50,-346,324.89)]
        [TestCase(0, 0, 0)]
        [TestCase(-7004353, 327682, 943690236)]
        public void Constructor_CreateInstance_CreatedCorrectly(double mult1,double mult2, double mult3)
        {
            //arrange is handled by params

            //act
            var p = new Polynomial(mult1,mult2,mult3);

            //assert
            Assert.AreEqual(mult1,p[0], ACCURACY);
            Assert.AreEqual(mult2, p[1], ACCURACY);
            Assert.AreEqual(mult3, p[2], ACCURACY);
        }


        [TestCase(new double[]{42,42,42},3)]
        [TestCase(new double[] { 0, 0, 42 }, 3)]
        [TestCase(new double[] { 42, 42, 0 }, 2)]
        [TestCase(new double[] { 0, 0, 0 }, 0)]
        public void PowerCount_Get_ReturnsExpectedCount(double[] mults,int expectedCount)
        {
            //arrange
            var p = new Polynomial(mults);

            //act
            int actualCount = p.PowerCount;

            //assert
            Assert.AreEqual(actualCount, expectedCount, ACCURACY);
        }

        [Test]
        public void Multiplers_Get_ContainsExpectedValues()
        {
            //arrange
            double[] mults = new double[] { 42, -236, 25136.456 };
            var p = new Polynomial(mults);

            //act
            var actual = p.Multiplers;

            //assert
            for(int i = 0; i < mults.Length; i++)
            {
                Assert.AreEqual(actual[i], mults[i], ACCURACY);
            }
        }
        
        [TestCase(0,715)]
        [TestCase(1, 403)]
        [TestCase(2, 200)]
        [TestCase(3, 0)]
        [TestCase(4, 0)]
        [TestCase(42, 0)]
        public void Indexer_Get_ReturnsExistingElementOrZero(int power, double expected)
        {
            //arrange
            var p = new Polynomial(715, 403, 200, 0);

            //act
            double actual = p[power];

            //assert
            Assert.AreEqual(expected,actual, ACCURACY);
        }
        [TestCase(-1)]
        [TestCase(-15)]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Indexer_GetNegativePower_ArgumentOutOfRangeException(int power)
        {
            //arrange
            var p = new Polynomial(42,42);

            //act
            double actual = p[power];

            //assert is handled by exception
        }

        [Test]
        public void operatorPlus_AddSomePolynomials_ReturnsExpectedPolynomial()
        {
            //arrange
            var p1 = new Polynomial(200, -300, 45.6);
            var p2 = new Polynomial(800, 300, 54.4);

            //act
            var actual = p1 + p2;

            //assert
            Assert.AreEqual(actual[0], 1000, ACCURACY);
            Assert.AreEqual(actual[1], 0, ACCURACY);
            Assert.AreEqual(actual[2], 100, ACCURACY);
        }

        [Test]
        public void operatorMinus_MinusSamePolynomials_ResultPolynomialHasZeroSize()
        {
            //arrange
            var p1 = new Polynomial(42, 16, -30);
            var p2 = new Polynomial(42, 16, -30);

            //act
            var actual = p1 - p2;

            //assert
            Assert.AreEqual(actual.PowerCount, 0);
        }

        [Test]
        public void operatorMultiply_MultiplyPolynomials_ReturnsExpectedPolynomial()
        {
            //arrange
            var p1 = new Polynomial(5, 15, -30);
            var p2 = new Polynomial(4, 3, -1);
            var expected = new Polynomial
            (5 * 4,//zero Power
             5 * 3 + 4 * 15,//first 
             15 * 3 - 30 * 4 - 1 * 5,//second
             -30 * 3 + 15 * -1,// ...
             -30 * -1
            );

            //act
            var actual = p1 * p2;

            //assert
            for (int i = 0; i < actual.PowerCount; i++)
            {
                Assert.AreEqual(expected[i], actual[i], ACCURACY);
            }
        }

        [Test]
        public void ElementOperation_MultiplyToMinuseOne_NegativePolynome()
        {
            //arrange
            var p = new Polynomial(15, 30, 45);
            var expected = new Polynomial(-15, -30, -45);

            //act
            var actual = p.ElementOperation((i) => i * -1);

            //arrange
            for (int i = 0; i < actual.PowerCount; i++)
            {
                Assert.AreEqual(expected[i], actual[i], ACCURACY);
            }
        }

        [Test]
        public void ElementOperation_DivideToMultiplersOfAnotherPolynomial_ReturnsExpectedPolynomial()
        {
            //arrange
            var p1 = new Polynomial(15, 30, -45);
            var p2 = new Polynomial(5, 10, -15);
            var expected = new Polynomial(3, 3, 3);

            //act
            var actual = p1.ElementOperation(p2, (i, j) => i / j);

            //arrange
            for (int i = 0; i < actual.PowerCount; i++)
            {
                Assert.AreEqual(expected[i], actual[i], ACCURACY);
            }
        }

        [TestCase(true, new[] { 42.0, 0.0, -34, 5 })]
        [TestCase(true, new[] { 42.0, 0.0, -34, 5.00000000001 })]
        [TestCase(false, new[] { 42.0, 0.0, -34, 5.1 })]
        [TestCase(false, new[] { 42.0, 0.0, -34 })]
        [TestCase(false, new[] { 42.0, 0.0, -34, 5, 6 })]
        [TestCase(true, new[] { 42.0, 0.0, -34, 5,0 })]
        public void Equals_CompareWithPolynomial_ReturnsExpectedResult(bool expected, params double[] mult)
        {
            //arrange
            var p1 = new Polynomial(42, 0, -34,5);
            var p2 = new Polynomial(mult);
            //act
            bool actual = p1.Equals(p2);

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void Equals_CompareWithObject_ReturnsExpectedResult()
        {
            //arrange
            var p1 = new Polynomial(42, 0, -34, 5);
            var p2 = "lol";
            //act
            bool actual = p1.Equals(p2);

            //assert
            Assert.IsFalse(actual);
        }

        [Test]
        public void ToString_SimpleCall_ReturnsExpectedString()
        {
            //arrange
            var p1 = new Polynomial(41, -42, 0, 13.6);
            string expected = "14X^3-42X+41";

            //act
            string actual = p1.ToString();

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormattedCall_ReturnsExpectedString()
        {
            //arrange
            var p1 = new Polynomial(41, -42, 0, 13.6);
            string expected = "13.6a^3-42.0a+41.0";

            //act
            string actual = p1.ToString("aF1");

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormattedCallWithGermanCulture_ReturnsExpectedString()
        {
            //arrange
            var p1 = new Polynomial(41, -42, 0, 13.6);
            string expected = "13.6a^3-42.0a+41.0";

            //act
            string actual = p1.ToString("aF1");

            //assert
            Assert.AreEqual(expected, actual);
        }

        [Test]
        public void ToString_FormattedCallWithRussianCulture_ReturnsExpectedString()
        {
            //arrange
            var p1 = new Polynomial(41, -42, 0, 13.6);
            string expected = "13,6a^3-42,0a+41,0";

            //act
            string actual = p1.ToString("aF1",new CultureInfo("ru-RU"));

            //assert
            Assert.AreEqual(expected, actual);
        }
    }
}
