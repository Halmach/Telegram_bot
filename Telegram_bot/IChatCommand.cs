namespace Telegram_bot
{
    public interface IChatCommand
    {
        bool CheckMessage(string message);
    }
}