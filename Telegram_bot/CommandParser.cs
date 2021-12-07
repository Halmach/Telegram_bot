using System;
using System.Collections.Generic;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_bot
{
    public class CommandParser
    {
        private AddController addcontroller;
        private List<IChatCommand> command;
        private ITelegramBotClient botClient;

        public CommandParser(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            this.command = new List<IChatCommand>();
            this.command.Add(new SayHiCommand());
            this.command.Add(new PoemButton(this.botClient));
            this.command.Add(new AddWordCommand(this.botClient));
            this.command.Add(new DeleteCommand(this.botClient));
            this.command.Add(new ShowDictionaryCommand(this.botClient));
            this.command.Add(new TrainingCommand(this.botClient));
            this.command.Add(new StopTrainingCommand(this.botClient));
            this.Addcontroller = new AddController();
        }

        public AddController Addcontroller { get => this.addcontroller; set => this.addcontroller = value; }

        public bool IsAddCommand(string message)
        {
            var command = this.command.Find(x => x.CheckMessage(message));
            return command is AddWordCommand;
        }

        public bool IsCommand(string message)
        {
            message = this.CommandParse(message);
            return this.command.Exists(x => x.CheckMessage(message));
        }

        public bool IsButtonCommand(string lastmessage)
        {
            var command = this.command.Find(x => x.CheckMessage(lastmessage));
            return command is IButtonCommand;
        }

        public bool IsTextCommand(string message)
        {
            var command = this.command.Find(x => x.CheckMessage(message));

            return command is IChatTextCommand;
        }

        public bool IsTextCommandWithAction(string message)
        {
            var onlyCommand = this.CommandParse(message);
            var command = this.command.Find(x => x.CheckMessage(onlyCommand));

            return command is IChatTextCommandWithAction;
        }

        public string GetTextCommandAnswer(string lastmessage)
        {
            var command = this.command.Find(x => x.CheckMessage(lastmessage)) as IChatTextCommand;
            return command.ReturnText();
        }

        public string GetTextButtonCommand(string lastmessage)
        {
            var command = this.command.Find(x => x.CheckMessage(lastmessage)) as IButtonCommand;
            return command.GetInformation();
        }

        public InlineKeyboardMarkup GetKeyBoard(string lastmessage)
        {
            var command = this.command.Find(x => x.CheckMessage(lastmessage)) as IButtonCommand;
            return command.ReturnKeyBoard();
        }

        public void AddCallback(string message, Conversation chat)
        {
            var command = this.command.Find(x => x.CheckMessage(message)) as IButtonCommand;
            command.AddCallBack(chat);
        }

        public void AddWord(string message, Conversation chat)
        {
            var command = this.command.Find(x => x.CheckMessage(message)) as AddWordCommand;
            this.Addcontroller.SetFirstState(chat);
            command.ExecuteCommandAsync(chat);
        }

        public void NextStage(Conversation chat, string message)
        {
            var command = this.command.Find(x => x is AddWordCommand) as AddWordCommand;
            command.NextStep(chat, message, this.Addcontroller.GetState(chat));
            this.Addcontroller.NextState(chat);
        }

        public void DoForTextCommandWithAction(Conversation chat, string message)
        {
            var onlyCommand = this.CommandParse(message);
            var command = this.command.Find(x => x.CheckMessage(onlyCommand)) as IChatTextCommandWithAction;
            command.TextOperation(chat);
        }

        public void NextWord(Conversation chat, string message)
        {
            var command = this.command.Find(x => x is TrainingCommand) as TrainingCommand;
            command.NextWord(chat, message);
        }

        private string CommandParse(string message)
        {
            var message_temp = message.Trim();
            char charTemp;
            var onlyCommand = string.Empty;
            for (int i = 0; i < message_temp.Length; i++)
            {
                charTemp = message_temp[i];
                if (charTemp.ToString() == " ")
                {
                    break;
                }
                else
                {
                    onlyCommand += charTemp.ToString();
                }
            }

            return onlyCommand;
        }
    }
}