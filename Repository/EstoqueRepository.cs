using Dapper;
using Domain;
using MySqlConnector;

namespace Repository
{
    public class EstoqueRepository : IEstoqueRepository
    {
        private readonly MySqlConnection _connection;
        public EstoqueRepository(string connectionString)
        {
            _connection = new MySqlConnection(connectionString);
        }

        public async Task<IEnumerable<MovimentacaoEstoque>> GetAllEstoqueAsync()
        {
            await _connection.OpenAsync();
            string sql = "SELECT id, tipo, qtd, dataMovimentacao, lote, dataValidade FROM estoque;";
            var estoques = await _connection.QueryAsync<MovimentacaoEstoque>(sql);
            await _connection.CloseAsync();
            return estoques;
        }

        public async Task<int> AddEstoqueAsync(MovimentacaoEstoque estoque)
        {
            if (estoque == null)
                throw new ArgumentNullException(nameof(estoque), "Estoque inválido.");
            await _connection.OpenAsync();
            string sql = @"
                INSERT INTO estoque (tipo, qtd, dataMovimentacao, lote, dataValidade)
                VALUES (@Tipo, @Qtd, @DataMovimentacao, @Lote, @DataValidade);
                SELECT LAST_INSERT_ID();
            ";
            var id = await _connection.ExecuteScalarAsync<int>(sql, estoque);
            await _connection.CloseAsync();
            return id;
        }

        public async Task UpdateEstoqueAsync(MovimentacaoEstoque estoque)
        {
            if (estoque == null || estoque.Id <= 0)
                throw new ArgumentException("Estoque inválido.", nameof(estoque));
            await _connection.OpenAsync();
            string sql = @"
                UPDATE estoque
                SET tipo = @Tipo, qtd = @Qtd, dataMovimentacao = @DataMovimentacao, lote = @Lote, dataValidacao = @DataValidade
                WHERE id = @Id;
            ";
            await _connection.ExecuteAsync(sql, estoque);
            await _connection.CloseAsync();
        }

        public async Task DeleteEstoqueAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.", nameof(id));
            await _connection.OpenAsync();
            string sql = "DELETE FROM estoque WHERE id = @Id;";
            await _connection.ExecuteAsync(sql, new { Id = id });
            await _connection.CloseAsync();
        }
    }
}
