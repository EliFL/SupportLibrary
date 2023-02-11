using SupportLibrary.Common.Utils.Extensions;

namespace SupportLibrary.Common.Utils.Helpers
{
    public static class FileAndSystemHelper
    {
        private const string FILE_NOT_FOUND_EXCEPTION_MESSAGE = "File does not exist is specified path";
        private const string EMPTY_CONTENT_EXCEPTION_MESSAGE = "Provided content is empty";
        private const string NO_INFORMATION_EXCEPTION_MESSAGE = "No information was found in provided path";

        public static bool IsFileExist(string pathToFile)
        {
            StringHelper.ValidateInput(pathToFile);

            return File.Exists(pathToFile);
        }

        public static void DeleteFile(string pathToFile)
        {
            if (!IsFileExist(pathToFile))
            {
                throw new FileNotFoundException($"{FILE_NOT_FOUND_EXCEPTION_MESSAGE}, wrong path: {pathToFile}");
            }

            File.Delete(pathToFile);
        }

        public static byte[] ReadFileAsByteArray(string pathToFile)
        {
            if(!IsFileExist(pathToFile))
            {
                throw new FileNotFoundException($"{FILE_NOT_FOUND_EXCEPTION_MESSAGE}, wrong path: {pathToFile}");
            }

            return File.ReadAllBytes(pathToFile);
        }

        public static string ReadFileAsString(string pathToFile)
        {
            if (!IsFileExist(pathToFile))
            {
                throw new FileNotFoundException($"{FILE_NOT_FOUND_EXCEPTION_MESSAGE}, wrong path: {pathToFile}");
            }

            return File.ReadAllText(pathToFile);
        }

        public static void WriteBytesToFile(string pathToFile, byte[] content)
        {
            StringHelper.ValidateInput(pathToFile);

            content.ThrowExceptionIfNull();

            if (content.Length <= 0)
            {
                throw new ArgumentException(EMPTY_CONTENT_EXCEPTION_MESSAGE);
            }

            File.WriteAllBytes(pathToFile, content);
        }

        public static void WriteTextToFile(string pathToFile, string content)
        {
            StringHelper.ValidateInput(pathToFile);

            content.ThrowExceptionIfNull();

            if (content.Length <= 0)
            {
                throw new ArgumentException(EMPTY_CONTENT_EXCEPTION_MESSAGE);
            }

            File.WriteAllText(pathToFile, content);
        }

        public static string GetFileName(string pathToFile, bool IsWithExtension = true)
        {
            StringHelper.ValidateInput(pathToFile);

            var fileName = IsWithExtension ? Path.GetFileName(pathToFile) : Path.GetFileNameWithoutExtension(pathToFile);

            if(fileName == String.Empty)
            {
                throw new ArgumentException($"{NO_INFORMATION_EXCEPTION_MESSAGE}, wrong path: {pathToFile}");
            }

            return fileName;
        }

        public static string GetFileExtension(string pathToFile)
        {
            StringHelper.ValidateInput(pathToFile);

            var extension = Path.GetExtension(pathToFile);

            if(extension == String.Empty)
            {
                throw new ArgumentException($"{NO_INFORMATION_EXCEPTION_MESSAGE}, wrong path: {pathToFile}");
            }

            return extension;
        }
    }
}