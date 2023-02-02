using callofitAPI.Interfaces;
using callofitAPI.Util;
using Npgsql;

namespace callofitAPI.Util
{
    public class NEBase
    {
        private readonly INotificador _notificador;

        public NEBase(INotificador notificador)
        {
            _notificador = notificador;
        }

        protected void Notificar(string mensagem)
        {
            this._notificador.Adicionar(new Notificacao(mensagem));
        }

        protected List<Notificacao> Notificacoes()
        {
            return this._notificador.ObterNotificacoes();
        }
    }
       
}
