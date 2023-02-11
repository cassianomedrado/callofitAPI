using callofitAPI.Extensions;
using callofitAPI.Interfaces;
using callofitAPI.Models;
using callofitAPI.Util;
using callofitAPI.ViewModels.SistemaSuportado;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netbullAPI.Security.MidwareDB;
using netbullAPI.ViewModels;
using System.Net;

namespace callofitAPI.Security.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SistemaSuportadoController : BaseController
    {
        public SistemaSuportadoController(INotificador notificador) : base(notificador) { }

        /// <summary>
        /// Retorna todos os sistemas suportados cadastrados.
        /// </summary>
        /// <param name="mwSistemaSuportado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getAllSistemaSuportadoAsync([FromServices] SistemaSuportadoMW mwSistemaSuportado)
        {
            try
            {
                var listaSistemaSuportado = await mwSistemaSuportado.getAllSistemaSuportadoAsync();
                if (listaSistemaSuportado == null || listaSistemaSuportado.Count() == 0 )
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
                    return Ok(listaSistemaSuportado);
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao buscar sistemas suportados.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Retorna sistema suportado por id.
        /// </summary>
        /// <param name="{"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> getSistemaSuportadoPorIdAsync([FromServices] SistemaSuportadoMW mwSistemaSuportado, int id)
        {
            try
            {
                var SistemaSuportado = await mwSistemaSuportado.getSistemaSuportadoPorIdAsync(id);
                if (SistemaSuportado == null)
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
                    return Ok(SistemaSuportado);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao buscar sistema suportado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Cadastrar sistema suportado.
        /// </summary>
        /// <param name="mwSistemaSuportado"></param>
        /// <param name="sistemaSuportado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CriarSistemaSuportadoAsync([FromServices] SistemaSuportadoMW mwSistemaSuportado,
                                      [FromBody] SistemaSuportadoPOSTViewModel sistemaSuportado)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<SistemaSuportadoPOSTViewModel>(ModelState.RecuperarErros()));

            try
            {
                var sistSuportado = await mwSistemaSuportado.criarSistemaSuportadoAsync(new SistemaSuportadoModel() { data_criacao = DateTime.Now ,nome = sistemaSuportado.nome });
                if (sistSuportado.id == 0)
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
                    return Created($"/{sistSuportado.id}", sistSuportado);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao cadastrar sistema suportado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Alterar sistema suportado por id.
        /// </summary>
        /// <param name="mwStatusChamado"></param>
        /// <param name="sistemaSuportado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> putSistemaSuportaAsync([FromServices] SistemaSuportadoMW mwSistemaSuportado, SistemaSuportadoPUTViewModel sistemaSuportado)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<SistemaSuportadoPUTViewModel>(ModelState.RecuperarErros()));

            try
            {
                var sucess = await mwSistemaSuportado.putSistemaSuportadoAsync(
                    new SistemaSuportadoModel() 
                    { 
                        id = sistemaSuportado.id,
                        nome = sistemaSuportado.nome,
                        data_criacao = DateTime.Now 
                    });

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
                Notificar("Falha ao alterar sistema suportado.");
                return BadRequest(Notificacoes());
            }
        }

        /// <summary>
        /// Deletar um sistema suportado passando o ID.
        /// </summary>
        /// <param name="mwSistemaSuportado"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteSistemaSuportaAsync([FromServices] SistemaSuportadoMW mwSistemaSuportado, int id)
        {
            try
            {
                var sucess = await mwSistemaSuportado.DeleteSistemaSuportadAsync(id);

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
                Notificar("Falha ao deletar sistema suportado.");
                return StatusCode(500, Notificacoes());
            }
        }
    }
}
