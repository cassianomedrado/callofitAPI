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

        //public async Task<bool> putStatusChamadoAsync(StatusChamadoModel StatusChamado)
        //{
        //    return await _statusChamadoDao.putStatusChamadoAsync(StatusChamado);
        //}

        //public async Task<StatusChamadoModel> criarStatusChamadoAsync(StatusChamadoModel StatusChamado)
        //{
        //    return await _statusChamadoDao.criarStatusChamadoAsync(StatusChamado);
        //}

        //public async Task<bool> DeleteStatusChamadoAsync(int id)
        //{
        //    return await _statusChamadoDao.DeleteStatusChamadoAsync(id);
        //}
    }
}
