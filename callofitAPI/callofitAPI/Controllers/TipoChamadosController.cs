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
    public class TipoChamadosController : BaseController
    {
        public TipoChamadosController(INotificador notificador) : base(notificador) { }

        /// <summary>
        /// Retorna todos os tipos de chamados cadastrados.
        /// </summary>
        /// <param name="mwTipoChamados"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getAllTiposChamadosAsync([FromServices] TipoChamadoMW mwTipoChamados)
        {
            try
            {
                var listaTiposChamados = await mwTipoChamados.getAllTiposChamadosAsync();
                if (listaTiposChamados == null || listaTiposChamados.Count() == 0)
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
                    return Ok(listaTiposChamados);
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao buscar tipos de chamados.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Retorna tipo de chamado por id.
        /// </summary>
        /// <param name="mwTipoChamados"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> getTipoChamadoPorIdAsync([FromServices] TipoChamadoMW mwTipoChamados, int id)
        {
            try
            {
                var TipoChamado = await mwTipoChamados.getTipoChamadoPorIdAsync(id);
                if (TipoChamado == null)
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
                    return Ok(TipoChamado);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao buscar tipo de chamado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Cadastrar tipo chamado.
        /// </summary>
        /// <param name="mwTipoChamados"></param>
        /// <param name="tipoChamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CriarTipoChamadoAsync([FromServices] TipoChamadoMW mwTipoChamados,
                                      [FromBody] TipoChamadoViewModel tipoChamado)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<TipoChamadoViewModel>(ModelState.RecuperarErros()));

            try
            {
                var tpChamado = await mwTipoChamados.criarTipoChamadoAsync(new TipoChamadoModel() {descricao = tipoChamado.descricao });
                if (tpChamado.id == 0)
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
                    return Created($"/{tpChamado.id}", tpChamado);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao cadastrar chamado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Alterar tipo de chamado.
        /// </summary>
        /// <param name="mwTipoChamados"></param>
        /// <param name="tipoChamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> putTipoChamadoAsync([FromServices] TipoChamadoMW mwTipoChamados, TipoChamadoModel tipoChamado)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<TipoChamadoModel>(ModelState.RecuperarErros()));

            try
            {
                var sucess = await mwTipoChamados.putTipoChamadoAsync(tipoChamado);

                if (!sucess)
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
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao alterar tipo chamado.");
                return BadRequest(Notificacoes());
            }
        }

        /// <summary>
        /// Deletar um tipo chamado do sistema passando o ID.
        /// </summary>
        /// <param name="mwTipoChamados"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTipoChamadoAsync([FromServices] TipoChamadoMW mwTipoChamados, int id)
        {
            try
            {
                var sucess = await mwTipoChamados.DeleteTipoChamadoAsync(id);

                if (!sucess)
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
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao deletar tipo chamado.");
                return StatusCode(500, Notificacoes());
            }
        }
    }
}
