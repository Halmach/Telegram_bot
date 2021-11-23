using System;
using System.Collections.Generic;
using Telegram.Bot.Args;
using Telegram.Bot.Types;

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

    class Conversation
    {
        private Chat telegramChat;

        private List<Message> telegramMessages;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
        }
    }
}
