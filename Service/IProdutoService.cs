using Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Service
{
    public interface IProdutoService
    {
        Task<Produto?> ConsultarProdutoAsync(string produto);
        Task<IEnumerable<Produto>> GetAllProdutosAsync();
        Task<ViaProdutoResponse> ConsultarViaProdutoAsync(string produto);
        Task<int> AdicionarProdutoAsync(string produto);
    }
}