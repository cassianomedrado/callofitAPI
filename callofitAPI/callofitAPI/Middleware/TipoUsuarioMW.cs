using callofitAPI.Models;
using callofitAPI.Security.DAO;
using callofitAPI.Security.Models;
using callofitAPI.Util;

namespace netbullAPI.Security.MidwareDB
{
    public class TipoUsuarioMW
    {
        private TipoUsuarioDAO _tipoUsuarioDao;

        public TipoUsuarioMW(TipoUsuarioDAO tipoUsuarioDao)
        {
            _tipoUsuarioDao = tipoUsuarioDao;
        }

        public async Task<List<TipoUsuarioModel>> getAllTiposUsuarioAsync()
        {
            return await _tipoUsuarioDao.getAllTiposUsuarioAsync();
        }

        public async Task<TipoUsuarioModel> getTipoUsuarioPorIdAsync(int id)
        {
            return await _tipoUsuarioDao.getTipoUsuarioPorIdAsync(id);
        }

        public async Task<bool> putTipoUsuarioAsync(TipoUsuarioModel tipoUsuario)
        {
            return await _tipoUsuarioDao.putTipoUsuarioAsync(tipoUsuario);
        }

        public async Task<TipoUsuarioModel> criarTipoUsuarioAsync(TipoUsuarioModel tipoUsuario)
        {
            return await _tipoUsuarioDao.criarTipoUsuarioAsync(tipoUsuario);
        }

        public async Task<bool> DeleteTipoUsuarioAsync(int id)
        {
            return await _tipoUsuarioDao.DeleteTipoUsuarioAsync(id);
        }
    }
}
