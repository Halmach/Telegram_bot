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
    public class TrainingCommand : AbstractCommand, IButtonCommand
    {
        private Dictionary<long, TrainingType> trainerType;
        private Dictionary<long, Conversation> trainerChats;
        private Dictionary<long, string> currentWord;
        private ITelegramBotClient botClient;

        public TrainingCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            this.commandText = "/training";
            this.trainerType = new Dictionary<long, TrainingType>();
            this.trainerChats = new Dictionary<long, Conversation>();
            this.currentWord = new Dictionary<long, string>();
        }

        public void AddCallBack(Conversation chat)
        {
            var id = chat.GetId();
            if (!this.trainerChats.ContainsKey(id))
            {
                this.trainerChats.Add(id, chat);
            }
            else
            {
                this.trainerChats[id] = chat;
            }

            this.botClient.OnCallbackQuery -= this.Bot_Callback;
            this.botClient.OnCallbackQuery += this.Bot_Callback;      
        }

        public string GetInformation()
        {
            return "Выберите тип тренировки:";
        }

        public string GetWord(Conversation chat, TrainingType type, out string text)
        {
            text = string.Empty;
            Random rnd = new Random();
            var index = 0;
            if (chat.WordDictionary.Count != 0)
            {
                index = rnd.Next(0, chat.WordDictionary.Count);
                var element = chat.WordDictionary.Values.ElementAt(index);
                text = (type == TrainingType.EngToRus) ? element.Russian : element.English;
                return (type == TrainingType.EngToRus) ? element.English : element.Russian;
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

        public async void NextWord(Conversation chat, string message)
        {
            var text = string.Empty;
            var id = chat.GetId();
            string translate = string.Empty;
            message = message.ToLower().Trim(); /// currentWord[id]
            if (message == this.currentWord[id])
            {
                text += "Ответ верный\n";
            }
            else
            {
                text += $"Ответ неверный! Правильный ответ:{currentWord[id]}\n";
            }

            text += this.GetWord(chat, this.trainerType[id], out translate);
            this.currentWord[id] = translate;
            await this.SendCommandText(text: text, chat: chat.GetId());
        }

        private async Task SendCommandText(string text, long chat)
        {
            await this.botClient.SendTextMessageAsync(chat, text: text);
        }

        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            var text = string.Empty;
            var id = e.CallbackQuery.Message.Chat.Id;
            Conversation chat;
            if (this.trainerChats.ContainsKey(id))
            {
                string translate = string.Empty;
                chat = this.trainerChats[id];
                if (chat.WordDictionary.Count > 0)
                {
                    chat.IsTrainingInProgress = true;
                }

                switch (e.CallbackQuery.Data)
                {
                    case "rusToEng":
                        text = this.GetWord(chat, TrainingType.RusToEng, out translate);
                        if (chat.WordDictionary.Count > 0)
                        {
                            if (!this.trainerType.ContainsKey(id))
                            {
                                this.trainerType.Add(id, TrainingType.RusToEng);
                            }
                            else
                            {
                                this.trainerType[id] = TrainingType.RusToEng;
                            }
                        }

                        break;
                    case "engToRus":
                        text = this.GetWord(chat, TrainingType.EngToRus, out translate);
                        if (chat.WordDictionary.Count > 0)
                        {
                            if (!this.trainerType.ContainsKey(id))
                            {
                                this.trainerType.Add(id, TrainingType.EngToRus);
                            }
                            else
                            {
                                this.trainerType[id] = TrainingType.EngToRus;
                            }
                        }

                        break;
                    default:
                        break;
                }

                if (this.trainerChats.ContainsKey(id))
                {
                    this.trainerChats.Remove(id);
                }

                if (chat.WordDictionary.Count > 0)
                {
                    if (!this.currentWord.ContainsKey(id))
                    {
                        this.currentWord.Add(id, translate);
                    }
                    else
                    {
                        this.currentWord[id] = translate;
                    }
                }

                await this.botClient.SendTextMessageAsync(id, text);
                await this.botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
            }
            else
            {
                if (this.trainerChats.ContainsKey(id))
                {
                    this.trainerChats.Remove(id);
                }

                text = "Словарь пуст";
                await this.botClient.SendTextMessageAsync(id, text);
                await this.botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
            }
        }
    }
}
