using Domain;
using Repository;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace Service
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICacheService _cacheService;

        public ProdutoService(IProdutoRepository produtoRepository, ICacheService cacheService)
        {
            _produtoRepository = produtoRepository;
            _cacheService = cacheService;
        }

        // Consulta no cache, depois banco
        public async Task<Produto?> ConsultarProdutoAsync(string produto)
        {
            if (string.IsNullOrWhiteSpace(produto))
                throw new ArgumentException("Insira o Produto.", nameof(produto));

            // Verifica cache
            var produtoCache = await _cacheService.GetAsync(produto);
            if (produtoCache != null)
            {
                return JsonSerializer.Deserialize<Produto>(produtoCache);
            }

            // Verifica banco
            var produtoDb = await _produtoRepository.GetProdutoByCodeAsync(produto);
            if (produtoDb != null)
            {
                var serializedProduto = JsonSerializer.Serialize(produtoDb);
                await _cacheService.SetAsync(produto, serializedProduto, TimeSpan.FromHours(1));
            }

            return produtoDb;
        }

        public async Task<IEnumerable<Produto>> GetAllProdutosAsync()
        {
            return await _produtoRepository.GetAllProdutosAsync();
        }

        // Consulta o ViaCEP e retorna os dados completos
        public async Task<ViaProdutoResponse> ConsultarViaProdutoAsync(string produto)
        {
            using var httpClient = new HttpClient();

            var cepLimpo = produto.Replace("-", "").Trim();
            var url = $"https://viacep.com.br/ws/{cepLimpo}/json/";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            var viaCepResponse = JsonSerializer.Deserialize<ViaProdutoResponse>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });

            if (viaCepResponse == null || viaCepResponse.Erro)
                throw new ArgumentException("CEP não encontrado no ViaCEP.");

            return viaCepResponse;
        }

        // Novo método para adicionar CEP completo
        public async Task<int> AdicionarProdutoAsync(string produto)
        {
            // Consulta ViaCEP
            var viaCep = await ConsultarViaProdutoAsync(produto);

            var cepObj = new Produto
            {
                Nome = viaCep.Nome,
                Categoria = viaCep.Categoria,
                PrecoUnitario = viaCep.PrecoUnitario,
                Qtdmin = viaCep.Qtdmin,
                DataCriacao = DateTime.UtcNow
    };

            return await _produtoRepository.AddProdutoAsync(cepObj);
        }
    }
}
