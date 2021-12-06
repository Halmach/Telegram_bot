using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_bot
{
    class Messenger
    {
        private CommandParser parser;
        private ITelegramBotClient botClient;

        public Messenger(ITelegramBotClient telegramBotClient)
        {
            botClient = telegramBotClient;
            parser = new CommandParser(botClient);
        }
        public async Task MakeAnswer(Conversation chat)
        {
            var lastmessage = chat.GetLastMessage();
            if(chat.IsTrainingInProgress)
            {
                parser.NextWord(chat,lastmessage);
                return;
            }
            if(chat.IsAddInProgress)
            {
                parser.NextStage(chat, lastmessage);
                return;
            }
            if (parser.IsCommand(lastmessage))
            {
                await ExecCommand(chat, lastmessage);
            }
            else
            {
                // var text = CreateMessageError();
                var text = CreateTextMessage(chat);
                await SendText(chat, text);
            }
        }

        private async Task SendText(Conversation chat, string text)
        {
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text);
        }

        private async Task SendTextAndKeyBoard(Conversation chat, string text, InlineKeyboardMarkup keyboard)
        {
            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text: text, replyMarkup: keyboard);
        }

        private string CreateMessageError()
        {
            return "Is not command";
        }

        private async Task ExecCommand(Conversation chat, string lastmessage)
        {
            if (parser.IsTextCommand(lastmessage))
            {
                var text = parser.GetTextCommandAnswer(lastmessage);
                await SendText(chat, text);
            }

            if (parser.IsButtonCommand(lastmessage))
            {
                var text = parser.GetTextButtonCommand(lastmessage); 
                var key = parser.GetKeyBoard(lastmessage);
                parser.AddCallback(lastmessage, chat);
                await SendTextAndKeyBoard(chat,text,key);
            }

            if (parser.IsAddCommand(lastmessage))
            {
                chat.IsAddInProgress = true;
                parser.AddWord(lastmessage,chat);
            }

            if(parser.IsTextCommandWithAction(lastmessage))
            {
                parser.DoForTextCommandWithAction(chat, lastmessage);
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


    }
}