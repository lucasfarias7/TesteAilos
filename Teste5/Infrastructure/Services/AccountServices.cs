using Questao5.Infrastructure.Sqlite;
using Teste5.Domain.Dto;
using Teste5.Domain.Entities;

namespace Teste5.Infrastructure.Services
{
    public class AccountServices : IAccountServices
    {
        private readonly IDatabaseBootstrap _database;

        public AccountServices(IDatabaseBootstrap database)
        {
            _database = database;
        }

        public async Task<ContaCorrente> GetContaCorrente(string contaId)
        {
            return await _database.GetContaCorrente(contaId);
        }

        public async Task<string> GetIdempotencia(string requestId)
        {
            return await _database.GetIdempotencia(requestId);
        }

        public async Task CreateMovimentoEIdEmpotencia(MovimentacaoRequest request, string movimentoId)
        {
            await _database.CreateMovimentoEIdEmpotencia(request, movimentoId);
        }        

        public async Task<SaldoContaDto> GetSaldoContaCorrente(ContaCorrente conta)
        {            
            var saldoConta = await _database.GetSaldoContaCorrente(conta.IdContaCorrente);

            if (saldoConta is null && !saldoConta.Any())
                return new SaldoContaDto()
                {
                    hasSaldo = false,
                    Saldo = 0
                };

            decimal saldo = saldoConta.Where(m => m.Tipo == "C").Sum(m => m.Valor) - saldoConta.Where(m => m.Tipo == "D").Sum(m => m.Valor);

            return new SaldoContaDto()
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                Saldo = saldo,
                dataConsulta = DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"),
                hasSaldo = true
            };         
        }
    }
}
