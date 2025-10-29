namespace Domain
{
    public class MovimentacaoEstoque
    {
        public string Tipo { get; set; }
        public string Qtd { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public string Lote { get; set; }
        public DateTime DataValidade { get; set; }
    }
}
