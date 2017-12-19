using System;

namespace TinyIoc
{
    public class TinyError : Exception
    {
        public TinyError(string message) : base(message) { }
    }
}
