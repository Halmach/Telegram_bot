using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace Telegram_bot
{
    public class ShowDictionaryCommand : AbstractCommand, IChatTextCommandWithAction
    {
        private ITelegramBotClient botClient;

        public ShowDictionaryCommand(ITelegramBotClient botClient)
        {
            this.commandText = "/dictionary";
            this.botClient = botClient;
        }

        public void TextOperation(Conversation chat)
        {
            long  key = chat.GetId();
            var text = string.Empty;
            foreach (var word in chat.WordDictionary)
            {
                text += word.Value.Russian + "\n"; 
            }

            text = text.Trim();
            if (text != string.Empty)
            {
                this.botClient.SendTextMessageAsync(key, "Список слов для тренировки:\n" + text);
            }
            else
            {
                this.botClient.SendTextMessageAsync(key, "Словарь пуст");
            }
        }
    }
}
