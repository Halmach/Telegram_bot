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

        public long GetId() => telegramChat.Id;

        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();
            foreach (var message in telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }

            return textMessages;
        }

        public string GetLastMessage() => telegramMessages[telegramMessages.Count - 1].Text;
    }
}