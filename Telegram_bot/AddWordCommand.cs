using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Telegram_bot
{
    /// <summary>
    /// AddWordCommand Class
    /// </summary>
    public class AddWordCommand : AbstractCommand
    {
        private ITelegramBotClient botClient;
        private Dictionary<long, Word> wordBufferOfChat;

        public AddWordCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            this.commandText = "/addword";
            this.wordBufferOfChat = new Dictionary<long, Word>();
        }

        public async void ExecuteCommandAsync(Conversation chat)
        {
            this.wordBufferOfChat.Add(chat.GetId(), new Word());
            var text = "Введите русское значение слова";
            await this.SendCommandText(text: text, chat: chat.GetId());
        }

        public async void NextStep(Conversation chat, string message, AddState addState)
        {
            var text = string.Empty;
            var word = this.wordBufferOfChat[chat.GetId()];
            message = message.ToLower().Trim();
            switch (addState)
            {
                case AddState.Russian:
                    word.Russian = message;        
                    text = "Введите английское значение слова"; 
                    break;
                case AddState.English:
                    word.English = message;
                    text = "Введите тематику";
                    break;
                case AddState.Theme:
                    word.Theme = message;
                    text = $"Успешно! Слово {word.Russian} добавлено в словарь";
                    chat.IsAddInProgress = false;
                    chat.WordDictionary.Add(word.Russian, word);
                    this.wordBufferOfChat.Remove(chat.GetId());
                    break;
            }

            await this.SendCommandText(text: text, chat: chat.GetId());
        }

        private async Task SendCommandText(string text, long chat)
        {
            await this.botClient.SendTextMessageAsync(chat, text: text);
        }
    }
}