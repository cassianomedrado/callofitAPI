using callofitAPI.Models;
using callofitAPI.Security.DAO;
using callofitAPI.ViewModels.Chamados;

namespace netbullAPI.Security.MidwareDB
{
    public class ChamadoMW
    {
        private ChamadoDAO _ChamadoDao;
        private HistoricoChamadoDAO _historicoChamadoDao;

        public ChamadoMW (ChamadoDAO ChamadoDao, HistoricoChamadoDAO historicoChamadoDao)
        {
            _ChamadoDao = ChamadoDao;
            _historicoChamadoDao = historicoChamadoDao;
        }

        public async Task<List<ChamadoModel>> getAllChamadosAsync()
        {
            return await _ChamadoDao.getAllChamadosAsync();
        }

        public async Task<ChamadoModel> getChamadoPorIdAsync(int id)
        {
            return await _ChamadoDao.getChamadoPorIdAsync(id);
        }

        public async Task<ChamadoModel> CriarChamadoAsync(ChamadoModel chamado)
        {
            var criado = await _ChamadoDao.CriarChamadoAsync(chamado);
            if (criado == null)
            {
                return null;
            }
            else
            {
                await _historicoChamadoDao.criarHistoricoChamadoAsync(new HistoricoChamadoModel()
                {
                    data_criacao = DateTime.Now,
                    acao = $"Chamado criado, Status ID: {criado.status_chamado_id}",
                    usuario_id = criado.usuario_id,
                    chamados_id = criado.id
                });
                
            }

            return criado;
        }

        public async Task<bool> putChamadoAsync(ChamadoModel chamado)
        {
            return await _ChamadoDao.putChamadoAsync(chamado);
        }

        public async Task<bool> DeletarChamadoAsync(int id)
        {
            return await _ChamadoDao.DeletarChamadoAsync(id);
        }

        public async Task<BuscaTotaisChamados> getAllTotaisChamadosPorUsuarioAsync(RequestTotaisChamados request)
        {
            return await _ChamadoDao.getAllTotaisChamadosPorUsuarioAsync(request);
        }

        public async Task<List<ChamadoModel>> getAllChamadosPorUsuarioAsync(RequestBuscarChamados request)
        {
            return await _ChamadoDao.getAllChamadosPorUsuarioAsync(request);
        }
    }
}
