using Dapper;
using callofitAPI.Util;
using callofitAPI.Interfaces;
using Npgsql;
using System.Data;
using callofitAPI.Models;

namespace callofitAPI.Security.DAO
{
    public class SistemaSuportadoDAO : DaoBase
    {
        protected IConfiguration _configuration;
        public SistemaSuportadoDAO(INotificador notificador, IConfiguration configuration) : base(notificador, configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<SistemaSuportadoModel>> getAllSistemaSuportadoAsync()
        {
            IEnumerable<SistemaSuportadoModel> sistemaSuportado = null;
            try
            {
                string sql = $@" SELECT * FROM tb_sistema_suportado ";
                var connection = getConnection();

                using (connection)
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        sistemaSuportado = await connection.QueryAsync<SistemaSuportadoModel>(sql, transaction);
                        transaction.Commit();
                    }
                }

                if (sistemaSuportado == null || sistemaSuportado.Count() == 0)
                {
                    Notificar("Não existem sistemas suportados cadastrados.");
                }

                return sistemaSuportado.ToList();
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível encontrar sistemas suportados.");
                return sistemaSuportado.ToList();
            }
        }

        public async Task<SistemaSuportadoModel> getSistemaSuportaPorIdAsync(int id)
        {
            SistemaSuportadoModel StatusChamado = null;
            try
            {
                string sqlStatusChamado = $@" SELECT * FROM tb_sistema_suportado WHERE id = @id";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlStatusChamado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        StatusChamado = await connection.QueryFirstOrDefaultAsync<SistemaSuportadoModel>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (StatusChamado == null)
                {
                    Notificar("Sistema suportado não encontrado.");
                }

                return StatusChamado;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar sistema suportado.");
                return StatusChamado;
            }
        }

        //public async Task<StatusChamadoModel> getStatusChamadoPorDescAsync(string desc)
        //{
        //    StatusChamadoModel StatusChamado = null;
        //    try
        //    {
        //        string sqlStatusChamado = $@" SELECT * FROM tb_status_chamado WHERE descricao = @desc";

        //        var connection = getConnection();

        //        using (connection)
        //        {
        //            NpgsqlCommand sql = connection.CreateCommand();
        //            sql.CommandType = CommandType.Text;
        //            sql.CommandText = sqlStatusChamado;

        //            var parameters = new DynamicParameters();
        //            parameters.Add("@desc", desc);

        //            connection.Open();

        //            using (var transaction = connection.BeginTransaction())
        //            {
        //                StatusChamado = await connection.QueryFirstOrDefaultAsync<StatusChamadoModel>(sql.CommandText, parameters, transaction);
        //                transaction.Commit();
        //            }
        //        }

        //        if (StatusChamado == null)
        //        {
        //            Notificar("Status de chamado não encontrado.");
        //        }

        //        return StatusChamado;
        //    }
        //    catch (Exception ex)
        //    {
        //        Notificar("Não foi possível recuperar status de chamado.");
        //        return StatusChamado;
        //    }
        //}

        //public async Task<bool> putStatusChamadoAsync(StatusChamadoModel StatusChamado)
        //{
        //    var retorno = false;
        //    try
        //    {
        //        var StatusChamadoExistente = await getStatusChamadoPorIdAsync(StatusChamado.id);
        //        if(Notificacoes().Count > 0)
        //            return retorno;

        //        string sqlUser = $@" UPDATE tb_status_chamado SET descricao = @descricao WHERE id = @id";

        //        var connection = getConnection();

        //        using (connection)
        //        {
        //            NpgsqlCommand sql = connection.CreateCommand();
        //            sql.CommandType = CommandType.Text;
        //            sql.CommandText = sqlUser;

        //            var parameters = new DynamicParameters();
        //            parameters.Add("@descricao", StatusChamado.descricao);
        //            parameters.Add("@id", StatusChamado.id);

        //            connection.Open();

        //            using (var transaction = connection.BeginTransaction())
        //            {
        //                connection.Execute(sql.CommandText, parameters, transaction);
        //                transaction.Commit();
        //            }
        //        }
        //        retorno = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        Notificar("Nao foi possível alterar dados do status de chamado.");
        //    }
        //    return retorno;
        //}

        //public async Task<StatusChamadoModel> criarStatusChamadoAsync(StatusChamadoModel StatusChamado)
        //{
        //    StatusChamadoModel StatusChamadoExistente = null;
        //    try
        //    {
        //        StatusChamadoExistente = await getStatusChamadoPorDescAsync(StatusChamado.descricao);

        //        if (StatusChamadoExistente == null)
        //        {
        //            string sqlStatusChamado = $@" INSERT INTO tb_status_chamado (descricao) VALUES(@descricao)";

        //            var connection = getConnection();

        //            using (connection)
        //            {
        //                NpgsqlCommand sql = connection.CreateCommand();
        //                sql.CommandType = CommandType.Text;
        //                sql.CommandText = sqlStatusChamado;

        //                var parameters = new DynamicParameters();
        //                parameters.Add("@descricao", StatusChamado.descricao);

        //                connection.Open();

        //                using (var transaction = connection.BeginTransaction())
        //                {
        //                    connection.Execute(sql.CommandText, parameters, transaction);

        //                    transaction.Commit();
        //                }
        //            }

        //            StatusChamadoExistente = await getStatusChamadoPorDescAsync(StatusChamado.descricao);

        //            return StatusChamadoExistente;
        //        }
        //        else
        //        {
        //            Notificar("Status de chamado já cadastrado.");
        //            StatusChamadoExistente.id = 0;
        //            return StatusChamadoExistente;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Notificar(ex.Message);
        //        return StatusChamadoExistente;
        //    }
        //}

        //public async Task<bool> DeleteStatusChamadoAsync(int id)
        //{
        //    var retorno = false;
        //    try
        //    {
        //        var StatusChamadoExistente = await getStatusChamadoPorIdAsync(id);
        //        if (Notificacoes().Count > 0)
        //            return retorno;

        //        if (StatusChamadoExistente != null)
        //        {
        //            string sqlUser = $@" DELETE FROM tb_status_chamado WHERE id = @id";

        //            var connection = getConnection();

        //            using (connection)
        //            {
        //                NpgsqlCommand sql = connection.CreateCommand();
        //                sql.CommandType = CommandType.Text;
        //                sql.CommandText = sqlUser;

        //                var parameters = new DynamicParameters();
        //                parameters.Add("@id", id);

        //                connection.Open();

        //                using (var transaction = connection.BeginTransaction())
        //                {
        //                    connection.Execute(sql.CommandText, parameters, transaction);
        //                    transaction.Commit();
        //                }
        //            }

        //            retorno = true;
        //        }              
        //    }
        //    catch (Exception ex)
        //    {
        //        Notificar("Não foi possível deletar status de chamado.");
        //    }
        //    return retorno;
        //}
    }
}
