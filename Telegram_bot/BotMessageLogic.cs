using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.ReplyMarkups;

namespace Telegram_bot
{
    public class BotMessageLogic
    {
        private Messenger messanger;
        private ITelegramBotClient botClient;
        private Dictionary<long, Conversation> chatList;

        public BotMessageLogic(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            this.chatList = new Dictionary<long, Conversation>();
            this.messanger = new Messenger(botClient);
        }

        public async Task Response(MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;
            if (!this.chatList.ContainsKey(id))
            {
                var newchat = new Conversation(e.Message.Chat);
                this.chatList.Add(id, newchat);
            }

            var chat = this.chatList[id];
            chat.AddMessage(e.Message);

            await this.messanger.MakeAnswer(chat);
        }
    }
}