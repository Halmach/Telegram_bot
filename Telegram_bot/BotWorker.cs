using Telegram.Bot;
using Telegram.Bot.Args;

namespace Telegram_bot
{
    class BotWorker
    {
        ITelegramBotClient botClient;
        private BotMessageLogic logic;
        public void Initialize(string token)
        {
            botClient = new TelegramBotClient(token);
            logic = new BotMessageLogic(botClient);
        }

        public void Start()
        {
            botClient.OnMessage += BotClient_OnMessage;
            botClient.OnCallbackQuery += Bot_Callback;
            botClient.StartReceiving();

        }

        private  async void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                await logic.Response(e);
            }
        }

        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            var text = "";

            switch (e.CallbackQuery.Data)
            {
                case "pushkin":
                    text = @"Я помню чудное мгновенье:
                                    Передо мной явилась ты,
                                    Как мимолетное виденье,
                                    Как гений чистой красоты.";
                    break;
                case "esenin":
                    text = @"Не каждый умеет петь,
                                Не каждому дано яблоком
                                Падать к чужим ногам.";
                    break;
                default:
                    break;
            }

            await botClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, text);
            await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
        }

        public void Stop()
        {
            botClient.StopReceiving();
        }

    }
}