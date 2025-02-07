using Teste5.Domain.Entities;

namespace Questao5.Infrastructure.Sqlite
{
    public interface IDatabaseBootstrap
    {
        Task CreateIdEmpotencia(MovimentacaoRequest request, string movimentoId);
        Task<string> CreateMovimento(MovimentacaoRequest request);
        Task<ContaCorrente> GetContaCorrente(string contaId);
        Task<string> GetIdempotencia(string requestId);
        void Setup();
    }
}