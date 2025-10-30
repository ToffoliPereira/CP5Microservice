namespace Domain
{
    public class MovimentacaoEstoque
    {
        public int Id { get; set; }
        public string Tipo { get; set; }
        public int Qtd { get; set; }
        public DateTime DataMovimentacao { get; set; }
        public string Lote { get; set; }
        public DateTime DataValidade { get; set; }
    }
}
