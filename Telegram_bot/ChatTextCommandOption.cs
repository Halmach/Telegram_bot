using System;
using System.Collections.Generic;
using System.Text;

namespace Telegram_bot
{
    abstract class ChatTextCommandOption:AbstractCommand
    {
        protected string KeepOnlyMessage(string message)
        {
            var message_temp = message.Trim();
            char charTemp;
            var onlyMessage = String.Empty;
            for (int i = 0; i < message_temp.Length; i++)
            {
                charTemp = message_temp[i];
                if (charTemp.ToString() == " ") { message_temp = message_temp.Remove(0, i); break; }
            }
            onlyMessage = message_temp.Trim();
            return onlyMessage;
        }
    }
}
