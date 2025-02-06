using System.Globalization;
using System.Text;

namespace Teste1.Model
{
    public class ContaBancaria
    {
        public int NumeroConta { get; private set; }
        public string? NomeTitular { get; set; }
        public decimal DepositoInicial { get; private set; }
        public decimal SaldoConta { get; private set; }

        private const decimal Taxa = 3.50m;


        public ContaBancaria(int numeroConta, string nomeTitular, decimal depositoInicial)
        {
            this.NumeroConta = numeroConta;
            this.NomeTitular = nomeTitular;
            this.DepositoInicial = depositoInicial;
            this.SaldoConta = this.DepositoInicial;
        }

        public ContaBancaria(int numeroConta, string? nomeTitular)
        {
            this.NumeroConta = numeroConta;
            this.NomeTitular = nomeTitular;
            this.SaldoConta = 0.0m;
            this.DepositoInicial = this.SaldoConta;
        }


        public void Depositar(decimal valor)
        {
            this.SaldoConta += valor;
        }

        public string Sacar(decimal valor)
        {
            if(valor <= this.SaldoConta)
            {
                this.SaldoConta -= valor;
                this.SaldoConta -= Taxa;
                return $"Foi realizado o saque no valor de {valor} da conta do titular {this.NomeTitular}";
            }

            return "Saldo insuficiente para saque";
        }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("Dados da conta atualizado:");
            sb.Append($"Conta {this.NumeroConta}, Titular: {this.NomeTitular}, Saldo: $ {this.SaldoConta.ToString(CultureInfo.InvariantCulture)}");

            return sb.ToString();
        }

    }
}
