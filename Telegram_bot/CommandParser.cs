using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_bot
{
    public class CommandParser
    {
        private List<IChatCommand> Command;
        private ITelegramBotClient botClient;

        public CommandParser(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            Command = new List<IChatCommand>();
            Command.Add(new SayHiCommand());
            Command.Add(new PoemButton(botClient));
        }

        public bool IsCommand(string message)
        {
            return Command.Exists(x => x.CheckMessage(message));

        }

        public bool IsTextCommand(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message));

            return command is IChatTextCommand;
        }

        public string GetTextCommandAnswer(string lastmessage)
        {
            var command = Command.Find(x => x.CheckMessage(lastmessage)) as IChatTextCommand;
            return command.ReturnText();
        }

        public bool IsButtonCommand(string lastmessage)
        {
            var command = Command.Find(x => x.CheckMessage(lastmessage));
            return command is IButtonCommand;
        }

        public string GetTextButtonCommand(string lastmessage)
        {
            var command = Command.Find(x => x.CheckMessage(lastmessage)) as IButtonCommand;
            return command.GetInformation();
        }

        public InlineKeyboardMarkup GetKeyBoard(string lastmessage)
        {
            var command = Command.Find(x => x.CheckMessage(lastmessage)) as IButtonCommand;
            return command.ReturnKeyBoard();
        }

        public void AddCallback(string message, Conversation chat)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as IButtonCommand;
            command.AddCallBack(chat);
        }
    }
}