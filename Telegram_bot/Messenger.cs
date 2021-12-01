using System.Collections.Generic;

namespace Telegram_bot
{
    class Messenger
    {
        public string CreateTextMessage(Conversation chat)
        {
            var text = "";
            switch (chat.GetLastMessage())
            {
                case "/saymechi": text = "Привет";
                    break;
                case "/askme": text = "Как дела?"; break;

                case "/poembuttons": text = "Выберите поэта"; break;

                default:
                {
                    var textList = chat.GetTextMessages();
                    var delimiter = ",";
                    text = "Your history:" + string.Join(delimiter, textList.ToArray());
                }
                    break;

            }
            return text;
        }

        public int isThisButton(Conversation chat)
        {
            string command = chat.GetLastMessage();
            if (command == "/poembuttons") return 1;
            else return 0;
        }


    }
}