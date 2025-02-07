namespace Teste5.Domain.Entities
{
    public class MovimentacaoRequest
    {
        public string? RequestId { get; set; }
        public string? ContaId { get; set; }
        public decimal Valor { get; set; }
        public string? Tipo { get; set; }
    }
}
