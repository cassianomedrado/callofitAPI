using callofitAPI.Interfaces;
using callofitAPI.Util;

namespace netbullAPI.Middleware
{
    public class NotificadorMW : INotificador
    {
        public List<Notificacao> notificacoes;

        public NotificadorMW()
        {
            this.notificacoes = new List<Notificacao>();
        }

        public void Adicionar(Notificacao notificacao)
        {
            this.notificacoes.Add(notificacao);
        }

        public List<Notificacao> ObterNotificacoes()
        {
            return this.notificacoes;
        }

        public void LimparNotificacoes()
        {
            this.notificacoes = new List<Notificacao>();
        }
    }
}