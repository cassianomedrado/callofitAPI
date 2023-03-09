using callofitAPI.Extensions;
using callofitAPI.Interfaces;
using callofitAPI.Models;
using callofitAPI.Util;
using callofitAPI.ViewModels.Chamados;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using netbullAPI.Security.MidwareDB;
using netbullAPI.ViewModels;
using System.Net;

namespace callofitAPI.Security.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChamadoController : BaseController
    {
        public ChamadoController(INotificador notificador) : base(notificador) { }

        /// <summary>
        /// Retorna todos os chamados.
        /// </summary>
        /// <param name="mwChamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getAllChamadosAsync([FromServices] ChamadoMW mwChamado)
        {
            try
            {
                var listaChamados = await mwChamado.getAllChamadosAsync();
                if (listaChamados == null || listaChamados.Count() == 0 )
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
                    return Ok(listaChamados);
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao buscar chamados.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Retorna chamado por id.
        /// </summary>
        /// <param name="mwChamado"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> getChamadoPorIdAsync([FromServices] ChamadoMW mwChamado, int id)
        {
            try
            {
                var chamado = await mwChamado.getChamadoPorIdAsync(id);
                if (chamado == null)
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
                    return Ok(chamado);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao buscar chamado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Criar chamado.
        /// </summary>
        /// <param name="mwChamado"></param>
        /// <param name="chamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CriarChamadoAsync([FromServices] ChamadoMW mwChamado,
                                      [FromBody] ChamadoPOSTViewModel chamado)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new ResultViewModel<ChamadoPOSTViewModel>(ModelState.RecuperarErros()));
            }
            else
            {
                if (chamado.sistema_suportado_id == 0)
                {
                    Notificar("Sistema suportado deve ser informado.");
                    return BadRequest(
                       new
                       {
                           status = HttpStatusCode.UnprocessableEntity,
                           Error = Notificacoes()
                       });
                }

                if (chamado.status_chamado_id == 0)
                {
                    Notificar("Stats do chamado deve ser informado..");
                    return BadRequest(
                       new
                       {
                           status = HttpStatusCode.UnprocessableEntity,
                           Error = Notificacoes()
                       });
                }

                if (chamado.tipo_chamado_id == 0)
                {
                    Notificar("Tipo do chamado deve ser informado..");
                    return BadRequest(
                       new
                       {
                           status = HttpStatusCode.UnprocessableEntity,
                           Error = Notificacoes()
                       });
                }
            }

            try
            {
                var criado = await mwChamado.CriarChamadoAsync(new ChamadoModel()
                {
                    data_criacao = DateTime.Now,
                    solicitante = chamado.solicitante,
                    data_limite = chamado.data_limite,
                    status_chamado_id = chamado.status_chamado_id,
                    sistema_suportado_id = chamado.sistema_suportado_id,
                    descricao_problema = chamado.descricao_problema,
                    usuario_id = chamado.usuario_id,
                    descricao_solucao = chamado.descricao_solucao,
                    tipo_chamado_id = chamado.tipo_chamado_id,
                    identificador_unico = Guid.NewGuid()
                });

                if (criado == null)
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
                    return Ok(criado);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao criar o chamado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Alterar chamado.
        /// </summary>
        /// <param name="mwChamado"></param>
        /// <param name="chamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> putChamadoAsync([FromServices] ChamadoMW mwChamado, ChamadoPUTViewModel chamado)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<ChamadoPUTViewModel>(ModelState.RecuperarErros()));

            try
            {
                var sucess = await mwChamado.putChamadoAsync(new ChamadoModel()
                {
                    id = chamado.id,
                    solicitante = chamado.solicitante,
                    data_limite = chamado.data_limite,
                    status_chamado_id = chamado.status_chamado_id,
                    sistema_suportado_id = chamado.sistema_suportado_id,
                    descricao_problema = chamado.descricao_problema,
                    descricao_solucao = chamado.descricao_solucao,
                    tipo_chamado_id = chamado.tipo_chamado_id,
                    tecnico_usuario_id = chamado.tecnico_usuario_id
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
                Notificar("Falha ao alterar chamado.");
                return BadRequest(Notificacoes());
            }
        }

        /// <summary>
        /// Deletar um chamado do sistema passando o ID.
        /// </summary>
        /// <param name="mwChamado"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeletarChamadoAsync([FromServices] ChamadoMW mwChamado, int id)
        {
            try
            {
                var sucess = await mwChamado.DeletarChamadoAsync(id);

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
                Notificar("Falha ao deletar chamado.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Buscar totais de chamados por usuário logado.
        /// </summary>
        /// <param name="mwChamado"></param>
        /// /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("totais")]
        public async Task<IActionResult> getAllTotaisChamadosPorUsuarioAsync([FromServices] ChamadoMW mwChamado, [FromBody]RequestTotaisChamados request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<RequestTotaisChamados>(ModelState.RecuperarErros()));

            try
            {
                var listaTotaisChamados = await mwChamado.getAllTotaisChamadosPorUsuarioAsync(request);
                if (listaTotaisChamados == null)
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
                    return Ok(listaTotaisChamados);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao buscar chamados.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Retorna todos os chamados por usuário logado.
        /// </summary>
        /// <param name="mwChamado"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("chamados-por-usuario")]
        public async Task<IActionResult> getAllChamadosPorUsuarioAsync([FromServices] ChamadoMW mwChamado, [FromBody] RequestBuscarChamados request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<RequestBuscarChamados>(ModelState.RecuperarErros()));

            try
            {
                var listaChamados = await mwChamado.getAllChamadosPorUsuarioAsync(request);
                if (listaChamados == null || listaChamados.Count() == 0)
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
                    return Ok(listaChamados);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao buscar chamados.");
                return StatusCode(500, Notificacoes());
            }
        }

    }
}
