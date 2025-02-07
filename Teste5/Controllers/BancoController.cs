using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Questao5.Infrastructure.Sqlite;
using System.Runtime.CompilerServices;
using Teste5.Domain.Entities;
using Teste5.Infrastructure.Services;

namespace Teste5.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BancoController : Controller
    {
        private readonly IAccountServices _accountServices;

        public BancoController(IAccountServices accountService)
        {
            _accountServices = accountService;
        }

        // POST: BancoController/Create
        [HttpPost]
        public IActionResult Movimentar([FromBody] MovimentacaoRequest request)
        {
            try
            {
                if (request is null)
                    return BadRequest("request is null");

                var idempotencia = _accountServices.GetIdempotencia(request.RequestId).Result;

                if (idempotencia is not null)
                    return Ok(new { Movimentoid = idempotencia });
                else
                {
                    var conta = _accountServices.GetContaCorrente(request.ContaId.Trim()).Result;

                    if (conta is null)
                        return BadRequest(new { Tipo = "INVALID_ACCOUNT", Mensagem = "Conta não cadastrada " });

                    ValidacaoConta(conta, request);

                    string movimentoId = _accountServices.CreateMovimento(request).Result;

                    _accountServices.CreateIdEmpotencia(request, movimentoId);

                    return Ok(new { MovimentoId = movimentoId });

                }
            }
            catch
            {
                return BadRequest(new { Mensagem = "Ocorreu um erro ao processar ao realizar a movimentação." });
            }
        }

        private BadRequestObjectResult ValidacaoConta(ContaCorrente conta, MovimentacaoRequest request)
        {
            if (conta.Ativo == 0)
                return BadRequest(new { Tipo = "INACTIVE_ACCOUNT", Mensagem = "Conta corrente inativa." });

            if (request.Valor <= 0)
                return BadRequest(new { Tipo = "INVALID_VALUE", Mensagem = "Valor deve ser positivo." });

            if (request.Tipo != "C" && request.Tipo != "D")
                return BadRequest(new { Tipo = "INVALID_TYPE", Mensagem = "Tipo de movimento inválido." });

            return null;
        }
    }
}
