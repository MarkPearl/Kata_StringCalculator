using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using StringCalculator;

namespace StringCalculatorTests
{
    [TestFixture]
    public class CalculatorTests
    {
        private Calculator _calculator;
        private IDelimiter _delimiter;

        [SetUp]
        public void Initialize()
        {
            _delimiter = MockRepository.GenerateStub<IDelimiter>();
            _calculator = new Calculator(_delimiter);
        }

        [Test]
        public void EmptyStringReturnsZero()
        {
            _delimiter.Expect(x => x.RemoveSpecification("")).IgnoreArguments().Return("");
            Assert.That(_calculator.Add(string.Empty), Is.EqualTo(0));
        }

        [Test]
        public void SingleNumberReturnsNumber()
        {
            _delimiter.Expect(x => x.RemoveSpecification("")).IgnoreArguments().Return("1");
            _delimiter.Expect(x => x.Determine("")).IgnoreArguments().Return(GenerateCollectionWithOneItem(","));
            Assert.That(_calculator.Add("1"), Is.EqualTo(1));
        }

        [Test]
        public void TwoNumbersReturnsSum()
        {
            _delimiter.Expect(x => x.RemoveSpecification("")).IgnoreArguments().Return("1,2");
            _delimiter.Expect(x => x.Determine("")).IgnoreArguments().Return(GenerateCollectionWithOneItem(","));
            Assert.That(_calculator.Add("1,2"), Is.EqualTo(3));
        }

        [Test]
        public void TreatNewLineAsDelimiter()
        {
            _delimiter.Expect(x => x.RemoveSpecification("")).IgnoreArguments().Return("1,2,3");
            _delimiter.Expect(x => x.Determine("")).IgnoreArguments().Return(GenerateCollectionWithOneItem(","));
            Assert.That(_calculator.Add("1\n2,3"), Is.EqualTo(6));
        }

        private static IEnumerable<string> GenerateCollectionWithOneItem(string item)
        {
            return new List<string> { item };
        }

        [Test]
        public void AllowAddMethodToHandleDifferentDelimiters()
        {
            _delimiter.Expect(x => x.Determine("")).IgnoreArguments().Return(GenerateCollectionWithOneItem(";"));
            _delimiter.Expect(x => x.RemoveSpecification("//;\n1;2")).Return("1;2");
            Assert.That(_calculator.Add("//;\n1;2"), Is.EqualTo(3));
        }


        [TestCase("-1,2", "Negatives not allowed: -1")]
        [TestCase("2,-4,3,-5", "Negatives not allowed: -4,-5")]
        public void CallingAddWithNegativeNumberWillThrowAnException(string input, string exceptionMessage)
        {
            _delimiter.Expect(x => x.Determine("")).IgnoreArguments().Return(GenerateCollectionWithOneItem(","));
            _delimiter.Expect(x => x.RemoveSpecification("")).IgnoreArguments().Return(input);
            var exception = Assert.Throws<NegativeNumberException>(() => _calculator.Add(input));
            Assert.That(exception.Message, Is.EqualTo(exceptionMessage));
        }

        [Test]
        public void NumberBiggerthan1000ShouldBeIgnored()
        {
            _delimiter.Expect(x => x.Determine("")).IgnoreArguments().Return(new List<string>
                {
                    ","
                });
            _delimiter.Expect(x => x.RemoveSpecification("")).IgnoreArguments().Return("1001,2");
            Assert.That(_calculator.Add("1001,2"), Is.EqualTo(2));
        }

        [Test]
        public void DelmitersCanBeOfAnyLength()
        {
            _delimiter.Expect(x => x.Determine("")).IgnoreArguments().Return(GenerateCollectionWithOneItem("***"));
            _delimiter.Expect(x => x.RemoveSpecification("")).IgnoreArguments().Return("1***2***3");
            Assert.That(_calculator.Add("1***2***3"), Is.EqualTo(6));
        }
    }

}