using CsvHelper;
using ToolCSVTransform.Models;

namespace ToolCSVTransform
{
    public class RecordCsvWriter
    {
        private readonly CsvWriter _csvWriter;

        public RecordCsvWriter(CsvWriter csvWriter)
        {
            _csvWriter = csvWriter;
        }

        public void Write(RecordOutput recordOutput)
        {
            try
            {
                _csvWriter.NextRecord();
                _csvWriter.WriteRecord(recordOutput);
            }
            catch (IOException exc)
            {
                Console.WriteLine(exc);
            }
        }
    }
}
