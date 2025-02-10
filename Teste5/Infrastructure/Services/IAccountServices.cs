using Teste5.Domain.Dto;
using Teste5.Domain.Entities;

namespace Teste5.Infrastructure.Services
{
    public interface IAccountServices
    {
        Task<string> GetIdempotencia(string requestId);
        Task<ContaCorrente> GetContaCorrente(string contaId);
        Task CreateMovimentoEIdEmpotencia(MovimentacaoRequest request, string movimentoId);        
        Task<SaldoContaDto> GetSaldoContaCorrente(ContaCorrente conta);
    }
}
