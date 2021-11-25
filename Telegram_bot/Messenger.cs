using System.Collections.Generic;

namespace Telegram_bot
{
    class Messenger
    {
        public string CreateTextMessage(Conversation chat)
        {
            var textList = chat.GetTextMessages();
            var delimiter = ",";
            var text = "Your history:" + string.Join(delimiter, textList.ToArray());
            return text;
        }


    }
}