using Dapper;
using callofitAPI.Util;
using callofitAPI.Interfaces;
using Npgsql;
using System.Data;
using callofitAPI.Models;
using netbullAPI.Security.MidwareDB;

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
                string sqlSistemaSuportado = $@" SELECT * FROM tb_sistema_suportado ";
                var connection = getConnection();

                using (connection)
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        sistemaSuportado = await connection.QueryAsync<SistemaSuportadoModel>(sqlSistemaSuportado, transaction);
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

        public async Task<SistemaSuportadoModel> criarSistemaSuportadoAsync(SistemaSuportadoModel sistemaSuportado)
        {
            SistemaSuportadoModel SistemaSuportadoExistente = null;
            try
            {
                SistemaSuportadoExistente = await getSistemaSuportadoPorNomeAsync(sistemaSuportado.nome);
                
                if (SistemaSuportadoExistente == null)
                {
                    LimparNotificacoes();
                    string sqlSistemaSuportado = $@" INSERT INTO tb_sistema_suportado (nome, data_criacao) VALUES(@nome, @data_criacao)";

                    var connection = getConnection();

                    using (connection)
                    {
                        NpgsqlCommand sql = connection.CreateCommand();
                        sql.CommandType = CommandType.Text;
                        sql.CommandText = sqlSistemaSuportado;

                        var parameters = new DynamicParameters();
                        parameters.Add("@nome", sistemaSuportado.nome);
                        parameters.Add("@data_criacao", sistemaSuportado.data_criacao);

                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            connection.Execute(sql.CommandText, parameters, transaction);

                            transaction.Commit();
                        }
                    }

                    SistemaSuportadoExistente = await getSistemaSuportadoPorNomeAsync(sistemaSuportado.nome);

                    return SistemaSuportadoExistente;
                }
                else
                {
                    Notificar("Sistema suportado já cadastrado.");
                    SistemaSuportadoExistente.id = 0;
                    return SistemaSuportadoExistente;
                }
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
                return SistemaSuportadoExistente;
            }
        }

        public async Task<SistemaSuportadoModel> getSistemaSuportadoPorNomeAsync(string nome)
        {
            SistemaSuportadoModel sistemaSuportado = null;
            try
            {
                string sqlSistemaSuportado = $@" SELECT * FROM tb_sistema_suportado WHERE nome = @nome";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlSistemaSuportado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@nome", nome);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        sistemaSuportado = await connection.QueryFirstOrDefaultAsync<SistemaSuportadoModel>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (sistemaSuportado == null)
                {
                    Notificar("Sistema suportado não encontrado.");
                }

                return sistemaSuportado;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar sistema suportado.");
                return sistemaSuportado;
            }
        }

        public async Task<bool> putSistemaSuportadoAsync(SistemaSuportadoModel sistemaSuportado)
        {
            var retorno = false;
            try
            {
                var SistemaSuportadoExistente = await getSistemaSuportaPorIdAsync(sistemaSuportado.id);
                if (Notificacoes().Count > 0)
                    return retorno;

                string sqlSistemaSuportado = $@" UPDATE tb_sistema_suportado SET nome = @nome, data_criacao = @data_criacao WHERE id = @id";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlSistemaSuportado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", sistemaSuportado.id);
                    parameters.Add("@nome", sistemaSuportado.nome);
                    parameters.Add("@data_criacao", sistemaSuportado.data_criacao);

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
                Notificar("Nao foi possível alterar dados do sistema suportado.");
            }
            return retorno;
        }

        public async Task<bool> DeleteSistemaSuportadAsync(int id)
        {
            var retorno = false;
            try
            {
                var SistemaSuportadoExistente = await getSistemaSuportaPorIdAsync(id);
                if (Notificacoes().Count > 0)
                    return retorno;

                if (SistemaSuportadoExistente != null)
                {
                    string sqlSistemaSuportado = $@" DELETE FROM tb_sistema_suportado WHERE id = @id";

                    var connection = getConnection();

                    using (connection)
                    {
                        NpgsqlCommand sql = connection.CreateCommand();
                        sql.CommandType = CommandType.Text;
                        sql.CommandText = sqlSistemaSuportado;

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
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível deletar sistema suportado.");
            }
            return retorno;
        }
    }
}
