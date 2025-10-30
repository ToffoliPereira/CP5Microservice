using Domain;

namespace Repository
{
    public interface IEstoqueRepository
    {
        Task<IEnumerable<MovimentacaoEstoque>> GetAllEstoqueAsync();
        Task<int> AddEstoqueAsync(MovimentacaoEstoque estoque);
        Task UpdateEstoqueAsync(MovimentacaoEstoque estoque);
        Task DeleteEstoqueAsync(int id);
    }
}
