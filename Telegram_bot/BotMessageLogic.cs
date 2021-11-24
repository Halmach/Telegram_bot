
using System.Collections.Generic;
using Telegram.Bot;

namespace Telegram_bot
{
    class BotMessageLogic
    {
        private Messenger messanger;
        ITelegramBotClient botClient;
        private Dictionary<long, Conversation> chatList;

        public void Response()
        {
            
        }

        public BotMessageLogic(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            chatList = new Dictionary<long, Conversation>();
            messanger = new Messenger();

        }
    }
}