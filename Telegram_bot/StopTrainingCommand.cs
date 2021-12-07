using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace Telegram_bot
{
    public class StopTrainingCommand : AbstractCommand, IChatTextCommandWithAction
    {
        private ITelegramBotClient botClient;

        public StopTrainingCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            this.commandText = "/stop";
        }

        public void TextOperation(Conversation chat)
        {
            long key = chat.GetId();
            chat.IsTrainingInProgress = false;
            this.botClient.SendTextMessageAsync(key, "Тренировка окончена");
        }
    }
}
