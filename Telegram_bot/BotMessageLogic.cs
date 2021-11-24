
using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;

namespace Telegram_bot
{
    class BotMessageLogic
    {
        private Messenger messanger;
        ITelegramBotClient botClient;
        private Dictionary<long, Conversation> chatList;

        public async Task Response(MessageEventArgs e)
        {
            var id = e.Message.Chat.Id;
            if (!chatList.ContainsKey(id))
            {
                var newchat = new Conversation(e.Message.Chat);
                chatList.Add(id,newchat);
            }

            var chat = chatList[id];
            chat.AddMessage(e.Message);

            await SendTextMessage(chat);
        }

        private async Task SendTextMessage(Conversation chat)
        {
            var text = messanger.CreateTextMessage(chat);

            await botClient.SendTextMessageAsync(chatId: chat.GetId(), text:text);
        }

        public BotMessageLogic(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            chatList = new Dictionary<long, Conversation>();
            messanger = new Messenger();

        }
    }
}