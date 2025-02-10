using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using Teste5.Domain.Dto;
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

        [HttpGet]
        [Route("GetSaldo")]
        public async Task<IActionResult> GetSaldoContaCorrente(string? contaId)
        {
            try
            {
                var contaCorrente = await _accountServices.GetContaCorrente(contaId);

                if (contaCorrente is null)
                    return BadRequest(new { Tipo = "INVALID_ACCOUNT", Mensagem = "Conta não cadastrada." });

                if (contaCorrente.Ativo == 0)
                    return BadRequest(new { Tipo = "INACTIVE_ACCOUNT", Mensagem = "Conta corrente inativa." });

                var saldoConta = await _accountServices.GetSaldoContaCorrente(contaCorrente);

                return Ok(saldoConta);
            }
            catch
            {
                return BadRequest(new { Mensagem = "Ocorreu algum erro ao obter o saldo da conta." });
            }
        }

        // POST: BancoController/Create
        [HttpPost]
        [Route("Movimentar")]
        public async Task<IActionResult> Movimentar([FromBody] MovimentacaoRequest request)
        {
            try
            {
                if (request is null)
                    return BadRequest("request is null");

                var idempotencia = await _accountServices.GetIdempotencia(request.RequestId);

                if (idempotencia is not null)
                    return Ok(new { Movimentoid = idempotencia });
                else
                {
                    var conta = await _accountServices.GetContaCorrente(request.ContaId.Trim());

                    if (conta is null)
                        return BadRequest(new { Tipo = "INVALID_ACCOUNT", Mensagem = "Conta não cadastrada " });

                    ValidacaoConta(conta, request);

                    var movimentoId = Guid.NewGuid().ToString();

                    await _accountServices.CreateMovimentoEIdEmpotencia(request, movimentoId);

                    return Ok(new { MovimentoId = movimentoId });

                }
            }
            catch
            {
                return BadRequest(new { Mensagem = "Ocorreu um erro ao processar a movimentação." });
            }
        }

        private BadRequestObjectResult ValidacaoConta(ContaCorrente conta, MovimentacaoRequest request)
        {
            if (conta.Ativo == 0)
                return BadRequest(new { Tipo = "INACTIVE_ACCOUNT", Mensagem = "Conta corrente inativa." });

            if (request.Valor < 0)
                return BadRequest(new { Tipo = "INVALID_VALUE", Mensagem = "Valor deve ser positivo." });

            if (request.Tipo != "C" && request.Tipo != "D")
                return BadRequest(new { Tipo = "INVALID_TYPE", Mensagem = "Tipo de movimento inválido." });

            return null;
        }
    }
}
