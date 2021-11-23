using Telegram.Bot;

namespace Telegram_bot
{
    class BotWorker
    {
        ITelegramBotClient botClient;
        private BotMessageLogic logic;
        public void Initialize(string token)
        {
            botClient = new TelegramBotClient(token);
            logic = new BotMessageLogic();
        }

        public void Start()
        {
            botClient.OnMessage += BotClient_OnMessage;
            botClient.StartReceiving();
        }

        private  async void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Вы написали:\n" + e.Message.Text);
            }
        }

        public void Stop()
        {
            botClient.StopReceiving();
        }

    }
}