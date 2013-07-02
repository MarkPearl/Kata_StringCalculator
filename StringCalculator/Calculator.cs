using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StringCalculator
{
    public class Calculator
    {
        private readonly IDelimiter _delimiter;
        private const string NormalDelimiter = "%";

        public Calculator(IDelimiter delimiter)
        {
            _delimiter = delimiter;
        }

        public int Add(string input)
        {
            if (input == string.Empty) return 0;
            var inputWithOutDelimiterSpecification = _delimiter.RemoveSpecification(input);
            var normalizedDelimiter = ReplaceNewLinesWithDelimiters(inputWithOutDelimiterSpecification);

            EnsureNoNegativeNumbers(normalizedDelimiter);

            var numbersInRange = RemoveNumbersOutOfRange(normalizedDelimiter);
            return numbersInRange.Sum();
        }

        private IEnumerable<int> RemoveNumbersOutOfRange(string normalizedInput)
        {
            var matches = Regex.Matches(normalizedInput, @"\d+");
            if (matches.Count == 0) return new List<int> { Convert.ToInt32(normalizedInput) };
            return matches.Cast<Match>().Select(item => Convert.ToInt32(item.Value)).Where(number => number <= 1000).ToList();
        }

        private void EnsureNoNegativeNumbers(string numbers)
        {
            var matches = Regex.Matches(numbers, @"-\d+").Cast<Match>().Select(item => item.Value).ToList();
            if (matches.Count >0) ThrowNegativeNumbersExceptonMessage(matches);
        }

        private void ThrowNegativeNumbersExceptonMessage(IEnumerable<string> numbers)
        {
            var negativeNumbers = GenerateNegativeNumbersText(numbers);
            throw new NegativeNumberException(string.Format("Negatives not allowed: {0}", negativeNumbers));
        }

        private string GenerateNegativeNumbersText(IEnumerable<string> numbers)
        {
            string negativeNumbers = string.Empty;
            foreach (var match in numbers)
            {
                negativeNumbers += match + ",";
            }
            negativeNumbers = negativeNumbers.Substring(0, negativeNumbers.Length - 1);
            return negativeNumbers;
        }

        private string ReplaceNewLinesWithDelimiters(string input)
        {
            return input.Replace(Environment.NewLine, NormalDelimiter);
        }
    }
}