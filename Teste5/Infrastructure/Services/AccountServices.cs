using Questao5.Infrastructure.Sqlite;
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

        public async Task<string> CreateMovimento(MovimentacaoRequest request)
        {
            return await _database.CreateMovimento(request);
        }

        public async Task CreateIdEmpotencia(MovimentacaoRequest request, string movimentoId)
        {
            await _database.CreateIdEmpotencia(request, movimentoId);
        }
    }
}
