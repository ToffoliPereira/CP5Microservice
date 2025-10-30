using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IProdutoRepository
    {
        Task<IEnumerable<Produto>> GetAllProdutosAsync();
        Task<int> AddProdutoAsync(Produto produto);
        Task<Produto?> GetProdutoByCodeAsync(string produto);
    }
}
