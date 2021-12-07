using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_bot
{
    public interface IButtonCommand
    {
        public InlineKeyboardMarkup ReturnKeyBoard();

        public string GetInformation();

        public void AddCallBack(Conversation chat);
    }
}
