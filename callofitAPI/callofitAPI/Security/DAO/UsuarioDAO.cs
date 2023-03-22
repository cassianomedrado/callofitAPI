using Dapper;
using callofitAPI.Security.ViewModels;
using callofitAPI.Util;
using callofitAPI.Interfaces;
using callofitAPI.Security.Models;
using Npgsql;
using System.Data;

namespace callofitAPI.Security.DAO
{
    public class UsuarioDAO : DaoBase
    {
        protected IConfiguration _configuration;
        public UsuarioDAO(INotificador notificador, IConfiguration configuration) : base(notificador, configuration)
        {
            _configuration = configuration;
        }

        public async Task<RetornarUserViewModel> CadastroDeUserAsync(Usuario usu)
        {
            RetornarUserViewModel usuView = new RetornarUserViewModel();
            try
            {

                var usuRecuperado = await RecuperarUsuarioAsync(usu);

                if (usuRecuperado == null)
                {
                    string sqlUser = $@" INSERT INTO tb_usuario (data_criacao, nome, email, tipo_usuario_id, username, senha, status)
                                            VALUES(@data_criacao, @nome, @email, @tipo_usuario_id, @username, @senha, @status)";

                    var connection = getConnection();

                    using (connection)
                    {
                        NpgsqlCommand sql = connection.CreateCommand();
                        sql.CommandType = CommandType.Text; 
                        sql.CommandText = sqlUser;

                        var parameters = new DynamicParameters();
                        parameters.Add("@data_criacao", usu.data_criacao);
                        parameters.Add("@nome", usu.nome);
                        parameters.Add("@email", usu.email);
                        parameters.Add("@tipo_usuario_id", usu.tipo_usuario_id);
                        parameters.Add("@username", usu.username);
                        parameters.Add("@senha", usu.senha);
                        parameters.Add("@status", usu.status);

                        connection.Open();

                        using (var transaction = connection.BeginTransaction())
                        {
                            connection.Execute(sql.CommandText, parameters, transaction);

                            transaction.Commit();
                        }
                    }

                    usu = await RecuperarUsuarioAsync(usu);

                    usuView.id = usu.id;
                    usuView.data_criacao = usu.data_criacao;
                    usuView.nome = usu.nome;
                    usuView.email = usu.email;
                    usuView.tipo_usuario_id = usu.tipo_usuario_id;
                    usuView.username = usu.username;
                    usuView.status = usu.status;

                    return usuView;
                }
                else
                {
                    Notificar("Usuário já cadastrado.");
                    usuView.id = 0;
                    return usuView;
                }
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
                return usuView;
            }
        }

        public async Task<Usuario> VerificarUsuarioSenhaAsync(Usuario usu)
        {
            try
            {
                var usuConsulta = await RecuperarUsuarioAsync(usu);
                if (usuConsulta != null)
                {
                    if (usuConsulta.senha == usu.senha)
                    {
                        return usuConsulta;
                    }
                    else
                    {
                        usu.id = 0;
                        Notificar("Senha incorreta");
                        return usu;
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível verificar senha do usuário.");
                return usu;
            }
        }

        public async Task<bool> alterarSenhaAsync(Usuario usu)
        {
            var retorno = false;
            try
            {
                var usuario = (await getAllUsersAsync()).Where(u => u.username == usu.username).FirstOrDefault();

                if (usuario != null)
                {
                    if (usuario.email.Equals(usu.email))
                    {
                        string sqlUser = $@" UPDATE tb_usuario SET senha = @senha WHERE id = @id";

                        var connection = getConnection();

                        using (connection)
                        {
                            NpgsqlCommand sql = connection.CreateCommand();
                            sql.CommandType = CommandType.Text;
                            sql.CommandText = sqlUser;

                            var parameters = new DynamicParameters();
                            parameters.Add("@senha", usu.senha);
                            parameters.Add("@id", usuario.id);

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
                        Notificar("Endereço de e-mail não pertence a esse usuário.");
                        retorno = false;
                    }
                }
                else
                {
                    Notificar("Usuário informado não foi encontrado.");
                    retorno = false;
                }
            }
            catch (Exception ex)
            {
                Notificar("Nao foi possível alterar a senha do usuário.");
            }
            return retorno;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var retorno = false;
            try
            {
                var listaUsu = await getAllUsersAsync();

                if (listaUsu.Exists(l => l.id == id))
                {
                    string sqlUser = $@" DELETE FROM tb_usuario WHERE id = @id";

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
                    Notificar("Usuário informado não foi encontrado.");
                    retorno = false;
                }
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível deletar usuario.");
            }
            return retorno;
        }

        public async Task<List<RetornarUserViewModel>> getAllUsersAsync()
        {
            IEnumerable<RetornarUserViewModel> users = null;
            try
            {
                string sqlUser = $@" SELECT id, data_criacao, nome, email, tipo_usuario_id, username, status FROM tb_usuario ";
                var connection = getConnection();

                using (connection)
                {
                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        users = await connection.QueryAsync<RetornarUserViewModel>(sqlUser, transaction);
                        transaction.Commit();
                    }
                }

                if (users == null || users.Count() == 0)
                {
                    Notificar("Usuários não encontrados.");
                }

                return users.ToList();
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível encontrar usuarios.");
                return users.ToList();
            }
        }

        public async Task<Usuario> RecuperarUsuarioAsync(Usuario usu)
        {
            try
            {
                string sqlUser = $@" SELECT * FROM tb_usuario WHERE username = @username";
                Usuario user;

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlUser;

                    var parameters = new DynamicParameters();
                    parameters.Add("@username", usu.username);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        user = await  connection.QueryFirstOrDefaultAsync<Usuario>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (user == null)
                {
                    Notificar("Usuário não encontrado.");
                }

                return user;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar usuario.");
                return usu;
            }
        }

        public async Task<bool> InativarUsuarioAsync(int id)
        {
            var retorno = false;
            try
            {
                var usuario = (await getAllUsersAsync()).Where(u => u.id == id).FirstOrDefault();

                if (usuario != null)
                {
                    string sqlUser = $@" UPDATE tb_usuario SET status = @status WHERE id = @id";

                    var connection = getConnection();

                    using (connection)
                    {
                        NpgsqlCommand sql = connection.CreateCommand();
                        sql.CommandType = CommandType.Text;
                        sql.CommandText = sqlUser;

                        var parameters = new DynamicParameters();
                        parameters.Add("@status", false);
                        parameters.Add("@id", usuario.id);

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
                    Notificar("Usuário informado não foi encontrado.");
                    retorno = false;
                }
            }
            catch (Exception ex)
            {
                Notificar("Nao foi possível alterar a senha do usuário.");
            }
            return retorno;
        }

        public async Task<Usuario> RecuperarUsuarioPorIdAsync(Usuario usu)
        {
            try
            {
                string sqlUser = $@" SELECT * FROM tb_usuario WHERE id = @id";
                Usuario user;

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlUser;

                    var parameters = new DynamicParameters();
                    parameters.Add("@id", usu.id);

                    connection.Open();

                    using (var transaction = connection.BeginTransaction())
                    {
                        user = await connection.QueryFirstOrDefaultAsync<Usuario>(sql.CommandText, parameters, transaction);
                        transaction.Commit();
                    }
                }

                if (user == null)
                {
                    Notificar("Usuário não encontrado.");
                }

                return user;
            }
            catch (Exception ex)
            {
                Notificar("Não foi possível recuperar usuario.");
                return usu;
            }
        }

        public async Task<bool> UpdateUsuarioAsync(RetornarUserViewModel usu)
        {
            var retorno = false;
            try
            {
                string sqlUser = $@" UPDATE tb_usuario SET nome = @nome, email = @email, tipo_usuario_id = @tipo_usuario_id WHERE id = @id";

                var connection = getConnection();

                using (connection)
                {
                    NpgsqlCommand sql = connection.CreateCommand();
                    sql.CommandType = CommandType.Text;
                    sql.CommandText = sqlUser;

                    var parameters = new DynamicParameters();
                    parameters.Add("@nome", usu.nome);
                    parameters.Add("@email", usu.email);
                    parameters.Add("@tipo_usuario_id", usu.tipo_usuario_id);
                    parameters.Add("@id", usu.id);

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
                Notificar("Nao foi possível alterar a dados do usuário.");
            }
            return retorno;
        }
    }
}
