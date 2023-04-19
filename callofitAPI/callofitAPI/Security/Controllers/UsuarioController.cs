using callofitAPI.Extensions;
using callofitAPI.Interfaces;
using callofitAPI.Security.Models;
using callofitAPI.Security.Service;
using callofitAPI.Security.ViewModels;
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
    public class UsuarioController : BaseController
    {
        public UsuarioController(INotificador notificador) : base(notificador) { }

        /// <summary>
        /// Retorna todos os usuários cadastrados.
        /// </summary>
        /// <param name="mwUser"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> getAllUsersAsync([FromServices] UsuarioMW mwUser)
        {
            try
            {
                var listaUsu = await mwUser.getAllUsersAsync();
                if (listaUsu == null || listaUsu.Count() == 0   )
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
                    return Ok(listaUsu);
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao buscar usuários.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Retorna dados do usuário pelo username.
        /// </summary>
        /// <param name="mwUser"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> GetUserPorUsernameAsync([FromServices] UsuarioMW mwUser, [FromBody] RetornarUserPorUsernameViewModel username)
        {
            try
            {
                var usu = await mwUser.RecuperarUsuarioAsync( new Usuario () { username = username.username });
                if (usu == null)
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
                    return Ok(usu);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao usuário.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Cadastrar usuário para acesso ao sistema.
        /// </summary>
        /// <param name="mwUser"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("registrar")]
        public async Task<IActionResult> RegisterAsync([FromServices] UsuarioMW neUser, 
                                      [FromBody] RegistrarUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<RegistrarUserViewModel>(ModelState.RecuperarErros()));

            try
            {
                Usuario usu = new Usuario()
                {
                    id = 0,
                    data_criacao = DateTime.Now,
                    nome = viewModel.nome,
                    email = viewModel.email,
                    tipo_usuario_id = viewModel.tipo_usuario_id,
                    username = viewModel.username,
                    senha = viewModel.senha, 
                    status = viewModel.status
                };

                var usuView = await neUser.CadastroDeUserAsync(usu);
                if (usuView.id == 0)
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
                    return Created($"/{usuView.id}", usuView);
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao cadastrar usuário.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Login no sistema com uma conta existente.
        /// </summary>
        /// <param name="_tokenService"></param>
        /// <param name="mwUsuario"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> LoginAsync([FromServices] TokenService _tokenService,
                                  [FromServices] UsuarioMW mwUsuario,
                                  [FromBody] LoginUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<LoginUserViewModel>(ModelState.RecuperarErros()));

            try
            {
                Usuario usu = new Usuario()
                {
                    id = 0,
                    username = viewModel.username,
                    email = null,
                    senha = viewModel.senha
                };

                var usuConsulta = await mwUsuario.VerificarUsuarioSenhaAsync(usu);

                if (usuConsulta != null)
                {
                    if(usuConsulta.id != 0)
                    {
                        var token = await _tokenService.GenerateToken(usuConsulta);
                        return Ok(
                            new
                            {
                                status = HttpStatusCode.OK,
                                Token = token
                            });
                    }
                    else
                    {
                        return Unauthorized(
                           new
                           {
                               status = HttpStatusCode.Unauthorized,
                               Error = Notificacoes()
                           });
                    }           
                }
                else
                {
                    return NotFound(
                        new
                        {
                            status = HttpStatusCode.NotFound,
                            Error = Notificacoes()
                         });
                }
            }
            catch(Exception ex)
            {
                Notificar("Falha ao realizar login.");
                return StatusCode(500,Notificacoes());
            }
        }

        /// <summary>
        /// Deletar um usuário do sistema passando o ID.
        /// </summary>
        /// <param name="mwUser"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> DeleteUserAsync([FromServices] UsuarioMW mwUser, int id)
        {
            try
            {
                var sucess = await mwUser.DeleteUserAsync(id);

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
            catch(Exception ex)
            {
                Notificar("Falha ao deletar usuário.");
                return StatusCode(500, Notificacoes());
            }       
        }

        /// <summary>
        /// Alterar senha de um usuário.
        /// </summary>
        /// <param name="mwUser"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPatch("alterarSenha")]
        public async Task<IActionResult> AlterarSenhaAsync([FromServices] UsuarioMW mwUser, [FromBody] AlterarUserSenhaViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<AlterarUserSenhaViewModel>(ModelState.RecuperarErros()));

            try
            {
                var UsuarioSenhaConfirmada = await mwUser.VerificarUsuarioSenhaAsync(new Usuario() { username = viewModel.username , senha = viewModel.senhaAtual});

                if(UsuarioSenhaConfirmada == null || UsuarioSenhaConfirmada.id == 0)
                {
                    return NotFound(
                      new
                      {
                          status = HttpStatusCode.NotFound,
                          Error = Notificacoes()
                      });
                }

                Usuario usu = new Usuario()
                {
                    id = 0,
                    username = viewModel.username,
                    email = viewModel.email,
                    senha = viewModel.senhaNova
                };
        
                var sucess = await mwUser.alterarSenhaAsync(usu);

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
                Notificar("Falha ao alterar senha do usuário.");
                return BadRequest(Notificacoes());
            }
        }

        /// <summary>
        /// Alterar status de um usuário do sistema.
        /// </summary>
        /// <param name="mwUser"></param>
        /// <param name="alterarStatusUser"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("alterar-status")]
        public async Task<IActionResult> AlterarStatusUsuarioAsync([FromServices] UsuarioMW mwUser, [FromBody] AlterarStatusUserViewModel alterarStatusUser)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<AlterarStatusUserViewModel>(ModelState.RecuperarErros()));

            try
            {
                var sucess = await mwUser.AlterarStatusUsuarioAsync(alterarStatusUser);

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
                Notificar("Falha ao alterar status do usuário.");
                return StatusCode(500, Notificacoes());
            }
        }


        /// <summary>
        /// Retorna dados do usuário pelo ID.
        /// </summary>
        /// <param name="mwUser"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("Usuario-por-id")]
        public async Task<IActionResult> RecuperarUsuarioPorIdAsync([FromServices] UsuarioMW mwUser, [FromBody] RetornarUserPorUIdViewModel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<RetornarUserPorUIdViewModel>(ModelState.RecuperarErros()));

            if (request.id == 0)
            {
                return Ok(new RetornarUserViewModel() { id = 0 });
            }

            try
            {
                var usu = await mwUser.RecuperarUsuarioPorIdAsync(new Usuario() { id = request.id });
                if (usu == null)
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
                    return Ok(usu);
                }
            }
            catch (Exception ex)
            {
                Notificar("Falha ao usuário.");
                return StatusCode(500, Notificacoes());
            }
        }

        /// <summary>
        /// Update de usuário.
        /// </summary>
        /// <param name="mwUser"></param>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut("update")]
        public async Task<IActionResult> UpdateUsuarioAsync([FromServices] UsuarioMW mwUser, [FromBody] AlterarDadosUserViewModel viewModel)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ResultViewModel<AlterarDadosUserViewModel>(ModelState.RecuperarErros()));

            try
            {
                var UsuarioRecuperado = await mwUser.RecuperarUsuarioPorIdAsync(new Usuario() { id = viewModel.id });

                if (UsuarioRecuperado == null || UsuarioRecuperado.id == 0)
                {
                    return NotFound(
                      new
                      {
                          status = HttpStatusCode.NotFound,
                          Error = Notificacoes()
                      });
                }

                UsuarioRecuperado.nome = String.IsNullOrEmpty(viewModel.nome) ? UsuarioRecuperado.nome : viewModel.nome;
                UsuarioRecuperado.email = String.IsNullOrEmpty(viewModel.email) ? UsuarioRecuperado.email : viewModel.email;
                UsuarioRecuperado.tipo_usuario_id = viewModel.tipo_usuario_id == 0  ? UsuarioRecuperado.tipo_usuario_id : viewModel.tipo_usuario_id;

                var sucess = await mwUser.UpdateUsuarioAsync(UsuarioRecuperado);

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
                Notificar("Falha ao alterar dados do usuário.");
                return BadRequest(Notificacoes());
            }
        }
    }
}
