using System.Collections.Generic;
using Telegram.Bot.Types;

namespace Telegram_bot
{
    class Conversation
    {
        private Chat telegramChat;

        private List<Message> telegramMessages;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
        }

        public void AddMessage(Message eMessage)
        {
            telegramMessages.Add(eMessage);
        }

        public ChatId GetId()
        {
            return null;
        }
    }
}