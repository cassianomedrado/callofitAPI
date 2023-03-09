using callofitAPI.Security.DAO;
using callofitAPI.Security.Models;
using callofitAPI.Security.ViewModels;
using callofitAPI.Util;

namespace netbullAPI.Security.MidwareDB
{
    public class UsuarioMW
    {
        private UsuarioDAO _userDao;

        public UsuarioMW(UsuarioDAO userDao)
        {
            _userDao = userDao;
        }

        public async Task <List<RetornarUserViewModel>> getAllUsersAsync()
        {
            return await _userDao.getAllUsersAsync();
        }

        public async Task<RetornarUserViewModel> CadastroDeUserAsync(Usuario usu)
        {
            usu.senha = Criptografia.HashValue(usu.senha);

            var usuView = await _userDao.CadastroDeUserAsync(usu);

            return usuView;
        }

        public async Task<RetornarUserViewModel> RecuperarUsuarioAsync(Usuario usu)
        {
            usu =  await _userDao.RecuperarUsuarioAsync(usu);

            RetornarUserViewModel retornoTratado = null;
            if (usu != null)
            {
                retornoTratado = new RetornarUserViewModel()
                {
                    id = usu.id,
                    data_criacao = usu.data_criacao,
                    nome = usu.nome,
                    email = usu.email,
                    tipo_usuario_id = usu.tipo_usuario_id,
                    username = usu.username,
                    status = usu.status
                };
            }

            return retornoTratado;
        }

        public async Task<Usuario> VerificarUsuarioSenhaAsync(Usuario usu)
        {
            usu.senha = Criptografia.HashValue(usu.senha);

            var verificado = await _userDao.VerificarUsuarioSenhaAsync(usu);
            
            return verificado;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            return await _userDao.DeleteUserAsync(id);
        }

        public async Task<bool> alterarSenhaAsync(Usuario usu)
        {
            usu.senha = Criptografia.HashValue(usu.senha);
            return await _userDao.alterarSenhaAsync(usu);
        }

        public async Task<bool> InativarUsuarioAsync(int id)
        {
            return await _userDao.InativarUsuarioAsync(id);
        }

        public async Task<RetornarUserViewModel> RecuperarUsuarioPorIdAsync(Usuario usu)
        {
            usu = await _userDao.RecuperarUsuarioPorIdAsync(usu);

            RetornarUserViewModel retornoTratado = null;
            if (usu != null)
            {
                retornoTratado = new RetornarUserViewModel()
                {
                    id = usu.id,
                    data_criacao = usu.data_criacao,
                    nome = usu.nome,
                    email = usu.email,
                    tipo_usuario_id = usu.tipo_usuario_id,
                    username = usu.username,
                    status = usu.status
                };
            }

            return retornoTratado;
        }
    }
}
