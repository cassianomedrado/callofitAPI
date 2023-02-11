using callofitAPI.Interfaces;
using Npgsql;

namespace callofitAPI.Util
{
    public class DaoBase
    {
        private readonly INotificador _notificador;
        private readonly IConfiguration _configuration;

        public DaoBase(INotificador notificador, IConfiguration configuration)
        {
            _notificador = notificador;
            _configuration = configuration; 
        }

        public NpgsqlConnection getConnection()
        {
            var connectionString = _configuration.GetConnectionString("CallOfITConnection");

            NpgsqlConnection connection = new NpgsqlConnection(connectionString);
            return connection;
        }

        protected void Notificar(string mensagem)
        {
            this._notificador.Adicionar(new Notificacao(mensagem));
        }

        protected List<Notificacao> Notificacoes()
        {
            return this._notificador.ObterNotificacoes();
        }

        protected void LimparNotificacoes()
        {
            this._notificador.LimparNotificacoes();
        }
    }
       
}
