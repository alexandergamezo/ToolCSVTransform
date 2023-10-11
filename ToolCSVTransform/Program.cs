using CsvHelper;
using System.Globalization;
using ToolCSVTransform.Models;

namespace ToolCSVTransform
{
    public static class Program
    {
        static async Task Main(string[] args)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Transformation is running");
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Wait...");
                Console.ResetColor();

                string[] csvFiles = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.csv");
                string csvFilePath = string.Empty;

                if (csvFiles.Length > 0)
                {
                    string csvFileName = Path.GetFileName(csvFiles[0]);
                    csvFilePath = Path.Combine(Directory.GetCurrentDirectory(), csvFileName);

                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine("CSV file found: " + csvFilePath);
                    Console.ResetColor();
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("CSV files are not found in the current directory.");
                    Console.ResetColor();
                }


                HashSet<RecordOutput> list;
                string newPath = $"{csvFilePath[..^4]}_Transformed_{DateTime.Now.ToString("yyyyMMdd_HHmmss")}.csv";

                using (StreamReader reader = new StreamReader(csvFilePath))
                {
                    list = await new RecordCsvReader(reader).ReadAll();
                }

                using (FileStream fileStream = File.Open(newPath, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    try
                    {
                        using (StreamWriter writer = new StreamWriter(fileStream))
                        using (CsvWriter csvWriter = new CsvWriter(writer, CultureInfo.InvariantCulture))
                        {
                            csvWriter.WriteHeader<RecordOutput>();
                            foreach (var record in list)
                            {
                                new RecordCsvWriter(csvWriter).Write(record);
                            }
                        }
                    }
                    catch (IOException exc)
                    {
                        Console.WriteLine(exc);
                    }
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Transformation completed. Check your file in the same folder:");
                Console.ResetColor();

                Console.WriteLine(newPath);
            }
            catch (Exception exc)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"An error occurred: {exc.Message}");
                Console.ResetColor();
            }

            Console.ReadKey();
        }
    }
}