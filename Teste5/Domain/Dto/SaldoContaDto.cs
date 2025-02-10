namespace Teste5.Domain.Dto
{
    public class SaldoContaDto
    {
        public bool hasSaldo { get; set; }
        public int NumeroConta { get; set; }
        public string? NomeTitular { get; set; }
        public string? dataConsulta { get; set; }
        public decimal Valor { get; set; }
        public string? Tipo { get; set; }     
        public decimal Saldo { get; set; }
    }
}
