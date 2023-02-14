using Dapper;
using callofitAPI.Util;
using callofitAPI.Interfaces;
using callofitAPI.Models;
using Npgsql;
using System.Data;

namespace callofitAPI.Security.DAO
{
    public class HistoricoChamadoDAO : DaoBase
    {
        protected IConfiguration _configuration;
        public HistoricoChamadoDAO(INotificador notificador, IConfiguration configuration) : base(notificador, configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<HistoricoChamadoModel>> getAllHistoricoChamadoAsync()
        {
            IEnumerable<HistoricoChamadoModel> listaHistoricoChamado = null;
            try
            {
                string sqlHistoricoChamado = $@" SELECT * FROM tb_historico_chamado ";
                var connection = getConnection();

                using (connection)
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        listaHistoricoChamado = await connection.QueryAsync<HistoricoChamadoModel>(sqlHistoricoChamado, transaction);
                        transaction.Commit();
                    }
                }

                if (listaHistoricoChamado == null || listaHistoricoChamado.Count() == 0)
                {
                    Notificar("Não existem historicos de chamados.");
                }

                return listaHistoricoChamado.ToList();
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível encontrar historicos de chamados.");
                return listaHistoricoChamado.ToList();
            }
        }

        public async Task<HistoricoChamadoModel> getHistoricoChamadoPorIdAsync(int id)
        {
            HistoricoChamadoModel HistoricoChamado = null;
            try
            {
                string sqlHistoricoChamado = $@" SELECT * FROM tb_historico_chamado WHERE id = @id";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlHistoricoChamado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        HistoricoChamado = await connection.QueryFirstOrDefaultAsync<HistoricoChamadoModel>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (HistoricoChamado == null)
                {
                    Notificar("Historico de chamado não encontrado.");
                }

                return HistoricoChamado;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar historico de chamado.");
                return HistoricoChamado;
            }
        }

        public async Task<bool> criarHistoricoChamadoAsync(HistoricoChamadoModel sistemaSuportado)
        {
            try
            {
                string sqlHistoricoChamado = $@" INSERT INTO tb_historico_chamado (data_criacao, acao, chamados_id, usuario_id) 
                                                                           VALUES (@data_criacao, @acao, @chamados_id, @usuario_id)";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlHistoricoChamado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@data_criacao", sistemaSuportado.data_criacao);
                    parameters.Add("@acao", sistemaSuportado.acao);
                    parameters.Add("@chamados_id", sistemaSuportado.chamados_id);
                    parameters.Add("@usuario_id", sistemaSuportado.usuario_id);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        connection.Execute(sql.CommandText, parameters, transaction);

                        transaction.Commit();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível criar historico de chamado.");
                Notificar(ex.Message);
                return false ;
            }
        }
    }
}
