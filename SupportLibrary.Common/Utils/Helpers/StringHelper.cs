using SupportLibrary.Common.Utils.Extensions;

namespace SupportLibrary.Common.Utils.Helpers
{
    internal static class StringHelper
    {
        internal static void ValidateInput(string input)
        {
            input.ThrowExceptionIfNull();
            input.ThrowExceptionIfEmptyOrWhiteSpace();
        }
    }
}