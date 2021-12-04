using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Telegram_bot
{
    public class AddWordCommand : AbstractCommand
    {
        ITelegramBotClient botClient;
        Dictionary<long, Word> wordBufferOfChat;
        public AddWordCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            CommandText = "/addword";
            wordBufferOfChat = new Dictionary<long, Word>();
        }

        public async void ExecuteCommandAsync(Conversation chat)
        {
            wordBufferOfChat.Add(chat.GetId(), new Word());
            var text = "Введите русское значение слова";
            await SendCommandText(text: text, chat: chat.GetId());
        }


        private async Task SendCommandText(string text, long chat)
        {
            await botClient.SendTextMessageAsync(chat, text: text);
        }

        public async void NextStep(Conversation chat, string message, AddState addState)
        {
            var text = String.Empty;
            var word = wordBufferOfChat[chat.GetId()];
            message = message.ToLower().Trim();
            switch (addState)
            {
                case AddState.Russian:
                    word.russian = message;        
                    text = "Введите английское значение слова"; 
                    break;
                case AddState.English:
                    word.english = message;
                    text = "Введите тематику";
                    break;
                case AddState.Theme:
                    word.theme = message;
                    text = $"Успешно! Слово {word.russian} добавлено в словарь";
                    chat.IsAddInProgress = false;
                    chat.wordDictionary.Add(word.russian, word);
                    wordBufferOfChat.Remove(chat.GetId());
                    break;
            }
            await SendCommandText(text: text, chat: chat.GetId());

        }
    }
}