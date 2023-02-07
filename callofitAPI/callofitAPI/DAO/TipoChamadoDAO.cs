using Dapper;
using callofitAPI.Util;
using callofitAPI.Interfaces;
using Npgsql;
using System.Data;
using callofitAPI.Models;

namespace callofitAPI.Security.DAO
{
    public class TipoChamadoDAO : DaoBase
    {
        protected IConfiguration _configuration;
        public TipoChamadoDAO(INotificador notificador, IConfiguration configuration) : base(notificador, configuration)
        {
            _configuration = configuration;
        }

        public async Task<List<TipoChamadoModel>> getAllTiposChamadosAsync()
        {
            IEnumerable<TipoChamadoModel> tiposChamados = null;
            try
            {
                string sql = $@" SELECT * FROM tb_tipo_chamados ";
                var connection = getConnection();

                using (connection)
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        tiposChamados = await connection.QueryAsync<TipoChamadoModel>(sql, transaction);
                        transaction.Commit();
                    }
                }

                if (tiposChamados == null || tiposChamados.Count() == 0)
                {
                    Notificar("Não existem tipos de chamados cadastrados.");
                }

                return tiposChamados.ToList();
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível encontrar tipos de chamados.");
                return tiposChamados.ToList();
            }
        }

        public async Task<TipoChamadoModel> getTipoChamadoPorIdAsync(int id)
        {
            TipoChamadoModel tipoChamado = null;
            try
            {
                string sqlTipoChamado = $@" SELECT * FROM tb_tipo_chamados WHERE id = @id";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlTipoChamado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", id);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        tipoChamado = await connection.QueryFirstOrDefaultAsync<TipoChamadoModel>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (tipoChamado == null)
                {
                    Notificar("Tipo de chamado não encontrado.");
                }

                return tipoChamado;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar tipo de chamado.");
                return tipoChamado;
            }
        }

        public async Task<TipoChamadoModel> getTipoChamadoPorDescAsync(string desc)
        {
            TipoChamadoModel tipoChamado = null;
            try
            {
                string sqlTipoChamado = $@" SELECT * FROM tb_tipo_chamados WHERE descricao = @desc";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlTipoChamado;

                    var parameters = new DynamicParameters();
                    parameters.Add("@desc", desc);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        tipoChamado = await connection.QueryFirstOrDefaultAsync<TipoChamadoModel>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (tipoChamado == null)
                {
                    Notificar("Tipo de chamado não encontrado.");
                }

                return tipoChamado;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar tipo de chamado.");
                return tipoChamado;
            }
        }

        public async Task<bool> putTipoChamadoAsync(TipoChamadoModel tipoChamado)
        {
            var retorno = false;
            try
            {
                var tipoChamadoExistente = await getTipoChamadoPorIdAsync(tipoChamado.id);
                if(Notificacoes().Count > 0)
                    return retorno;
        
                string sqlUser = $@" UPDATE tb_tipo_chamados SET descricao = @descricao WHERE id = @id";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlUser;

                    var parameters = new DynamicParameters();
                    parameters.Add("@descricao", tipoChamado.descricao);
                    parameters.Add("@id", tipoChamado.id);

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
                Notificar("Nao foi possível alterar dados do tipo chamado.");
            }
            return retorno;
        }

        public async Task<TipoChamadoModel> criarTipoChamadoAsync(TipoChamadoModel tipoChamado)
        {
            TipoChamadoModel tipoChamadoExistente = null;
            try
            {
                tipoChamadoExistente = await getTipoChamadoPorDescAsync(tipoChamado.descricao);

                if (tipoChamadoExistente == null)
                {
                    string sqlTipoChamado = $@" INSERT INTO tb_tipo_chamados (descricao) VALUES(@descricao)";

                    var connection = getConnection();

                    using (connection)
                    {
                        NpgsqlCommand sql = connection.CreateCommand();
                        sql.CommandType = CommandType.Text;
                        sql.CommandText = sqlTipoChamado;

                        var parameters = new DynamicParameters();
                        parameters.Add("@descricao", tipoChamado.descricao);

                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            connection.Execute(sql.CommandText, parameters, transaction);

                            transaction.Commit();
                        }
                    }

                    tipoChamadoExistente = await getTipoChamadoPorDescAsync(tipoChamado.descricao);

                    return tipoChamadoExistente;
                }
                else
                {
                    Notificar("Tipo chamado já cadastrado.");
                    tipoChamadoExistente.id = 0;
                    return tipoChamadoExistente;
                }
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
                return tipoChamadoExistente;
            }
        }

        public async Task<bool> DeleteTipoChamadoAsync(int id)
        {
            var retorno = false;
            try
            {
                var tipoChamadoExistente = await getTipoChamadoPorIdAsync(id);
                if (Notificacoes().Count > 0)
                    return retorno;

                if (tipoChamadoExistente != null)
                {
                    string sqlUser = $@" DELETE FROM tb_tipo_chamados WHERE id = @id";

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
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível deletar tipo chamado.");
            }
            return retorno;
        }
    }
}
