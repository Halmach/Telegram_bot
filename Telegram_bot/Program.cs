using System;
using Telegram.Bot.Args;

namespace Telegram_bot
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var bot = new BotWorker();
            bot.Initialize(BotCredentials.BotToken);
            bot.Start();
            Console.WriteLine("Напишите stop для прекращения работы");

            string command;
            do
            {
                command = Console.ReadLine();
            } 
            while (command != "stop");

            bot.Stop();
        }
    }
}
