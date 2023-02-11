using callofitAPI.Models;
using callofitAPI.Security.DAO;

namespace netbullAPI.Security.MidwareDB
{
    public class SistemaSuportadoMW
    {
        private SistemaSuportadoDAO _sistemaSuportadoDao;

        public SistemaSuportadoMW(SistemaSuportadoDAO sistemaSuportadoDao)
        {
            _sistemaSuportadoDao = sistemaSuportadoDao;
        }

        public async Task<List<SistemaSuportadoModel>> getAllSistemaSuportadoAsync()
        {
            return await _sistemaSuportadoDao.getAllSistemaSuportadoAsync();
        }

        public async Task<SistemaSuportadoModel> getSistemaSuportadoPorIdAsync(int id)
        {
            return await _sistemaSuportadoDao.getSistemaSuportaPorIdAsync(id);
        }

        public async Task<SistemaSuportadoModel> criarSistemaSuportadoAsync(SistemaSuportadoModel sistemaSuportado)
        {
            return await _sistemaSuportadoDao.criarSistemaSuportadoAsync(sistemaSuportado);
        }

        public async Task<bool> putSistemaSuportadoAsync(SistemaSuportadoModel sistemaSuportado)
        {
            return await _sistemaSuportadoDao.putSistemaSuportadoAsync(sistemaSuportado);
        }

        public async Task<bool> DeleteSistemaSuportadAsync(int id)
        {
            return await _sistemaSuportadoDao.DeleteSistemaSuportadAsync(id);
        }
    }
}
