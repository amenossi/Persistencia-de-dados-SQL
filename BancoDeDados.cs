using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EstoqueFacil.Banco
{
    internal class Connection
    {
        private string connection = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=EstoqueFacil;Integrated Security=True;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
        public SqlConnection connectionBD()
        {
            return new SqlConnection(connection);
        }
    }
}
