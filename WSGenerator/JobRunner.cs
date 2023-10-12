using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WSGenerator
{
    public static class JobRunner
    {
        private static string fileconnectionString = "";
        private static string filequerySQL = "";
        private static string csvFile = "";
        private static string csvDelimiter = "";
        public static async Task Execute(string[] args)
        {

            if (args.Count() > 0)
            {
                if (args[0].ToLower() == "exportcsv")
                {
                    DateTimeOffset starting = DateTimeOffset.Now;

                    Console.WriteLine("generating file....");

                    #region setup config
                    StringBuilder sb = new StringBuilder();

                    int l = 0;
                    foreach (string arg in args)
                    {
                        if (l > 0)
                        {
                            sb.Append($"{arg} ");
                        }
                        l++;
                    }

                    string parameters = sb.ToString();
                    string[] param = parameters.Split(new string[] { "--" }, StringSplitOptions.None);
                    foreach (string arg in param)
                    {
                        string[] paramQuery = arg.Split(' ');
                        if (paramQuery.Count() > 0)
                        {
                            if (paramQuery[0].ToLower() == "dbconnection")
                            {
                                fileconnectionString = paramQuery[1];
                            }
                            else if (paramQuery[0].ToLower() == "query")
                            {
                                filequerySQL = paramQuery[1];
                            }
                            else if (paramQuery[0].ToLower() == "csvfile")
                            {
                                string fileName = paramQuery[1].Replace("{Date}", DateTime.Now.ToString("yyyyMMdd HHmmss"));
                                csvFile = fileName;
                            }
                            else if (paramQuery[0].ToLower() == "delimiter")
                            {
                                csvDelimiter = paramQuery[1];
                            }
                        }
                    }
                    #endregion

                    if (!string.IsNullOrEmpty(fileconnectionString) && !string.IsNullOrEmpty(filequerySQL))
                    {
                        string connectionString = "";
                        string querySQL = "";

                        #region config
                        try
                        {
                            string[] lines = File.ReadAllLines(fileconnectionString);
                            if (lines.Length > 0)
                            {
                                connectionString = lines[0];
                            }
                        }
                        catch (Exception)
                        {
                        }

                        try
                        {
                            string[] lines = File.ReadAllLines(filequerySQL);
                            if (lines.Length > 0)
                            {
                                querySQL = string.Join(Environment.NewLine,lines);
                            }
                        }
                        catch (Exception)
                        {
                        }
                        #endregion

                        DataTable dataTable = await DataGenerator.Execute(connectionString, querySQL);
                        await FileGenerator.ExportToCsv(dataTable, csvFile, csvDelimiter);
                    }

                    DateTimeOffset end = DateTimeOffset.Now;
                    Console.WriteLine($"generate file done {(end - starting).TotalSeconds} seconds");
                }
            }
        }
    }
}
