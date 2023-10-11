using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using System.Text.Json;
using ToolCSVTransform.Models;

namespace ToolCSVTransform
{
    public class RecordCsvReader
    {
        private readonly StreamReader _reader;

        public RecordCsvReader(StreamReader reader)
        {

            _reader = reader;
        }

        public async Task<HashSet<RecordOutput>> ReadAll()
        {
            HashSet<RecordOutput> uniqeSet = new HashSet<RecordOutput>();

            using (var csv = new CsvReader(_reader, new CsvConfiguration(CultureInfo.InvariantCulture)))
            {
                await csv.ReadAsync();
                csv.ReadHeader();

                while (await csv.ReadAsync())
                {
                    try
                    {
                        var requestJson = csv.GetField<string>("request");

                        if (!string.IsNullOrEmpty(requestJson))
                        {
                            var requestData = JsonSerializer.Deserialize<RequestData>(requestJson);

                            if (requestData != null)
                            {
                                var record = new RecordOutput
                                {
                                    DeviceId = requestData.MacId,
                                    DeviceType = "9",
                                    DeviceFamily = "4",
                                    PublicKey = requestData.PublicKey,
                                    DeviceSubType = requestData.DeviceSubType,
                                    SKU = requestData.SKU,
                                    SerialNumber = requestData.SerialNumber
                                };
                                                                
                                uniqeSet.Add(record);
                            }
                            else
                            {
                                Console.WriteLine("Error: Deserialized request data is null.");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Error: Request data is null or empty.");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"An error occurred while processing a line: {ex.Message}");
                    }
                }
            }

            return uniqeSet;
        }
    }
}
