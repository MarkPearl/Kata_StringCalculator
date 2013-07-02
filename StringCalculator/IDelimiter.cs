using System.Collections.Generic;

namespace StringCalculator
{
    public interface IDelimiter
    {
        IEnumerable<string> Determine(string input);
        string RemoveSpecification(string input);
    }
}