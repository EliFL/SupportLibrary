namespace SupportLibrary.Common.Utils.Exceptions
{
    internal class StringWhiteSpacesOnlyException : Exception
    {
        internal StringWhiteSpacesOnlyException() { }

        internal StringWhiteSpacesOnlyException(string message) : base(message) { }
    }
}