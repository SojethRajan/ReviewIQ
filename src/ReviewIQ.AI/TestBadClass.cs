using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReviewIQ.AI
{
    public class TestBadClass
    {
        private string connectionString = "Server=prod-db;Password=Admin123;";

        public void ProcessOrder(int id)
        {
            var conn = new SqlConnection(connectionString);
            conn.Open();

            string query = "SELECT * FROM Orders WHERE Id = " + id;
            var cmd = new SqlCommand(query, conn);
            var result = cmd.ExecuteReader();

            if (result != null)
            {
                // TODO: fix this later
                int x = 0;
                int a = 110;
                var data = result;
            }
        }
    }
}
