using System.Collections.Generic;
using System.Linq;
using Telegram.Bot.Types;

namespace Telegram_bot
{
    public class Conversation
    {
        private Dictionary<string, Word> wordDictionary;

        private Chat telegramChat;

        private List<Message> telegramMessages;

        private bool isAddInProgress = false;

        private bool isTrainingInProgress = false;

        public Conversation(Chat chat)
        {
            this.telegramChat = chat;
            this.telegramMessages = new List<Message>();
            this.WordDictionary = new Dictionary<string, Word>();
        }

        public bool IsAddInProgress { get => this.isAddInProgress; set => this.isAddInProgress = value; }

        public bool IsTrainingInProgress { get => this.isTrainingInProgress; set => this.isTrainingInProgress = value; }

        public Dictionary<string, Word> WordDictionary { get => this.wordDictionary; set => this.wordDictionary = value; }

        public void AddMessage(Message message)
        {
            this.telegramMessages.Add(message);
        }

        public long GetId() => this.telegramChat.Id;

        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();
            foreach (var message in this.telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }

            return textMessages;
        }

        public string GetLastMessage() => this.telegramMessages[this.telegramMessages.Count - 1].Text.Trim().ToLower();     
    }
}