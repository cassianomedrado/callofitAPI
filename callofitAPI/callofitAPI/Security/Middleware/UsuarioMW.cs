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

        internal async Task<RetornarUserViewModel> CadastroDeUserAsync(Usuario usu)
        {
            usu.senha = Criptografia.HashValue(usu.senha);

            var usuView = await _userDao.CadastroDeUserAsync(usu);

            return usuView;
        }

        public async Task<Usuario> RecuperarUsuarioAsync(Usuario usu)
        {
            usu =  await _userDao.RecuperarUsuarioAsync(usu);
            return usu;
        }

        public async Task<Usuario> VerificarUsuarioSenhaAsync(Usuario usu)
        {
            usu.senha = Criptografia.HashValue(usu.senha);

            var verificado = await _userDao.VerificarUsuarioSenhaAsync(usu);
            
            return verificado;
        }

        internal async Task<bool> DeleteUserAsync(int id)
        {
            return await _userDao.DeleteUserAsync(id);
        }

        internal async Task<bool> alterarSenhaAsync(Usuario usu)
        {
            usu.senha = Criptografia.HashValue(usu.senha);
            return await _userDao.alterarSenhaAsync(usu);
        }
    }
}
