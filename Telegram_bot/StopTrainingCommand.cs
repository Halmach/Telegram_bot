using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace Telegram_bot
{
    public class StopTrainingCommand : AbstractCommand, IChatTextCommandWithAction
    {
        ITelegramBotClient botClient;

        public StopTrainingCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            CommandText = "/stop";
        }

        public void textOperation(Conversation chat)
        {
            long key = chat.GetId();
            chat.IsTrainingInProgress = false;
            botClient.SendTextMessageAsync(key, "Тренировка окончена");
        }
    }
}
