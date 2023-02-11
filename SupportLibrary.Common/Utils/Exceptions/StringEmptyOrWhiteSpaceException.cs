namespace SupportLibrary.Common.Utils.Exceptions
{
    internal class StringEmptyException : Exception
    {
        internal StringEmptyException() { }

        internal StringEmptyException(string message) : base(message) { }
    }
}