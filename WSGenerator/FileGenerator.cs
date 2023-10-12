using CsvHelper;
using CsvHelper.Configuration;
using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Threading.Tasks;

namespace WSGenerator
{
    public static class FileGenerator
    {
        public static async Task ExportToCsv(DataTable dataTable, string fileName, string delimiter)
        {

            if (File.Exists(fileName)) File.Create(fileName);
            Console.WriteLine($"file:{Path.GetFileName(fileName)}");

            //string fileCsv = Path.GetFileName(fileName);
            if (string.IsNullOrEmpty(delimiter)) delimiter = ";";

            CsvConfiguration csvConfig = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                Delimiter = delimiter
            };

            using (var writer = new StreamWriter(fileName))
            using (var csv = new CsvWriter(writer, csvConfig))
            {
                foreach (var c in dataTable.Columns)
                {
                    csv.WriteField(c.ToString());
                }
                await csv.NextRecordAsync();

                int no = 0;
                foreach (var d in dataTable.Rows)
                {
                    DataRow row = dataTable.Rows[no];
                    foreach (var c in dataTable.Columns)
                    {
                        var dataField = row[c.ToString()];
                        csv.WriteField(dataField);
                    }

                    await csv.NextRecordAsync();
                    no++;
                }
            }
        }
    }
}
