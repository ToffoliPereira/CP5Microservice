using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class ViaProdutoResponse
    {
        public string Nome { get; set; }
        public string Categoria { get; set; }
        public string PrecoUnitario { get; set; }
        public string Qtdmin { get; set; }
        public bool Erro { get; set; }
    }
}
