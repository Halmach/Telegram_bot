using System;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Telegram_bot
{
    class Program
    {

        static void Main(string[] args)
        {
            var bot = new BotWorker();
            bot.Initialize(BotCredentials.BotToken);
            bot.Start();
            Console.WriteLine("Напишите stop для прекращения работы");


            string command;
            do
            {
                command = Console.ReadLine();
            } while (command != "stop");

            bot.Stop();
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
