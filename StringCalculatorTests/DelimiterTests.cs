using System.Collections.Generic;
using NUnit.Framework;

namespace StringCalculator
{
    [TestFixture]
    public class DelimiterTests
    {
        private Delimiter _delimiter;

        [SetUp]
        public void Initialize()
        {
            _delimiter = new Delimiter();
        }

        [Test]
        public void NoSpecificationReturnsDefaultDelimiter()
        {
            CollectionAssert.AreEquivalent(_delimiter.Determine(string.Empty), new List<string> {","});
        }

        [TestCase("//;\n1;2", ";")]
        public void SpecifiedDelimeterIsDetected(string input, string expected)
        {
            CollectionAssert.AreEquivalent(_delimiter.Determine(input), new List<string> {expected});
        }

        [Test]
        public void DelimitersCanBeOfAnyLength()
        {
            CollectionAssert.AreEquivalent(_delimiter.Determine("//[***]\n1;2"),new List<string> { "***"});
        }

        [Test]
        public void HandleMultipleDelimiters()
        {
            CollectionAssert.AreEquivalent(_delimiter.Determine("//[*][%]\n"), new List<string> { "*", "%" });
        }

        [Test]
        public void InputWithoutDelimiterSpecificationReturnsInput()
        {
            Assert.That(_delimiter.RemoveSpecification("1;2"), Is.EqualTo("1;2"));
        }

        [Test]
        public void InputWithDelimiterSpecificationReturnInputWithoutDelimiterSpecification()
        {
            Assert.That(_delimiter.RemoveSpecification("//;\n1;2"), Is.EqualTo("1;2"));
        }       
    }
}