using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_bot
{
    public class Messenger
    {
        private CommandParser parser;
        private ITelegramBotClient botClient;

        public Messenger(ITelegramBotClient telegramBotClient)
        {
            this.botClient = telegramBotClient;
            this.parser = new CommandParser(this.botClient);
        }

        public async Task MakeAnswer(Conversation chat)
        {
            var lastmessage = chat.GetLastMessage();
            if (chat.IsTrainingInProgress && !this.parser.IsTextCommandWithAction(lastmessage))
            {
                this.parser.NextWord(chat, lastmessage);
                return;
            }

            if (chat.IsAddInProgress)
            {
                this.parser.NextStage(chat, lastmessage);
                return;
            }

            if (this.parser.IsCommand(lastmessage))
            {
                await this.ExecCommand(chat, lastmessage);
            }
            else
            {
                // var text = CreateMessageError();
                var text = this.CreateTextMessage(chat);
                await this.SendText(chat, text);
            }
        }

        public string CreateTextMessage(Conversation chat)
        {
            var text = string.Empty;
            var textList = chat.GetTextMessages();
            var delimiter = ",";
            text = "Your history:" + string.Join(delimiter, textList.ToArray());

            return text;
        }

        private async Task SendText(Conversation chat, string text)
        {
            await this.botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }

        private async Task SendTextAndKeyBoard(Conversation chat, string text, InlineKeyboardMarkup keyboard)
        {
            await this.botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text, replyMarkup: keyboard);
        }

        private string CreateMessageError()
        {
            return "Is not command";
        }

        private async Task ExecCommand(Conversation chat, string lastmessage)
        {
            if (this.parser.IsTextCommand(lastmessage))
            {
                var text = this.parser.GetTextCommandAnswer(lastmessage);
                await this.SendText(chat, text);
            }

            if (this.parser.IsButtonCommand(lastmessage))
            {
                var text = this.parser.GetTextButtonCommand(lastmessage); 
                var key = this.parser.GetKeyBoard(lastmessage);
                this.parser.AddCallback(lastmessage, chat);
                await this.SendTextAndKeyBoard(chat, text, key);
            }

            if (this.parser.IsAddCommand(lastmessage))
            {
                chat.IsAddInProgress = true;
                this.parser.AddWord(lastmessage, chat);
            }

            if (this.parser.IsTextCommandWithAction(lastmessage))
            {
                this.parser.DoForTextCommandWithAction(chat, lastmessage);
            }
        }
    }
}