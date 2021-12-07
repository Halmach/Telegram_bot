namespace Telegram_bot
{
    public class SayHiCommand : AbstractCommand,
        IChatTextCommand
    {
        public SayHiCommand()
        {
            this.commandText = "/saymehi";
        }

        public string ReturnText()
        {
            return "привет";
        }
    }
}