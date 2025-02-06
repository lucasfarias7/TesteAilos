using System.Globalization;
using Teste1.Model;

ContaBancaria conta;

Console.Write("Entre o número da conta: ");
int numero = Convert.ToInt32(Console.ReadLine());
Console.Write("Entre o titular da conta: ");
string? nomeTitular = Console.ReadLine();
Console.Write("Haverá o deposito inicial (s/n)? ");
char resp = Convert.ToChar(Console.ReadLine());

if (resp == 's' || resp == 'S')
{
    Console.Write("Entre o valor de depósito inicial: ");
    decimal depositoInicial = Convert.ToDecimal(Console.ReadLine(), CultureInfo.InvariantCulture);
    conta = new ContaBancaria(numero, nomeTitular, depositoInicial);
}
else
    conta = new ContaBancaria(numero, nomeTitular);

Console.WriteLine();
Console.WriteLine(conta);

Console.WriteLine();
Console.Write("Entre um valor para depósito: ");
decimal valor =  Convert.ToDecimal(Console.ReadLine(), CultureInfo.InvariantCulture);
conta.Depositar(valor);
Console.WriteLine(conta);

Console.WriteLine();
Console.Write("Entre um valor para saque: ");
valor = Convert.ToDecimal(Console.ReadLine(),CultureInfo.InvariantCulture);
string saldoAtualizadoPosSaque = conta.Sacar(valor);
Console.WriteLine($"\n{saldoAtualizadoPosSaque}\n");
Console.WriteLine(conta);