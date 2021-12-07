namespace Telegram_bot
{
    public abstract class AbstractCommand : IChatCommand
    {
        protected string commandText;

        public bool CheckMessage(string message)
        {
            return this.commandText == message;
        }
    }
}