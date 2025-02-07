using Teste5.Domain.Entities;

namespace Teste5.Infrastructure.Services
{
    public interface IAccountServices
    {
        Task<string> GetIdempotencia(string requestId);

        Task<ContaCorrente> GetContaCorrente(string contaId);

        Task<string> CreateMovimento(MovimentacaoRequest request);
        Task CreateIdEmpotencia(MovimentacaoRequest request, string movimentoId);
    }
}
