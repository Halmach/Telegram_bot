using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot;

namespace Telegram_bot
{
    public interface IChatTextCommandWithAction
    {
        public void TextOperation(Conversation chat);
    }
}
