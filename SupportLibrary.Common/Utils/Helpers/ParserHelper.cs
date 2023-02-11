using CsvHelper;
using CsvHelper.Configuration;
using SupportLibrary.Common.Utils.Extensions;
using System.Globalization;
using System.IO.Compression;

namespace SupportLibrary.Common.Utils.Helpers
{
    public static class ParserHelper
    {
        public static IList<TType> ParseCSVFileFromStream<TType, TMap>(Stream stream, CsvConfiguration csvConfiguration = null) 
            where TMap : ClassMap<TType>
        {
            stream.ThrowExceptionIfNull();

            using (var reader = new StreamReader(stream))
            {
                using (var csvReader = CreateCsvReader(reader, csvConfiguration))
                {
                    csvReader.Configuration.RegisterClassMap<TMap>();

                    var records = csvReader.GetRecords<TType>().ToList();

                    return records;
                }
            }
        }        

        public static string Unzip(byte[] zippedBuffer)
        {
            zippedBuffer.ThrowExceptionIfNull();

            if (zippedBuffer.Length == 0) { return String.Empty; }

            using (var zippedStream = new MemoryStream(zippedBuffer))
            {
                using (var archive = new ZipArchive(zippedStream))
                {
                    var entry = archive.Entries.FirstOrDefault();

                    if (entry is not null)
                    {
                        using (var unzippedEntryStream = entry.Open())
                        {
                            using (var memoryStream = new MemoryStream())
                            {
                                unzippedEntryStream.CopyTo(memoryStream);
                                var unzippedArray = memoryStream.ToArray();

                                return unzippedArray.ConvertToString();
                            }
                        }
                    }

                    return null;
                }
            }
        }

        private static CsvReader CreateCsvReader(StreamReader reader, CsvConfiguration csvConfiguration)
        {
            return csvConfiguration is null ? new CsvReader(reader, CultureInfo.InvariantCulture) : new CsvReader(reader, csvConfiguration);
        }
    }
}
