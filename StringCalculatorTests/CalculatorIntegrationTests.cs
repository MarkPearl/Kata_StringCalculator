using NUnit.Framework;
using StringCalculator;

namespace StringCalculatorTests
{
    [TestFixture]
    public class CalculatorIntegrationTests
    {
        [TestCase("1,2",3)]
        [TestCase("//[***]\n1***2***3", 6)]
        [TestCase("//[*][%]\n1*2%3", 6)]
        [TestCase("//[**][%][,]\n1**2%3,4", 10)]
        public void ShouldAddCorrectly(string input, int expected)
        {
            var calculator = new Calculator(new Delimiter());
            Assert.That(calculator.Add(input), Is.EqualTo(expected));
        }
    }
}