using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace WSGenerator
{
    public static class DataGenerator
    {
        public static async Task<DataTable> Execute(string conn, string query)
        {
            DataTable dataTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = await command.ExecuteReaderAsync())
                    {
                        dataTable.Load(reader);
                    }
                }
            }

            return dataTable;
        }
    }
}