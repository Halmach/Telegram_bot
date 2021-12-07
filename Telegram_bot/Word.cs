namespace Telegram_bot
{
    public class Word
    {
        private string russian;
        private string english;
        private string theme;

        public string Russian { get => this.russian; set => this.russian = value; }

        public string English { get => this.english; set => this.english = value; }

        public string Theme { get => this.theme; set => this.theme = value; }
    }
}