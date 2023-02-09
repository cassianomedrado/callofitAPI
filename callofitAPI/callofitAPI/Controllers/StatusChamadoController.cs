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
    public class StatusChamadoController : BaseController
    {
        public StatusChamadoController(INotificador notificador) : base(notificador) { }

        /// <summary>
        /// Retorna todos os status de chamado cadastrados.
        /// </summary>
        /// <param name="mwStatusChamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getAllStatusChamadoAsync([FromServices] StatusChamadoMW mwStatusChamado)
        {
            try
            {
                var listaStatusChamado = await mwStatusChamado.getAllStatusChamadoAsync();
                if (listaStatusChamado == null || listaStatusChamado.Count() == 0 )
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
                    return Ok(listaStatusChamado);
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao buscar status de chamado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Retorna status de chamado por id.
        /// </summary>
        /// <param name="mwStatusChamado"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> getStatusChamadoPorIdAsync([FromServices] StatusChamadoMW mwStatusChamado, int id)
        {
            try
            {
                var StatusChamado = await mwStatusChamado.getStatusChamadoPorIdAsync(id);
                if (StatusChamado == null)
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
                    return Ok(StatusChamado);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao buscar status de chamado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Cadastrar status de chamado.
        /// </summary>
        /// <param name="mwStatusChamado"></param>
        /// <param name="statusChamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CriarStatusChamadoAsync([FromServices] StatusChamadoMW mwStatusChamado,
                                      [FromBody] StatusChamadoViewModel statusChamado)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<StatusChamadoViewModel>(ModelState.RecuperarErros()));

            try
            {
                var sTatusChamado = await mwStatusChamado.criarStatusChamadoAsync(new StatusChamadoModel(){ descricao = statusChamado.descricao });
                if (sTatusChamado.id == 0)
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
                    return Created($"/{sTatusChamado.id}", sTatusChamado);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao cadastrar status de chamado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Alterar status de chamado por id.
        /// </summary>
        /// <param name="mwStatusChamado"></param>
        /// <param name="statusChamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> putStatusChamadoAsync([FromServices] StatusChamadoMW mwStatusChamado, StatusChamadoModel statusChamado)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<StatusChamadoModel>(ModelState.RecuperarErros()));

            try
            {
                var sucess = await mwStatusChamado.putStatusChamadoAsync(statusChamado);

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
                Notificar("Falha ao alterar status de chamado.");
                return BadRequest(Notificacoes());
            }
        }

        /// <summary>
        /// Deletar um status de chamado do sistema passando o ID.
        /// </summary>
        /// <param name="mwStatusChamado"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteStatusChamadoAsync([FromServices] StatusChamadoMW mwStatusChamado, int id)
        {
            try
            {
                var sucess = await mwStatusChamado.DeleteStatusChamadoAsync(id);

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
                Notificar("Falha ao deletar status de chamado.");
                return StatusCode(500, Notificacoes());
            }
        }
    }
}
