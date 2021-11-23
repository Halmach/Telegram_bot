using System;
using Telegram.Bot;

namespace Telegram_bot
{
    class Program
    {

        private static string token { get; set; } = "2134031347:AAHUa4ERojB-8-4g-t_eQ5UqHdwJHdQqXq4";
        private static TelegramBotClient botClient;
        static void Main(string[] args)
        {
            botClient = new TelegramBotClient(token);
            botClient.StartReceiving();
            botClient.OnMessage += BotClient_OnMessage;
            Console.ReadLine();
            botClient.StopReceiving();
            

        }

        private static async void BotClient_OnMessage(object sender, Telegram.Bot.Args.MessageEventArgs e)
        {
            var me = await botClient.GetMeAsync();
            Console.WriteLine(me.FirstName);
            Console.WriteLine(e.Message.Text);
        }
    }
}
