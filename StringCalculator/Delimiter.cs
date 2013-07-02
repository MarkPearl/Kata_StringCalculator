using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace StringCalculator
{
    public class Delimiter : IDelimiter
    {
        public IEnumerable<string> Determine(string input)
        {
            var innerDelimiterSpecification = ExtractInnerDelimiterSpecification(input);
            var multipleDelimiters = ExtractMultipleDelimiterSpecification(innerDelimiterSpecification);
            if (multipleDelimiters.Count == 0) return DetermineSingleDelimiter(input);

            var delimitersCollection = (from Match item in multipleDelimiters select item.Value).ToList<string>();
            return delimitersCollection;
        }

        private static MatchCollection ExtractMultipleDelimiterSpecification(string innerDelimiterSpecification)
        {
            return Regex.Matches(innerDelimiterSpecification, "(?<=\\[).+?(?=\\])");
        }

        private string ExtractInnerDelimiterSpecification(string input)
        {
            var innerDelimiterSpecification = Regex.Match(input, "(?<=^//).*(?=\n)").Value;
            if (innerDelimiterSpecification == string.Empty) return "//,\n";
            return innerDelimiterSpecification;
        }

        private IEnumerable<string> DetermineSingleDelimiter(string input)
        {
            var singleDelimiter = Regex.Matches(input, @"(?<=//).(?=\n)");
            if (singleDelimiter.Count == 0) return new List<string> { "," };
            return new List<string> { singleDelimiter[0].Value };
        }

        public string RemoveSpecification(string input)
        {
            var matches = Regex.Matches(input, @"^//.\n");
            if (matches.Count == 0) return input;
            return input.Substring(matches[0].Value.Length);
        }
    }
}