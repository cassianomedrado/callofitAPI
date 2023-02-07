using callofitAPI.Models;
using callofitAPI.Security.DAO;

namespace netbullAPI.Security.MidwareDB
{
    public class TipoChamadoMW
    {
        private TipoChamadoDAO _tipoChamadoDao;

        public TipoChamadoMW(TipoChamadoDAO tipoChamadoDao)
        {
            _tipoChamadoDao = tipoChamadoDao;
        }

        public async Task<List<TipoChamadoModel>> getAllTiposChamadosAsync()
        {
            return await _tipoChamadoDao.getAllTiposChamadosAsync();
        }

        public async Task<TipoChamadoModel> getTipoChamadoPorIdAsync(int id)
        {
            return await _tipoChamadoDao.getTipoChamadoPorIdAsync(id);
        }

        public async Task<bool> putTipoChamadoAsync(TipoChamadoModel tipoChamado)
        {
            return await _tipoChamadoDao.putTipoChamadoAsync(tipoChamado);
        }

        public async Task<TipoChamadoModel> criarTipoChamadoAsync(TipoChamadoModel tipoChamado)
        {
            return await _tipoChamadoDao.criarTipoChamadoAsync(tipoChamado);
        }

        public async Task<bool> DeleteTipoChamadoAsync(int id)
        {
            return await _tipoChamadoDao.DeleteTipoChamadoAsync(id);
        }
    }
}
