using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_bot
{
    public class PoemButton : AbstractCommand, IButtonCommand
    {
        private ITelegramBotClient botClient;

        public PoemButton(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            this.commandText = "/poembuttons";
        }

        public InlineKeyboardMarkup ReturnKeyBoard()
        {
            var buttonList = new List<InlineKeyboardButton>
            {
                new InlineKeyboardButton
                {
                    Text = "Пушкин",
                    CallbackData = "pushkin"
                },

                new InlineKeyboardButton
                {
                    Text = "Есенин",
                    CallbackData = "esenin"
                }
            };

            var keyboard = new InlineKeyboardMarkup(buttonList);
            return keyboard;
        }

        public string GetInformation()
        {
            return "Выберите поэта";
        }

        public void AddCallBack(Conversation chat)
        {
            this.botClient.OnCallbackQuery -= this.Bot_Callback;
            this.botClient.OnCallbackQuery += this.Bot_Callback;
        }

        private async void Bot_Callback(object sender, CallbackQueryEventArgs e)
        {
            var text = string.Empty;

            switch (e.CallbackQuery.Data)
            {
                case "pushkin":
                    text = @"Я помню чудное мгновенье:
                                    Передо мной явилась ты,
                                    Как мимолетное виденье,
                                    Как гений чистой красоты.";
                    break;
                case "esenin":
                    text = @"Не каждый умеет петь,
                                Не каждому дано яблоком
                                Падать к чужим ногам.";
                    break;
                default:
                    break;
            }

            await this.botClient.SendTextMessageAsync(e.CallbackQuery.Message.Chat.Id, text);
            await this.botClient.AnswerCallbackQueryAsync(e.CallbackQuery.Id);
        }
    }
}
