﻿using callofitAPI.Extensions;
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
    public class TipoUsuarioController : BaseController
    {
        public TipoUsuarioController(INotificador notificador) : base(notificador) { }

        /// <summary>
        /// Retorna todos os tipos de usuário cadastrados.
        /// </summary>
        /// <param name="mwTipoUsuario"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getAllTiposUsuarioAsync([FromServices] TipoUsuarioMW mwTipoUsuario)
        {
            try
            {
                var listaTiposUsuario = await mwTipoUsuario.getAllTiposUsuarioAsync();
                if (listaTiposUsuario == null || listaTiposUsuario.Count() == 0 )
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
                    return Ok(listaTiposUsuario);
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao buscar tipos de usuário.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Retorna tipo de usuário por id.
        /// </summary>
        /// <param name="mwTipoUsuario"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> getTipoUsuarioPorIdAsync([FromServices] TipoUsuarioMW mwTipoUsuario, int id)
        {
            try
            {
                var TipoUsuario = await mwTipoUsuario.getTipoUsuarioPorIdAsync(id);
                if (TipoUsuario == null)
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
                    return Ok(TipoUsuario);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao buscar tipo de usuário.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Cadastrar tipo usuário.
        /// </summary>
        /// <param name="mwTipoUsuario"></param>
        /// <param name="tipoUsuario"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CriarTipoUsuarioAsync([FromServices] TipoUsuarioMW mwTipoUsuario,
                                      [FromBody] TipoUsuarioViewModel tipoUsuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<TipoUsuarioViewModel>(ModelState.RecuperarErros()));

            try
            {
                var tpUsuario = await mwTipoUsuario.criarTipoUsuarioAsync(new TipoUsuarioModel(){ descricao = tipoUsuario .descricao });
                if (tpUsuario.id == 0)
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
                    return Created($"/{tpUsuario.id}", tpUsuario);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao cadastrar tipo usuário.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Alterar tipo de usuário por id.
        /// </summary>
        /// <param name="mwTipoUsuario"></param>
        /// <param name="tipoUsuario"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut]
        public async Task<IActionResult> putTipoUsuarioAsync([FromServices] TipoUsuarioMW mwTipoUsuario, TipoUsuarioModel tipoUsuario)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<TipoUsuarioModel>(ModelState.RecuperarErros()));

            try
            {
                var sucess = await mwTipoUsuario.putTipoUsuarioAsync(tipoUsuario);

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
                Notificar("Falha ao alterar tipo de usuário.");
                return BadRequest(Notificacoes());
            }
        }

        /// <summary>
        /// Deletar um tipo usuário do sistema passando o ID.
        /// </summary>
        /// <param name="mwTipoUsuario"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteTipoUsuarioAsync([FromServices] TipoUsuarioMW mwTipoUsuario, int id)
        {
            try
            {
                var sucess = await mwTipoUsuario.DeleteTipoUsuarioAsync(id);

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
                Notificar("Falha ao deletar tipo usuário.");
                return StatusCode(500, Notificacoes());
            }
        }
    }
}
