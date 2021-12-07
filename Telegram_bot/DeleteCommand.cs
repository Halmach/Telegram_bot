using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace Telegram_bot
{
    public class DeleteCommand : ChatTextCommandOption, IChatTextCommandWithAction
    {
        private ITelegramBotClient botClient;

        public DeleteCommand(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            this.commandText = "/deleteword";
        }

        public void TextOperation(Conversation chat)
        {
            var message = chat.GetLastMessage();
            var key = KeepOnlyMessage(message);
            bool removeFlag = chat.WordDictionary.Remove(key);
            if (removeFlag)
            {
                this.botClient.SendTextMessageAsync(chat.GetId(), "Слово успешно удалено");
            }
            else
            {
                this.botClient.SendTextMessageAsync(chat.GetId(), "Слово для удаления не найдено");
            }
        }
    }
}
