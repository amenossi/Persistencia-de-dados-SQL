using Microsoft.Data.SqlClient;
using EstoqueFacil.Objetos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace EstoqueFacil.Banco
{
    public class ProdutoDAL
    {

        public void adicionarProduto(Produto produto)
        {
            using var connection = new Connection().connectionBD();
            connection.Open();

            string sql = @"INSERT INTO tbl_Produtos(IDProduto, Descricao, Valor, Quantidade) VALUES(@id, @descricao, @valor, @quantidade)";
            using SqlCommand command = new SqlCommand(sql, connection);
            {
                command.Parameters.AddWithValue("@id", produto.Id);
                command.Parameters.AddWithValue("@descricao", produto.descricao);
                command.Parameters.AddWithValue("@valor", produto.valor);
                command.Parameters.AddWithValue("@quantidade", produto.quantidade);
            }
            int retorno = command.ExecuteNonQuery();
            Console.WriteLine(retorno);
        }

        public void adcionarProdExistente(Produto produto)
        {
            using var connection = new Connection().connectionBD();
            connection.Open();

            string sql = @"UPDATE tbl_Produtos SET Quantidade = Quantidade + @quantidade WHERE(IDProduto = @id);";

            using SqlCommand command = new SqlCommand( sql, connection);
            command.Parameters.AddWithValue("@id", produto.Id);
            command.Parameters.AddWithValue("@quantidade", produto.quantidade);

            command.ExecuteNonQuery();
        }

        public void subtraiItem(Produto produto)
        {
            using var connection = new Connection().connectionBD();
            connection.Open();

            string sql = @"UPDATE tbl_Produtos SET Quantidade = Quantidade - @quantidade WHERE(IDProduto = @id);";

            using SqlCommand command = new SqlCommand(sql, connection);
            command.Parameters.AddWithValue("@id", produto.Id);
            command.Parameters.AddWithValue("@quantidade", produto.quantidade);

            command.ExecuteNonQuery();

        }

        public void getProdutos(out List<Produto> produtos)
        {
            produtos = new List<Produto>();
            using var connection = new Connection().connectionBD();
            connection.Open();
            string sql = "SELECT * FROM tbl_Produtos";
            using SqlCommand command = new SqlCommand(sql, connection);
            using SqlDataReader dataReader = command.ExecuteReader();
            
            while (dataReader.Read())
            {
                Produto product = new Produto();
                product.Id = Convert.ToInt32(dataReader["IDProduto"]);
                product.descricao = Convert.ToString(dataReader["Descricao"]);
                product.valor = Convert.ToDouble(dataReader["Valor"]);
                product.quantidade = Convert.ToInt32(dataReader["Quantidade"]);

                produtos.Add(product);
            }
        }

        public void deleteProduto(int id)
        {
            using var connection = new Connection().connectionBD();
            connection.Open();

            string sql = @"delete from tbl_Produtos where(IDProduto = @ID);";
            using SqlCommand command = new SqlCommand(sql, connection);

            command.Parameters.AddWithValue($"@ID", id);
            command.ExecuteNonQuery();
            Console.WriteLine("Item removido com sucesso");
            Console.ReadKey();
            
        }

        public void updateProduto(Produto produto)
        {
            using var connection = new Connection().connectionBD();
            connection.Open();

            int Id = produto.Id;
            deleteProduto(Id);
            adicionarProduto(produto);

            Console.WriteLine("Produto alterado com sucesso");
            Console.ReadKey();

        }

        public int validaID(int id)
        {
            using var connection = new Connection().connectionBD();
            connection.Open();

            string sql = @"SELECT CASE 
                     WHEN EXISTS (SELECT 1 FROM tbl_Produtos WHERE IDProduto = @id) 
                     THEN 1 
                     ELSE 0 
                   END AS IDExists";

            using SqlCommand command = new SqlCommand( sql, connection);
            command.Parameters.AddWithValue ("id" , id);

            int result = (int)command.ExecuteScalar();

            return result;
        }

        public void pesquisaItem(string item)
        {
            Console.Clear();
            Produto produto = new();
            using var connection = new Connection().connectionBD();
            connection.Open();
            string sql = "SELECT * FROM tbl_Produtos WHERE LOWER(Descricao) = LOWER(@descricao);";

            using SqlCommand command = new SqlCommand (sql, connection);

            command.Parameters.AddWithValue("descricao", item);
            using SqlDataReader dataReader = command.ExecuteReader();

            while (dataReader.Read())
            {
                produto.descricao = Convert.ToString(dataReader["Descricao"]);
                produto.valor = Convert.ToDouble(dataReader["Valor"]);
                produto.quantidade = Convert.ToInt32(dataReader["Quantidade"]);
            }
            //arruma essa gambiarra aqu, Allan. Pelo amor de Deus heim
            Console.WriteLine(produto.descricao ,produto.quantidade," - R$",produto.valor);
            Console.ReadKey();
        }
    }
}
