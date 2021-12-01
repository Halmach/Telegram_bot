using System.Collections.Generic;

namespace Telegram_bot
{
    public class CommandParser
    {
        private List<IChatCommand> Command;

        public CommandParser()
        {
            Command = new List<IChatCommand>();
        }
    }
}