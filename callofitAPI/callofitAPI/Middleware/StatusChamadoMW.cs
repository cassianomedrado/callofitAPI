using callofitAPI.Models;
using callofitAPI.Security.DAO;

namespace netbullAPI.Security.MidwareDB
{
    public class StatusChamadoMW
    {
        private StatusChamadoDAO _statusChamadoDao;

        public StatusChamadoMW(StatusChamadoDAO StatusChamadoDao)
        {
            _statusChamadoDao = StatusChamadoDao;
        }

        public async Task<List<StatusChamadoModel>> getAllStatusChamadoAsync()
        {
            return await _statusChamadoDao.getAllStatusChamadoAsync();
        }

        public async Task<StatusChamadoModel> getStatusChamadoPorIdAsync(int id)
        {
            return await _statusChamadoDao.getStatusChamadoPorIdAsync(id);
        }

        public async Task<bool> putStatusChamadoAsync(StatusChamadoModel StatusChamado)
        {
            return await _statusChamadoDao.putStatusChamadoAsync(StatusChamado);
        }

        public async Task<StatusChamadoModel> criarStatusChamadoAsync(StatusChamadoModel StatusChamado)
        {
            return await _statusChamadoDao.criarStatusChamadoAsync(StatusChamado);
        }

        public async Task<bool> DeleteStatusChamadoAsync(int id)
        {
            return await _statusChamadoDao.DeleteStatusChamadoAsync(id);
        }
    }
}
