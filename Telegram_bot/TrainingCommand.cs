using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_bot
{
    class TrainingCommand : AbstractCommand, IButtonCommand
    {
        Dictionary<long, TrainingType> trainerType;
        Dictionary<long, Conversation> trainerChats;
        Dictionary<long, string> currentWord;
        ITelegramBotClient botClient;

        public TrainingCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            CommandText = "/training";
            trainerType = new Dictionary<long, TrainingType>();
            trainerChats = new Dictionary<long, Conversation>();
            currentWord = new Dictionary<long, string>();
        }

        public void AddCallBack(Conversation chat)
        {
            trainerChats.Add(chat.GetId(), chat);
            botClient.OnCallbackQuery -= Bot_Callback;
            botClient.OnCallbackQuery += Bot_Callback;      
        }

        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            var text = "";
            var id = e.CallbackQuery.Message.Chat.Id;
            Conversation chat;
            if (trainerChats.ContainsKey(id))
            {
                string translate = string.Empty;
                chat = trainerChats[id];
                if (chat.wordDictionary.Count > 0) chat.IsTrainingInProgress = true;
                switch (e.CallbackQuery.Data) 
                {
                    case "rusToEng":
                        text = GetWord(chat, TrainingType.RusToEng, out translate);
                        if (chat.wordDictionary.Count > 0) trainerType.Add(id, TrainingType.RusToEng);
                        break;
                    case "engToRus":
                        text = GetWord(chat, TrainingType.EngToRus, out translate);
                        if (chat.wordDictionary.Count > 0) trainerType.Add(id, TrainingType.EngToRus);
                        break;
                    default:
                        break;
                }

                if (trainerChats.ContainsKey(id)) trainerChats.Remove(id);
                if (chat.wordDictionary.Count > 0) currentWord.Add(id, translate);

                await botClient.SendTextMessageAsync(id, text);
                await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
            }
            else
            {
                if (trainerChats.ContainsKey(id)) trainerChats.Remove(id);
                text = "Словарь пуст";
                await botClient.SendTextMessageAsync(id, text);
                await botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
            }
        }

        public string GetInformation()
        {
            return "Выберите тип тренировки:";
        }

        public string GetWord(Conversation chat, TrainingType type, out string text)
        {
            text = String.Empty;
            Random rnd = new Random();
            var Index = 0;
            if (chat.wordDictionary.Count != 0)
            {
                Index = rnd.Next(0, chat.wordDictionary.Count);
                var element = chat.wordDictionary.Values.ElementAt(Index);
                text = (type == TrainingType.EngToRus) ? element.russian : element.english;
                return (type == TrainingType.EngToRus) ? element.english : element.russian;
            }
            return "Словарь пуст";
        }

        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    Text = "С русского на английский",
                    CallbackData = "rusToEng"
                },

                new InlineKeyboardButton
                {
                    Text = "С английского на русский",
                    CallbackData = "engToRus"
                }
            };
            var keyboard = new InlineKeyboardMarkup(buttonList);
            return keyboard;
        }

        private async Task SendCommandText(string text, long chat)
        {
            await botClient.SendTextMessageAsync(chat, text: text);
        }

        public async void NextWord(Conversation chat, string message)
        {
            var text = String.Empty;
            var id = chat.GetId();
            string translate = string.Empty;
            message = message.ToLower().Trim(); /// currentWord[id]
            if (message == currentWord[id]) text += "Ответ верный\n";
            else text += $"Ответ неверный! Правильный ответ:{currentWord[id]}\n";
            text += GetWord(chat, trainerType[id], out translate);
            currentWord[id] = translate;
            await SendCommandText(text: text, chat: chat.GetId());
        }
    }
}
