using callofitAPI.Models;
using callofitAPI.Security.DAO;

namespace netbullAPI.Security.MidwareDB
{
    public class HistoricoChamadoMW
    {
        private HistoricoChamadoDAO _historicoChamadoDao;

        public HistoricoChamadoMW (HistoricoChamadoDAO historicoChamadoDao)
        {
            _historicoChamadoDao = historicoChamadoDao;
        }

        public async Task<List<HistoricoChamadoModel>> getAllHistoricoChamadoAsync()
        {
            return await _historicoChamadoDao.getAllHistoricoChamadoAsync();
        }

        public async Task<HistoricoChamadoModel> getHistoricoChamadoPorIdAsync(int id)
        {
            return await _historicoChamadoDao.getHistoricoChamadoPorIdAsync(id);
        }

        public async Task<bool> criarHistoricoChamadoAsync(HistoricoChamadoModel historicoChamado)
        {
            return await _historicoChamadoDao.criarHistoricoChamadoAsync(historicoChamado);
        }
    }
}
