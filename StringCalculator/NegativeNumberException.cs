using System;

namespace StringCalculator
{
    public class NegativeNumberException : Exception
    {
        public NegativeNumberException(string message)
            : base(message)
        {
        }
    }
}