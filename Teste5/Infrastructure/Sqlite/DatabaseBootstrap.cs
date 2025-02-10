using Dapper;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Teste5.Domain.Dto;
using Teste5.Domain.Entities;

namespace Questao5.Infrastructure.Sqlite
{
    public class DatabaseBootstrap : IDatabaseBootstrap
    {
        private readonly DatabaseConfig _databaseConfig;

        public DatabaseBootstrap(DatabaseConfig databaseConfig)
        {
            _databaseConfig = databaseConfig;
        }

        public async Task<ContaCorrente> GetContaCorrente(string contaId)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();            

            var query = "SELECT * FROM contacorrente WHERE idcontacorrente = @ContaId";
            var contaC = await connection.QueryFirstOrDefaultAsync<ContaCorrente>(query, new { ContaId = contaId });

            return contaC;
        }

        public async Task<string> GetIdempotencia(string requestId)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();            

            var query = "SELECT resultado FROM idempotencia WHERE chave_idempotencia = @RequestId";
            var idempotencia = await connection.QueryFirstOrDefaultAsync<string>(query, new { RequestId = requestId });

            return idempotencia;
        }

        public async Task CreateMovimentoEIdEmpotencia(MovimentacaoRequest request, string movimentoId)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();            

            using var transaction = await connection.BeginTransactionAsync();

            try
            {
                await connection.ExecuteAsync(
                  "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) VALUES (@Id, @ContaId, @DataMovimento, @Tipo, @Valor)",
                   new
                   {
                       Id = movimentoId,
                       ContaId = request.ContaId,
                       DataMovimento = DateTime.UtcNow.ToString("o"),
                       request.Tipo,
                       request.Valor
                   },
                   transaction
               );

                var serializedRequest = JsonConvert.SerializeObject(request);

                await connection.ExecuteAsync("INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) VALUES (@RequestId, @Requisicao, @Resultado)",
                    new { RequestId = request.RequestId, Requisicao = serializedRequest, Resultado = movimentoId }, transaction);

                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;                
            }                     
        }        

        public async Task<IEnumerable<SaldoContaDto>> GetSaldoContaCorrente(string? contaId)
        {
            using var connection = new SqliteConnection(_databaseConfig.Name);
            await connection.OpenAsync();

            var query = @"
                SELECT cc.numero AS NumeroConta, cc.nome AS NomeTitular, m.valor AS Valor, m.tipomovimento as Tipo
                FROM movimento m 
                INNER JOIN contacorrente cc 
                ON CAST(m.idcontacorrente AS TEXT) = cc.idcontacorrente
                WHERE CAST(m.idcontacorrente AS TEXT) = @idcontacorrente";

            var saldoConta = await connection.QueryAsync<SaldoContaDto>(query, new { idcontacorrente = contaId });

            return saldoConta;
        }

        public void Setup()
        {

            using var connection = new SqliteConnection(_databaseConfig.Name);

            var table = connection.Query<string>("SELECT name FROM sqlite_master WHERE type='table' AND (name = 'contacorrente' or name = 'movimento' or name = 'idempotencia' or name = 'log');");
            var tableName = table.FirstOrDefault();
            if (!string.IsNullOrEmpty(tableName) && (tableName == "contacorrente" || tableName == "movimento" || tableName == "idempotencia" || tableName == "log"))
                return;

            connection.Execute("CREATE TABLE contacorrente ( " +
                               "idcontacorrente TEXT(37) PRIMARY KEY," +
                               "numero INTEGER(10) NOT NULL UNIQUE," +
                               "nome TEXT(100) NOT NULL," +
                               "ativo INTEGER(1) NOT NULL default 0," +
                               "CHECK(ativo in (0, 1)) " +
                               ");");

            connection.Execute("CREATE TABLE movimento ( " +
                "idmovimento TEXT(37) PRIMARY KEY," +
                "idcontacorrente INTEGER(10) NOT NULL," +
                "datamovimento TEXT(25) NOT NULL," +
                "tipomovimento TEXT(1) NOT NULL," +
                "valor REAL NOT NULL," +
                "CHECK(tipomovimento in ('C', 'D')), " +
                "FOREIGN KEY(idcontacorrente) REFERENCES contacorrente(idcontacorrente) " +
                ");");

            connection.Execute("CREATE TABLE idempotencia (" +
                               "chave_idempotencia TEXT(37) PRIMARY KEY," +
                               "requisicao TEXT(1000)," +
                               "resultado TEXT(1000));");


            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('B6BAFC09-6967-ED11-A567-055DFA4A16C9', 123, 'Katherine Sanchez', 1);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('FA99D033-7067-ED11-96C6-7C5DFA4A16C9', 456, 'Eva Woodward', 1);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('382D323D-7067-ED11-8866-7D5DFA4A16C9', 789, 'Tevin Mcconnell', 1);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('F475F943-7067-ED11-A06B-7E5DFA4A16C9', 741, 'Ameena Lynn', 0);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('BCDACA4A-7067-ED11-AF81-825DFA4A16C9', 852, 'Jarrad Mckee', 0);");
            connection.Execute("INSERT INTO contacorrente(idcontacorrente, numero, nome, ativo) VALUES('D2E02051-7067-ED11-94C0-835DFA4A16C9', 963, 'Elisha Simons', 0);");
        }
    }
}
