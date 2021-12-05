using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;

namespace Telegram_bot
{
    class ShowDictionaryCommand : AbstractCommand, IChatTextCommandWithAction
    {
        ITelegramBotClient botClient;
        public ShowDictionaryCommand(ITelegramBotClient botClient)
        {
            CommandText = "/dictionary";
            this.botClient = botClient;
        }



        public void textOperation(Conversation chat)
        {
            long  key = chat.GetId();
            var text = String.Empty;
            foreach (var word in chat.wordDictionary)
            {
                text += word.Value.russian + "\n"; 
            }
            text = text.Trim();
            if(text != String.Empty) botClient.SendTextMessageAsync(key,"Список слов для тренировки:\n" + text);
            else botClient.SendTextMessageAsync(key, "Словарь пуст");
        }
    }
}
