using callofitAPI.Extensions;
using callofitAPI.Interfaces;
using callofitAPI.Models;
using callofitAPI.Util;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netbullAPI.Security.MidwareDB;
using netbullAPI.ViewModels;
using System.Net;

namespace callofitAPI.Security.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HistoricoChamadoController : BaseController
    {
        public HistoricoChamadoController(INotificador notificador) : base(notificador) { }

        /// <summary>
        /// Retorna todos os historico de chamados.
        /// </summary>
        /// <param name="mwHistoricoChamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getAllHistoricoChamadoAsync([FromServices] HistoricoChamadoMW mwHistoricoChamado)
        {
            try
            {
                var listaHistoricoChamado = await mwHistoricoChamado.getAllHistoricoChamadoAsync();
                if (listaHistoricoChamado == null || listaHistoricoChamado.Count() == 0 )
                {
                    return NotFound(
                       new
                       {
                           status = HttpStatusCode.NotFound,
                           Error = Notificacoes()
                       });
                }
                else
                {
                    return Ok(listaHistoricoChamado);
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao buscar os historicos de chamados.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Retorna historico de chamado por id.
        /// </summary>
        /// <param name="mwHistoricoChamado"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> getHistoricoChamadoPorIdAsync([FromServices] HistoricoChamadoMW mwHistoricoChamado, int id)
        {
            try
            {
                var HistoricoChamado = await mwHistoricoChamado.getHistoricoChamadoPorIdAsync(id);
                if (HistoricoChamado == null)
                {
                    return NotFound(
                       new
                       {
                           status = HttpStatusCode.NotFound,
                           Error = Notificacoes()
                       });
                }
                else
                {
                    return Ok(HistoricoChamado);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao buscar historico de chamado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Criar historico de chamado.
        /// </summary>
        /// <param name="mwHistoricoChamado"></param>
        /// <param name="historicoChamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CriarSistemaSuportadoAsync([FromServices] HistoricoChamadoMW mwHistoricoChamado,
                                      [FromBody] HistoricoChamadoViewModel historicoChamado)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<HistoricoChamadoViewModel>(ModelState.RecuperarErros()));

            try
            {
                var criado = await mwHistoricoChamado.criarHistoricoChamadoAsync(new HistoricoChamadoModel() { 
                    data_criacao = DateTime.Now,                                                                                                            
                    acao = historicoChamado.acao,
                    usuario_id = historicoChamado.usuario_id,
                    chamados_id = historicoChamado.chamados_id
                });

                if (!criado)
                {
                    return UnprocessableEntity(
                       new
                       {
                           status = HttpStatusCode.UnprocessableEntity,
                           Error = Notificacoes()
                       });
                }
                else
                {
                    return Ok(historicoChamado);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao criar historico de chamado.");
                return StatusCode(500, Notificacoes());
            }
        }

    }
}
