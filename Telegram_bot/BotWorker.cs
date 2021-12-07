using Telegram.Bot;
using Telegram.Bot.Args;

namespace Telegram_bot
{
    public class BotWorker
    {
        private ITelegramBotClient botClient;
        private BotMessageLogic logic;

        public void Initialize(string token)
        {
            this.botClient = new TelegramBotClient(token);
            this.logic = new BotMessageLogic(this.botClient);
        }

        public void Start()
        {
            this.botClient.OnMessage += this.BotClient_OnMessage;
            this.botClient.StartReceiving();
        }

        public void Stop()
        {
            this.botClient.StopReceiving();
        }

        private async void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                await this.logic.Response(e);
            }
        }
    }
}