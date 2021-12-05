using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace Telegram_bot
{
    class DeleteCommand : ChatTextCommandOption, IChatTextCommandWithAction
    {
        ITelegramBotClient botClient;
        public void textOperation(Conversation chat)
        {
            var message = chat.GetLastMessage();
            var key = KeepOnlyMessage(message);
            bool removeFlag = chat.wordDictionary.Remove(key);
            if (removeFlag) botClient.SendTextMessageAsync(chat.GetId(),"Слово успешно удалено");
            else botClient.SendTextMessageAsync(chat.GetId(), "Слово для удаления не найдено");
        }

        public DeleteCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            CommandText = "/deleteword";
        }
    }
}
