using Dapper;
using callofitAPI.Util;
using callofitAPI.Interfaces;
using callofitAPI.Models;
using Npgsql;
using System.Data;
using netbullAPI.Security.MidwareDB;
using System;
using callofitAPI.ViewModels.Chamados;
using callofitAPI.ViewModels.HistoricoChamado;

namespace callofitAPI.Security.DAO
{
    public class ChamadoDAO : DaoBase
    {
        protected IConfiguration _configuration;
        public ChamadoDAO(INotificador notificador, IConfiguration configuration) : base(notificador, configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<ChamadoModel>> getAllChamadosAsync()
        {
            IEnumerable<ChamadoModel> listaChamado = null;
            try
            {
                string sqlChamado = $@" SELECT * FROM tb_chamados ";
                var connection = getConnection();

                using (connection)
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        listaChamado = await connection.QueryAsync<ChamadoModel>(sqlChamado, transaction);
                        transaction.Commit();
                    }
                }

                if (listaChamado == null || listaChamado.Count() == 0)
                {
                    Notificar("Não existem chamados.");
                }

                return listaChamado.ToList();
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível encontrar chamados.");
                return listaChamado.ToList();
            }
        }

        public async Task<ChamadoModel> getChamadoPorIdAsync(int id)
        {
            ChamadoModel chamado = null;
            try
            {
                string sqlChamado = $@" SELECT * FROM tb_chamados WHERE id = @id";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlChamado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        chamado = await connection.QueryFirstOrDefaultAsync<ChamadoModel>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (chamado == null)
                {
                    Notificar("Chamado não encontrado.");
                }

                return chamado;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar chamado.");
                return chamado;
            }
        }

        public async Task<ChamadoModel> getChamadoPorGuidAsync(Guid guid)
        {
            ChamadoModel chamado = null;
            try
            {
                string sqlChamado = $@" SELECT * FROM tb_chamados WHERE identificador_unico = @guid";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlChamado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@guid", guid);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        chamado = await connection.QueryFirstOrDefaultAsync<ChamadoModel>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (chamado == null)
                {
                    Notificar("Chamado não encontrado.");
                }

                return chamado;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar chamado.");
                return chamado;
            }
        }

        public async Task<ChamadoModel> CriarChamadoAsync(ChamadoModel chamado)
        {
            ChamadoModel ChamadoExistente = null;
            try
            {
                ChamadoExistente = await getChamadoPorGuidAsync(chamado.identificador_unico);
                if (ChamadoExistente == null)
                {
                    LimparNotificacoes();

                    string sqlChamado = $@" INSERT INTO tb_chamados (data_criacao, 
                                                                     solicitante, 
                                                                     data_limite, 
                                                                     status_chamado_id, 
                                                                     sistema_suportado_id, 
                                                                     descricao_problema, 
                                                                     usuario_id, 
                                                                     descricao_solucao, 
                                                                     tipo_chamado_id, 
                                                                     identificador_unico) 

                                                             VALUES (@data_criacao, 
                                                                     @solicitante, 
                                                                     @data_limite, 
                                                                     @status_chamado_id, 
                                                                     @sistema_suportado_id, 
                                                                     @descricao_problema, 
                                                                     @usuario_id, 
                                                                     @descricao_solucao, 
                                                                     @tipo_chamado_id, 
                                                                     @identificador_unico)";
                    var connection = getConnection();

                    using (connection)
                    {
                        NpgsqlCommand sql = connection.CreateCommand();
                        sql.CommandType = CommandType.Text;
                        sql.CommandText = sqlChamado;

                        var parameters = new DynamicParameters();
                        parameters.Add("@data_criacao", chamado.data_criacao);
                        parameters.Add("@solicitante", chamado.solicitante);
                        parameters.Add("@data_limite", chamado.data_limite);
                        parameters.Add("@status_chamado_id", chamado.status_chamado_id);
                        parameters.Add("@sistema_suportado_id", chamado.sistema_suportado_id);
                        parameters.Add("@descricao_problema", chamado.descricao_problema);
                        parameters.Add("@usuario_id", chamado.usuario_id);
                        parameters.Add("@descricao_solucao", chamado.descricao_solucao);
                        parameters.Add("@tipo_chamado_id", chamado.tipo_chamado_id);
                        parameters.Add("@identificador_unico", chamado.identificador_unico);

                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            connection.Execute(sql.CommandText, parameters, transaction);

                            transaction.Commit();
                        }
                    }

                     return await getChamadoPorGuidAsync(chamado.identificador_unico);
                }
                else
                {
                    Notificar("Tipo chamado já cadastrado.");
                    ChamadoExistente.id = 0;
                    return ChamadoExistente;
                }
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
                return ChamadoExistente;
            }
        }

        public async Task<bool> putChamadoAsync(ChamadoModel chamado)
        {
            var retorno = false;
            try
            {
                var ChamadoExistente = await getChamadoPorIdAsync(chamado.id);
                if (Notificacoes().Count > 0)
                    return retorno;

                string sqlChamado = $@" UPDATE tb_chamados SET 
                                               solicitante = @solicitante,
                                               data_limite = @data_limite,
                                               status_chamado_id = @status_chamado_id,
                                               sistema_suportado_id = @sistema_suportado_id,
                                               descricao_problema = @descricao_problema,
                                               descricao_solucao = @descricao_solucao,
                                               tipo_chamado_id = @tipo_chamado_id
                                         WHERE id = @id";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlChamado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", chamado.id);
                    parameters.Add("@data_criacao", chamado.data_criacao);
                    parameters.Add("@solicitante", chamado.solicitante);
                    parameters.Add("@data_limite", chamado.data_limite);
                    parameters.Add("@status_chamado_id", chamado.status_chamado_id);
                    parameters.Add("@sistema_suportado_id", chamado.sistema_suportado_id);
                    parameters.Add("@descricao_problema", chamado.descricao_problema);
                    parameters.Add("@descricao_solucao", chamado.descricao_solucao);
                    parameters.Add("@tipo_chamado_id", chamado.tipo_chamado_id);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        connection.Execute(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }
                retorno = true;
            }
            catch (Exception ex)
            {
                Notificar("Nao foi possível alterar chamado.");
                Notificar(ex.Message);
            }
            return retorno;
        }

        public async Task<bool> DeletarChamadoAsync(RequestDeleteChamado request)
        {
            var retorno = false;
            try
            {
                var ChamadoExistente = await getChamadoPorIdAsync(request.chamado_id);
                if (Notificacoes().Count > 0)
                    return retorno;

                if (ChamadoExistente != null)
                {
                    string sqlChamado = $@" DELETE FROM tb_chamados WHERE id = @id";

                    var connection = getConnection();

                    using (connection)
                    {
                        NpgsqlCommand sql = connection.CreateCommand();
                        sql.CommandType = CommandType.Text;
                        sql.CommandText = sqlChamado;

                        var parameters = new DynamicParameters();
                        parameters.Add("@id", request.chamado_id);

                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            connection.Execute(sql.CommandText, parameters, transaction);
                            transaction.Commit();
                        }
                    }

                    retorno = true;
                }
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível deletar chamado.");
                Notificar(ex.Message);
            }
            return retorno;
        }

        public async Task<BuscaTotaisChamados> getAllTotaisChamadosPorUsuarioAsync(RequestTotaisChamados request)
        {
            BuscaTotaisChamados chamado = null;
            try
            {
                string sqlChamado = $@" select distinct (select count(1) from tb_chamados where status_chamado_id = 1 and tb_chamados.usuario_id = @usuario_id ) as chamadosEmAberto,
			                                            (select count(1) from tb_chamados where status_chamado_id = 2 and tb_chamados.usuario_id = @usuario_id ) as chamadosPendentes,
			                                            (select count(1) from tb_chamados where status_chamado_id = 3 and tb_chamados.usuario_id = @usuario_id  ) as chamadosFinalizados,
			                                            (select count(1) from tb_chamados where status_chamado_id = 4 and tb_chamados.usuario_id = @usuario_id ) as chamadosAtrasados
                                        from tb_chamados   ";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlChamado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@usuario_id", request.usuario_id);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        chamado = await connection.QueryFirstAsync<BuscaTotaisChamados>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                return chamado;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar totalizadores de chamados.");
                return chamado;
            }
        }

        public async Task<List<ChamadoModel>> getAllChamadosPorUsuarioAsync(RequestBuscarChamados request)
        {
            IEnumerable<ChamadoModel> listaChamado = null;
            try
            {
                string sqlChamado = $@" SELECT * FROM tb_chamados where usuario_id = @usuario_id";
                var connection = getConnection();

                using (connection)
                {
                    var parameters = new DynamicParameters();
                    parameters.Add("@usuario_id", request.usuario_id);
                    if (request.status_chamado_id != 0)
                    {
                        parameters.Add("@status_chamado_id", request.status_chamado_id);
                        sqlChamado += " and status_chamado_id = @status_chamado_id ";
                    }

                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlChamado;

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        listaChamado = await connection.QueryAsync<ChamadoModel>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (listaChamado == null || listaChamado.Count() == 0)
                {
                    Notificar("Não existem chamados.");
                }

                return listaChamado.ToList();
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível encontrar chamados.");
                return listaChamado.ToList();
            }
        }
    }
}
