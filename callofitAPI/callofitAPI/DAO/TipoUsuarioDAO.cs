using Dapper;
using callofitAPI.Util;
using callofitAPI.Interfaces;
using callofitAPI.Security.Models;
using Npgsql;
using NpgsqlTypes;
using System.Data;
using callofitAPI.Models;
using netbullAPI.Security.MidwareDB;
using callofitAPI.Security.ViewModels;

namespace callofitAPI.Security.DAO
{
    public class TipoUsuarioDAO : DaoBase
    {
        protected IConfiguration _configuration;
        public TipoUsuarioDAO(INotificador notificador, IConfiguration configuration) : base(notificador, configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<TipoUsuarioModel>> getAllTiposUsuarioAsync()
        {
            IEnumerable<TipoUsuarioModel> tiposUsuarios = null;
            try
            {
                string sql = $@" SELECT * FROM tb_tipo_usuario ";
                var connection = getConnection();

                using (connection)
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        tiposUsuarios = await connection.QueryAsync<TipoUsuarioModel>(sql, transaction);
                        transaction.Commit();
                    }
                }

                if (tiposUsuarios == null)
                {
                    Notificar("Não existem tipos de usuários cadastrados.");
                }

                return tiposUsuarios.ToList();
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível encontrar tipos de usuários.");
                return tiposUsuarios.ToList();
            }
        }

        public async Task<TipoUsuarioModel> getTipoUsuarioPorIdAsync(int id)
        {
            TipoUsuarioModel tipoUsuario = null;
            try
            {
                string sqlTipoUsuario = $@" SELECT * FROM tb_tipo_usuario WHERE id = @id";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlTipoUsuario;

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        tipoUsuario = await connection.QueryFirstAsync<TipoUsuarioModel>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (tipoUsuario == null)
                {
                    Notificar("Tipo de usuário não encontrado.");
                }

                return tipoUsuario;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar tipo de usuário.");
                return tipoUsuario;
            }
        }

        public async Task<TipoUsuarioModel> getTipoUsuarioPorDescAsync(string desc)
        {
            TipoUsuarioModel tipoUsuario = null;
            try
            {
                string sqlTipoUsuario = $@" SELECT * FROM tb_tipo_usuario WHERE descricao = @desc";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlTipoUsuario;

                    var parameters = new DynamicParameters();
                    parameters.Add("@desc", desc);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        tipoUsuario = await connection.QueryFirstAsync<TipoUsuarioModel>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (tipoUsuario == null)
                {
                    Notificar("Tipo de usuário não encontrado.");
                }

                return tipoUsuario;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar tipo de usuário.");
                return tipoUsuario;
            }
        }

        public async Task<bool> putTipoUsuarioAsync(TipoUsuarioModel tipoUsuario)
        {
            var retorno = false;
            try
            {
                var tipoUsuarioExistente = await getTipoUsuarioPorIdAsync(tipoUsuario.id);

                if (tipoUsuarioExistente != null)
                {           
                    string sqlUser = $@" UPDATE tb_tipo_usuario SET descricao = @descricao WHERE id = @id";

                    var connection = getConnection();

                    using (connection)
                    {
                        NpgsqlCommand sql = connection.CreateCommand();
                        sql.CommandType = CommandType.Text;
                        sql.CommandText = sqlUser;

                        var parameters = new DynamicParameters();
                        parameters.Add("@descricao", tipoUsuario.descricao);
                        parameters.Add("@id", tipoUsuario.id);

                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            connection.Execute(sql.CommandText, parameters, transaction);
                            transaction.Commit();
                        }
                    }
                    retorno = true;
     
                }
                else
                {
                    Notificar("Tipo usuário informado não foi encontrado.");
                    retorno = false;
                }
            }
            catch (Exception ex)
            {
                Notificar("Nao foi possível alterar dados do tipo usuário.");
            }
            return retorno;
        }

        public async Task<TipoUsuarioModel> criarTipoUsuarioAsync(TipoUsuarioModel tipoUsuario)
        {
            TipoUsuarioModel tipoUsuarioExistente = null;
            try
            {
                tipoUsuarioExistente = await getTipoUsuarioPorDescAsync(tipoUsuario.descricao);

                if (tipoUsuarioExistente == null)
                {
                    string sqlTipoUsuario = $@" INSERT INTO tb_tipo_usuario (descricao) VALUES(@descricao)";

                    var connection = getConnection();

                    using (connection)
                    {
                        NpgsqlCommand sql = connection.CreateCommand();
                        sql.CommandType = CommandType.Text;
                        sql.CommandText = sqlTipoUsuario;

                        var parameters = new DynamicParameters();
                        parameters.Add("@descricao", tipoUsuario.descricao);

                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            connection.Execute(sql.CommandText, parameters, transaction);

                            transaction.Commit();
                        }
                    }

                    tipoUsuarioExistente = await getTipoUsuarioPorDescAsync(tipoUsuario.descricao);

                    return tipoUsuarioExistente;
                }
                else
                {
                    Notificar("Usuário já cadastrado.");
                    tipoUsuarioExistente.id = 0;
                    return tipoUsuarioExistente;
                }
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
                return tipoUsuarioExistente;
            }
        }

        public async Task<bool> DeleteTipoUsuarioAsync(int id)
        {
            var retorno = false;
            try
            {
                var tipoUsuarioExistente = await getTipoUsuarioPorIdAsync(id);

                if (tipoUsuarioExistente != null)
                {
                    string sqlUser = $@" DELETE FROM tb_tipo_usuario WHERE id = @id";

                    var connection = getConnection();

                    using (connection)
                    {
                        NpgsqlCommand sql = connection.CreateCommand();
                        sql.CommandType = CommandType.Text;
                        sql.CommandText = sqlUser;

                        var parameters = new DynamicParameters();
                        parameters.Add("@id", id);

                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            connection.Execute(sql.CommandText, parameters, transaction);
                            transaction.Commit();
                        }
                    }

                    retorno = true;
                }
                else
                {
                    Notificar("Tipo usuário informado não foi encontrado.");
                    retorno = false;
                }
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível deletar tipo usuário.");
            }
            return retorno;
        }
    }
}
