using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Telegram_bot
{
    class Program
    {
        static ITelegramBotClient botClient;

        static void Main(string[] args)
        {
            // Этот объект обрабатывает все обращения к боту. 
            botClient = new TelegramBotClient(BotCredentials.BotToken);
            var me = botClient.GetMeAsync().Result;
            Console.WriteLine("Привет! Меня зовут {0}.", me.FirstName);
            botClient.OnMessage += BotClient_OnMessage;
            botClient.StartReceiving();
            Console.ReadKey();
            botClient.StopReceiving();
        }

        private static async void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            if (e.Message.Text !=null)
            {
                Console.WriteLine($"Получено сообщение в чате:{e.Message.Chat.Id}");
                Console.WriteLine(e.Message.Text);
                await botClient.SendTextMessageAsync(chatId: e.Message.Chat, text: "Вы написали:\n" + e.Message.Text);
            }
        }
    }

    class BotWorker
    {
        ITelegramBotClient botClient;
        public void Initialize(string token)
        {
            botClient = new TelegramBotClient(token);
        }

        public void Start()
        {
            botClient.StartReceiving();
        }

        public void Stop()
        {
            botClient.StopReceiving();
        }

    }
}
