using Teste5.Domain.Dto;
using Teste5.Domain.Entities;

namespace Questao5.Infrastructure.Sqlite
{
    public interface IDatabaseBootstrap
    {        
        Task CreateMovimentoEIdEmpotencia(MovimentacaoRequest request, string movimentoId);
        Task<ContaCorrente> GetContaCorrente(string contaId);
        Task<string> GetIdempotencia(string requestId);
        Task<IEnumerable<SaldoContaDto>> GetSaldoContaCorrente(string? contaId);
        void Setup();
    }
}