using Dapper;
using Domain;
using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Repository
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly MySqlConnection _connection;

        public ProdutoRepository(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public async Task<IEnumerable<Produto>> GetAllProdutosAsync()
        {
            await _connection.OpenAsync();
            try
            {
                string sql = @"
                    SELECT Id, Nome, Categoria, PrecoUnitario, Qtdmin, DataCriacao
                    FROM Produtos;
                ";
                return await _connection.QueryAsync<Produto>(sql);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<int> AddProdutoAsync(Produto produto)
        {
            if (produto == null)
                throw new ArgumentNullException(nameof(produto), "Produto inválido.");

            await _connection.OpenAsync();
            try
            {
                string sql = @"
                    INSERT INTO Produtos 
                        (Nome, Categoria, PrecoUnitario, Qtdmin, DataCriacao)
                    VALUES
                        (@Nome, @Categoria, @PrecoUnitario, @Qtdmin, @DataCriacao);
                    SELECT LAST_INSERT_ID();
                ";
                return await _connection.ExecuteScalarAsync<int>(sql, produto);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }

        public async Task<Produto?> GetProdutoByCodeAsync(string produto)
        {
            if (string.IsNullOrWhiteSpace(produto))
                throw new ArgumentException("Insira o produto.", nameof(produto));

            await _connection.OpenAsync();
            try
            {
                string sql = @"
                    SELECT Id, Nome, Categoria, PrecoUnitario, Qtdmin, DataCriacao
                    FROM Produtos
                    WHERE Nome = @Nome
                    LIMIT 1;
                ";
                return await _connection.QueryFirstOrDefaultAsync<Produto>(sql);
            }
            finally
            {
                await _connection.CloseAsync();
            }
        }
    }
}
